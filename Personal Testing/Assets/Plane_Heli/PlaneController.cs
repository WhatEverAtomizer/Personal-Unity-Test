using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the plane
    public float tiltSpeed = 50f; // Speed of the tilt

    void Update()
    {
        // Handle movement with WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D
        float moveVertical = Input.GetAxis("Vertical"); // W, S

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Handle tilt with 8 and 5 keys on the numpad
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.Rotate(Vector3.right * tiltSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.Rotate(-Vector3.right * tiltSpeed * Time.deltaTime);
        }
    }
}
