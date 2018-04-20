using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	// Constants
	private const float Y_ANGLE_MIN = -50f;
	private const float Y_ANGLE_MAX = 50f;

	// Components
	public Transform lookAt; // Target for camera
	private Transform self;

	private float distance = 2f;

	private float currentX;
	private float currentY = -25f;
	private float sensitivityX = 2f;
	private float sensitivityY = 1f;
	public bool hit;

	void Start()
	{
		self = transform;
	}

	void Update()
	{
		currentX += Input.GetAxis("Mouse X") * sensitivityX;
		currentY += Input.GetAxis("Mouse Y") * sensitivityY;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	void LateUpdate()
	{
		Vector3 dir = new Vector3(0, 2f, -distance);

		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
		self.position = lookAt.position + rotation * dir;

		self.LookAt(lookAt.position);

		//if (Physics.Raycast()) [
		//]

		//var relativePos = transform.position - (lookAt.position);
		//RaycastHit hit;
		//if (Physics.Raycast(lookAt.position, relativePos, hit.point, distance + 0.5))
		//{
		//    Debug.DrawLine(target.position, hit.point);
		//    distanceOffset = distance - hit.distance + 0.8;
		//    distanceOffset = Mathf.Clamp(distanceOffset, 0, distance);
		//}
		//else
		//{
		//    distanceOffset = 0;
		//}
	}

	void OnTriggerEnter()
	{
		hit = true;
	}

	void OnTriggerExit()
	{
		hit = false;
	}
}
