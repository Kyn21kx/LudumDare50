using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour {

	public Rigidbody Rig { get; private set; }

	public float DetectionRadius => detectionRadius;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float detectionRadius;

	private void Start() {
		this.Rig = GetComponent<Rigidbody>();
		this.Rig.useGravity = false;
	}

	public void MoveTowards(Vector2 target) {
		//Move towards a target
		Vector2 nextPos = SpartanMath.Lerp(transform.position, target, speed * Time.fixedDeltaTime);
		this.Rig.MovePosition(nextPos);
		//Do some animation, and probably VFX stuff here, idk
	}

	public bool LookForPlayer() {
		//Find the player based on our radius, maybe doing a sphere cast or smth
		return false;
	}

}
