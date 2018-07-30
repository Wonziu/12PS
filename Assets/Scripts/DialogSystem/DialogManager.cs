using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public UIManager uiManager;

    private bool _invokeEvent;
    private int _currentId;
    private string _npcName;
    private DialogFile _currentDialog;
    private UnityEvent _dialogEvent;

    public void StartDialog(EventDialog e, string npcName)
    {
        ResetDialog();

        _currentDialog = e.dialog;

        if (e.onDialogEnd != null)
        {
            _invokeEvent = !(e.invokeEventOnce && e.wasEventInvoked);

            if (_invokeEvent)
            {
                _dialogEvent = e.onDialogEnd;
                e.wasEventInvoked = true;
            }
        }

        SetDialog(npcName);
    }

    public void StartDialog(QuestDialog q, string npcName)
    {
        ResetDialog();

        _currentDialog = q.dialog;

        _dialogEvent = q.onDialogEnd;
        _invokeEvent = true;

        SetDialog(npcName);
    }

    public void StartDialog(Dialog d, string npcName)
    {
        ResetDialog();

        _currentDialog = d.dialog;
        _invokeEvent = false;

        SetDialog(npcName);
    }

    public DialogLine[] GetChoices()
    {
        if (_currentDialog.lines[_currentId].type != NodeType.Choice)
            return new DialogLine[] { };

        List<DialogLine> choices = new List<DialogLine>();

        foreach (int id in _currentDialog.lines[_currentId].choices)
            choices.Add(_currentDialog.lines[id]);

        return choices.ToArray();
    }

    public void PickChoice(int id)
    {
        if (id == -1)
        {
            if (_invokeEvent)
                _dialogEvent.Invoke();

            uiManager.ToggleDialog();
            return;
        }

        _currentId = id;

        if (GetChoices().Length == 0)
            uiManager.DisplayDialog(_npcName, _currentDialog.lines[_currentId]);
        else
            uiManager.DisplayChoice(_npcName, _currentDialog.lines[_currentId], GetChoices());
    }

    private void SetDialog(string s)
    {
        _npcName = s;

        uiManager.ToggleDialog();
        uiManager.DisplayDialog(_npcName, _currentDialog.lines[0]);
    }

    private void ResetDialog()
    {
        _currentId = 0;
        _dialogEvent = null;
    }
}