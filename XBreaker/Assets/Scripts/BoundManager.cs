using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundManager : MonoBehaviour {
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject botBound;
    public GameObject topBound;
    public float boardWidth;

    private Vector2 topBoundSpriteSize;
    private Vector2 botBoundSpriteSize;
    private Vector2 botBoundLocalScale;
    private Vector2 topBoundLocalScale;

    public float optimalCellPixelSize;

    private float width;
    private float height;

    private void Awake()
    {
        Camera.main.orthographicSize = Screen.height / 2;
    }

    private void Start()
    {

        optimalCellPixelSize = GameManager.instance.GetLevelManager().GetCellPixelSize(); //Получаем размер ячейки из LevelManager-a
        width = Camera.main.pixelWidth;
        height = Camera.main.pixelHeight;
        float cellDeltha = height % optimalCellPixelSize; // Остаток

        BoxCollider2D leftBC2D = leftBound.GetComponent<BoxCollider2D>();
        BoxCollider2D rightBC2D = rightBound.GetComponent<BoxCollider2D>();
        BoxCollider2D topBC2D = topBound.GetComponent<BoxCollider2D>();
        BoxCollider2D botBC2D = botBound.GetComponent<BoxCollider2D>();

        //Задаем размеры коллайдеров и местоположение относительно геймобджекта
        leftBC2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        leftBC2D.autoTiling = true; // включаем авто растягивание коллайдера
        leftBC2D.size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        leftBound.transform.localScale = new Vector2(boardWidth, height);

        rightBC2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        rightBC2D.autoTiling = true; // включаем авто растягивание коллайдера
        rightBC2D.size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        rightBound.transform.localScale = new Vector2(boardWidth, height);

        topBoundSpriteSize = topBound.GetComponent<SpriteRenderer>().sprite.bounds.size;
        topBoundLocalScale = GetRatioSpriteToGlobal(topBound, width, optimalCellPixelSize * 2f + cellDeltha / 2);
        topBC2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        topBC2D.autoTiling = true; // включаем авто растягивание коллайдера
        topBC2D.size = topBoundSpriteSize; //размер коллайдера = размеру спрайта
        topBound.transform.localScale = topBoundLocalScale; // задаем размеры GameObject

        botBoundSpriteSize = botBound.GetComponent<SpriteRenderer>().sprite.bounds.size;
        botBoundLocalScale = GetRatioSpriteToGlobal(botBound, width, optimalCellPixelSize * 2.3f + cellDeltha / 2);
        botBC2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        botBC2D.autoTiling = true; // включаем авто растягивание коллайдера
        botBC2D.size = botBoundSpriteSize; //размер коллайдера = размеру спрайта
        botBound.transform.localScale = botBoundLocalScale; // задаем размеры GameObject

        //Передвигаем коллайдеры в зависимости от размера камеры
        leftBound.transform.position = new Vector2(-width / 2 - boardWidth / 2, 0);
        rightBound.transform.position = new Vector2(width / 2 + boardWidth / 2, 0);

        topBound.transform.position = new Vector2(0, height / 2 - topBoundSpriteSize.y * topBoundLocalScale.y / 2);
        botBound.transform.position = new Vector2(0, -height / 2 + botBoundSpriteSize.y * botBoundLocalScale.y / 2);
    }

    public Vector2 GetTopMiddleGameZone()
    {
        return new Vector2(0, height / 2 - topBoundSpriteSize.y * topBoundLocalScale.y);
    }

    public Vector2 GetBotMiddleGameZone()
    {
        return new Vector2(0, -height / 2 + botBoundSpriteSize.y * botBoundLocalScale.y);
    }


    public Vector2 GetRatioSpriteToGlobal(GameObject gameObject, float x, float y)
    {
        float newX = x / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float newY = y / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        return new Vector2(newX, newY);
    }

    public Vector2 СonvertGlobalToLocalScale(GameObject gameObject, Vector2 localScale)
    {
        float newX = localScale.x / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float newY = localScale.y / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        return new Vector2(newX, newY);
    }

    public void SetBoundsColor(Color color){
        topBound.GetComponent<SpriteRenderer>().material.color = color;
        botBound.GetComponent<SpriteRenderer>().material.color = color;
    }

}
