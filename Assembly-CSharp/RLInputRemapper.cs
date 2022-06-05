using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class RLInputRemapper : MonoBehaviour
{
	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x06002633 RID: 9779 RVA: 0x00015363 File Offset: 0x00013563
	public static bool IsInitialized
	{
		get
		{
			return RLInputRemapper.Instance.m_isInitialized;
		}
	}

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x06002634 RID: 9780 RVA: 0x0001536F File Offset: 0x0001356F
	// (set) Token: 0x06002635 RID: 9781 RVA: 0x0001537B File Offset: 0x0001357B
	public static Action<bool> OnRemapComplete
	{
		get
		{
			return RLInputRemapper.Instance.m_onCompleteAction;
		}
		set
		{
			RLInputRemapper.Instance.m_onCompleteAction = value;
		}
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x00015388 File Offset: 0x00013588
	private string GetWindowActionName(string actionName)
	{
		if (actionName != null)
		{
			if (actionName == "MoveHorizontal")
			{
				return "Window_Horizontal";
			}
			if (actionName == "MoveVertical")
			{
				return "Window_Vertical";
			}
		}
		return null;
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x000153B6 File Offset: 0x000135B6
	public static void SetRemappingFlagDirty(bool flagGamepad)
	{
		if (!flagGamepad)
		{
			RLInputRemapper.Instance.m_keyboardWasRemapped = true;
			return;
		}
		RLInputRemapper.Instance.m_gamepadWasRemapped = true;
	}

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x06002638 RID: 9784 RVA: 0x000153D2 File Offset: 0x000135D2
	private bool RemappingSucceeded
	{
		get
		{
			return this.m_inputMapperUsed != null;
		}
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x000153DD File Offset: 0x000135DD
	private void Awake()
	{
		if (RLInputRemapper.Instance == null)
		{
			RLInputRemapper.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x000153FE File Offset: 0x000135FE
	private void Start()
	{
		if (!RLInputRemapper.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x000B54A0 File Offset: 0x000B36A0
	private void OnEnable()
	{
		this.m_gamepadWasRemapped = false;
		this.m_keyboardWasRemapped = false;
		Controller controller = ReInput.controllers.GetLastActiveController();
		int firstAvailableJoystickControllerID = RewiredOnStartupController.GetFirstAvailableJoystickControllerID();
		if (controller.identifier.controllerType != ControllerType.Joystick && firstAvailableJoystickControllerID != -1)
		{
			controller = Rewired_RL.Player.controllers.GetController(ControllerType.Joystick, firstAvailableJoystickControllerID);
		}
		if (controller.type == ControllerType.Joystick)
		{
			this.m_currentlyUsedGamepadType = ControllerGlyphLibrary.GetGlyphData(controller.hardwareTypeGuid, true).GamepadType;
		}
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x0001540D File Offset: 0x0001360D
	private void OnDisable()
	{
		if (this.m_gamepadWasRemapped)
		{
			SaveManager.SaveControllerMap(true);
			SaveManager.LoadAllControllerMaps();
			this.m_gamepadWasRemapped = false;
		}
		if (this.m_keyboardWasRemapped)
		{
			SaveManager.SaveControllerMap(false);
			this.m_keyboardWasRemapped = false;
		}
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x0001543E File Offset: 0x0001363E
	private void OnDestroy()
	{
		this.m_inputMapper.Stop();
		this.m_mouseInputMapper.Stop();
		this.m_inputMapper.RemoveAllEventListeners();
		this.m_mouseInputMapper.RemoveAllEventListeners();
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x0001546C File Offset: 0x0001366C
	private void Initialize()
	{
		this.InitializeMapperOptions(this.m_inputMapper);
		this.InitializeMapperOptions(this.m_mouseInputMapper);
		this.m_waitYield = new WaitForSecondsRealtime(0.1f);
		this.m_isInitialized = true;
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x000B5514 File Offset: 0x000B3714
	private void InitializeControllerMap(ControllerType controllerType)
	{
		Player player = ReInput.players.GetPlayer(0);
		int mapCategoryID = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.ActionRemappable);
		int mapCategoryID2 = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.WindowRemappable);
		int mapCategoryID3 = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.IllegalAssignments);
		int controllerId = ReInput.controllers.GetLastActiveController().identifier.controllerId;
		switch (controllerType)
		{
		case ControllerType.Keyboard:
			this.m_keyboardControllerMap = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID, 0);
			this.m_windowKBControllerMap = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID2, 0);
			this.m_illegalKBControllerMap = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID3, 0);
			return;
		case ControllerType.Mouse:
			this.m_mouseControllerMap = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID, 0);
			this.m_windowMouseControllerMap = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID2, 0);
			this.m_illegalMouseControllerMap = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID3, 0);
			return;
		case ControllerType.Joystick:
			this.m_gamepadControllerMap = player.controllers.maps.GetMap(ControllerType.Joystick, controllerId, mapCategoryID, 0);
			this.m_illegalGamepadControllerMap = player.controllers.maps.GetMap(ControllerType.Joystick, controllerId, mapCategoryID3, 0);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000B5644 File Offset: 0x000B3844
	private void InitializeMapperOptions(InputMapper inputMapper)
	{
		inputMapper.options.allowButtonsOnFullAxisAssignment = true;
		inputMapper.options.allowKeyboardKeysWithModifiers = false;
		inputMapper.options.allowKeyboardModifierKeyAsPrimary = true;
		inputMapper.options.ignoreMouseXAxis = true;
		inputMapper.options.ignoreMouseYAxis = true;
		inputMapper.options.timeout = 3f;
		InputMapper.Options options = inputMapper.options;
		options.isElementAllowedCallback = (Predicate<ControllerPollingInfo>)Delegate.Combine(options.isElementAllowedCallback, new Predicate<ControllerPollingInfo>(this.OnIsElementAllowed));
		inputMapper.InputMappedEvent += this.OnInputDetected;
		inputMapper.StoppedEvent += this.OnMappingComplete;
		inputMapper.ConflictFoundEvent += this.OnConflictFound;
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x0001549D File Offset: 0x0001369D
	public static void ChangeInputRequested(Rewired_RL.InputActionType actionInputType, bool useGamepad, Pole axis = Pole.Positive)
	{
		if (!RLInputRemapper.IsInitialized)
		{
			RLInputRemapper.Instance.Initialize();
		}
		if (!useGamepad)
		{
			RLInputRemapper.Instance.Internal_ChangeInputRequested(actionInputType, ControllerType.Keyboard, axis);
			return;
		}
		RLInputRemapper.Instance.Internal_ChangeInputRequested(actionInputType, ControllerType.Joystick, axis);
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000154CE File Offset: 0x000136CE
	public static void ChangeInputRequested(Rewired_RL.WindowInputActionType windowInputType, bool useGamepad, Pole axis = Pole.Positive)
	{
		if (!RLInputRemapper.IsInitialized)
		{
			RLInputRemapper.Instance.Initialize();
		}
		if (!useGamepad)
		{
			RLInputRemapper.Instance.Internal_ChangeInputRequested(windowInputType, ControllerType.Keyboard, axis);
			return;
		}
		RLInputRemapper.Instance.Internal_ChangeInputRequested(windowInputType, ControllerType.Joystick, axis);
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000B56FC File Offset: 0x000B38FC
	private void Internal_ChangeInputRequested(Rewired_RL.WindowInputActionType windowInputActionType, ControllerType controllerType, Pole axis)
	{
		this.m_inputActionToRemap = ReInput.mapping.GetAction(Rewired_RL.GetString(windowInputActionType));
		this.m_controllerTypeToRemap = controllerType;
		this.m_axisContributionToRemap = axis;
		if (controllerType == ControllerType.Keyboard)
		{
			this.InitializeControllerMap(ControllerType.Keyboard);
			this.InitializeControllerMap(ControllerType.Mouse);
			if (Rewired_RL.DoesActionExistInMapCategory(this.m_inputActionToRemap.name, Rewired_RL.MapCategoryType.WindowRemappable, ControllerType.Mouse, axis))
			{
				this.m_controllerMapToRemap = this.m_windowMouseControllerMap;
			}
			else
			{
				if (!Rewired_RL.DoesActionExistInMapCategory(this.m_inputActionToRemap.name, Rewired_RL.MapCategoryType.WindowRemappable, ControllerType.Keyboard, axis))
				{
					throw new Exception("RLInputRemapper.Internal_ChangeInputRequested() - Could not find ActionElementMap for Action: " + windowInputActionType.ToString() + " assigned on either the Keyboard map or Mouse map.  Double-check RewiredInputManager.");
				}
				this.m_controllerMapToRemap = this.m_windowKBControllerMap;
			}
		}
		else
		{
			this.InitializeControllerMap(ControllerType.Joystick);
			this.m_controllerMapToRemap = this.m_gamepadControllerMap;
		}
		if (this.m_controllerMapToRemap == null)
		{
			Debug.Log("Failed to find controller to remap. Chances are the player changed controllers in mid-remap.");
			this.m_onCompletePlayed = true;
			this.OnFailedMapping();
			return;
		}
		int controllerID = 0;
		if (controllerType == ControllerType.Joystick)
		{
			controllerID = this.m_controllerMapToRemap.controllerId;
		}
		this.m_aemToRemap = Rewired_RL.GetActionElementMap(controllerType == ControllerType.Joystick, this.m_inputActionToRemap, this.m_axisContributionToRemap, false, controllerID);
		ControllerMap controllerMap = (controllerType == ControllerType.Keyboard) ? this.m_windowKBControllerMap : this.m_gamepadControllerMap;
		AxisRange actionRange = AxisRange.Positive;
		if (axis == Pole.Negative)
		{
			actionRange = AxisRange.Negative;
		}
		this.m_context.actionId = this.m_inputActionToRemap.id;
		this.m_context.controllerMap = controllerMap;
		this.m_context.actionRange = actionRange;
		this.m_context.actionElementMapToReplace = controllerMap.GetElementMap(this.m_aemToRemap.id);
		if (controllerType == ControllerType.Keyboard)
		{
			this.m_mouseContext.actionId = this.m_inputActionToRemap.id;
			this.m_mouseContext.controllerMap = this.m_windowMouseControllerMap;
			this.m_mouseContext.actionRange = actionRange;
			this.m_mouseContext.actionElementMapToReplace = this.m_windowMouseControllerMap.GetElementMap(this.m_aemToRemap.id);
		}
		this.m_inputMapperUsed = null;
		base.StartCoroutine(this.StartListeningCoroutine(controllerType == ControllerType.Keyboard));
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x000B58DC File Offset: 0x000B3ADC
	private void Internal_ChangeInputRequested(Rewired_RL.InputActionType inputActionType, ControllerType controllerType, Pole axis)
	{
		this.m_inputActionToRemap = ReInput.mapping.GetAction(Rewired_RL.GetString(inputActionType));
		this.m_controllerTypeToRemap = controllerType;
		this.m_axisContributionToRemap = axis;
		if (controllerType == ControllerType.Keyboard)
		{
			this.InitializeControllerMap(ControllerType.Keyboard);
			this.InitializeControllerMap(ControllerType.Mouse);
			if (Rewired_RL.DoesActionExistInMapCategory(this.m_inputActionToRemap.name, Rewired_RL.MapCategoryType.ActionRemappable, ControllerType.Mouse, axis))
			{
				this.m_controllerMapToRemap = this.m_mouseControllerMap;
			}
			else
			{
				if (!Rewired_RL.DoesActionExistInMapCategory(this.m_inputActionToRemap.name, Rewired_RL.MapCategoryType.ActionRemappable, ControllerType.Keyboard, axis))
				{
					throw new Exception("RLInputRemapper.Internal_ChangeInputRequested() - Could not find ActionElementMap for Action: " + inputActionType.ToString() + " assigned on either the Keyboard map or Mouse map.  Double-check RewiredInputManager.");
				}
				this.m_controllerMapToRemap = this.m_keyboardControllerMap;
			}
		}
		else
		{
			this.InitializeControllerMap(ControllerType.Joystick);
			this.m_controllerMapToRemap = this.m_gamepadControllerMap;
		}
		if (this.m_controllerMapToRemap == null)
		{
			Debug.Log("Failed to find controller to remap. Chances are the player changed controllers in mid-remap.");
			this.m_onCompletePlayed = true;
			this.OnFailedMapping();
			return;
		}
		int controllerID = 0;
		if (controllerType == ControllerType.Joystick)
		{
			controllerID = this.m_controllerMapToRemap.controllerId;
		}
		this.m_aemToRemap = Rewired_RL.GetActionElementMap(controllerType == ControllerType.Joystick, this.m_inputActionToRemap, this.m_axisContributionToRemap, false, controllerID);
		ControllerMap controllerMap = (controllerType == ControllerType.Keyboard) ? this.m_keyboardControllerMap : this.m_gamepadControllerMap;
		AxisRange actionRange = AxisRange.Positive;
		if (axis == Pole.Negative)
		{
			actionRange = AxisRange.Negative;
		}
		this.m_context.actionId = this.m_inputActionToRemap.id;
		this.m_context.controllerMap = controllerMap;
		this.m_context.actionRange = actionRange;
		this.m_context.actionElementMapToReplace = controllerMap.GetElementMap(this.m_aemToRemap.id);
		if (controllerType == ControllerType.Keyboard)
		{
			this.m_mouseContext.actionId = this.m_inputActionToRemap.id;
			this.m_mouseContext.controllerMap = this.m_mouseControllerMap;
			this.m_mouseContext.actionRange = actionRange;
			this.m_mouseContext.actionElementMapToReplace = this.m_mouseControllerMap.GetElementMap(this.m_aemToRemap.id);
		}
		this.m_inputMapperUsed = null;
		base.StartCoroutine(this.StartListeningCoroutine(controllerType == ControllerType.Keyboard));
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000154FF File Offset: 0x000136FF
	private IEnumerator StartListeningCoroutine(bool startMouseInputMapper)
	{
		this.m_onCompletePlayed = false;
		yield return this.m_waitYield;
		this.m_inputMapper.Start(this.m_context);
		if (startMouseInputMapper)
		{
			this.m_mouseInputMapper.Start(this.m_mouseContext);
		}
		Debug.Log("Listening for Remap input...");
		yield break;
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x000B5ABC File Offset: 0x000B3CBC
	private void OnInputDetected(InputMapper.InputMappedEventData data)
	{
		this.m_inputMapperUsed = data.inputMapper;
		Debug.Log("Remap input detected...");
		this.m_inputMapper.Stop();
		this.m_mouseInputMapper.Stop();
		if (data.actionElementMap.controllerMap != this.m_controllerMapToRemap)
		{
			this.m_controllerMapToRemap.DeleteElementMap(this.m_aemToRemap.id);
		}
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000B5B20 File Offset: 0x000B3D20
	private void OnMappingComplete(InputMapper.StoppedEventData data)
	{
		if (this.RemappingSucceeded && this.m_inputMapperUsed != data.inputMapper)
		{
			return;
		}
		if (this.RemappingSucceeded && data.inputMapper == this.m_inputMapperUsed && !this.m_onCompletePlayed)
		{
			this.m_onCompletePlayed = true;
			base.StartCoroutine(this.OnSuccessfulMappingCoroutine());
			return;
		}
		if (!this.RemappingSucceeded && !this.m_onCompletePlayed)
		{
			this.m_onCompletePlayed = true;
			this.OnFailedMapping();
		}
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x00015515 File Offset: 0x00013715
	private void OnFailedMapping()
	{
		Debug.Log("Remapping failed or cancelled by user.");
		if (this.m_onCompleteAction != null)
		{
			this.m_onCompleteAction(false);
		}
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x00015535 File Offset: 0x00013735
	private IEnumerator OnSuccessfulMappingCoroutine()
	{
		yield return this.m_waitYield;
		ActionElementMap actionElementMap;
		if (this.m_controllerTypeToRemap == ControllerType.Joystick)
		{
			actionElementMap = Rewired_RL.GetActionElementMap(true, this.m_inputActionToRemap, this.m_axisContributionToRemap, false, GamepadImageSelectorController.ControllerID);
		}
		else
		{
			actionElementMap = Rewired_RL.GetActionElementMap(false, this.m_inputActionToRemap, this.m_axisContributionToRemap, false, 0);
		}
		Debug.Log("Remapping successful.  Action: " + this.m_inputActionToRemap.name + " remapped to element: " + actionElementMap.elementIdentifierName);
		string windowActionName = this.GetWindowActionName(this.m_inputActionToRemap.name);
		if (this.m_controllerTypeToRemap == ControllerType.Keyboard && windowActionName != null)
		{
			this.ReassignAllWindowMappableActions();
		}
		if (this.m_controllerTypeToRemap == ControllerType.Keyboard)
		{
			this.m_keyboardWasRemapped = true;
		}
		else if (this.m_controllerTypeToRemap == ControllerType.Joystick)
		{
			foreach (Joystick joystick in ReInput.controllers.Joysticks)
			{
				if (Rewired_RL.IsStandardJoystick(joystick))
				{
					ControllerMap map = this.m_gamepadControllerMap.ToControllerTemplateMap<IGamepadTemplate>().ToControllerMap(joystick);
					Rewired_RL.Player.controllers.maps.AddMap(ControllerType.Joystick, joystick.identifier.controllerId, map);
				}
			}
			this.m_gamepadWasRemapped = true;
		}
		if (this.m_controllerTypeToRemap == ControllerType.Joystick)
		{
			TextGlyphManager.CreateAllGlyphTables(ControllerType.Joystick);
		}
		else
		{
			TextGlyphManager.CreateAllGlyphTables(ControllerType.Keyboard);
		}
		if (this.m_onCompleteAction != null)
		{
			this.m_onCompleteAction(true);
		}
		yield break;
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x00015544 File Offset: 0x00013744
	private void ReassignAllWindowMappableActions()
	{
		Debug.Log("Reassigning all WindowRemappable entries...");
		this.ReassignWindowMappableAction("MoveHorizontal", Pole.Positive);
		this.ReassignWindowMappableAction("MoveHorizontal", Pole.Negative);
		this.ReassignWindowMappableAction("MoveVertical", Pole.Positive);
		this.ReassignWindowMappableAction("MoveVertical", Pole.Negative);
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x000B5B94 File Offset: 0x000B3D94
	private void ReassignWindowMappableAction(string movementActionName, Pole axis)
	{
		string windowActionName = this.GetWindowActionName(movementActionName);
		InputAction action = ReInput.mapping.GetAction(windowActionName);
		ControllerMap controllerMap = Rewired_RL.DoesActionExistInMapCategory(windowActionName, Rewired_RL.MapCategoryType.WindowRemappable, ControllerType.Mouse, axis) ? this.m_windowMouseControllerMap : this.m_windowKBControllerMap;
		ActionElementMap actionElementMap = Rewired_RL.GetActionElementMap(false, windowActionName, axis, false, 0);
		controllerMap.DeleteElementMap(actionElementMap.id);
		bool flag = Rewired_RL.DoesActionExistInMapCategory(movementActionName, Rewired_RL.MapCategoryType.ActionRemappable, ControllerType.Mouse, axis);
		if (!flag)
		{
			ControllerMap keyboardControllerMap = this.m_keyboardControllerMap;
		}
		else
		{
			ControllerMap mouseControllerMap = this.m_mouseControllerMap;
		}
		ActionElementMap actionElementMap2 = Rewired_RL.GetActionElementMap(false, movementActionName, axis, false, 0);
		controllerMap = (flag ? this.m_windowMouseControllerMap : this.m_windowKBControllerMap);
		ElementAssignment elementAssignment = new ElementAssignment(controllerMap.controllerType, actionElementMap2.elementType, actionElementMap2.elementIdentifierId, actionElementMap2.axisRange, actionElementMap2.keyCode, actionElementMap2.modifierKeyFlags, action.id, axis, actionElementMap2.invert);
		controllerMap.CreateElementMap(elementAssignment);
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x000B5C68 File Offset: 0x000B3E68
	private bool OnIsElementAllowed(ControllerPollingInfo info)
	{
		if (info.keyboardKey == KeyCode.Escape)
		{
			this.m_inputMapper.Stop();
			this.m_mouseInputMapper.Stop();
			return false;
		}
		if (info.controllerType == ControllerType.Joystick)
		{
			if (ControllerGlyphLibrary.GetGlyphData(info.controller.hardwareTypeGuid, true).GamepadType != this.m_currentlyUsedGamepadType)
			{
				return false;
			}
			if (this.m_illegalGamepadControllerMap.ContainsElementIdentifier(info.elementIdentifierId))
			{
				return false;
			}
			if (info.axisPole == Pole.Negative)
			{
				return false;
			}
		}
		return (info.controllerType != ControllerType.Keyboard || !this.m_illegalKBControllerMap.ContainsElementIdentifier(info.elementIdentifierId)) && (info.controllerType != ControllerType.Mouse || !this.m_illegalMouseControllerMap.ContainsElementIdentifier(info.elementIdentifierId));
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x000B5D28 File Offset: 0x000B3F28
	private void OnConflictFound(InputMapper.ConflictFoundEventData data)
	{
		bool flag = false;
		foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in data.conflicts)
		{
			ActionElementMap elementMap = elementAssignmentConflictInfo.controllerMap.GetElementMap(elementAssignmentConflictInfo.elementMapId);
			if (elementMap.elementIdentifierId != this.m_aemToRemap.elementIdentifierId || elementMap.controllerMap.controllerType != this.m_aemToRemap.controllerMap.controllerType)
			{
				ElementAssignment elementAssignment = new ElementAssignment(this.m_aemToRemap.controllerMap.controllerType, this.m_aemToRemap.elementType, this.m_aemToRemap.elementIdentifierId, elementMap.axisRange, this.m_aemToRemap.keyCode, this.m_aemToRemap.modifierKeyFlags, elementMap.actionId, elementMap.axisContribution, this.m_aemToRemap.invert);
				this.m_controllerMapToRemap.ReplaceOrCreateElementMap(elementAssignment);
				flag = true;
			}
		}
		if (flag)
		{
			data.responseCallback(InputMapper.ConflictResponse.Replace);
			return;
		}
		data.responseCallback(InputMapper.ConflictResponse.Ignore);
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x000B5E48 File Offset: 0x000B4048
	private void CreateDefaultElementMap(Rewired_RL.InputActionType actionInputType, ControllerType controllerType)
	{
		Controller controller = ReInput.controllers.GetController(controllerType, 0);
		ControllerMap controllerMap = (controllerType == ControllerType.Keyboard) ? this.m_keyboardControllerMap : this.m_gamepadControllerMap;
		int layoutId = ReInput.mapping.GetLayoutId(ControllerType.Keyboard, "Default");
		int mapCategoryId = ReInput.mapping.GetMapCategoryId("Player");
		ActionElementMap firstElementMapWithAction = ReInput.mapping.GetControllerMapInstance(controller, mapCategoryId, layoutId).GetFirstElementMapWithAction(Rewired_RL.GetString(actionInputType));
		ElementAssignment elementAssignment = new ElementAssignment(controllerMap.controllerType, firstElementMapWithAction.elementType, firstElementMapWithAction.elementIdentifierId, firstElementMapWithAction.axisRange, firstElementMapWithAction.keyCode, firstElementMapWithAction.modifierKeyFlags, firstElementMapWithAction.actionId, firstElementMapWithAction.axisContribution, firstElementMapWithAction.invert);
		controllerMap.CreateElementMap(elementAssignment);
	}

	// Token: 0x0400211C RID: 8476
	private static RLInputRemapper Instance;

	// Token: 0x0400211D RID: 8477
	private Action<bool> m_onCompleteAction;

	// Token: 0x0400211E RID: 8478
	private InputMapper m_inputMapper = new InputMapper();

	// Token: 0x0400211F RID: 8479
	private InputMapper m_mouseInputMapper = new InputMapper();

	// Token: 0x04002120 RID: 8480
	private InputMapper.Context m_context = new InputMapper.Context();

	// Token: 0x04002121 RID: 8481
	private InputMapper.Context m_mouseContext = new InputMapper.Context();

	// Token: 0x04002122 RID: 8482
	private ControllerMap m_mouseControllerMap;

	// Token: 0x04002123 RID: 8483
	private ControllerMap m_keyboardControllerMap;

	// Token: 0x04002124 RID: 8484
	private ControllerMap m_gamepadControllerMap;

	// Token: 0x04002125 RID: 8485
	private bool m_isInitialized;

	// Token: 0x04002126 RID: 8486
	private ControllerMap m_windowKBControllerMap;

	// Token: 0x04002127 RID: 8487
	private ControllerMap m_windowMouseControllerMap;

	// Token: 0x04002128 RID: 8488
	private ControllerMap m_illegalMouseControllerMap;

	// Token: 0x04002129 RID: 8489
	private ControllerMap m_illegalKBControllerMap;

	// Token: 0x0400212A RID: 8490
	private ControllerMap m_illegalGamepadControllerMap;

	// Token: 0x0400212B RID: 8491
	private ControllerType m_controllerTypeToRemap;

	// Token: 0x0400212C RID: 8492
	private ActionElementMap m_aemToRemap;

	// Token: 0x0400212D RID: 8493
	private InputAction m_inputActionToRemap;

	// Token: 0x0400212E RID: 8494
	private ControllerMap m_controllerMapToRemap;

	// Token: 0x0400212F RID: 8495
	private Pole m_axisContributionToRemap;

	// Token: 0x04002130 RID: 8496
	private WaitForSecondsRealtime m_waitYield;

	// Token: 0x04002131 RID: 8497
	private GamepadType m_currentlyUsedGamepadType;

	// Token: 0x04002132 RID: 8498
	private InputMapper m_inputMapperUsed;

	// Token: 0x04002133 RID: 8499
	private bool m_onCompletePlayed;

	// Token: 0x04002134 RID: 8500
	private bool m_gamepadWasRemapped;

	// Token: 0x04002135 RID: 8501
	private bool m_keyboardWasRemapped;
}
