using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
	// Arrays
	public Transform[] waypoints;

	// Variables
	public int currentWaypoint;
	private int waypointsMax;

	void Start()
	{
		currentWaypoint = DataSave.lastWaypoint;

		waypoints = GameObject.Find("Waypoints").GetComponentsInChildren<Transform>();
		waypointsMax = waypoints.Length;
	}

	void Update()
	{
		if (DataSave.help)
		{
			GetComponent<Renderer>().enabled = true;
		}
		else
		{
			GetComponent<Renderer>().enabled = false;
		}

		if (currentWaypoint + 1 < waypointsMax)
		{
			transform.LookAt(waypoints[currentWaypoint + 1].position);

			if (Vector3.Distance(transform.position, waypoints[currentWaypoint + 1].position) < 5f)
			{
				currentWaypoint++;
				DataSave.lastWaypoint = currentWaypoint;
			}
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
