using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMessage : MonoBehaviour
{
	private AudioSource player;
	private AudioSource camAudio;
	public AudioClip message0, message1, message2;
	private string thisName;
	private AudioClip thisAudioClip;
	private float timeLeft;
	private GameObject self;

	void Start()
	{
		player = GameObject.Find("Player").GetComponent<AudioSource>();
		thisName = transform.name;

		camAudio = Camera.main.gameObject.transform.Find("Audio").GetComponent<AudioSource>();
		self = gameObject;

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
				thisAudioClip = message0;
				break;
			case "AudioMessage (1)":
				thisAudioClip = message1;
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
			// getcomponents turn renderes off

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
