using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class People : MonoBehaviour
{
    public float moveSpeed;
    public int emotion;
    public string _name;
    public string[] dialogues;
    public int rate;

    Rigidbody rb;
    Renderer rend;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
    public void ChangeColor(Color32 newColor)
    {
        rend.material.color = newColor;
    }
    private void OnMouseEnter()
    {
        Actions.onMouseOverPpl.Invoke(this);
    }
    private void OnMouseDown()
    {
        Actions.onClickPpl.Invoke(this);
    }
    private void OnMouseExit()
    {
        Actions.onMouseExitPpl.Invoke();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * 3;
        Gizmos.DrawLine(startPosition, endPosition);

        Gizmos.DrawSphere(endPosition, 0.3f);
    }
}
