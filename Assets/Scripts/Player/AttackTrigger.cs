using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
	public static bool mindControl;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") && transform.parent.GetComponent<MindControl>() == null && !mindControl && !other.GetComponent<EnemyAI>().combatStart)
		{
			transform.parent.GetComponent<AudioEventController>().PlaySFX("mindcontrol");
			mindControl = true;
			other.gameObject.GetComponent<MindControl>().mindControl = true;
		}

		if (other.gameObject.CompareTag("Button"))
		{
			if (!other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().securedLock)
			{
				OpenDoor(other);
				transform.parent.GetComponent<AudioEventController>().PlaySFX("interact");
			}
			else
			{
				if (transform.parent.GetComponent<MindControl>() != null)
				{
					other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().securedLock = false;
					transform.parent.GetComponent<AudioEventController>().PlaySFX("interact");
					OpenDoor(other);
				}
			}
		}
	}

	void OpenDoor(Collider other)
	{
		other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().doorLock = false;
		other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().anim.SetBool("character_nearby", true);

		Collider[] colliders = other.gameObject.transform.parent.Find("door_2").GetComponentsInChildren<Collider>();

		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = false;
		}

		other.gameObject.transform.parent.Find("door_2").GetComponent<Collider>().enabled = true;
	}
}
