using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
	// Static
	public static bool interact;

	// gameObjects
	public GameObject cameraTarget;
	public GameObject targetLookAt = null;
	private GameObject player;
	private GameObject playerCam;
	private GameObject cutscenePlayer;
	private GameObject cutsceneCam;

	// Scripts
	private Dialogs GetDialogs;

	// Lists
	public List<float> list;

	// Variables
	public float cameraSpeed = 1f;
	private Vector3 lookAtTarget;
	private int currentPoint;
	private bool cutscene;
	private bool cutsceneDone;

	void Start()
	{
		player = GameObject.Find("Player");
		playerCam = GameObject.Find("Player Camera");
		cutscenePlayer = transform.Find("CutsceneCharacter").gameObject;
		cutsceneCam = transform.Find("Cutscene Camera").gameObject;

		GetDialogs = GameObject.Find("Audio").transform.Find("Dialogs").GetComponent<Dialogs>();

		lookAtTarget = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + 2, cameraTarget.transform.position.z);

		cutscenePlayer.transform.LookAt(cameraTarget.transform.position);

		if (targetLookAt != null)
		{
			cameraTarget.transform.LookAt(targetLookAt.transform.position);
		}

		ToggleGameObjects(false);
	}

	void Update()
	{
		if (cutscene)
		{
			Cutscene(true);
		}
		else if (cutsceneDone)
		{
			Cutscene(false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			cutscene = true;

			ToggleGameObjects(true);

			list = GetDialogs.PlayDialog(name); // list[0] = length first audio / list[1] = length all audio

			if (interact)
			{
				interact = false;
				StartCoroutine(CutsceneDuration(list[0]));
			}
			else
			{
				StartCoroutine(CutsceneDuration(list[1]));
			}
		}
	}

	void Cutscene(bool yes)
	{
		if (yes)
		{
			player.SetActive(false);
			CameraMovement();
		}
		else
		{
			player.SetActive(true);
			playerCam.SetActive(true);
			cutsceneCam.SetActive(false);

			gameObject.SetActive(false); // Disable trigger
		}
	}

	IEnumerator CutsceneDuration(float time)
	{
		yield return new WaitForSeconds(time);

		cutscene = false;
		cutsceneDone = true;
	}

	void CameraMovement()
	{
		cutsceneCam.transform.LookAt(lookAtTarget);

		Transform[] cameraRails = transform.Find("Camera Rail").GetComponentsInChildren<Transform>();

		int cameraPointsMax = transform.Find("Camera Rail").transform.childCount;

		cutsceneCam.transform.position = Vector3.MoveTowards(cutsceneCam.transform.position, cameraRails[currentPoint + 1].position, cameraSpeed * Time.deltaTime);

		if (Vector3.Distance(cutsceneCam.transform.position, cameraRails[currentPoint + 1].position) < 1f)
		{
			if (currentPoint + 1 < cameraPointsMax)
			{
				currentPoint++;
			}
		}
	}

	void ToggleGameObjects(bool on)
	{
		int children = transform.childCount;

		if (on)
		{
			for (int i = 0; i < children; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true); // https://forum.unity.com/threads/how-to-activate-a-child-of-a-parent-object.378133/
			}

			playerCam.SetActive(false);
			cutsceneCam.SetActive(true);
		}
		else
		{
			for (int i = 0; i < children; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
