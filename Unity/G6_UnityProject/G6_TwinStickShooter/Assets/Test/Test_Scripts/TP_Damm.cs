using UnityEngine;
using UnityEngine.InputSystem;

public class TP_Damm : MonoBehaviour
{
	public Rigidbody rbody; //the player's rigidbody
	public GameObject arrowPrefab;
	public Transform firePoint; //position the arrow is fired from

	//player parameters
	public float moveSpeed = 5f; //player move speed
	public float jumpForce = 10f; //force applied on player jump

	//arrow parameters
	public float arrowDuration = 5f;
	public float minArrowSpeed = 20f; //arrow speed
	public float maxCharge = 3.0f;

	//input fields
	private Vector2 moveStick; //position of the left stick
	private Vector2 lookStick; //position of the right stick
	private bool isJumping = false;

	private void FixedUpdate()
	{
		//movement & rotation
		Moving();
		Looking();
		Rolling();
	}

	//physics helpers
	void Moving()
	{
		Vector3 move = new Vector3(moveStick.x * moveSpeed, 0, moveStick.y * moveSpeed);
		rbody.AddForce(move, ForceMode.Acceleration);
	}

	void Looking()
	{
		Vector3 look = (Vector3.right * -lookStick.x) + (Vector3.forward * -lookStick.y);
		if (!look.Equals(Vector3.zero))
			transform.rotation = Quaternion.LookRotation(look, Vector3.up);
	}

	void Rolling()
	{
		if (isJumping)
		{
			rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			isJumping = false;
		}
	}

	//controls
	public void Move(InputAction.CallbackContext ctx)
	{
		moveStick = ctx.ReadValue<Vector2>();	
	}

	public void Look(InputAction.CallbackContext ctx)
	{
		lookStick = ctx.ReadValue<Vector2>();
	}

	public void Fire(InputAction.CallbackContext ctx)
	{
		GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
		Rigidbody rb = arrow.GetComponent<Rigidbody>();
		//arrow.GetComponent<Arrow>().ID = this.gameObject.GetInstanceID();
		rb.AddForce(firePoint.forward * minArrowSpeed, ForceMode.Impulse);
		Destroy(arrow, arrowDuration);
	}

	public void Jump(InputAction.CallbackContext ctx)
	{
		isJumping = true;
	}	
}
