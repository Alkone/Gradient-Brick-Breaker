using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {

    //HP блока
    public int lifeCount;
    private TextMesh hpText; // Ссылка на Text компонент дочернего объекта
    private LevelManager levelManager; //Ссылка на контролер
    private SpriteRenderer spriteRenderer; //Ссылка на спрайтрендер
    private Color[] colors; //Массив возможных цветов
    private Animation blockAnimation;

	// Use this for initialization
	void Start () {
        hpText = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = GameManager.instance.GetLevelManager();
        colors = GameManager.instance.GetColorManager().GetBlockColors();
        UpdateBlock();
    }
	
    private void UpdateBlock()
    {
        hpText.text = lifeCount.ToString(); // обновляет Text в дочернем объекте
        spriteRenderer.material.color = colors[lifeCount]; // Задает цвет в соответствии с hp
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponent<Animation>().Play();
            if (lifeCount < 2)
            {
                SelfDestroy();
            }
            else
            {
                lifeCount--;
                UpdateBlock();
            }
        }
    }

    public void SelfDestroy()
    {
        StartCoroutine("DestroyThisObject");
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.01);
        levelManager.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
