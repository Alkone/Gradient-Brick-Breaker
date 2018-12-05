using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BaseThrowable, IThrowable

{    public void Launch(Vector2 vector)
    {
        rb2D.AddForce(vector, ForceMode2D.Impulse);
            Debug.Log("Ball " + gameObject.GetInstanceID() + " has launched!");
            isLaunched = true;
        StartCoroutine(YVelocityFixer(rb2D));
    }

    public void Stop()
    {
        //Гашение перемещения по x
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
     

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

    public void DestroyBall()
    {
        StartCoroutine(DestroyThisObject());
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.02);
        Destroy(gameObject);
    }


}
