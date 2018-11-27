using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBall : MonoBehaviour {

    [SerializeField] private GameObject addablePrefab;

    private bool ballAdded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && !ballAdded)
        {
            Destroy();
            GameManager.instance.CreateBall(gameObject.transform.position, addablePrefab);
            ballAdded = true;
        }
    }

    public void Destroy()
    {
        StartCoroutine("DestroyThisObject");
    }


    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.02);
        Destroy(gameObject);
    }
}
