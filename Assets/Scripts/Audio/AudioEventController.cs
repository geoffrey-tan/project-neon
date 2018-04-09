using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventController : MonoBehaviour
{
	// gameObjects
	private GameObject player;

	// Unity Objects
	public AudioClip footSteps;
	public AudioClip alert0;
	public AudioClip gun0;

	// Component
	private AudioSource audioSource;

	void Start()
	{
		player = GameObject.Find("Player");
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySFXEvent(int number) // https://docs.unity3d.com/540/Documentation/Manual/animeditor-AnimationEvents.html
	{
		if (number == 1)
		{
			audioSource.PlayOneShot(footSteps);
		}
	}

	public void PlaySFX(string audio)
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
