using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximun engine thrust when at 100% throttle.")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing")]
    public float responsiveness = 10f;
    public float pitchResponsiveness = 10f;
    [Space(20)]
    [Header("Info")]
    [SerializeField] private float throttle;
    [SerializeField] private float roll;
    [SerializeField] private float pitch;
    [SerializeField] private float yaw;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }
    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInput()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, maxThrust);
    }
    private void Update()
    {
        HandleInput();
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * pitchResponsiveness);
        rb.AddTorque(-transform.forward * roll * responseModifier);
    }
}
