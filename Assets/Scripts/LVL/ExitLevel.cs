using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
	public void EnterLVL(int enter)
	{
		DataSave.lastCheckpoint = new Vector3(0, 0, 0);

		SceneManager.LoadScene(enter, LoadSceneMode.Single);
	}

	public void ExitLVL()
	{
		DataSave.lastCheckpoint = new Vector3(0, 0, 0);

		SceneManager.LoadScene("Hub", LoadSceneMode.Single);
	}
}
