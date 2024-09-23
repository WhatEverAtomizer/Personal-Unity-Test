using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AntiRoll : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider WheelL;
    public WheelCollider WheelR;

    [Header("Anti-Roll Settings")]
    public float antiRoll = 5000.0f;

    public Rigidbody rb;

    void Start()
    {

        // Ensure both wheel colliders are assigned
        if (WheelL == null || WheelR == null)
        {
            Debug.LogError("WheelL and WheelR must be assigned in the inspector.");
        }
    }

    void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        // Check if the left wheel is grounded and calculate its suspension travel
        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
        {
            Vector3 wheelLocalPos = WheelL.transform.InverseTransformPoint(hit.point);
            travelL = (-wheelLocalPos.y - WheelL.radius) / WheelL.suspensionDistance;
        }

        // Check if the right wheel is grounded and calculate its suspension travel
        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            Vector3 wheelLocalPos = WheelR.transform.InverseTransformPoint(hit.point);
            travelR = (-wheelLocalPos.y - WheelR.radius) / WheelR.suspensionDistance;
        }

        // Calculate the anti-roll force based on suspension travel difference
        float antiRollForce = (travelL - travelR) * antiRoll;

        // Apply the anti-roll force to the left wheel if grounded
        if (groundedL)
        {
            rb.AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position);
        }

        // Apply the anti-roll force to the right wheel if grounded
        if (groundedR)
        {
            rb.AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);
        }
    }
}
