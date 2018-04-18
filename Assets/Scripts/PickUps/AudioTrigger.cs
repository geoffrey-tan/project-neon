using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	// Components
	private GameObject self;
	private AudioSource messageAudio;
	private AudioSource dialogAudio;

	//Scripts
	private AudioMessage GetAudioMessage;
	private Dialogs GetDialogs;

	// Variables
	private AudioClip thisAudioClip;
	public string dialogID;
	private string thisName;
	private float thisLength;
	private float timeLeft;

	void Start()
	{
		self = gameObject;
		messageAudio = Camera.main.gameObject.transform.Find("Audio/AudioMessage").GetComponent<AudioSource>();
		dialogAudio = Camera.main.gameObject.transform.Find("Audio/Dialogs").GetComponent<AudioSource>();

		GetAudioMessage = Camera.main.gameObject.transform.Find("Audio/AudioMessage").GetComponent<AudioMessage>();
		GetDialogs = Camera.main.gameObject.transform.Find("Audio/Dialogs").GetComponent<Dialogs>();

		thisName = transform.name;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (self.gameObject.CompareTag("PickUp"))
			{
				dialogAudio.volume = 0f;
				messageAudio.volume = 1f;

				PlayAudioMessage(thisName);

				transform.gameObject.SetActive(false);
			}

			if (self.gameObject.CompareTag("Dialog"))
			{
				if (dialogID != "")
				{
					dialogAudio.volume = 1f;
					messageAudio.volume = 0f;

					GetDialogs.PlayDialog(dialogID);

					transform.gameObject.SetActive(false);
				}
				else
				{
					Debug.Log("Missing Dialog ID for: " + thisName); // Debug if ID is missing
				}
			}
		}
	}

	void PlayAudioMessage(string number)
	{
		GetAudioMessage.ConvertToAudio(number);
	}
}
