using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	////Components
	//private Rigidbody rb;
	//private Animator anim;

	//public GameObject mainCamera;

	//public float speed = 5f;
	//private float jumpSpeed = 5f;
	//private bool jumpPressed;
	//private bool crouching;

	//public bool grounded;

	//[SerializeField] float m_MovingTurnSpeed = 360;
	//[SerializeField] float m_StationaryTurnSpeed = 180;
	//[SerializeField] float m_JumpPower = 12f;
	//[Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
	//[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	//[SerializeField] float m_MoveSpeedMultiplier = 1f;
	//[SerializeField] float m_AnimSpeedMultiplier = 1f;
	//[SerializeField] float m_GroundCheckDistance = 0.1f;

	//Rigidbody m_Rigidbody;
	//Animator m_Animator;
	//bool m_IsGrounded;
	//float m_OrigGroundCheckDistance;
	//const float k_Half = 0.5f;
	//float m_TurnAmount;
	//float m_ForwardAmount;
	//Vector3 m_GroundNormal;
	//float m_CapsuleHeight;
	//Vector3 m_CapsuleCenter;
	//CapsuleCollider m_Capsule;
	//bool m_Crouching;

	//void Start()
	//{
	//	rb = GetComponent<Rigidbody>();
	//	anim = GetComponent<Animator>();
	//}

	//void FixedUpdate()
	//{
	//	float moveHorizontal = Input.GetAxis("Horizontal");
	//	float moveVertical = Input.GetAxis("Vertical");



	//	//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

	//	Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);



	//	//transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

	//	//transform.Translate(movement * speed * Time.deltaTime, Space.World);

	//	// Movement
	//	if (grounded)
	//	{
	//		// Idle
	//		if (Input.GetAxisRaw("Vertical") < float.Epsilon && Input.GetAxisRaw("Horizontal") < float.Epsilon && !crouching) // No input
	//		{
	//			anim.SetInteger("Animation", 0);
	//		}
	//		else if (crouching)
	//		{
	//			anim.SetInteger("Animation", 7);
	//		}

	//		if (movement != Vector3.zero) // https://answers.unity.com/questions/803365/make-the-player-face-his-movement-direction.html
	//		{
	//			anim.SetInteger("Animation", 1);

	//			//Vector3 dir; // https://forum.unity.com/threads/move-character-relative-to-camera-axis.429070/
	//			////dir = Camera.main.transform.forward;
	//			////dir.y = transform.position.y;
	//			////dir.Normalize();


	//			//dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
	//			//transform.Translate(movement * speed * Time.deltaTime);

	//			//var cameraLeftVector = Camera.main.transform.rotation.y;
	//			//transform.Translate(cameraLeftVector, Space.World);


	//			//transform.Translate(movement * speed * Time.deltaTime, Space.World);

	//			//var a = transform.rotation.y;
	//			//a = mainCamera.transform.rotation.y;

	//			//var a = Quaternion.Euler(transform.rotation.x, transform.rotation.y + mainCamera.transform.rotation.y, transform.rotation.z);

	//			//var a = movement + new Vector3(Camera.main.transform.rotation.x, 0, Camera.main.transform.rotation.z);


	//			var cam = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
	//			movement = moveVertical * cam + moveHorizontal * Camera.main.transform.right;

	//			if (movement.magnitude > 1f) movement.Normalize();
	//			movement = transform.InverseTransformDirection(movement);
	//			m_TurnAmount = Mathf.Atan2(movement.x, movement.z);
	//			m_ForwardAmount = movement.z;

	//			transform.Translate(movement * speed * Time.deltaTime);

	//			if (movement != Vector3.zero)
	//			{


	//				transform.rotation = Quaternion.Slerp(
	//					transform.rotation,
	//					Quaternion.LookRotation(movement),
	//					Time.deltaTime * 5f
	//				);
	//			}

	//			//ApplyExtraTurnRotation();

	//			//var move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
	//			//// dont worry about a jump right now.
	//			//var look = Camera.main.transform.TransformDirection(move);
	//			//look.y = 0;
	//			//move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
	//			//transform.LookAt(transform.position + look);
	//			//transform.Translate(move * speed * Time.deltaTime);

	//			//var lookPos = transform.position + Camera.main.transform.TransformDirection(move);
	//			//lookPos.y = 0;
	//			//var rotation = Quaternion.LookRotation(lookPos);
	//			//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

	//			//var lookPos = Camera.main.transform.TransformDirection(move);
	//			//lookPos.y = 0;
	//			//var rotation = Quaternion.LookRotation(lookPos);
	//			//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

	//			//moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
	//			//moveDirection = transform.TransformDirection(moveDirection);
	//			//moveDirection.Normalize();

	//			//moveDirection *= BaseSpeed;



	//			//transform.rotation = Quaternion.LookRotation(dir);



	//			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
	//			//var a = Mathf.Abs(transform.rotation.y - Camera.main.transform.rotation.y);
	//			//Debug.Log(a);
	//			//var b = transform.rotation.y + a;

	//			//transform.rotation = Quaternion.FromToRotation(new Vector3(0, 0, 0), new Vector3(0, Camera.main.transform.rotation.y, 0));
	//			//var lookPos = controller.velocity;
	//			//lookPos.y = transform.position.y;
	//			//if (lookPos.sqrMagnitude > 0.0)
	//			//{
	//			//    lookPos += transform.position;
	//			//    transform.LookAt(lookPos);
	//			//}


	//		}

	//		// Walking         
	//		//if (movement != Vector3.zero && Input.GetAxisRaw("Fire1") > 0) // Run
	//		//{
	//		//	anim.SetInteger("Animation", 6);
	//		//}

	//		//// Crouching
	//		//if (Input.GetAxisRaw("Fire3") > 0)
	//		//{
	//		//	crouching = true;
	//		//}

	//		//if (Input.GetAxisRaw("Fire3") < float.Epsilon) // No input
	//		//{
	//		//	crouching = false;
	//		//}

	//		//if (movement != Vector3.zero && crouching)
	//		//{
	//		//	//transform.Translate(transform.forward * Time.deltaTime * (Input.GetAxis("Vertical") * (speed * 1.25f)), Space.World); // https://answers.unity.com/questions/608928/moving-forward-in-world-space-with-rotation.html
	//		//	anim.SetInteger("Animation", 8);
	//		//}

	//	}
	//	else if (!grounded)
	//	{
	//	}

	//	// Rotate
	//	//if (Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Camera X") > 0)
	//	//{
	//	//	transform.Rotate(0, 4f, 0, Space.World);
	//	//}
	//	//else if (Input.GetAxisRaw("Mouse X") < 0 || Input.GetAxisRaw("Camera X") < 0)
	//	//{
	//	//	transform.Rotate(0, -4f, 0, Space.World);
	//	//}

	//	// Jump
	//	if (Input.GetAxisRaw("Jump") > 0 && grounded && !jumpPressed)
	//	{
	//		jumpPressed = true;

	//		if (Input.GetAxisRaw("Horizontal") < float.Epsilon)
	//		{
	//		}

	//		rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode.Impulse);
	//		grounded = false;
	//		anim.SetBool("Grounded", false);
	//		anim.SetInteger("Animation", 5);
	//	}
	//	else if (Input.GetAxisRaw("Jump") < float.Epsilon && jumpPressed)
	//	{
	//		jumpPressed = false;
	//	}
	//}

	//void ApplyExtraTurnRotation()
	//{
	//	// help the character turn faster (this is in addition to root rotation in the animation)
	//	float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
	//	transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	//}

	//void OnCollisionEnter()
	//{
	//	anim.SetBool("Grounded", true);
	//	grounded = true;
	//}

	//void OnCollisionStay()
	//{
	//	anim.SetBool("Grounded", true);
	//	grounded = true;
	//}

	//void OnCollisionExit()
	//{
	//	anim.SetBool("Grounded", false);
	//	grounded = false;
	//}
}
