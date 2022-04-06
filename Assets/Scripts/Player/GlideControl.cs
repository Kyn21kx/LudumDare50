using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

public class GlideControl : MonoBehaviour {

	public bool Gliding { get; private set; }

	[SerializeField]
	private float glidingSeconds;

	private float remainingGlidingSeconds;
	private Movement movRef;

	private void Start() {
		this.Gliding = false;
		this.remainingGlidingSeconds = glidingSeconds;
		this.movRef = GetComponent<Movement>();
	}

	private void Update() {
		HandleInput();
	}

	private void FixedUpdate() {
		SpartanMath.Clamp(ref this.remainingGlidingSeconds, 0f, glidingSeconds);
		if (Gliding)
			Glide();
		else if (movRef.Grounded)
			this.remainingGlidingSeconds += Time.fixedDeltaTime;
	}

	private void Glide() {
		Vector2 currVelocity = movRef.Rig.velocity;
		if (currVelocity.y >= 0f || remainingGlidingSeconds <= 0f) return;
		float forceFactor = remainingGlidingSeconds / glidingSeconds;
		float counterVelocity = Mathf.Abs(currVelocity.y) * SpartanMath.SmoothStart(0f, 1f, forceFactor, 2f);
		remainingGlidingSeconds -= Time.fixedDeltaTime;
		currVelocity.y += counterVelocity;
		movRef.Rig.velocity = currVelocity;
	}

	private void HandleInput() {
		this.Gliding = (Input.GetMouseButton(1) || Input.GetAxisRaw("Triggers") > 0f) && !movRef.Grounded;
	}

}
