using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
	// Constants
	private float Y_ANGLE_MIN = -150f;
	private float Y_ANGLE_MAX = 150f;

	// Components
	public Transform lookAt; // Target for camera
	private Transform self;
	public LayerMask hitMask;

	private bool lineCast;

	public float distance = 2f;

	private float currentX;
	private float currentY = -40f;
	private float sensitivityX = 2f;
	private float sensitivityY = 1f;
	private bool adjust;

	private float increase = 1.7f; // Speed
	private float maxIncrease = 2f;
	private float increaseAdjust = 35f; // Speed

	private float decrease = 3.4f; // Speed
	private float maxDecrease = 2f;
	private float decreaseAdjust = 70f; // Speed

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
		lineCast = Physics.Linecast(transform.position, lookAt.transform.position, hitMask);

		if (lineCast)
		{
			DistanceChange(false);
		}
		else
		{
			DistanceChange(true);
		}

		Vector3 dir = new Vector3(0, 2f, -distance);

		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
		self.position = lookAt.position + rotation * dir;

		self.LookAt(lookAt.position);
	}

	void DistanceChange(bool isIncreasing)
	{
		if (isIncreasing)
		{
			distance += increase * Time.deltaTime; // https://answers.unity.com/questions/1242672/how-to-increase-and-decrease-speed-gradually.html
			distance = Mathf.Clamp(distance, 0f, maxIncrease);

			if (currentY >= Y_ANGLE_MIN && distance < 1.99f)
			{
				currentY += increaseAdjust * Time.deltaTime;
			}

			currentX += Input.GetAxis("Mouse X") * sensitivityX;
			currentY += Input.GetAxis("Mouse Y") * sensitivityY;

			currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		}
		else
		{
			distance -= decrease * Time.deltaTime;
			distance = Mathf.Clamp(distance, 0f, maxDecrease);

			if (currentY <= Y_ANGLE_MAX && distance > 0.01f)
			{
				currentY -= decreaseAdjust * Time.deltaTime;
			}

			currentX += Input.GetAxis("Mouse X") * sensitivityX;
			currentY += Input.GetAxis("Mouse Y") * sensitivityY;

			currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		}
	}
}
