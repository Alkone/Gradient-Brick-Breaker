﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {

    //HP блока
    [SerializeField] 
    private int lifeCount;
    private TextMesh hpText;

    //Ссылка на контролер
    private LevelsController levelsController;
    //Ссылка на спрайтрендер
    private SpriteRenderer spriteRenderer;
    //Массив возможных цветов
    private Color[] colors;

	// Use this for initialization
	void Start () {
        hpText = GetComponentInChildren<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelsController = GetComponentInParent<LevelsController>();
        colors = GameObject.Find("GameController").GetComponent<ColorManager>().GetColors();
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
