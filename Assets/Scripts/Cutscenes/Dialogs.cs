using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs : MonoBehaviour
{
	//Statics
	public static bool mindControl;
	public static bool distract;

	// Components
	private AudioSource audioSource;

	// Prologue OW
	public AudioClip P1_1, P1_2, P1_3, P1_4, P1_5;

	// Tutorial
	public AudioClip T2_1_1, T2_1_2;
	public AudioClip T2_2_1, T2_2_2;
	public AudioClip T2_3_1, T2_3_2;
	public AudioClip T2_4_1, T2_4_2;
	public AudioClip T2_5_5, T2_5_6, T2_5_7, T2_5_9, T2_5_10, T2_5_11, T2_5_13, T2_5_14;

	// Tutorial Done
	public AudioClip NT_A_1, NT_L_1, NT_A_2, NT_L_2, NT_A_3, NT_L_3;

	// Level 1 OW
	public AudioClip VLVL1_S_1, VLVL1_L_1, VLVL1_S_2, VLVL1_S_3;

	// Level 1
	public AudioClip LVL1_R_4; // Target
	public AudioClip LVL1_L_1; // Kill
	public AudioClip LVL1_L_2, LVL1_R_5; // Live

	// If spare life Cutscene
	public AudioClip S_A_1, S_L_1, S_A_2, S_L_2, S_A_3, S_L_3;

	// Level 2 OW
	public AudioClip VLVL2_S_1;

	// Level 2
	public AudioClip LVL2_R_4; // Target
	public AudioClip LVL2_L_1; // Kill
	public AudioClip LVL2_L_2, LVL2_R_5, LVL2_L_3; // Live

	// Level 3 OW
	public AudioClip VLVL3_S_1;

	// Level 3
	public AudioClip LVL3_R_4; // Target
	public AudioClip LVL3_L_1; // Kill
	public AudioClip LVL3_L_2, LVL3_R_5; // Live

	// Level Boss OW Liberty
	public AudioClip VBL_S_1, VBL_A_1, VBL_S_2;

	// Level Boss OW The Resistance
	public AudioClip VBL_S_3, VBL_L_1, VBL_S_4;

	// Level Boss
	public AudioClip BL_L_1, BL_S_1, BL_A_1, BL_S_2, BL_A_2, BL_S_3, BL_A_3, BL_L_2;

	// Lists
	public static List<AudioClip> PlayList = new List<AudioClip>();
	public List<float> list;

	// Variables
	public Coroutine coroutine;
	private string currentDialog;
	public bool finalCutscene;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public List<float> PlayDialog(string dialog)
	{
		float count = 0;

		if (coroutine != null)
		{
			StopCoroutine(coroutine); // https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html
									  //coroutine = null;
		}

		PlayList.Clear();

		audioSource.Stop();

		switch (dialog)
		{
			// ### Prologue ###
			case "P-1": // Cutscene            
				PlayList.Add(P1_1);
				PlayList.Add(P1_2);
				PlayList.Add(P1_3);
				PlayList.Add(P1_4);
				PlayList.Add(P1_5);
				break;

			// ### Tutorial ###
			case "T-1": // Start            
				CutsceneEvents.interact = true;
				PlayList.Add(T2_1_1);
				PlayList.Add(T2_1_2);
				break;
			case "T-2": // Sneak            
				CutsceneEvents.interact = true;
				PlayList.Add(T2_2_1);
				PlayList.Add(T2_2_2);
				break;
			case "T-3": // Checkpoint Trigger            
				PlayList.Add(T2_3_1);
				PlayList.Add(T2_3_2);
				break;
			case "T-4": // Stairs Trigger            
				CutsceneEvents.interact = true;
				PlayList.Add(T2_4_1);
				PlayList.Add(T2_4_2);
				break;
			case "T-5-5": // Distract            
				CutsceneEvents.interact = true;
				distract = true;
				PlayList.Add(T2_5_5);
				break;

			// Triggers
			case "T-5-6": // Distract -> Completed           
				PlayList.Add(T2_5_6);
				break;
			case "T-5-7": // Mind-Control  
				mindControl = true;
				PlayList.Add(T2_5_7);
				break;
			case "T-5-9": // Mind-Control -> Completed            
				PlayList.Add(T2_5_9);
				break;
			case "T-5-10": // Distract            
				PlayList.Add(T2_5_10);
				break;

			case "T-5-11": // Cutscene - Pick-Up            
				CutsceneEvents.interact = true;
				PlayList.Add(T2_5_11);
				break;
			case "T-5-13": // Cutscene - Enter room            
				CutsceneEvents.interact = true;
				PlayList.Add(T2_5_13);
				break;

			// ### Tutorial Target ###
			case "T-5-14": // Cutscene Target            
				PlayList.Add(T2_5_14);
				break;

			// ### Target Follow-Up ###
			case "NT-A-1":
				PlayList.Add(NT_A_1);
				PlayList.Add(NT_L_1);
				PlayList.Add(NT_A_2);
				PlayList.Add(NT_L_2);
				PlayList.Add(NT_A_3);
				PlayList.Add(NT_L_3);
				break;

			// ### Level 1 Prologue ###
			case "VLVL1_S_1":
				PlayList.Add(VLVL1_S_1);
				PlayList.Add(VLVL1_L_1);
				PlayList.Add(VLVL1_S_2);
				PlayList.Add(VLVL1_S_3);
				break;

			// ### Level 1 Target ###
			case "LVL1_R_4": // Cutscene Target            
				PlayList.Add(LVL1_R_4);
				break;

			// ### Target Follow-Up ###
			case "LVL1_L_1": // Kill            
				PlayList.Add(LVL1_L_1);
				break;
			case "LVL1_L_2": // Live           
				PlayList.Add(LVL1_L_2);
				PlayList.Add(LVL1_R_5);
				break;

			// ### Level 2 Prologue ###
			case "VLVL2-S-1":
				PlayList.Add(VLVL2_S_1);
				break;

			// ### Level 2 Target ###
			case "LVL2_R_4": // Cutscene Target            
				PlayList.Add(LVL2_R_4);
				break;

			// ### Target Follow-Up ###
			case "LVL2_L_1": // Kill            
				PlayList.Add(LVL2_L_1);
				break;
			case "LVL2_L_2": // Live           
				PlayList.Add(LVL2_L_2);
				PlayList.Add(LVL2_R_5);
				PlayList.Add(LVL2_L_3);
				break;

			// ### Level 3 Prologue ###
			case "VLVL3_S_1":
				PlayList.Add(VLVL3_S_1);
				break;

			// ### Level 3 Target ###
			case "LVL3_R_4": // Cutscene Target            
				PlayList.Add(LVL3_R_4);
				break;

			// ### Target Follow-Up ###
			case "LVL3_L_1": // Kill            
				PlayList.Add(LVL3_L_1);
				break;
			case "LVL3_L_2": // Live           
				PlayList.Add(LVL3_L_2);
				PlayList.Add(LVL3_R_5);
				break;

			// ### Level Boss Prologue ###
			case "BL_L_1":
				finalCutscene = true;

				PlayList.Add(BL_L_1);
				PlayList.Add(BL_S_1);
				PlayList.Add(BL_A_1);
				PlayList.Add(BL_S_2);
				PlayList.Add(BL_A_2);
				PlayList.Add(BL_S_3);
				PlayList.Add(BL_A_3);
				PlayList.Add(BL_L_2);
				break;
		}

		coroutine = StartCoroutine(AudioPlayList(PlayList[0].length));

		for (int i = 0; i < PlayList.Count; i++)
		{
			count += PlayList[i].length;
		}

		count += (PlayList.Count / 2);

		list.Add(PlayList[0].length); // Length first audio
		list.Add(count); // Length all audio

		return list;
	}

	IEnumerator AudioPlayList(float timer)
	{
		audioSource.clip = PlayList[0];
		audioSource.Play(); // Play first audio

		yield return new WaitForSeconds(timer + 0.5f);

		PlayList.RemoveAt(0); // Remove first audio

		if (PlayList.Count != 0)
		{
			coroutine = StartCoroutine(AudioPlayList(PlayList[0].length)); // Play next audio
		}
		else
		{
			if (!finalCutscene)
			{
				coroutine = null;
			}
			else
			{

			}

		}


	}
}
