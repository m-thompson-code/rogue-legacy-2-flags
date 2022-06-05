using System;
using System.Collections.Generic;
using System.Text;
using Rewired.UI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000EC1 RID: 3777
	public abstract class RewiredPointerInputModule : BaseInputModule
	{
		// Token: 0x170023C1 RID: 9153
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x00184864 File Offset: 0x00182A64
		private RewiredPointerInputModule.UnityInputSource defaultInputSource
		{
			get
			{
				if (this.__m_DefaultInputSource == null)
				{
					return this.__m_DefaultInputSource = new RewiredPointerInputModule.UnityInputSource();
				}
				return this.__m_DefaultInputSource;
			}
		}

		// Token: 0x170023C2 RID: 9154
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x0003B95A File Offset: 0x00039B5A
		private IMouseInputSource defaultMouseInputSource
		{
			get
			{
				return this.defaultInputSource;
			}
		}

		// Token: 0x170023C3 RID: 9155
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x0003B95A File Offset: 0x00039B5A
		protected ITouchInputSource defaultTouchInputSource
		{
			get
			{
				return this.defaultInputSource;
			}
		}

		// Token: 0x06006CD5 RID: 27861 RVA: 0x0003B962 File Offset: 0x00039B62
		protected bool IsDefaultMouse(IMouseInputSource mouse)
		{
			return this.defaultMouseInputSource == mouse;
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x00184890 File Offset: 0x00182A90
		public IMouseInputSource GetMouseInputSource(int playerId, int mouseIndex)
		{
			if (mouseIndex < 0)
			{
				throw new ArgumentOutOfRangeException("mouseIndex");
			}
			if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
			{
				return this.defaultMouseInputSource;
			}
			int count = this.m_MouseInputSourcesList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IMouseInputSource mouseInputSource = this.m_MouseInputSourcesList[i];
				if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSource) && mouseInputSource.playerId == playerId)
				{
					if (mouseIndex == num)
					{
						return mouseInputSource;
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x0003B96D File Offset: 0x00039B6D
		public void RemoveMouseInputSource(IMouseInputSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this.m_MouseInputSourcesList.Remove(source);
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x0003B98A File Offset: 0x00039B8A
		public void AddMouseInputSource(IMouseInputSource source)
		{
			if (UnityTools.IsNullOrDestroyed<IMouseInputSource>(source))
			{
				throw new ArgumentNullException("source");
			}
			this.m_MouseInputSourcesList.Add(source);
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x0018490C File Offset: 0x00182B0C
		public int GetMouseInputSourceCount(int playerId)
		{
			if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
			{
				return 1;
			}
			int count = this.m_MouseInputSourcesList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IMouseInputSource mouseInputSource = this.m_MouseInputSourcesList[i];
				if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSource) && mouseInputSource.playerId == playerId)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x0003B9AB File Offset: 0x00039BAB
		public ITouchInputSource GetTouchInputSource(int playerId, int sourceIndex)
		{
			if (!UnityTools.IsNullOrDestroyed<ITouchInputSource>(this.m_UserDefaultTouchInputSource))
			{
				return this.m_UserDefaultTouchInputSource;
			}
			return this.defaultTouchInputSource;
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x0003B9C7 File Offset: 0x00039BC7
		public void RemoveTouchInputSource(ITouchInputSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (this.m_UserDefaultTouchInputSource == source)
			{
				this.m_UserDefaultTouchInputSource = null;
			}
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x0003B9E7 File Offset: 0x00039BE7
		public void AddTouchInputSource(ITouchInputSource source)
		{
			if (UnityTools.IsNullOrDestroyed<ITouchInputSource>(source))
			{
				throw new ArgumentNullException("source");
			}
			this.m_UserDefaultTouchInputSource = source;
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x0003BA03 File Offset: 0x00039C03
		public int GetTouchInputSourceCount(int playerId)
		{
			if (!this.IsDefaultPlayer(playerId))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x0003BA11 File Offset: 0x00039C11
		protected void ClearMouseInputSources()
		{
			this.m_MouseInputSourcesList.Clear();
		}

		// Token: 0x170023C4 RID: 9156
		// (get) Token: 0x06006CDF RID: 27871 RVA: 0x00184970 File Offset: 0x00182B70
		protected virtual bool isMouseSupported
		{
			get
			{
				int count = this.m_MouseInputSourcesList.Count;
				if (count == 0)
				{
					return this.defaultMouseInputSource.enabled;
				}
				for (int i = 0; i < count; i++)
				{
					if (this.m_MouseInputSourcesList[i].enabled)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06006CE0 RID: 27872
		protected abstract bool IsDefaultPlayer(int playerId);

		// Token: 0x06006CE1 RID: 27873 RVA: 0x001849BC File Offset: 0x00182BBC
		protected bool GetPointerData(int playerId, int pointerIndex, int pointerTypeId, out PlayerPointerEventData data, bool create, PointerEventType pointerEventType)
		{
			Dictionary<int, PlayerPointerEventData>[] array;
			if (!this.m_PlayerPointerData.TryGetValue(playerId, out array))
			{
				array = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Dictionary<int, PlayerPointerEventData>();
				}
				this.m_PlayerPointerData.Add(playerId, array);
			}
			if (pointerIndex >= array.Length)
			{
				Dictionary<int, PlayerPointerEventData>[] array2 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = array[j];
				}
				array2[pointerIndex] = new Dictionary<int, PlayerPointerEventData>();
				array = array2;
				this.m_PlayerPointerData[playerId] = array;
			}
			Dictionary<int, PlayerPointerEventData> dictionary = array[pointerIndex];
			if (dictionary.TryGetValue(pointerTypeId, out data))
			{
				data.mouseSource = ((pointerEventType == PointerEventType.Mouse) ? this.GetMouseInputSource(playerId, pointerIndex) : null);
				data.touchSource = ((pointerEventType == PointerEventType.Touch) ? this.GetTouchInputSource(playerId, pointerIndex) : null);
				return false;
			}
			if (!create)
			{
				return false;
			}
			data = this.CreatePointerEventData(playerId, pointerIndex, pointerTypeId, pointerEventType);
			dictionary.Add(pointerTypeId, data);
			return true;
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x00184AA4 File Offset: 0x00182CA4
		private PlayerPointerEventData CreatePointerEventData(int playerId, int pointerIndex, int pointerTypeId, PointerEventType pointerEventType)
		{
			PlayerPointerEventData playerPointerEventData = new PlayerPointerEventData(base.eventSystem)
			{
				playerId = playerId,
				inputSourceIndex = pointerIndex,
				pointerId = pointerTypeId,
				sourceType = pointerEventType
			};
			if (pointerEventType == PointerEventType.Mouse)
			{
				playerPointerEventData.mouseSource = this.GetMouseInputSource(playerId, pointerIndex);
			}
			else if (pointerEventType == PointerEventType.Touch)
			{
				playerPointerEventData.touchSource = this.GetTouchInputSource(playerId, pointerIndex);
			}
			if (pointerTypeId == -1)
			{
				playerPointerEventData.buttonIndex = 0;
			}
			else if (pointerTypeId == -2)
			{
				playerPointerEventData.buttonIndex = 1;
			}
			else if (pointerTypeId == -3)
			{
				playerPointerEventData.buttonIndex = 2;
			}
			else if (pointerTypeId >= -2147483520 && pointerTypeId <= -2147483392)
			{
				playerPointerEventData.buttonIndex = pointerTypeId - -2147483520;
			}
			return playerPointerEventData;
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x00184B48 File Offset: 0x00182D48
		protected void RemovePointerData(PlayerPointerEventData data)
		{
			Dictionary<int, PlayerPointerEventData>[] array;
			if (this.m_PlayerPointerData.TryGetValue(data.playerId, out array) && data.inputSourceIndex < array.Length)
			{
				array[data.inputSourceIndex].Remove(data.pointerId);
			}
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x00184B8C File Offset: 0x00182D8C
		protected PlayerPointerEventData GetTouchPointerEventData(int playerId, int touchDeviceIndex, Touch input, out bool pressed, out bool released)
		{
			PlayerPointerEventData playerPointerEventData;
			bool pointerData = this.GetPointerData(playerId, touchDeviceIndex, input.fingerId, out playerPointerEventData, true, PointerEventType.Touch);
			playerPointerEventData.Reset();
			pressed = (pointerData || input.phase == TouchPhase.Began);
			released = (input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended);
			if (pointerData)
			{
				playerPointerEventData.position = input.position;
			}
			if (pressed)
			{
				playerPointerEventData.delta = Vector2.zero;
			}
			else
			{
				playerPointerEventData.delta = input.position - playerPointerEventData.position;
			}
			playerPointerEventData.position = input.position;
			playerPointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(playerPointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			playerPointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			return playerPointerEventData;
		}

		// Token: 0x06006CE5 RID: 27877 RVA: 0x00184C60 File Offset: 0x00182E60
		protected virtual RewiredPointerInputModule.MouseState GetMousePointerEventData(int playerId, int mouseIndex)
		{
			IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
			if (mouseInputSource == null)
			{
				return null;
			}
			PlayerPointerEventData playerPointerEventData;
			bool pointerData = this.GetPointerData(playerId, mouseIndex, -1, out playerPointerEventData, true, PointerEventType.Mouse);
			playerPointerEventData.Reset();
			if (pointerData)
			{
				playerPointerEventData.position = mouseInputSource.screenPosition;
			}
			Vector2 screenPosition = mouseInputSource.screenPosition;
			if (mouseInputSource.locked || !mouseInputSource.enabled)
			{
				playerPointerEventData.position = new Vector2(-1f, -1f);
				playerPointerEventData.delta = Vector2.zero;
			}
			else
			{
				playerPointerEventData.delta = screenPosition - playerPointerEventData.position;
				playerPointerEventData.position = screenPosition;
			}
			playerPointerEventData.scrollDelta = mouseInputSource.wheelDelta;
			playerPointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(playerPointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			playerPointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PlayerPointerEventData playerPointerEventData2;
			this.GetPointerData(playerId, mouseIndex, -2, out playerPointerEventData2, true, PointerEventType.Mouse);
			this.CopyFromTo(playerPointerEventData, playerPointerEventData2);
			playerPointerEventData2.button = PointerEventData.InputButton.Right;
			PlayerPointerEventData playerPointerEventData3;
			this.GetPointerData(playerId, mouseIndex, -3, out playerPointerEventData3, true, PointerEventType.Mouse);
			this.CopyFromTo(playerPointerEventData, playerPointerEventData3);
			playerPointerEventData3.button = PointerEventData.InputButton.Middle;
			for (int i = 3; i < mouseInputSource.buttonCount; i++)
			{
				PlayerPointerEventData playerPointerEventData4;
				this.GetPointerData(playerId, mouseIndex, -2147483520 + i, out playerPointerEventData4, true, PointerEventType.Mouse);
				this.CopyFromTo(playerPointerEventData, playerPointerEventData4);
				playerPointerEventData4.button = (PointerEventData.InputButton)(-1);
			}
			this.m_MouseState.SetButtonState(0, this.StateForMouseButton(playerId, mouseIndex, 0), playerPointerEventData);
			this.m_MouseState.SetButtonState(1, this.StateForMouseButton(playerId, mouseIndex, 1), playerPointerEventData2);
			this.m_MouseState.SetButtonState(2, this.StateForMouseButton(playerId, mouseIndex, 2), playerPointerEventData3);
			for (int j = 3; j < mouseInputSource.buttonCount; j++)
			{
				PlayerPointerEventData data;
				this.GetPointerData(playerId, mouseIndex, -2147483520 + j, out data, false, PointerEventType.Mouse);
				this.m_MouseState.SetButtonState(j, this.StateForMouseButton(playerId, mouseIndex, j), data);
			}
			return this.m_MouseState;
		}

		// Token: 0x06006CE6 RID: 27878 RVA: 0x00184E3C File Offset: 0x0018303C
		protected PlayerPointerEventData GetLastPointerEventData(int playerId, int pointerIndex, int pointerTypeId, bool ignorePointerTypeId, PointerEventType pointerEventType)
		{
			if (!ignorePointerTypeId)
			{
				PlayerPointerEventData result;
				this.GetPointerData(playerId, pointerIndex, pointerTypeId, out result, false, pointerEventType);
				return result;
			}
			Dictionary<int, PlayerPointerEventData>[] array;
			if (!this.m_PlayerPointerData.TryGetValue(playerId, out array))
			{
				return null;
			}
			if (pointerIndex >= array.Length)
			{
				return null;
			}
			using (Dictionary<int, PlayerPointerEventData>.Enumerator enumerator = array[pointerIndex].GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<int, PlayerPointerEventData> keyValuePair = enumerator.Current;
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06006CE7 RID: 27879 RVA: 0x00184EC4 File Offset: 0x001830C4
		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
		}

		// Token: 0x06006CE8 RID: 27880 RVA: 0x00184EF0 File Offset: 0x001830F0
		protected virtual void ProcessMove(PlayerPointerEventData pointerEvent)
		{
			GameObject newEnterTarget;
			if (pointerEvent.sourceType == PointerEventType.Mouse)
			{
				IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
				if (mouseInputSource != null)
				{
					newEnterTarget = ((!mouseInputSource.enabled || mouseInputSource.locked) ? null : pointerEvent.pointerCurrentRaycast.gameObject);
				}
				else
				{
					newEnterTarget = null;
				}
			}
			else
			{
				if (pointerEvent.sourceType != PointerEventType.Touch)
				{
					throw new NotImplementedException();
				}
				newEnterTarget = pointerEvent.pointerCurrentRaycast.gameObject;
			}
			base.HandlePointerExitAndEnter(pointerEvent, newEnterTarget);
		}

		// Token: 0x06006CE9 RID: 27881 RVA: 0x00184F6C File Offset: 0x0018316C
		protected virtual void ProcessDrag(PlayerPointerEventData pointerEvent)
		{
			if (!pointerEvent.IsPointerMoving() || pointerEvent.pointerDrag == null)
			{
				return;
			}
			if (pointerEvent.sourceType == PointerEventType.Mouse)
			{
				IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
				if (mouseInputSource == null || mouseInputSource.locked || !mouseInputSource.enabled)
				{
					return;
				}
			}
			if (!pointerEvent.dragging && RewiredPointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
			{
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging)
			{
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		// Token: 0x06006CEA RID: 27882 RVA: 0x0018505C File Offset: 0x0018325C
		public override bool IsPointerOverGameObject(int pointerTypeId)
		{
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					PlayerPointerEventData playerPointerEventData;
					if (value[i].TryGetValue(pointerTypeId, out playerPointerEventData) && playerPointerEventData.pointerEnter != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006CEB RID: 27883 RVA: 0x001850E4 File Offset: 0x001832E4
		protected void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in value[i])
					{
						base.HandlePointerExitAndEnter(keyValuePair2.Value, null);
					}
					value[i].Clear();
				}
			}
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		// Token: 0x06006CEC RID: 27884 RVA: 0x001851B0 File Offset: 0x001833B0
		public override string ToString()
		{
			string str = "<b>Pointer Input Module of type: </b>";
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder(str + ((type != null) ? type.ToString() : null));
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				stringBuilder.AppendLine("<B>Player Id:</b> " + keyValuePair.Key.ToString());
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					stringBuilder.AppendLine("<B>Pointer Index:</b> " + i.ToString());
					foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in value[i])
					{
						stringBuilder.AppendLine("<B>Button Id:</b> " + keyValuePair2.Key.ToString());
						stringBuilder.AppendLine(keyValuePair2.Value.ToString());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006CED RID: 27885 RVA: 0x0003BA1E File Offset: 0x00039C1E
		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x06006CEE RID: 27886 RVA: 0x0003BA45 File Offset: 0x00039C45
		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		// Token: 0x06006CEF RID: 27887 RVA: 0x001852FC File Offset: 0x001834FC
		protected PointerEventData.FramePressState StateForMouseButton(int playerId, int mouseIndex, int buttonId)
		{
			IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
			if (mouseInputSource == null)
			{
				return PointerEventData.FramePressState.NotChanged;
			}
			bool buttonDown = mouseInputSource.GetButtonDown(buttonId);
			bool buttonUp = mouseInputSource.GetButtonUp(buttonId);
			if (buttonDown && buttonUp)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (buttonDown)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (buttonUp)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		// Token: 0x040057CC RID: 22476
		public const int kMouseLeftId = -1;

		// Token: 0x040057CD RID: 22477
		public const int kMouseRightId = -2;

		// Token: 0x040057CE RID: 22478
		public const int kMouseMiddleId = -3;

		// Token: 0x040057CF RID: 22479
		public const int kFakeTouchesId = -4;

		// Token: 0x040057D0 RID: 22480
		private const int customButtonsStartingId = -2147483520;

		// Token: 0x040057D1 RID: 22481
		private const int customButtonsMaxCount = 128;

		// Token: 0x040057D2 RID: 22482
		private const int customButtonsLastId = -2147483392;

		// Token: 0x040057D3 RID: 22483
		private readonly List<IMouseInputSource> m_MouseInputSourcesList = new List<IMouseInputSource>();

		// Token: 0x040057D4 RID: 22484
		private Dictionary<int, Dictionary<int, PlayerPointerEventData>[]> m_PlayerPointerData = new Dictionary<int, Dictionary<int, PlayerPointerEventData>[]>();

		// Token: 0x040057D5 RID: 22485
		private ITouchInputSource m_UserDefaultTouchInputSource;

		// Token: 0x040057D6 RID: 22486
		private RewiredPointerInputModule.UnityInputSource __m_DefaultInputSource;

		// Token: 0x040057D7 RID: 22487
		private readonly RewiredPointerInputModule.MouseState m_MouseState = new RewiredPointerInputModule.MouseState();

		// Token: 0x02000EC2 RID: 3778
		protected class MouseState
		{
			// Token: 0x06006CF1 RID: 27889 RVA: 0x0018533C File Offset: 0x0018353C
			public bool AnyPressesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.PressedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06006CF2 RID: 27890 RVA: 0x0018537C File Offset: 0x0018357C
			public bool AnyReleasesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.ReleasedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06006CF3 RID: 27891 RVA: 0x001853BC File Offset: 0x001835BC
			public RewiredPointerInputModule.ButtonState GetButtonState(int button)
			{
				RewiredPointerInputModule.ButtonState buttonState = null;
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].button == button)
					{
						buttonState = this.m_TrackedButtons[i];
						break;
					}
				}
				if (buttonState == null)
				{
					buttonState = new RewiredPointerInputModule.ButtonState
					{
						button = button,
						eventData = new RewiredPointerInputModule.MouseButtonEventData()
					};
					this.m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			// Token: 0x06006CF4 RID: 27892 RVA: 0x0003BAAC File Offset: 0x00039CAC
			public void SetButtonState(int button, PointerEventData.FramePressState stateForMouseButton, PlayerPointerEventData data)
			{
				RewiredPointerInputModule.ButtonState buttonState = this.GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}

			// Token: 0x040057D8 RID: 22488
			private List<RewiredPointerInputModule.ButtonState> m_TrackedButtons = new List<RewiredPointerInputModule.ButtonState>();
		}

		// Token: 0x02000EC3 RID: 3779
		public class MouseButtonEventData
		{
			// Token: 0x06006CF6 RID: 27894 RVA: 0x0003BADF File Offset: 0x00039CDF
			public bool PressedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x06006CF7 RID: 27895 RVA: 0x0003BAF4 File Offset: 0x00039CF4
			public bool ReleasedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x040057D9 RID: 22489
			public PointerEventData.FramePressState buttonState;

			// Token: 0x040057DA RID: 22490
			public PlayerPointerEventData buttonData;
		}

		// Token: 0x02000EC4 RID: 3780
		protected class ButtonState
		{
			// Token: 0x170023C5 RID: 9157
			// (get) Token: 0x06006CF9 RID: 27897 RVA: 0x0003BB0A File Offset: 0x00039D0A
			// (set) Token: 0x06006CFA RID: 27898 RVA: 0x0003BB12 File Offset: 0x00039D12
			public RewiredPointerInputModule.MouseButtonEventData eventData
			{
				get
				{
					return this.m_EventData;
				}
				set
				{
					this.m_EventData = value;
				}
			}

			// Token: 0x170023C6 RID: 9158
			// (get) Token: 0x06006CFB RID: 27899 RVA: 0x0003BB1B File Offset: 0x00039D1B
			// (set) Token: 0x06006CFC RID: 27900 RVA: 0x0003BB23 File Offset: 0x00039D23
			public int button
			{
				get
				{
					return this.m_Button;
				}
				set
				{
					this.m_Button = value;
				}
			}

			// Token: 0x040057DB RID: 22491
			private int m_Button;

			// Token: 0x040057DC RID: 22492
			private RewiredPointerInputModule.MouseButtonEventData m_EventData;
		}

		// Token: 0x02000EC5 RID: 3781
		private sealed class UnityInputSource : IMouseInputSource, ITouchInputSource
		{
			// Token: 0x170023C7 RID: 9159
			// (get) Token: 0x06006CFE RID: 27902 RVA: 0x0003BB2C File Offset: 0x00039D2C
			int IMouseInputSource.playerId
			{
				get
				{
					this.TryUpdate();
					return 0;
				}
			}

			// Token: 0x170023C8 RID: 9160
			// (get) Token: 0x06006CFF RID: 27903 RVA: 0x0003BB2C File Offset: 0x00039D2C
			int ITouchInputSource.playerId
			{
				get
				{
					this.TryUpdate();
					return 0;
				}
			}

			// Token: 0x170023C9 RID: 9161
			// (get) Token: 0x06006D00 RID: 27904 RVA: 0x0003BB35 File Offset: 0x00039D35
			bool IMouseInputSource.enabled
			{
				get
				{
					this.TryUpdate();
					return true;
				}
			}

			// Token: 0x170023CA RID: 9162
			// (get) Token: 0x06006D01 RID: 27905 RVA: 0x0003BB3E File Offset: 0x00039D3E
			bool IMouseInputSource.locked
			{
				get
				{
					this.TryUpdate();
					return Cursor.lockState == CursorLockMode.Locked;
				}
			}

			// Token: 0x170023CB RID: 9163
			// (get) Token: 0x06006D02 RID: 27906 RVA: 0x0003BB4E File Offset: 0x00039D4E
			int IMouseInputSource.buttonCount
			{
				get
				{
					this.TryUpdate();
					return 3;
				}
			}

			// Token: 0x06006D03 RID: 27907 RVA: 0x0003BB57 File Offset: 0x00039D57
			bool IMouseInputSource.GetButtonDown(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButtonDown(button);
			}

			// Token: 0x06006D04 RID: 27908 RVA: 0x0003BB65 File Offset: 0x00039D65
			bool IMouseInputSource.GetButtonUp(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButtonUp(button);
			}

			// Token: 0x06006D05 RID: 27909 RVA: 0x0003BB73 File Offset: 0x00039D73
			bool IMouseInputSource.GetButton(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButton(button);
			}

			// Token: 0x170023CC RID: 9164
			// (get) Token: 0x06006D06 RID: 27910 RVA: 0x0003BB81 File Offset: 0x00039D81
			Vector2 IMouseInputSource.screenPosition
			{
				get
				{
					this.TryUpdate();
					return Input.mousePosition;
				}
			}

			// Token: 0x170023CD RID: 9165
			// (get) Token: 0x06006D07 RID: 27911 RVA: 0x0003BB93 File Offset: 0x00039D93
			Vector2 IMouseInputSource.screenPositionDelta
			{
				get
				{
					this.TryUpdate();
					return this.m_MousePosition - this.m_MousePositionPrev;
				}
			}

			// Token: 0x170023CE RID: 9166
			// (get) Token: 0x06006D08 RID: 27912 RVA: 0x0003BBAC File Offset: 0x00039DAC
			Vector2 IMouseInputSource.wheelDelta
			{
				get
				{
					this.TryUpdate();
					return Input.mouseScrollDelta;
				}
			}

			// Token: 0x170023CF RID: 9167
			// (get) Token: 0x06006D09 RID: 27913 RVA: 0x0003BBB9 File Offset: 0x00039DB9
			bool ITouchInputSource.touchSupported
			{
				get
				{
					this.TryUpdate();
					return Input.touchSupported;
				}
			}

			// Token: 0x170023D0 RID: 9168
			// (get) Token: 0x06006D0A RID: 27914 RVA: 0x0003BBC6 File Offset: 0x00039DC6
			int ITouchInputSource.touchCount
			{
				get
				{
					this.TryUpdate();
					return Input.touchCount;
				}
			}

			// Token: 0x06006D0B RID: 27915 RVA: 0x0003BBD3 File Offset: 0x00039DD3
			Touch ITouchInputSource.GetTouch(int index)
			{
				this.TryUpdate();
				return Input.GetTouch(index);
			}

			// Token: 0x06006D0C RID: 27916 RVA: 0x0003BBE1 File Offset: 0x00039DE1
			private void TryUpdate()
			{
				if (Time.frameCount == this.m_LastUpdatedFrame)
				{
					return;
				}
				this.m_LastUpdatedFrame = Time.frameCount;
				this.m_MousePositionPrev = this.m_MousePosition;
				this.m_MousePosition = Input.mousePosition;
			}

			// Token: 0x040057DD RID: 22493
			private Vector2 m_MousePosition;

			// Token: 0x040057DE RID: 22494
			private Vector2 m_MousePositionPrev;

			// Token: 0x040057DF RID: 22495
			private int m_LastUpdatedFrame = -1;
		}
	}
}
