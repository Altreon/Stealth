using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NavMode { idle, wait, preMoveRot, move, postMoveRot, chase, stopChase };

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNav : MonoBehaviour
{

	[Header("Inscribed")]
	public bool             drawGizmos;
	public List<Waypoint>   waypoints;
	public float            speed = 4;
	public float            angularSpeed = 90;

	[Header("Dynamic")]
	[SerializeField]
	private NavMode           _mode = NavMode.wait;
	public int              wpNum = 0;
	public float            pathTime;
	public float            waitUntil;
	[Tooltip("Left=-1, None=0, Right=1")]
	public int              turnDir = 0;

	protected NavMeshAgent  nav;

	public NavMode mode
	{
		get
		{
			return _mode;
		}

		set
		{
			_mode = value;
		}
	}

	void Start()
	{
		nav = GetComponent<NavMeshAgent>();
		nav.stoppingDistance = 0.01f;

		AlertModeManager.alertModeStatusChangeDelegate += AlertModeChange;

		pathTime = 0;
		wpNum = 0;
		MoveToWaypoint(0);
	}

	void OnDestroy () {
		AlertModeManager.alertModeStatusChangeDelegate -= AlertModeChange;
	}

	void MoveToNextWaypoint()
	{
		int wpNum1 = wpNum + 1;
		if (wpNum1 >= waypoints.Count)
		{
			wpNum1 = 0;
		}

		MoveToWaypoint(wpNum1);
	}

	void MoveToWaypoint(int num)
	{
		wpNum = num;
		nav.SetDestination(waypoints[wpNum].Position);
		nav.isStopped = true;
		nav.updatePosition = false;

		mode = NavMode.preMoveRot;
	}


	bool RotateTowards(Vector3 goalPos, float deg, bool rotYOnly = true)
	{
		Vector3 delta = goalPos - transform.position;
		if (rotYOnly)
		{
			delta.y = 0;
		}
		Quaternion r0 = transform.rotation;
		Vector3 fwd0 = transform.forward;
		transform.LookAt(transform.position + delta);
		Quaternion r1 = transform.rotation;
		Vector3 fwd1 = transform.forward;
		transform.rotation = Quaternion.RotateTowards(r0, r1, deg);

		Vector3 cross = Vector3.Cross(fwd0,fwd1);
		if (cross.y > 0) {
			turnDir = 1;
		} else if (cross.y < 0) {
			turnDir = -1;
		}

		return (Quaternion.Angle(transform.rotation, r1) < 1);
	}
		
	void FixedUpdate()
	{
		if (!Mathf.Approximately(nav.speed, speed)
			|| !Mathf.Approximately(nav.angularSpeed, angularSpeed) )
		{
			nav.speed = speed;
			nav.angularSpeed = angularSpeed;
		}

		pathTime += Time.fixedDeltaTime;
		turnDir = 0;

		switch (mode)
		{
		case NavMode.preMoveRot:
			Vector3 goalPos = waypoints [wpNum].Position;
			if (RotateTowards(goalPos, angularSpeed * Time.fixedDeltaTime))
			{
				nav.isStopped = false;
				nav.updatePosition = true;
				mode = NavMode.move;
			}
			break;

		case NavMode.move:
			if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
			{
				mode = NavMode.postMoveRot;
			}
			break;

		case NavMode.postMoveRot:
			if (RotateTowards(transform.position + waypoints[wpNum].Forward, angularSpeed * Time.fixedDeltaTime))
			{
				waitUntil = pathTime + waypoints[wpNum].PauseTime;
				mode = NavMode.wait;
			}
			break;

		case NavMode.wait:
			if (pathTime < waitUntil)
			{
				break;
			}
			MoveToNextWaypoint();
			break;

		case NavMode.chase:
			nav.SetDestination(InteractingPlayer.S.transform.position);
			nav.isStopped = false;
			nav.updatePosition = true;

			break;

		case NavMode.stopChase:            
			nav.SetDestination(waypoints[wpNum].Position);
			nav.isStopped = false;
			nav.updatePosition = true;
			mode = NavMode.move;
			break;
		}

	}

	void AlertModeChange (bool alert) {
		if (alert) {
			mode = NavMode.chase;
		} else {
			mode = NavMode.stopChase;
		}
	}


	const string gizmoIconPrefix = "GizmoIcon_128_";
	private void OnDrawGizmos()
	{
		if (!drawGizmos || !Application.isEditor || 
			Application.isPlaying || waypoints.Count < 2)
		{
			return;
		}

		Vector3 p0, p1;
		Vector3 iconDrawLoc;
		List<Vector3> usedIconDrawLocs = new List<Vector3>();
		Gizmos.color = Color.red;
		Vector3 gizmoIconOverlapOffset = Camera.current.transform.right * 0.75f
			- Camera.current.transform.up * 0.25f;

		for (int i = 0; i < waypoints.Count; i++)
		{
			p0 = waypoints[i].Position + Vector3.up;
			p1 = Vector3.up + ((i < waypoints.Count - 1) ? waypoints[i + 1].Position
				: waypoints[0].Position);
			Gizmos.DrawLine(p0, p1);

			if (i < 10)
			{
				iconDrawLoc = p0 + Vector3.up;
				while (usedIconDrawLocs.Contains(iconDrawLoc))
				{
					iconDrawLoc += gizmoIconOverlapOffset;
				}
				usedIconDrawLocs.Add(iconDrawLoc);
				Gizmos.DrawIcon(iconDrawLoc, gizmoIconPrefix + i, true);
			}
		}
	}
}
