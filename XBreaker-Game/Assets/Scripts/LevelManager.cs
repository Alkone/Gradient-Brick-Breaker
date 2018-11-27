using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    [SerializeField] private int m_BlocksInLine = 10;
    [SerializeField] private int currentLevel;
    [SerializeField] private GameObject m_BlockPrefub1;
    [SerializeField] private GameObject m_AddBallPoint1;

    private float cellSize;
    private Vector2 screenSize;
    private Vector3 spawnPos;

    //Листы хранящие игровые объекты
    private List<GameObject> blocksList;
    private List<GameObject> addBallsList;

    //Status
    private bool permissionToGenBlockLine;

    void Start () {
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
        spawnPos = new Vector3 (-screenSize.x + cellSize / 2 , screenSize.y + cellSize/2, 0);
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


    private void CreateLevel()
    {
        bool addPointCreated = false;
        Vector2 tempSpawnPos = spawnPos;
        for (int column = 0; column < m_BlocksInLine; column++)
        {
            switch ((int)Random.Range(1, 9))
            {
                case 1:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, currentLevel);
                    break;
                case 2:
                    CreateGameObject(m_BlockPrefub1, tempSpawnPos, currentLevel);
                    break;
                case 3:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, currentLevel);
                        addPointCreated = true;
                    }
                    break;
                case 4:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, currentLevel);
                        addPointCreated = true;
                    }
                    break;
                case 5:
                    if (!addPointCreated)
                    {
                        CreateGameObject(m_AddBallPoint1, tempSpawnPos, currentLevel);
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
                        CreateGameObject(m_BlockPrefub1, tempSpawnPos, currentLevel * 2);
                    }
                    break;
            }
            tempSpawnPos.x += cellSize;
        }
    }


    //Создает объекты и добавляет в списки
    private void CreateGameObject(GameObject prefub, Vector2 pos, int blockLife)
    {
        GameObject go;
        go = Instantiate(prefub, pos, Quaternion.identity, gameObject.transform);

        //if Block
        if (prefub.GetComponent<Block>())
        {
            go.GetComponent<Block>().SetLifeCount(blockLife);
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
    private void CleanLevel()
    {
        //if Block exists
        if (blocksList != null)
        {
            foreach (var block in blocksList)
            {
                block.GetComponent<Block>().Destroy();
            }
        }
        //if AddBall exists
        if (addBallsList != null)
        {
            foreach (var addBall in addBallsList)
            {
                addBall.GetComponent<AddBall>().Destroy();
            }
        }
    }

    //Move level down on one cell size.
    private void MoveLevelDownOnOneCell()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - cellSize);
    }

    private void FixedUpdate()
    {
        if (permissionToGenBlockLine)
        {
            CreateLevel();
            MoveLevelDownOnOneCell();
            permissionToGenBlockLine = false;
            currentLevel++;
        }
    }
}
