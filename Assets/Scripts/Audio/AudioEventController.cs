using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventController : MonoBehaviour
{
	// Components
	private GameObject player;
	private AudioSource audioSource;

	// Assets
	public AudioClip footSteps;
	public AudioClip alert0;
	public AudioClip gun0;

	void Start()
	{
		player = GameObject.Find("Player");
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySFXEvent(int number) // https://docs.unity3d.com/540/Documentation/Manual/animeditor-AnimationEvents.html
	{
		switch (number)
		{
			case 1:
				audioSource.PlayOneShot(footSteps);
				break;
		}
	}

	public void PlaySFX(string audio) // Public function
	{
		switch (audio)
		{
			case "alert0":
				player.GetComponent<AudioSource>().PlayOneShot(alert0);
				break;
			case "gun0":
				player.GetComponent<AudioSource>().PlayOneShot(gun0);
				break;
		}
	}
}
