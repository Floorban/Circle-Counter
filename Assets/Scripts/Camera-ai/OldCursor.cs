using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldCursor : MonoBehaviour
{
    [SerializeField] GameObject spotLight;
    [SerializeField] GameObject UIPanel;
    [SerializeField] float lightHeight;
    Vector3 mousePos;
    public float followSpeed;

    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = followSpeed;
        spotLight.transform.position = new Vector3 (Camera.main.ScreenToWorldPoint(mousePos).x, lightHeight, Camera.main.ScreenToWorldPoint(mousePos).z);
        UIPanel.GetComponent<RectTransform>().position = new Vector3(mousePos.x + 200f, mousePos.y, 0);
    }
}
