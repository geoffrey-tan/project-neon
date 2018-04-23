using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
	public static void EnterLVL(int enter)
	{
		if (!DataSave.levelBeaten.Contains(enter))
		{
			DataSave.lastCheckpoint = new Vector3(0, 0, 0);

			SceneManager.LoadScene(enter, LoadSceneMode.Single);
		}
	}

	public static void ExitLVL(int exit)
	{
		DataSave.levelBeaten.Add(exit);
		DataSave.lastCheckpoint = new Vector3(0, 0, 0);

		SceneManager.LoadScene("Hub", LoadSceneMode.Single);
	}
}
