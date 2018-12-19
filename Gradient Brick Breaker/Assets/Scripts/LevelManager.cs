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
        cellPixelSize = ((Screen.width - pixelMarginLRBounds * 2) +2 ) / m_BlocksInLine;
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
        m_AddBallPointPrefub.transform.localScale = new Vector3(cellLocalSize/2, cellLocalSize/2, 0);
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
        currentLevel = startLevel-1;
    }

    public void SetupCheckPointScene()
    {
        CleanLevel();
        currentLevel = checkPoint-1;
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
        if (currentLevel>0 && gameObjects.Count == 0)
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


    private void CreateLevel(int blockLife)
    {
        bool addPointCreated = false;
        bool coinCreated = false;
        Vector2 tempSpawnPos = spawnPos;
        for (int column = 0; column < m_BlocksInLine; column++)
        {
            switch ((int)Random.Range(1, 20))
            {
                case 1:
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    break;
                case 2:
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    break;
                case 3:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPointPrefub, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    else
                    {
                        if (currentLevel > 150)
                        {
                            CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                        }
                    }
                    break;
                case 4:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPointPrefub, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    else
                    {
                        if (currentLevel > 50)
                        {
                            CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                        }
                    }
                    break;
                case 5:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPointPrefub, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    break;
                case 6:
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    break;
                case 7:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPointPrefub, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    break;
                case 8:
                    if (addPointCreated)
                    {
                        CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife * 2);
                    }
                    break;
                case 13:
                    if (currentLevel > 100)
                    {
                        CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    }
                    break;
                case 14:
                    if (currentLevel > 200)
                    {
                        CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    }
                    break;
                case 15:
                    if (!coinCreated)
                    {
                        CreateGameObject(m_CoinPrefub, tempSpawnPos, blockLife);
                        coinCreated = true;
                    }
                    break;
                case 16:
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    break;
                case 17:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPointPrefub, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    else
                    {
                        if (currentLevel > 400)
                        {
                            CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                        }
                    }
                    break;
                case 18:
                    if (currentLevel > 300)
                    {
                        CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    }
                    break;
                case 19:
                    CreateGameObject(m_BlockPrefub, tempSpawnPos, blockLife);
                    break;
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
            CreateLevel(currentLevel);
            MoveLevelDownOnOneCell(parentObject);
            permissionToGenBlockLine = false;
        }
    }
}
