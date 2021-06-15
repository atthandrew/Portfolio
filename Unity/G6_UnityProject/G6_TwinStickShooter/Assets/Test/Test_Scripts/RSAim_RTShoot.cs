using UnityEngine;
using UnityEngine.InputSystem;

public class RSAim_RTShoot : MonoBehaviour
{
	public Rigidbody rb;
	public Transform firePoint;
	public GameObject arrow;

	public float moveSpeed = 6f;
	public float baseArrowSpeed = 20f;
	public float maxCharge = 2f;
	public float shotIntervalTime = 2f;

	private Vector2 i_move; //move vector
	private Vector2 i_look; //rotation vector
	private float chargeTime;
	private float lastShotTime;

	// Update is called once per frame
	void FixedUpdate()
    {
		Looking();
		Moving();
    }

	// INPUT FUNCTIONS

	public void Move(InputAction.CallbackContext ctx)
	{
		i_move = ctx.ReadValue<Vector2>();
	}

	public void Look(InputAction.CallbackContext ctx)
	{
		i_look = ctx.ReadValue<Vector2>();
	}

	public void Fire(InputAction.CallbackContext ctx)
	{
		switch (ctx.phase)
		{
			case InputActionPhase.Performed:
				break;

			case InputActionPhase.Started:
				chargeTime = Time.time;
				break;

			case InputActionPhase.Canceled:
				if (Time.time > lastShotTime + shotIntervalTime)
					Firing();
				break;
		}
	}

	// INPUT HELPER FUNCTIONS

	void Moving()
	{
		Vector3 movement = new Vector3(i_move.x * moveSpeed, 0, i_move.y * moveSpeed);
		rb.AddForce(movement, ForceMode.Impulse);
	}

	void Looking()
	{
		Vector3 lookVector = (Vector3.right * i_look.x) + (Vector3.forward * i_look.y);
		if (lookVector.sqrMagnitude > 0.3)
			transform.rotation = Quaternion.LookRotation(-lookVector);
	}
	
	void Firing()
	{
		lastShotTime = Time.time;

		GameObject arw = Instantiate(arrow, firePoint.position, firePoint.rotation);
		Rigidbody rb = arw.GetComponent<Rigidbody>();
		arw.GetComponent<Arrow>().ID = this.GetInstanceID();
		
		if (Time.time - chargeTime <= 0.2)
		{
			rb.AddForce(firePoint.forward * baseArrowSpeed * 0.5f, ForceMode.Impulse);

		}
		else if (Time.time - chargeTime <= maxCharge)
		{
			//rb.useGravity = false;
			rb.AddForce(firePoint.forward * baseArrowSpeed * (Time.time - chargeTime + 0.5f), ForceMode.Impulse);
		}
		else
		{
			//rb.useGravity = true;
			rb.AddForce(firePoint.forward * baseArrowSpeed * maxCharge, ForceMode.Impulse);
		}

		Destroy(arw, 5f);
	}
}
