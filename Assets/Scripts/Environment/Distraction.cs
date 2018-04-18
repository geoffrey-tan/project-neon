using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
	// Scripts
	private EnemyAI GetEnemyAI;

	// Lists   
	public List<GameObject> targets; // https://stackoverflow.com/questions/249452/add-new-item-in-existing-array-in-c-net


	// Variables
	public bool canDistract;
	public bool distract;

	void Start()
	{
		targets = new List<GameObject>();
	}

	void Update()
	{
		if (distract && canDistract && targets != null)
		{
			Distracted();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			canDistract = true;
		}

		if (other.gameObject.CompareTag("Enemy"))
		{
			targets.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			canDistract = false;
		}

		if (other.gameObject.CompareTag("Enemy"))
		{
			var objectOne = other.gameObject.GetInstanceID();

			for (int i = 0; i < targets.Count; i++)
			{
				if (objectOne == targets[i].gameObject.GetInstanceID())
				{
					targets.RemoveAt(i); // https://stackoverflow.com/questions/10018957/c-sharp-how-to-remove-item-from-list
				}
			}
		}
	}

	void Distracted()
	{
		distract = false;

		for (int i = 0; i < targets.Count; i++)
		{
			// https://answers.unity.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html
			Vector3 targetPosition = new Vector3(transform.position.x, targets[i].transform.position.y, transform.position.z);

			targets[i].GetComponent<EnemyAI>().distracted = true;
			targets[i].transform.LookAt(targetPosition);
		}
	}
}
