using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {
    //Характеристики блоков
    public static float pixelSize = 0;
    //HP блока
    public int lifeCount;

    private TextMesh hpTextMesh; // Ссылка на Text компонент дочернего объекта
    private SpriteRenderer spriteRenderer; //Ссылка на спрайтрендер
    private Color[] colors; //Массив возможных цветов

    // Use this for initialization
    void Start () {
        if (pixelSize < 0.01f)
        {
            pixelSize = GameManager.instance.GetLevelManager().GetOptimalCellPixelSize();
        }
        gameObject.transform.localScale = new Vector3(pixelSize, pixelSize, 0);
        hpTextMesh = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colors = GameManager.instance.GetColorManager().generatedColors;
        UpdateBlock();
    }
	
    private void UpdateBlock()
    {
        hpTextMesh.text = lifeCount.ToString(); // обновляет Text в дочернем объекте
        spriteRenderer.material.color = colors[lifeCount]; // Задает цвет в соответствии с hp
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
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
       //levelManager.RemoveGameObject(gameObject);    ЗАМЕНИТЬ НА ЭВЕНТ
        Destroy(gameObject);
    }
}
