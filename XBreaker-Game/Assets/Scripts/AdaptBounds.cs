using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptBounds : MonoBehaviour {


    [SerializeField] private Camera camera;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject botBound;
    [SerializeField] private GameObject topBound;

    [SerializeField] private float collider2dWidth = 1;
    

	// Use this for initialization
	void Start () {
        float width = camera.pixelWidth;
        float height = camera.pixelHeight;
        Vector2 worldCameraSize = camera.ScreenToWorldPoint(new Vector2(width, height));

        //Задаем размеры коллайдеров и местоположение относительно геймобджекта
        leftBound.GetComponent<BoxCollider2D>().offset = new Vector2(-leftBound.transform.localScale.x / 2, 0);
        leftBound.GetComponent<BoxCollider2D>().size = new Vector2(collider2dWidth, worldCameraSize.y*2);

        rightBound.GetComponent<BoxCollider2D>().offset = new Vector2(leftBound.transform.localScale.x / 2, 0);
        rightBound.GetComponent<BoxCollider2D>().size = new Vector2(collider2dWidth, worldCameraSize.y * 2);

        topBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, leftBound.transform.localScale.x / 2);
        topBound.GetComponent<BoxCollider2D>().size = new Vector2(worldCameraSize.x * 2, collider2dWidth);

        botBound.GetComponent<BoxCollider2D>().offset = new Vector2(0, -leftBound.transform.localScale.x / 2);
        botBound.GetComponent<BoxCollider2D>().size = new Vector2(worldCameraSize.x * 2, collider2dWidth);

        //Передвигаем коллайдеры в зависимости от размера камеры
        Vector2 middleBot = camera.ScreenToWorldPoint(new Vector2(width / 2, 0));
        Vector2 middleTop = camera.ScreenToWorldPoint(new Vector2(width / 2, height));
        Vector2 middleLeft = camera.ScreenToWorldPoint(new Vector2(0, height/2));
        Vector2 middleRight = camera.ScreenToWorldPoint(new Vector2(width, height/2));

        leftBound.transform.position = middleLeft;
        rightBound.transform.position = middleRight;
        topBound.transform.position = middleTop;
        botBound.transform.position = middleBot;
    }
}
