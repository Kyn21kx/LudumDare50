using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour {

	public Rigidbody Rig { get; private set; }
	
	public Transform[] PatrolPositions => patrolPositions;

	public float DetectionRadius => detectionRadius;

	public float PatrolWaitTime => patrolWaitTime;

	public float StoppingDistance => stoppingDistance;

	public int CurrentPatrolIndex {
		get => this.currPatrolIndex;
		set => this.currPatrolIndex = value < PatrolPositions.Length ? value : 0;
	}

	[SerializeField]
	private Transform[] patrolPositions;

	[SerializeField]
	private float patrolWaitTime;

	[SerializeField]
	private float stoppingDistance;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float detectionRadius;

	private int currPatrolIndex;

	private void Start() {
		this.Rig = GetComponent<Rigidbody>();
		this.Rig.useGravity = false;
	}

	public void MoveTowards(Vector2 target) {
		//Move towards a target, for now this is a pretty simple movement, but we'll incorporate A* down the line
		Vector3 nextPos = SpartanMath.Lerp(transform.position, target, speed * Time.fixedDeltaTime);
		nextPos.z = transform.position.z;
		this.Rig.MovePosition(nextPos);
		//Do some animation, and probably VFX stuff here, idk
	}

	public bool LookForPlayer() {
		//Find the player based on our radius, maybe doing a sphere cast or smth
		int targetMask = 1 << LayerMask.NameToLayer("Player");
		return Physics.OverlapSphere(transform.position, detectionRadius, targetMask).Length > 0;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}

}
