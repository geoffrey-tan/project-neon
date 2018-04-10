using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") && transform.parent.GetComponent<MindControl>() == null)
		{
			other.gameObject.GetComponent<MindControl>().mindControl = true;
			StartCoroutine(other.gameObject.GetComponent<MindControl>().MindControlTimer(30f));

		}

		if (other.gameObject.CompareTag("Button"))
		{
			if (!other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().securedLock)
			{

				OpenDoor(other);

			}
			else
			{
				if (transform.parent.GetComponent<MindControl>() != null)
				{
					other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().securedLock = false;
					OpenDoor(other);
				}

			}


		}
	}

	void OpenDoor(Collider other)
	{
		other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().doorLock = false;
		Collider[] colliders = other.gameObject.transform.parent.Find("door_2").GetComponentsInChildren<Collider>();

		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = false;
		}

		other.gameObject.transform.parent.Find("door_2").GetComponent<Collider>().enabled = true;
	}
}

