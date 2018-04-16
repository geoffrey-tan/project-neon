using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	// Components
	private Animator anim;

	// Variables
	public bool doorLock = true;
	public bool securedLock;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter()
	{
		if (!doorLock && !securedLock)
		{
			anim.SetBool("character_nearby", true);
		}
	}

	void OnTriggerStay()
	{
		if (!doorLock && !securedLock)
		{
			anim.SetBool("character_nearby", true);
		}
	}

	void OnTriggerExit()
	{
		anim.SetBool("character_nearby", false);
	}
}
