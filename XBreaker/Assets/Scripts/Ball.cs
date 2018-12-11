using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private CircleCollider2D cc2D;
    private RaycastHit2D hit;

    public bool isLaunched { get; set; } = false;
    public bool isPrepairing { get; set; } = false;
    public bool isFalling { get; set; } = false;
    public float speed;
    public int damage;

    private float cirlceCastRadius;

    private Vector2 movingVector;
    private Vector2 startPosition;



    // Launch
    private int layerMask;
    Vector2 nextPoint;
    float movePerFixedUpdate;
    int stepCountBeforeCollision;
    private enum NextCollision { BLOCK, BOUND, BOTBOUND }
    private NextCollision nextCollision;
    bool giveDamage;


    private void Start()
    {
        gameObject.layer = 8;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        cc2D = gameObject.GetComponent<CircleCollider2D>();
        layerMask = (1 << 10) | (1 << 11) | (1 << 13);
        stepCountBeforeCollision = 0;
        cirlceCastRadius = cc2D.radius * gameObject.transform.localScale.x;
        movingVector = Vector2.down * 150;
        isFalling = true;
        isLaunched = true;
        giveDamage = false;
    }

    private void FixedUpdate()
    {

        if (isLaunched)
        {

            nextPoint = rb2D.position + movingVector * Time.fixedDeltaTime * speed * gameObject.transform.localScale.y;

            if (stepCountBeforeCollision == 0)
            {
                movePerFixedUpdate = (nextPoint - rb2D.position).magnitude;
                hit = Physics2D.CircleCast(rb2D.position, cirlceCastRadius, movingVector, 2000, layerMask);
                if (hit)
                {
                    stepCountBeforeCollision = (int)((hit.centroid - rb2D.position).magnitude / movePerFixedUpdate);
                    switch (hit.collider.gameObject.layer)
                    {
                        case 10:
                            {
                                nextCollision = NextCollision.BLOCK;
                                break;
                            }
                        case 11:  // Hit Bound
                            {
                                nextCollision = NextCollision.BOUND;
                            }
                            break;
                        case 13:  // Hit BotBound
                            {
                                nextCollision = NextCollision.BOTBOUND;
                            }
                            break;

                    }
                }
            }
            if (stepCountBeforeCollision <= 1)
            {
                switch (nextCollision)
                {
                    case NextCollision.BLOCK:
                        {
                            hit = Physics2D.CircleCast(rb2D.position, cirlceCastRadius, movingVector, Vector2.Distance(rb2D.position, nextPoint*2f), layerMask);

                            if (hit)
                            {
                               //Debug.Log("ID: " + gameObject.GetInstanceID() + " BLOOOOCK!!!");
                                nextPoint = hit.centroid;
                                if (hit.collider.gameObject.GetComponent<Block>())
                                {
                                    giveDamage = true;
                                }
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

            //Debug.Log("ID: " + gameObject.GetInstanceID() + " nextColission - " + nextCollision);
            //Debug.Log("ID: " + gameObject.GetInstanceID() + " stepCountBeforeCollision - " + stepCountBeforeCollision);
            if(stepCountBeforeCollision > 0) stepCountBeforeCollision--;

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
            if (Vector2.Distance(rb2D.position, startPosition) < 10)
            {
                nextPoint = startPosition;
                isPrepairing = false;
            }
            else
            {
                nextPoint = rb2D.position + (startPosition - rb2D.position) * Time.fixedDeltaTime * speed;
            }

            rb2D.MovePosition(nextPoint);
        }
    }

    public void Launch(Vector2 movingVector)
    {
        isPrepairing = false;
        this.movingVector = movingVector;
        gameObject.layer = 8;
        rb2D.WakeUp();
        isLaunched = true;

    }

    public void MoveToPosition(Vector2 position)
    {
        rb2D.WakeUp();
        isLaunched = false;
        startPosition = position;
        isPrepairing = true;
    }

    public void Stop(Vector2 stopPosition)
    {
        isLaunched = false;
        isFalling = false;
        gameObject.layer = 9;
        GameManager.instance.SetNewStartPosition(stopPosition);
        rb2D.Sleep();
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
