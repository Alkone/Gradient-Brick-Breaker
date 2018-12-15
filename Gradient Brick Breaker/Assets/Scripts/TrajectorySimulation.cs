using UnityEngine;

/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class TrajectorySimulation
{

    // Ссылка на LineRender
    private LineRenderer sightLine;

    // Максимальная длина сегмента
    private float segmentScale = Mathf.Infinity;
    private float circleRadius;
    private LayerMask layerMask;

    public TrajectorySimulation(LineRenderer sightLine)
    {
        this.sightLine = sightLine;
        layerMask = LayerMask.GetMask("Bound", "Block", "BotBound");
    }


    /// <summary>
    /// Simulate the path of a launched ball.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    public void SimulatePath(GameObject go, Vector2 launchVector, float segmentCount)
    {
        int tempSegmentCount = (int)segmentCount;
        float tempSegmentRemaind = segmentCount - tempSegmentCount;
        if (tempSegmentRemaind > 0.0f) tempSegmentCount++;

        Vector2[] segments = new Vector2[tempSegmentCount];
        circleRadius = go.GetComponent<CircleCollider2D>().radius * go.transform.localScale.x;

        // Инициализация скорости
        Vector2 segVelocity = launchVector.normalized;

        RaycastHit2D hit;

        // Первая точка, равная начальной позиции объекта
        segments[0] = go.transform.position;


        GameObject tempGo = null;
        int oldLayer = 0;

        for (int i = 1; i < tempSegmentCount; i++)
        {


            // Check to see if we're going to hit a physics object
            hit = Physics2D.CircleCast(segments[i - 1], circleRadius, segVelocity, segmentScale, layerMask);
            if (hit)
            {
                //Вовзращаем исходные значения
                if (tempGo != null)
                {
                    if (tempGo.layer == 14)
                    {
                        tempGo.layer = oldLayer;
                    }
                }
                // Если след. точка == нижней границе (BotBound), то сворачиваемся
                if (hit.collider.gameObject.layer == 13)
                {
                    tempSegmentCount = i;
                    break;
                }

                if (tempSegmentRemaind > 0.0f && i == tempSegmentCount - 1)
                {
                    segments[i] = ReflectCutVector(segments[i - 1], segments[i], hit, tempSegmentRemaind);
                }
                else segments[i] = hit.centroid;
                // flip the velocity to simulate a bounce
                segVelocity = Vector2.Reflect(segVelocity, hit.normal);

                //Меняем layer последнего коснувшегося объекта
                tempGo = hit.collider.gameObject;
                oldLayer = tempGo.layer;
                tempGo.layer = 14;
                /*
                 * Here you could check if the object hit by the Raycast had some property - was 
                 * sticky, would cause the ball to explode, or was another ball in the air for 
                 * instance. You could then end the simulation by setting all further points to 
                 * this last point and then breaking this for loop.
                 */
            }
        }



        //Вовзращаем исходные значения
        if (tempGo != null)
        {
            if (tempGo.layer == 14)
            {
                tempGo.layer = oldLayer;
            }
        }


        //Конфигурирование LineRenderer
        sightLine.positionCount = tempSegmentCount;
        sightLine.widthMultiplier = circleRadius * 2;
        float magnitudeSum = 0;
        for (int i = 1; i < tempSegmentCount; i++)
        {
            magnitudeSum += (segments[i] - segments[i - 1]).magnitude;
        }
        sightLine.material.SetTextureOffset("_MainTex", new Vector2(-Time.timeSinceLevelLoad * 4f, 0f));
        sightLine.material.SetTextureScale("_MainTex", new Vector2(magnitudeSum / sightLine.widthMultiplier, 1f));

        //LineRenderer
        for (int i = 0; i < tempSegmentCount; i++)
        {
            sightLine.SetPosition(i, segments[i]);
        }
    }

    public Vector2 ReflectCutVector(Vector2 startPos, Vector2 endPos, RaycastHit2D hit, float tempSegmentRemaind)
    {
        Vector2 vector = hit.centroid - startPos;
        endPos = Vector2.Reflect(hit.centroid, hit.normal);
        Vector2 vectorNormalize = vector.normalized * tempSegmentRemaind * circleRadius * 10;
        if (vectorNormalize.magnitude > vector.magnitude) return startPos + vector;
        else return startPos + vectorNormalize;
    }
}