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
	// Token: 0x02000932 RID: 2354
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ExternalTools : IExternalTools
	{
		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x00113A21 File Offset: 0x00111C21
		// (set) Token: 0x06004EA8 RID: 20136 RVA: 0x00113A28 File Offset: 0x00111C28
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

		// Token: 0x06004EAA RID: 20138 RVA: 0x00113A38 File Offset: 0x00111C38
		public void Destroy()
		{
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x00113A3A File Offset: 0x00111C3A
		public bool isEditorPaused
		{
			get
			{
				return this._isEditorPaused;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06004EAC RID: 20140 RVA: 0x00113A42 File Offset: 0x00111C42
		// (remove) Token: 0x06004EAD RID: 20141 RVA: 0x00113A5B File Offset: 0x00111C5B
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

		// Token: 0x06004EAE RID: 20142 RVA: 0x00113A74 File Offset: 0x00111C74
		public object GetPlatformInitializer()
		{
			return Main.GetPlatformInitializer();
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x00113A7B File Offset: 0x00111C7B
		public string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x00113A82 File Offset: 0x00111C82
		public bool IsEditorSceneViewFocused()
		{
			return false;
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x00113A85 File Offset: 0x00111C85
		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06004EB2 RID: 20146 RVA: 0x00113A88 File Offset: 0x00111C88
		// (remove) Token: 0x06004EB3 RID: 20147 RVA: 0x00113AC0 File Offset: 0x00111CC0
		public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		// Token: 0x06004EB4 RID: 20148 RVA: 0x00113AF5 File Offset: 0x00111CF5
		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x00113AF8 File Offset: 0x00111CF8
		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x00113AFC File Offset: 0x00111CFC
		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x00113AFF File Offset: 0x00111CFF
		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x00113B06 File Offset: 0x00111D06
		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0U;
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x00113B09 File Offset: 0x00111D09
		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x00113B0B File Offset: 0x00111D0B
		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x00113B0E File Offset: 0x00111D0E
		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x00113B10 File Offset: 0x00111D10
		public Vector3 PS4Input_GetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x00113B17 File Offset: 0x00111D17
		public Vector3 PS4Input_GetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x00113B1E File Offset: 0x00111D1E
		public Vector4 PS4Input_GetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x00113B25 File Offset: 0x00111D25
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

		// Token: 0x06004EC0 RID: 20160 RVA: 0x00113B41 File Offset: 0x00111D41
		public void PS4Input_GetPadControllerInformation(int id, out float touchpixelDensity, out int touchResolutionX, out int touchResolutionY, out int analogDeadZoneLeft, out int analogDeadZoneright, out int connectionType)
		{
			touchpixelDensity = 0f;
			touchResolutionX = 0;
			touchResolutionY = 0;
			analogDeadZoneLeft = 0;
			analogDeadZoneright = 0;
			connectionType = 0;
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x00113B5D File Offset: 0x00111D5D
		public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x00113B5F File Offset: 0x00111D5F
		public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x00113B61 File Offset: 0x00111D61
		public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x00113B63 File Offset: 0x00111D63
		public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x00113B65 File Offset: 0x00111D65
		public void PS4Input_PadResetLightBar(int id)
		{
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x00113B67 File Offset: 0x00111D67
		public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x00113B69 File Offset: 0x00111D69
		public void PS4Input_PadResetOrientation(int id)
		{
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x00113B6B File Offset: 0x00111D6B
		public bool PS4Input_PadIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x00113B6E File Offset: 0x00111D6E
		public void PS4Input_GetUsersDetails(int slot, object loggedInUser)
		{
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x00113B70 File Offset: 0x00111D70
		public int PS4Input_GetDeviceClassForHandle(int handle)
		{
			return -1;
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x00113B73 File Offset: 0x00111D73
		public string PS4Input_GetDeviceClassString(int intValue)
		{
			return null;
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x00113B76 File Offset: 0x00111D76
		public int PS4Input_PadGetUsersHandles2(int maxControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x00113B79 File Offset: 0x00111D79
		public void PS4Input_GetSpecialControllerInformation(int id, int padIndex, object controllerInformation)
		{
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x00113B7B File Offset: 0x00111D7B
		public Vector3 PS4Input_SpecialGetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x00113B82 File Offset: 0x00111D82
		public Vector3 PS4Input_SpecialGetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x00113B89 File Offset: 0x00111D89
		public Vector4 PS4Input_SpecialGetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x00113B90 File Offset: 0x00111D90
		public int PS4Input_SpecialGetUsersHandles(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x00113B93 File Offset: 0x00111D93
		public int PS4Input_SpecialGetUsersHandles2(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x00113B96 File Offset: 0x00111D96
		public bool PS4Input_SpecialIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x00113B99 File Offset: 0x00111D99
		public void PS4Input_SpecialResetLightSphere(int id)
		{
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x00113B9B File Offset: 0x00111D9B
		public void PS4Input_SpecialResetOrientation(int id)
		{
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x00113B9D File Offset: 0x00111D9D
		public void PS4Input_SpecialSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x00113B9F File Offset: 0x00111D9F
		public void PS4Input_SpecialSetLightSphere(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x00113BA1 File Offset: 0x00111DA1
		public void PS4Input_SpecialSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x00113BA3 File Offset: 0x00111DA3
		public void PS4Input_SpecialSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x00113BA5 File Offset: 0x00111DA5
		public void PS4Input_SpecialSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x00113BA7 File Offset: 0x00111DA7
		public Vector3 PS4Input_AimGetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x00113BAE File Offset: 0x00111DAE
		public Vector3 PS4Input_AimGetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00113BB5 File Offset: 0x00111DB5
		public Vector4 PS4Input_AimGetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x00113BBC File Offset: 0x00111DBC
		public int PS4Input_AimGetUsersHandles(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x00113BBF File Offset: 0x00111DBF
		public int PS4Input_AimGetUsersHandles2(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x00113BC2 File Offset: 0x00111DC2
		public bool PS4Input_AimIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x00113BC5 File Offset: 0x00111DC5
		public void PS4Input_AimResetLightSphere(int id)
		{
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x00113BC7 File Offset: 0x00111DC7
		public void PS4Input_AimResetOrientation(int id)
		{
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00113BC9 File Offset: 0x00111DC9
		public void PS4Input_AimSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x00113BCB File Offset: 0x00111DCB
		public void PS4Input_AimSetLightSphere(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x00113BCD File Offset: 0x00111DCD
		public void PS4Input_AimSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00113BCF File Offset: 0x00111DCF
		public void PS4Input_AimSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x00113BD1 File Offset: 0x00111DD1
		public void PS4Input_AimSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x00113BD3 File Offset: 0x00111DD3
		public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x00113BDA File Offset: 0x00111DDA
		public Vector3 PS4Input_GetLastMoveGyro(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x00113BE1 File Offset: 0x00111DE1
		public int PS4Input_MoveGetButtons(int id, int index)
		{
			return 0;
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x00113BE4 File Offset: 0x00111DE4
		public int PS4Input_MoveGetAnalogButton(int id, int index)
		{
			return 0;
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x00113BE7 File Offset: 0x00111DE7
		public bool PS4Input_MoveIsConnected(int id, int index)
		{
			return false;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x00113BEA File Offset: 0x00111DEA
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles, int[] secondaryHandles)
		{
			return 0;
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x00113BED File Offset: 0x00111DED
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles)
		{
			return 0;
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x00113BF0 File Offset: 0x00111DF0
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers)
		{
			return 0;
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00113BF3 File Offset: 0x00111DF3
		public IntPtr PS4Input_MoveGetControllerInputForTracking()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00113BFA File Offset: 0x00111DFA
		public int PS4Input_MoveSetLightSphere(int id, int index, int red, int green, int blue)
		{
			return 0;
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00113BFD File Offset: 0x00111DFD
		public int PS4Input_MoveSetVibration(int id, int index, int motor)
		{
			return 0;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x00113C00 File Offset: 0x00111E00
		public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
		{
			vids = new List<int>();
			pids = new List<int>();
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00113C10 File Offset: 0x00111E10
		public int GetAndroidAPILevel()
		{
			return -1;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00113C13 File Offset: 0x00111E13
		public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
		{
			return !(graphic as Graphic == null) && (graphic as Graphic).raycastTarget;
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00113C30 File Offset: 0x00111E30
		public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
		{
			if (graphic as Graphic == null)
			{
				return;
			}
			(graphic as Graphic).raycastTarget = value;
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x06004EF7 RID: 20215 RVA: 0x00113C4D File Offset: 0x00111E4D
		public bool UnityInput_IsTouchPressureSupported
		{
			get
			{
				return Input.touchPressureSupported;
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00113C54 File Offset: 0x00111E54
		public float UnityInput_GetTouchPressure(ref Touch touch)
		{
			return touch.pressure;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00113C5C File Offset: 0x00111E5C
		public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
		{
			return touch.maximumPossiblePressure;
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x00113C64 File Offset: 0x00111E64
		public IControllerTemplate CreateControllerTemplate(Guid typeGuid, object payload)
		{
			return ControllerTemplateFactory.Create(typeGuid, payload);
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x00113C6D File Offset: 0x00111E6D
		public Type[] GetControllerTemplateTypes()
		{
			return ControllerTemplateFactory.templateTypes;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00113C74 File Offset: 0x00111E74
		public Type[] GetControllerTemplateInterfaceTypes()
		{
			return ControllerTemplateFactory.templateInterfaceTypes;
		}

		// Token: 0x04004217 RID: 16919
		private static Func<object> _getPlatformInitializerDelegate;

		// Token: 0x04004218 RID: 16920
		private bool _isEditorPaused;

		// Token: 0x04004219 RID: 16921
		private Action<bool> _EditorPausedStateChangedEvent;
	}
}
