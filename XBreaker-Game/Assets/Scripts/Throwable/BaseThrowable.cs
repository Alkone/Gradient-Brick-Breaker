using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseThrowable : MonoBehaviour
{
    protected Rigidbody2D rb2D;

    public bool isLaunched { get; set; } = false;
    public bool isMoving { get; set; } = false;
    public bool inLaunchPosition { get; set; } = false;

    private Vector2 fixVelocity;
    protected Vector2 movingPosition;



    //Фиксит скорость по y, чтобы шарик не летал горизонтально вечно
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        if (isMoving)
        {
            rb2D.MovePosition(rb2D.position + movingPosition * Time.fixedDeltaTime * 20);

        } else if(GameManager.instance.gameStatus == GameStatus.PREPARING)
        {
            Vector2 startPosition = GameManager.instance.startPosition;
            if (rb2D.position == startPosition)
            {
                inLaunchPosition = true;
            }
            else
            {
                Debug.Log("ball pos " + rb2D.position.x + " " + rb2D.position.y);
                Debug.Log("startpos " + startPosition.x +" " + startPosition.y);
                if (rb2D.position != startPosition)
                {
                    if (Mathf.Abs(rb2D.position.x - startPosition.x) < 0.05 && Mathf.Abs(rb2D.position.y - startPosition.y) < 0.05)
                    {
                        rb2D.MovePosition(startPosition);
                    }
                    else
                    {

                        rb2D.MovePosition(rb2D.position + (startPosition - rb2D.position).normalized * Time.fixedDeltaTime * 5);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 11)
        {
            movingPosition = Vector2.Reflect(movingPosition, collision.contacts[0].normal);
        }
        //if (collision.gameObject.layer == 13)
        //{
        //    isMoving = false;
        //}
    }

    protected void FixYVelocity(Rigidbody2D rb2D)
    {
        Vector2 fixedVelocity = new Vector2(); ;
        fixVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        if (fixVelocity.y > 0 && fixVelocity.y < 0.1)
        {
            fixedVelocity.x = fixVelocity.x;
            fixedVelocity.y = fixVelocity.y + (float)0.1;
            rb2D.velocity = fixedVelocity;
            Debug.Log(gameObject.GetInstanceID().ToString() + " y_velocity=" + fixVelocity.y + " fixed to " + fixedVelocity.y);
        }
        else if (fixVelocity.y > -0.1 && fixVelocity.y <= 0)
        {
            fixedVelocity.x = fixVelocity.x;
            fixedVelocity.y = fixVelocity.y - (float)0.1;
            rb2D.velocity = fixedVelocity;
        }
        Debug.Log(gameObject.GetInstanceID().ToString() + " y_velocity=" + fixVelocity.y + " fixed to " + fixedVelocity.y);
    }

    protected IEnumerator YVelocityFixer(Rigidbody2D rb2D)
    {
        float timeYVelocityZero = 0;
        bool timeGetted = false;

        while (isLaunched)
        {
            if (rb2D.velocity.y < 0.1)
            {
                if (!timeGetted)
                {
                    timeYVelocityZero = Time.realtimeSinceStartup;
                    timeGetted = true;
                }
                else
                {
                    if ((Time.realtimeSinceStartup - timeYVelocityZero) > 2)
                    {
                        FixYVelocity(rb2D);
                    }
                }
            }
            else if (timeGetted)
            {
                timeGetted = false;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    //---
}
