using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
	// gameObjects
	public GameObject playerCam;
	public GameObject target;
	private GameObject cutsceneCam;

	// Variables
	private Vector3 startPos, endPos;
	private bool cutscene;

	void Start()
	{
		cutsceneCam = transform.Find("Cutscene Camera").gameObject;

		startPos = transform.Find("Camera Rail/StartPos").position;
		endPos = transform.Find("Camera Rail/EndPos").position;

		target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z);
	}

	void Update()
	{
		if (cutscene)
		{
			cutsceneCam.transform.position = Vector3.MoveTowards(cutsceneCam.transform.position, endPos, 1f * Time.deltaTime);
			cutsceneCam.transform.LookAt(target.transform.position);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Hi");
			playerCam.SetActive(false);
			cutsceneCam.SetActive(true);

			cutscene = true;
		}
	}

	IEnumerator CutsceneDuration(float time)
	{
		yield return new WaitForSeconds(time);
	}
}
