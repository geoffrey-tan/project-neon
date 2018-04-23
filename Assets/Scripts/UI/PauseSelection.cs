using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseSelection : MonoBehaviour
{
	void OnEnable() // https://answers.unity.com/questions/372752/does-finction-start-or-awake-run-when-the-object-o.html
	{
		StartCoroutine(SelectButton()); // https://answers.unity.com/questions/1142958/buttonselect-doesnt-highlight.html      
	}

	IEnumerator SelectButton()
	{
		yield return null;

		transform.parent.Find("Player Info").gameObject.SetActive(false);
		transform.parent.Find("Overlay").gameObject.SetActive(false);

		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(transform.Find("Menu/Resume").gameObject, new BaseEventData(EventSystem.current));
	}
}
