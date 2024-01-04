using System;
using System.Xml.Linq;
using UnityEngine;

public class ProcessMovementInput : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private GameObject model;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float jumpingVelocity;
    [SerializeField] private float distanceOfContact;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float rotatingSpeed = 2f;
    [SerializeField] private Quaternion targetModelRotation;
    [SerializeField] private bool canJump;
    [SerializeField] private bool justTeleported;
    [SerializeField] private bool isKrishnaGameForAndroid = false;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float joystickSmoothTarget = 0.2f;


    private float movingVelocity;
    private Health playerHealth;
    private float knockBackTimer;
    private Rigidbody playerRigidbody;
    private Vector3 originalPlayerAnimatorPosition;

    public float playerRotatingSpeed;

    //public static event Action<bool> InformCanJump;

    //public bool JustTeleported { 
    //    get {
    //        bool val = justTeleported;
    //        justTeleported = false;
    //        return val;
    //    } 
    //    set => justTeleported = value; 
    //}

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        targetModelRotation = Quaternion.Euler(0, 0, 0);
        playerHealth = GetComponent<Health>();

        if (playerAnimator != null )
        {
            originalPlayerAnimatorPosition = playerAnimator.transform.localPosition;
        }

        if (isKrishnaGameForAndroid && !Helper.isEditor())
        {
            playerRotatingSpeed = 50f;
            movingVelocity = 8f;
        }
        else if (Application.isMobilePlatform)
        {
            playerRotatingSpeed = 25f;
            movingVelocity = 5;
        }
        else
        {
            movingVelocity = 10;
            if  (playerRotatingSpeed < 0)
            {
                playerRotatingSpeed = 100f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.31f))
        {
            canJump = true;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("OnGround", canJump);
        }

        model.transform.rotation = Quaternion.Lerp(
            model.transform.rotation,
            targetModelRotation,
            Time.deltaTime * playerRotatingSpeed
        );

        if (knockBackTimer > 0)
        {
            knockBackTimer -= Time.deltaTime;
        }
        else
        {
            ProcessInput();
        }

    }

    void ProcessInput()
    {
        // Move in the XZ plane.
        playerRigidbody.velocity = new Vector3(
            0,
            playerRigidbody.velocity.y,
            0
        );


        Vector2 movement = inputManager.MovementVectorNormalized;
        ProcessMovement(movement);


    }

    private string CheckIfShouldMove(Vector2 movement)
    {
        if (Application.isMobilePlatform)
        {
            if (movement.y > .5f) { return "forward"; }
            else if (movement.y < -.5f) { return "backward"; }
        } 
        else
        {
            if (movement.y > 0) { return "forward"; }
            else if (movement.y < 0) { return "backward"; }
        }
        return null;
    }

    private string CheckIfShouldMoveLateral(Vector2 movement)
    {
        if (Application.isMobilePlatform)
        {
            if (movement.x > .5f) { return "right"; }
            else if (movement.x < -.5f) { return "left"; }
        }
        else
        {
            if (movement.x > 0) { return "right"; }
            else if (movement.x < 0) { return "left"; }
        }
        return null;
    }

    private void ProcessMovement(Vector2 movement)
    {
        bool isPlayerMoving = false;
        string shouldMove = CheckIfShouldMove(movement); 
        if (shouldMove == "forward")
        {
            float curveValue = Mathf.Clamp01(movement.y);
            float sensitivity = Mathf.Pow(curveValue, 2);
            float smoothedMovement = Mathf.SmoothDamp(
                movement.y,
                0,
                ref sensitivity,
                0.1f,
                0.1f
            );

            //Debug.Log("smoothedMovement $ => " + smoothedMovement);

            playerRigidbody.velocity = new Vector3(
                model.transform.forward.x * (smoothedMovement * movingVelocity),
                playerRigidbody.velocity.y,
                model.transform.forward.z * (smoothedMovement * movingVelocity)
            );
            isPlayerMoving = true;
        }
        else if (shouldMove == "backward")
        {
            playerRigidbody.velocity = new Vector3(
                -model.transform.forward.x * movingVelocity,
                playerRigidbody.velocity.y,
                -model.transform.forward.z * movingVelocity
            );
            isPlayerMoving = true;
        }
        shouldMove = CheckIfShouldMoveLateral(movement);
        if (shouldMove == "right")
        {
            targetModelRotation = Quaternion.Euler(
                0,
                model.transform.localEulerAngles.y + (playerRotatingSpeed * movement.x) * Time.deltaTime,
                0
            );
        }
        if (shouldMove == "left")
        {
            targetModelRotation = Quaternion.Euler(
                0,
                model.transform.localEulerAngles.y - (playerRotatingSpeed * -movement.x) * Time.deltaTime,
                0
            );
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Forward", isPlayerMoving ? 1f : 0f);
        }
    }


    void LateUpdate()
    {
        if (playerAnimator != null)
        {
            // Keep the character animator's gameobject in place.
            playerAnimator.transform.localPosition = originalPlayerAnimatorPosition;
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.GetComponent<EnemyBullet>() != null)
        {
            Hit((transform.position - otherCollider.transform.position).normalized);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Arrow a = collision.gameObject.GetComponent<Arrow>();
        if (a != null && !playerAnimator)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), a.gameObject.GetComponent<BoxCollider>());
        } else
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            bool isHittingEnemy = enemy != null;

            if (isHittingEnemy)
            {
                Debug.Log(" isHittingEnemy => player collision hit");
                Hit((transform.position - collision.transform.position).normalized);
            }
        }
    }

    private void Hit(Vector3 direction)
    {
        Vector3 knockBackDirection = (direction + Vector3.up).normalized;
        playerRigidbody.AddForce(knockBackDirection * knockBackForce);

        knockBackTimer = .4f;
        int health = ReduceHealth();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private int ReduceHealth()
    {
        int health = playerHealth.HealthValue;
        playerHealth.HealthValue = health--;
        return health;
    }
}
