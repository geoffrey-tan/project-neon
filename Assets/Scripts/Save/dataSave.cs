using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}
}
