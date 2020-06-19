using UnityEngine;

/// <summary>
/// Creates a layer of haze for the starfields.
/// </summary>
public class HazeLayer : MonoBehaviour
{
    public float ParallaxFactor = 1f;

    Transform haze;
    Vector3 theDimension;

    Vector3 theStartPosition;

    /// <summary>
    /// Get camera, start position of the texture and the screen bounds.
    /// </summary>
    void Start()
    {
        haze = gameObject.transform; ;
        theStartPosition = transform.position;

        theDimension = GetComponent<Renderer>().bounds.size;
    }

    /// <summary>
    /// Move the texture up the screen over time.
    /// </summary>
    void Update()
    {
        Vector3 newPos = haze.position * ParallaxFactor; // Calculate the position of the object
        //newPos.x += theStartPosition.x;
        newPos.y += theStartPosition.y;
        //newPos.z = 20;
        transform.position = newPos;

        EndlessRepeater();
    }

    /// <summary>
    /// Repeat the scrolling of the textures by moving the texture that just left the camera view back to the start etc.
    /// </summary>
    void EndlessRepeater()
    {
        if (haze.position.y > (transform.position.y + theDimension.y))
        {
            theStartPosition.y += theDimension.y + theDimension.y;
        }
    }
}