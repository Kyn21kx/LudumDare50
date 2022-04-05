using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

	private Camera camRef;

	[SerializeField]
	private float followSpeed;

	private Transform target;

	[SerializeField]
	private Vector3 initialOffset;

	private void Start() {
		camRef = GetComponent<Camera>();
		target = EntityFetcher.Instance.Player.transform;
		//Right here we set an initial offset that will be used to maintain the distance relative to the player
		initialOffset = target.position - transform.position;
	}

	private void Update() {
		HandleInput();
	}

	private void FixedUpdate() {
		Follow();
	}

	private void HandleInput() {
		//Do zoom and wacky stuff idk
	}

	private void Follow() {
		//TODO: Change this for a velocity approach
		transform.position = Vector3.Lerp(transform.position, target.position - initialOffset, followSpeed * Time.fixedDeltaTime);
	}

}
