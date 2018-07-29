using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public DialogManager dialogManager;

    public Button buttonToInstantiate;
    public Text npcName;
    public Text npcText;
    public RectTransform choicesParent;
    public RectTransform dialogParent;

    public void DisplayDialog(string name, DialogLine line)
    {
        npcName.text = name;
        ClearButtons();

        npcText.text = line.text;

        var button = Instantiate(buttonToInstantiate, choicesParent);

        button.transform.GetChild(0).GetComponent<Text>().text = "Kontynuj";

        button.onClick.AddListener(() =>
        {
            if (line.choices.Count == 0)
                dialogManager.PickChoice(-1);
            else
                dialogManager.PickChoice(line.choices[0]);
        });
    }

    public void DisplayChoice(string name, DialogLine parent, params DialogLine[] lines)
    {
        npcName.text = name;
        ClearButtons();

        npcText.text = parent.text;

        foreach (var dialogLine in lines)
        {
            var button = Instantiate(buttonToInstantiate, choicesParent);
            button.transform.GetChild(0).GetComponent<Text>().text = dialogLine.text;

            var line = dialogLine;
            button.onClick.AddListener(() =>
            {
                if (line.choices.Count == 0)
                    dialogManager.PickChoice(-1);
                else
                    dialogManager.PickChoice(line.choices[0]);
            });
        }
    }

    public void HideDialog()
    {
        dialogParent.gameObject.SetActive(!dialogParent.gameObject.activeSelf);
    }

    private void ClearButtons()
    {
        foreach (RectTransform child in choicesParent)
        {
            Destroy(child.gameObject); // TODO: pool object
        }
    }
}
