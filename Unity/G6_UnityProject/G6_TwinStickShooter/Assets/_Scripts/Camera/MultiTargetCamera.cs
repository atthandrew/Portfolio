using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
	public List<Transform> targets;
	public Vector3 offset;
	
	public float smoothTime = 2f;

	public float minZoom = 20f;
	public float maxZoom = 10f;
	//public float zoomLimiter = 2f;

	private Vector3 velocity;
	public Camera cam;

	private float size;
	private Vector3 startLocation;

	// Start is called before the first frame update
	void Start()
    {
		size = cam.orthographicSize;
		startLocation = cam.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
		if (targets.Count == 0)
			return;

		var bounds = EncapsulateBounds();
		Move(bounds.center);
		if (bounds.size.x > bounds.size.z)
			Zoom(bounds.size.x);
		else
			Zoom(bounds.size.z);
    }

	private void Move(Vector3 bounds)
	{
		Vector3 newPosition = bounds + offset;
		transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
	}

	void Zoom(float bounds)
	{
		float newZoom;

		if (bounds > minZoom)
			newZoom = Mathf.Lerp(maxZoom, minZoom, smoothTime);
		else if (bounds < maxZoom)
			newZoom = Mathf.Lerp(bounds, maxZoom, smoothTime);
		else
			newZoom = Mathf.Lerp(maxZoom, bounds, smoothTime);
		//float newZoom = Mathf.Lerp(maxZoom, bounds, smoothTime);

		//if (newZoom <= 0.1)
		//	return;

		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
	}

	Bounds EncapsulateBounds()
	{
		var bounds = new Bounds(targets[0].position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate(targets[i].position);
		}

		return bounds;
	}

	public void ResetPosition()
	{
		cam.orthographicSize = size;
		cam.transform.position = startLocation;
	}
}
