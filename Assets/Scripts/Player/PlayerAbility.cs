using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
	// Components
	private GameObject distractObject;
	private Animator anim;
	private Collider attackTrigger;
	private GameObject self;
	private Dialogs GetDialogs;
	//private GameObject laser;

	// Variables
	private bool abilityCD; // Global ability cooldown

	void Start()
	{
		anim = GetComponent<Animator>();
		attackTrigger = transform.Find("AttackTrigger").GetComponent<Collider>();
		self = gameObject;
		//laser = transform.Find("Lasers").gameObject;
		GetDialogs = GameObject.Find("Audio/Dialogs").GetComponent<Dialogs>();
	}

	void Update() // https://answers.unity.com/questions/20717/inputgetbuttondown-inconsistent.html
	{
		if (Input.GetButtonDown("Interact") && !abilityCD) // Attack 
		{
			Interact();
		}

		if (Input.GetButtonDown("Distract") && !abilityCD && !self.CompareTag("Enemy")) // Distract
		{
			Distract();
		}

		//if (sight)
		//{
		//    laser.GetComponent<LineRenderer>().enabled = true;

		//    laser.GetComponent<LineRenderer>().SetPosition(0, enemyGun1.position);
		//    laser.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);
		//}
		//else
		//{
		//    laser.GetComponent<LineRenderer>().enabled = false;
		//}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Distraction"))
		{
			distractObject = other.gameObject; // Distraction object
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Distraction"))
		{
			distractObject = null;
		}
	}

	void Interact()
	{
		anim.SetTrigger("Interact");

		StartCoroutine(InteractTimer(0.2f));
		StartCoroutine(AbilityCooldown(1f));
	}

	void Distract()
	{
		if (distractObject != null)
		{
			GetComponent<AudioEventController>().PlaySFX("distract");

			if (Dialogs.distract) // Tut
			{
				Dialogs.distract = false;
				GetDialogs.PlayDialog("T-5-6");
			}

			anim.SetTrigger("Distract");

			StartCoroutine(AbilityCooldown(1f));

			distractObject.GetComponent<Distraction>().distract = true;
		}
	}

	IEnumerator InteractTimer(float time)
	{
		attackTrigger.enabled = true;

		yield return new WaitForSeconds(time);

		attackTrigger.enabled = false;
	}

	IEnumerator AbilityCooldown(float time)
	{
		abilityCD = true;

		yield return new WaitForSeconds(time);

		abilityCD = false;
	}
}
