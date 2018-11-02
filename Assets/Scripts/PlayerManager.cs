using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float movementSpeed;

    private Interactable _interactableObject;
    private Rigidbody2D _rigidbody2D;
    private float _horizontal;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _rigidbody2D.velocity = new Vector2(_horizontal * movementSpeed, 0);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && _interactableObject != null)
            _interactableObject.Interact();

        _horizontal = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "interactable")
            _interactableObject = coll.GetComponent<Interactable>();
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "interactable")
            _interactableObject = null;
    }
}
