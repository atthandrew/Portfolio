
using UnityEngine;

public class RotationHazard : MonoBehaviour
{
	public bool active = true;
	public float rotationSpeed;

	private Vector3 rotate;

	private void Start()
	{
		rotate = new Vector3(0f, rotationSpeed, 0f);	
	}

	// Update is called once per frame
	void Update()
    {
		if (active)
			transform.Rotate(rotate * Time.deltaTime);
    }
}
