using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class GroundCheck : MonoBehaviour
{
	private ThirdPersonCharacter GetThirdPersonCharacter;

	void Start()
	{
		GetThirdPersonCharacter = transform.parent.GetComponent<ThirdPersonCharacter>();
	}

	void OnTriggerEnter()
	{
		GetThirdPersonCharacter.m_IsGrounded = true;
	}

	void OnTriggerExit()
	{
		GetThirdPersonCharacter.m_IsGrounded = false;
	}
}
