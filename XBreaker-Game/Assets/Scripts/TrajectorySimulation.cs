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

    //Ссылка на игровой объект
    private GameObject gameObject;

    private LayerMask layerMask;

    public TrajectorySimulation(LineRenderer sightLine, int segmentCount, GameObject gameObject)
    {
        this.sightLine = sightLine;
        this.segmentCount = segmentCount;
        this.gameObject = gameObject;
        layerMask = LayerMask.GetMask("Bound", "Block");

    }


    /// <summary>
    /// Simulate the path of a launched ball.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    public void SimulatePath(Vector2 launchVector)
    {
        Vector2[] segments = new Vector2[segmentCount];

        float circleRadius = gameObject.GetComponent<CircleCollider2D>().radius * gameObject.transform.localScale.x;

        // Инициализация скорости
        Vector2 segVelocity = launchVector.normalized * 500 * Time.deltaTime;

        RaycastHit2D hit;

        // Первая точка, равная начальной позиции объекта
        segments[0] = gameObject.transform.position;

        for (int i = 1; i < segmentCount; i++)
        {
            // Check to see if we're going to hit a physics object
            hit = Physics2D.CircleCast(segments[i - 1] + segVelocity.normalized, circleRadius, segVelocity, segmentScale, layerMask);
            if (hit)
            {
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

        sightLine.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            Debug.Log("Segment " + segments[i]);
            sightLine.SetPosition(i, segments[i]);
        }
    }
}