using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
	private Animator anim;
	private Renderer playerSword0, playerSword1, playerSword2, playerSword3;

	private Coroutine sheathCoroutine;

	private Collider attackTrigger;

	private float sheathTime = 5f;
	private bool attackCD;
	private bool weaponLethal;

	void Start()
	{
		anim = GetComponent<Animator>();

		playerSword0 = transform.Find("Root/Ribs/PlayerSword (0)").GetComponent<Renderer>();
		playerSword1 = transform.Find("Root/Ribs/PlayerSword (1)").GetComponent<Renderer>();
		playerSword2 = transform.Find("Root/Ribs/Left_Shoulder_Joint_01/Left_Upper_Arm_Joint_01/Left_Forearm_Joint_01/Left_Wrist_Joint_01/PlayerSword (2)").GetComponent<Renderer>();
		playerSword3 = transform.Find("Root/Ribs/Right_Shoulder_Joint_01/Right_Upper_Arm_Joint_01/Right_Forearm_Joint_01/Right_Wrist_Joint_01/PlayerSword (3)").GetComponent<Renderer>();

		attackTrigger = transform.Find("AttackTrigger").GetComponent<Collider>();
	}
	void Update() // https://answers.unity.com/questions/20717/inputgetbuttondown-inconsistent.html
	{
		if (Input.GetButtonDown("Interact") && !attackCD) // Attack 
		{

			WeaponAttack();
		}
	}

	void WeaponDraw(bool draw)
	{
		if (!draw)
		{
			playerSword0.enabled = true;
			playerSword1.enabled = true;
			playerSword2.enabled = false;
			playerSword3.enabled = false;
		}
		else
		{
			playerSword0.enabled = false;
			playerSword1.enabled = false;
			playerSword2.enabled = true;
			playerSword3.enabled = true;

			if (sheathCoroutine != null) // https://answers.unity.com/questions/1029332/restart-a-coroutine.html
			{
				StopCoroutine(sheathCoroutine);
			}

			sheathCoroutine = StartCoroutine(SheathTimer(sheathTime));
		}
	}

	void WeaponAttack()
	{

		StartCoroutine(AttackTimer(1f));

		anim.SetTrigger("MeleeLethal");

		WeaponDraw(true);
	}

	void Interact()
	{


	}

	private IEnumerator AttackTimer(float time)
	{
		attackCD = true;
		attackTrigger.enabled = true;

		yield return new WaitForSeconds(0.2f);

		attackTrigger.enabled = false;

		yield return new WaitForSeconds(time);

		attackCD = false;
	}

	private IEnumerator SheathTimer(float time)
	{
		yield return new WaitForSeconds(time);

		WeaponDraw(false);
	}

}
