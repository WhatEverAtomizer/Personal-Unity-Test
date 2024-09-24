using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public float maxSpeed;

    private void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(speed * 20 * Time.deltaTime, Vector3.up);
    }
}
