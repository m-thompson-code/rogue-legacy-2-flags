using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x0200093C RID: 2364
	[AddComponentMenu("")]
	public class CustomControllerDemo : MonoBehaviour
	{
		// Token: 0x06005023 RID: 20515 RVA: 0x0011A6CC File Offset: 0x001188CC
		private void Awake()
		{
			ScreenOrientation screenOrientation = ScreenOrientation.LandscapeLeft;
			if (SystemInfo.deviceType == DeviceType.Handheld && Screen.orientation != screenOrientation)
			{
				Screen.orientation = screenOrientation;
			}
			this.Initialize();
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x0011A6F8 File Offset: 0x001188F8
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

		// Token: 0x06005025 RID: 20517 RVA: 0x0011A831 File Offset: 0x00118A31
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

		// Token: 0x06005026 RID: 20518 RVA: 0x0011A849 File Offset: 0x00118A49
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

		// Token: 0x06005027 RID: 20519 RVA: 0x0011A86C File Offset: 0x00118A6C
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

		// Token: 0x06005028 RID: 20520 RVA: 0x0011A8CC File Offset: 0x00118ACC
		private void GetSourceButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.buttonValues[i] = this.buttons[i].isPressed;
			}
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x0011A904 File Offset: 0x00118B04
		private void SetControllerAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				this.controller.SetAxisValue(i, this.axisValues[i]);
			}
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x0011A938 File Offset: 0x00118B38
		private void SetControllerButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.controller.SetButtonValue(i, this.buttonValues[i]);
			}
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x0011A96C File Offset: 0x00118B6C
		private float GetAxisValueCallback(int index)
		{
			if (index >= this.axisValues.Length)
			{
				return 0f;
			}
			return this.axisValues[index];
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x0011A987 File Offset: 0x00118B87
		private bool GetButtonValueCallback(int index)
		{
			return index < this.buttonValues.Length && this.buttonValues[index];
		}

		// Token: 0x04004285 RID: 17029
		public int playerId;

		// Token: 0x04004286 RID: 17030
		public string controllerTag;

		// Token: 0x04004287 RID: 17031
		public bool useUpdateCallbacks;

		// Token: 0x04004288 RID: 17032
		private int buttonCount;

		// Token: 0x04004289 RID: 17033
		private int axisCount;

		// Token: 0x0400428A RID: 17034
		private float[] axisValues;

		// Token: 0x0400428B RID: 17035
		private bool[] buttonValues;

		// Token: 0x0400428C RID: 17036
		private TouchJoystickExample[] joysticks;

		// Token: 0x0400428D RID: 17037
		private TouchButtonExample[] buttons;

		// Token: 0x0400428E RID: 17038
		private CustomController controller;

		// Token: 0x0400428F RID: 17039
		[NonSerialized]
		private bool initialized;
	}
}
