using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
	public bool canDistract;
	public bool distract;
	public GameObject targets;

	private EnemyAI GetEnemyAI;


	private void Update()
	{

		if (distract && canDistract && targets != null && GetEnemyAI != null)
		{
			Distracted();
		}
	}

	void Distracted()
	{

		distract = false;
		GetEnemyAI.distracted = true;
		targets.transform.LookAt(transform.position);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			canDistract = true;
		}

		if (other.gameObject.CompareTag("Enemy"))
		{
			GetEnemyAI = other.GetComponent<EnemyAI>();

			targets = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			canDistract = false;
		}
	}
}
