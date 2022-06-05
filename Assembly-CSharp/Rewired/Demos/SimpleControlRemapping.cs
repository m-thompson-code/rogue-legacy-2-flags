using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x0200094C RID: 2380
	[AddComponentMenu("")]
	public class SimpleControlRemapping : MonoBehaviour
	{
		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x0011CFD1 File Offset: 0x0011B1D1
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x060050A4 RID: 20644 RVA: 0x0011CFE0 File Offset: 0x0011B1E0
		private ControllerMap controllerMap
		{
			get
			{
				if (this.controller == null)
				{
					return null;
				}
				return this.player.controllers.maps.GetMap(this.controller.type, this.controller.id, "Default", "Default");
			}
		}

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x060050A5 RID: 20645 RVA: 0x0011D02C File Offset: 0x0011B22C
		private Controller controller
		{
			get
			{
				return this.player.controllers.GetController(this.selectedControllerType, this.selectedControllerId);
			}
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0011D04C File Offset: 0x0011B24C
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			ReInput.ControllerConnectedEvent += this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent += this.OnControllerChanged;
			this.inputMapper.InputMappedEvent += this.OnInputMapped;
			this.inputMapper.StoppedEvent += this.OnStopped;
			this.InitializeUI();
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x0011D0EE File Offset: 0x0011B2EE
		private void OnDisable()
		{
			this.inputMapper.Stop();
			this.inputMapper.RemoveAllEventListeners();
			ReInput.ControllerConnectedEvent -= this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerChanged;
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0011D128 File Offset: 0x0011B328
		private void RedrawUI()
		{
			if (this.controller == null)
			{
				this.ClearUI();
				return;
			}
			this.controllerNameUIText.text = this.controller.name;
			for (int i = 0; i < this.rows.Count; i++)
			{
				SimpleControlRemapping.Row row = this.rows[i];
				InputAction action = this.rows[i].action;
				string text = string.Empty;
				int actionElementMapId = -1;
				foreach (ActionElementMap actionElementMap in this.controllerMap.ElementMapsWithAction(action.id))
				{
					if (actionElementMap.ShowInField(row.actionRange))
					{
						text = actionElementMap.elementIdentifierName;
						actionElementMapId = actionElementMap.id;
						break;
					}
				}
				row.text.text = text;
				row.button.onClick.RemoveAllListeners();
				int index = i;
				row.button.onClick.AddListener(delegate()
				{
					this.OnInputFieldClicked(index, actionElementMapId);
				});
			}
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x0011D264 File Offset: 0x0011B464
		private void ClearUI()
		{
			if (this.selectedControllerType == ControllerType.Joystick)
			{
				this.controllerNameUIText.text = "No joysticks attached";
			}
			else
			{
				this.controllerNameUIText.text = string.Empty;
			}
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x0011D2D0 File Offset: 0x0011B4D0
		private void InitializeUI()
		{
			foreach (object obj in this.actionGroupTransform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (object obj2 in this.fieldGroupTransform)
			{
				UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
			}
			foreach (InputAction inputAction in ReInput.mapping.ActionsInCategory("Default"))
			{
				if (inputAction.type == InputActionType.Axis)
				{
					this.CreateUIRow(inputAction, AxisRange.Full, inputAction.descriptiveName);
					this.CreateUIRow(inputAction, AxisRange.Positive, (!string.IsNullOrEmpty(inputAction.positiveDescriptiveName)) ? inputAction.positiveDescriptiveName : (inputAction.descriptiveName + " +"));
					this.CreateUIRow(inputAction, AxisRange.Negative, (!string.IsNullOrEmpty(inputAction.negativeDescriptiveName)) ? inputAction.negativeDescriptiveName : (inputAction.descriptiveName + " -"));
				}
				else if (inputAction.type == InputActionType.Button)
				{
					this.CreateUIRow(inputAction, AxisRange.Positive, inputAction.descriptiveName);
				}
			}
			this.RedrawUI();
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x0011D448 File Offset: 0x0011B648
		private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
			gameObject.transform.SetParent(this.actionGroupTransform);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<Text>().text = label;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
			gameObject2.transform.SetParent(this.fieldGroupTransform);
			gameObject2.transform.SetAsLastSibling();
			this.rows.Add(new SimpleControlRemapping.Row
			{
				action = action,
				actionRange = actionRange,
				button = gameObject2.GetComponent<Button>(),
				text = gameObject2.GetComponentInChildren<Text>()
			});
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x0011D4E8 File Offset: 0x0011B6E8
		private void SetSelectedController(ControllerType controllerType)
		{
			bool flag = false;
			if (controllerType != this.selectedControllerType)
			{
				this.selectedControllerType = controllerType;
				flag = true;
			}
			int num = this.selectedControllerId;
			if (this.selectedControllerType == ControllerType.Joystick)
			{
				if (this.player.controllers.joystickCount > 0)
				{
					this.selectedControllerId = this.player.controllers.Joysticks[0].id;
				}
				else
				{
					this.selectedControllerId = -1;
				}
			}
			else
			{
				this.selectedControllerId = 0;
			}
			if (this.selectedControllerId != num)
			{
				flag = true;
			}
			if (flag)
			{
				this.inputMapper.Stop();
				this.RedrawUI();
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x0011D57E File Offset: 0x0011B77E
		public void OnControllerSelected(int controllerType)
		{
			this.SetSelectedController((ControllerType)controllerType);
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x0011D587 File Offset: 0x0011B787
		private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
		{
			if (index < 0 || index >= this.rows.Count)
			{
				return;
			}
			if (this.controller == null)
			{
				return;
			}
			base.StartCoroutine(this.StartListeningDelayed(index, actionElementMapToReplaceId));
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x0011D5B4 File Offset: 0x0011B7B4
		private IEnumerator StartListeningDelayed(int index, int actionElementMapToReplaceId)
		{
			yield return new WaitForSeconds(0.1f);
			this.inputMapper.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = this.controllerMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = this.controllerMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.player.controllers.maps.SetMapsEnabled(false, "UI");
			this.statusUIText.text = "Listening...";
			yield break;
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x0011D5D1 File Offset: 0x0011B7D1
		private void OnControllerChanged(ControllerStatusChangedEventArgs args)
		{
			this.SetSelectedController(this.selectedControllerType);
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x0011D5DF File Offset: 0x0011B7DF
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.RedrawUI();
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x0011D5E7 File Offset: 0x0011B7E7
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x040042F0 RID: 17136
		private const string category = "Default";

		// Token: 0x040042F1 RID: 17137
		private const string layout = "Default";

		// Token: 0x040042F2 RID: 17138
		private const string uiCategory = "UI";

		// Token: 0x040042F3 RID: 17139
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x040042F4 RID: 17140
		public GameObject buttonPrefab;

		// Token: 0x040042F5 RID: 17141
		public GameObject textPrefab;

		// Token: 0x040042F6 RID: 17142
		public RectTransform fieldGroupTransform;

		// Token: 0x040042F7 RID: 17143
		public RectTransform actionGroupTransform;

		// Token: 0x040042F8 RID: 17144
		public Text controllerNameUIText;

		// Token: 0x040042F9 RID: 17145
		public Text statusUIText;

		// Token: 0x040042FA RID: 17146
		private ControllerType selectedControllerType;

		// Token: 0x040042FB RID: 17147
		private int selectedControllerId;

		// Token: 0x040042FC RID: 17148
		private List<SimpleControlRemapping.Row> rows = new List<SimpleControlRemapping.Row>();

		// Token: 0x02000F10 RID: 3856
		private class Row
		{
			// Token: 0x04005A57 RID: 23127
			public InputAction action;

			// Token: 0x04005A58 RID: 23128
			public AxisRange actionRange;

			// Token: 0x04005A59 RID: 23129
			public Button button;

			// Token: 0x04005A5A RID: 23130
			public Text text;
		}
	}
}
