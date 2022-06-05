using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200085A RID: 2138
	public class CameraController : MonoBehaviour
	{
		// Token: 0x060046EC RID: 18156 RVA: 0x000FD924 File Offset: 0x000FBB24
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

		// Token: 0x060046ED RID: 18157 RVA: 0x000FD977 File Offset: 0x000FBB77
		private void Start()
		{
			if (this.CameraTarget == null)
			{
				this.dummyTarget = new GameObject("Camera Target").transform;
				this.CameraTarget = this.dummyTarget;
			}
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x000FD9A8 File Offset: 0x000FBBA8
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

		// Token: 0x060046EF RID: 18159 RVA: 0x000FDB28 File Offset: 0x000FBD28
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

		// Token: 0x04003BDC RID: 15324
		private Transform cameraTransform;

		// Token: 0x04003BDD RID: 15325
		private Transform dummyTarget;

		// Token: 0x04003BDE RID: 15326
		public Transform CameraTarget;

		// Token: 0x04003BDF RID: 15327
		public float FollowDistance = 30f;

		// Token: 0x04003BE0 RID: 15328
		public float MaxFollowDistance = 100f;

		// Token: 0x04003BE1 RID: 15329
		public float MinFollowDistance = 2f;

		// Token: 0x04003BE2 RID: 15330
		public float ElevationAngle = 30f;

		// Token: 0x04003BE3 RID: 15331
		public float MaxElevationAngle = 85f;

		// Token: 0x04003BE4 RID: 15332
		public float MinElevationAngle;

		// Token: 0x04003BE5 RID: 15333
		public float OrbitalAngle;

		// Token: 0x04003BE6 RID: 15334
		public CameraController.CameraModes CameraMode;

		// Token: 0x04003BE7 RID: 15335
		public bool MovementSmoothing = true;

		// Token: 0x04003BE8 RID: 15336
		public bool RotationSmoothing;

		// Token: 0x04003BE9 RID: 15337
		private bool previousSmoothing;

		// Token: 0x04003BEA RID: 15338
		public float MovementSmoothingValue = 25f;

		// Token: 0x04003BEB RID: 15339
		public float RotationSmoothingValue = 5f;

		// Token: 0x04003BEC RID: 15340
		public float MoveSensitivity = 2f;

		// Token: 0x04003BED RID: 15341
		private Vector3 currentVelocity = Vector3.zero;

		// Token: 0x04003BEE RID: 15342
		private Vector3 desiredPosition;

		// Token: 0x04003BEF RID: 15343
		private float mouseX;

		// Token: 0x04003BF0 RID: 15344
		private float mouseY;

		// Token: 0x04003BF1 RID: 15345
		private Vector3 moveVector;

		// Token: 0x04003BF2 RID: 15346
		private float mouseWheel;

		// Token: 0x04003BF3 RID: 15347
		private const string event_SmoothingValue = "Slider - Smoothing Value";

		// Token: 0x04003BF4 RID: 15348
		private const string event_FollowDistance = "Slider - Camera Zoom";

		// Token: 0x02000E76 RID: 3702
		public enum CameraModes
		{
			// Token: 0x040057FA RID: 22522
			Follow,
			// Token: 0x040057FB RID: 22523
			Isometric,
			// Token: 0x040057FC RID: 22524
			Free
		}
	}
}
