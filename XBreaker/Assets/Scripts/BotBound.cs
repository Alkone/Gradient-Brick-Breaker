using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBound : MonoBehaviour
{
    public bool doOnce = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 && !doOnce)
        {
            GameManager.instance.LoseGame();
            doOnce = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AddBall>())
        {
            other.gameObject.GetComponent<AddBall>().AddBallAndDestroyThis();
        }
    }
}
