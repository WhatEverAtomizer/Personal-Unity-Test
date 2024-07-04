using UnityEngine;
using UnityEngine.InputSystem;

public class HeliController : MonoBehaviour
{
    [SerializeField] private float responsiveness = 500f;
    [SerializeField] private float throttleAmt = 25f;
    private float throttle;

    private Rigidbody rigidBody;
    private float roll;
    private float pitch;
    private float yaw;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleInputes();
    }
    private void FixedUpdate()
    {
        rigidBody.AddForce(transform.up * throttle, ForceMode.Impulse);
        rigidBody.AddTorque(transform.right * pitch * responsiveness);
        rigidBody.AddTorque(transform.forward * roll * responsiveness);
        rigidBody.AddTorque(transform.up * yaw * responsiveness);
    }
    private void HandleInputes()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if(Input.GetKey(KeyCode.Space))
        {
            throttle += Time.deltaTime + throttleAmt;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle -= Time.deltaTime + throttleAmt;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }
}