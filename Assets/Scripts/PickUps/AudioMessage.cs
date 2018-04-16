using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMessage : MonoBehaviour
{
	// Assets
	public AudioClip message1, message2, message3;
	public AudioClip returnedAudio;

	public AudioClip ReturnMessage(string number)
	{
		switch (number)
		{
			case "AudioMessage (1)":
				returnedAudio = message1;
				break;
			case "AudioMessage (2)":
				returnedAudio = message2;
				break;
		}

		return returnedAudio;
	}
}
