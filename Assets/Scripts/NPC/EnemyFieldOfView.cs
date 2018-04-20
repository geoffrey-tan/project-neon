using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
	// Components
	public List<Transform> visibleTargets = new List<Transform>();
	public Transform currentTarget;

	// Scripts
	private EnemyAI GetEnemyAI;
	private MindControl GetMindControl;

	// Variables
	public RaycastHit lastSeen;
	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public float viewRadius; // https://www.youtube.com/watch?v=rQG9aUWarwE
	[Range(0, 360)]
	public float viewAngle;

	void Start()
	{
		StartCoroutine("FindTargetsWithDelay", .6f);

		GetEnemyAI = transform.GetComponent<EnemyAI>();
		GetMindControl = transform.GetComponent<MindControl>();
	}

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);

			FindVisibleTargets();
		}
	}

	void Update()
	{
		if (GetMindControl.mindControl && Input.GetButton("Distract"))
		{
			Distraction(); // Shoot at enemy
		}
	}

	void FindVisibleTargets()
	{
		visibleTargets.Clear();

		if (GetMindControl.mindControl)
		{
			targetMask = LayerMask.GetMask("Enemy");
		}
		else if (!GetEnemyAI.distracted)
		{
			targetMask = LayerMask.GetMask("Player");
		}

		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;

			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float distToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
				{
					visibleTargets.Add(target);

					if (GetMindControl.mindControl)
					{
						currentTarget = visibleTargets[0]; // Nearby guard
					}

					if (!GetEnemyAI.searching && !GetMindControl.mindControl) // Patrol -> Searching
					{
						GetEnemyAI.Searching(target.transform.position); // Last seen position

					}
					else if (GetEnemyAI.searchWait && !GetEnemyAI.combatStart) // 2f Wait -> Combat
					{
						GetEnemyAI.combatStart = true;
					}

					if (GetEnemyAI.combatStart)
					{
						if (!GetEnemyAI.shot && GetEnemyAI.canShoot)
						{
							GetEnemyAI.Shoot();
						}
					}
				}
			}
		}
	}

	void Distraction()
	{
		if (currentTarget != null)
		{
			GetMindControl.Mindcontrol(false);

			currentTarget.transform.GetComponent<EnemyAI>().Distracted(gameObject);

			transform.GetComponent<EnemyAI>().Distracted(currentTarget.gameObject);
			transform.LookAt(currentTarget.position);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}
