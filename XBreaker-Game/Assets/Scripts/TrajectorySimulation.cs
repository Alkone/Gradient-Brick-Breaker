using UnityEngine;
 
/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class TrajectorySimulation
{

    // Ссылка на LineRender
    private LineRenderer sightLine;

    // Колоичество сегментов
    private int segmentCount = 5;

    // Максимальная длина сегмента
    private float segmentScale = Mathf.Infinity;

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
    public void SimulatePath(GameObject go,  Vector2 launchVector, int segmentCount)
    {
        int tempSegmentCount = segmentCount;

        Vector2[] segments = new Vector2[segmentCount];

        float circleRadius = go.GetComponent<CircleCollider2D>().radius * go.transform.localScale.x;

        // Инициализация скорости
        Vector2 segVelocity = launchVector.normalized * 500 * Time.deltaTime;

        RaycastHit2D hit;

        // Первая точка, равная начальной позиции объекта
        segments[0] = go.transform.position;

        for (int i = 1; i < segmentCount; i++)
        {
            // Check to see if we're going to hit a physics object
            hit = Physics2D.CircleCast(segments[i - 1] + segVelocity.normalized*0.005f, circleRadius, segVelocity, segmentScale, layerMask);
            if (hit)
            {

                // Если след. точка == нижней границе (BotBound), то сворачиваемся
                if(hit.collider.gameObject.layer == 13){
                    tempSegmentCount = i;
                    break;
                }
                segments[i] = hit.centroid;
                // flip the velocity to simulate a bounce
                segVelocity = Vector2.Reflect(segVelocity, hit.normal);
                /*
                 * Here you could check if the object hit by the Raycast had some property - was 
                 * sticky, would cause the ball to explode, or was another ball in the air for 
                 * instance. You could then end the simulation by setting all further points to 
                 * this last point and then breaking this for loop.
                 */
            }
        }

        sightLine.positionCount = tempSegmentCount;
        for (int i = 0; i < tempSegmentCount; i++)
        {
            Debug.Log("Segment " + segments[i]);
            sightLine.SetPosition(i, segments[i]);
        }
    }
}