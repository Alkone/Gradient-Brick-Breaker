using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBound : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            GameController.StopBall(collision.gameObject);
        }
    }

}
