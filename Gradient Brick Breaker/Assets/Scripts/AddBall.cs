using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBall : MonoBehaviour, Destroyable {

    [SerializeField] private GameObject addablePrefab;
    private AudioSource audioSource;

    //Ссылка на контролер
    private LevelManager levelManager;

    private bool ballAdded = false;

    private void Start()
    {
        levelManager = GameManager.instance.GetLevelManager();
        audioSource = GetComponent<AudioSource>();
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
        PlaySound();
        Add();
        SelfDestroy();
    }

    public void SelfDestroy()
    {
        StartCoroutine("DestroyThisObject");
    }

    private void PlaySound()
    {
        if (GameManager.instance.sound == true)
        {
            audioSource.Play();
        }
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.06);
        levelManager.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
