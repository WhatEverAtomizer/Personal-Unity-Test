using UnityEngine;
using UnityEngine.InputSystem;

public class HeliController : MonoBehaviour
{
    [SerializeField] public float rollResponsiveness = 10f;
    [SerializeField] public float pitchResponsiveness = 10f;
    [SerializeField] public float yawResponsiveness = 10f;

    //[SerializeField] private float responsiveness = 500f;
    [SerializeField] private float throttleAmt = 25f;
    [SerializeField] private float maxThrust = 5f;
    [SerializeField] private float rotorSpeedModifier = 10f;
    [SerializeField] private Transform rotorsTransform;
    [SerializeField] private Rotator proppelerRotation;
    [Space(20)]
    [Header("Info")]
    [SerializeField] private float throttle;
    [SerializeField] private float roll;
    [SerializeField] private float pitch;
    [SerializeField] private float yaw;

    private bool throttleUp;
    private bool throttleDown;
    private Rigidbody rigidBody;
    private Controls controls;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
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
    private void Update()
    {
        //HandleInputes();
        //rotorsTransform.Rotate(Vector3.up * (maxThrust * throttle) * rotorSpeedModifier);

        if (throttleUp)
        {
            throttle += throttleAmt * throttleAmt;
        }
        if (throttleDown)
        {
            throttle -= throttleAmt * throttleAmt;
        }
        throttle = Mathf.Clamp(throttle, 0f, maxThrust);
        proppelerRotation.speed = throttle;
    }
    private void FixedUpdate()
    {
        rigidBody.AddForce(transform.up * maxThrust * throttle);
        rigidBody.AddTorque(-transform.right * roll * rollResponsiveness * 200);
        rigidBody.AddTorque(-transform.forward * pitch * pitchResponsiveness * 200);
        rigidBody.AddTorque(transform.up * yaw * yawResponsiveness * 200);
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