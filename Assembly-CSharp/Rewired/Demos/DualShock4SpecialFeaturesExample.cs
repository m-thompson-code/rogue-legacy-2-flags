using System;
using System.Collections.Generic;
using Rewired.ControllerExtensions;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000940 RID: 2368
	[AddComponentMenu("")]
	public class DualShock4SpecialFeaturesExample : MonoBehaviour
	{
		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x06005046 RID: 20550 RVA: 0x0011AE2C File Offset: 0x0011902C
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x0011AE3E File Offset: 0x0011903E
		private void Awake()
		{
			this.InitializeTouchObjects();
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x0011AE48 File Offset: 0x00119048
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				base.transform.rotation = firstDS.GetOrientation();
				this.HandleTouchpad(firstDS);
				Vector3 accelerometerValue = firstDS.GetAccelerometerValue();
				this.accelerometerTransform.LookAt(this.accelerometerTransform.position + accelerometerValue);
			}
			if (this.player.GetButtonDown("CycleLight"))
			{
				this.SetRandomLightColor();
			}
			if (this.player.GetButtonDown("ResetOrientation"))
			{
				this.ResetOrientation();
			}
			if (this.player.GetButtonDown("ToggleLightFlash"))
			{
				if (this.isFlashing)
				{
					this.StopLightFlash();
				}
				else
				{
					this.StartLightFlash();
				}
				this.isFlashing = !this.isFlashing;
			}
			if (this.player.GetButtonDown("VibrateLeft"))
			{
				firstDS.SetVibration(0, 1f, 1f);
			}
			if (this.player.GetButtonDown("VibrateRight"))
			{
				firstDS.SetVibration(1, 1f, 1f);
			}
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x0011AF58 File Offset: 0x00119158
		private void OnGUI()
		{
			if (this.textStyle == null)
			{
				this.textStyle = new GUIStyle(GUI.skin.label);
				this.textStyle.fontSize = 20;
				this.textStyle.wordWrap = true;
			}
			if (this.GetFirstDS4(this.player) == null)
			{
				return;
			}
			GUILayout.BeginArea(new Rect(200f, 100f, (float)Screen.width - 400f, (float)Screen.height - 200f));
			GUILayout.Label("Rotate the Dual Shock 4 to see the model rotate in sync.", this.textStyle, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Touch the touchpad to see them appear on the model.", this.textStyle, Array.Empty<GUILayoutOption>());
			ActionElementMap firstElementMapWithAction = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ResetOrientation", true);
			if (firstElementMapWithAction != null)
			{
				GUILayout.Label("Press " + firstElementMapWithAction.elementIdentifierName + " to reset the orientation. Hold the gamepad facing the screen with sticks pointing up and press the button.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			firstElementMapWithAction = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "CycleLight", true);
			if (firstElementMapWithAction != null)
			{
				GUILayout.Label("Press " + firstElementMapWithAction.elementIdentifierName + " to change the light color.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			firstElementMapWithAction = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ToggleLightFlash", true);
			if (firstElementMapWithAction != null)
			{
				GUILayout.Label("Press " + firstElementMapWithAction.elementIdentifierName + " to start or stop the light flashing.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			firstElementMapWithAction = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateLeft", true);
			if (firstElementMapWithAction != null)
			{
				GUILayout.Label("Press " + firstElementMapWithAction.elementIdentifierName + " vibrate the left motor.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			firstElementMapWithAction = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateRight", true);
			if (firstElementMapWithAction != null)
			{
				GUILayout.Label("Press " + firstElementMapWithAction.elementIdentifierName + " vibrate the right motor.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndArea();
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x0011B160 File Offset: 0x00119360
		private void ResetOrientation()
		{
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				firstDS.ResetOrientation();
			}
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x0011B184 File Offset: 0x00119384
		private void SetRandomLightColor()
		{
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				Color color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
				firstDS.SetLightColor(color);
				this.lightObject.GetComponent<MeshRenderer>().material.color = color;
			}
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x0011B1F8 File Offset: 0x001193F8
		private void StartLightFlash()
		{
			DualShock4Extension dualShock4Extension = this.GetFirstDS4(this.player) as DualShock4Extension;
			if (dualShock4Extension != null)
			{
				dualShock4Extension.SetLightFlash(0.5f, 0.5f);
			}
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x0011B22C File Offset: 0x0011942C
		private void StopLightFlash()
		{
			DualShock4Extension dualShock4Extension = this.GetFirstDS4(this.player) as DualShock4Extension;
			if (dualShock4Extension != null)
			{
				dualShock4Extension.StopLightFlash();
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x0011B254 File Offset: 0x00119454
		private IDualShock4Extension GetFirstDS4(Player player)
		{
			foreach (Joystick joystick in player.controllers.Joysticks)
			{
				IDualShock4Extension extension = joystick.GetExtension<IDualShock4Extension>();
				if (extension != null)
				{
					return extension;
				}
			}
			return null;
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x0011B2B0 File Offset: 0x001194B0
		private void InitializeTouchObjects()
		{
			this.touches = new List<DualShock4SpecialFeaturesExample.Touch>(2);
			this.unusedTouches = new Queue<DualShock4SpecialFeaturesExample.Touch>(2);
			for (int i = 0; i < 2; i++)
			{
				DualShock4SpecialFeaturesExample.Touch touch = new DualShock4SpecialFeaturesExample.Touch();
				touch.go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				touch.go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				touch.go.transform.SetParent(this.touchpadTransform, true);
				touch.go.GetComponent<MeshRenderer>().material.color = ((i == 0) ? Color.red : Color.green);
				touch.go.SetActive(false);
				this.unusedTouches.Enqueue(touch);
			}
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x0011B370 File Offset: 0x00119570
		private void HandleTouchpad(IDualShock4Extension ds4)
		{
			for (int i = this.touches.Count - 1; i >= 0; i--)
			{
				DualShock4SpecialFeaturesExample.Touch touch = this.touches[i];
				if (!ds4.IsTouchingByTouchId(touch.touchId))
				{
					touch.go.SetActive(false);
					this.unusedTouches.Enqueue(touch);
					this.touches.RemoveAt(i);
				}
			}
			for (int j = 0; j < ds4.maxTouches; j++)
			{
				if (ds4.IsTouching(j))
				{
					int touchId = ds4.GetTouchId(j);
					DualShock4SpecialFeaturesExample.Touch touch2 = this.touches.Find((DualShock4SpecialFeaturesExample.Touch x) => x.touchId == touchId);
					if (touch2 == null)
					{
						touch2 = this.unusedTouches.Dequeue();
						this.touches.Add(touch2);
					}
					touch2.touchId = touchId;
					touch2.go.SetActive(true);
					Vector2 vector;
					ds4.GetTouchPosition(j, out vector);
					touch2.go.transform.localPosition = new Vector3(vector.x - 0.5f, 0.5f + touch2.go.transform.localScale.y * 0.5f, vector.y - 0.5f);
				}
			}
		}

		// Token: 0x040042A1 RID: 17057
		private const int maxTouches = 2;

		// Token: 0x040042A2 RID: 17058
		public int playerId;

		// Token: 0x040042A3 RID: 17059
		public Transform touchpadTransform;

		// Token: 0x040042A4 RID: 17060
		public GameObject lightObject;

		// Token: 0x040042A5 RID: 17061
		public Transform accelerometerTransform;

		// Token: 0x040042A6 RID: 17062
		private List<DualShock4SpecialFeaturesExample.Touch> touches;

		// Token: 0x040042A7 RID: 17063
		private Queue<DualShock4SpecialFeaturesExample.Touch> unusedTouches;

		// Token: 0x040042A8 RID: 17064
		private bool isFlashing;

		// Token: 0x040042A9 RID: 17065
		private GUIStyle textStyle;

		// Token: 0x02000F09 RID: 3849
		private class Touch
		{
			// Token: 0x04005A42 RID: 23106
			public GameObject go;

			// Token: 0x04005A43 RID: 23107
			public int touchId = -1;
		}
	}
}
