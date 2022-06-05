using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000EEF RID: 3823
	[AddComponentMenu("")]
	public class SimpleControlRemapping : MonoBehaviour
	{
		// Token: 0x1700240F RID: 9231
		// (get) Token: 0x06006E87 RID: 28295 RVA: 0x0003CC1D File Offset: 0x0003AE1D
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x17002410 RID: 9232
		// (get) Token: 0x06006E88 RID: 28296 RVA: 0x0018B650 File Offset: 0x00189850
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

		// Token: 0x17002411 RID: 9233
		// (get) Token: 0x06006E89 RID: 28297 RVA: 0x0003CD0B File Offset: 0x0003AF0B
		private Controller controller
		{
			get
			{
				return this.player.controllers.GetController(this.selectedControllerType, this.selectedControllerId);
			}
		}

		// Token: 0x06006E8A RID: 28298 RVA: 0x0018B69C File Offset: 0x0018989C
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

		// Token: 0x06006E8B RID: 28299 RVA: 0x0003CD29 File Offset: 0x0003AF29
		private void OnDisable()
		{
			this.inputMapper.Stop();
			this.inputMapper.RemoveAllEventListeners();
			ReInput.ControllerConnectedEvent -= this.OnControllerChanged;
			ReInput.ControllerDisconnectedEvent -= this.OnControllerChanged;
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x0018B740 File Offset: 0x00189940
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

		// Token: 0x06006E8D RID: 28301 RVA: 0x0018B87C File Offset: 0x00189A7C
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

		// Token: 0x06006E8E RID: 28302 RVA: 0x0018B8E8 File Offset: 0x00189AE8
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

		// Token: 0x06006E8F RID: 28303 RVA: 0x0018BA60 File Offset: 0x00189C60
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

		// Token: 0x06006E90 RID: 28304 RVA: 0x0018BB00 File Offset: 0x00189D00
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

		// Token: 0x06006E91 RID: 28305 RVA: 0x0003CD63 File Offset: 0x0003AF63
		public void OnControllerSelected(int controllerType)
		{
			this.SetSelectedController((ControllerType)controllerType);
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x0003CD6C File Offset: 0x0003AF6C
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

		// Token: 0x06006E93 RID: 28307 RVA: 0x0003CD99 File Offset: 0x0003AF99
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

		// Token: 0x06006E94 RID: 28308 RVA: 0x0003CDB6 File Offset: 0x0003AFB6
		private void OnControllerChanged(ControllerStatusChangedEventArgs args)
		{
			this.SetSelectedController(this.selectedControllerType);
		}

		// Token: 0x06006E95 RID: 28309 RVA: 0x0003CDC4 File Offset: 0x0003AFC4
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.RedrawUI();
		}

		// Token: 0x06006E96 RID: 28310 RVA: 0x0003CDCC File Offset: 0x0003AFCC
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x040058E5 RID: 22757
		private const string category = "Default";

		// Token: 0x040058E6 RID: 22758
		private const string layout = "Default";

		// Token: 0x040058E7 RID: 22759
		private const string uiCategory = "UI";

		// Token: 0x040058E8 RID: 22760
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x040058E9 RID: 22761
		public GameObject buttonPrefab;

		// Token: 0x040058EA RID: 22762
		public GameObject textPrefab;

		// Token: 0x040058EB RID: 22763
		public RectTransform fieldGroupTransform;

		// Token: 0x040058EC RID: 22764
		public RectTransform actionGroupTransform;

		// Token: 0x040058ED RID: 22765
		public Text controllerNameUIText;

		// Token: 0x040058EE RID: 22766
		public Text statusUIText;

		// Token: 0x040058EF RID: 22767
		private ControllerType selectedControllerType;

		// Token: 0x040058F0 RID: 22768
		private int selectedControllerId;

		// Token: 0x040058F1 RID: 22769
		private List<SimpleControlRemapping.Row> rows = new List<SimpleControlRemapping.Row>();

		// Token: 0x02000EF0 RID: 3824
		private class Row
		{
			// Token: 0x040058F2 RID: 22770
			public InputAction action;

			// Token: 0x040058F3 RID: 22771
			public AxisRange actionRange;

			// Token: 0x040058F4 RID: 22772
			public Button button;

			// Token: 0x040058F5 RID: 22773
			public Text text;
		}
	}
}
