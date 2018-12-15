using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Генерирует массив цветов с n-шагом
public class ColorManager : MonoBehaviour {
    public Material skyboxGradient;

    public Color[] peakTopColors;  //массив промежуточных цветов
    public Color[] peakBotColors;  //массив промежуточных цветов

    public Color[] generatedTopColors;  //массив готовых цветов
    public Color[] generatedBotColors;  //массив готовых цветов
    public int stepCount = 200;    //шаг


    //генерирует цвета с шагом 1/stepCount
    public void Awake()
    {
        generatedTopColors = GenerateColors(peakTopColors);
        generatedBotColors = GenerateColors(peakBotColors);
    }

    private Color[] GenerateColors(Color[] colors)
    {
        Color[] result = new Color[stepCount * colors.Length];
        int index = 0;
        float step = 1f / stepCount;
        for (int colorNumber = 1; colorNumber < colors.Length; colorNumber++)
        {
            float tempStep = step;
            for (int i = 0; i < stepCount; i++)
            {
                result[index] = Color.Lerp(colors[colorNumber - 1], colors[colorNumber], tempStep);
                result[index].a = 1;
                index++;
                tempStep += step;
            }
        }
        return result;
    }

    public void SetGradientColor(int level)
    {
        skyboxGradient.SetColor("_Color2", generatedTopColors[level]);
        skyboxGradient.SetColor("_Color1", generatedBotColors[level]);
    }
}
