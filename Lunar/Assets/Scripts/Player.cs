
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
    private const float RAYCAST_LANDING_RANGE = 1.05f;

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
    private float timeTakenToLerp, timeStartedLerping;
    private Quaternion lerpStartRot, lerpEndRot;
    private bool isLerping = false;

    //Mobile Vars
    bool touchUp = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 2.0f;
        rb.maxDepenetrationVelocity = 10.0f;
    }

    void Update()
    {
        if (!isLerping)
        {
            HandleInput();
        }

        HandleParticles();
    }

    void FixedUpdate()
    {
        if (isLerping)
            Lerp();
        else
            HandleMovement();
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
        if (currentTorque.z < 0 && !fireRight.isPlaying)
        {
            fireRight.Play();
        }
        if (currentTorque.z > 0 && !fireLeft.isPlaying)
        {
            fireLeft.Play();
        }
        if (currentForce.y > 0)
        {
            fireUp.Emit(1);
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
            rb.AddRelativeForce(currentForce, ForceMode.Force);
        }
        if (currentTorque != Vector3.zero)
        {
            rb.AddTorque(currentTorque, ForceMode.Force);
            //transform.Rotate(currentTorque);
        }
        if (rb.velocity.magnitude > MAX_FORCE)
        {
            rb.AddForce(-rb.velocity);
        }

    }

    void HandleInput()
    {
        #region MOBILE
        bool usingPhone = false;
        int touchCount = Input.touchCount;

        if (touchCount > 0)
        {
            usingPhone = true;
            for (int i = 0; i < touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                {
                    if (touch.position.y < Screen.height / 2)
                    {
                        touchUp = true;
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    touchUp = false;
                }
            }
        }
        else
        {
            touchUp = false;
        }

        #endregion
#if UNITY_EDITOR
        if ((Input.GetKey(LEFT_KEY) && !usingPhone))
        {
            currentTorque.z += turnRate;
            //currentForce.x -= pushRate;
            isRotating = true;
        }
        else if ((Input.GetKey(RIGHT_KEY) && !usingPhone))
        {
            currentTorque.z -= turnRate;
            // currentForce.x += pushRate;
            isRotating = true;
        }

        if (((Input.GetKeyUp(LEFT_KEY) || Input.GetKeyUp(RIGHT_KEY)) && !usingPhone))
        {
            rb.angularVelocity = Vector3.zero;
            currentTorque.z = 0;
            currentForce.x = 0;
            isRotating = false;
        }


#endif




#if UNITY_ANDROID && !UNITY_EDITOR
        currentTorque.z += turnRate * -Input.acceleration.x;
        if (Mathf.Abs(Input.acceleration.x) > 0.07f)
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
            currentTorque = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
#endif
        if ((Input.GetKey(UP_KEY) && !usingPhone) || (touchUp && usingPhone))
        {
            currentForce.y += boostRate;
            isMovingUpwards = true;
        }

        if ((Input.GetKeyUp(UP_KEY) && !usingPhone) || (!touchUp && usingPhone))
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
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
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


