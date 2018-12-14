using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundManager : MonoBehaviour {
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject botBound;
    public GameObject topBound;
    public float boardWidth;


    public float optimalCellPixelSize;

    private float width;
    private float height;

    private Vector2 topScale;
    private Vector2 botScale;

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
        float pixelMarginLRBounds = GameManager.instance.GetLevelManager().pixelMarginLRBounds;
        float pixelMarginTBBounds = GameManager.instance.GetLevelManager().pixelMarginTBBounds;

        BoxCollider2D leftBC2D = leftBound.GetComponent<BoxCollider2D>();
        BoxCollider2D rightBC2D = rightBound.GetComponent<BoxCollider2D>();
        BoxCollider2D topBC2D = topBound.GetComponent<BoxCollider2D>();
        BoxCollider2D botBC2D = botBound.GetComponent<BoxCollider2D>();

        //Задаем размеры коллайдеров и местоположение относительно геймобджекта
        BaseSettings(leftBC2D);
        leftBound.transform.localScale = new Vector2(boardWidth, height);

        BaseSettings(rightBC2D);
        rightBound.transform.localScale = new Vector2(boardWidth, height);

        topScale = new Vector2(width, height / (1920 / pixelMarginTBBounds));
        BaseSettings(topBC2D);
        topBound.transform.localScale = topScale;

        botScale = new Vector2(width, height / (1920/ pixelMarginTBBounds));
        BaseSettings(botBC2D);
        botBound.transform.localScale = botScale;

        //Передвигаем коллайдеры в зависимости от размера камеры
        leftBound.transform.position = new Vector2(-width / 2 - boardWidth / 2 + pixelMarginLRBounds, 0);
        rightBound.transform.position = new Vector2(width / 2 + boardWidth / 2 - pixelMarginLRBounds, 0);

        topBound.transform.position = new Vector2(0, height / 2 - topScale.y/2);
        botBound.transform.position = new Vector2(0, -height / 2 + botScale.y/2);

        //настраиваем line renders
        LineRenderer topLine = topBound.GetComponent<LineRenderer>();
        topLine.widthMultiplier = width * 0.01f;
        topLine.SetPosition(0, new Vector2(-width * 0.7f, topBound.transform.position.y - topScale.y/2));
        topLine.SetPosition(1, new Vector2(width * 0.7f, topBound.transform.position.y - topScale.y/2));

        LineRenderer botLine = botBound.GetComponent<LineRenderer>();
        botLine.widthMultiplier = width * 0.01f;
        botLine.SetPosition(0, new Vector2(-width * 0.7f, botBound.transform.position.y + topScale.y/2));
        botLine.SetPosition(1, new Vector2(width * 0.7f, botBound.transform.position.y + topScale.y/2));

    }

    private void BaseSettings(BoxCollider2D bc2D)
    {
        bc2D.offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        bc2D.autoTiling = true; // включаем авто растягивание коллайдера
        bc2D.size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
    }

    public Vector2 GetTopMiddleGameZone()
    {
        return new Vector2(0, height / 2 - topScale.y);
    }

    public Vector2 GetBotMiddleGameZone()
    {
        return new Vector2(0, -height / 2 + botScale.y);
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
}
