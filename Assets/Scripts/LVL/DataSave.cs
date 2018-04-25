using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DataSave : MonoBehaviour
{
	// Instance
	public static DataSave instance;

	// Statics   
	public static Vector3 lastCheckpoint;
	public static int lastWaypoint;
	public static bool help;
	public static int nextLevel;
	public static int lifesSaved;

	// Lists
	public static List<int> levelBeaten = new List<int>();
	public static List<string> collected = new List<string>();

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

	void OnLevelWasLoaded(int level)
	{
		if (level == 0)
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
}
