using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Debug.Log(other);
			other.gameObject.GetComponent<EnemyAI>().mindControl = true;
		}

		if (other.gameObject.CompareTag("Button"))
		{
			Debug.Log(other);
			other.gameObject.transform.parent.Find("door_2").GetComponent<DoorController>().doorLock = false;
			Collider[] colliders = other.gameObject.transform.parent.Find("door_2").GetComponentsInChildren<Collider>();

			for (int i = 0; i < colliders.Length; i++)
			{
				colliders[i].enabled = false;
			}

			other.gameObject.transform.parent.Find("door_2").GetComponent<Collider>().enabled = true;

		}
	}
}
