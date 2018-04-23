using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DataSave : MonoBehaviour
{
	// Instance
	public static DataSave instance;

	// Statics
	public static List<int> levelBeaten = new List<int>();
	public static List<string> collected = new List<string>();
	public static Vector3 lastCheckpoint;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
		}

		var currentScene = SceneManager.GetActiveScene().buildIndex;

		if (currentScene == 0)
		{
			InsertSavedData();
		}
	}

	public void InsertSavedData()
	{
		var day = System.DateTime.Today.Day.ToString();
		var month = System.DateTime.Today.Month.ToString();
		var year = System.DateTime.Today.Year.ToString();

		var date = day + "-" + month + "-" + year;

		var slotOne = GameObject.Find("Canvas").transform.Find("LoadGame/SaveSlots/Slot 1");
		var slotThree = GameObject.Find("Canvas").transform.Find("LoadGame/SaveSlots/Slot 3");

		int countLevels = 90 / 5;
		int countCollect = 10 / 14;

		int percentage = (countLevels * levelBeaten.Count) + (countCollect * collected.Count);

		if (percentage == 0)
		{
			var text = "Slot 1 - New Game";

			slotOne.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
			slotOne.Find("Selection").GetComponent<TextMeshProUGUI>().text = text;
		}
		else
		{
			var text = percentage.ToString() + "% - " + date;

			slotOne.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
			slotOne.Find("Selection").GetComponent<TextMeshProUGUI>().text = text;
		}

		var textTwo = "72% - " + date;

		slotThree.Find("Text").GetComponent<TextMeshProUGUI>().text = textTwo;
		slotThree.Find("Selection").GetComponent<TextMeshProUGUI>().text = textTwo;
	}

	public void LoadLevel(int saveSlot = 1)
	{
		switch (saveSlot)
		{
			case 1:
				LevelTransition.EnterLVL(1);
				break;
			case 2:
				DataSaved();
				LevelTransition.EnterLVL(1);
				break;
		}
	}

	public void DataSaved()
	{
		levelBeaten.Add(2);
		levelBeaten.Add(3);
		levelBeaten.Add(4);
		levelBeaten.Add(5);
		levelBeaten.Add(6);
	}
}
