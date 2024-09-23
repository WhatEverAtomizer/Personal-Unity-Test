using UnityEngine;
using System.Collections;

public class CameraNoClip : MonoBehaviour
{

    [Header("Camera Properties")]
    public float DistanceAway;                     //how far the camera is from the player.

    public float minDistance;                //min camera distance
    public float maxDistance;                //max camera distance

    public float DistanceUp;                    //how high the camera is above the player
    public float smooth;                    //how smooth the camera moves into place
    public float rotateAround = 70f;             //the angle at which you will rotate the camera (on an axis)

    public float speed;
    public Vector3 offSet;

    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision

    [Header("Map coordinate script")]
    //    public worldVectorMap wvm;
    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    public float camRotateSpeed = 180f;
    public Vector3 camPosition;
    public Vector3 camMask;
    public Vector3 followMask;

    //private float HorizontalAxis;
    private float VerticalAxis;
    private Rigidbody playerRB;

    // Use this for initialization
    void Start()
    {
        playerRB = target.GetComponent<Rigidbody>();

        //the statement below automatically positions the camera behind the target.
        rotateAround = target.eulerAngles.y - 45f;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {

        //HorizontalAxis = 0f; Input.GetAxis("Horizontal");
        VerticalAxis = 0f; //Input.GetAxis("Vertical");

        //Offset of the targets transform (Since the pivot point is usually at the feet).
        Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + 2f), target.position.z);
        Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;

        //this determines where both the camera and it's mask will be.
        //the camMask is for forcing the camera to push away from walls.
        camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
        camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;

        Vector3 playerForward = (playerRB.velocity + transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            target.position + target.transform.TransformVector(offSet)
            + playerForward * (-5f),
            speed * Time.deltaTime);

        occludeRay(ref targetOffset);
        smoothCamMethod();
        RearViewCam();

        transform.LookAt(target);

        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camPosition, Time.deltaTime * smooth);
       
        #region wrap the cam orbit rotation
        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround = (rotateAround + 360f);
        }
        #endregion


        //rotateAround += 1 * camRotateSpeed * Time.deltaTime;

        rotateAround = target.eulerAngles.y - 35f;

        DistanceUp = Mathf.Clamp(DistanceUp += VerticalAxis, -0.79f, 2.3f);

        //Old code making it not possible for a rear view cam
        //DistanceAway = Mathf.Clamp(DistanceAway += VerticalAxis, minDistance, maxDistance);

    }
    void smoothCamMethod()
    {
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
    }
    void occludeRay(ref Vector3 targetFollow)
    {
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            smooth = 10f;
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.

            camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
        #endregion
    }
    void RearViewCam()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            DistanceAway = -2f;
            DistanceUp = 0f;
        }
        else
        {
            DistanceAway = 2.5f;
            DistanceUp = -1f;
        }
    }
}