using UnityEngine;
using UnityEditor;
using Auxiliars;

[ExecuteInEditMode]
public class SpartanCamera : MonoBehaviour {

	public float Distance => distance;

	public Transform Target => target;

	public Vector2 DeadZone => deadZone;

	public Vector3 CenterPosition => new Vector3(transform.position.x, transform.position.y, Mathf.Abs(transform.position.z) - Distance);

	[SerializeField]
	private Transform target;	
	
	[SerializeField]
	private Vector2 deadZone;

	[SerializeField]
	private bool showDisplayInfo;

	[SerializeField]
	private float distance;

	private void OnValidate() {
		if (Application.isPlaying || target == null) return;
		Vector3 adjustedPosition = transform.position;
		adjustedPosition.z = Target.position.z - Distance;
		transform.position = adjustedPosition;
	}

	private void OnDrawGizmos() {
		if (!showDisplayInfo) return;
		//There's actually something to draw
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0f), deadZone);
	}
}
