using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

public enum EnemyStates {
	IDLE,
	PATROL,
	DETECTED,
	NEUTRALIZED
}

[RequireComponent(typeof(EnemyMovement))]
public class EnemyBehaviour : MonoBehaviour {

	public EnemyStates CurrentState => currState;

	[SerializeField]
	private EnemyStates currState;

	private EnemyMovement movRef;

	private SpartanTimer patrolTimer;


	private void Start() {
		this.movRef = GetComponent<EnemyMovement>();
		patrolTimer = new SpartanTimer(TimeMode.Fixed);
	}

	private void Update() {
		//Here we check some basic functions of the FSM, mainly, those that are not
		//movement dependant, so things like detecting, dying, combat, etc.
		switch (currState) {
			case EnemyStates.IDLE:
			case EnemyStates.PATROL:
				Detection();
				break;

			case EnemyStates.DETECTED:
				break;

			case EnemyStates.NEUTRALIZED:
				break;
		}
	}

	private void FixedUpdate() {
		//And here we check all physics operations
		switch (currState) {
			case EnemyStates.PATROL:
				Detection();
				//Here we move and stuff
				PatrolAll();
				break;

			case EnemyStates.DETECTED:
				movRef.MoveTowards(EntityFetcher.Instance.Player.transform.position);
				break;

			case EnemyStates.NEUTRALIZED:
				break;
		}
	}

	private void Detection() {
		if (!movRef.LookForPlayer()) return;
		this.currState = EnemyStates.DETECTED;
	}

	private void PatrolAll() {
		//Go through each of the points, patrol them, stop for about a second, and then continue
		//So, if we are within a stopping distance
		Transform post = movRef.PatrolPositions[movRef.CurrentPatrolIndex];
		float currDistanceSqr = SpartanMath.DistanceSqr(transform.position, post.position);
		
		if ((movRef.StoppingDistance * movRef.StoppingDistance) <= currDistanceSqr)
			movRef.CurrentPatrolIndex++;

		movRef.MoveTowards(post.position);
	}

}
