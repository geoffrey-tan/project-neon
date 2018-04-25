using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour
{
	void OnEnable() // https://answers.unity.com/questions/372752/does-finction-start-or-awake-run-when-the-object-o.html
	{
		StartCoroutine(SelectButton(name)); // https://answers.unity.com/questions/1142958/buttonselect-doesnt-highlight.html      
	}

	void Update()
	{
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			if (Input.GetButtonDown("Cancel") && (name == "LoadGame" || name == "Audiologs"))
			{
				transform.parent.Find("Menu").gameObject.SetActive(true);
				gameObject.SetActive(false);
			}
			if (DataSave.theEnd)
			{
				if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")) && name == "Menu")
				{
					transform.parent.Find("EndGame").gameObject.SetActive(false);
				}
			}

		}
	}

	IEnumerator SelectButton(string thisName)
	{
		yield return null;

		EventSystem.current.SetSelectedGameObject(null);

		switch (thisName)
		{
			case "Menu":
				EventSystem.current.SetSelectedGameObject(transform.Find("Start").gameObject, new BaseEventData(EventSystem.current));
				break;
			case "LoadGame":
				EventSystem.current.SetSelectedGameObject(transform.Find("SaveSlots/Slot 1").gameObject, new BaseEventData(EventSystem.current));
				break;
			case "Audiologs":
				EventSystem.current.SetSelectedGameObject(transform.Find("Logs/Log 1").gameObject, new BaseEventData(EventSystem.current));
				break;
			case "MenuPause":
				EventSystem.current.SetSelectedGameObject(transform.Find("Resume").gameObject, new BaseEventData(EventSystem.current));
				break;
			case "AudiologsPause":
				EventSystem.current.SetSelectedGameObject(transform.Find("Back").gameObject, new BaseEventData(EventSystem.current));
				break;
		}
	}
}
