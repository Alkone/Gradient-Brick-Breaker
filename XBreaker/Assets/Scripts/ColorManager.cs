using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Генерирует массив цветов с n-шагом
public class ColorManager : MonoBehaviour {

    public Color[] mainBlockColors;  //массив промежуточных цветов
    private Color[] generatedBlockColors;  //массив готовых цветов
    public int stepCount = 200;    //шаг

    //генерирует цвета с шагом 1/stepCount и возвращает массив цветов
    public  Color[] GetBlockColors()
    {
        if (generatedBlockColors == null)
        {
            generatedBlockColors = new Color[stepCount* mainBlockColors.Length];
            int index = 0;
            float step = 1f / stepCount;
            for (int colorNumber = 1; colorNumber < mainBlockColors.Length; colorNumber++)
            {
                float tempStep = step;
                for (int i = 0; i < stepCount; i++)
                {
                    generatedBlockColors[index] = Color.Lerp(mainBlockColors[colorNumber-1], mainBlockColors[colorNumber], tempStep);
                    generatedBlockColors[index].a = 1;
                    index++;
                    tempStep += step;
                    //Debug.Log(tempStep.ToString());
                    //Debug.Log(generatedBlockColors[i].ToString());
                }
            }
        }
        return generatedBlockColors;
    }

}
