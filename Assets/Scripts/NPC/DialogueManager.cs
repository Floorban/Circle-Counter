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
    public bool canTalk, hasTalked;

    private void Update()
    {
        HandleDialogue();
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        canTalk = true;
        index = 0;
        FindObjectOfType<RevolverController>().canInteract = false;
        FindObjectOfType<PlayerController>().interactPanel.SetActive(false);
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
            yield return new WaitForSeconds(typeSpeed * Time.deltaTime);
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
            hasTalked = true;
            FindObjectOfType<RevolverController>().canInteract = true;
            FindObjectOfType<RevolverController>().hasStarted = true;
            //GetComponent<Bartender>().OnInteract();
        }
    }
}
