using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Инициализирует состояние игры
    public static GameStatus gameStatus = GameStatus.PREPARING;

    //Позиция создания первого шарика
    public static Vector2 startPosition;

    [SerializeField] LevelsController mainLevelsController;
    [SerializeField] private GameObject ballPrefub1;
    [SerializeField] private float ballTouchPower;
    [SerializeField] private float ballLaunchInterval;


    //список существующих шариков
    private static List<GameObject> ballObjectsList;
    private LineRenderer lineRenderer;

    private bool mouseDownIsDetected = false;
    private Vector2 m_MouseDownPosition;


    private static bool firstBallIsStoped = false;

    float angle;

    void Start()
    {
        startPosition = new Vector2(0, -4);
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        ballObjectsList = new List<GameObject>();
        //Запускаем корутин на отслеживание состояния игры
        StartCoroutine(GameStateObserver());
        NewGame();
    }

    private void Update()
    {

    }

    //
    void FixedUpdate()
    {
        if (gameStatus == GameStatus.READY)
        {
            lineRenderer.positionCount = 2;
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
                Time.timeScale = 2;
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
        mainLevelsController.CleanLevel();
        ballObjectsList.Clear();
        CreateBall(startPosition, ballPrefub1);
    }

    //Создает шарик в заданной позиции с гравитацией
    public static bool CreateBall(Vector2 position, GameObject ballPrefub)
    {
        GameObject go = Instantiate<GameObject>(ballPrefub, position, Quaternion.identity);
        go.layer = 9;
        go.GetComponent<Ball>().isLaunched = true;
        ballObjectsList.Add(go);
        go.GetComponent<Rigidbody2D>().gravityScale = 2;
        return true;
    }

    //Останавливает шарик
    public static void StopBall(GameObject ballObject)
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
    public void WaitTouchToLunch()
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
            startVector = RotateVector(Vector2.left, 180-angle) + startPosition;
            Debug.Log("Start vector - " + startVector);
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, startVector);
            if (Input.GetMouseButtonUp(0) == true)
            {
                //Запускает шарик
                firstBallIsStoped = false;
                StartCoroutine(StartBall(ballObjectsList, GetVectorByPoints(startPosition, startVector), ballLaunchInterval));
                mouseDownIsDetected = false;
            }
        }

    }

        //Возвращает угол между 15 и 165
        private float GetFixetAngle(Vector2 vector1, Vector2 vector2)
    {
        float angle = Vector2.Angle(vector1, vector2);
        if (angle < 15) angle = 15;
        else if (angle > 165) angle = 165;
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
        List<GameObject> currentStateObjectsList = new List<GameObject>(ballObjectsList);
        IThrowable throwable = currentStateObjectsList[0].GetComponent<IThrowable>();

        throwable.Launch(startingVector * ballTouchPower);

        for (int i = 1; i < ballObjectsList.Count; i++)
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
