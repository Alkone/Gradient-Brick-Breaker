using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BaseThrowable, IThrowable
{
    public void Launch(Vector2 vector)
    {
            rb2D.AddForce(vector, ForceMode2D.Impulse);
            Debug.Log("Ball " + gameObject.GetInstanceID() + " has launched!");
            isLaunched = true;
    }

    public void Stop()
    {
        //Гашение перемещения по x
        Vector2 stopVector = gameObject.GetComponent<Rigidbody2D>().velocity;
        if(stopVector.x != 0)
        {
            stopVector.x = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = stopVector;
            Debug.Log("X velocity is Fixed to " + gameObject.ToString());
        }
        //---

        //Подготовка новых шариков шариков
        if(gameObject.layer == 9)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.layer = 8;
        }
        ///---
 
        Debug.Log("Ball " + gameObject.GetInstanceID() + " has stopped!");
        isLaunched = false;
    }

    private void FixedUpdate()
    {
        if (isLaunched)
        {
            if(gameObject.GetComponent<Rigidbody2D>().velocity.y<0.2 && gameObject.GetComponent<Rigidbody2D>().velocity.y > -0.2) FixYVelocity();
        }
    }
}
