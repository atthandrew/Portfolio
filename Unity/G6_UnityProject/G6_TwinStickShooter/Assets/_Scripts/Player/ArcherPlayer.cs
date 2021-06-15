using UnityEngine;
using UnityEngine.InputSystem;

public class ArcherPlayer : MonoBehaviour
{
	public LevelUIManager levelManager;
	public AudioSource deathSound;
	Animator anim;
	public Rigidbody rbody;
	public CapsuleCollider cc;
	public GameObject arrow;
	public Transform firePoint;
	public Transform respawn;

	public float animMultiplier = 2f;

	public float moveSpeed = 5f; //player move speed
	public float baseArrowSpeed = 20f; //arrow speed
	public float maxCharge = 3.0f;

	private bool isFallen = false;
	private bool arrowNotched = false;
	private bool arrowPulled = false;
	private float chargeTime;
	private bool isDead;
	
	private Vector2 i_move; //move vector
	private Vector2 i_look; //rotation vector

	// animator fields
	Transform cam;
	Vector3 camForward;
	Vector3 move;
	Vector3 moveInput;
	float forwardAmount;
	float turnAmount;

	// START/AWAKE

	private void Start()
	{
		SetupAnimator();
		cam = Camera.main.transform;
		isDead = false;
	}

	// UPDATE FUNCTIONS

	void Update()
	{
		if (isFallen)
		{
			transform.position = respawn.position;
			isFallen = false;
		}

		if (arrowPulled && chargeTime < maxCharge)
		{
			chargeTime += Time.deltaTime;
		}

		if (this.transform.position.y < -10)
			isFallen = true;
	}

	void FixedUpdate()
	{
		if (!isDead)
		{
			Moving();
			Looking();
		}
	}

	// COLLISION HANDLERS

	private void OnCollisionEnter(Collision collision)
	{
		GameObject coll = collision.gameObject;

		if (coll.CompareTag("Arrow") && this.GetInstanceID() != coll.GetComponent<Arrow>().ID)
		{
			// play death animation
			anim.SetFloat("Death", 1f);
			deathSound.Play();

			// stop arrow
			Destroy(coll);

			// induce ragdoll physics

			// show win message

			isDead = true;
			cc.enabled = false;
			this.GetComponent<CustomGravity>().enabled = false;
			Invoke("RespawnReset", 2.5f);
		}
	}

	// INPUT FUNCTIONS

	public void Move(InputAction.CallbackContext ctx)
	{
		i_move = ctx.ReadValue<Vector2>();

		// Moving animations
		float horizontal = i_move.x * animMultiplier;
		float vertical = i_move.y * animMultiplier;

		if (cam != null)
		{
			camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
			move = vertical * camForward + horizontal * cam.right;
		}
		else
		{
			move = vertical * Vector3.forward + horizontal * Vector3.right;
		}

		if (move.magnitude > 1)
		{
			move.Normalize();
		}
		
		if (i_move.sqrMagnitude < 0.5)
		{
			move = Vector3.zero;
		}

		AnimMove(move);
	}

	public void Look(InputAction.CallbackContext ctx)
	{
		i_look = ctx.ReadValue<Vector2>();
	}

	public void DodgeRoll(InputAction.CallbackContext ctx)
	{
		//TODO
	}

	public void Fire(InputAction.CallbackContext ctx)
	{
		switch (ctx.phase)
		{
			case InputActionPhase.Performed:
				break;

			case InputActionPhase.Started:
				arrowNotched = true;
				break;

			case InputActionPhase.Canceled:
				if (arrowNotched && arrowPulled)
				{
					Firing();
				}

				arrowNotched = false;
				arrowPulled = false;

				break;
		}
	}

	public void PullArrow(InputAction.CallbackContext ctx)
	{
		switch (ctx.phase)
		{
			case InputActionPhase.Started:
				if (arrowNotched)
				{
					arrowPulled = true;
					chargeTime = 1.0f;

					// animation
					anim.SetFloat("Aim", 1);
				}
				break;
			
			case InputActionPhase.Canceled:
				arrowPulled = false;

				// animation
				anim.SetFloat("Aim", 0);
				break;
		}
	}

	// INPUT HELPER FUNCTIONS

	void Moving()
	{
		Vector3 movement = new Vector3(i_move.x * moveSpeed, 0, i_move.y * moveSpeed);
		rbody.AddForce(movement, ForceMode.Impulse);
	}

	void Looking()
	{
		Vector3 lookVector = (Vector3.right * i_look.x) + (Vector3.forward * i_look.y);
		if (!lookVector.Equals(Vector3.zero))	
			transform.rotation = Quaternion.LookRotation(-lookVector, Vector3.up);
	}
	void Firing()
	{
		GameObject arw = Instantiate(arrow, firePoint.position, firePoint.rotation);
		Rigidbody rb = arw.GetComponent<Rigidbody>();
		arw.GetComponent<Arrow>().ID = this.GetInstanceID();
		rb.AddForce(firePoint.forward * baseArrowSpeed * chargeTime, ForceMode.Impulse);
		Destroy(arw, 5f);
		
		// animation
		anim.SetFloat("Aim", 0);
	}

	// ANIMATOR FUNCTIONS

	public void AnimMove(Vector3 move)
	{
		if (move.magnitude > 1)
		{
			move.Normalize();
		}

		this.moveInput = move;

		ConvertMoveInput();
		UpdateAnimator();

	}

	public void ConvertMoveInput()
	{
		Vector3 localMove = transform.InverseTransformDirection(moveInput);
		turnAmount = localMove.x;

		forwardAmount = localMove.z;
	}

	public void UpdateAnimator()
	{
		anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);

		//Aimming is character moving 
		if (anim.GetFloat("Forward") > -0.5 && anim.GetFloat("Forward") < .5)
		{
			if (anim.GetFloat("Turn") > -0.5 && anim.GetFloat("Turn") < .5)
			{
				// Character is not moving
				anim.SetFloat("Moving", 0);
				return;
			}
		}

		//Character is moving
		anim.SetFloat("Moving", 1);
	}

	//Puts the animator on to the player
	public void SetupAnimator()
	{
		anim = GetComponent<Animator>();

		foreach (var childAnimator in GetComponentsInChildren<Animator>())
		{
			if (childAnimator != anim)
			{
				anim.avatar = childAnimator.avatar;
				Destroy(childAnimator);
				break;
			}
		}
	}

	public void RespawnReset()
	{
		anim.SetFloat("Death", 0);
		anim.SetFloat("Forward", 1f);
		cc.enabled = true;
		this.GetComponent<CustomGravity>().enabled = true;
		isDead = false;
		levelManager.RoundOver(this);
	}
}
