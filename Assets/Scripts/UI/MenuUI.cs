using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
	// GameObjects
	private GameObject UI;

	// Components
	private AudioSource playerCamera;

	// Variables
	private bool gameIsPaused;

	private void Start()
	{
		UI = GameObject.Find("UI");
		playerCamera = Camera.main.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel") && !CutsceneEvents.cutcenePlaying)
		{
			Pause(gameIsPaused);
		}
	}

	public void Pause(bool toggle)
	{
		gameIsPaused = !toggle;
		playerCamera.mute = !toggle;

		UI.transform.Find("Menu").gameObject.SetActive(!toggle);

		Dialogs.PlayList.Clear();
		AudioMessage.PlayList.Clear();

		if (!toggle)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Quit()
	{
		DataSave.lastCheckpoint = Vector3.zero;

		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}
}
