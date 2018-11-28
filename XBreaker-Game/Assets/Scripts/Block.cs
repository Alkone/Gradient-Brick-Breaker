using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {

    //HP блока
    [SerializeField] private int lifeCount;
    private TextMesh hpText;

    //Ссылка на контролер
    private LevelManager levelManager;
    //Ссылка на спрайтрендер
    private SpriteRenderer spriteRenderer;
    //Массив возможных цветов
    private Color[] colors;
    private Animation blockAnimation;

	// Use this for initialization
	void Start () {
        hpText = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = GameManager.instance.GetLevelManager();
        colors = GameManager.instance.GetColorManager().GetBlockColors();
        SetColor();
    }
	
	// Update is called once per frame
	void Update () {
        hpText.text = lifeCount.ToString();
	}

    public void SetLifeCount(int life)
    {
        lifeCount = life;
    }

    private void SetColor(){
        spriteRenderer.material.color = colors[lifeCount];
    }

    //DON'T WORKING
    private void SetupAnimation()
    {
        AnimationCurve curve = AnimationCurve.Linear(0.0F, 1.0F, 2.0F, 0.0F);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Animation>().Play();
        if (lifeCount < 2)
        {
            Destroy();
        }
        else{
            lifeCount--;
            SetColor();
        }
    }

    public void Destroy()
    {
        StartCoroutine("DestroyThisObject");
    }

    private IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds((float)0.02);
        levelManager.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
