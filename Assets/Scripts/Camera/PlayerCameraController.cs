using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	private const float Y_ANGLE_MIN = -50f;
	private const float Y_ANGLE_MAX = 50f;

	public Transform lookAt;
	private Transform self;
	private Camera cam;

	private float distance = 4f;
	private float currentX;
	private float currentY = -25f;
	private float sensitivityX = 2f;
	private float sensitivityY = 1f;

	private Vector3 offset;

	void Start()
	{
		self = transform;
		cam = Camera.main;
	}

	void Update()
	{
		currentX += Input.GetAxis("Camera X") * sensitivityX;
		currentY += Input.GetAxis("Camera Y") * sensitivityY;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	void LateUpdate()
	{
		Vector3 dir = new Vector3(0, 2f, -distance);

		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
		self.position = lookAt.position + rotation * dir;

		self.LookAt(lookAt.position);
	}
}
