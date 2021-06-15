using UnityEngine;

public class WindHazard : MonoBehaviour
{
	public float windStrength;
	public bool affectsArrows = true;
	public bool affectsPlayers = false;

	// applied once per frame while other is within the collider
	private void OnTriggerStay(Collider other)
	{
		// wind effect for players
		if (affectsPlayers)
		{
			if (other.CompareTag("Player"))
			{
				Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
				rb.AddForce(transform.forward * windStrength, ForceMode.Impulse);
			}
		}
		
		// wind effect for arrows
		if (affectsArrows)
		{
			if (other.CompareTag("Arrow"))
			{
				Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
				rb.AddForce(transform.forward * windStrength, ForceMode.Acceleration);
			}
		}
	}
}
