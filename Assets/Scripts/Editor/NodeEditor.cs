using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public class NodeEditor : EditorWindow
{
    private Texture2D _backgroundTexture;

    private DialogFile _currentDialog;
    private DialogLine _selectedLine;

    private int _selectedNodeId = -1;
    private bool _connecting = false;

    private readonly Color _bigLines = new Color(0.25f, 0.25f, 0.25f);
    private readonly Color _smallLines = new Color(0.30f, 0.30f, 0.30f);

    private Vector2 _scrollPosition = Vector2.zero;
    private Vector2 _previousPosition = Vector2.zero;

    private Vector2 _workSpace = new Vector2(2000, 2000);

    [MenuItem("Window/Dialog Editor")]
    public static void ShowDialog()
    {
        NodeEditor editor = GetWindow<NodeEditor>();
        editor.titleContent = new GUIContent("Dialog Editor");
    }

    private void OnEnable()
    {
        _backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        _backgroundTexture.SetPixel(0, 0, new Color(0.35f, 0.35f, 0.35f));
        _backgroundTexture.Apply();
    }

    public void OnGUI()
    {
        LoadDialog();

        ProcessEvents(Event.current);

        _scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scrollPosition, new Rect(Vector2.zero, _workSpace), GUIStyle.none, GUIStyle.none);
        GUI.DrawTexture(new Rect(0, 0, _workSpace.x, _workSpace.y), _backgroundTexture, ScaleMode.StretchToFill);

        DrawGrid();

        if (IfDialogExists())
        {
            BeginWindows();
            DrawNodes();
            EndWindows();

            DrawConnections();
        }

        GUI.EndScrollView();

        if (_currentDialog.lines.Count == 0)
            if (GUILayout.Button("Create New", GUILayout.Width(200), GUILayout.Height(30)))
                ClearNodes();
    }

    private void LoadDialog()
    {
        if (Selection.activeObject && Selection.activeObject is DialogFile)
        {
            _currentDialog = (DialogFile)Selection.activeObject;
            Debug.Log(_currentDialog.maxId);
        }
        else
        {
            _currentDialog = null;
            Debug.Log("No object is currently selected");
        }
    }

    private bool IfDialogExists()
    {
        return _currentDialog != null;
    }

    private void CreateFirstNode()
    {
        DialogLine newLine = new DialogLine
        {
            id = 0,
            rect = new Rect(20, 50, 150, 120),
            text = "Witaj!",
            type = NodeType.Start
        };

        if (_currentDialog.lines == null)
            _currentDialog.lines = new List<DialogLine>();

        _selectedNodeId = 0;
        _currentDialog.lines.Add(newLine);
    }

    private void WindowFunction(int windowId)
    {
        if (Event.current.type == EventType.MouseDown)
        {
            _selectedNodeId = windowId;

            if (_connecting)
                ConnectNodes();
        }

        DialogLine currentLine = _currentDialog.lines[windowId];

        currentLine.text = GUI.TextArea(new Rect(0, 15, 150, 85), currentLine.text);


        if (GUI.Button(new Rect(0, 100, 20, 20), "-"))
            RemoveNode();
        if (GUI.Button(new Rect(20, 100, 20, 20), "<-"))
            RemoveConnections(currentLine.id);

        if (currentLine.type == NodeType.Choice || currentLine.choices.Count == 0)
        {
            if (GUI.Button(new Rect(130, 100, 20, 20), "+"))
                CreateNode();
            if (GUI.Button(new Rect(110, 100, 20, 20), "->"))
            {
                _connecting = true;
                _selectedLine = currentLine;
            }
        }

        GUI.DragWindow();
    }

    private void DrawConnections()
    {
        foreach (var line in _currentDialog.lines)
            foreach (var choice in line.choices)
                DrawNodeCurve(line.rect, _currentDialog.lines.Find(x => x.id == choice).rect);

        if (!_connecting) return;

        Rect mousePosRect = new Rect(Event.current.mousePosition, Vector2.zero);

        DrawNodeCurve(_currentDialog.lines[_selectedNodeId].rect, mousePosRect);
        Repaint();
    }

    private void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;

        Color color = new Color(153, 153, 153);

        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 4);
    }

    private void DrawNodes()
    {
        int windowsIndex = 0;
        foreach (DialogLine dialogLine in _currentDialog.lines)
        {
            if (dialogLine.choices.Count > 1)
                dialogLine.type = NodeType.Choice;

            if (dialogLine.type == NodeType.Choice)
                foreach (var line in dialogLine.choices)
                    _currentDialog.lines.Find(x => x.id == line).type = NodeType.Option;

            string style = "flow node 1";
            string boxName = dialogLine.choices.Count == 0 ? "End (Dialog)" : "Dialog";
            switch (dialogLine.type)
            {
                case NodeType.Start:
                    style = "flow node 6";
                    boxName = "Start";
                    break;
                case NodeType.Choice:
                    style = "flow node 4";
                    boxName = "Choice";
                    break;
                case NodeType.Option:
                    style = "flow node 3";
                    boxName = dialogLine.choices.Count == 0 ? "End (Option)" : "Option";
                    break;
            }

            GUIStyle finalStyle = new GUIStyle(style)
            {
                fontSize = 14,
                clipping = TextClipping.Clip,
                contentOffset = new Vector2(0, -30)
            };

            dialogLine.rect = CheckPositions(dialogLine.rect);
            dialogLine.rect = GUI.Window(windowsIndex, dialogLine.rect, WindowFunction, boxName, finalStyle);
            windowsIndex++;
        }
    }

    private void CreateNode()
    {
        DialogLine newLine = new DialogLine
        {
            id = ++_currentDialog.maxId,
            type = NodeType.Default,
            text = "Sample text",
            rect = _currentDialog.lines[_selectedNodeId].rect
        };

        newLine.rect.position = new Vector2(newLine.rect.x + 220, newLine.rect.y);

        _currentDialog.lines[_selectedNodeId].choices.Add(newLine.id);
        _currentDialog.lines.Add(newLine);

        EditorUtility.SetDirty(_currentDialog);
    }

    private void DrawGrid()
    {
        Handles.BeginGUI();

        for (int i = 0; i < _workSpace.x / 10; i++)
            EditorGUI.DrawRect(new Rect(i * 10, 0, 2, _workSpace.y), _smallLines);
        for (int i = 0; i < _workSpace.y / 10; i++)
            EditorGUI.DrawRect(new Rect(0, i * 10, _workSpace.x, 2), _smallLines);
        for (int i = 0; i < _workSpace.x / 100; i++)
            EditorGUI.DrawRect(new Rect(i * 100, 0, 2, _workSpace.y), _bigLines);
        for (int i = 0; i < _workSpace.y / 100; i++)
            EditorGUI.DrawRect(new Rect(0, i * 100, _workSpace.x, 2), _bigLines);

        Handles.EndGUI();
    }

    private void ConnectNodes()
    {
        _connecting = false;

        if (_currentDialog.lines[_selectedNodeId].type == NodeType.Start ||
            _currentDialog.lines[_selectedNodeId].id == _selectedLine.id ||
            _selectedLine.choices.Contains(_currentDialog.lines[_selectedNodeId].id))
            return;

        _selectedLine.choices.Add(_currentDialog.lines[_selectedNodeId].id);
    }

    private void RemoveConnections(int lineId)
    {
        foreach (var line in _currentDialog.lines)
            if (line.choices.Contains(lineId))
                line.choices.Remove(lineId);
    }

    private void RemoveNode()
    {
        if (_currentDialog.lines[_selectedNodeId].type == NodeType.Start)
            return;

        var lineToRemove = _currentDialog.lines[_selectedNodeId];

        RemoveConnections(lineToRemove.id);

        _currentDialog.lines.Remove(lineToRemove);
    }

    private Rect CheckPositions(Rect line)
    {
        Vector2 newPos = line.position;

        if (line.center.x - 75 < 0)
            line.position = new Vector2(0, newPos.y);
        if (line.center.y - 60 < 0)
            line.position = new Vector2(newPos.x, 0);

        return line;
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                    ProcessContextMenu();
                break;
            case EventType.MouseDrag:
                if (e.button == 2)
                {
                    Vector2 currentPos = Event.current.mousePosition;

                    if (Vector2.Distance(currentPos, _previousPosition) < 50)
                    {
                        float x = _previousPosition.x - currentPos.x;
                        float y = _previousPosition.y - currentPos.y;

                        _scrollPosition.x += x;
                        _scrollPosition.y += y;
                        Event.current.Use();
                    }
                    _previousPosition = currentPos;
                }
                break;
        }
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Clear All"), false, ClearNodes);

        if (_connecting)
            genericMenu.AddItem(new GUIContent("Cancel"), false, CancelConnection);

        for (var i = 0; i < _currentDialog.lines.Count; i++)
        {
            if (_connecting)
                return;

            var line = _currentDialog.lines[i];
            Rect windowArea = new Rect(line.rect.position - _scrollPosition, line.rect.size);

            if (windowArea.Contains(Event.current.mousePosition))
            {
                if (line.type == NodeType.Default)
                    genericMenu.AddItem(new GUIContent("Convert to Choice"), false, () => Convert(line));
                else if (line.type == NodeType.Choice && line.choices.Count == 1)
                    genericMenu.AddItem(new GUIContent("Convert to Default"), false, () => Convert(line));
                else if (line.type == NodeType.Choice)
                    genericMenu.AddDisabledItem(new GUIContent("Convert to Default"));
            }
        }

        genericMenu.ShowAsContext();
    }

    private void ClearNodes()
    {
        _currentDialog.lines.Clear();
        _currentDialog.maxId = 0;

        CreateFirstNode();
    }

    private void CancelConnection()
    {
        _connecting = false;
    }

    private void Convert(DialogLine line)
    {
        line.type = line.type == NodeType.Choice ? NodeType.Default : NodeType.Choice;
    }
}