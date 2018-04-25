using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEvents : MonoBehaviour
{
	// Static
	public static bool interact;
	public static bool cutcenePlaying;

	// gameObjects
	public GameObject cameraTarget;
	public GameObject targetLookAt = null;
	private GameObject player;
	private GameObject playerCam;
	private GameObject cutscenePlayer;
	private GameObject cutsceneCam;
	private GameObject UI;

	// Components
	private AudioSource messageAudio;

	// Scripts
	private Dialogs GetDialogs;

	// Lists
	public List<float> list;

	// Variables
	public bool decisionCutscene; // True = Cutscene waits for player input
	private bool playerInput;
	private int decision;
	public float cameraSpeed = 1f;
	private Vector3 lookAtTarget;
	private int currentPoint;
	private bool cutscene;
	private bool cutsceneDone;

	void Start()
	{
		UI = GameObject.Find("UI");
		player = GameObject.Find("Player");
		playerCam = GameObject.Find("Player Camera");
		cutscenePlayer = transform.Find("CutsceneCharacter").gameObject;
		cutsceneCam = transform.Find("Cutscene Camera").gameObject;

		messageAudio = GameObject.Find("Audio").transform.Find("AudioMessage").GetComponent<AudioSource>();

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

			if (playerInput)
			{
				UI.transform.Find("Player Info/Decision").gameObject.SetActive(true);
				list.Clear();

				if (Input.GetButtonDown("Submit"))
				{
					playerInput = false;

					decision = 0; // Kill
					var followUp = transform.Find("FollowUp").GetChild(decision);

					list = GetDialogs.PlayDialog(followUp.name);

					StartCoroutine(CutsceneDuration(list[1], 2));
				}
				else if (Input.GetButtonDown("Cancel"))
				{
					playerInput = false;

					decision = 1; // Life
					var followUp = transform.Find("FollowUp").GetChild(decision);

					list = GetDialogs.PlayDialog(followUp.name);

					StartCoroutine(CutsceneDuration(list[1], 2));
				}
			}
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
				StartCoroutine(CutsceneDuration(list[0])); // Plays 1 audio
			}
			else if (decisionCutscene)
			{
				StartCoroutine(CutsceneDuration(list[1], 1)); // Wait for player input
			}
			else
			{
				StartCoroutine(CutsceneDuration(list[1])); // Plays all audio
			}
		}
	}

	void Cutscene(bool yes)
	{
		if (yes)
		{
			messageAudio.volume = 0f;

			cutcenePlaying = true;

			player.SetActive(false);
			CameraMovement();
		}
		else
		{
			messageAudio.volume = 1f;

			cutcenePlaying = false;

			player.SetActive(true);
			playerCam.SetActive(true);
			cutsceneCam.SetActive(false);

			EnemyAI.safeSpot = false;
			gameObject.SetActive(false); // Disable trigger
		}
	}

	IEnumerator CutsceneDuration(float time, int waitForInput = 0)
	{
		UI.transform.Find("Player Info/Decision").gameObject.SetActive(false);

		yield return new WaitForSeconds(time);

		switch (waitForInput)
		{
			case 0:
				cutscene = false;
				cutsceneDone = true;
				break;
			case 1:
				playerInput = true;
				break;
			case 2:
				var sceneIndex = SceneManager.GetActiveScene().buildIndex;

				LevelTransition.ExitLVL(sceneIndex);
				break;
		}
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
