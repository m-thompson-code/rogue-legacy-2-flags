using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE0 RID: 3808
	[AddComponentMenu("")]
	public class FallbackJoystickIdentificationDemo : MonoBehaviour
	{
		// Token: 0x06006E32 RID: 28210 RVA: 0x0003C94E File Offset: 0x0003AB4E
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

		// Token: 0x06006E33 RID: 28211 RVA: 0x0003C980 File Offset: 0x0003AB80
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x0003C980 File Offset: 0x0003AB80
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x00189EEC File Offset: 0x001880EC
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

		// Token: 0x06006E36 RID: 28214 RVA: 0x0003C988 File Offset: 0x0003AB88
		private void SetInputDelay()
		{
			this.nextInputAllowedTime = Time.time + 1f;
		}

		// Token: 0x06006E37 RID: 28215 RVA: 0x00189F34 File Offset: 0x00188134
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

		// Token: 0x06006E38 RID: 28216 RVA: 0x0018A018 File Offset: 0x00188218
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

		// Token: 0x06006E39 RID: 28217 RVA: 0x0003C99B File Offset: 0x0003AB9B
		private void Reset()
		{
			this.joysticksToIdentify = null;
			this.identifyRequired = false;
		}

		// Token: 0x04005896 RID: 22678
		private const float windowWidth = 250f;

		// Token: 0x04005897 RID: 22679
		private const float windowHeight = 250f;

		// Token: 0x04005898 RID: 22680
		private const float inputDelay = 1f;

		// Token: 0x04005899 RID: 22681
		private bool identifyRequired;

		// Token: 0x0400589A RID: 22682
		private Queue<Joystick> joysticksToIdentify;

		// Token: 0x0400589B RID: 22683
		private float nextInputAllowedTime;

		// Token: 0x0400589C RID: 22684
		private GUIStyle style;
	}
}
