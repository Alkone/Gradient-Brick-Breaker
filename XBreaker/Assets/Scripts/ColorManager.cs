using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Генерирует массив цветов с n-шагом
public class ColorManager : MonoBehaviour {

    public Color[] peakColors;  //массив промежуточных цветов
    public Color[] generatedColors;  //массив готовых цветов
    public int stepCount = 200;    //шаг

    //генерирует цвета с шагом 1/stepCount
    public void Awake()
    {
        if (generatedColors == null)
        {
            generatedColors = new Color[stepCount * peakColors.Length];
            int index = 0;
            float step = 1f / stepCount;
            for (int colorNumber = 1; colorNumber < peakColors.Length; colorNumber++)
            {
                float tempStep = step;
                for (int i = 0; i < stepCount; i++)
                {
                    generatedColors[index] = Color.Lerp(peakColors[colorNumber - 1], peakColors[colorNumber], tempStep);
                    generatedColors[index].a = 1;
                    index++;
                    tempStep += step;
                }
            }
        }
    }
}
