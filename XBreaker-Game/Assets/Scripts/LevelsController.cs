using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour {

    public int blocksInLine = 10;
    public int level = 1;
    [SerializeField]
    private GameObject blockPrefub_1;
    [SerializeField]
    private GameObject addBallPoint_1;

    //
    private Vector2 screenSize;
    private float cellSize;
    //
    private Vector3 spawnPos;
    //Листы хранящие игровые объекты
    private List<GameObject> blocksList;
    private List<GameObject> addBallsList;
    //Вспомогательный флаг 
    bool lineCreated = true;

    void Start () {
        //Инициализация списков
        blocksList = new List<GameObject>();
        addBallsList = new List<GameObject>();

        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //делим камеру по x на n-равных частей
        cellSize = 2 * screenSize.x / blocksInLine;
        //задаем размер блоку
        blockPrefub_1.transform.localScale = new Vector3(cellSize, cellSize, 0);
        //задаём координаты первого блока
        spawnPos = new Vector3 (-screenSize.x + cellSize / 2 , screenSize.y + cellSize/2, 0);
    }


    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }

   
    public void CreateLevel(int level)
    {
        bool addPointCreated = false;
        Vector2 tempSpawnPos = spawnPos;
        for (int column = 0; column < blocksInLine; column++)
        {
            switch ((int)Random.Range(1, 9))
            {
                case 1:
                    SpawnGameObject(blockPrefub_1, tempSpawnPos, level);
                    break;
                case 2:
                    SpawnGameObject(blockPrefub_1, tempSpawnPos, level);
                    break;
                case 3:
                    if (!addPointCreated)
                    {
                        SpawnGameObject(addBallPoint_1, tempSpawnPos, level);
                        addPointCreated = true;
                    }
                    break;
                case 4:
                    if (!addPointCreated)
                    {
                        SpawnGameObject(addBallPoint_1, tempSpawnPos, level);
                        addPointCreated = true;
                    }
                    break;
                case 5:
                    if (!addPointCreated)
                    {
                        SpawnGameObject(addBallPoint_1, tempSpawnPos, level);
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
                        SpawnGameObject(blockPrefub_1, tempSpawnPos, level * 2);
                    }
                    break;
            }
            tempSpawnPos.x += cellSize;
        }
    }


    //Создает объекты и добавляет в списки
    private void SpawnGameObject(GameObject prefub, Vector2 pos, int blockLife)
    {
        GameObject go;
        go = Instantiate(prefub, pos, Quaternion.identity, gameObject.transform);
        if (prefub.GetComponent<Block>())
        {
            go.GetComponent<Block>().SetLifeCount(blockLife);
            blocksList.Add(go);
        } else if (prefub.GetComponent<AddBall>())
        {
            addBallsList.Add(go);
        }
       
    }

    //Удаляет объекты из списков
    public void RemoveGameObject(GameObject go)
    {
        if (go.GetComponent<Block>())
        {
            blocksList.Remove(go);
        }else if (go.GetComponent<AddBall>())
        {
            addBallsList.Remove(go);
        }
    }

    //Очищает уровень
    public void CleanLevel()
    {
        if (blocksList != null)
        {
            foreach (var block in blocksList)
            {
                block.GetComponent<Block>().Destroy();
            }
        }
        if (addBallsList != null)
        {
            foreach (var addBall in addBallsList)
            {
                addBall.GetComponent<AddBall>().Destroy();
            }
        }
    }

        // Update is called once per frame
        void Update () {
        if (GameController.gameStatus == GameStatus.ENDED)
        {
            lineCreated = false;
        }
        if (GameController.gameStatus==GameStatus.PREPARING && !lineCreated)
        {
            CreateLevel(level);
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - cellSize);
            level++;
            lineCreated = true;
        }
	}
}
