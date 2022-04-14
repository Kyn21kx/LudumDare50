using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auxiliars;


[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(SpartanCamera))]
public class CameraFollow : MonoBehaviour {

	public bool InDeadZone { get; private set; }

	[SerializeField]
	private UpdateModes updateMode;

	[SerializeField]
	private float followSpeed;

	private SpartanCamera camProperties;

	private float followingBlend;

	private Vector3 offset;

	private void Start() {
		camProperties = GetComponent<SpartanCamera>();
		//Right here we set an initial offset that will be used to maintain the distance relative to the player
		followingBlend = 0f;
		offset = camProperties.Target.position - transform.position;
	}

	private void FixedUpdate() {
		if (updateMode != UpdateModes.FIXED) return;
		Follow(Time.fixedDeltaTime);
	}

	private void Update() {
		this.InDeadZone = IsInDeadZone(camProperties.Target.position);
		if (updateMode != UpdateModes.FRAMED) return;
		Follow(Time.deltaTime);
	}

	private void LateUpdate() {
		if (updateMode != UpdateModes.LATE) return;
		Follow(Time.deltaTime);
	}

	private void Follow(float timeStep) {
		if (InDeadZone) {
			followingBlend = 0f;
			return;
		}
		followingBlend += followSpeed * timeStep;
		if (followingBlend > 1f)
			followingBlend = 0f;
		//Uncomment this if you want non linear camera returns :D
		//transform.position = SpartanMath.SmoothStart(transform.position, camProperties.Target.position, followingBlend, exponentialReturn);
		transform.position = SpartanMath.Lerp(transform.position, camProperties.Target.position - offset, followingBlend);
	}

	public bool IsInDeadZone(Vector3 target) {
		float dis = SpartanMath.DistanceSqr(target, camProperties.CenterPosition);
		Debug.Log($"Distance: {dis}");
		Vector2 deadZoneMapped = camProperties.DeadZone / 2f;
		return dis <= (deadZoneMapped.x * deadZoneMapped.x) && dis <= (deadZoneMapped.y * deadZoneMapped.y);
	}
}
