using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class DialogHandler
{
    public Sprite characterImg;
    public string dialogueTxt;
}
public class MissionManager : MonoBehaviour
{
    public DialogHandler[] dialogs;
    private int dialogIndex;
    public TextMeshProUGUI dialogTxt;
    public Image characterImg;
    // Start is called before the first frame update
   public void NextDialog()
    {
        if(dialogIndex<dialogs.Length-1)
        {
            dialogIndex++;
            characterImg.sprite = dialogs[dialogIndex].characterImg;
            dialogTxt.text = dialogs[dialogIndex].dialogueTxt;
        }
    }
    public void PreviousDialog()
    {
        if (dialogIndex > 0)
        {
            dialogIndex--;
            characterImg.sprite = dialogs[dialogIndex].characterImg;
            dialogTxt.text = dialogs[dialogIndex].dialogueTxt;
        }
    }
}
