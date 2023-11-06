using UnityEngine;

public class MoveObj : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraDistanceZ;

    // Use this for Initialization.
    void Start()
    {
        // Reference to the main camera.
        mainCamera = Camera.main;

        // Z axis of the game object for the screen view.
        cameraDistanceZ = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    // Called when the mouse is dragged over the object.
    void OnMouseDrag()
    {
        // Z axis added to screen point to maintain the same depth.
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistanceZ);

        // Screen Point converted to World Point.
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        transform.position = newPosition;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Update game logic here if needed.
    }
}
