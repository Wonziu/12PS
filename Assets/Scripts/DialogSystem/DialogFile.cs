using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogFile : ScriptableObject
{
    public int maxId = 0;
    public List<DialogLine> lines = new List<DialogLine>();	
}