using UnityEngine;

public class GunRotation : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer mainRenderer;

    void Start()
    {
        mainCamera = Camera.main;
        mainRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get the mouse curser position in world space
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the gun to the mouse
        Vector3 direction = mousePos - transform.position;

        // Set the Z-axis rotation based on the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation, keeping the gun's back static
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float zRotation = gameObject.transform.eulerAngles.z;

        // Convert zRotation to a range of -180 to 180 degrees for easier comparison
        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        if (zRotation > 90 || zRotation < -90)
        {
            mainRenderer.flipY = true;
        }
        else { mainRenderer.flipY = false; }

    }
}