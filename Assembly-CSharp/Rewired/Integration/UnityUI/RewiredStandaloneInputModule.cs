using System;
using System.Collections.Generic;
using Rewired.Components;
using Rewired.UI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000EC7 RID: 3783
	[AddComponentMenu("Rewired/Rewired Standalone Input Module")]
	public sealed class RewiredStandaloneInputModule : RewiredPointerInputModule
	{
		// Token: 0x170023D1 RID: 9169
		// (get) Token: 0x06006D0E RID: 27918 RVA: 0x0003BC27 File Offset: 0x00039E27
		// (set) Token: 0x06006D0F RID: 27919 RVA: 0x0003BC2F File Offset: 0x00039E2F
		public InputManager_Base RewiredInputManager
		{
			get
			{
				return this.rewiredInputManager;
			}
			set
			{
				this.rewiredInputManager = value;
			}
		}

		// Token: 0x170023D2 RID: 9170
		// (get) Token: 0x06006D10 RID: 27920 RVA: 0x0003BC38 File Offset: 0x00039E38
		// (set) Token: 0x06006D11 RID: 27921 RVA: 0x0003BC40 File Offset: 0x00039E40
		public bool UseAllRewiredGamePlayers
		{
			get
			{
				return this.useAllRewiredGamePlayers;
			}
			set
			{
				bool flag = value != this.useAllRewiredGamePlayers;
				this.useAllRewiredGamePlayers = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170023D3 RID: 9171
		// (get) Token: 0x06006D12 RID: 27922 RVA: 0x0003BC5D File Offset: 0x00039E5D
		// (set) Token: 0x06006D13 RID: 27923 RVA: 0x0003BC65 File Offset: 0x00039E65
		public bool UseRewiredSystemPlayer
		{
			get
			{
				return this.useRewiredSystemPlayer;
			}
			set
			{
				bool flag = value != this.useRewiredSystemPlayer;
				this.useRewiredSystemPlayer = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170023D4 RID: 9172
		// (get) Token: 0x06006D14 RID: 27924 RVA: 0x0003BC82 File Offset: 0x00039E82
		// (set) Token: 0x06006D15 RID: 27925 RVA: 0x0003BC94 File Offset: 0x00039E94
		public int[] RewiredPlayerIds
		{
			get
			{
				return (int[])this.rewiredPlayerIds.Clone();
			}
			set
			{
				this.rewiredPlayerIds = ((value != null) ? ((int[])value.Clone()) : new int[0]);
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170023D5 RID: 9173
		// (get) Token: 0x06006D16 RID: 27926 RVA: 0x0003BCB8 File Offset: 0x00039EB8
		// (set) Token: 0x06006D17 RID: 27927 RVA: 0x0003BCC0 File Offset: 0x00039EC0
		public bool UsePlayingPlayersOnly
		{
			get
			{
				return this.usePlayingPlayersOnly;
			}
			set
			{
				this.usePlayingPlayersOnly = value;
			}
		}

		// Token: 0x170023D6 RID: 9174
		// (get) Token: 0x06006D18 RID: 27928 RVA: 0x0003BCC9 File Offset: 0x00039EC9
		// (set) Token: 0x06006D19 RID: 27929 RVA: 0x0003BCD6 File Offset: 0x00039ED6
		public List<PlayerMouse> PlayerMice
		{
			get
			{
				return new List<PlayerMouse>(this.playerMice);
			}
			set
			{
				if (value == null)
				{
					this.playerMice = new List<PlayerMouse>();
					this.SetupRewiredVars();
					return;
				}
				this.playerMice = new List<PlayerMouse>(value);
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170023D7 RID: 9175
		// (get) Token: 0x06006D1A RID: 27930 RVA: 0x0003BCFF File Offset: 0x00039EFF
		// (set) Token: 0x06006D1B RID: 27931 RVA: 0x0003BD07 File Offset: 0x00039F07
		public bool MoveOneElementPerAxisPress
		{
			get
			{
				return this.moveOneElementPerAxisPress;
			}
			set
			{
				this.moveOneElementPerAxisPress = value;
			}
		}

		// Token: 0x170023D8 RID: 9176
		// (get) Token: 0x06006D1C RID: 27932 RVA: 0x0003BD10 File Offset: 0x00039F10
		// (set) Token: 0x06006D1D RID: 27933 RVA: 0x0003BD18 File Offset: 0x00039F18
		public bool allowMouseInput
		{
			get
			{
				return this.m_allowMouseInput;
			}
			set
			{
				this.m_allowMouseInput = value;
			}
		}

		// Token: 0x170023D9 RID: 9177
		// (get) Token: 0x06006D1E RID: 27934 RVA: 0x0003BD21 File Offset: 0x00039F21
		// (set) Token: 0x06006D1F RID: 27935 RVA: 0x0003BD29 File Offset: 0x00039F29
		public bool allowMouseInputIfTouchSupported
		{
			get
			{
				return this.m_allowMouseInputIfTouchSupported;
			}
			set
			{
				this.m_allowMouseInputIfTouchSupported = value;
			}
		}

		// Token: 0x170023DA RID: 9178
		// (get) Token: 0x06006D20 RID: 27936 RVA: 0x0003BD32 File Offset: 0x00039F32
		// (set) Token: 0x06006D21 RID: 27937 RVA: 0x0003BD3A File Offset: 0x00039F3A
		public bool allowTouchInput
		{
			get
			{
				return this.m_allowTouchInput;
			}
			set
			{
				this.m_allowTouchInput = value;
			}
		}

		// Token: 0x170023DB RID: 9179
		// (get) Token: 0x06006D22 RID: 27938 RVA: 0x0003BD43 File Offset: 0x00039F43
		// (set) Token: 0x06006D23 RID: 27939 RVA: 0x0003BD4B File Offset: 0x00039F4B
		public bool deselectIfBackgroundClicked
		{
			get
			{
				return this.m_deselectIfBackgroundClicked;
			}
			set
			{
				this.m_deselectIfBackgroundClicked = value;
			}
		}

		// Token: 0x170023DC RID: 9180
		// (get) Token: 0x06006D24 RID: 27940 RVA: 0x0003BD54 File Offset: 0x00039F54
		// (set) Token: 0x06006D25 RID: 27941 RVA: 0x0003BD5C File Offset: 0x00039F5C
		private bool deselectBeforeSelecting
		{
			get
			{
				return this.m_deselectBeforeSelecting;
			}
			set
			{
				this.m_deselectBeforeSelecting = value;
			}
		}

		// Token: 0x170023DD RID: 9181
		// (get) Token: 0x06006D26 RID: 27942 RVA: 0x0003BD65 File Offset: 0x00039F65
		// (set) Token: 0x06006D27 RID: 27943 RVA: 0x0003BD6D File Offset: 0x00039F6D
		public bool SetActionsById
		{
			get
			{
				return this.setActionsById;
			}
			set
			{
				if (this.setActionsById == value)
				{
					return;
				}
				this.setActionsById = value;
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170023DE RID: 9182
		// (get) Token: 0x06006D28 RID: 27944 RVA: 0x0003BD86 File Offset: 0x00039F86
		// (set) Token: 0x06006D29 RID: 27945 RVA: 0x0018542C File Offset: 0x0018362C
		public int HorizontalActionId
		{
			get
			{
				return this.horizontalActionId;
			}
			set
			{
				if (value == this.horizontalActionId)
				{
					return;
				}
				this.horizontalActionId = value;
				if (ReInput.isReady)
				{
					this.m_HorizontalAxis = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170023DF RID: 9183
		// (get) Token: 0x06006D2A RID: 27946 RVA: 0x0003BD8E File Offset: 0x00039F8E
		// (set) Token: 0x06006D2B RID: 27947 RVA: 0x0018547C File Offset: 0x0018367C
		public int VerticalActionId
		{
			get
			{
				return this.verticalActionId;
			}
			set
			{
				if (value == this.verticalActionId)
				{
					return;
				}
				this.verticalActionId = value;
				if (ReInput.isReady)
				{
					this.m_VerticalAxis = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170023E0 RID: 9184
		// (get) Token: 0x06006D2C RID: 27948 RVA: 0x0003BD96 File Offset: 0x00039F96
		// (set) Token: 0x06006D2D RID: 27949 RVA: 0x001854CC File Offset: 0x001836CC
		public int SubmitActionId
		{
			get
			{
				return this.submitActionId;
			}
			set
			{
				if (value == this.submitActionId)
				{
					return;
				}
				this.submitActionId = value;
				if (ReInput.isReady)
				{
					this.m_SubmitButton = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170023E1 RID: 9185
		// (get) Token: 0x06006D2E RID: 27950 RVA: 0x0003BD9E File Offset: 0x00039F9E
		// (set) Token: 0x06006D2F RID: 27951 RVA: 0x0018551C File Offset: 0x0018371C
		public int CancelActionId
		{
			get
			{
				return this.cancelActionId;
			}
			set
			{
				if (value == this.cancelActionId)
				{
					return;
				}
				this.cancelActionId = value;
				if (ReInput.isReady)
				{
					this.m_CancelButton = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170023E2 RID: 9186
		// (get) Token: 0x06006D30 RID: 27952 RVA: 0x0003BDA6 File Offset: 0x00039FA6
		protected override bool isMouseSupported
		{
			get
			{
				return base.isMouseSupported && this.m_allowMouseInput && (!this.isTouchSupported || this.m_allowMouseInputIfTouchSupported);
			}
		}

		// Token: 0x170023E3 RID: 9187
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x0003BD32 File Offset: 0x00039F32
		private bool isTouchAllowed
		{
			get
			{
				return this.m_allowTouchInput;
			}
		}

		// Token: 0x170023E4 RID: 9188
		// (get) Token: 0x06006D32 RID: 27954 RVA: 0x0003BDCC File Offset: 0x00039FCC
		// (set) Token: 0x06006D33 RID: 27955 RVA: 0x0003BDD4 File Offset: 0x00039FD4
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170023E5 RID: 9189
		// (get) Token: 0x06006D34 RID: 27956 RVA: 0x0003BDCC File Offset: 0x00039FCC
		// (set) Token: 0x06006D35 RID: 27957 RVA: 0x0003BDD4 File Offset: 0x00039FD4
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170023E6 RID: 9190
		// (get) Token: 0x06006D36 RID: 27958 RVA: 0x0003BDDD File Offset: 0x00039FDD
		// (set) Token: 0x06006D37 RID: 27959 RVA: 0x0003BDE5 File Offset: 0x00039FE5
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x170023E7 RID: 9191
		// (get) Token: 0x06006D38 RID: 27960 RVA: 0x0003BDEE File Offset: 0x00039FEE
		// (set) Token: 0x06006D39 RID: 27961 RVA: 0x0003BDF6 File Offset: 0x00039FF6
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x170023E8 RID: 9192
		// (get) Token: 0x06006D3A RID: 27962 RVA: 0x0003BDFF File Offset: 0x00039FFF
		// (set) Token: 0x06006D3B RID: 27963 RVA: 0x0003BE07 File Offset: 0x0003A007
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				if (this.m_HorizontalAxis == value)
				{
					return;
				}
				this.m_HorizontalAxis = value;
				if (ReInput.isReady)
				{
					this.horizontalActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x170023E9 RID: 9193
		// (get) Token: 0x06006D3C RID: 27964 RVA: 0x0003BE37 File Offset: 0x0003A037
		// (set) Token: 0x06006D3D RID: 27965 RVA: 0x0003BE3F File Offset: 0x0003A03F
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				if (this.m_VerticalAxis == value)
				{
					return;
				}
				this.m_VerticalAxis = value;
				if (ReInput.isReady)
				{
					this.verticalActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x170023EA RID: 9194
		// (get) Token: 0x06006D3E RID: 27966 RVA: 0x0003BE6F File Offset: 0x0003A06F
		// (set) Token: 0x06006D3F RID: 27967 RVA: 0x0003BE77 File Offset: 0x0003A077
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				if (this.m_SubmitButton == value)
				{
					return;
				}
				this.m_SubmitButton = value;
				if (ReInput.isReady)
				{
					this.submitActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x170023EB RID: 9195
		// (get) Token: 0x06006D40 RID: 27968 RVA: 0x0003BEA7 File Offset: 0x0003A0A7
		// (set) Token: 0x06006D41 RID: 27969 RVA: 0x0003BEAF File Offset: 0x0003A0AF
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				if (this.m_CancelButton == value)
				{
					return;
				}
				this.m_CancelButton = value;
				if (ReInput.isReady)
				{
					this.cancelActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x0018556C File Offset: 0x0018376C
		private RewiredStandaloneInputModule()
		{
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x00185614 File Offset: 0x00183814
		protected override void Awake()
		{
			base.Awake();
			this.isTouchSupported = base.defaultTouchInputSource.touchSupported;
			TouchInputModule component = base.GetComponent<TouchInputModule>();
			if (component != null)
			{
				component.enabled = false;
			}
			ReInput.InitializedEvent += this.OnRewiredInitialized;
			this.InitializeRewired();
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x0003BEDF File Offset: 0x0003A0DF
		public override void UpdateModule()
		{
			this.CheckEditorRecompile();
			if (this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus)
			{
				this.ShouldIgnoreEventsOnNoFocus();
				return;
			}
		}

		// Token: 0x06006D45 RID: 27973 RVA: 0x00003DA1 File Offset: 0x00001FA1
		public override bool IsModuleSupported()
		{
			return true;
		}

		// Token: 0x06006D46 RID: 27974 RVA: 0x00185668 File Offset: 0x00183868
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			bool flag = this.m_ForceModuleActive;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					flag |= this.GetButtonDown(player, this.submitActionId);
					flag |= this.GetButtonDown(player, this.cancelActionId);
					if (this.moveOneElementPerAxisPress)
					{
						flag |= (this.GetButtonDown(player, this.horizontalActionId) || this.GetNegativeButtonDown(player, this.horizontalActionId));
						flag |= (this.GetButtonDown(player, this.verticalActionId) || this.GetNegativeButtonDown(player, this.verticalActionId));
					}
					else
					{
						flag |= !Mathf.Approximately(this.GetAxis(player, this.horizontalActionId), 0f);
						flag |= !Mathf.Approximately(this.GetAxis(player, this.verticalActionId), 0f);
					}
				}
			}
			if (this.isMouseSupported)
			{
				flag |= this.DidAnyMouseMove();
				flag |= this.GetMouseButtonDownOnAnyMouse(0);
			}
			if (this.isTouchAllowed)
			{
				for (int j = 0; j < base.defaultTouchInputSource.touchCount; j++)
				{
					Touch touch = base.defaultTouchInputSource.GetTouch(j);
					flag |= (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary);
				}
			}
			return flag;
		}

		// Token: 0x06006D47 RID: 27975 RVA: 0x001857F4 File Offset: 0x001839F4
		public override void ActivateModule()
		{
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			base.ActivateModule();
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06006D48 RID: 27976 RVA: 0x0003BF08 File Offset: 0x0003A108
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x06006D49 RID: 27977 RVA: 0x0018584C File Offset: 0x00183A4C
		public override void Process()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			if (!this.ProcessTouchEvents() && this.isMouseSupported)
			{
				this.ProcessMouseEvents();
			}
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x001858C8 File Offset: 0x00183AC8
		private bool ProcessTouchEvents()
		{
			if (!this.isTouchAllowed)
			{
				return false;
			}
			for (int i = 0; i < base.defaultTouchInputSource.touchCount; i++)
			{
				Touch touch = base.defaultTouchInputSource.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool pressed;
					bool flag;
					PlayerPointerEventData touchPointerEventData = base.GetTouchPointerEventData(0, 0, touch, out pressed, out flag);
					this.ProcessTouchPress(touchPointerEventData, pressed, flag);
					if (!flag)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return base.defaultTouchInputSource.touchCount > 0;
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x00185950 File Offset: 0x00183B50
		private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				double unscaledTime = ReInput.time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					if (unscaledTime - (double)pointerEvent.clickTime < 0.30000001192092896)
					{
						int clickCount = pointerEvent.clickCount + 1;
						pointerEvent.clickCount = clickCount;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = (float)unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = (float)unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x06006D4C RID: 27980 RVA: 0x00185B80 File Offset: 0x00183D80
		private bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					if (this.GetButtonDown(player, this.submitActionId))
					{
						ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
						break;
					}
					if (this.GetButtonDown(player, this.cancelActionId))
					{
						ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
						break;
					}
				}
			}
			return baseEventData.used;
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x00185C48 File Offset: 0x00183E48
		private Vector2 GetRawMoveVector()
		{
			if (this.recompiling)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					float num = this.GetAxis(player, this.horizontalActionId);
					float num2 = this.GetAxis(player, this.verticalActionId);
					if (Mathf.Approximately(num, 0f))
					{
						num = 0f;
					}
					if (Mathf.Approximately(num2, 0f))
					{
						num2 = 0f;
					}
					if (this.moveOneElementPerAxisPress)
					{
						if (this.GetButtonDown(player, this.horizontalActionId) && num > 0f)
						{
							zero.x += 1f;
						}
						if (this.GetNegativeButtonDown(player, this.horizontalActionId) && num < 0f)
						{
							zero.x -= 1f;
						}
						if (this.GetButtonDown(player, this.verticalActionId) && num2 > 0f)
						{
							zero.y += 1f;
						}
						if (this.GetNegativeButtonDown(player, this.verticalActionId) && num2 < 0f)
						{
							zero.y -= 1f;
						}
					}
					else
					{
						if (this.GetButton(player, this.horizontalActionId) && num > 0f)
						{
							zero.x += 1f;
						}
						if (this.GetNegativeButton(player, this.horizontalActionId) && num < 0f)
						{
							zero.x -= 1f;
						}
						if (this.GetButton(player, this.verticalActionId) && num2 > 0f)
						{
							zero.y += 1f;
						}
						if (this.GetNegativeButton(player, this.verticalActionId) && num2 < 0f)
						{
							zero.y -= 1f;
						}
					}
				}
			}
			return zero;
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x00185E48 File Offset: 0x00184048
		private bool SendMoveEventToSelectedObject()
		{
			if (this.recompiling)
			{
				return false;
			}
			double unscaledTime = ReInput.time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			bool flag2;
			bool flag3;
			this.CheckButtonOrKeyMovement(out flag2, out flag3);
			AxisEventData axisEventData = null;
			bool flag4 = flag2 || flag3;
			if (flag4)
			{
				axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0f);
				MoveDirection moveDir = axisEventData.moveDir;
				flag4 = (((moveDir == MoveDirection.Up || moveDir == MoveDirection.Down) && flag3) || ((moveDir == MoveDirection.Left || moveDir == MoveDirection.Right) && flag2));
			}
			if (!flag4)
			{
				if (this.m_RepeatDelay > 0f)
				{
					if (flag && this.m_ConsecutiveMoveCount == 1)
					{
						flag4 = (unscaledTime > this.m_PrevActionTime + (double)this.m_RepeatDelay);
					}
					else
					{
						flag4 = (unscaledTime > this.m_PrevActionTime + (double)(1f / this.m_InputActionsPerSecond));
					}
				}
				else
				{
					flag4 = (unscaledTime > this.m_PrevActionTime + (double)(1f / this.m_InputActionsPerSecond));
				}
			}
			if (!flag4)
			{
				return false;
			}
			if (axisEventData == null)
			{
				axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0f);
			}
			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				if (!flag)
				{
					this.m_ConsecutiveMoveCount = 0;
				}
				if (this.m_ConsecutiveMoveCount == 0 || (!flag2 && !flag3))
				{
					this.m_ConsecutiveMoveCount++;
				}
				this.m_PrevActionTime = unscaledTime;
				this.m_LastMoveVector = rawMoveVector;
			}
			else
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			return axisEventData.used;
		}

		// Token: 0x06006D4F RID: 27983 RVA: 0x00186000 File Offset: 0x00184200
		private void CheckButtonOrKeyMovement(out bool downHorizontal, out bool downVertical)
		{
			downHorizontal = false;
			downVertical = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					downHorizontal |= (this.GetButtonDown(player, this.horizontalActionId) || this.GetNegativeButtonDown(player, this.horizontalActionId));
					downVertical |= (this.GetButtonDown(player, this.verticalActionId) || this.GetNegativeButtonDown(player, this.verticalActionId));
				}
			}
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x00186094 File Offset: 0x00184294
		private void ProcessMouseEvents()
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(this.playerIds[i]);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						this.ProcessMouseEvent(this.playerIds[i], j);
					}
				}
			}
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x00186104 File Offset: 0x00184304
		private void ProcessMouseEvent(int playerId, int pointerIndex)
		{
			RewiredPointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(playerId, pointerIndex);
			if (mousePointerEventData == null)
			{
				return;
			}
			RewiredPointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(0).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(1).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(1).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(2).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(2).eventData.buttonData);
			IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, pointerIndex);
			if (mouseInputSource == null)
			{
				return;
			}
			for (int i = 3; i < mouseInputSource.buttonCount; i++)
			{
				this.ProcessMousePress(mousePointerEventData.GetButtonState(i).eventData);
				this.ProcessDrag(mousePointerEventData.GetButtonState(i).eventData.buttonData);
			}
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06006D52 RID: 27986 RVA: 0x00186230 File Offset: 0x00184430
		private bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x00186278 File Offset: 0x00184478
		private void ProcessMousePress(RewiredPointerInputModule.MouseButtonEventData data)
		{
			PlayerPointerEventData buttonData = data.buttonData;
			if (base.GetMouseInputSource(buttonData.playerId, buttonData.inputSourceIndex) == null)
			{
				return;
			}
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				double unscaledTime = ReInput.time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - (double)buttonData.clickTime < 0.30000001192092896)
					{
						PlayerPointerEventData playerPointerEventData = buttonData;
						int clickCount = playerPointerEventData.clickCount + 1;
						playerPointerEventData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = (float)unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = (float)unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x00186494 File Offset: 0x00184694
		private void HandleMouseTouchDeselectionOnSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (this.m_deselectIfBackgroundClicked && this.m_deselectBeforeSelecting)
			{
				base.DeselectIfSelectionChanged(currentOverGo, pointerEvent);
				return;
			}
			GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
			if (this.m_deselectIfBackgroundClicked)
			{
				if (eventHandler != base.eventSystem.currentSelectedGameObject && eventHandler != null)
				{
					base.eventSystem.SetSelectedGameObject(null, pointerEvent);
					return;
				}
			}
			else if (this.m_deselectBeforeSelecting && eventHandler != null && eventHandler != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x0003BF16 File Offset: 0x0003A116
		private void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
		}

		// Token: 0x06006D56 RID: 27990 RVA: 0x0003BF1F File Offset: 0x0003A11F
		private bool ShouldIgnoreEventsOnNoFocus()
		{
			return !ReInput.isReady || ReInput.configuration.ignoreInputWhenAppNotInFocus;
		}

		// Token: 0x06006D57 RID: 27991 RVA: 0x0003BF34 File Offset: 0x0003A134
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ReInput.InitializedEvent -= this.OnRewiredInitialized;
			ReInput.ShutDownEvent -= this.OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= this.OnEditorRecompile;
		}

		// Token: 0x06006D58 RID: 27992 RVA: 0x00186524 File Offset: 0x00184724
		protected override bool IsDefaultPlayer(int playerId)
		{
			if (this.playerIds == null)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < this.playerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(this.playerIds[j]);
					if (player != null && (i >= 1 || !this.usePlayingPlayersOnly || player.isPlaying) && (i >= 2 || player.controllers.hasMouse))
					{
						return this.playerIds[j] == playerId;
					}
				}
			}
			return false;
		}

		// Token: 0x06006D59 RID: 27993 RVA: 0x001865AC File Offset: 0x001847AC
		private void InitializeRewired()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			ReInput.ShutDownEvent -= this.OnRewiredShutDown;
			ReInput.ShutDownEvent += this.OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= this.OnEditorRecompile;
			ReInput.EditorRecompileEvent += this.OnEditorRecompile;
			this.SetupRewiredVars();
		}

		// Token: 0x06006D5A RID: 27994 RVA: 0x00186618 File Offset: 0x00184818
		private void SetupRewiredVars()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.SetUpRewiredActions();
			if (this.useAllRewiredGamePlayers)
			{
				IList<Player> list = this.useRewiredSystemPlayer ? ReInput.players.AllPlayers : ReInput.players.Players;
				this.playerIds = new int[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					this.playerIds[i] = list[i].id;
				}
			}
			else
			{
				bool flag = false;
				List<int> list2 = new List<int>(this.rewiredPlayerIds.Length + 1);
				for (int j = 0; j < this.rewiredPlayerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(this.rewiredPlayerIds[j]);
					if (player != null && !list2.Contains(player.id))
					{
						list2.Add(player.id);
						if (player.id == 9999999)
						{
							flag = true;
						}
					}
				}
				if (this.useRewiredSystemPlayer && !flag)
				{
					list2.Insert(0, ReInput.players.GetSystemPlayer().id);
				}
				this.playerIds = list2.ToArray();
			}
			this.SetUpRewiredPlayerMice();
		}

		// Token: 0x06006D5B RID: 27995 RVA: 0x00186738 File Offset: 0x00184938
		private void SetUpRewiredPlayerMice()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			base.ClearMouseInputSources();
			for (int i = 0; i < this.playerMice.Count; i++)
			{
				PlayerMouse playerMouse = this.playerMice[i];
				if (!UnityTools.IsNullOrDestroyed<PlayerMouse>(playerMouse))
				{
					base.AddMouseInputSource(playerMouse);
				}
			}
		}

		// Token: 0x06006D5C RID: 27996 RVA: 0x00186788 File Offset: 0x00184988
		private void SetUpRewiredActions()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.setActionsById)
			{
				this.horizontalActionId = ReInput.mapping.GetActionId(this.m_HorizontalAxis);
				this.verticalActionId = ReInput.mapping.GetActionId(this.m_VerticalAxis);
				this.submitActionId = ReInput.mapping.GetActionId(this.m_SubmitButton);
				this.cancelActionId = ReInput.mapping.GetActionId(this.m_CancelButton);
				return;
			}
			InputAction action = ReInput.mapping.GetAction(this.horizontalActionId);
			this.m_HorizontalAxis = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.horizontalActionId = -1;
			}
			action = ReInput.mapping.GetAction(this.verticalActionId);
			this.m_VerticalAxis = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.verticalActionId = -1;
			}
			action = ReInput.mapping.GetAction(this.submitActionId);
			this.m_SubmitButton = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.submitActionId = -1;
			}
			action = ReInput.mapping.GetAction(this.cancelActionId);
			this.m_CancelButton = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.cancelActionId = -1;
			}
		}

		// Token: 0x06006D5D RID: 27997 RVA: 0x0003BF6F File Offset: 0x0003A16F
		private bool GetButton(Player player, int actionId)
		{
			return actionId >= 0 && player.GetButton(actionId);
		}

		// Token: 0x06006D5E RID: 27998 RVA: 0x0003BF7E File Offset: 0x0003A17E
		private bool GetButtonDown(Player player, int actionId)
		{
			return actionId >= 0 && player.GetButtonDown(actionId);
		}

		// Token: 0x06006D5F RID: 27999 RVA: 0x0003BF8D File Offset: 0x0003A18D
		private bool GetNegativeButton(Player player, int actionId)
		{
			return actionId >= 0 && player.GetNegativeButton(actionId);
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x0003BF9C File Offset: 0x0003A19C
		private bool GetNegativeButtonDown(Player player, int actionId)
		{
			return actionId >= 0 && player.GetNegativeButtonDown(actionId);
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x0003BFAB File Offset: 0x0003A1AB
		private float GetAxis(Player player, int actionId)
		{
			if (actionId < 0)
			{
				return 0f;
			}
			return player.GetAxis(actionId);
		}

		// Token: 0x06006D62 RID: 28002 RVA: 0x0003BFBE File Offset: 0x0003A1BE
		private void CheckEditorRecompile()
		{
			if (!this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.recompiling = false;
			this.InitializeRewired();
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x0003BFDE File Offset: 0x0003A1DE
		private void OnEditorRecompile()
		{
			this.recompiling = true;
			this.ClearRewiredVars();
		}

		// Token: 0x06006D64 RID: 28004 RVA: 0x0003BFED File Offset: 0x0003A1ED
		private void ClearRewiredVars()
		{
			Array.Clear(this.playerIds, 0, this.playerIds.Length);
			base.ClearMouseInputSources();
		}

		// Token: 0x06006D65 RID: 28005 RVA: 0x001868C4 File Offset: 0x00184AC4
		private bool DidAnyMouseMove()
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				int playerId = this.playerIds[i];
				Player player = ReInput.players.GetPlayer(playerId);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(playerId);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, j);
						if (mouseInputSource != null && mouseInputSource.screenPositionDelta.sqrMagnitude > 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006D66 RID: 28006 RVA: 0x00186950 File Offset: 0x00184B50
		private bool GetMouseButtonDownOnAnyMouse(int buttonIndex)
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				int playerId = this.playerIds[i];
				Player player = ReInput.players.GetPlayer(playerId);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(playerId);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, j);
						if (mouseInputSource != null && mouseInputSource.GetButtonDown(buttonIndex))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x0003C009 File Offset: 0x0003A209
		private void OnRewiredInitialized()
		{
			this.InitializeRewired();
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x0003C011 File Offset: 0x0003A211
		private void OnRewiredShutDown()
		{
			this.ClearRewiredVars();
		}

		// Token: 0x040057E3 RID: 22499
		private const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";

		// Token: 0x040057E4 RID: 22500
		private const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";

		// Token: 0x040057E5 RID: 22501
		private const string DEFAULT_ACTION_SUBMIT = "UISubmit";

		// Token: 0x040057E6 RID: 22502
		private const string DEFAULT_ACTION_CANCEL = "UICancel";

		// Token: 0x040057E7 RID: 22503
		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Player ids, etc.")]
		[SerializeField]
		private InputManager_Base rewiredInputManager;

		// Token: 0x040057E8 RID: 22504
		[SerializeField]
		[Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
		private bool useAllRewiredGamePlayers;

		// Token: 0x040057E9 RID: 22505
		[SerializeField]
		[Tooltip("Allow the Rewired System Player to control the UI.")]
		private bool useRewiredSystemPlayer;

		// Token: 0x040057EA RID: 22506
		[SerializeField]
		[Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
		private int[] rewiredPlayerIds = new int[1];

		// Token: 0x040057EB RID: 22507
		[SerializeField]
		[Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
		private bool usePlayingPlayersOnly;

		// Token: 0x040057EC RID: 22508
		[SerializeField]
		[Tooltip("Player Mice allowed to interact with the UI. Each Player that owns a Player Mouse must also be allowed to control the UI or the Player Mouse will not function.")]
		private List<PlayerMouse> playerMice = new List<PlayerMouse>();

		// Token: 0x040057ED RID: 22509
		[SerializeField]
		[Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
		private bool moveOneElementPerAxisPress;

		// Token: 0x040057EE RID: 22510
		[SerializeField]
		[Tooltip("If enabled, Action Ids will be used to set the Actions. If disabled, string names will be used to set the Actions.")]
		private bool setActionsById;

		// Token: 0x040057EF RID: 22511
		[SerializeField]
		[Tooltip("Id of the horizontal Action for movement (if axis events are used).")]
		private int horizontalActionId = -1;

		// Token: 0x040057F0 RID: 22512
		[SerializeField]
		[Tooltip("Id of the vertical Action for movement (if axis events are used).")]
		private int verticalActionId = -1;

		// Token: 0x040057F1 RID: 22513
		[SerializeField]
		[Tooltip("Id of the Action used to submit.")]
		private int submitActionId = -1;

		// Token: 0x040057F2 RID: 22514
		[SerializeField]
		[Tooltip("Id of the Action used to cancel.")]
		private int cancelActionId = -1;

		// Token: 0x040057F3 RID: 22515
		[SerializeField]
		[Tooltip("Name of the horizontal axis for movement (if axis events are used).")]
		private string m_HorizontalAxis = "UIHorizontal";

		// Token: 0x040057F4 RID: 22516
		[SerializeField]
		[Tooltip("Name of the vertical axis for movement (if axis events are used).")]
		private string m_VerticalAxis = "UIVertical";

		// Token: 0x040057F5 RID: 22517
		[SerializeField]
		[Tooltip("Name of the action used to submit.")]
		private string m_SubmitButton = "UISubmit";

		// Token: 0x040057F6 RID: 22518
		[SerializeField]
		[Tooltip("Name of the action used to cancel.")]
		private string m_CancelButton = "UICancel";

		// Token: 0x040057F7 RID: 22519
		[SerializeField]
		[Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x040057F8 RID: 22520
		[SerializeField]
		[Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
		private float m_RepeatDelay;

		// Token: 0x040057F9 RID: 22521
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements.")]
		private bool m_allowMouseInput = true;

		// Token: 0x040057FA RID: 22522
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
		private bool m_allowMouseInputIfTouchSupported = true;

		// Token: 0x040057FB RID: 22523
		[SerializeField]
		[Tooltip("Allows touch input to be used to select elements.")]
		private bool m_allowTouchInput = true;

		// Token: 0x040057FC RID: 22524
		[SerializeField]
		[Tooltip("Deselects the current selection on mouse/touch click when the pointer is not over a selectable object.")]
		private bool m_deselectIfBackgroundClicked = true;

		// Token: 0x040057FD RID: 22525
		[SerializeField]
		[Tooltip("Deselects the current selection on mouse/touch click before selecting the next object.")]
		private bool m_deselectBeforeSelecting = true;

		// Token: 0x040057FE RID: 22526
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		[Tooltip("Forces the module to always be active.")]
		private bool m_ForceModuleActive;

		// Token: 0x040057FF RID: 22527
		[NonSerialized]
		private int[] playerIds;

		// Token: 0x04005800 RID: 22528
		private bool recompiling;

		// Token: 0x04005801 RID: 22529
		[NonSerialized]
		private bool isTouchSupported;

		// Token: 0x04005802 RID: 22530
		[NonSerialized]
		private double m_PrevActionTime;

		// Token: 0x04005803 RID: 22531
		[NonSerialized]
		private Vector2 m_LastMoveVector;

		// Token: 0x04005804 RID: 22532
		[NonSerialized]
		private int m_ConsecutiveMoveCount;

		// Token: 0x04005805 RID: 22533
		[NonSerialized]
		private bool m_HasFocus = true;

		// Token: 0x02000EC8 RID: 3784
		[Serializable]
		public class PlayerSetting
		{
			// Token: 0x06006D69 RID: 28009 RVA: 0x0003C019 File Offset: 0x0003A219
			public PlayerSetting()
			{
			}

			// Token: 0x06006D6A RID: 28010 RVA: 0x001869CC File Offset: 0x00184BCC
			private PlayerSetting(RewiredStandaloneInputModule.PlayerSetting other)
			{
				if (other == null)
				{
					throw new ArgumentNullException("other");
				}
				this.playerId = other.playerId;
				this.playerMice = new List<PlayerMouse>();
				if (other.playerMice != null)
				{
					foreach (PlayerMouse item in other.playerMice)
					{
						this.playerMice.Add(item);
					}
				}
			}

			// Token: 0x06006D6B RID: 28011 RVA: 0x0003C02C File Offset: 0x0003A22C
			public RewiredStandaloneInputModule.PlayerSetting Clone()
			{
				return new RewiredStandaloneInputModule.PlayerSetting(this);
			}

			// Token: 0x04005806 RID: 22534
			public int playerId;

			// Token: 0x04005807 RID: 22535
			public List<PlayerMouse> playerMice = new List<PlayerMouse>();
		}
	}
}
