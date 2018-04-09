using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter()
	{
		anim.SetBool("character_nearby", true);
	}

	void OnTriggerStay()
	{
		anim.SetBool("character_nearby", true);
	}

	void OnTriggerExit()
	{
		anim.SetBool("character_nearby", false);
	}
}
