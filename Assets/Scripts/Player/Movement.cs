using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(JumpControl))]
[RequireComponent(typeof(DashControl))]
public class Movement : MonoBehaviour {
	
	#region Properties
	public Rigidbody Rig { get; private set; }
	public float PrevDirection { get; private set; }
	public bool Grounded => Rig.velocity.y == 0f;	
	#endregion

	#region Editor variables
	[SerializeField]
	private float speed;

	[SerializeField]
	private float accelerationBoostFactor;

	[SerializeField]
	private float gravityModifier;

	#endregion

	#region Fields

	private float blend;

	private float currDirection;

	public DashControl dashRef;

	#endregion

	private void Start() {
		Rig = GetComponent<Rigidbody>();
		blend = 0f;
		PrevDirection = 1f;
	}

	private void Update() {
		HandleInput();
		if (currDirection == 0f) blend = 0f;
	}

	private void FixedUpdate() {
		Move();
	}

	private void ModGravity(ref Vector2 velocity) {
		if (Grounded) return;
		velocity.y -= gravityModifier;
	}

	private void Move() {
		blend += Time.fixedDeltaTime * accelerationBoostFactor;
		float velocityX = SpartanMath.Lerp(0f, currDirection * speed, blend);
		SpartanMath.Clamp(ref velocityX, -speed, speed);
		if (dashRef.DashDurationTimer.Started) velocityX = 0f;
		Vector2 velVector = new Vector2(velocityX, Rig.velocity.y);
		ModGravity(ref velVector);
		Rig.velocity = velVector;
	}

	private void HandleInput() {
		currDirection = Input.GetAxisRaw("Horizontal");
		if (currDirection != 0f)
			PrevDirection = currDirection;
	}

}
