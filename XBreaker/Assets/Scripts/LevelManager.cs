using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_BlocksInLine = 10;
    [SerializeField] private int currentLevel;

    //
    public GameObject m_BlockPrefub1;
    public GameObject m_HalfBlock0;
    public GameObject m_HalfBlock90;
    public GameObject m_HalfBlock180;
    public GameObject m_HalfBlock270;
    public GameObject m_AddBallPoint1;
    public GameObject parentObject; //папка куда будем складывать все объекты

    //UI
    public Text textLevel;

    private float cellPixelSize;
    private float cellLocalSize;
    private Vector2 screenSize;
    private Vector3 spawnPos;

    //Листы хранящие игровые объекты
    private List<GameObject> blocksList;
    private List<GameObject> addBallsList;

    //Status
    private bool permissionToGenBlockLine;

    void Awake()
    {
        // geting screen size in global cordinates
        screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log("screenSize = " + screenSize);

        //get optimal block size
        Debug.Log("screenSize.x / m_BlocksInLine = " + screenSize.x + " / " + m_BlocksInLine);
        cellPixelSize = screenSize.x / m_BlocksInLine;
        Debug.Log("cellPixelSize - " + cellPixelSize);
        Debug.Log("m_BlockPrefub1.GetComponent<SpriteRenderer>().sprite.bounds.size.x - " + m_BlockPrefub1.GetComponent<SpriteRenderer>().sprite.bounds.size.x);
        cellLocalSize = cellPixelSize / m_BlockPrefub1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        Debug.Log("cellLocalSize - " + cellLocalSize);

        //setting start point of the blocks
        spawnPos = new Vector3(-screenSize.x / 2f + cellPixelSize / 2f, screenSize.y / 2f - cellPixelSize * 2.5f, 0f);
        Debug.Log("spawnPos -  " + spawnPos);
    }

    public void Start()
    {
        //Create Lists
        blocksList = new List<GameObject>();
        addBallsList = new List<GameObject>();
        m_BlockPrefub1.transform.localScale = new Vector3(cellLocalSize, cellLocalSize, 0);
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
        GenerateNextBlockLine();
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
            blocksList.Add(go);
            //if AddBall
        }
        else if (prefub.GetComponent<AddBall>())
        {
            addBallsList.Add(go);
        }
    }

    //Delete all level GameObjects
    public void RemoveGameObject(GameObject go)
    {

        //if Block
        if (go.GetComponent<Block>())
        {
            blocksList.Remove(go);
        }
        else if (go.GetComponent<AddBall>())

        //if AddBall
        {
            addBallsList.Remove(go);
        }
    }

    //Clean level
    public void CleanLevel()
    {
        //if Block exists
        if (blocksList != null)
        {
            foreach (var block in blocksList)
            {
                block.GetComponent<Block>().SelfDestroy();
            }
        }
        //if AddBall exists
        if (addBallsList != null)
        {
            foreach (var addBall in addBallsList)
            {
                addBall.GetComponent<AddBall>().DestroyOnly();
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
            GameManager.instance.GetBoundManager().SetBoundsColor(GameManager.instance.GetColorManager().generatedColors[currentLevel]);
            textLevel.text = currentLevel.ToString();
            CreateLevel(currentLevel);
            MoveLevelDownOnOneCell(parentObject);
            permissionToGenBlockLine = false;
        }
    }
}
