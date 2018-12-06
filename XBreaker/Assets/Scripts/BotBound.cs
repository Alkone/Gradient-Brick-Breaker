using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBound : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
         //   GameManager.instance.StopBall(collision.gameObject);
        }
        else if (collision.gameObject.layer == 10)
        {
            GameManager.instance.LoseGame();
        }
        else if (collision.gameObject.layer == 12)
        {
            collision.gameObject.GetComponent<AddBall>().AddBallAndDestroyThis();
        }
    }

}
