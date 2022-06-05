using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000942 RID: 2370
	[AddComponentMenu("")]
	public class FallbackJoystickIdentificationDemo : MonoBehaviour
	{
		// Token: 0x06005058 RID: 20568 RVA: 0x0011B63C File Offset: 0x0011983C
		private void Awake()
		{
			if (!ReInput.unityJoystickIdentificationRequired)
			{
				return;
			}
			ReInput.ControllerConnectedEvent += this.JoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.JoystickDisconnected;
			this.IdentifyAllJoysticks();
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x0011B66E File Offset: 0x0011986E
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0011B676 File Offset: 0x00119876
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x0011B680 File Offset: 0x00119880
		public void IdentifyAllJoysticks()
		{
			this.Reset();
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick[] joysticks = ReInput.controllers.GetJoysticks();
			if (joysticks == null)
			{
				return;
			}
			this.identifyRequired = true;
			this.joysticksToIdentify = new Queue<Joystick>(joysticks);
			this.SetInputDelay();
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0011B6C8 File Offset: 0x001198C8
		private void SetInputDelay()
		{
			this.nextInputAllowedTime = Time.time + 1f;
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x0011B6DC File Offset: 0x001198DC
		private void OnGUI()
		{
			if (!this.identifyRequired)
			{
				return;
			}
			if (this.joysticksToIdentify == null || this.joysticksToIdentify.Count == 0)
			{
				this.Reset();
				return;
			}
			Rect screenRect = new Rect((float)Screen.width * 0.5f - 125f, (float)Screen.height * 0.5f - 125f, 250f, 250f);
			GUILayout.Window(0, screenRect, new GUI.WindowFunction(this.DrawDialogWindow), "Joystick Identification Required", Array.Empty<GUILayoutOption>());
			GUI.FocusWindow(0);
			if (Time.time < this.nextInputAllowedTime)
			{
				return;
			}
			if (!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(this.joysticksToIdentify.Peek().id, 0.8f, false))
			{
				return;
			}
			this.joysticksToIdentify.Dequeue();
			this.SetInputDelay();
			if (this.joysticksToIdentify.Count == 0)
			{
				this.Reset();
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0011B7C0 File Offset: 0x001199C0
		private void DrawDialogWindow(int windowId)
		{
			if (!this.identifyRequired)
			{
				return;
			}
			if (this.style == null)
			{
				this.style = new GUIStyle(GUI.skin.label);
				this.style.wordWrap = true;
			}
			GUILayout.Space(15f);
			GUILayout.Label("A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:", this.style, Array.Empty<GUILayoutOption>());
			Joystick joystick = this.joysticksToIdentify.Peek();
			GUILayout.Label("Press any button on \"" + joystick.name + "\" now.", this.style, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Skip", Array.Empty<GUILayoutOption>()))
			{
				this.joysticksToIdentify.Dequeue();
				return;
			}
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0011B872 File Offset: 0x00119A72
		private void Reset()
		{
			this.joysticksToIdentify = null;
			this.identifyRequired = false;
		}

		// Token: 0x040042B3 RID: 17075
		private const float windowWidth = 250f;

		// Token: 0x040042B4 RID: 17076
		private const float windowHeight = 250f;

		// Token: 0x040042B5 RID: 17077
		private const float inputDelay = 1f;

		// Token: 0x040042B6 RID: 17078
		private bool identifyRequired;

		// Token: 0x040042B7 RID: 17079
		private Queue<Joystick> joysticksToIdentify;

		// Token: 0x040042B8 RID: 17080
		private float nextInputAllowedTime;

		// Token: 0x040042B9 RID: 17081
		private GUIStyle style;
	}
}
