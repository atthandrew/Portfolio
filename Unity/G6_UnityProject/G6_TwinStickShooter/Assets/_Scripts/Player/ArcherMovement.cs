using UnityEngine;
using UnityEngine.InputSystem;

public class ArcherMovement : MonoBehaviour
{
	// REFERENCES
	public Rigidbody rbody;
	public CapsuleCollider cc;
	public AudioSource footStepSound;
	public AudioSource deathSound;

	// PUBLIC FIELDS
	[HideInInspector] public int playerNumber = 1;
	public float moveSpeed = 6f;

	[HideInInspector] public bool isDead;

	// PRIVATE FIELDS
	private Vector2 i_move; // movement input
	private Vector2 i_look; // rotation input

	// ANIMATOR FIELDS
	private Animator anim;
	//private Transform animTransform;
	private Vector3 animForward;
	private Vector3 animMove;
	private Vector3 animMoveInput;
	private float animForwardAmount;
	private float animTurnAmount;
	private float animMultiplier = 2f;

	// AWAKE, START

	void Start()
	{
		// initialize other fields
		isDead = false;

		// setup animator fields
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

		//animTransform = Camera.main.transform;
	}

	// UPDATE FUNCTIONS

	void FixedUpdate()
	{
		if (!isDead)
		{
			Moving();
			Looking();
		}
	}

	// COLLISION HANDLERS

	void OnCollisionEnter(Collision collision)
	{
		GameObject coll = collision.gameObject;
		
		if (coll.CompareTag("Arrow") && playerNumber != coll.GetComponent<Arrow>().ID)
		{
			coll.GetComponent<Rigidbody>().velocity = Vector3.zero;
			Death();
		}
	}

	// INPUT HANDLERS

	public void Move(InputAction.CallbackContext ctx)
	{
		i_move = ctx.ReadValue<Vector2>();

		// Moving animations
		float horizontal = i_move.x * animMultiplier;
		float vertical = i_move.y * animMultiplier;

		//if (animTransform != null)
		//{
		//	animForward = Vector3.Scale(animTransform.up, new Vector3(1, 0, 1)).normalized;
		//	animMove = vertical * animForward + horizontal * animTransform.right;
		//}
		//else
		{
			animMove = vertical * Vector3.forward + horizontal * Vector3.right;
		}

		if (animMove.magnitude > 1)
		{
			animMove.Normalize();
		}

		if (i_move.sqrMagnitude < 0.5)
		{
			animMove = Vector3.zero;
		}

		AnimMove(animMove);
	}

	public void Look(InputAction.CallbackContext ctx)
	{
		i_look = ctx.ReadValue<Vector2>();
	}

	private void Moving()
	{
		Vector3 movement = new Vector3(i_move.x * moveSpeed, 0, i_move.y * moveSpeed);
		rbody.AddForce(movement, ForceMode.Impulse);
	}

	private void Looking()
	{
		Vector3 lookVector = (Vector3.right * i_look.x) + (Vector3.forward * i_look.y);
		if (lookVector.sqrMagnitude > 0.85f)
			transform.rotation = Quaternion.LookRotation(-lookVector, Vector3.up);
	}

	// ANIMATION FUNCTIONS
	public void AnimMove(Vector3 move)
	{
		if (move.magnitude > 1)
		{
			move.Normalize();
		}

		animMoveInput = move;

		ConvertMoveInput();
		UpdateAnimator();

	}

	public void ConvertMoveInput()
	{
		Vector3 localMove = transform.InverseTransformDirection(animMoveInput);
		animTurnAmount = localMove.x;

		animForwardAmount = localMove.z;
	}

	public void UpdateAnimator()
	{
		anim.SetFloat("Forward", animForwardAmount, 0.1f, Time.deltaTime);
		anim.SetFloat("Turn", animTurnAmount, 0.1f, Time.deltaTime);

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


	// OTHER HANDLERS

	void Death()
	{
		// play death animation and sound
		anim.SetFloat("Death", 1f);
		deathSound.Play();

		// disable dead player
		isDead = true;
		cc.enabled = false;
	}

	public void RespawnReset()
	{
		// reset animations
		anim.SetFloat("Death", 0f);
		anim.SetFloat("Forward", 0f);
        anim.SetFloat("Turn", 0f);

		// reset other components
		isDead = false;
		cc.enabled = true;
	}
}
