using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class DialogHandler
{
    public string titleDesignation;
    public Sprite characterImg;
    public string dialogueTxt;
}
public class MissionManager : MonoBehaviour
{
    public DialogHandler[] dialogs;
    private int dialogIndex;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI dialogTxt;
    public Image characterImg;
    // Start is called before the first frame update
    private void Start()
    {
        ShowDialogUI(dialogIndex);
    }
    public void NextDialog()
    {
        if(dialogIndex<dialogs.Length-1)
        {
            dialogIndex++;
            ShowDialogUI(dialogIndex);
        }
    }
    public void PreviousDialog()
    {
        if (dialogIndex > 0)
        {
            dialogIndex--;
            ShowDialogUI(dialogIndex);
        }
    }
    private void ShowDialogUI(int index)
    {
        characterImg.sprite = dialogs[index].characterImg;
        dialogTxt.text = dialogs[index].dialogueTxt;
        titleTxt.text = dialogs[index].titleDesignation;
    }
}
