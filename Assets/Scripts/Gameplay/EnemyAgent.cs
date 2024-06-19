using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class EnemyAgent : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Follow,
        Attack,
    }
    public EnemyState state;
    public bool activated;

    [Header("Patrol")]
    public NavMeshAgent agent;
    public float patrolZoneRange = 5f;
    public float nextPointDistance = 0.2f;
    public float stopDuration = 2f;
    float waitingTime;
    public bool canPatrol;
    public Vector3 centerPoint;
    public bool isHit;

    [Header("Attack")]
    public float attackRange;
    public int dmg;
    public int attackCooldown;
    bool canAttack, attacking;
    [SerializeField] Transform attackTrans;

    [Header("Health")]
    public int hp;
    public int maxHp;
    public bool isDead;

    [Header("FOV")]
    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    [Header("Visual")]
    [SerializeField] GameObject spriteObj;
    SpriteRenderer spriteRend;
    float idleAnimSpeed;
    float moveAmplitude;
    [SerializeField] float speedMin;
    [SerializeField] float speedMax;
    [SerializeField] float amplitudeMin;
    [SerializeField] float amplitudeMax;
    private Vector3 startPosition;
    private float timeCounter = 0;

    private void OnEnable()
    {
        Actions.OnLevelStart += StartLevel;
    }
    private void OnDisable()
    {
        Actions.OnLevelStart -= StartLevel;
    }

    void StartLevel()
    {
        activated = true;
    }
    void Start()
    {
        UpDownVisual();
        agent = GetComponent<NavMeshAgent>();
        state = EnemyState.Idle;
        centerPoint = transform.position;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        centerPoint = transform.position;
        hp = maxHp;
    }
    void Update()
    {
        canAttack = Physics.CheckBox(transform.position, new Vector3(attackRange/2, attackRange/2, attackRange/2), transform.rotation, targetMask);
        FieldOfViewCheck();
        HandleStateTransitions();
        UpdateCurrentState();
    }
    void UpDownVisual()
    {
        spriteRend = spriteObj.GetComponent<SpriteRenderer>();
        startPosition = spriteObj.transform.position;
        StartCoroutine(IdleAnim(spriteObj.transform));
    }
    IEnumerator IdleAnim(Transform targetTrans)
    {
        while (true)
        {
            idleAnimSpeed = Random.Range(speedMin, speedMax);
            moveAmplitude = Random.Range(amplitudeMin, amplitudeMax);

            float initialTime = timeCounter;

            // Complete one full cycle of sine wave (2 * Mathf.PI radians)
            while (timeCounter < initialTime + 2 * Mathf.PI)
            {
                timeCounter += Time.deltaTime * idleAnimSpeed;
                float newY = startPosition.y + Mathf.Sin(timeCounter) * moveAmplitude;
                targetTrans.position = new Vector3(transform.position.x, newY, transform.position.z);

                yield return null;
            }

            timeCounter -= 2 * Mathf.PI;
        }
    }
    void HandleStateTransitions()
    {
        switch (state)
        {
            case EnemyState.Idle:
                if (activated)
                    state = EnemyState.Patrol;
                break;
            case EnemyState.Patrol:
                if (canSeePlayer || isHit)
                {
                    state = EnemyState.Follow;
                }
                else if (canAttack)
                {
                    state = EnemyState.Attack;
                    attacking = false;
                }
                break;
            case EnemyState.Follow:
                if (!canSeePlayer && !isHit)
                {
                    state = EnemyState.Patrol;
                    canPatrol = false;
                }
                else if (canAttack && canSeePlayer)
                {
                    state = EnemyState.Attack;
                    attacking = false;
                }
                break;
            case EnemyState.Attack:
                if (!canAttack && !canSeePlayer)
                    state = EnemyState.Patrol;
                if (!canAttack && canSeePlayer)
                    state = EnemyState.Follow;
                break;
        }
    }
    void UpdateCurrentState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                activated = false;
                centerPoint = transform.position;
                break;
            case EnemyState.Patrol:
                if (agent != null && centerPoint != null)
                    Patrol();
                break;
            case EnemyState.Follow:
                if (playerRef != null)
                    ChaseTarget(playerRef.transform, 2f);
                break;
            case EnemyState.Attack:
                if (playerRef != null)
                    Attack();
                break;
        }
    }
    void Patrol()
    {
        spriteRend.color = Color.white;
        agent.speed = 1f;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (canPatrol)
            {
                Vector3 point;
                if (RandomPoint(centerPoint, patrolZoneRange, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    canPatrol = false;
                }
            }
            else
            {
                waitingTime += Time.deltaTime;
                if (waitingTime >= stopDuration)
                {
                    canPatrol = true;
                    waitingTime = 0f;
                }
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, nextPointDistance, NavMesh.AllAreas))
        {

            result = hit.position;
            canPatrol = false;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    void ChaseTarget(Transform target, float chaseSpeed)
    {
        spriteRend.color = Color.red;

        // Move towards the target
        agent.SetDestination(target.position);
        agent.speed = chaseSpeed;

        //Vector3 directionToTarget = (target.position - transform.position).normalized;
        //transform.LookAt(target);
        //rb.MovePosition(transform.position + (directionToTarget * moveSpeed * chaseSpeedMultiplier * Time.deltaTime));
    }
    void Attack()
    {
        spriteRend.color = Color.red;
        agent.SetDestination(transform.position);
        transform.LookAt(playerRef.transform);

        if (!attacking)
        {
            Vector3 attackPos = attackTrans.position;
            Collider[] cols = Physics.OverlapSphere(attackPos, 1f);
            foreach (Collider col in cols)
            {
                //if (col.gameObject == gameObject) continue;
                if (col.gameObject == playerRef)
                {
                    Rigidbody playerRb = col.gameObject.GetComponentInParent<Rigidbody>();
                    if (playerRb != null)
                    {
                        Vector3 forceDirection = (col.transform.position - transform.position).normalized;
                        float forceMagnitude = dmg;
                        playerRb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                        Debug.Log("Player pushed with force: " + forceDirection * forceMagnitude);
                    }
                    else
                    {
                        Debug.Log("target no found");
                    }
                }
            }
            attacking = true;
            Invoke("ResetAttack", attackCooldown);
        }
    }
    void ResetAttack()
    {
        attacking = false;
        isHit = false;
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(centerPoint, Vector3.up, Vector3.forward, 360, patrolZoneRange);

        /*    Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);*/
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(attackRange, attackRange, attackRange));

        Handles.color = Color.blue;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, nextPointDistance);

        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Handles.DrawLine(transform.position, transform.position + viewAngle02 * radius);

        if (canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(transform.position, playerRef.transform.position);
        }
    }
}
