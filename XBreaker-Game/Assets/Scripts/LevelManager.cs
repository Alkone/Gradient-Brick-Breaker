using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    [SerializeField] private int m_BlocksInLine = 10;
    [SerializeField] private int currentLevel;
    [SerializeField] private int linesCount = 0;
    [SerializeField] private GameObject m_BlockPrefub1;
    [SerializeField] private GameObject m_HalfBlock0;
    [SerializeField] private GameObject m_HalfBlock90;
    [SerializeField] private GameObject m_HalfBlock180;
    [SerializeField] private GameObject m_HalfBlock270;
    [SerializeField] private GameObject m_AddBallPoint1;
    [SerializeField] GameObject parentObject; //папка куда будем складывать все объекты

    //UI
    [SerializeField] private Text textLevel;

    private float cellSize;
    private Vector2 screenSize;
    private Vector3 spawnPos;

    //Листы хранящие игровые объекты
    private List<GameObject> blocksList;
    private List<GameObject> addBallsList;

    //Status
    private bool permissionToGenBlockLine;

    void Awake () {
        permissionToGenBlockLine = false;

        //Create Lists
        blocksList = new List<GameObject>();
        addBallsList = new List<GameObject>();

        // geting screen size in global cordinates
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //get optimal block size
        cellSize = 2 * screenSize.x / m_BlocksInLine;
        //get block size = cellSize*cellSize
        m_BlockPrefub1.transform.localScale = new Vector3(cellSize, cellSize, 0);
        //setting start point of the blocks
        spawnPos = new Vector3 (-screenSize.x + cellSize / 2 , screenSize.y - cellSize*2, 0);
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    //Clean and start create new levels
    public void SetupScene(int startLevel)
    {
        CleanLevel();
        currentLevel = startLevel;
        GenerateNextBlockLine();
    }

    //Start
    public void GenerateNextBlockLine()
    {
        permissionToGenBlockLine = true;
    }

    //Stop
    public void StopLevelEngine()
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
                case 9:
                        CreateGameObject(m_HalfBlock0, tempSpawnPos, blockLife / 2);
                    break;
                case 10:
                        CreateGameObject(m_HalfBlock90, tempSpawnPos, blockLife / 2);
                    break;
                case 11:
                        CreateGameObject(m_HalfBlock180, tempSpawnPos, blockLife / 2);
                    break;
                case 12:
                        CreateGameObject(m_HalfBlock270, tempSpawnPos, blockLife / 2);
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
                    CreateGameObject(m_AddBallPoint1, tempSpawnPos, blockLife);
                    break;
                case 18:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
                case 19:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, blockLife);
                    break;
            }
            tempSpawnPos.x += cellSize;
        }
    }


    //Создает объекты и добавляет в списки
    private void CreateGameObject(GameObject prefub, Vector2 pos, int blockLife)
    {
        GameObject go;
        go = Instantiate(prefub, pos, Quaternion.identity, parentObject.transform);

        //if Block
        if (prefub.GetComponent<Block>())
        {
            go.GetComponent<Block>().lifeCount = blockLife;
            blocksList.Add(go);

        //if AddBall
        } else if (prefub.GetComponent<AddBall>())
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
        }else if (go.GetComponent<AddBall>())

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
        parent.transform.position = new Vector2(parent.transform.position.x, parent.transform.position.y - cellSize);
    }

    private void FixedUpdate()
    {
        if (permissionToGenBlockLine)
        {
            textLevel.text = currentLevel.ToString();
            CreateLevel(currentLevel);
            linesCount++;
            currentLevel++;
            MoveLevelDownOnOneCell(parentObject);
            permissionToGenBlockLine = false;
        }
        
    }
}
