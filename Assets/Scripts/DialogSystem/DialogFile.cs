using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogFile : ScriptableObject
{
    public List<DialogLine> lines = new List<DialogLine>();	
}