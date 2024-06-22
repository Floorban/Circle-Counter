using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //Animator animator;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string[] dialogues;
    [SerializeField] float typeSpeed;
    int index;
    public bool canTalk;

    private void Update()
    {
        HandleDialogue();
    }

    public void StartDialogue()
    {
        canTalk = true;
        index = 0;
        StartCoroutine(TypeTextLine());
    }
    public void HandleDialogue()
    {
        if (!canTalk) return;

        if (Input.GetKeyDown(KeyCode.E))
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
    IEnumerator TypeTextLine()
    {
        //FindObjectOfType<SoundManager>().PlaySound("Typing", 1f);

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
            text.text = string.Empty;
            dialoguePanel.SetActive(false);
        }
    }
}
