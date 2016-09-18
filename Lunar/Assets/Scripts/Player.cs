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
   // private Rigidbody rb;
    [SerializeField]
    private ParticleSystem fireUp, fireLeft, fireRight;

    //Constant Variables
    private const float MAX_FORCE = 10.0f;
    private const float MAX_TORQUE = 3.0f;
    private const float MAX_ANGLE = 25.0f;
    private const float RAYCAST_LANDING_RANGE = 1.2f;

    //Variables
    private Vector2 currentForce = Vector2.zero;
    private Vector3 currentTorque = Vector3.zero;
    private float pushRate = 0.05f;
    private float turnRate = 0.3f;
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
    bool touchLeft = false;
    bool touchRight = false;
    bool touchUp = false;

	//New Movement Vars
	public float currentXPos = 0.0f;
	public float currentYPos = 0.0f;
	public float laneWidth = 2.5f;
	public float laneHeight = 5.0f;
	public float laneChangeSpeed = 0.25f;
	public bool isLaunched = false;
	public Rigidbody rb;
	public float currentSpeed = 0.0f;
	public float desiredSpeed = 0.0f;
	public float acceleration = 1.0f;
	float maxX = 5.0f;
	float maxY = 6.0f;
	float maxYD = -10.0f;
	public bool moveUp = false; 
	public bool moveDown = false;
	public float moveUpTimer = 0.0f;
	public int yDisplacement = 0;

    void Start()
    {
      rb = GetComponent<Rigidbody>();
      //  rb.maxAngularVelocity = 2.0f;
      //  rb.maxDepenetrationVelocity = 10.0f;
    }

    void Update()
    {
		Movement();
        //HandleInput();
       // HandleMovement();
       // HandleParticles();
    }

	public void UP() {
		if(yDisplacement < 4) {
			moveUp = true;
			currentYPos += laneHeight;
			yDisplacement += 1;
		}
	}

	public void DOWN() {
		if(yDisplacement > -2) {
			moveDown = true;
			currentYPos -= laneHeight;
			yDisplacement -= 1;
		}
	}

	public void RIGHT() {
		if(currentXPos < maxX) {
			currentXPos += laneWidth;
			fireRight.Play();
		}
	}

	public void LEFT() {
		if(currentXPos > -maxX) {
			currentXPos -= laneWidth;
			fireLeft.Play();
		}
	}

	public void LAUNCH() {
		isLaunched = true;
	}
		
	void Movement() {
		if(Input.GetKeyDown(KeyCode.RightArrow) && currentXPos < maxX) {
			currentXPos += laneWidth;
			fireRight.Play();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow) && currentXPos > -maxX) {
			currentXPos -= laneWidth;
			fireLeft.Play();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && yDisplacement < 4) {
			moveUp = true;
			currentYPos += laneHeight;
			yDisplacement += 1;

		}

		if(Input.GetKeyDown(KeyCode.DownArrow) && yDisplacement > -2) {
			moveDown = true;
			currentYPos -= laneHeight;
			yDisplacement -= 1;

		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			isLaunched = true;
		}
		if(isLaunched) {
			fireUp.Emit(1);
			desiredSpeed = 5.0f;
		}
			
		currentYPos += desiredSpeed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, new Vector3(currentXPos, currentYPos, transform.position.z), laneChangeSpeed);
	}
		


//    void HandleParticles()
//    {
//        if (currentTorque.z > 0 && !fireRight.isPlaying)
//        {
//            fireRight.Play();
//        }
//        if (currentTorque.z < 0 && !fireLeft.isPlaying)
//        {
//            fireLeft.Play();
//        }
//        if (currentForce.y > 0)
//        {
//            fireUp.Emit(1);
//        }
//
//
//        if (!isMovingUpwards)
//        {
//            if (fireUp.isPlaying)
//            {
//                fireUp.Stop();
//            }
//        }
//        if (!isRotating)
//        {
//            if (fireRight.isPlaying || fireLeft.isPlaying)
//            {
//                fireRight.Stop();
//                fireLeft.Stop();
//            }
//        }
//
//    }
//
//    void HandleMovement()
//    {
//        if (currentForce != Vector2.zero)
//        {
//            rb.AddRelativeForce(currentForce, ForceMode.Acceleration);
//        }
//		if (currentTorque != Vector3.zero && Mathf.Abs(transform.rotation.z) < MAX_ANGLE)
//        {
//            rb.AddTorque(currentTorque, ForceMode.Force);
//        }
//		if(transform.rotation.z*Mathf.Rad2Deg > MAX_ANGLE) {
//			rb.AddTorque(-currentTorque, ForceMode.Force);
//		}
//        if (rb.velocity.magnitude > MAX_FORCE)
//        {
//            rb.AddForce(-rb.velocity);
//        }
//		Debug.Log(transform.rotation.z*Mathf.Rad2Deg);
//
//    }

