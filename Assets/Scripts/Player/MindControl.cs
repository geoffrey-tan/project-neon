using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MindControl : MonoBehaviour
{
	public bool mindControl;

	private EnemyAI GetEnemyAI;

	private GameObject player;
	private ThirdPersonCharacter GetCharacter;
	private ThirdPersonUserControl GetThirdPersonUserControl;
	private PlayerCameraController cameraController;
	private Transform cameraPos;
	private Collider attackTrigger;

	public RuntimeAnimatorController animatorController;
	public RuntimeAnimatorController animatorControllerMindControl;

	private Animator anim;

	private bool interactCD;

	void Start()
	{
		transform.GetComponent<Animator>().runtimeAnimatorController = animatorController;

		player = GameObject.Find("Player");

		GetCharacter = gameObject.transform.GetComponent<ThirdPersonCharacter>();
		GetThirdPersonUserControl = gameObject.transform.GetComponent<ThirdPersonUserControl>();

		cameraController = Camera.main.GetComponent<PlayerCameraController>();

		cameraPos = transform.Find("CameraPosMindControl");
		anim = transform.GetComponent<Animator>();

		attackTrigger = transform.Find("AttackTrigger").GetComponent<Collider>();
	}

	void Update()
	{
		if (mindControl)
		{
			transform.GetComponent<Animator>().runtimeAnimatorController = animatorControllerMindControl;
			cameraController.lookAt = cameraPos;

			//transform.GetComponent<Rigidbody>().detectCollisions = true;

			player.GetComponent<ThirdPersonCharacter>().enabled = false;
			player.GetComponent<ThirdPersonUserControl>().enabled = false;
			transform.GetComponent<NavMeshAgent>().enabled = false;
			GetCharacter.enabled = true;
			GetThirdPersonUserControl.enabled = true;
			//transform.GetComponent<Rigidbody>().detectCollisions = true;

			Interact();

		}


	}

	void Interact()
	{
		if (Input.GetButtonDown("Interact") && !interactCD) // Interact
		{

			StartCoroutine(InteractTimer(1f));

			anim.SetTrigger("MeleeLethal");

		}
	}

	private IEnumerator InteractTimer(float time)
	{
		interactCD = true;
		attackTrigger.enabled = true;

		yield return new WaitForSeconds(0.2f);

		attackTrigger.enabled = false;

		yield return new WaitForSeconds(time);

		interactCD = false;
	}

	public IEnumerator MindControlTimer(float time)
	{
		yield return new WaitForSeconds(time);

		mindControl = false;

		transform.GetComponent<Animator>().runtimeAnimatorController = animatorController;

		cameraController.lookAt = player.transform.Find("CameraPos");

		player.GetComponent<ThirdPersonCharacter>().enabled = true;
		player.GetComponent<ThirdPersonUserControl>().enabled = true;
		transform.GetComponent<NavMeshAgent>().enabled = true;
		GetCharacter.enabled = false;
		GetThirdPersonUserControl.enabled = false;

		transform.GetComponent<EnemyAI>().BacktoPatrol();

	}


}
