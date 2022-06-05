using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rewired.Internal;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Platforms.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Utils
{
	// Token: 0x02000EB4 RID: 3764
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ExternalTools : IExternalTools
	{
		// Token: 0x170023A7 RID: 9127
		// (get) Token: 0x06006BFA RID: 27642 RVA: 0x0003B2E7 File Offset: 0x000394E7
		// (set) Token: 0x06006BFB RID: 27643 RVA: 0x0003B2EE File Offset: 0x000394EE
		public static Func<object> getPlatformInitializerDelegate
		{
			get
			{
				return ExternalTools._getPlatformInitializerDelegate;
			}
			set
			{
				ExternalTools._getPlatformInitializerDelegate = value;
			}
		}

		// Token: 0x06006BFD RID: 27645 RVA: 0x00002FCA File Offset: 0x000011CA
		public void Destroy()
		{
		}

		// Token: 0x170023A8 RID: 9128
		// (get) Token: 0x06006BFE RID: 27646 RVA: 0x0003B2F6 File Offset: 0x000394F6
		public bool isEditorPaused
		{
			get
			{
				return this._isEditorPaused;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06006BFF RID: 27647 RVA: 0x0003B2FE File Offset: 0x000394FE
		// (remove) Token: 0x06006C00 RID: 27648 RVA: 0x0003B317 File Offset: 0x00039517
		public event Action<bool> EditorPausedStateChangedEvent
		{
			add
			{
				this._EditorPausedStateChangedEvent = (Action<bool>)Delegate.Combine(this._EditorPausedStateChangedEvent, value);
			}
			remove
			{
				this._EditorPausedStateChangedEvent = (Action<bool>)Delegate.Remove(this._EditorPausedStateChangedEvent, value);
			}
		}

		// Token: 0x06006C01 RID: 27649 RVA: 0x0003B330 File Offset: 0x00039530
		public object GetPlatformInitializer()
		{
			return Main.GetPlatformInitializer();
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x0002FEF1 File Offset: 0x0002E0F1
		public string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool IsEditorSceneViewFocused()
		{
			return false;
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06006C05 RID: 27653 RVA: 0x00182F74 File Offset: 0x00181174
		// (remove) Token: 0x06006C06 RID: 27654 RVA: 0x00182FAC File Offset: 0x001811AC
		public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		// Token: 0x06006C07 RID: 27655 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x0003B337 File Offset: 0x00039537
		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		// Token: 0x06006C09 RID: 27657 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x0002FEF1 File Offset: 0x0002E0F1
		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0U;
		}

		// Token: 0x06006C0C RID: 27660 RVA: 0x00002FCA File Offset: 0x000011CA
		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		// Token: 0x06006C0D RID: 27661 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x00002FCA File Offset: 0x000011CA
		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_GetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C10 RID: 27664 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_GetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C11 RID: 27665 RVA: 0x0003B342 File Offset: 0x00039542
		public Vector4 PS4Input_GetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06006C12 RID: 27666 RVA: 0x0003B349 File Offset: 0x00039549
		public void PS4Input_GetLastTouchData(int id, out int touchNum, out int touch0x, out int touch0y, out int touch0id, out int touch1x, out int touch1y, out int touch1id)
		{
			touchNum = 0;
			touch0x = 0;
			touch0y = 0;
			touch0id = 0;
			touch1x = 0;
			touch1y = 0;
			touch1id = 0;
		}

		// Token: 0x06006C13 RID: 27667 RVA: 0x0003B365 File Offset: 0x00039565
		public void PS4Input_GetPadControllerInformation(int id, out float touchpixelDensity, out int touchResolutionX, out int touchResolutionY, out int analogDeadZoneLeft, out int analogDeadZoneright, out int connectionType)
		{
			touchpixelDensity = 0f;
			touchResolutionX = 0;
			touchResolutionY = 0;
			analogDeadZoneLeft = 0;
			analogDeadZoneright = 0;
			connectionType = 0;
		}

		// Token: 0x06006C14 RID: 27668 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C16 RID: 27670 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C17 RID: 27671 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadResetLightBar(int id)
		{
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06006C1A RID: 27674 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_PadResetOrientation(int id)
		{
		}

		// Token: 0x06006C1B RID: 27675 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool PS4Input_PadIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06006C1C RID: 27676 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_GetUsersDetails(int slot, object loggedInUser)
		{
		}

		// Token: 0x06006C1D RID: 27677 RVA: 0x00037B9A File Offset: 0x00035D9A
		public int PS4Input_GetDeviceClassForHandle(int handle)
		{
			return -1;
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x0000F49B File Offset: 0x0000D69B
		public string PS4Input_GetDeviceClassString(int intValue)
		{
			return null;
		}

		// Token: 0x06006C1F RID: 27679 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_PadGetUsersHandles2(int maxControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_GetSpecialControllerInformation(int id, int padIndex, object controllerInformation)
		{
		}

		// Token: 0x06006C21 RID: 27681 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_SpecialGetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_SpecialGetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C23 RID: 27683 RVA: 0x0003B342 File Offset: 0x00039542
		public Vector4 PS4Input_SpecialGetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06006C24 RID: 27684 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_SpecialGetUsersHandles(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06006C25 RID: 27685 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_SpecialGetUsersHandles2(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06006C26 RID: 27686 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool PS4Input_SpecialIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06006C27 RID: 27687 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialResetLightSphere(int id)
		{
		}

		// Token: 0x06006C28 RID: 27688 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialResetOrientation(int id)
		{
		}

		// Token: 0x06006C29 RID: 27689 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C2A RID: 27690 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialSetLightSphere(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06006C2B RID: 27691 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C2C RID: 27692 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C2D RID: 27693 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_SpecialSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06006C2E RID: 27694 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_AimGetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C2F RID: 27695 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_AimGetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C30 RID: 27696 RVA: 0x0003B342 File Offset: 0x00039542
		public Vector4 PS4Input_AimGetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06006C31 RID: 27697 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_AimGetUsersHandles(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06006C32 RID: 27698 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_AimGetUsersHandles2(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06006C33 RID: 27699 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool PS4Input_AimIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06006C34 RID: 27700 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimResetLightSphere(int id)
		{
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimResetOrientation(int id)
		{
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C37 RID: 27703 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimSetLightSphere(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06006C38 RID: 27704 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06006C3A RID: 27706 RVA: 0x00002FCA File Offset: 0x000011CA
		public void PS4Input_AimSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06006C3B RID: 27707 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x0003B33B File Offset: 0x0003953B
		public Vector3 PS4Input_GetLastMoveGyro(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveGetButtons(int id, int index)
		{
			return 0;
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveGetAnalogButton(int id, int index)
		{
			return 0;
		}

		// Token: 0x06006C3F RID: 27711 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool PS4Input_MoveIsConnected(int id, int index)
		{
			return false;
		}

		// Token: 0x06006C40 RID: 27712 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles, int[] secondaryHandles)
		{
			return 0;
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles)
		{
			return 0;
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers)
		{
			return 0;
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x0003B381 File Offset: 0x00039581
		public IntPtr PS4Input_MoveGetControllerInputForTracking()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06006C44 RID: 27716 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveSetLightSphere(int id, int index, int red, int green, int blue)
		{
			return 0;
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public int PS4Input_MoveSetVibration(int id, int index, int motor)
		{
			return 0;
		}

		// Token: 0x06006C46 RID: 27718 RVA: 0x0003B388 File Offset: 0x00039588
		public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
		{
			vids = new List<int>();
			pids = new List<int>();
		}

		// Token: 0x06006C47 RID: 27719 RVA: 0x00037B9A File Offset: 0x00035D9A
		public int GetAndroidAPILevel()
		{
			return -1;
		}

		// Token: 0x06006C48 RID: 27720 RVA: 0x0003B398 File Offset: 0x00039598
		public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
		{
			return !(graphic as Graphic == null) && (graphic as Graphic).raycastTarget;
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x0003B3B5 File Offset: 0x000395B5
		public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
		{
			if (graphic as Graphic == null)
			{
				return;
			}
			(graphic as Graphic).raycastTarget = value;
		}

		// Token: 0x170023A9 RID: 9129
		// (get) Token: 0x06006C4A RID: 27722 RVA: 0x0003B3D2 File Offset: 0x000395D2
		public bool UnityInput_IsTouchPressureSupported
		{
			get
			{
				return Input.touchPressureSupported;
			}
		}

		// Token: 0x06006C4B RID: 27723 RVA: 0x0003B3D9 File Offset: 0x000395D9
		public float UnityInput_GetTouchPressure(ref Touch touch)
		{
			return touch.pressure;
		}

		// Token: 0x06006C4C RID: 27724 RVA: 0x0003B3E1 File Offset: 0x000395E1
		public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
		{
			return touch.maximumPossiblePressure;
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x0003B3E9 File Offset: 0x000395E9
		public IControllerTemplate CreateControllerTemplate(Guid typeGuid, object payload)
		{
			return ControllerTemplateFactory.Create(typeGuid, payload);
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x0003B3F2 File Offset: 0x000395F2
		public Type[] GetControllerTemplateTypes()
		{
			return ControllerTemplateFactory.templateTypes;
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x0003B3F9 File Offset: 0x000395F9
		public Type[] GetControllerTemplateInterfaceTypes()
		{
			return ControllerTemplateFactory.templateInterfaceTypes;
		}

		// Token: 0x0400579B RID: 22427
		private static Func<object> _getPlatformInitializerDelegate;

		// Token: 0x0400579C RID: 22428
		private bool _isEditorPaused;

		// Token: 0x0400579D RID: 22429
		private Action<bool> _EditorPausedStateChangedEvent;
	}
}
