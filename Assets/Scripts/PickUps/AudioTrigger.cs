using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	// Components
	private GameObject self;
	private AudioSource camAudio;
	private AudioClip thisAudioClip;

	//Scripts
	private AudioMessage GetAudioMessage;

	// Variables
	private string thisName;
	private float timeLeft;

	void Start()
	{
		self = gameObject;
		camAudio = Camera.main.gameObject.transform.Find("Audio").GetComponent<AudioSource>();
		GetAudioMessage = Camera.main.gameObject.transform.Find("Audio").GetComponent<AudioMessage>();

		thisName = transform.name;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			PlayAudioMessage(thisName);
		}
	}

	void PlayAudioMessage(string number)
	{
		switch (number)
		{
			case "AudioMessage (0)":
				thisAudioClip = GetAudioMessage.message0;
				break;
			case "AudioMessage (1)":
				thisAudioClip = GetAudioMessage.message1;
				break;
		}

		if (!camAudio.isPlaying)
		{
			camAudio.clip = thisAudioClip;
			camAudio.Play();

			transform.gameObject.SetActive(false);
		}
		else
		{
			Renderer[] renders = self.transform.GetComponentsInChildren<Renderer>();

			for (int i = 0; i < renders.Length; i++)
			{
				renders[i].enabled = false;
			}

			timeLeft = thisAudioClip.length - camAudio.time;
			StartCoroutine(AudioPlayList(timeLeft));
		}

	}

	IEnumerator AudioPlayList(float timer)
	{
		yield return new WaitForSeconds(timer + 1f);

		camAudio.clip = thisAudioClip;
		camAudio.Play();
		transform.gameObject.SetActive(false);
	}
}
