using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFieldOfView))]
public class FieldOfViewEditor : Editor
{
	void OnSceneGUI() // https://www.youtube.com/watch?v=rQG9aUWarwE
	{
		EnemyFieldOfView fov = (EnemyFieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

		Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
		Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

		Handles.color = Color.red;

		foreach (Transform visibleTargets in fov.visibleTargets)
		{
			Handles.DrawLine(fov.transform.position, visibleTargets.position);
		}
	}
}
