using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private CircleCollider2D cc2D;
    private RaycastHit2D hit;

    public bool isLaunched = false;
    public bool isPrepairing = false;
    public bool isFalling = false;
    public float speed;
    public int damage;

    private float cirlceCastRadius;

    private Vector2 movingVector;
    private Vector2 startPosition;



    // Launch
    private int layerMask;
    Vector2 nextPoint;
    float movePerFixedUpdate;
    float stepCountBeforeCollision;
    private enum NextCollision { BLOCK, BOUND, BOTBOUND }
    private NextCollision nextCollision;
    bool giveDamage;

    LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        cc2D = gameObject.GetComponent<CircleCollider2D>();

        line.positionCount = 2;
        gameObject.layer = 8;
        layerMask = (1 << 10) | (1 << 11) | (1 << 13);
        cirlceCastRadius = cc2D.radius * gameObject.transform.localScale.x;

        Fall(Vector2.down);
    }

    private void FixedUpdate()
    {

        if (isLaunched)
        {

            nextPoint = rb2D.position + movingVector * Time.fixedDeltaTime * speed * gameObject.transform.localScale.y;
            if (stepCountBeforeCollision == 0)
            {
                movePerFixedUpdate = (nextPoint - rb2D.position).magnitude;
                if (isFalling)
                {
                    hit = Physics2D.CircleCast(rb2D.position, cirlceCastRadius, movingVector, Mathf.Infinity, 1 << 13);
                }
                else
                {
                    hit = Physics2D.CircleCast(rb2D.position, cirlceCastRadius, movingVector, Mathf.Infinity, layerMask);
                }
                if (hit)
                {
                    line.SetPosition(0, rb2D.position);
                    line.SetPosition(1, hit.centroid);
                    stepCountBeforeCollision = (hit.centroid - rb2D.position).magnitude / movePerFixedUpdate;
                    switch (hit.collider.gameObject.layer)
                    {
                        case 10:
                            nextCollision = NextCollision.BLOCK;
                            break;
                        case 11:  // Hit Bound
                            nextCollision = NextCollision.BOUND;
                            break;
                        case 13:  // Hit BotBound
                            nextCollision = NextCollision.BOTBOUND;
                            break;

                    }
                }
            }
            if (stepCountBeforeCollision <= 1f)
            {
                switch (nextCollision)
                {
                    case NextCollision.BLOCK:
                        {
                            nextPoint = hit.centroid;
                            if (hit.collider != null)
                            {
                                giveDamage = true;
                            }
                        }
                        break;
                    case NextCollision.BOUND:
                        {
                            nextPoint = hit.centroid;
                            movingVector = Vector2.Reflect(movingVector, hit.normal);
                        }
                        break;
                    case NextCollision.BOTBOUND:
                        {
                            nextPoint = hit.centroid;
                            Stop(nextPoint);
                        }
                        break;
                }
                stepCountBeforeCollision = 0;
            }
            else
            {
                stepCountBeforeCollision--;
                // Debug.Log("stepCountBeforeCollision" + stepCountBeforeCollision);
            }

            //bool giveDamage = false;
            //nextPoint = rb2D.position + movingVector * Time.fixedDeltaTime * speed * gameObject.transform.localScale.y;
            //hit = Physics2D.CircleCast(rb2D.position, cirlceCastRadius, movingVector, Vector2.Distance(rb2D.position, nextPoint), layerMask);
            //if (hit)
            //{
            //    if (hit.collider.gameObject.layer == 13)
            //    {
            //        nextPoint = hit.centroid;
            //        Stop(nextPoint);
            //    }
            //    else if (!isFalling && hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11)
            //    {
            //        nextPoint = hit.centroid;
            //        if(hit.collider.gameObject.GetComponent<Block>()){
            //            giveDamage = true;
            //        } else{
            //            movingVector = Vector2.Reflect(movingVector, hit.normal);
            //        }
            //    }
            //}

            rb2D.MovePosition(nextPoint);
            if (giveDamage)
            {
                int blockHP = hit.collider.gameObject.GetComponent<Block>().TakeDamage(damage);
                if (blockHP > 0)
                {
                    movingVector = Vector2.Reflect(movingVector, hit.normal);
                }
                giveDamage = false;
            }
        }
        else if (isPrepairing)
        {
            if (Vector2.Distance(rb2D.position, startPosition) < 100)
            {
                nextPoint = startPosition;
                Stop();
            }
            else
            {
                nextPoint = rb2D.position + (startPosition - rb2D.position) * Time.fixedDeltaTime * speed / 2;
            }

            rb2D.MovePosition(nextPoint);
        }
    }

    public void Launch(Vector2 movingVector)
    {
        stepCountBeforeCollision = 0;
        isPrepairing = false;
        this.movingVector = movingVector;
        gameObject.layer = 8;
        rb2D.WakeUp();
        isLaunched = true;

    }

    public void MoveToPosition(Vector2 position)
    {
        if (rb2D.position != position)
        {
            if (rb2D.IsSleeping())
            {
                rb2D.WakeUp();
            }
            if (gameObject.layer != 9)
            {
                gameObject.layer = 9;
            }
            isLaunched = false;
            startPosition = position;
            isPrepairing = true;
        }
    }

    public void Stop(Vector2 stopPosition)
    {
        isLaunched = false;
        isFalling = false;
        isPrepairing = false;
        gameObject.layer = 9;
        GameManager.instance.SetNewStartPosition(stopPosition);
        rb2D.Sleep();
    }

    public void Stop()
    {
        isLaunched = false;
        isFalling = false;
        isPrepairing = false;
        gameObject.layer = 9;
        rb2D.Sleep();
    }

    public void Return(Vector2 startPosition)
    {
        MoveToPosition(startPosition);
    }

    private void Fall(Vector2 movingVector)
    {
        stepCountBeforeCollision = 0;
        this.movingVector = movingVector;
        isFalling = true;
        isLaunched = true;
        giveDamage = false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 10 || collision.gameObject.layer == 11)
    //    {
    //        movingVector = Vector2.Reflect(movingVector, collision.contacts[0].normal);
    //    }
    //}

    //private void FixYVelocity(Rigidbody2D rb2D)
    //{
    //    Vector2 fixedVelocity = new Vector2(); ;
    //    velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
    //    if (velocity.y > 0 && velocity.y < 0.1)
    //    {
    //        fixedVelocity.x = velocity.x;
    //        fixedVelocity.y = velocity.y + (float)0.1;
    //        rb2D.velocity = fixedVelocity;
    //        Debug.Log(gameObject.GetInstanceID().ToString() + " y_velocity=" + velocity.y + " fixed to " + fixedVelocity.y);
    //    }
    //    else if (velocity.y > -0.1 && velocity.y <= 0)
    //    {
    //        fixedVelocity.x = velocity.x;
    //        fixedVelocity.y = velocity.y - (float)0.1;
    //        rb2D.velocity = fixedVelocity;
    //    }
    //    Debug.Log(gameObject.GetInstanceID().ToString() + " y_velocity=" + velocity.y + " fixed to " + fixedVelocity.y);
    //}

    //private IEnumerator YVelocityFixer(Rigidbody2D rb2D)
    //{
    //    float timeYVelocityZero = 0;
    //    bool timeGetted = false;

    //    while (isLaunched)
    //    {
    //        if (rb2D.velocity.y < 0.1)
    //        {
    //            if (!timeGetted)
    //            {
    //                timeYVelocityZero = Time.realtimeSinceStartup;
    //                timeGetted = true;
    //            }
    //            else
    //            {
    //                if ((Time.realtimeSinceStartup - timeYVelocityZero) > 2)
    //                {
    //                    FixYVelocity(rb2D);
    //                }
    //            }
    //        }
    //        else if (timeGetted)
    //        {
    //            timeGetted = false;
    //        }
    //        yield return new WaitForSecondsRealtime(0.5f);
    //    }
    //}



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
