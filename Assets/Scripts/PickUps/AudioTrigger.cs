using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	// Components
	private GameObject self;
	public static AudioSource messageAudio;

	//Scripts
	private AudioMessage GetAudioMessage;

	// Variables
	private AudioClip thisAudioClip;
	public string dialogID;
	private float thisLength;
	private float timeLeft;

	void Start()
	{
		self = gameObject;
		messageAudio = GameObject.Find("Audio").transform.Find("AudioMessage").GetComponent<AudioSource>();

		GetAudioMessage = GameObject.Find("Audio").transform.Find("AudioMessage").GetComponent<AudioMessage>();

		if (DataSave.collected.Contains(name))
		{
			gameObject.SetActive(false); // https://bytes.com/topic/c-sharp/answers/437626-cannot-convert-string-system-predicate-string
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (self.gameObject.CompareTag("PickUp"))
			{
				messageAudio.volume = 1f;

				PlayAudioMessage(name);

				DataSave.collected.Add(name);

				transform.gameObject.SetActive(false);
			}
		}
	}

	void PlayAudioMessage(string number)
	{
		GetAudioMessage.ConvertToAudio(number);
	}
}
