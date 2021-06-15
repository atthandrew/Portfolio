using UnityEngine;

public class Arrow : MonoBehaviour
{
	public Rigidbody rb;
	public CapsuleCollider cc;

	private float despawnTime = 1f;
	private bool deadArrow = false;

	// property for arrow ID
	public int ID { get; set; }

    // Update is called once per frame
    void Update()
    {
		Vector3 rotateVector = new Vector3(rb.velocity.normalized.x, 0, rb.velocity.normalized.z);
		if (!deadArrow && rotateVector != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(rotateVector, Vector3.up);
		}
    }

	// called when arrow collides with another object
	private void OnCollisionEnter(Collision collision)
	{
		GameObject coll = collision.gameObject;

		if (coll.CompareTag("Player") && ID != coll.GetComponent<ArcherMovement>().playerNumber)
		{
			cc.enabled = false;
			rb.velocity = Vector3.zero;
			//rb.useGravity = true;
			deadArrow = true;
			Destroy(this.gameObject, despawnTime);
		}
		else if (coll.CompareTag("Terrain"))
		{
			cc.enabled = false;
			rb.velocity = Vector3.zero;
			deadArrow = true;
			Destroy(this.gameObject, despawnTime);
		}
		else if (coll.CompareTag("Arrow"))
		{
			cc.enabled = false;
			rb.velocity = Vector3.zero;
			//rb.useGravity = true;
			deadArrow = true;
		}
	}

	public bool IsDeadArrow()
	{
		return deadArrow;
	}
}
