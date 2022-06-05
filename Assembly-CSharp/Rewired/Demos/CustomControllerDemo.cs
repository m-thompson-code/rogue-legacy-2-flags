using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000ED8 RID: 3800
	[AddComponentMenu("")]
	public class CustomControllerDemo : MonoBehaviour
	{
		// Token: 0x06006DFA RID: 28154 RVA: 0x00189238 File Offset: 0x00187438
		private void Awake()
		{
			ScreenOrientation screenOrientation = ScreenOrientation.LandscapeLeft;
			if (SystemInfo.deviceType == DeviceType.Handheld && Screen.orientation != screenOrientation)
			{
				Screen.orientation = screenOrientation;
			}
			this.Initialize();
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x00189264 File Offset: 0x00187464
		private void Initialize()
		{
			ReInput.InputSourceUpdateEvent += this.OnInputSourceUpdate;
			this.joysticks = base.GetComponentsInChildren<TouchJoystickExample>();
			this.buttons = base.GetComponentsInChildren<TouchButtonExample>();
			this.axisCount = this.joysticks.Length * 2;
			this.buttonCount = this.buttons.Length;
			this.axisValues = new float[this.axisCount];
			this.buttonValues = new bool[this.buttonCount];
			Player player = ReInput.players.GetPlayer(this.playerId);
			this.controller = player.controllers.GetControllerWithTag<CustomController>(this.controllerTag);
			if (this.controller == null)
			{
				Debug.LogError("A matching controller was not found for tag \"" + this.controllerTag + "\"");
			}
			if (this.controller.buttonCount != this.buttonValues.Length || this.controller.axisCount != this.axisValues.Length)
			{
				Debug.LogError("Controller has wrong number of elements!");
			}
			if (this.useUpdateCallbacks && this.controller != null)
			{
				this.controller.SetAxisUpdateCallback(new Func<int, float>(this.GetAxisValueCallback));
				this.controller.SetButtonUpdateCallback(new Func<int, bool>(this.GetButtonValueCallback));
			}
			this.initialized = true;
		}

		// Token: 0x06006DFC RID: 28156 RVA: 0x0003C694 File Offset: 0x0003A894
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x0003C6AC File Offset: 0x0003A8AC
		private void OnInputSourceUpdate()
		{
			this.GetSourceAxisValues();
			this.GetSourceButtonValues();
			if (!this.useUpdateCallbacks)
			{
				this.SetControllerAxisValues();
				this.SetControllerButtonValues();
			}
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x001893A0 File Offset: 0x001875A0
		private void GetSourceAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				if (i % 2 != 0)
				{
					this.axisValues[i] = this.joysticks[i / 2].position.y;
				}
				else
				{
					this.axisValues[i] = this.joysticks[i / 2].position.x;
				}
			}
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x00189400 File Offset: 0x00187600
		private void GetSourceButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.buttonValues[i] = this.buttons[i].isPressed;
			}
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x00189438 File Offset: 0x00187638
		private void SetControllerAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				this.controller.SetAxisValue(i, this.axisValues[i]);
			}
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x0018946C File Offset: 0x0018766C
		private void SetControllerButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.controller.SetButtonValue(i, this.buttonValues[i]);
			}
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x0003C6CE File Offset: 0x0003A8CE
		private float GetAxisValueCallback(int index)
		{
			if (index >= this.axisValues.Length)
			{
				return 0f;
			}
			return this.axisValues[index];
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x0003C6E9 File Offset: 0x0003A8E9
		private bool GetButtonValueCallback(int index)
		{
			return index < this.buttonValues.Length && this.buttonValues[index];
		}

		// Token: 0x04005865 RID: 22629
		public int playerId;

		// Token: 0x04005866 RID: 22630
		public string controllerTag;

		// Token: 0x04005867 RID: 22631
		public bool useUpdateCallbacks;

		// Token: 0x04005868 RID: 22632
		private int buttonCount;

		// Token: 0x04005869 RID: 22633
		private int axisCount;

		// Token: 0x0400586A RID: 22634
		private float[] axisValues;

		// Token: 0x0400586B RID: 22635
		private bool[] buttonValues;

		// Token: 0x0400586C RID: 22636
		private TouchJoystickExample[] joysticks;

		// Token: 0x0400586D RID: 22637
		private TouchButtonExample[] buttons;

		// Token: 0x0400586E RID: 22638
		private CustomController controller;

		// Token: 0x0400586F RID: 22639
		[NonSerialized]
		private bool initialized;
	}
}
