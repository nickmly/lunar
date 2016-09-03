using UnityEngine;
using System.Collections;


/// <summary>
/// Main controller for the player's ship
/// </summary>  
public class Player : MonoBehaviour {

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

    //Variables
    private Vector2 currentForce = Vector2.zero;
    private float pushRate = 0.5f;
    private float boostRate = 0.25f;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
    }

    void ShowParticles()
    {
        if(currentForce.x < 0)
        {
            fireRight.Play();
        }
        if(currentForce.x > 0)
        {
            fireLeft.Play();
        }
        if(currentForce.y > 0)
        {
            fireUp.Play();
        }
    }

    void HandleMovement()
    {
        if (currentForce != Vector2.zero)
        {
            Debug.Log("Adding Force: " + currentForce.ToString());
            rb.AddForce(currentForce);
            ShowParticles();
        }
        else
        {
            if(fireRight.isPlaying || fireLeft.isPlaying || fireUp.isPlaying)
            {
                fireRight.Stop();
                fireLeft.Stop();
                fireUp.Stop();
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKey(LEFT_KEY))
        {
            currentForce.x -= pushRate;
        }
        if (Input.GetKey(RIGHT_KEY))
        {
            currentForce.x += pushRate;
        }

        if (Input.GetKeyUp(LEFT_KEY) || Input.GetKeyUp(RIGHT_KEY))
        {
            currentForce.x = 0;
            isMoving = false;
        }
     

        if (Input.GetKey(UP_KEY))
        {
            currentForce.y += boostRate;
        }

        if (Input.GetKeyUp(UP_KEY))
        {
            currentForce.y = 0;
            isMoving = false;
        }

        currentForce.x = Mathf.Clamp(currentForce.x, -MAX_FORCE, MAX_FORCE);
        currentForce.y = Mathf.Clamp(currentForce.y, -MAX_FORCE, MAX_FORCE);

        if (Input.GetKey(LEFT_KEY) || Input.GetKey(RIGHT_KEY) || Input.GetKey(UP_KEY))
        {
            isMoving = true;
        }
    }
}
