using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //Animator animator;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string[] dialogues;
    [SerializeField] float typeSpeed;
    int index;
    void Start()
    {
        StartCoroutine(StartTutorial());
    }
    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(0f);
        //animator = GetComponent<Animator>();
        text.text = string.Empty;
        StartDialogue();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (text.text == dialogues[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = dialogues[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeTextLine());
    }
    IEnumerator TypeTextLine()
    {
        FindObjectOfType<SoundManager>().PlaySound("Typing", 1f);

        foreach (char c in dialogues[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
    void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeTextLine());
        }
        else
        {
            //if (animator) animator.SetTrigger("Exit");
        }
    }
}
