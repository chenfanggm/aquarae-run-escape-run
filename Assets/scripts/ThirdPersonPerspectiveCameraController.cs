using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPerspectiveCameraController : MonoBehaviour {

	public Transform target;
	public Vector3 offsetPos;
	public float moveSpeed = 5;
	public float rotationSpeed = 10;
	public float smoothSpeed = 0.5f;

	Quaternion targetRotation;
	Vector3 targetPos;
	
	void Update () {
		MoveWithTarget();
		LookAtTarget();
	}

	void MoveWithTarget() {
		targetPos = target.position + offsetPos;
		transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
	}

	void LookAtTarget() {
		targetRotation = Quaternion.LookRotation(target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}
}
