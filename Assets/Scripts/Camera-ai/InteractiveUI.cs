using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractiveUI : MonoBehaviour
{
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject choicePanel;
    [SerializeField] TextMeshProUGUI choiceNameText;
    public People targetPpl;
    private void OnEnable()
    {
        Actions.onMouseOverPpl += ShowTargetInfo;
        Actions.onClickPpl += ShowRateButtons;
        Actions.onMouseExitPpl += HideInfoPanel;
    }
    private void OnDisable()
    {
        Actions.onMouseOverPpl -= ShowTargetInfo;
        Actions.onClickPpl -= ShowRateButtons;
        Actions.onMouseExitPpl -= HideInfoPanel;
    }
    private void Start()
    {
        infoPanel.SetActive(false);
    }
    public void ShowTargetInfo(People ppl)
    {
        infoPanel.SetActive(true);
        targetPpl = ppl;
        dialogueText.text = targetPpl.dialogues[0];
        nameText.text = targetPpl._name;
    }
    public void ShowRateButtons(People ppl)
    {
        choicePanel.SetActive(true);
        choiceNameText.text = ppl._name + " IS";
    }
    public void HideInfoPanel()
    {
        infoPanel.SetActive(false);
        dialogueText.text = string.Empty;
        nameText.text = string.Empty;
    }
    
    public void RateTarget(int my_rate)
    {
        if (targetPpl == null) return;

        switch (my_rate)
        {
            case 0:
                targetPpl.ChangeColor(Color.green);
                break;

            case 1:
                targetPpl.ChangeColor(Color.grey);
                break;

            case 2:
                targetPpl.ChangeColor(Color.red);
                break;
        }    

        targetPpl.rate = my_rate;
        targetPpl = null;
    }
}
