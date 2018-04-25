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
	public AudioClip hit0;
	public AudioClip death0;
	public AudioClip mindcontrol;
	public AudioClip distract;
	public AudioClip interact;

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

	public void PlaySFX(string audio, GameObject target = null) // Public function
	{
		switch (audio)
		{
			case "alert0":
				player.GetComponent<AudioSource>().PlayOneShot(alert0);
				break;
			case "gun0":
				player.GetComponent<AudioSource>().PlayOneShot(gun0);
				break;
			case "gun1":
				target.GetComponent<AudioSource>().PlayOneShot(gun0);
				break;
			case "hit0":
				player.GetComponent<AudioSource>().PlayOneShot(hit0);
				break;
			case "death0":
				player.GetComponent<AudioSource>().PlayOneShot(death0);
				break;
			case "mindcontrol":
				player.GetComponent<AudioSource>().PlayOneShot(mindcontrol);
				break;
			case "distract":
				player.GetComponent<AudioSource>().PlayOneShot(distract);
				break;
			case "interact":
				player.GetComponent<AudioSource>().PlayOneShot(interact);
				break;
		}
	}
}
