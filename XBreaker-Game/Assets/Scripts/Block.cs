using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {

    [SerializeField] private int lifeCount;

    private LevelsController levelsController;
    private SpriteRenderer spriteRenderer;
    private TextMesh hpText;
    private Color myColor;

	// Use this for initialization
	void Start () {
        hpText = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelsController = GetComponentInParent<LevelsController>();
        myColor = new Color(100 + lifeCount, 100 +lifeCount, 100+lifeCount);
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
        myColor.r = 100 + lifeCount;
        myColor.g = 100 + lifeCount;
        myColor.b = 100 + lifeCount;
        spriteRenderer.material.SetColor(0,myColor);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        levelsController.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }
}
