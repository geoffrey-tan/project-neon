using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelection : MonoBehaviour
{
	void Update()
	{
		if (Input.GetButtonDown("Cancel") && (name == "LoadGame" || name == "Audiologs"))
		{
			transform.parent.Find("Menu").gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
	}

	void OnEnable() // https://answers.unity.com/questions/372752/does-finction-start-or-awake-run-when-the-object-o.html
	{
		StartCoroutine(SelectButton(name)); // https://answers.unity.com/questions/1142958/buttonselect-doesnt-highlight.html      
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
		}

	}
}
