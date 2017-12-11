using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 5;
	public float rotationSpeed = 150;
	public float height = 0.5f;
	public float width = 0.15f;	
	public float heightPadding = 0.15f;
	public LayerMask groundLayer;
	public float maxSlope = 50;
	public float heightCorrectionRate = 1.5f;
	public bool isDebug;		

	Vector2 directionInput;
	float groundAngle;

	Vector3 targetPos;
	Quaternion targetRotation;
	Transform camera;

	Vector3 forward;
	RaycastHit downRay;
	RaycastHit forwardRay;	
	bool isGrounded;

	void Start () {
		camera = Camera.main.transform;
	}
	
	void Update() {
		GetInput();
		CalForward();
		CalGroundAngle();
		CalTargetPos();

		CheckGround();
		ApplyGravity();
		Rotate();
		Move();
		// debug
		DrawDebugLines();		
    }

	void GetInput() {
        directionInput.y = Input.GetAxis("Vertical");			
		directionInput.x = Input.GetAxis("Horizontal");
	}

	void CalForward() {
		if (!isGrounded) {
			forward = transform.forward;
			return;
		}
		forward = Vector3.Cross(transform.right, downRay.normal);
	}

	void CalGroundAngle() {
		if (!isGrounded) {
			groundAngle = 0;
			return;
		}
		groundAngle = Vector3.Angle(downRay.normal, transform.forward) - 90;
	}

	void CalTargetPos() {
		targetPos = transform.position + directionInput.y * forward * moveSpeed * Time.deltaTime;
	}

	void CheckGround() {
		if (Physics.Raycast(transform.position, -Vector3.up, out downRay, height + heightPadding, groundLayer)) {
			if (Vector3.Distance(targetPos, downRay.point) < height) {
				transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, heightCorrectionRate * Time.deltaTime);
			}
			isGrounded = true;
		} else {
			isGrounded = false;
		}
	}

	void ApplyGravity() {
		if (!isGrounded) {
			transform.position += Physics.gravity * Time.deltaTime;
		}
	}

	void Rotate() {
		transform.Rotate(0, directionInput.x * rotationSpeed * Time.deltaTime, 0);
		targetRotation = transform.rotation;
	}

	void Move() {
		// no move forward if the slope is too deep
		if (groundAngle >= maxSlope) return;
		// no move forward if there's an obstacle in front
		if (Physics.Raycast(transform.position, forward, out forwardRay, 1, groundLayer)) {
			Debug.Log(Vector3.Distance(targetPos, forwardRay.point));
			if (Vector3.Distance(targetPos, forwardRay.point) < width) return;
		}
		transform.position = transform.position + directionInput.y * forward * moveSpeed * Time.deltaTime;;
	}

	void DrawDebugLines() {
		if (!isDebug) return;
		Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
		Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
	}
}
