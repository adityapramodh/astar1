using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Source and Destination objects.
    public Transform seeker, target;
    
    GridCreate grid;

    private void Awake()
    {
        grid = GetComponent<GridCreate>();
    }

    // Update is called once per frame.
    private void Update()
    {
        // Find and calculate the path continuously.
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos) 
    {
        // Get the Node for the starting position.
        Node startNode = grid.NodeFromWorldPoint(startPos); // Source Node
        
        // Get the Node for the target position.
        Node targetNode = grid.NodeFromWorldPoint(targetPos); // Destination Node

        // Create open and closed sets for pathfinding.
        List<Node> openSet = new List<Node>(); // Open list
        HashSet<Node> closedSet = new HashSet<Node>(); // Closed list
        
        // Add the starting Node to the open set.
        openSet.Add(startNode);

        // Main pathfinding loop.
        while (openSet.Count > 0) 
        {
            // Find the node with the lowest F cost in the open set.
            Node currentNode = openSet[0];
            
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) 
                {
                    // Select the node with the lower F cost.
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);

            // Add the current node to the closed set.
            closedSet.Add(currentNode);

            // Check if the target node is reached.
            if (currentNode == targetNode) 
            {
                // If the target is reached, retrace the path.
                RetracePath(startNode, targetNode); 
                return;
            }

            // Get the neighboring nodes of the current node.
            foreach (Node neighbor in grid.GetNeighbors(currentNode)) 
            {
                // Skip nodes that are not walkable or are in the closed set.
                if (!neighbor.walkable || closedSet.Contains(neighbor)) 
                {
                    continue;
                }

                // Calculate the movement cost to the neighbor node.
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // If the neighbor is not in the open set or has a lower G cost, update its values.
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) 
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    // Add the neighbor to the open set.
                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        // Calculate the heuristic distance between two nodes (H value).
        int GetDistance(Node nodeA, Node nodeB) 
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); 
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            // Calculate the heuristic distance based on diagonal movement.
            if (dstX > dstY) 
            {
                return 14 * dstY + 10 * (dstX - dstY); 
            }

            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    // Retrace the path from the end node to the start node.
    void RetracePath(Node startNode, Node endNode) 
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) 
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }
}