//    void HandleInput()
//    {
//        #region MOBILE
//        bool usingPhone = false;
//        int touchCount = Input.touchCount;
//        if (touchCount > 0)
//        {
//            usingPhone = true;
//            for (int i = 0; i < touchCount; i++)
//            {
//                Touch touch = Input.GetTouch(i);
//                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
//                {
//                    if (touch.position.y > Screen.height / 2)
//                    {
//                        if (touch.position.x < Screen.width / 2)
//                        {
//                            touchLeft = true;
//                        }
//                        else if (touch.position.x > Screen.width / 2)
//                        {
//                            touchRight = true;
//                        }
//                    }
//                    else if (touch.position.y < Screen.height / 2)
//                    {
//                        touchUp = true;
//                    }
//                }
//                if (touch.phase == TouchPhase.Ended)
//                {
//                    if (touch.position.x < Screen.width / 2)
//                    {
//                        touchLeft = false;
//                    }
//                    else if (touch.position.x > Screen.width / 2)
//                    {
//                        touchRight = false;
//                    }
//
//                    if (touch.position.y < Screen.height / 2)
//                    {
//                        touchUp = false;
//                    }
//                }
//            }
//        }
//
//        #endregion
//
//        if ((Input.GetKey(LEFT_KEY) && !usingPhone) || (touchLeft && usingPhone))
//        {
//            currentTorque.z += turnRate;
//            //currentForce.x -= pushRate;
//            isRotating = true;
//        }
//        else if ((Input.GetKey(RIGHT_KEY) && !usingPhone) || (touchRight && usingPhone))
//        {
//            currentTorque.z -= turnRate;
//            // currentForce.x += pushRate;
//            isRotating = true;
//        }
//
//
//        if (((Input.GetKeyUp(LEFT_KEY) || Input.GetKeyUp(RIGHT_KEY)) && !usingPhone) || ((!touchLeft && !touchRight) && usingPhone))
//        {
//            currentTorque.z = 0;
//            currentForce.x = 0;
//            isRotating = false;
//        }
//
//
//        if ((Input.GetKey(UP_KEY) && !usingPhone) || (touchUp && usingPhone))
//        {
//            currentForce.y += boostRate;
//            isMovingUpwards = true;
//        }
//
//        if ((Input.GetKeyUp(UP_KEY) && !usingPhone) || (!touchUp && usingPhone))
//        {
//            currentForce.y = 0;
//            isMovingUpwards = false;
//        }
//
//        currentForce.x = Mathf.Clamp(currentForce.x, -MAX_FORCE, MAX_FORCE);
//        currentForce.y = Mathf.Clamp(currentForce.y, -MAX_FORCE, MAX_FORCE);
//        currentTorque.z = Mathf.Clamp(currentTorque.z, -MAX_TORQUE, MAX_TORQUE);
//
//    }

//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawRay(transform.position, -transform.up * RAYCAST_LANDING_RANGE);
//    }
//
//    public bool IsGrounded()
//    {
//        return Physics.Raycast(transform.position, -transform.up, RAYCAST_LANDING_RANGE, 1 << 9);
//    }
//
//    void ResetConstraints()
//    {
//        rb.constraints = RigidbodyConstraints.None;
//        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
//    }
//
//    public void Land(Vector3 landPosition)
//    {
//        //transform.position = landPosition; //optional, but it looks weird
//        SetLanded(true);
//    }
//
//    public void SetLanded(bool value)
//    {
//        landed = value;
//        if (landed)
//        {
//            rb.velocity = Vector3.zero;
//            StartLerping(Quaternion.identity, 0.5f);
//            //transform.rotation = Quaternion.identity;
//            // rb.freezeRotation = true;
//        }
//        else
//        {
//            ResetConstraints();
//        }
//    }
//
//    public bool HasLanded()
//    {
//        return landed;
//    }

//	void StartLerping(Quaternion endRot, float lerpTime)
//	{
//		timeTakenToLerp = lerpTime;
//		lerpStartRot = transform.rotation;
//		lerpEndRot = endRot;
//
//		timeStartedLerping = Time.time;
//		isLerping = true;
//	}


//
//	void EndLerp()
//	{
//		rb.freezeRotation = true;
//	}
//
//	void Lerp()
//	{
//		float timeSinceStarted = Time.time - timeStartedLerping;
//		float percentageComplete = timeSinceStarted / timeTakenToLerp;
//		transform.rotation = Quaternion.Lerp(lerpStartRot, lerpEndRot, percentageComplete);
//		if (percentageComplete >= 1.0f)
//		{
//			isLerping = false;
//			EndLerp();
//		}
//	}
}
