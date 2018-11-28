﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class GameManager : MonoBehaviour
{    
    //Singleton
    public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script.
    //
    private AdaptBounds adaptBounds;
    private GameStatus gameStatus; //Store a reference to our GameStatus which control level.
    private LevelManager levelManager; //Store a reference to our LevelManager which control level.
    private TrajectorySimulation trajectorySimulator; // Store a reference to our LevelManager which simulate gameObeject path.
    private ColorManager colorManager; //
    private LineRenderer lineRenderer; // Store a reference to our LineRenderer.
    

    //Lists
    private List<GameObject> ballObjectsList;

    ////Inspector fields
    [SerializeField] private GameObject bounds;
    [SerializeField] private GameObject ballPrefub1; 
    [SerializeField] private float ballTouchPower; 
    [SerializeField] private float ballLaunchInterval; 
    [SerializeField] private Vector2 startPosition; // Start ball pos
    [SerializeField] private int segmentCount = 3; //Кол-во предсказанных скачков
    [SerializeField] private int startLevel = 1;  //Current level number
    

    //для  WaitTouchToLunch()
    private bool mouseDownIsDetected = false;
    private Vector2 m_MouseDownPosition;
    private bool firstBallIsStoped = false;
    float angle;


    //Awake is always called before any Start functions
    void Awake()
    {
        Application.targetFrameRate = 300;
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get components
        adaptBounds = bounds.GetComponent<AdaptBounds>();
        levelManager = GetComponent<LevelManager>();
        lineRenderer = GetComponent<LineRenderer>();
        colorManager = GetComponent<ColorManager>();

        //Init objects
        trajectorySimulator = new TrajectorySimulation(lineRenderer);
        ballObjectsList = new List<GameObject>();

        gameStatus = GameStatus.PREPARING; //Instance of GameStatus.

        //Call the InitGame function to initialize the first level 
        
    }
    //
    //
    //

    void InitGame()
    {
        startPosition = new Vector2(0, -4);
        StartCoroutine(GameStateObserver());
        NewGame();
    }

    void Start()
    {
        InitGame();
    }
 
    void FixedUpdate()
    {
        if (gameStatus == GameStatus.READY)
        {
            WaitTouchToLunch();
        }
        if (gameStatus == GameStatus.LAUNCHED)
        {
            if (lineRenderer.positionCount != 0)
                lineRenderer.positionCount = 0;

            //Ускоряем время по тапу
            if (Input.GetMouseButtonDown(0) == true && !mouseDownIsDetected)
            {
                mouseDownIsDetected = true;
                Time.timeScale = 4;
            }
            if (mouseDownIsDetected)
            {

                if (Input.GetMouseButtonUp(0) == true)
                {
                    Time.timeScale = 1;
                    mouseDownIsDetected = false;
                }

            }
        }
        if (gameStatus == GameStatus.ENDED)
        {
            Time.timeScale = 1;
            levelManager.GenerateNextBlockLine();
            gameStatus = GameStatus.PREPARING;
        }
        if (gameStatus == GameStatus.PREPARING)
        {
            //Перебераем лист шариков и двигаем их в начальную позицию
            foreach (var go in ballObjectsList)
            {
                Rigidbody2D rb2d = go.GetComponent<Rigidbody2D>();
                rb2d.MovePosition(startPosition);
                Debug.Log(rb2d.GetRelativeVector(startPosition));
            }
        }
    }

    public void NewGame()
    {
        levelManager.SetupScene(startLevel);
        ballObjectsList.Clear();
        CreateBall(startPosition, ballPrefub1);
    }

    //Getters private field link
    public ColorManager GetColorManager()
    {
        return colorManager;
    }
    public LevelManager GetLevelManager()
    {
        return levelManager;
    }

    //end getters

    //Создает шарик в заданной позиции с гравитацией
    public bool CreateBall(Vector2 position, GameObject ballPrefub)
    {
        GameObject go = Instantiate<GameObject>(ballPrefub, position, Quaternion.identity);
        go.layer = 9;
        go.GetComponent<Ball>().isLaunched = true;
        ballObjectsList.Add(go);
        go.GetComponent<Rigidbody2D>().gravityScale = 2;
        return true;
    }

    //Останавливает шарик
    public void StopBall(GameObject ballObject)
    {
        ballObject.GetComponent<IThrowable>().Stop();
        if (!firstBallIsStoped)
        {
            startPosition = ballObject.transform.position;
            Debug.Log(startPosition);
            firstBallIsStoped = true;
        }
    }

    //Ждет пока игрок прикоснется к экрану и начнет игру
    private void WaitTouchToLunch()
    {
        Vector2 startVector;
        if (Input.GetMouseButtonDown(0) == true && !mouseDownIsDetected)
        {
            m_MouseDownPosition = GetCurrentGMousePos();
            mouseDownIsDetected = true;
        }
        if (mouseDownIsDetected)
        {
            angle = GetFixetAngle(Vector2.left, GetCurrentGMousePos() - m_MouseDownPosition);
            Debug.Log("Angle - " + angle);
            startVector = RotateVector(Vector2.left, 180 - angle) + startPosition;
            Debug.Log("Start vector - " + startVector);
            trajectorySimulator.SimulatePath(ballObjectsList[0], GetVectorByPoints(startPosition, startVector), segmentCount);
            if (Input.GetMouseButtonUp(0) == true)
            {
                //Запускает шарик
                firstBallIsStoped = false;
                StartCoroutine(StartBall(ballObjectsList, GetVectorByPoints(startPosition, startVector), ballLaunchInterval));
                mouseDownIsDetected = false;
            }
        }

    }

 

    //Возвращает угол между 10 и 170
    private float GetFixetAngle(Vector2 vector1, Vector2 vector2)
    {
        float angle = Vector2.Angle(vector1, vector2);
        if (angle < 10) angle = 10;
        else if (angle > 170) angle = 170;
        return angle;
    }

    //Возвращает повернутый вектор
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) + (sin * ty);
        v.y = -(sin * tx) + (cos * ty);
        return v;
    }

    //Возвращает вектор по двум точкам
    private Vector2 GetVectorByPoints(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 deltaVector = new Vector2(endPoint.x - startPoint.x, endPoint.y - startPoint.y);
        return deltaVector;
    }

    //Возвращает текущую позиция указателя в глобальных координатах
    private Vector2 GetCurrentGMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //Возвращает позицию шарика
    private Vector2 GetBallGCoord(GameObject ballObject)
    {
        return ballObject.transform.position;
    }

    //Корутин на зпуск шариков с интервалом
    public IEnumerator StartBall(List<GameObject> ballObjectsList, Vector2 startingVector, float delay)
    {
        IThrowable throwable;
        List<GameObject> currentStateObjectsList = new List<GameObject>(ballObjectsList);
 
        for (int i = 0; i < ballObjectsList.Count; i++)
        {
            throwable = currentStateObjectsList[i].GetComponent<IThrowable>();
            yield return new WaitForSeconds(delay);
            throwable.Launch(startingVector * ballTouchPower);
        }

    }

    //Корутин состояния игры
    private IEnumerator GameStateObserver()
    {
        for (; ; )
        {
            if (gameStatus == GameStatus.PREPARING)
            {
                //if (AllBallsInSomePos(startPosition))
                gameStatus = GameStatus.READY;
            }
            else if (gameStatus == GameStatus.READY)
            {
                if (!AllBallsIsStoped()) gameStatus = GameStatus.LAUNCHED;
            }
            else if (gameStatus == GameStatus.LAUNCHED)
            {
                if (AllBallsIsStoped()) gameStatus = GameStatus.ENDED;
            }
            else if (gameStatus == GameStatus.ENDED)
            {
                if (AllBallsInSomePos(startPosition)) gameStatus = GameStatus.READY;
                else gameStatus = GameStatus.PREPARING;
            }
            Debug.Log(gameStatus.ToString());
            yield return new WaitForSeconds(.1f);
        }
    }

    //Проверяем остались ли запущенные шарики
    public bool AllBallsIsStoped()
    {
        bool status = true;
        foreach (var ballObject in ballObjectsList)
        {
            if (ballObject.GetComponent<Ball>().isLaunched)
            {
                status = false;
            }
        }
        return status;
    }

    //Проверяет все ли шарики в стартовой позиции
    public bool AllBallsInSomePos(Vector2 pos)
    {
        bool status = true;
        foreach (var ballObject in ballObjectsList)
        {
            if (!pos.Equals(ballObject.transform.position))
            {
                status = false;
            }
        }
        Debug.Log(status.ToString());
        return status;
    }
}
