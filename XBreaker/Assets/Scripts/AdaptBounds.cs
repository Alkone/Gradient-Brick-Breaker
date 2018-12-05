using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptBounds : MonoBehaviour {


    [SerializeField] private Camera camera;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject botBound;
    [SerializeField] private GameObject topBound;


    // Use this for initialization
    void Start() {
        float cellSize = GameManager.instance.GetLevelManager().GetCellSize(); //Получаем размер ячейки из LevelManager-a
        float width = camera.pixelWidth;
        float height = camera.pixelHeight;
        Vector2 worldCameraSize = camera.ScreenToWorldPoint(new Vector2(width, height));

        //Задаем размеры коллайдеров и местоположение относительно геймобджекта
        leftBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        leftBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        leftBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        leftBound.transform.localScale = new Vector2(cellSize, worldCameraSize.y * 2); // задаем размеры GameObject

        rightBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        rightBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        rightBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        rightBound.transform.localScale = new Vector2(cellSize, worldCameraSize.y * 2); // задаем размеры GameObject

        topBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        topBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        topBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        topBound.transform.localScale = new Vector2(worldCameraSize.x*2, cellSize*2); // задаем размеры GameObject

        botBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0); //положение коллайдера относительно объекта
        botBound.GetComponent<BoxCollider2D>().autoTiling = true; // включаем авто растягивание коллайдера
        botBound.GetComponent<BoxCollider2D>().size = new Vector2(1, 1); // размер коллайдера = размеру gameObject
        botBound.transform.localScale = new Vector2(worldCameraSize.x * 2, cellSize*2.3f); // задаем размеры GameObject

        //Передвигаем коллайдеры в зависимости от размера камеры
        Vector2 middleBot = new Vector2(0, -worldCameraSize.y + cellSize/2);
        Vector2 middleTop = new Vector2(0, worldCameraSize.y - cellSize);
        Vector2 middleLeft = new Vector2(-worldCameraSize.x-cellSize/2, 0);
        Vector2 middleRight = new Vector2(worldCameraSize.x + cellSize / 2, 0);

        leftBound.transform.position = middleLeft;
        rightBound.transform.position = middleRight;
        topBound.transform.position = middleTop;
        botBound.transform.position = middleBot;
    }
}
