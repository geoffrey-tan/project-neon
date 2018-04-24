using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	// Components
	private GameObject self;
	public static AudioSource messageAudio;
	public static AudioSource dialogAudio;

	//Scripts
	private AudioMessage GetAudioMessage;
	private Dialogs GetDialogs;

	// Variables
	private AudioClip thisAudioClip;
	private float thisLength;
	private float timeLeft;

	void Start()
	{
		self = gameObject;
		messageAudio = GameObject.Find("Audio").transform.Find("AudioMessage").GetComponent<AudioSource>();
		dialogAudio = GameObject.Find("Audio").transform.Find("Dialogs").GetComponent<AudioSource>();

		GetAudioMessage = GameObject.Find("Audio").transform.Find("AudioMessage").GetComponent<AudioMessage>();
		GetDialogs = GameObject.Find("Audio").transform.Find("Dialogs").GetComponent<Dialogs>();

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
			if (self.gameObject.CompareTag("Dialog"))
			{
				messageAudio.volume = 0f;
				dialogAudio.volume = 1f;

				GetDialogs.PlayDialog(name);

				transform.gameObject.SetActive(false);
			}
		}
	}

	void PlayAudioMessage(string number)
	{
		GetAudioMessage.ConvertToAudio(number);
	}
}
