using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Генерирует массив цветов с n-шагом
public class ColorManager : MonoBehaviour {

    //массив цветов
    public Color[] mainColors;

    //шаг = размер массива
    public int stepCount = 200;
    //массив цветов
    private Color[] generatedColors;

    //генерирует цвета с шагом 1/stepCount и возвращает массив цветов
    public  Color[] GetColors()
    {
        if (generatedColors == null)
        {
            generatedColors = new Color[stepCount];
            float step = 1f / stepCount;
            for (int colorNumber = 1; colorNumber < mainColors.Length; colorNumber++)
            {
                float tempStep = step;
                for (int i = 0; i < stepCount; i++)
                {
                    generatedColors[i] = Color.Lerp(mainColors[colorNumber-1], mainColors[colorNumber], tempStep);
                    tempStep += step;
                    Debug.Log(tempStep.ToString());
                    Debug.Log(generatedColors[i].ToString());
                }
            }
        }
        return generatedColors;
    }

}
