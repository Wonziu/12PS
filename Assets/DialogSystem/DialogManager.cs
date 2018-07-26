using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public UIManager uiManager;

    private int _currentId;
    private string _npcName;
    private Dialog _currentDialog;

    public void StartDialog(Dialog d, string name)
    {
        foreach (var dialogLine in d.dialog.lines)
            _currentDialog = d;

        ResetDialog();
        _npcName = name;

        uiManager.DisplayDialog(_npcName, _currentDialog.dialog.lines[0]);
    }

    public void OpenDoor()
    {
        Debug.Log("Opened");
    }

    public DialogLine[] GetChoices()
    {
        if (_currentDialog.dialog.lines[_currentId].type != NodeType.Choice)
            return new DialogLine[] { };

        List<DialogLine> choices = new List<DialogLine>();

        foreach (int id in _currentDialog.dialog.lines[_currentId].choices)
            choices.Add(_currentDialog.dialog.lines[id]);

        return choices.ToArray();
    }

    public void PickChoice(int id)
    {
        if (id == -1)
        {
            if (!(_currentDialog.invokeEventOnce && _currentDialog.wasEventInvoked) && _currentDialog.onDialogEnd != null)
            {
                _currentDialog.onDialogEnd.Invoke();
                _currentDialog.wasEventInvoked = true;
            }

            uiManager.HideDialog();
            return;
        }

        _currentId = id;

        if (GetChoices().Length == 0)
            uiManager.DisplayDialog(_npcName, _currentDialog.dialog.lines[_currentId]);
        else
            uiManager.DisplayChoice(_npcName, _currentDialog.dialog.lines[_currentId], GetChoices());
    }

    private void ResetDialog()
    {
        _currentId = 0;
    }
}