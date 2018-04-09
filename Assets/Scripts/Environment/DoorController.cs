using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private Animator anim;
	public bool doorLock;

	void Start()
	{
		anim = GetComponent<Animator>();

		doorLock = true;
	}

	void OnTriggerEnter()
	{
		if (!doorLock)
		{
			anim.SetBool("character_nearby", true);
		}
	}

	void OnTriggerStay()
	{
		if (!doorLock)
		{
			anim.SetBool("character_nearby", true);
		}
	}

	void OnTriggerExit()
	{
		anim.SetBool("character_nearby", false);
	}
}
