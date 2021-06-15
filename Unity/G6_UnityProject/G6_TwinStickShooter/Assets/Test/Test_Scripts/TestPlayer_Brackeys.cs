using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayer_Brackeys : MonoBehaviour
{
	TestControls_Brackeys ctrl;

	public Rigidbody rbody; //the player's rigidbody
	public float moveSpeed = 5f; //player move speed
	public float jumpForce = 10f; //force applied on player jump
	public GameObject arrowPrefab;
	public Transform firePoint; //position the arrow is fired from
	//public bool useComplex;
	public float minArrowSpeed = 20f; //arrow speed
	public float maxCharge = 3.0f;
	//private float chargeTime;
	private Vector2 moveStick; //position of the left stick
	private Vector2 lookStick; //position of the right stick

	private void Awake()
	{
		ctrl = new TestControls_Brackeys();

		ctrl.Test.Move.performed += OnMove;

		ctrl.Test.Look.performed += OnLook;

		ctrl.Test.Jump.performed += OnJump;

		ctrl.Test.Fire.performed += OnFire;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
		Move();
		Rotate();
	}

	//Physics helpers
	void Move()
	{
		Vector3 move = new Vector3(moveStick.x * moveSpeed, 0, moveStick.y * moveSpeed);
		rbody.AddForce(move, ForceMode.Impulse);
	}

	void Rotate()
	{
		Vector3 look = (Vector3.right * -lookStick.x) + (Vector3.forward * -lookStick.y);
		if (!look.Equals(Vector3.zero))	
			transform.rotation = Quaternion.LookRotation(look, Vector3.up);
	}

	// Unity Event Methods for input
	void OnMove(InputAction.CallbackContext ctx)
	{
		moveStick = ctx.ReadValue<Vector2>();
	}

	void OnLook(InputAction.CallbackContext ctx)
	{
		lookStick = ctx.ReadValue<Vector2>();
	}

	void OnFire (InputAction.CallbackContext ctx)
	{
		GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
		Rigidbody rb = arrow.GetComponent<Rigidbody>();
		//arrow.GetComponent<Arrow>().ID = this.gameObject.GetInstanceID();
		rb.AddForce(firePoint.forward * minArrowSpeed, ForceMode.Impulse);
	}

	void OnJump(InputAction.CallbackContext ctx)
	{
		rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	}

	//Enable and disable controls

	void OnEnable()
	{
		ctrl.Test.Enable();
	}

	void OnDisable()
	{
		ctrl.Test.Disable();
	}
}
