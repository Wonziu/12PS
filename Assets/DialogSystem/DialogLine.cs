using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public int id;
    public string text;

    public NodeType type;

    public List<int> choices = new List<int>();

    //for editor
    public Rect rect;
}