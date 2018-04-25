using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
	public Sprite crouch, distract, interact, jump, mind;

	public void Hint(string name)
	{
		transform.Find("Controls (Help)").gameObject.SetActive(true);

		switch (name)
		{
			case "Crouch":
				transform.Find("Controls (Help)").GetComponent<Image>().sprite = crouch;
				break;
			case "Distract":
				transform.Find("Controls (Help)").GetComponent<Image>().sprite = distract;
				break;
			case "Interact":
				transform.Find("Controls (Help)").GetComponent<Image>().sprite = interact;
				break;
			case "Jump":
				transform.Find("Controls (Help)").GetComponent<Image>().sprite = jump;
				break;
			case "Mind":
				transform.Find("Controls (Help)").GetComponent<Image>().sprite = mind;
				break;
		}
	}
}
