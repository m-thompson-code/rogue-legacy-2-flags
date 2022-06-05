using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x02000EF5 RID: 3829
	public class GamepadTemplateUI : MonoBehaviour
	{
		// Token: 0x17002415 RID: 9237
		// (get) Token: 0x06006EAE RID: 28334 RVA: 0x0003CF5A File Offset: 0x0003B15A
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06006EAF RID: 28335 RVA: 0x0018BFA4 File Offset: 0x0018A1A4
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

		// Token: 0x06006EB0 RID: 28336 RVA: 0x0003CF6C File Offset: 0x0003B16C
		private void Start()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawLabels();
		}

		// Token: 0x06006EB1 RID: 28337 RVA: 0x0003CF7C File Offset: 0x0003B17C
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		}

		// Token: 0x06006EB2 RID: 28338 RVA: 0x0003CFA0 File Offset: 0x0003B1A0
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.DrawActiveElements();
		}

		// Token: 0x06006EB3 RID: 28339 RVA: 0x0018C1BC File Offset: 0x0018A3BC
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

		// Token: 0x06006EB4 RID: 28340 RVA: 0x0018C244 File Offset: 0x0018A444
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

		// Token: 0x06006EB5 RID: 28341 RVA: 0x0018C360 File Offset: 0x0018A560
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

		// Token: 0x06006EB6 RID: 28342 RVA: 0x0018C3C4 File Offset: 0x0018A5C4
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

		// Token: 0x06006EB7 RID: 28343 RVA: 0x0018C450 File Offset: 0x0018A650
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

		// Token: 0x06006EB8 RID: 28344 RVA: 0x0018C5A8 File Offset: 0x0018A7A8
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

		// Token: 0x06006EB9 RID: 28345 RVA: 0x0003CFB0 File Offset: 0x0003B1B0
		private void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x0003CFB0 File Offset: 0x0003B1B0
		private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.DrawLabels();
		}

		// Token: 0x04005910 RID: 22800
		private const float stickRadius = 20f;

		// Token: 0x04005911 RID: 22801
		public int playerId;

		// Token: 0x04005912 RID: 22802
		[SerializeField]
		private RectTransform leftStick;

		// Token: 0x04005913 RID: 22803
		[SerializeField]
		private RectTransform rightStick;

		// Token: 0x04005914 RID: 22804
		[SerializeField]
		private ControllerUIElement leftStickX;

		// Token: 0x04005915 RID: 22805
		[SerializeField]
		private ControllerUIElement leftStickY;

		// Token: 0x04005916 RID: 22806
		[SerializeField]
		private ControllerUIElement leftStickButton;

		// Token: 0x04005917 RID: 22807
		[SerializeField]
		private ControllerUIElement rightStickX;

		// Token: 0x04005918 RID: 22808
		[SerializeField]
		private ControllerUIElement rightStickY;

		// Token: 0x04005919 RID: 22809
		[SerializeField]
		private ControllerUIElement rightStickButton;

		// Token: 0x0400591A RID: 22810
		[SerializeField]
		private ControllerUIElement actionBottomRow1;

		// Token: 0x0400591B RID: 22811
		[SerializeField]
		private ControllerUIElement actionBottomRow2;

		// Token: 0x0400591C RID: 22812
		[SerializeField]
		private ControllerUIElement actionBottomRow3;

		// Token: 0x0400591D RID: 22813
		[SerializeField]
		private ControllerUIElement actionTopRow1;

		// Token: 0x0400591E RID: 22814
		[SerializeField]
		private ControllerUIElement actionTopRow2;

		// Token: 0x0400591F RID: 22815
		[SerializeField]
		private ControllerUIElement actionTopRow3;

		// Token: 0x04005920 RID: 22816
		[SerializeField]
		private ControllerUIElement leftShoulder;

		// Token: 0x04005921 RID: 22817
		[SerializeField]
		private ControllerUIElement leftTrigger;

		// Token: 0x04005922 RID: 22818
		[SerializeField]
		private ControllerUIElement rightShoulder;

		// Token: 0x04005923 RID: 22819
		[SerializeField]
		private ControllerUIElement rightTrigger;

		// Token: 0x04005924 RID: 22820
		[SerializeField]
		private ControllerUIElement center1;

		// Token: 0x04005925 RID: 22821
		[SerializeField]
		private ControllerUIElement center2;

		// Token: 0x04005926 RID: 22822
		[SerializeField]
		private ControllerUIElement center3;

		// Token: 0x04005927 RID: 22823
		[SerializeField]
		private ControllerUIElement dPadUp;

		// Token: 0x04005928 RID: 22824
		[SerializeField]
		private ControllerUIElement dPadRight;

		// Token: 0x04005929 RID: 22825
		[SerializeField]
		private ControllerUIElement dPadDown;

		// Token: 0x0400592A RID: 22826
		[SerializeField]
		private ControllerUIElement dPadLeft;

		// Token: 0x0400592B RID: 22827
		private GamepadTemplateUI.UIElement[] _uiElementsArray;

		// Token: 0x0400592C RID: 22828
		private Dictionary<int, ControllerUIElement> _uiElements = new Dictionary<int, ControllerUIElement>();

		// Token: 0x0400592D RID: 22829
		private IList<ControllerTemplateElementTarget> _tempTargetList = new List<ControllerTemplateElementTarget>(2);

		// Token: 0x0400592E RID: 22830
		private GamepadTemplateUI.Stick[] _sticks;

		// Token: 0x02000EF6 RID: 3830
		private class Stick
		{
			// Token: 0x17002416 RID: 9238
			// (get) Token: 0x06006EBC RID: 28348 RVA: 0x0003CFD7 File Offset: 0x0003B1D7
			// (set) Token: 0x06006EBD RID: 28349 RVA: 0x0003D003 File Offset: 0x0003B203
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

			// Token: 0x06006EBE RID: 28350 RVA: 0x0018C5E4 File Offset: 0x0018A7E4
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

			// Token: 0x06006EBF RID: 28351 RVA: 0x0003D02B File Offset: 0x0003B22B
			public void Reset()
			{
				if (this._transform == null)
				{
					return;
				}
				this._transform.anchoredPosition = this._origPosition;
			}

			// Token: 0x06006EC0 RID: 28352 RVA: 0x0003D04D File Offset: 0x0003B24D
			public bool ContainsElement(int elementId)
			{
				return !(this._transform == null) && (elementId == this._xAxisElementId || elementId == this._yAxisElementId);
			}

			// Token: 0x06006EC1 RID: 28353 RVA: 0x0018C638 File Offset: 0x0018A838
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

			// Token: 0x0400592F RID: 22831
			private RectTransform _transform;

			// Token: 0x04005930 RID: 22832
			private Vector2 _origPosition;

			// Token: 0x04005931 RID: 22833
			private int _xAxisElementId = -1;

			// Token: 0x04005932 RID: 22834
			private int _yAxisElementId = -1;
		}

		// Token: 0x02000EF7 RID: 3831
		private class UIElement
		{
			// Token: 0x06006EC2 RID: 28354 RVA: 0x0003D073 File Offset: 0x0003B273
			public UIElement(int id, ControllerUIElement element)
			{
				this.id = id;
				this.element = element;
			}

			// Token: 0x04005933 RID: 22835
			public int id;

			// Token: 0x04005934 RID: 22836
			public ControllerUIElement element;
		}
	}
}
