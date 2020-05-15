using UnityEngine;

/// <summary>
/// Handles the behaviour of the current active tetromino object. Checks for valid moves, if a line should be cleared, etc.
/// </summary>
public class TetrishBlock : MonoBehaviour
{
    public static int height = 20;
    public static int width = 10;

    public Vector3 rotationPoint;

    private float previousTime;
    public float fallTime = 0.8f;
    
    private static Transform[,] grid = new Transform[width, height];

    private AudioSource aSrc;
    public AudioClip rotate, clearLine, GameOver, blockLand;
	
    void Start()
    {
        aSrc = GetComponent<AudioSource>();
    }

	/// <summary>
    /// Handles input for the tetromino currently in play. 
    /// </summary>
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0 , 0);
            }
        }	
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
            aSrc.clip = rotate;
            aSrc.Play();
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();

                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().NewTetromino();
            }
            previousTime = Time.time;
        }
    }

    /// <summary>
    /// Check to see if there are any completed lines.
    /// </summary>
    void CheckForLines()
    {
        for (int i = height-1; i >=0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    /// <summary>
    /// Helper function to check for completed lines.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Deletes the row of blocks if player gets 10 in a row.
    /// </summary>
    /// <param name="i"></param>
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
        aSrc.clip = clearLine;
        aSrc.Play();
    }

    /// <summary>
    /// Move the rows down when player clears lines.
    /// </summary>
    /// <param name="i"></param>
    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    /// <summary>
    /// Adds all the cubes in every tetromino to the 2D grid so we can check the position of each one.
    /// </summary>
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children; 
        }
    }

    /// <summary>
    /// Checks to see if the move is valid. If move is not valid we reverse the move.
    /// </summary>
    /// <returns>True if move is valid, flase if not.</returns>
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
           
            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }
}