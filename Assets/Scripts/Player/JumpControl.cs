using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum JumpStates {
	SIGNALED,
	JUMPING,
	FALLING,
	NONE = -1
};


public class JumpControl : MonoBehaviour {
	/// <summary>
	/// This is the state of the controller isolated in a jumping context
	/// (i.e. A falling state will not be triggered each time the character falls, but rather, each time
	/// the character falls after having jumped previously)
	/// </summary>
	public JumpStates JumpState { get; private set; }

	[SerializeField]
	private float jumpVelocity;

	[SerializeField]
	private float fallModifier;

	private Movement movRef;
	private DashControl dashRef;

	private void Start() {
		this.movRef = GetComponent<Movement>();
		this.dashRef = GetComponent<DashControl>();
		JumpState = JumpStates.NONE;
	}

	private void Update() {
		HandleInput();
		if ((JumpState == JumpStates.JUMPING || JumpState == JumpStates.FALLING) && movRef.Grounded)
			JumpState = JumpStates.NONE;
	}

	private void FixedUpdate() {
		switch (JumpState) {
			case JumpStates.SIGNALED:
				Jump();
				break;
			case JumpStates.FALLING:
			case JumpStates.JUMPING:
				ApplyVelocityModifiers();
				break;
		}
	}

	private void HandleInput() {
		if (AbleToJump() && Input.GetButtonDown("Jump")) {
			JumpState = JumpStates.SIGNALED;
		}
	}

	public void Jump() {
		this.JumpState = JumpStates.JUMPING;
		movRef.Rig.AddForce(Vector2.up * jumpVelocity, ForceMode.VelocityChange);
	}
	public bool AbleToJump() {
		return movRef.Grounded && JumpState != JumpStates.SIGNALED && !dashRef.DashDurationTimer.Started;
	}

	private void ApplyVelocityModifiers() {
		if (movRef.Rig.velocity.y < 0f) {
			float velY = (jumpVelocity) * fallModifier * Time.fixedDeltaTime;
			Vector2 velVector = new Vector2(movRef.Rig.velocity.x, movRef.Rig.velocity.y - velY);
			movRef.Rig.velocity = velVector;
		}
	}

}
