using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

//Singleton
public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script.
    //
    public GameStatus gameStatus; //Store a reference to our GameStatus which control level.
    private LevelManager levelManager; //Store a reference to our LevelManager which control level.

    private BoundManager boundManager;
    private AdMobManager adMobManager;
    private ColorManager colorManager; //
    private LineRenderer lineRenderer; // Store a reference to our LineRenderer.
    private TrajectorySimulation trajectorySimulator; // Store a reference to our LevelManager which simulate gameObeject path.
    private IAPManager iAPManager;

    ////Inspector fields
    public GameObject loseScreen, revardedVideoButton, spendCoinsButton, ballCountLabel, coinCountTextLabel, returnAllBalsButton, pauseScreen, currentPrefab, removeAdsButton, continueWithoutAdsButton, soundButton, noSoundButton;

    public Vector2 startPosition; // Start ball pos
    [SerializeField] private float segmentCount = 2.2f; //Кол-во предсказанных скачков
    [SerializeField] private int startLevel = 1;  //Current level number
    [SerializeField] private int checkPoint;

    //Saved//Ads
    private string adsProperty;


    //для  WaitTouchToLunch()
    private bool mouseDownIsDetected = false;
    private bool firstBallIsStoped = false;
    private float angle;

    //Lists
    public List<GameObject> ballObjectsList;

    //Переменные состояния игры
    private bool gameLosed = false;
    private bool loseFlag = false; // вспомогательная переменная
    private bool gamePaused = false;
    private bool pauseFlag = false; // вспомогательная переменная
    private bool nextLevelIsCreate;
    private bool doOnce;

    //Time
    private bool timeIsSetted;
    private float levelStartTime;
    public float timeBeforLevelStarting;

    //
    Coroutine startBallsCoroutine;

    //saveded
    private int coins;
    private string prefabsAllow;


    //Sound
    public bool sound;

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
        levelManager = GetComponent<LevelManager>();
        boundManager = GetComponent<BoundManager>();
        lineRenderer = GetComponent<LineRenderer>();
        adMobManager = GetComponent<AdMobManager>();
        colorManager = GetComponent<ColorManager>();
        iAPManager = new IAPManager();

        //Init objects
        trajectorySimulator = new TrajectorySimulation(lineRenderer);
        ballObjectsList = new List<GameObject>();


    }
    //
    //Time

    //
    void Start()
    {
        // LoadGame();
        adsProperty = "ads";
        adMobManager.InitAdmob(adsProperty);
        InitGame();
    }

    void InitGame()
    {
        sound = true;
        doOnce = false;
        checkPoint = startLevel;
        timeIsSetted = false;
        nextLevelIsCreate = false;
        StartCoroutine(GameStateObserver());
        StartGame("new");
    }

    void Update()
    {

        if (gameLosed && !loseFlag)
        {
            if (adsProperty == "noads") // реклама включена
            {
                revardedVideoButton.SetActive(false);
                removeAdsButton.SetActive(false);
                continueWithoutAdsButton.SetActive(true);
            }
            else
            {
                continueWithoutAdsButton.SetActive(false);
                removeAdsButton.SetActive(true);
                revardedVideoButton.SetActive(true);

                if (adMobManager.GetRewardBasedVideoIsLoaded())
                {
                    revardedVideoButton.GetComponent<Button>().interactable = true;
                    revardedVideoButton.GetComponent<Animation>().enabled = true;
                }
                else
                {
                    revardedVideoButton.GetComponent<Button>().interactable = false;
                    revardedVideoButton.GetComponent<Animation>().enabled = false;
                }
            }

            loseScreen.SetActive(true);
            loseFlag = true;
        }
        else if (!gameLosed && loseFlag)
        {

            loseScreen.SetActive(false);
            removeAdsButton.SetActive(false);
            loseFlag = false;
        }

        if (gamePaused && !pauseFlag)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            pauseFlag = true;
        }
        else if (!gamePaused && pauseFlag)
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            pauseFlag = false;
        }
    }

    void FixedUpdate()
    {
        if (!gameLosed)
        {
            if (gameStatus == GameStatus.READY)
            {
                if (!doOnce)
                {
                    ballCountLabel.transform.position = new Vector2(startPosition.x + 70, startPosition.y + 70);
                    ballCountLabel.SetActive(true);
                    doOnce = true;
                }
                WaitTouchToLunch();
            }
            else if (gameStatus == GameStatus.LAUNCHED)
            {
                if (doOnce)
                {
                    ballCountLabel.SetActive(false);
                    doOnce = false;
                }
                if (!timeIsSetted)
                {
                    levelStartTime = Time.realtimeSinceStartup;
                    timeIsSetted = true;
                    nextLevelIsCreate = false;
                }
                timeBeforLevelStarting = Time.realtimeSinceStartup - levelStartTime;
                if (timeBeforLevelStarting > 7)
                {
                    returnAllBalsButton.SetActive(true);
                }
            }
            else if (gameStatus == GameStatus.ENDED)
            {
                if (!nextLevelIsCreate)
                {
                    levelManager.CheckPointCheck();
                    levelManager.GenerateNextBlockLine();
                    returnAllBalsButton.SetActive(false);
                    timeIsSetted = false;
                    nextLevelIsCreate = true;
                }
                gameStatus = GameStatus.PREPARING;
            }
            else if (gameStatus == GameStatus.PREPARING)
            {
                //Перебераем лист шариков и двигаем их в начальную позицию
                foreach (var go in ballObjectsList)
                {
                    go.GetComponent<Ball>().MoveToPosition(startPosition);
                }
            }
        }
    }

    public void StartGame(string type)
    {
        currentPrefab.transform.localScale = Vector2.one * levelManager.GetCellLocalSize() * 0.33f;
        switch (type)
        {
            case "new":
                coinCountTextLabel.GetComponent<Text>().text = coins.ToString();
                levelManager.SetupNewScene(startLevel); //Очищает сцену 
                DestroyAllBals(); //Уничтожает GameObject и чистит список
                boundManager.botBound.GetComponent<BotBound>().doOnce = false;
                startPosition.x = 0;
                gameLosed = false;
                CreateBall(startPosition, currentPrefab);
                gameStatus = GameStatus.LAUNCHED;
                break;

            case "continue":
                gameStatus = GameStatus.PREPARING;
                levelManager.SetupContinueScene(); //Очищает сцену 
                boundManager.botBound.GetComponent<BotBound>().doOnce = false;
                foreach (var go in ballObjectsList)
                {
                    go.GetComponent<Ball>().MoveToPosition(startPosition);
                }
                levelManager.GenerateNextBlockLine();
                gameLosed = false;
                gameStatus = GameStatus.LAUNCHED;
                break;
            case "checkpoint":
                gameStatus = GameStatus.PREPARING;
                SpendCoin(10);
                DestroyAllBals(); //Уничтожает GameObject-ы и чистит список
                levelManager.SetupCheckPointScene(); //Очищает сцену 
                for (int i = 0; i < levelManager.checkPointBallsCount; i++)
                {
                    CreateBall(startPosition, currentPrefab);
                }
                boundManager.botBound.GetComponent<BotBound>().doOnce = false;
                levelManager.GenerateNextBlockLine();
                gameLosed = false;
                gameStatus = GameStatus.LAUNCHED;
                break;
        }
    }

    public void LoseGame()
    {
        gameLosed = true;
    }

    public void PauseGame()
    {
        gamePaused = true;
    }

    public void ResumeGame()
    {
        gamePaused = false;
    }

    public void RemoveAds()
    {
        adsProperty = "noads";
        PlayerPrefs.SetString("AdsProperty", adsProperty);
        PlayerPrefs.Save();
        if (gameLosed)
        {
            loseFlag = false ;
        }
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

    public BoundManager GetBoundManager()
    {
        return boundManager;
    }


    public void AddCoin()
    {
        coins++;
        coinCountTextLabel.GetComponent<Text>().text = coins.ToString();
        if (coins >= 10)
        {
            spendCoinsButton.GetComponent<Button>().interactable = true;
        }
    }

    public void SpendCoin(int count)
    {
        coins -= count;
        coinCountTextLabel.GetComponent<Text>().text = coins.ToString();
        if (coins < 10)
        {
            spendCoinsButton.GetComponent<Button>().interactable = false;
        }
    }


    //Создает шарик в заданной позиции с гравитацией
    public bool CreateBall(Vector2 position, GameObject ballPrefub)
    {
        GameObject go = Instantiate(ballPrefub, position, Quaternion.identity);
        ballObjectsList.Add(go);
        ballCountLabel.GetComponentInChildren<Text>().text = "x " + ballObjectsList.Count.ToString();
        return true;
    }

    //Останавливает шарик
    public void SetNewStartPosition(Vector2 stopPosition)
    {
        if (!firstBallIsStoped)
        {
            startPosition = stopPosition;
            firstBallIsStoped = true;
        }
    }

    //Ждет пока игрок прикоснется к экрану и начнет игру
    private void WaitTouchToLunch()
    {
        Vector2 currentMousePos = GetCurrentGMousePos();
        if (currentMousePos.y < boundManager.GetTopMiddleGameZone().y && currentMousePos.y > boundManager.GetBotMiddleGameZone().y)
        {
            Vector2 startVector;
            if (Input.GetMouseButton(0) && !mouseDownIsDetected)
            {
                mouseDownIsDetected = true;
            }
            if (mouseDownIsDetected)
            {
                angle = GetFixetAngle(Vector2.left, currentMousePos - startPosition);
                startVector = RotateVector(Vector2.left, angle) + startPosition;
                trajectorySimulator.SimulatePath(ballObjectsList[0], GetVectorByPoints(startPosition, startVector), segmentCount);
                if (!Input.GetMouseButton(0))
                {
                    lineRenderer.positionCount = 0;
                    //Запускает шарик
                    firstBallIsStoped = false;
                    mouseDownIsDetected = false;
                    gameStatus = GameStatus.LAUNCHED;
                    startBallsCoroutine = StartCoroutine(StartBall(ballObjectsList, GetVectorByPoints(startPosition, startVector)));
                }
            }
        }
        else if (mouseDownIsDetected && !Input.GetMouseButton(0))
        {
            lineRenderer.positionCount = 0;
            mouseDownIsDetected = false;
        }
    }

    //Уничтожает все шарики и очищает лист
    private void DestroyAllBals()
    {
        foreach (var ball in ballObjectsList)
        {
            ball.GetComponent<Ball>().DestroyBall();
        }
        ballObjectsList.Clear();
    }

    //Уничтожает все шарики и очищает лист
    public void ReturnAllBalsToStartPos()
    {
        if (startBallsCoroutine != null)
        {
            StopCoroutine(startBallsCoroutine);
        }
        foreach (var ball in ballObjectsList)
        {
            ball.GetComponent<Ball>().Return(startPosition);
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
    public IEnumerator StartBall(List<GameObject> ballObjectsList, Vector2 startingVector)
    {
        Ball[] balls = new Ball[ballObjectsList.Count];
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = ballObjectsList[i].GetComponent<Ball>();
        }

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].Launch(startingVector);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

        }

    }

    //Корутин состояния игры
    private IEnumerator GameStateObserver()
    {
        for (; ; )
        {
            if (gameStatus == GameStatus.PREPARING)
            {
                if (AllBallsPrepeared()) gameStatus = GameStatus.READY;
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
                gameStatus = GameStatus.PREPARING;
            }
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
    public bool AllBallsPrepeared()
    {
        bool status = true;
        foreach (var ballObject in ballObjectsList)
        {
            if (ballObject.GetComponent<Ball>().isPrepairing)
            {
                status = false;
            }
        }
        return status;
    }

    public void SetSound(bool state)
    {
        if (state == true)
        {
            noSoundButton.SetActive(false);
            soundButton.SetActive(true);
        }
        else
        {
            soundButton.SetActive(false);
            noSoundButton.SetActive(true);
        }
        sound = state;
    }
    
    private void LoadGame()
    {
        string[] keys = { "AdsProperty", "coins", "prefabsAllow" };

        if (PlayerPrefs.HasKey("AdsProperty"))
        {
            adsProperty = PlayerPrefs.GetString("AdsProperty");
        }
        else
        {
            adsProperty = "ads";
        }


    }

    private void SaveGame()
    {
        PlayerPrefs.SetString("AdsProperty", "noads");
        PlayerPrefs.Save();
    }
}
