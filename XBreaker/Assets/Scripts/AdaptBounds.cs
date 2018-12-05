using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptBounds : MonoBehaviour {

    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject botBound;
    [SerializeField] private GameObject topBound;

    public Vector2 m_TopMiddleGameZone;
    public Vector2 m_BotMiddleGameZone;

    // Use this for initialization
    void Start() {

        float calculatedCellSize = GameManager.instance.GetLevelManager().GetCellSize(); //Получаем размер ячейки из LevelManager-a
        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;
        float cellDeltha = height * 2 % calculatedCellSize; // Остаток
        Vector2 worldCameraSize = Camera.main.ScreenToWorldPoint(new Vector2(width, height));
        m_TopMiddleGameZone = new Vector2(0, worldCameraSize.y - (calculatedCellSize * 1.5f + cellDeltha / 2));
        m_BotMiddleGameZone = new Vector2(0, -worldCameraSize.y + (calculatedCellSize * 1.5f + cellDeltha / 2));

        //Задаем размеры коллайдеров и местоположение относительно геймобджекта
        leftBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        leftBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        leftBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        leftBound.transform.localScale = new Vector2(calculatedCellSize, worldCameraSize.y * 2); // задаем размеры GameObject

        rightBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        rightBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        rightBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        rightBound.transform.localScale = new Vector2(calculatedCellSize, worldCameraSize.y * 2); // задаем размеры GameObject

        topBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        topBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        topBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        topBound.transform.localScale = new Vector2(worldCameraSize.x*2, calculatedCellSize * 1.5f + cellDeltha/2); // задаем размеры GameObject

        botBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        botBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        botBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        botBound.transform.localScale = new Vector2(worldCameraSize.x * 2, calculatedCellSize * 1.5f + cellDeltha/2); // задаем размеры GameObject

        //Передвигаем коллайдеры в зависимости от размера камеры
        Vector2 middleBot = new Vector2(0, -worldCameraSize.y + (calculatedCellSize * 1.5f + cellDeltha / 2) / 2);
        Vector2 middleTop = new Vector2(0, worldCameraSize.y - (calculatedCellSize * 1.5f + cellDeltha / 2)/2);
        Vector2 middleLeft = new Vector2(-worldCameraSize.x- calculatedCellSize / 2, 0);
        Vector2 middleRight = new Vector2(worldCameraSize.x + calculatedCellSize / 2, 0);

        leftBound.transform.position = middleLeft;
        rightBound.transform.position = middleRight;
        topBound.transform.position = middleTop;
        botBound.transform.position = middleBot;
    }
}
