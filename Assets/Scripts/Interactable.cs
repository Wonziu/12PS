using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool _hasInteracted;
    private bool _isInRange = false;

    public virtual void Interact()
    {
        Debug.Log("Interact with base class");
    }

    private void Update()
    {
        if (_isInRange)
            return;

        if (Input.GetKeyDown(KeyCode.E) && _hasInteracted)
        {
            _hasInteracted = true;
            Interact();
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isInRange = false;
        _hasInteracted = false;
    }
}
