using UnityEngine;

public class GridLayoutManager : MonoBehaviour
{
    public int rows = 3; // Number of rows in the grid
    public int columns = 3; // Number of columns in the grid
    public float spacing = 1.0f; // Spacing between objects in the grid

    void Start()
    {
        ArrangeObjectsInGrid();
    }

    void ArrangeObjectsInGrid()
    {
        // Get all child GameObjects
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // Calculate cell size based on the number of rows, columns, and spacing
        float cellWidth = (transform.localScale.x - (columns - 1) * spacing) / columns;
        float cellHeight = (transform.localScale.y - (rows - 1) * spacing) / rows;

        // Arrange objects in a grid
        int index = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index < children.Length)
                {
                    // Calculate position for each object
                    float xPos = col * (cellWidth + spacing) - (transform.localScale.x / 2) + (cellWidth / 2);
                    float yPos = row * (cellHeight + spacing) - (transform.localScale.y / 2) + (cellHeight / 2);

                    // Set the position of the object
                    children[index].localPosition = new Vector3(xPos, yPos, 0);

                    index++;
                }
                else
                {
                    break; // Stop if there are no more objects
                }
            }
        }
    }
}
