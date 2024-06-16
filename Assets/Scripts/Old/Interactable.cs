using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject highlightGO;
    public GunController gun;
    private void Start()
    {
        highlightGO.SetActive(false);
    }
    private void OnMouseEnter()
    {
        highlightGO.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlightGO.SetActive(false);
    }
    public virtual void OnMouseDown()
    {
        highlightGO.SetActive(false);
    }
}
