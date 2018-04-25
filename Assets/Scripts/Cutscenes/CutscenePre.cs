using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutscenePre : MonoBehaviour
{
	public VideoClip prologue, level1, level2, level3, bossRes, bossLib;
	private GameObject UI;
	public int nextLevel;

	// Use this for initialization
	void Start()
	{
		UI = GameObject.Find("UI/Cutscenes");

		nextLevel = DataSave.nextLevel;

		PlayVideo(nextLevel + 1);
	}

	public void PlayVideo(int level)
	{
		switch (level)
		{
			case 1:
				UI.GetComponent<VideoPlayer>().clip = prologue;
				break;
			case 3:
				UI.GetComponent<VideoPlayer>().clip = level1;
				break;
			case 4:
				UI.GetComponent<VideoPlayer>().clip = level2;
				break;
			case 5:
				UI.GetComponent<VideoPlayer>().clip = level3;
				break;
			case 6:
				UI.GetComponent<VideoPlayer>().clip = bossRes;
				break;
			case 7:
				UI.GetComponent<VideoPlayer>().clip = bossLib;
				break;
		}

		UI.GetComponent<VideoPlayer>().Play();

		float videoLength = (float)UI.GetComponent<VideoPlayer>().clip.length;

		StartCoroutine(VideoPlayer(videoLength));
	}

	IEnumerator VideoPlayer(float time)
	{
		yield return new WaitForSeconds(time);

		if (nextLevel == 0)
		{
			LevelTransition.EnterLVL(nextLevel + 2);
		}
		else
		{
			LevelTransition.EnterLVL(nextLevel + 1);
		}
	}
}
