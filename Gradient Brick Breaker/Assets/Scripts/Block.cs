using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour, Destroyable {
    //HP блока
    public int lifeCount;

    private BoxCollider2D boxCollider2D;
    private TextMesh hpTextMesh; // Ссылка на Text компонент дочернего объекта
    private SpriteRenderer spriteRenderer; //Ссылка на спрайтрендер
    private Color[] colors; //Массив возможных цветов
    private LevelManager levelManager;

    // Use this for initialization
    void Start () {
        boxCollider2D = GetComponent<BoxCollider2D>();
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

    public int TakeDamage(int damdge)
    {

        GetComponent<Animation>().Play();
        if (lifeCount < 2)
        {
            SelfDestroy();
        }
        else
        {
            lifeCount-=damdge;
            UpdateBlock();
        }
        return lifeCount;
    }

    public void SelfDestroy()
    {
        boxCollider2D.enabled = false;
        StartCoroutine(DestroyThisObject());
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.02);
        levelManager.RemoveGameObject(gameObject);   // ЗАМЕНИТЬ НА ЭВЕНТ
        Destroy(gameObject);
    }
}
