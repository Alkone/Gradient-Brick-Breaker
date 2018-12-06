using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptBounds : MonoBehaviour {

    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject botBound;
    [SerializeField] private GameObject topBound;
    public float boardWidth;

    public Vector2 m_TopMiddleGameZone;
    public Vector2 m_BotMiddleGameZone;

    private void Awake()
    {
        Camera.main.orthographicSize = Screen.height / 2;
    }

    // Use this for initialization
    void Start() {

        float calculatedCellSize = GameManager.instance.GetLevelManager().GetCellSize(); //Получаем размер ячейки из LevelManager-a
        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;
        float cellDeltha = height % calculatedCellSize; // Остаток


        //Задаем размеры коллайдеров и местоположение относительно геймобджекта
        leftBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        leftBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        leftBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        leftBound.transform.localScale = new Vector2(boardWidth, height);

        rightBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        rightBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        rightBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        rightBound.transform.localScale = new Vector2(boardWidth, height);

        Vector2 topBoundSpriteSize = topBound.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 topBoundRatio = GetRatioSpriteToGlobal(topBound, width, calculatedCellSize * 2f + cellDeltha / 2);
        topBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        topBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        topBound.GetComponent<BoxCollider2D>().size = topBoundSpriteSize; //размер коллайдера = размеру спрайта
        topBound.transform.localScale = topBoundRatio; // задаем размеры GameObject

        Vector2 botBoundSpriteSize = botBound.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Vector2 botBoundRatio = GetRatioSpriteToGlobal(botBound, width, calculatedCellSize * 2f + cellDeltha / 2);
        botBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        botBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        botBound.GetComponent<BoxCollider2D>().size = botBoundSpriteSize; //размер коллайдера = размеру спрайта
        botBound.transform.localScale = botBoundRatio; // задаем размеры GameObject

        //Передвигаем коллайдеры в зависимости от размера камеры
        leftBound.transform.position = new Vector2(-width / 2 - boardWidth/2 , 0);
        rightBound.transform.position = new Vector2(width / 2 + boardWidth / 2, 0);

        topBound.transform.position = new Vector2(0, height / 2 - topBoundSpriteSize.y * topBoundRatio.y/2);
        botBound.transform.position = new Vector2(0, -height / 2 + botBoundSpriteSize.y * botBoundRatio.y/2);

        m_TopMiddleGameZone = new Vector2(0, height - (topBoundSpriteSize.y * topBoundRatio.y));
        m_BotMiddleGameZone = new Vector2(0, -height + (botBoundSpriteSize.y * botBoundRatio.y));
    }

    public static Vector2 GetRatioSpriteToGlobal(GameObject gameObject, float x, float y)
    {
        float newX = x / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float newY = y / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        return new Vector2(newX, newY);
    }

    public static Vector2 СonvertGlobalToLocalScale(GameObject gameObject, Vector2 localScale)
    {
        float newX = localScale.x / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float newY = localScale.y / gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        return new Vector2(newX, newY);
    }

}
