using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseThrowable : MonoBehaviour 
    {
    protected Rigidbody2D rb2D;

    public bool isLaunched { get; set; } = false;
    public bool isMoving { get; set; } = false;

    private Vector2 velocity;


    //Фиксит скорость по y, чтобы шарик не летал горизонтально вечно
    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    protected void FixYVelocity()
    {
         Vector2 fixedVelocity = new Vector2(); ;
        velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        if (velocity.y > 0 && velocity.y < 0.2)
        {
            fixedVelocity.x = velocity.x;
            fixedVelocity.y = velocity.y + (float)0.01;
            gameObject.GetComponent<Rigidbody2D>().velocity = fixedVelocity;
            Debug.Log(gameObject.GetInstanceID().ToString() + " y_velocity=" + velocity.y + " fixed to " + fixedVelocity.y);
        }
        else if (velocity.y > -0.2 && velocity.y <= 0)
        {
            fixedVelocity.x = velocity.x;
            fixedVelocity.y = velocity.y - (float)0.01;
            gameObject.GetComponent<Rigidbody2D>().velocity = fixedVelocity;
        }
        Debug.Log(gameObject.GetInstanceID().ToString() + " y_velocity=" + velocity.y + " fixed to " + fixedVelocity.y);
    }
    //---
}
