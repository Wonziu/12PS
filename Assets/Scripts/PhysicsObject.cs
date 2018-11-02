using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f;

    protected Vector2 _velocity;

    private void FixedUpdate()
    {
        _velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = _velocity * Time.deltaTime;

        Vector2 move = Vector2.up * deltaPosition.y;
        
        Movement(move);
    }

    private void Movement(Vector2 move)
    {

    }
}