using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Destroyable {

    private bool coinPicked;
    private LevelManager levelManager;
    private AudioSource audioSource;

    void Start()
    {
        levelManager = GameManager.instance.GetLevelManager();
        audioSource = GetComponent<AudioSource>();
        coinPicked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && !coinPicked)
        {
            AddCoinAndDestroyThis();
            coinPicked = true;
        }
    }

    private void Add()
    {
        GameManager.instance.AddCoin();
    }

    public void SelfDestroy()
    {
        StartCoroutine("DestroyThisObject");
    }

    public void AddCoinAndDestroyThis()
    {
        Add();
        PlaySound();
        SelfDestroy();
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
        yield return new WaitForSeconds((float)0.02);
        levelManager.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
