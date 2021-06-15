using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody rbody; //the player's rigidbody
	public float moveSpeed = 5f; //player move speed
	public GameObject arrowPrefab;
	public Transform firePoint; //position the arrow is fired from
    public bool useComplex;
    public float minArrowSpeed = 20f; //arrow speed
    public float maxCharge = 3.0f;

    private bool arrowNotched = false;
    private bool arrowPulled = false;
    private float chargeTime;
    private Vector2 moveStick; //position of the left stick
    private Vector2 rotateStick; //position of the right stick

    void Awake()
	{
		
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.position.y < -2)
		{
			Destroy(this.gameObject);
		}

        if (arrowPulled && chargeTime < maxCharge)
        {
            chargeTime += Time.deltaTime;
        }
    }

    // Called 50 times per second - if I remember right
    void FixedUpdate()
    {
        Move();
        Rotate();
	}

	// Right trigger is released
	void OnFire()
	{
        //uses complex controls
        if (useComplex)
        {
            if (arrowNotched && arrowPulled)
            {
                GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = arrow.GetComponent<Rigidbody>();
                //arrow.GetComponent<Arrow>().ID = this.gameObject.GetInstanceID();
                rb.AddForce(firePoint.forward * -minArrowSpeed * chargeTime, ForceMode.Impulse);

                arrowPulled = false;
            }

            arrowNotched = false;
        }
        //uses simple controls
        else
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            //arrow.GetComponent<Arrow>().ID = this.gameObject.GetInstanceID();
            rb.AddForce(firePoint.forward * -minArrowSpeed * chargeTime, ForceMode.Impulse);
        }
    }

	void OnMove(InputValue value)
	{
		moveStick = value.Get<Vector2>();
	}

	void OnRotate(InputValue value)
	{
		rotateStick = value.Get<Vector2>();	
	}

	void OnDodgeRoll()
	{
		//TODO
	}

    void OnNotchArrow()
    {
        arrowNotched = true;
        //TODO start timer
    }

    void OnPullArrow()
    {
        if (arrowNotched)
        {
            arrowPulled = true;
            chargeTime = 1.0f;
        }
            
    }

	void OnPause()
	{
		
	}

	void Move()
    {
        Vector3 movement = new Vector3(moveStick.x * moveSpeed, 0, moveStick.y * moveSpeed);
        rbody.AddForce(movement, ForceMode.Impulse);
    }

    void Rotate()
    {
        Vector3 rotateVector = (Vector3.right * rotateStick.x) + (Vector3.forward * rotateStick.y);
        transform.rotation = Quaternion.LookRotation(rotateVector, Vector3.up);
    }
}
