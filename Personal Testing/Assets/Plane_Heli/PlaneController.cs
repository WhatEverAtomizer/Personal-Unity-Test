using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximun engine thrust when at 100% throttle.")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing")]
    public float rollResponsiveness = 10f;
    public float pitchResponsiveness = 10f;
    public float yawResponsiveness = 10f;
    [SerializeField] private Rotator proppelerRotation;
    [Space(20)]
    [Header("Info")]
    [SerializeField] private float throttle;
    [SerializeField] private float roll;
    [SerializeField] private float pitch;
    [SerializeField] private float yaw;

    private bool throttleUp;
    private bool throttleDown;
    private Controls controls;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //proppelerRotation = FindAnyObjectByType<Rotator>();
        controls = new Controls();

        controls.PlaneControls.Roll.performed += ctx => roll = ctx.ReadValue<float>();
        controls.PlaneControls.Roll.canceled += ctx => roll = 0f;
        controls.PlaneControls.Pitch.performed += ctx => pitch = ctx.ReadValue<float>();
        controls.PlaneControls.Pitch.canceled += ctx => pitch = 0f;
        controls.PlaneControls.Yaw.performed += ctx => yaw = ctx.ReadValue<float>();
        controls.PlaneControls.Yaw.canceled += ctx => yaw = 0f;
        controls.PlaneControls.ThrottleUp.started += ctx => throttleUp = true;
        controls.PlaneControls.ThrottleUp.canceled += ctx => throttleUp = false;
        controls.PlaneControls.ThrottleDown.started += ctx => throttleDown = true;
        controls.PlaneControls.ThrottleDown.canceled += ctx => throttleDown = false;
    }
    private void OnEnable()
    {
        controls.PlaneControls.Enable();
    }
    private void OnDisable()
    {
        controls.PlaneControls.Disable();
    }
    /*
    private void AdjustThrottle(float value)
    {
        if (value > 0) throttle += throttleIncrement;
        else if (value < 0) throttle -= throttleIncrement;

        throttle = Mathf.Clamp(throttle, 0f, maxThrust);
    }
    */
    private void Update()
    {
        if (throttleUp)
        {
            throttle += throttleIncrement * throttleIncrement;
        }
        if (throttleDown)
        {
            throttle -= throttleIncrement * throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, maxThrust);
        proppelerRotation.speed = throttle;
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * yawResponsiveness * 200);
        rb.AddTorque(transform.right * pitch * pitchResponsiveness * 200);
        rb.AddTorque(-transform.forward * roll * rollResponsiveness * 200);
    }
}