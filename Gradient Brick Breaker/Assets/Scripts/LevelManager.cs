using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_BlocksInLine = 10;
    public int currentLevel;
    public int gameObjectsCount;
    private int checkPoint;
    public int checkPointBallsCount;

    public float pixelMarginLRBounds;
    public float pixelMarginTBBounds;

    public GameObject m_BlockPrefub;
    public GameObject m_AddBallPointPrefub;
    public GameObject m_CoinPrefub;
    public GameObject parentObject; //папка куда будем складывать все объекты

    //UI
    public Text textLevel;


    private float cellPixelSize;
    private float cellLocalSize;
    private Vector3 spawnPos;

    //Листы хранящие игровые объекты
    private List<GameObject> gameObjects;

    //Status
    private bool permissionToGenBlockLine;

    void Awake()
    {
        //get optimal block size
        cellPixelSize = ((Screen.width - pixelMarginLRBounds * 2) + 2) / m_BlocksInLine;
        cellLocalSize = (cellPixelSize - 2) / m_BlockPrefub.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        //setting start point of the blocks

        float delta = (Screen.height - (Screen.height / (1920 / pixelMarginTBBounds) * 2)) % cellPixelSize;
        if (delta > cellPixelSize / 2)
        {
            spawnPos = new Vector3(-Screen.width / 2f + cellPixelSize / 2f + pixelMarginLRBounds, Screen.height / 2 - (Screen.height / (1920 / pixelMarginTBBounds)) - delta + cellPixelSize * 0.4f, 0f);
        }
        else
        {
            spawnPos = new Vector3(-Screen.width / 2f + cellPixelSize / 2f + pixelMarginLRBounds, Screen.height / 2 - (Screen.height / (1920 / pixelMarginTBBounds)) - delta - cellPixelSize * 0.6f, 0f);
        }
    }

    public void Start()
    {


        //Create Lists
        gameObjects = new List<GameObject>();
        checkPointBallsCount = 1;
        checkPoint = currentLevel;
        m_BlockPrefub.transform.localScale = new Vector3(cellLocalSize, cellLocalSize, 0);
        m_AddBallPointPrefub.transform.localScale = new Vector3(cellLocalSize / 2, cellLocalSize / 2, 0);
        m_CoinPrefub.transform.localScale = new Vector3(cellLocalSize / 2, cellLocalSize / 2, 0);
        permissionToGenBlockLine = false;
    }

    public float GetCellPixelSize()
    {
        return cellPixelSize;
    }

    public float GetCellLocalSize()
    {
        return cellLocalSize;
    }

    //Clean and start create new levels
    public void SetupNewScene(int startLevel)
    {
        CleanLevel();
        currentLevel = startLevel - 1;
    }

    public void SetupCheckPointScene()
    {
        CleanLevel();
        currentLevel = checkPoint - 1;
    }

    public void SetupContinueScene()
    {
        CleanLevel();
        currentLevel--;
    }

    private void OptimazeScene()
    {
        parentObject.transform.position = Vector2.zero;
    }

    public void CheckPointCheck()
    {
        if (currentLevel > 0 && gameObjects.Count == 0)
        {
            GameObject.Find("Text_CheckPoint").GetComponent<Animation>().Play();
            OptimazeScene();
            checkPoint = currentLevel;
            checkPointBallsCount = GameManager.instance.ballObjectsList.Count;
        }
    }

    //Start
    public void GenerateNextBlockLine()
    {
        permissionToGenBlockLine = true;
    }

    //Stop
    public void StopGenerateNextBlockLine()
    {
        permissionToGenBlockLine = false;
    }


    private void CreateLevel()
    {
        float randValue = 0;
        float defaultDoubleBlockSpawnChance = 0;
        float defaultBlockSpawnChance = 0;
        float defaultAddBallSpawnChance = 0;
        float defaultCoinSpawnChance = 0;

        if (currentLevel < 50)
        {
            defaultDoubleBlockSpawnChance = .07f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .20f;
        }
        else if (currentLevel < 100)
        {
            defaultDoubleBlockSpawnChance = .03f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .30f;
        }
        else if (currentLevel < 200)
        {
            defaultDoubleBlockSpawnChance = .04f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .40f;
        }
        else if (currentLevel < 300)
        {
            defaultDoubleBlockSpawnChance = .05f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .40f;
        }
        else if (currentLevel < 400)
        {
            defaultDoubleBlockSpawnChance = .06f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .50f;
        }
        else if (currentLevel < 500)
        {
            defaultDoubleBlockSpawnChance = .07f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .50f;
        }
        else if (currentLevel < 600)
        {
            defaultDoubleBlockSpawnChance = .08f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .60f;
        }
        else if (currentLevel < 700)
        {
            defaultDoubleBlockSpawnChance = .09f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .60f;
        }
        else if (currentLevel < 800)
        {
            defaultDoubleBlockSpawnChance = .1f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .70f;
        }
        else if (currentLevel < 900)
        {
            defaultDoubleBlockSpawnChance = .1f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .75f;
        }
        else if (currentLevel < 1000)
        {
            defaultDoubleBlockSpawnChance = .15f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .70f;
        }
        else
        {
            defaultDoubleBlockSpawnChance = .20f;
            defaultBlockSpawnChance = defaultDoubleBlockSpawnChance + .30f;
        }

            defaultAddBallSpawnChance = defaultBlockSpawnChance + .1f;
            defaultCoinSpawnChance = defaultAddBallSpawnChance + .05f;

            Vector2 tempSpawnPos = spawnPos;

            for (int column = 0; column < m_BlocksInLine; column++)
            {
                randValue = Random.value;
                if (randValue < defaultDoubleBlockSpawnChance)
                {
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, currentLevel * 2);
                }
                else if (randValue < defaultBlockSpawnChance) // 45% of the time
                {
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, currentLevel);
                }
                else if (randValue < defaultAddBallSpawnChance) // 45% of the time
                {
                    CreateGameObject(m_AddBallPointPrefub, tempSpawnPos, currentLevel);
                }
                else if (randValue < defaultCoinSpawnChance) // 45% of the time
                {
                    CreateGameObject(m_CoinPrefub, tempSpawnPos, currentLevel);
                }
                else // 10% of the time
                {

                }
                tempSpawnPos.x += cellPixelSize;
            }
        }


        
        //Создает объекты и добавляет в списки
        private void CreateGameObject(GameObject prefub, Vector2 pos, int blockLife)
        {
            if (blockLife == 0) blockLife = 1;
            GameObject go;
            go = Instantiate(prefub, pos, Quaternion.identity, parentObject.transform);

            //if Block
            if (prefub.GetComponent<Block>())
            {
                go.GetComponent<Block>().lifeCount = blockLife;
            }
            gameObjects.Add(go);
        }

        //Delete all level GameObjects
        public void RemoveGameObject(GameObject go)
        {
            gameObjects.Remove(go);
        }

        //Clean level
        private void CleanLevel()
        {
            if (gameObjects != null)
            {
                foreach (var go in gameObjects)
                {
                    go.GetComponent<Destroyable>().SelfDestroy();
                }
            }
        }


        //Move level down on one cell size.
        private void MoveLevelDownOnOneCell(GameObject parent)
        {
            parent.transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y - cellPixelSize);
        }

        private void FixedUpdate()
        {
            gameObjectsCount = gameObjects.Count;
            if (permissionToGenBlockLine)
            {
                currentLevel++;
                GameManager.instance.GetColorManager().SetGradientColor(currentLevel);
                textLevel.text = currentLevel.ToString();
                CreateLevel();
                MoveLevelDownOnOneCell(parentObject);
                permissionToGenBlockLine = false;
            }
        }
    }
