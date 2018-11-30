﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BaseThrowable

{    public void Launch(Vector2 vector)
    {
        //rb2D.AddForce(vector, ForceMode2D.Impulse);
        //    Debug.Log("Ball " + gameObject.GetInstanceID() + " has launched!");
        //    isLaunched = true;
        //StartCoroutine(YVelocityFixer(rb2D));
        movingPosition = vector;
        isMoving = true;
        isLaunched = true;
    }

    public Vector2 Stop()
    {
        isMoving = false;
        isLaunched = false;
        inLaunchPosition = false;

        if (gameObject.layer == 9)
        {
            gameObject.layer = 8;
        }

        ///---
        ////Гашение перемещения по x
        //Vector2 stopVector = gameObject.GetComponent<Rigidbody2D>().velocity;
        //if(stopVector.x != 0)
        //{
        //    stopVector.x = 0;
        //    gameObject.GetComponent<Rigidbody2D>().velocity = stopVector;
        //    Debug.Log("X velocity is STOPED to " + gameObject.ToString());
        //}
        ////---
        ///
        Debug.Log("POSITION IN BALL " + rb2D.position.y);
        return rb2D.position;
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
