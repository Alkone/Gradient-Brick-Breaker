﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {
    //Static
    public static Vector2 ScreenSizeInGlobalCoordinates { get; private set; }
    public static float CellSize { get; private set; }


    [SerializeField] private int m_BlocksInLine = 10;
    [SerializeField] private int currentLevel;
    [SerializeField] private int linesCount = 0;
    [SerializeField] private GameObject m_BlockPrefub1;
    [SerializeField] private GameObject m_AddBallPoint1;
    [SerializeField] GameObject parentObject; //папка куда будем складывать все объекты

    //UI
    [SerializeField] private Text textLevel;

    private Vector3 spawnPos;

    //Листы хранящие игровые объекты
    private List<GameObject> blocksList;
    private List<GameObject> addBallsList;

    //Status
    private bool permissionToGenBlockLine;



    void Awake() {
        // geting screen size in global cordinates
        ScreenSizeInGlobalCoordinates = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //Create Lists
        blocksList = new List<GameObject>();
        addBallsList = new List<GameObject>();

        //get optimal block size
        CellSize = 2 * ScreenSizeInGlobalCoordinates.x / m_BlocksInLine;
        //get block size = cellSize*cellSize
        m_BlockPrefub1.transform.localScale = new Vector3(CellSize, CellSize, 0);
        //setting start point of the blocks
        spawnPos = new Vector3(-ScreenSizeInGlobalCoordinates.x + CellSize / 2, ScreenSizeInGlobalCoordinates.y - CellSize * 2, 0);

    }

    private void Start()
    {
        permissionToGenBlockLine = false;
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
            switch ((int)Random.Range(1, 9))
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
            }
            tempSpawnPos.x += CellSize;
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
    private void CleanLevel()
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

    //Change position to 0,0
    private void ResetPosition(GameObject parent)
    {
        parent.transform.position = new Vector2(0, 0);
    }


    //Move level down on one cell size.
    private void MoveLevelDownOnOneCell(GameObject parent)
    {
        parent.transform.position = new Vector2(0, parent.transform.position.y - CellSize);
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
