using Auxiliars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class DashControl : MonoBehaviour {

	[SerializeField]
	private float dashSpeed;

	[SerializeField]
	private float dashCooldown;

	[SerializeField]
	private float dashDuration;

	private SpartanTimer dashTimer;

	public SpartanTimer DashDurationTimer => dashDurationTimer;

	private bool dashedAirbone;

	private Movement movRef;

	private SpartanTimer dashDurationTimer;

	private void Start() {
		movRef = GetComponent<Movement>();
		movRef.dashRef = this;
		//Instantiate 2 timers, one for the cooldown, and one for the dashing time
		dashTimer = new SpartanTimer(TimeMode.Framed);
		dashDurationTimer = new SpartanTimer(TimeMode.Fixed);
	}

	private void Update() {
		HandleInput();
		RestoreDashWhenGrounded();
	}

	private void FixedUpdate() {
		if (dashDurationTimer.Started)
			Dash();
	}

	private void HandleInput() {
		if (Input.GetButtonDown("Dash") && AbleToDash()) {
			dashDurationTimer.Start();
		}
	}

	private void Dash() {
		float currTime = dashDurationTimer.GetCurrentTime(TimeScaleMode.Seconds);
		if (currTime >= dashDuration) {
			dashDurationTimer.Stop();
			this.dashedAirbone = !movRef.Grounded;
			dashTimer.Start();
			return;
		}
		movRef.Rig.AddForce(Vector3.right * Mathf.Sign(movRef.PrevDirection) * dashSpeed * 10f * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

	private void RestoreDashWhenGrounded() {
		if (movRef.Grounded) this.dashedAirbone = false;
	}

	public bool AbleToDash() {
		if (!dashTimer.Started) return true;
		float timePassed = dashTimer.GetCurrentTime(TimeScaleMode.Seconds);
		return (timePassed >= dashCooldown && !dashedAirbone && !dashDurationTimer.Started);
	}

}
