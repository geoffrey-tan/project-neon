using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
	public float viewRadius; // https://www.youtube.com/watch?v=rQG9aUWarwE
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	public List<Transform> visibleTargets = new List<Transform>();

	public RaycastHit lastSeen;

	private EnemyAI enemyAI;

	void Start()
	{
		StartCoroutine("FindTargetsWithDelay", .6f);

		enemyAI = transform.GetComponent<EnemyAI>();

	}

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);

			FindVisibleTargets();
		}
	}

	void FindVisibleTargets()
	{
		visibleTargets.Clear();

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

					if (!enemyAI.searching) // Patrol -> Searching
					{
						Debug.Log("Searching");
						enemyAI.Searching(target.transform.position); // Last seen position

					}
					else if (enemyAI.searchWait && !enemyAI.combatStart) // 2f Wait -> Combat
					{
						enemyAI.combatStart = true;
					}

					if (enemyAI.combatStart)
					{
						if (!enemyAI.shot && enemyAI.canShoot)
						{
							Debug.Log("Shoot called");
							enemyAI.shot = true;

							enemyAI.Shoot();
						}

					}

				}
			}
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
