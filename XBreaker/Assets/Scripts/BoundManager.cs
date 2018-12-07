using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundManager : MonoBehaviour {
    private GameManager GM;

    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject botBound;
    public GameObject topBound;
    public float boardWidth;

    public Vector2 m_TopMiddleGameZone;
    public Vector2 m_BotMiddleGameZone;
    public float optimalCellPixelSize;

    private void Awake()
    {
        GM = GameManager.instance;
        Camera.main.orthographicSize = Screen.height / 2;
    }

    // Use this for initialization
    void Start() {

        optimalCellPixelSize = GM.GetLevelManager().GetOptimalCellPixelSize(); //Получаем размер ячейки из LevelManager-a
        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;
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

        Vector2 topBoundSpriteSize = topBound.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 topBoundLocalScale = GetRatioSpriteToGlobal(topBound, width, optimalCellPixelSize * 2f + cellDeltha / 2);
        topBC2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        topBC2D.autoTiling = true; // включаем авто растягивание коллайдера
        topBC2D.size = topBoundSpriteSize; //размер коллайдера = размеру спрайта
        topBound.transform.localScale = topBoundLocalScale; // задаем размеры GameObject

        Vector2 botBoundSpriteSize = botBound.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 botBoundLocalScale = GetRatioSpriteToGlobal(botBound, width, optimalCellPixelSize * 2f + cellDeltha / 2);
        botBC2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        botBC2D.autoTiling = true; // включаем авто растягивание коллайдера
        botBC2D.size = botBoundSpriteSize; //размер коллайдера = размеру спрайта
        botBound.transform.localScale = botBoundLocalScale; // задаем размеры GameObject

        //Передвигаем коллайдеры в зависимости от размера камеры
        leftBound.transform.position = new Vector2(-width / 2 - boardWidth/2 , 0);
        rightBound.transform.position = new Vector2(width / 2 + boardWidth / 2, 0);

        topBound.transform.position = new Vector2(0, height / 2 - topBoundSpriteSize.y * topBoundLocalScale.y/2);
        botBound.transform.position = new Vector2(0, -height / 2 + botBoundSpriteSize.y * botBoundLocalScale.y/2);

        m_TopMiddleGameZone = new Vector2(0, height/2 - (topBoundSpriteSize.y * topBoundLocalScale.y));
        m_BotMiddleGameZone = new Vector2(0, -height/2 + (botBoundSpriteSize.y * botBoundLocalScale.y));
        Debug.Log(" botBoundSpriteSize.y " + botBoundSpriteSize.y);
        Debug.Log(" botBoundRatio.y " + botBoundLocalScale.y);
        Debug.Log(" m_TopMiddleGameZone " + m_TopMiddleGameZone);
        Debug.Log(" m_BotMiddleGameZone " + m_BotMiddleGameZone);
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
