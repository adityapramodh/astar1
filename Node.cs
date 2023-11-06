using UnityEngine;

// Node Model Class
public class Node 
{
    // Indicates whether the node is walkable or not.
    public bool walkable;

    // The world position of the node.
    public Vector3 worldPosition;

    // The X and Y grid coordinates of the node.
    public int gridX;
    public int gridY;

    // Reference to the parent node for pathfinding.
    public Node parent;

    // G Value (the cost of getting from the start node to this node).
    public int gCost;

    // H Value - Heuristic Value (the estimated cost from this node to the target node).
    public int hCost;

    // F Value (the sum of G and H values).
    public int fCost 
    {
        get 
        {
            return hCost + gCost; // F value = G Value + H value.
        }
    }

    // Constructor for creating a new node.
    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        // Initialize node properties.
        walkable = _walkable; // Is walkable
        worldPosition = _worldPosition; // Position in Scene World
        gridX = _gridX; // X axis 
        gridY = _gridY; // Y axis.
    }
}
