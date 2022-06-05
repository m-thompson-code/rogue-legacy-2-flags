using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D5F RID: 3423
	public class CameraController : MonoBehaviour
	{
		// Token: 0x06006199 RID: 24985 RVA: 0x0016AA10 File Offset: 0x00168C10
		private void Awake()
		{
			if (QualitySettings.vSyncCount > 0)
			{
				Application.targetFrameRate = 60;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				Input.simulateMouseWithTouches = false;
			}
			this.cameraTransform = base.transform;
			this.previousSmoothing = this.MovementSmoothing;
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x00035C31 File Offset: 0x00033E31
		private void Start()
		{
			if (this.CameraTarget == null)
			{
				this.dummyTarget = new GameObject("Camera Target").transform;
				this.CameraTarget = this.dummyTarget;
			}
		}

		// Token: 0x0600619B RID: 24987 RVA: 0x0016AA64 File Offset: 0x00168C64
		private void LateUpdate()
		{
			this.GetPlayerInput();
			if (this.CameraTarget != null)
			{
				if (this.CameraMode == CameraController.CameraModes.Isometric)
				{
					this.desiredPosition = this.CameraTarget.position + Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0f, 0f, -this.FollowDistance);
				}
				else if (this.CameraMode == CameraController.CameraModes.Follow)
				{
					this.desiredPosition = this.CameraTarget.position + this.CameraTarget.TransformDirection(Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0f, 0f, -this.FollowDistance));
				}
				if (this.MovementSmoothing)
				{
					this.cameraTransform.position = Vector3.SmoothDamp(this.cameraTransform.position, this.desiredPosition, ref this.currentVelocity, this.MovementSmoothingValue * Time.fixedDeltaTime);
				}
				else
				{
					this.cameraTransform.position = this.desiredPosition;
				}
				if (this.RotationSmoothing)
				{
					this.cameraTransform.rotation = Quaternion.Lerp(this.cameraTransform.rotation, Quaternion.LookRotation(this.CameraTarget.position - this.cameraTransform.position), this.RotationSmoothingValue * Time.deltaTime);
					return;
				}
				this.cameraTransform.LookAt(this.CameraTarget);
			}
		}

		// Token: 0x0600619C RID: 24988 RVA: 0x0016ABE4 File Offset: 0x00168DE4
		private void GetPlayerInput()
		{
			this.moveVector = Vector3.zero;
			this.mouseWheel = Input.GetAxis("Mouse ScrollWheel");
			float num = (float)Input.touchCount;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || num > 0f)
			{
				this.mouseWheel *= 10f;
				if (Input.GetKeyDown(KeyCode.I))
				{
					this.CameraMode = CameraController.CameraModes.Isometric;
				}
				if (Input.GetKeyDown(KeyCode.F))
				{
					this.CameraMode = CameraController.CameraModes.Follow;
				}
				if (Input.GetKeyDown(KeyCode.S))
				{
					this.MovementSmoothing = !this.MovementSmoothing;
				}
				if (Input.GetMouseButton(1))
				{
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");
					if (this.mouseY > 0.01f || this.mouseY < -0.01f)
					{
						this.ElevationAngle -= this.mouseY * this.MoveSensitivity;
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}
					if (this.mouseX > 0.01f || this.mouseX < -0.01f)
					{
						this.OrbitalAngle += this.mouseX * this.MoveSensitivity;
						if (this.OrbitalAngle > 360f)
						{
							this.OrbitalAngle -= 360f;
						}
						if (this.OrbitalAngle < 0f)
						{
							this.OrbitalAngle += 360f;
						}
					}
				}
				if (num == 1f && Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
					if (deltaPosition.y > 0.01f || deltaPosition.y < -0.01f)
					{
						this.ElevationAngle -= deltaPosition.y * 0.1f;
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}
					if (deltaPosition.x > 0.01f || deltaPosition.x < -0.01f)
					{
						this.OrbitalAngle += deltaPosition.x * 0.1f;
						if (this.OrbitalAngle > 360f)
						{
							this.OrbitalAngle -= 360f;
						}
						if (this.OrbitalAngle < 0f)
						{
							this.OrbitalAngle += 360f;
						}
					}
				}
				RaycastHit raycastHit;
				if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 300f, 23552))
				{
					if (raycastHit.transform == this.CameraTarget)
					{
						this.OrbitalAngle = 0f;
					}
					else
					{
						this.CameraTarget = raycastHit.transform;
						this.OrbitalAngle = 0f;
						this.MovementSmoothing = this.previousSmoothing;
					}
				}
				if (Input.GetMouseButton(2))
				{
					if (this.dummyTarget == null)
					{
						this.dummyTarget = new GameObject("Camera Target").transform;
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					else if (this.dummyTarget != this.CameraTarget)
					{
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");
					this.moveVector = this.cameraTransform.TransformDirection(this.mouseX, this.mouseY, 0f);
					this.dummyTarget.Translate(-this.moveVector, Space.World);
				}
			}
			if (num == 2f)
			{
				Touch touch = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);
				Vector2 a = touch.position - touch.deltaPosition;
				Vector2 b = touch2.position - touch2.deltaPosition;
				float magnitude = (a - b).magnitude;
				float magnitude2 = (touch.position - touch2.position).magnitude;
				float num2 = magnitude - magnitude2;
				if (num2 > 0.01f || num2 < -0.01f)
				{
					this.FollowDistance += num2 * 0.25f;
					this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
				}
			}
			if (this.mouseWheel < -0.01f || this.mouseWheel > 0.01f)
			{
				this.FollowDistance -= this.mouseWheel * 5f;
				this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
			}
		}

		// Token: 0x04004F61 RID: 20321
		private Transform cameraTransform;

		// Token: 0x04004F62 RID: 20322
		private Transform dummyTarget;

		// Token: 0x04004F63 RID: 20323
		public Transform CameraTarget;

		// Token: 0x04004F64 RID: 20324
		public float FollowDistance = 30f;

		// Token: 0x04004F65 RID: 20325
		public float MaxFollowDistance = 100f;

		// Token: 0x04004F66 RID: 20326
		public float MinFollowDistance = 2f;

		// Token: 0x04004F67 RID: 20327
		public float ElevationAngle = 30f;

		// Token: 0x04004F68 RID: 20328
		public float MaxElevationAngle = 85f;

		// Token: 0x04004F69 RID: 20329
		public float MinElevationAngle;

		// Token: 0x04004F6A RID: 20330
		public float OrbitalAngle;

		// Token: 0x04004F6B RID: 20331
		public CameraController.CameraModes CameraMode;

		// Token: 0x04004F6C RID: 20332
		public bool MovementSmoothing = true;

		// Token: 0x04004F6D RID: 20333
		public bool RotationSmoothing;

		// Token: 0x04004F6E RID: 20334
		private bool previousSmoothing;

		// Token: 0x04004F6F RID: 20335
		public float MovementSmoothingValue = 25f;

		// Token: 0x04004F70 RID: 20336
		public float RotationSmoothingValue = 5f;

		// Token: 0x04004F71 RID: 20337
		public float MoveSensitivity = 2f;

		// Token: 0x04004F72 RID: 20338
		private Vector3 currentVelocity = Vector3.zero;

		// Token: 0x04004F73 RID: 20339
		private Vector3 desiredPosition;

		// Token: 0x04004F74 RID: 20340
		private float mouseX;

		// Token: 0x04004F75 RID: 20341
		private float mouseY;

		// Token: 0x04004F76 RID: 20342
		private Vector3 moveVector;

		// Token: 0x04004F77 RID: 20343
		private float mouseWheel;

		// Token: 0x04004F78 RID: 20344
		private const string event_SmoothingValue = "Slider - Smoothing Value";

		// Token: 0x04004F79 RID: 20345
		private const string event_FollowDistance = "Slider - Camera Zoom";

		// Token: 0x02000D60 RID: 3424
		public enum CameraModes
		{
			// Token: 0x04004F7B RID: 20347
			Follow,
			// Token: 0x04004F7C RID: 20348
			Isometric,
			// Token: 0x04004F7D RID: 20349
			Free
		}
	}
}
