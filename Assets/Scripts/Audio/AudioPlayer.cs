using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // https://answers.unity.com/questions/1231225/cannot-change-button-interactable-in-a-unityaction.html
using UnityEngine.UI;
using TMPro;

public class AudioPlayer : MonoBehaviour
{
	// GameObjects
	private GameObject UI;

	// Arrays
	private RectTransform[] audioLogs;

	void OnEnable()
	{
		InsertAudio();
	}

	void InsertAudio()
	{
		UI = GameObject.Find("UI");
		audioLogs = UI.transform.Find("Menu/AudiologsPause/Audiologs").GetComponentsInChildren<RectTransform>();

		if (DataSave.collected.Count != 0)
		{
			for (int i = 1; i < audioLogs.Length; i++)
			{
				if (DataSave.collected.Contains(audioLogs[i].name))
				{
					audioLogs[i].Find("Text").GetComponent<TextMeshProUGUI>().text = audioLogs[i].name;
					audioLogs[i].Find("Selection").GetComponent<TextMeshProUGUI>().text = audioLogs[i].name;
					audioLogs[i].GetComponent<Button>().interactable = true;
				}
			}
		}
	}
}