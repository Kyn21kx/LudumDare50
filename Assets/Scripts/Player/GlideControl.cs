using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;

public class GlideControl : MonoBehaviour {

	public bool Gliding { get; private set; }

	[SerializeField]
	private GameObject mainGlideUI;
	[SerializeField]
	private ParticleSystem vfx;
	[SerializeField]
	private GameObject secondGlideUI;
	[SerializeField]
	private float glidingSeconds;
	[SerializeField]
	private float glidingDecay;

	private Movement movRef;
	private float remainingGlidingSeconds;
	private float originalScaleUI;
	private float originalVfxSize;

	private void Start() {
		this.Gliding = false;
		this.remainingGlidingSeconds = glidingSeconds;
		this.originalScaleUI = mainGlideUI.transform.localScale.x;
		this.movRef = GetComponent<Movement>();
		this.vfx.Stop();
		var mainModule = this.vfx.main;
		this.originalVfxSize = mainModule.startSize.constant;
		mainModule.duration = this.glidingSeconds * 1.5f;
	}

	private void Update() {
		HandleInput();
		ScaleUI();
		HandleGlideVfx();
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
		float counterVelocity = Mathf.Abs(currVelocity.y) * SpartanMath.SmoothStart(0f, 1f, forceFactor, glidingDecay);

		var particleModule = this.vfx.main;
		particleModule.startSize = originalVfxSize * forceFactor;

		remainingGlidingSeconds -= Time.fixedDeltaTime;
		
		currVelocity.y += counterVelocity;
		movRef.Rig.velocity = currVelocity;
	}

	private void HandleInput() {
		this.Gliding = (Input.GetMouseButton(1) || Input.GetAxisRaw("Triggers") > 0f) && !movRef.Grounded;
	}

	private void ScaleUI() {
		Vector3 currScale = mainGlideUI.transform.localScale;
		if (currScale.x >= originalScaleUI && !Gliding) {
			//Disable the things
			mainGlideUI.SetActive(false);
			secondGlideUI.SetActive(false);
			return;
		}
		mainGlideUI.SetActive(true);
		secondGlideUI.SetActive(true);
		//Get the percentage we're going to substract
		float percentage = remainingGlidingSeconds / glidingSeconds;
		currScale.x = originalScaleUI * percentage;
		mainGlideUI.transform.localScale = currScale;
	}

	private void HandleGlideVfx() {
		var particleModule = this.vfx.main;
		if (!Gliding) {
			particleModule.startSize = originalVfxSize;
			this.vfx.Clear();
			this.vfx.Stop();
			return;
		}
		else if (this.vfx.isStopped) {
			this.vfx.Play();
			return;
		}
		//Guaranteed to be gliding and playing
		//Here we rotate the thingy based on our velocity
		Vector3 rot = this.vfx.transform.localRotation.eulerAngles;
		if (movRef.Rig.velocity.x > 0f) {
			//45f as the upper end

		}
		else {

		}
	}

}
