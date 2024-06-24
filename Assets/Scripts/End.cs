using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EndGame());
        }
    }
    IEnumerator EndGame()
    {
        FindObjectOfType<SoundManager>().PlayRand("Death");
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(2);
    }
}
