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
				PlayAudioMessage(thisName);
			}
			else if (self.gameObject.CompareTag("Dialog"))
			{
				if (dialogID != "")
				{
					messageAudio.Stop();
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
		thisAudioClip = GetAudioMessage.ReturnMessage(number);

		if (!messageAudio.isPlaying && !dialogAudio.isPlaying)
		{
			messageAudio.clip = thisAudioClip;
			messageAudio.Play();

			transform.gameObject.SetActive(false);
		}
		else if (dialogAudio.isPlaying)
		{
			DelayedPlay(dialogAudio);
		}
		else
		{
			DelayedPlay(messageAudio);
		}
	}

	void DelayedPlay(AudioSource audioSource)
	{
		Renderer[] renders = self.transform.GetComponentsInChildren<Renderer>();

		for (int i = 0; i < renders.Length; i++)
		{
			renders[i].enabled = false;
		}

		thisLength = audioSource.clip.length;
		timeLeft = thisLength - audioSource.time;

		StartCoroutine(AudioPlayList(timeLeft));
	}

	IEnumerator AudioPlayList(float timer)
	{
		yield return new WaitForSeconds(timer + 1f);

		messageAudio.clip = thisAudioClip;
		messageAudio.Play();
		transform.gameObject.SetActive(false);
	}
}
