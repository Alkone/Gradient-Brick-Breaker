using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Destroyable {

    private bool coinPicked;
    private LevelManager levelManager;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D cc2D;

    void Start()
    {
        levelManager = GameManager.instance.GetLevelManager();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cc2D = GetComponent<CircleCollider2D>();
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
        spriteRenderer.enabled = false;
        cc2D.enabled = false;
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
        yield return new WaitForSeconds((float)0.1);
        levelManager.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
