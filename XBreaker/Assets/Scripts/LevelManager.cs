using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_BlocksInLine = 10;
    [SerializeField] private int currentLevel;
    [SerializeField] private int gameObjectsCount;

    public float pixelMarginLRBounds;
    public float pixelMarginTBBounds;

    public GameObject m_BlockPrefub1;
    public GameObject m_AddBallPoint1;
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
        cellPixelSize = (Screen.width - pixelMarginLRBounds * 2) / m_BlocksInLine;
        cellLocalSize = cellPixelSize / m_BlockPrefub1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
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

        m_BlockPrefub1.transform.localScale = new Vector3(cellLocalSize, cellLocalSize, 0);
        m_AddBallPoint1.transform.localScale = new Vector3(cellLocalSize/2, cellLocalSize/2, 0);
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
    public void SetupScene(int startLevel)
    {
        CleanLevel();
        currentLevel = startLevel-1;
    }

    private void OptimazeScene()
    {

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
        Vector2 tempSpawnPos = spawnPos;
        for (int column = 0; column < m_BlocksInLine; column++)
        {
            switch ((int)Random.Range(1, 20))
            {
                case 1:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
                case 2:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
                case 3:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    break;
                case 4:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    break;
                case 5:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    break;
                case 6:
                    CreateGameObject(m_AddBallPoint1, tempSpawnPos, blockLife);
                    break;
                case 7:
                    break;
                case 8:
                    if (addPointCreated)
                    {
                        CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife * 2);
                    }
                    break;
                case 13:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
                case 14:

                    break;
                case 15:

                    break;
                case 16:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
                case 17:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, blockLife);
                        addPointCreated = true;
                    }
                    break;
                case 18:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
                case 19:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
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
    public void CleanLevel()
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
        if (permissionToGenBlockLine)
        {
            currentLevel++;
            GameManager.instance.GetColorManager().SetGradientColor(currentLevel);
            textLevel.text = currentLevel.ToString();
            CreateLevel(currentLevel);
            gameObjectsCount = gameObjects.Count;

            MoveLevelDownOnOneCell(parentObject);
            permissionToGenBlockLine = false;
        }
    }
}
