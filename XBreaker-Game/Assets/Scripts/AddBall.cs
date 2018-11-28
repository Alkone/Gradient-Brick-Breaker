using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBall : MonoBehaviour {

    [SerializeField] private GameObject addablePrefab;

    //Ссылка на контролер
    private LevelManager levelManager;

    private bool ballAdded = false;

    private void Start()
    {
        levelManager = GameManager.instance.GetLevelManager();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && !ballAdded)
        {
            AddBallAndDestroyThis();
        }
    }

    private void Add()
    {
        GameManager.instance.CreateBall(gameObject.transform.position, addablePrefab);
        ballAdded = true;
    }

    public void AddBallAndDestroyThis()
    {
        Add();
        StartCoroutine("DestroyThisObject");
    }

    public void DestroyOnly()
    {
        StartCoroutine("DestroyThisObject");
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.02);
        levelManager.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
