using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawning Attributes")]
    [Range(0.2f, 2f)] public float maxMoveSpeed;
    [Range(0, 3)] public int maxEmotion;
    public List<string> nameList;
    [SerializeField] GameObject pplPrefab;

    [Header("Spawning Conditions")]
    public GameObject ground;
    Renderer groundRenderer;
    public Vector3 spawnPos;
    public float maxSpawnDelay;

    [Header("Spawned Instance")]
    public List<GameObject> livingPpl;
    public int maxNumOfPpl;

    void Start()
    {
        InitializeSpawner();
    }
    public void InitializeSpawner()
    {
        groundRenderer = ground.GetComponent<Renderer>();
        livingPpl = new List<GameObject>();

        for (int i = 0; i < maxNumOfPpl; i++)
        {
            SpawnPeople();
        }
    }
    
    public void SpawnPeople()
    {
        if (livingPpl.Count >= maxNumOfPpl) return;

        float _moveSpeed = Random.Range(0.5f, maxMoveSpeed);
        int _emotion = Random.Range(0, maxEmotion + 1); //Random.Range for int is exclusive on the upper bound
        //string _name = nameList[Random.Range(0, nameList.Length)];
        int nameIndex = Random.Range(0, nameList.Count);
        string _name = nameList[nameIndex];
        nameList.RemoveAt(nameIndex);

        StartCoroutine(SpawnWithDelay(_moveSpeed, _emotion, _name));
    }
    IEnumerator SpawnWithDelay(float _moveSpeed, int _emotion, string _name)
    {
        // Calculate random delay
        float delay = Random.Range(0f, maxSpawnDelay);
        yield return new WaitForSeconds(delay);

        GameObject ppl = Instantiate(pplPrefab);
        People people = ppl.GetComponent<People>();
        people.moveSpeed = _moveSpeed;
        people.emotion = _emotion;
        people._name = _name;

        ppl.transform.position = GetRandomSpawnPosition();
        ppl.transform.rotation = GetRandomSpawnRotation(ppl.transform.position);

        livingPpl.Add(ppl);
    }
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 groundSize = groundRenderer.bounds.size;
        float pplHeight = 2f;
        float x, z;
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Bottom edge
                x = Random.Range(0, groundSize.x);
                z = 0;
                break;
            case 1: // Top edge
                x = Random.Range(0, groundSize.x);
                z = groundSize.z;
                break;
            case 2: // Left edge
                x = 0;
                z = Random.Range(0, groundSize.z);
                break;
            case 3: // Right edge
                x = groundSize.x;
                z = Random.Range(0, groundSize.z);
                break;
            default:
                x = 0;
                z = 0;
                break;
        }

        return ground.transform.position + new Vector3(x - groundSize.x / 2, pplHeight, z - groundSize.z / 2);
    }
    Quaternion GetRandomSpawnRotation(Vector3 spawnPosition)
    {
        Vector3 groundCenter = groundRenderer.bounds.center;
        Vector3 directionToCenter = groundCenter - spawnPosition;
        float angle = Mathf.Atan2(directionToCenter.x, directionToCenter.z) * Mathf.Rad2Deg;
        angle += Random.Range(-30f, 30f);

        return Quaternion.Euler(0, angle, 0);
    }
}
