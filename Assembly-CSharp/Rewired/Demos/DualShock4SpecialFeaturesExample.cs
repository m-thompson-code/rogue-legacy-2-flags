using System;
using System.Collections.Generic;
using Rewired.ControllerExtensions;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EDC RID: 3804
	[AddComponentMenu("")]
	public class DualShock4SpecialFeaturesExample : MonoBehaviour
	{
		// Token: 0x17002408 RID: 9224
		// (get) Token: 0x06006E1D RID: 28189 RVA: 0x0003C8A6 File Offset: 0x0003AAA6
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x0003C8B8 File Offset: 0x0003AAB8
		private void Awake()
		{
			this.InitializeTouchObjects();
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x0018976C File Offset: 0x0018796C
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

		// Token: 0x06006E20 RID: 28192 RVA: 0x0018987C File Offset: 0x00187A7C
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

		// Token: 0x06006E21 RID: 28193 RVA: 0x00189A84 File Offset: 0x00187C84
		private void ResetOrientation()
		{
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				firstDS.ResetOrientation();
			}
		}

		// Token: 0x06006E22 RID: 28194 RVA: 0x00189AA8 File Offset: 0x00187CA8
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

		// Token: 0x06006E23 RID: 28195 RVA: 0x00189B1C File Offset: 0x00187D1C
		private void StartLightFlash()
		{
			DualShock4Extension dualShock4Extension = this.GetFirstDS4(this.player) as DualShock4Extension;
			if (dualShock4Extension != null)
			{
				dualShock4Extension.SetLightFlash(0.5f, 0.5f);
			}
		}

		// Token: 0x06006E24 RID: 28196 RVA: 0x00189B50 File Offset: 0x00187D50
		private void StopLightFlash()
		{
			DualShock4Extension dualShock4Extension = this.GetFirstDS4(this.player) as DualShock4Extension;
			if (dualShock4Extension != null)
			{
				dualShock4Extension.StopLightFlash();
			}
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x00189B78 File Offset: 0x00187D78
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

		// Token: 0x06006E26 RID: 28198 RVA: 0x00189BD4 File Offset: 0x00187DD4
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

		// Token: 0x06006E27 RID: 28199 RVA: 0x00189C94 File Offset: 0x00187E94
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

		// Token: 0x04005881 RID: 22657
		private const int maxTouches = 2;

		// Token: 0x04005882 RID: 22658
		public int playerId;

		// Token: 0x04005883 RID: 22659
		public Transform touchpadTransform;

		// Token: 0x04005884 RID: 22660
		public GameObject lightObject;

		// Token: 0x04005885 RID: 22661
		public Transform accelerometerTransform;

		// Token: 0x04005886 RID: 22662
		private List<DualShock4SpecialFeaturesExample.Touch> touches;

		// Token: 0x04005887 RID: 22663
		private Queue<DualShock4SpecialFeaturesExample.Touch> unusedTouches;

		// Token: 0x04005888 RID: 22664
		private bool isFlashing;

		// Token: 0x04005889 RID: 22665
		private GUIStyle textStyle;

		// Token: 0x02000EDD RID: 3805
		private class Touch
		{
			// Token: 0x0400588A RID: 22666
			public GameObject go;

			// Token: 0x0400588B RID: 22667
			public int touchId = -1;
		}
	}
}
