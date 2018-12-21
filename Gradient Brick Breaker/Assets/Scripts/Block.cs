using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour, Destroyable {
    //HP блока
    public int lifeCount;

    private BoxCollider2D boxCollider2D;
    private AudioSource audioSource;
    private Animation animation;
    private TextMesh hpTextMesh; // Ссылка на Text компонент дочернего объекта
    private SpriteRenderer spriteRenderer; //Ссылка на спрайтрендер
    private Color[] colors; //Массив возможных цветов
    private LevelManager levelManager;

    // Use this for initialization
    void Start () {
        boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animation>();
        hpTextMesh = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = GameManager.instance.GetLevelManager();
        colors = GameManager.instance.GetColorManager().generatedTopColors;
        UpdateBlock();
    }
	
    private void UpdateBlock()
    {
        hpTextMesh.text = lifeCount.ToString(); // обновляет Text в дочернем объекте
        spriteRenderer.material.color = colors[lifeCount]; // Задает цвет в соответствии с hp
    }

    public int TakeDamage(int damage)
    {
       

        if (damage >= lifeCount)
        {
            SelfDestroy();
        }
        else
        {
            lifeCount-=damage;
            UpdateBlock();
        }
        animation.Play();
        PlaySound();
        return lifeCount;
    }

    private void PlaySound()
    {
        if (GameManager.instance.sound == true)
        {
            audioSource.Play(0);
        }
    }

    public void SelfDestroy()
    {
        boxCollider2D.enabled = false;
        StartCoroutine(DestroyThisObject());
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.03);
        levelManager.RemoveGameObject(gameObject);   // ЗАМЕНИТЬ НА ЭВЕНТ
        Destroy(gameObject);
    }
}
