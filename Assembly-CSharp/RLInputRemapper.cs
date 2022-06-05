using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class RLInputRemapper : MonoBehaviour
{
	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x06001B8B RID: 7051 RVA: 0x000585BA File Offset: 0x000567BA
	public static bool IsInitialized
	{
		get
		{
			return RLInputRemapper.Instance.m_isInitialized;
		}
	}

	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x06001B8C RID: 7052 RVA: 0x000585C6 File Offset: 0x000567C6
	// (set) Token: 0x06001B8D RID: 7053 RVA: 0x000585D2 File Offset: 0x000567D2
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

	// Token: 0x06001B8E RID: 7054 RVA: 0x000585DF File Offset: 0x000567DF
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

	// Token: 0x06001B8F RID: 7055 RVA: 0x0005860D File Offset: 0x0005680D
	public static void SetRemappingFlagDirty(bool flagGamepad)
	{
		if (!flagGamepad)
		{
			RLInputRemapper.Instance.m_keyboardWasRemapped = true;
			return;
		}
		RLInputRemapper.Instance.m_gamepadWasRemapped = true;
	}

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x06001B90 RID: 7056 RVA: 0x00058629 File Offset: 0x00056829
	private bool RemappingSucceeded
	{
		get
		{
			return this.m_inputMapperUsed != null;
		}
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x00058634 File Offset: 0x00056834
	private void Awake()
	{
		if (RLInputRemapper.Instance == null)
		{
			RLInputRemapper.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x00058655 File Offset: 0x00056855
	private void Start()
	{
		if (!RLInputRemapper.IsInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x00058664 File Offset: 0x00056864
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

	// Token: 0x06001B94 RID: 7060 RVA: 0x000586D7 File Offset: 0x000568D7
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

	// Token: 0x06001B95 RID: 7061 RVA: 0x00058708 File Offset: 0x00056908
	private void OnDestroy()
	{
		this.m_inputMapper.Stop();
		this.m_mouseInputMapper.Stop();
		this.m_inputMapper.RemoveAllEventListeners();
		this.m_mouseInputMapper.RemoveAllEventListeners();
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x00058736 File Offset: 0x00056936
	private void Initialize()
	{
		this.InitializeMapperOptions(this.m_inputMapper);
		this.InitializeMapperOptions(this.m_mouseInputMapper);
		this.m_waitYield = new WaitForSecondsRealtime(0.1f);
		this.m_isInitialized = true;
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x00058768 File Offset: 0x00056968
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

	// Token: 0x06001B98 RID: 7064 RVA: 0x00058898 File Offset: 0x00056A98
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

	// Token: 0x06001B99 RID: 7065 RVA: 0x0005894E File Offset: 0x00056B4E
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

	// Token: 0x06001B9A RID: 7066 RVA: 0x0005897F File Offset: 0x00056B7F
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

	// Token: 0x06001B9B RID: 7067 RVA: 0x000589B0 File Offset: 0x00056BB0
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

	// Token: 0x06001B9C RID: 7068 RVA: 0x00058B90 File Offset: 0x00056D90
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

	// Token: 0x06001B9D RID: 7069 RVA: 0x00058D6F File Offset: 0x00056F6F
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

	// Token: 0x06001B9E RID: 7070 RVA: 0x00058D88 File Offset: 0x00056F88
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

	// Token: 0x06001B9F RID: 7071 RVA: 0x00058DEC File Offset: 0x00056FEC
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

	// Token: 0x06001BA0 RID: 7072 RVA: 0x00058E60 File Offset: 0x00057060
	private void OnFailedMapping()
	{
		Debug.Log("Remapping failed or cancelled by user.");
		if (this.m_onCompleteAction != null)
		{
			this.m_onCompleteAction(false);
		}
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x00058E80 File Offset: 0x00057080
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

	// Token: 0x06001BA2 RID: 7074 RVA: 0x00058E8F File Offset: 0x0005708F
	private void ReassignAllWindowMappableActions()
	{
		Debug.Log("Reassigning all WindowRemappable entries...");
		this.ReassignWindowMappableAction("MoveHorizontal", Pole.Positive);
		this.ReassignWindowMappableAction("MoveHorizontal", Pole.Negative);
		this.ReassignWindowMappableAction("MoveVertical", Pole.Positive);
		this.ReassignWindowMappableAction("MoveVertical", Pole.Negative);
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00058ECC File Offset: 0x000570CC
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

	// Token: 0x06001BA4 RID: 7076 RVA: 0x00058FA0 File Offset: 0x000571A0
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

	// Token: 0x06001BA5 RID: 7077 RVA: 0x00059060 File Offset: 0x00057260
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

	// Token: 0x06001BA6 RID: 7078 RVA: 0x00059180 File Offset: 0x00057380
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

	// Token: 0x04001936 RID: 6454
	private static RLInputRemapper Instance;

	// Token: 0x04001937 RID: 6455
	private Action<bool> m_onCompleteAction;

	// Token: 0x04001938 RID: 6456
	private InputMapper m_inputMapper = new InputMapper();

	// Token: 0x04001939 RID: 6457
	private InputMapper m_mouseInputMapper = new InputMapper();

	// Token: 0x0400193A RID: 6458
	private InputMapper.Context m_context = new InputMapper.Context();

	// Token: 0x0400193B RID: 6459
	private InputMapper.Context m_mouseContext = new InputMapper.Context();

	// Token: 0x0400193C RID: 6460
	private ControllerMap m_mouseControllerMap;

	// Token: 0x0400193D RID: 6461
	private ControllerMap m_keyboardControllerMap;

	// Token: 0x0400193E RID: 6462
	private ControllerMap m_gamepadControllerMap;

	// Token: 0x0400193F RID: 6463
	private bool m_isInitialized;

	// Token: 0x04001940 RID: 6464
	private ControllerMap m_windowKBControllerMap;

	// Token: 0x04001941 RID: 6465
	private ControllerMap m_windowMouseControllerMap;

	// Token: 0x04001942 RID: 6466
	private ControllerMap m_illegalMouseControllerMap;

	// Token: 0x04001943 RID: 6467
	private ControllerMap m_illegalKBControllerMap;

	// Token: 0x04001944 RID: 6468
	private ControllerMap m_illegalGamepadControllerMap;

	// Token: 0x04001945 RID: 6469
	private ControllerType m_controllerTypeToRemap;

	// Token: 0x04001946 RID: 6470
	private ActionElementMap m_aemToRemap;

	// Token: 0x04001947 RID: 6471
	private InputAction m_inputActionToRemap;

	// Token: 0x04001948 RID: 6472
	private ControllerMap m_controllerMapToRemap;

	// Token: 0x04001949 RID: 6473
	private Pole m_axisContributionToRemap;

	// Token: 0x0400194A RID: 6474
	private WaitForSecondsRealtime m_waitYield;

	// Token: 0x0400194B RID: 6475
	private GamepadType m_currentlyUsedGamepadType;

	// Token: 0x0400194C RID: 6476
	private InputMapper m_inputMapperUsed;

	// Token: 0x0400194D RID: 6477
	private bool m_onCompletePlayed;

	// Token: 0x0400194E RID: 6478
	private bool m_gamepadWasRemapped;

	// Token: 0x0400194F RID: 6479
	private bool m_keyboardWasRemapped;
}
