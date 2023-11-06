using System.Collections.Generic;
using UnityEngine;

public class GridCreate : MonoBehaviour
{
    // The layer mask to determine walkable areas.
    public LayerMask unwalkableMask;

    // Dimensions of the grid in world space.
    public Vector2 gridWorldSize;

    // Radius of each grid node.
    public float nodeRadius;

    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Use this for Initialization.
    private void Start()
    {
        // Calculate the diameter of each node.
        nodeDiameter = nodeRadius * 2;
        
        // Calculate the number of nodes in the X and Y directions based on the grid's world size.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Create the grid.
        CreateGrid();
    }

    // Create a 2D grid of nodes.
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        // Calculate the bottom-left corner of the grid in world space.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calculate the world position of the current node.
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                // Check if the node is walkable by using a spherecast.
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                // Create a new node with its properties.
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // Convert a world position to a grid node.
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;

        // Clamp values to ensure they are within the grid boundaries.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        // Return the grid node at the specified world position.
        return grid[x, y];
    }

    // Get neighboring nodes for a given node.
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Ensure neighbors are within the grid boundaries.
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // Add valid neighbors to the list.
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        // Return the list of neighboring nodes.
        return neighbors;
    }

    // List to store the path for visualization.
    public List<Node> path;

    // OnDrawGizmos will use a mouse position that is relative to the Scene View.
    private void OnDrawGizmos()
    {
        // Draw a wireframe cube to visualize the grid's boundaries.
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                // Set node color to blue if walkable, black if not.
                Gizmos.color = n.walkable ? Color.blue : Color.black;

                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        // Set the color of nodes in the path to white.
                        Gizmos.color = Color.white;
                    }
                }

                // Draw cubes at the node positions.
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
