using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x0200094F RID: 2383
	public class GamepadTemplateUI : MonoBehaviour
	{
		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x060050C1 RID: 20673 RVA: 0x0011DA6A File Offset: 0x0011BC6A
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x0011DA7C File Offset: 0x0011BC7C
		private void Awake()
		{
			this._uiElementsArray = new GamepadTemplateUI.UIElement[]
			{
				new GamepadTemplateUI.UIElement(0, this.leftStickX),
				new GamepadTemplateUI.UIElement(1, this.leftStickY),
				new GamepadTemplateUI.UIElement(17, this.leftStickButton),
				new GamepadTemplateUI.UIElement(2, this.rightStickX),
				new GamepadTemplateUI.UIElement(3, this.rightStickY),
				new GamepadTemplateUI.UIElement(18, this.rightStickButton),
				new GamepadTemplateUI.UIElement(4, this.actionBottomRow1),
				new GamepadTemplateUI.UIElement(5, this.actionBottomRow2),
				new GamepadTemplateUI.UIElement(6, this.actionBottomRow3),
				new GamepadTemplateUI.UIElement(7, this.actionTopRow1),
				new GamepadTemplateUI.UIElement(8, this.actionTopRow2),
				new GamepadTemplateUI.UIElement(9, this.actionTopRow3),
				new GamepadTemplateUI.UIElement(14, this.center1),
				new GamepadTemplateUI.UIElement(15, this.center2),
				new GamepadTemplateUI.UIElement(16, this.center3),
				new GamepadTemplateUI.UIElement(19, this.dPadUp),
				new GamepadTemplateUI.UIElement(20, this.dPadRight),
				new GamepadTemplateUI.UIElement(21, this.dPadDown),
				new GamepadTemplateUI.UIElement(22, this.dPadLeft),
				new GamepadTemplateUI.UIElement(10, this.leftShoulder),
				new GamepadTemplateUI.UIElement(11, this.leftTrigger),
				new GamepadTemplateUI.UIElement(12, this.rightShoulder),
				new GamepadTemplateUI.UIElement(13, this.rightTrigger)
			};
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElements.Add(this._uiElementsArray[i].id, this._uiElementsArray[i].element);
			}
			this._sticks = new GamepadTemplateUI.Stick[]
			{
				new GamepadTemplateUI.Stick(this.leftStick, 0, 1),
				new GamepadTemplateUI.Stick(this.rightStick, 2, 3)
			};
			ReInput.ControllerConnectedEvent += this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x0011DC91 File Offset: 0x0011BE91
		private void Start()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawLabels();
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x0011DCA1 File Offset: 0x0011BEA1
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x0011DCC5 File Offset: 0x0011BEC5
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawActiveElements();
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x0011DCD8 File Offset: 0x0011BED8
		private void DrawActiveElements()
		{
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElementsArray[i].element.Deactivate();
			}
			for (int j = 0; j < this._sticks.Length; j++)
			{
				this._sticks[j].Reset();
			}
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int k = 0; k < actions.Count; k++)
			{
				this.ActivateElements(this.player, actions[k].id);
			}
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x0011DD60 File Offset: 0x0011BF60
		private void ActivateElements(Player player, int actionId)
		{
			float axis = player.GetAxis(actionId);
			if (axis == 0f)
			{
				return;
			}
			IList<InputActionSourceData> currentInputSources = player.GetCurrentInputSources(actionId);
			for (int i = 0; i < currentInputSources.Count; i++)
			{
				InputActionSourceData inputActionSourceData = currentInputSources[i];
				IGamepadTemplate template = inputActionSourceData.controller.GetTemplate<IGamepadTemplate>();
				if (template != null)
				{
					template.GetElementTargets(inputActionSourceData.actionElementMap, this._tempTargetList);
					for (int j = 0; j < this._tempTargetList.Count; j++)
					{
						ControllerTemplateElementTarget controllerTemplateElementTarget = this._tempTargetList[j];
						int id = controllerTemplateElementTarget.element.id;
						ControllerUIElement controllerUIElement = this._uiElements[id];
						if (controllerTemplateElementTarget.elementType == ControllerTemplateElementType.Axis)
						{
							controllerUIElement.Activate(axis);
						}
						else if (controllerTemplateElementTarget.elementType == ControllerTemplateElementType.Button && (player.GetButton(actionId) || player.GetNegativeButton(actionId)))
						{
							controllerUIElement.Activate(1f);
						}
						GamepadTemplateUI.Stick stick = this.GetStick(id);
						if (stick != null)
						{
							stick.SetAxisPosition(id, axis * 20f);
						}
					}
				}
			}
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x0011DE7C File Offset: 0x0011C07C
		private void DrawLabels()
		{
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				this._uiElementsArray[i].element.ClearLabels();
			}
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int j = 0; j < actions.Count; j++)
			{
				this.DrawLabels(this.player, actions[j]);
			}
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x0011DEE0 File Offset: 0x0011C0E0
		private void DrawLabels(Player player, InputAction action)
		{
			Controller firstControllerWithTemplate = player.controllers.GetFirstControllerWithTemplate<IGamepadTemplate>();
			if (firstControllerWithTemplate == null)
			{
				return;
			}
			IGamepadTemplate template = firstControllerWithTemplate.GetTemplate<IGamepadTemplate>();
			ControllerMap map = player.controllers.maps.GetMap(firstControllerWithTemplate, "Default", "Default");
			if (map == null)
			{
				return;
			}
			for (int i = 0; i < this._uiElementsArray.Length; i++)
			{
				ControllerUIElement element = this._uiElementsArray[i].element;
				int id = this._uiElementsArray[i].id;
				IControllerTemplateElement element2 = template.GetElement(id);
				this.DrawLabel(element, action, map, template, element2);
			}
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x0011DF6C File Offset: 0x0011C16C
		private void DrawLabel(ControllerUIElement uiElement, InputAction action, ControllerMap controllerMap, IControllerTemplate template, IControllerTemplateElement element)
		{
			if (element.source == null)
			{
				return;
			}
			if (element.source.type == ControllerTemplateElementSourceType.Axis)
			{
				IControllerTemplateAxisSource controllerTemplateAxisSource = element.source as IControllerTemplateAxisSource;
				if (controllerTemplateAxisSource.splitAxis)
				{
					ActionElementMap firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.positiveTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Positive);
					}
					firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.negativeTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Negative);
						return;
					}
				}
				else
				{
					ActionElementMap firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateAxisSource.fullTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Full);
						return;
					}
					ControllerElementTarget elementTarget = new ControllerElementTarget(controllerTemplateAxisSource.fullTarget)
					{
						axisRange = AxisRange.Positive
					};
					firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(elementTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Positive);
					}
					elementTarget = new ControllerElementTarget(controllerTemplateAxisSource.fullTarget)
					{
						axisRange = AxisRange.Negative
					};
					firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(elementTarget, action.id, true);
					if (firstElementMapWithElementTarget != null)
					{
						uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Negative);
						return;
					}
				}
			}
			else if (element.source.type == ControllerTemplateElementSourceType.Button)
			{
				IControllerTemplateButtonSource controllerTemplateButtonSource = element.source as IControllerTemplateButtonSource;
				ActionElementMap firstElementMapWithElementTarget = controllerMap.GetFirstElementMapWithElementTarget(controllerTemplateButtonSource.target, action.id, true);
				if (firstElementMapWithElementTarget != null)
				{
					uiElement.SetLabel(firstElementMapWithElementTarget.actionDescriptiveName, AxisRange.Full);
				}
			}
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x0011E0C4 File Offset: 0x0011C2C4
		private GamepadTemplateUI.Stick GetStick(int elementId)
		{
			for (int i = 0; i < this._sticks.Length; i++)
			{
				if (this._sticks[i].ContainsElement(elementId))
				{
					return this._sticks[i];
				}
			}
			return null;
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x0011E0FE File Offset: 0x0011C2FE
		private void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x0011E106 File Offset: 0x0011C306
		private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x0400430F RID: 17167
		private const float stickRadius = 20f;

		// Token: 0x04004310 RID: 17168
		public int playerId;

		// Token: 0x04004311 RID: 17169
		[SerializeField]
		private RectTransform leftStick;

		// Token: 0x04004312 RID: 17170
		[SerializeField]
		private RectTransform rightStick;

		// Token: 0x04004313 RID: 17171
		[SerializeField]
		private ControllerUIElement leftStickX;

		// Token: 0x04004314 RID: 17172
		[SerializeField]
		private ControllerUIElement leftStickY;

		// Token: 0x04004315 RID: 17173
		[SerializeField]
		private ControllerUIElement leftStickButton;

		// Token: 0x04004316 RID: 17174
		[SerializeField]
		private ControllerUIElement rightStickX;

		// Token: 0x04004317 RID: 17175
		[SerializeField]
		private ControllerUIElement rightStickY;

		// Token: 0x04004318 RID: 17176
		[SerializeField]
		private ControllerUIElement rightStickButton;

		// Token: 0x04004319 RID: 17177
		[SerializeField]
		private ControllerUIElement actionBottomRow1;

		// Token: 0x0400431A RID: 17178
		[SerializeField]
		private ControllerUIElement actionBottomRow2;

		// Token: 0x0400431B RID: 17179
		[SerializeField]
		private ControllerUIElement actionBottomRow3;

		// Token: 0x0400431C RID: 17180
		[SerializeField]
		private ControllerUIElement actionTopRow1;

		// Token: 0x0400431D RID: 17181
		[SerializeField]
		private ControllerUIElement actionTopRow2;

		// Token: 0x0400431E RID: 17182
		[SerializeField]
		private ControllerUIElement actionTopRow3;

		// Token: 0x0400431F RID: 17183
		[SerializeField]
		private ControllerUIElement leftShoulder;

		// Token: 0x04004320 RID: 17184
		[SerializeField]
		private ControllerUIElement leftTrigger;

		// Token: 0x04004321 RID: 17185
		[SerializeField]
		private ControllerUIElement rightShoulder;

		// Token: 0x04004322 RID: 17186
		[SerializeField]
		private ControllerUIElement rightTrigger;

		// Token: 0x04004323 RID: 17187
		[SerializeField]
		private ControllerUIElement center1;

		// Token: 0x04004324 RID: 17188
		[SerializeField]
		private ControllerUIElement center2;

		// Token: 0x04004325 RID: 17189
		[SerializeField]
		private ControllerUIElement center3;

		// Token: 0x04004326 RID: 17190
		[SerializeField]
		private ControllerUIElement dPadUp;

		// Token: 0x04004327 RID: 17191
		[SerializeField]
		private ControllerUIElement dPadRight;

		// Token: 0x04004328 RID: 17192
		[SerializeField]
		private ControllerUIElement dPadDown;

		// Token: 0x04004329 RID: 17193
		[SerializeField]
		private ControllerUIElement dPadLeft;

		// Token: 0x0400432A RID: 17194
		private GamepadTemplateUI.UIElement[] _uiElementsArray;

		// Token: 0x0400432B RID: 17195
		private Dictionary<int, ControllerUIElement> _uiElements = new Dictionary<int, ControllerUIElement>();

		// Token: 0x0400432C RID: 17196
		private IList<ControllerTemplateElementTarget> _tempTargetList = new List<ControllerTemplateElementTarget>(2);

		// Token: 0x0400432D RID: 17197
		private GamepadTemplateUI.Stick[] _sticks;

		// Token: 0x02000F13 RID: 3859
		private class Stick
		{
			// Token: 0x1700246C RID: 9324
			// (get) Token: 0x06007029 RID: 28713 RVA: 0x0019EFD0 File Offset: 0x0019D1D0
			// (set) Token: 0x0600702A RID: 28714 RVA: 0x0019EFFC File Offset: 0x0019D1FC
			public Vector2 position
			{
				get
				{
					if (!(this._transform != null))
					{
						return Vector2.zero;
					}
					return this._transform.anchoredPosition - this._origPosition;
				}
				set
				{
					if (this._transform == null)
					{
						return;
					}
					this._transform.anchoredPosition = this._origPosition + value;
				}
			}

			// Token: 0x0600702B RID: 28715 RVA: 0x0019F024 File Offset: 0x0019D224
			public Stick(RectTransform transform, int xAxisElementId, int yAxisElementId)
			{
				if (transform == null)
				{
					return;
				}
				this._transform = transform;
				this._origPosition = this._transform.anchoredPosition;
				this._xAxisElementId = xAxisElementId;
				this._yAxisElementId = yAxisElementId;
			}

			// Token: 0x0600702C RID: 28716 RVA: 0x0019F075 File Offset: 0x0019D275
			public void Reset()
			{
				if (this._transform == null)
				{
					return;
				}
				this._transform.anchoredPosition = this._origPosition;
			}

			// Token: 0x0600702D RID: 28717 RVA: 0x0019F097 File Offset: 0x0019D297
			public bool ContainsElement(int elementId)
			{
				return !(this._transform == null) && (elementId == this._xAxisElementId || elementId == this._yAxisElementId);
			}

			// Token: 0x0600702E RID: 28718 RVA: 0x0019F0C0 File Offset: 0x0019D2C0
			public void SetAxisPosition(int elementId, float value)
			{
				if (this._transform == null)
				{
					return;
				}
				Vector2 position = this.position;
				if (elementId == this._xAxisElementId)
				{
					position.x = value;
				}
				else if (elementId == this._yAxisElementId)
				{
					position.y = value;
				}
				this.position = position;
			}

			// Token: 0x04005A63 RID: 23139
			private RectTransform _transform;

			// Token: 0x04005A64 RID: 23140
			private Vector2 _origPosition;

			// Token: 0x04005A65 RID: 23141
			private int _xAxisElementId = -1;

			// Token: 0x04005A66 RID: 23142
			private int _yAxisElementId = -1;
		}

		// Token: 0x02000F14 RID: 3860
		private class UIElement
		{
			// Token: 0x0600702F RID: 28719 RVA: 0x0019F10E File Offset: 0x0019D30E
			public UIElement(int id, ControllerUIElement element)
			{
				this.id = id;
				this.element = element;
			}

			// Token: 0x04005A67 RID: 23143
			public int id;

			// Token: 0x04005A68 RID: 23144
			public ControllerUIElement element;
		}
	}
}
