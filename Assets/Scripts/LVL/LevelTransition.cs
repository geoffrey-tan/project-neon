using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
	public void LoadLevel(int saveSlot = 1)
	{
		switch (saveSlot)
		{
			case 1:
				EnterLVL(1);
				break;
			case 2:
				DataSaved();
				EnterLVL(1);
				break;
		}
	}

	public void DataSaved()
	{
		DataSave.levelBeaten.Add(2);
		DataSave.levelBeaten.Add(3);
		DataSave.levelBeaten.Add(4);
		DataSave.levelBeaten.Add(5);
		DataSave.levelBeaten.Add(6);
	}

	public static void EnterLVL(int enter)
	{
		DataSave.help = false;
		DataSave.lastCheckpoint = new Vector3(0, 0, 0);

		SceneManager.LoadScene(enter, LoadSceneMode.Single);
	}

	public static void ExitLVL(int exit)
	{
		DataSave.levelBeaten.Add(exit);
		DataSave.nextLevel = exit;
		DataSave.help = false;
		DataSave.lastCheckpoint = new Vector3(0, 0, 0);

		SceneManager.LoadScene("Hub", LoadSceneMode.Single);
	}
}
