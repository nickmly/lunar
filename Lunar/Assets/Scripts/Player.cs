using UnityEngine;
using System.Collections;


/// <summary>
/// Main controller for the player's ship
/// </summary>  
public class Player : MonoBehaviour
{

    //Keys
    const KeyCode LEFT_KEY = KeyCode.LeftArrow;
    const KeyCode RIGHT_KEY = KeyCode.RightArrow;
    const KeyCode UP_KEY = KeyCode.UpArrow;
    const KeyCode DOWN_KEY = KeyCode.DownArrow;
    const KeyCode LAND_KEY = KeyCode.Space;

    //Components
    private Rigidbody rb;
    [SerializeField]
    private ParticleSystem fireUp, fireLeft, fireRight;

    //Constant Variables
    private const float MAX_FORCE = 10.0f;
    private const float MAX_TORQUE = 1.0f;
    private const float MAX_ANGLE = 25.0f;
    private const float RAYCAST_LANDING_RANGE = 1.2f;

    //Variables
    private Vector2 currentForce = Vector2.zero;
    private Vector3 currentTorque = Vector3.zero;
    private float pushRate = 0.05f;
    private float turnRate = 0.085f;
    private float boostRate = 0.85f;
    private float currentRotation = 0.0f;
    private bool isMovingUpwards = false;
    private bool isRotating = false;
    private bool landed = false;

    //Lerping
    private float timeTakenToLerp,timeStartedLerping;
    private Quaternion lerpStartRot, lerpEndRot;
    private bool isLerping = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 2.0f;
        rb.maxDepenetrationVelocity = 10.0f;
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleParticles();
    }

    void FixedUpdate()
    {
        if (isLerping)
            Lerp();
    }

    void StartLerping(Quaternion endRot, float lerpTime)
    {
        timeTakenToLerp = lerpTime;
        lerpStartRot = transform.rotation;
        lerpEndRot = endRot;

        timeStartedLerping = Time.time;
        isLerping = true;
    }


    void EndLerp()
    {
        rb.freezeRotation = true;
    }

    void Lerp()
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / timeTakenToLerp;
        transform.rotation = Quaternion.Lerp(lerpStartRot, lerpEndRot, percentageComplete);
        if (percentageComplete >= 1.0f)
        {
            isLerping = false;
            EndLerp();
        }
    }

    void HandleParticles()
    {
        if (currentTorque.z > 0 && !fireRight.isPlaying)
        {
            fireRight.Play();
        }
        if (currentTorque.z < 0 && !fireLeft.isPlaying)
        {
            fireLeft.Play();
        }
        if (currentForce.y > 0 && !fireUp.isPlaying)
        {
            fireUp.Play();
        }

        if (!isMovingUpwards)
        {
            if (fireUp.isPlaying)
            {
                fireUp.Stop();
            }
        }
        if (!isRotating)
        {
            if (fireRight.isPlaying || fireLeft.isPlaying)
            {
                fireRight.Stop();
                fireLeft.Stop();
            }
        }

    }

    void HandleMovement()
    {
        if (currentForce != Vector2.zero)
        {
            rb.AddRelativeForce(currentForce, ForceMode.Acceleration);
        }
        if (currentTorque != Vector3.zero)
        {
            rb.AddTorque(currentTorque, ForceMode.Force);
        }
        if (rb.velocity.magnitude > MAX_FORCE)
        {
            rb.AddForce(-rb.velocity);
        }
    }

    void HandleInput()
    {
        if (Input.GetKey(LEFT_KEY))
        {
            currentTorque.z += turnRate;
            //currentForce.x -= pushRate;
            isRotating = true;
        }
        if (Input.GetKey(RIGHT_KEY))
        {
            currentTorque.z -= turnRate;
            // currentForce.x += pushRate;
            isRotating = true;
        }


        if (Input.GetKeyUp(LEFT_KEY) || Input.GetKeyUp(RIGHT_KEY))
        {
            currentTorque.z = 0;
            currentForce.x = 0;
            isRotating = false;
        }


        if (Input.GetKey(UP_KEY))
        {
            currentForce.y += boostRate;
            isMovingUpwards = true;
        }

        if (Input.GetKeyUp(UP_KEY))
        {
            currentForce.y = 0;
            isMovingUpwards = false;
        }

        currentForce.x = Mathf.Clamp(currentForce.x, -MAX_FORCE, MAX_FORCE);
        currentForce.y = Mathf.Clamp(currentForce.y, -MAX_FORCE, MAX_FORCE);
        currentTorque.z = Mathf.Clamp(currentTorque.z, -MAX_TORQUE, MAX_TORQUE);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * RAYCAST_LANDING_RANGE);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, RAYCAST_LANDING_RANGE, 1 << 9);
    }

    void ResetConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    public void Land(Vector3 landPosition)
    {
        //transform.position = landPosition; //optional, but it looks weird
        SetLanded(true);
    }

    public void SetLanded(bool value)
    {
        landed = value;
        if (landed)
        {
            rb.velocity = Vector3.zero;
            StartLerping(Quaternion.identity, 0.5f);
            //transform.rotation = Quaternion.identity;
           // rb.freezeRotation = true;
        }
        else
        {
            ResetConstraints();
        }
    }

    public bool HasLanded()
    {
        return landed;
    }
}
