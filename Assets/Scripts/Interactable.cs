using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool hasInteracted;

    public virtual void Interact()
    {
        Debug.Log("Interact with base class");
    }
}
