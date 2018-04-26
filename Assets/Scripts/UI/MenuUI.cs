using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
	// GameObjects
	private GameObject UI;
	private GameObject audioContainer;

	// Components
	private AudioSource playerCamera;

	// Variables
	public static bool gameIsPaused;

	private void Start()
	{
		UI = GameObject.Find("UI");
		playerCamera = Camera.main.GetComponent<AudioSource>();
		audioContainer = GameObject.Find("Audio");
	}

	private void Update()
	{
		Debug.Log(CutsceneEvents.cutcenePlaying);
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

		UI.transform.Find("Player Info").gameObject.SetActive(toggle);
		UI.transform.Find("Overlay").gameObject.SetActive(toggle);

		Dialogs.PlayList.Clear();
		AudioMessage.PlayList.Clear();

		audioContainer.transform.Find("Dialogs").GetComponent<AudioSource>().Stop();
		audioContainer.transform.Find("AudioMessage").GetComponent<AudioSource>().Stop();

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
