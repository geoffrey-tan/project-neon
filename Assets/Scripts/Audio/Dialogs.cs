using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs : MonoBehaviour
{
	// Components
	private AudioSource audioSource;

	// Scripts
	private Dialogs GetDialogs;

	// Assets
	public AudioClip P1_1, P1_2, P1_3, P1_4, P1_5;

	public AudioClip T2_1_1, T2_1_2;
	public AudioClip T2_2_1, T2_2_2;
	public AudioClip T2_3_1, T2_3_2;
	public AudioClip T2_4_1, T2_4_2;
	public AudioClip T2_5_5, T2_5_6, T2_5_7, T2_5_9, T2_5_10, T2_5_11, T2_5_13, T2_5_14;

	// Lists
	public List<AudioClip> PlayList;

	// Variables
	public Coroutine coroutine;
	private string currentDialog;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayDialog(string dialog)
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine); // https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html
			coroutine = null;
		}

		audioSource.Stop();

		PlayList.Clear();

		switch (dialog)
		{
			case "P-1": // Cutscene
				PlayList.Add(P1_1);
				PlayList.Add(P1_2);
				PlayList.Add(P1_3);
				PlayList.Add(P1_4);
				PlayList.Add(P1_5);
				break;

			case "T-1": // Start
				PlayList.Add(T2_1_1);
				PlayList.Add(T2_1_2);
				break;
			case "T-2": // Sneak
				PlayList.Add(T2_2_1);
				PlayList.Add(T2_2_2);
				break;
			case "T-3": // Checkpoint
				PlayList.Add(T2_3_1);
				PlayList.Add(T2_3_2);
				break;
			case "T-4": // Stairs
				PlayList.Add(T2_4_1);
				PlayList.Add(T2_4_2);
				break;

			case "T-5-5": // Distract
				PlayList.Add(T2_5_5);
				break;
			case "T-5-6": // Distract -> Completed
				PlayList.Add(T2_5_6);
				break;
			case "T-5-7": // Mind-Control
				PlayList.Add(T2_5_7);
				break;
			case "T-5-9": // Mind-Control -> Completed
				PlayList.Add(T2_5_9);
				break;
			case "T-5-10": // Distract 
				PlayList.Add(T2_5_10);
				break;
			case "T-5-11": // Pick-Up
				PlayList.Add(T2_5_11);
				break;
			case "T-5-13": // Enter room
				PlayList.Add(T2_5_13);
				break;
			case "T-5-14": // Cutscene
				PlayList.Add(T2_5_14);
				break;
		}

		coroutine = StartCoroutine(AudioPlayList(PlayList[0].length));
	}

	IEnumerator AudioPlayList(float timer)
	{
		audioSource.clip = PlayList[0];
		audioSource.Play(); // Play first audio

		yield return new WaitForSeconds(timer + 1f);

		PlayList.RemoveAt(0); // Remove first audio

		if (PlayList.Count != 0)
		{
			coroutine = StartCoroutine(AudioPlayList(PlayList[0].length)); // Play next audio
		}
		else
		{
			coroutine = null;
		}
	}
}
