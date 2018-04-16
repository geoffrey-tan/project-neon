using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs : MonoBehaviour
{
	public AudioClip dialog1, dialog2, dialog3;
	private AudioSource dialogAudio;

	// Scripts
	private Dialogs GetDialogs;

	void Start()
	{
		dialogAudio = Camera.main.gameObject.transform.Find("Audio/Dialogs").GetComponent<AudioSource>();
	}

	public void PlayDialog(string dialog)
	{
		switch (dialog)
		{
			case "T-1":
				dialogAudio.clip = dialog1;
				dialogAudio.Play();
				break;
			case "T-2":
				dialogAudio.clip = dialog2;
				dialogAudio.Play();
				break;
			case "T-3":
				dialogAudio.clip = dialog3;
				dialogAudio.Play();
				break;
			case "T-4":
				break;
			case "T-5":
				break;
			case "T-6":
				break;

		}
	}
}
