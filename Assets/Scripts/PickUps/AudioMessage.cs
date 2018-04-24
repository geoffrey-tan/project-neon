using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // https://answers.unity.com/questions/1231225/cannot-change-button-interactable-in-a-unityaction.html
using TMPro;

public class AudioMessage : MonoBehaviour
{
	// Components
	private AudioSource audioSource;

	// Assets
	public AudioClip T1;
	public AudioClip LV0_1, LV0_2, LV0_3;
	public AudioClip LV1_1, LV1_2, LV1_3, LV1_4;
	public AudioClip LV2_1, LV2_2, LV2_3;
	public AudioClip LV3_1, LV3_2, LV3_3;

	private GameObject UI;

	// Lists
	public static List<AudioClip> PlayList = new List<AudioClip>();

	private void Start()
	{
		UI = GameObject.Find("UI").transform.Find("Overlay/AudioPlayer/Text").gameObject;
		audioSource = GetComponent<AudioSource>();
	}

	public void ConvertToAudio(string number)
	{
		switch (number)
		{
			case "AudioMessage (T1)":
				PlayList.Add(T1);
				break;

			case "AudioMessage (LV0_1)":
				PlayList.Add(LV0_1);
				break;
			case "AudioMessage (LV0_2)":
				PlayList.Add(LV0_2);
				break;
			case "AudioMessage (LV0_3)":
				PlayList.Add(LV0_3);
				break;

			case "AudioMessage (LV1_1)":
				PlayList.Add(LV1_1);
				break;
			case "AudioMessage (LV1_2)":
				PlayList.Add(LV1_2);
				break;
			case "AudioMessage (LV1_3)":
				PlayList.Add(LV1_3);
				break;
			case "AudioMessage (LV1_4)":
				PlayList.Add(LV1_4);
				break;

			case "AudioMessage (LV2_1)":
				PlayList.Add(LV2_1);
				break;
			case "AudioMessage (LV2_2)":
				PlayList.Add(LV2_2);
				break;
			case "AudioMessage (LV2_3)":
				PlayList.Add(LV2_3);
				break;

			case "AudioMessage (LV3_1)":
				PlayList.Add(LV3_1);
				break;
			case "AudioMessage (LV3_2)":
				PlayList.Add(LV3_2);
				break;
			case "AudioMessage (LV3_3)":
				PlayList.Add(LV3_3);
				break;
		}

		if (PlayList.Count >= 1)
		{
			StartCoroutine(AudioPlayList(PlayList[0].length));
		}
	}

	public IEnumerator AudioPlayList(float timer)
	{
		audioSource.clip = PlayList[0];
		audioSource.Play(); // Play first audio

		UI.transform.parent.gameObject.SetActive(true);
		UI.GetComponent<TextMeshProUGUI>().text = PlayList[0].name;

		yield return new WaitForSeconds(timer + 1f);

		PlayList.RemoveAt(0); // Remove first audio

		UI.transform.parent.gameObject.SetActive(false);

		if (PlayList.Count != 0)
		{
			StartCoroutine(AudioPlayList(PlayList[0].length)); // Play next audio
		}
	}
}
