using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x0200094B RID: 2379
	[AddComponentMenu("")]
	public class SimpleCombinedKeyboardMouseRemapping : MonoBehaviour
	{
		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x06005097 RID: 20631 RVA: 0x0011C93D File Offset: 0x0011AB3D
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x0011C94C File Offset: 0x0011AB4C
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.inputMapper_keyboard.options.timeout = 5f;
			this.inputMapper_mouse.options.timeout = 5f;
			this.inputMapper_mouse.options.ignoreMouseXAxis = true;
			this.inputMapper_mouse.options.ignoreMouseYAxis = true;
			this.inputMapper_keyboard.options.allowButtonsOnFullAxisAssignment = false;
			this.inputMapper_mouse.options.allowButtonsOnFullAxisAssignment = false;
			this.inputMapper_keyboard.InputMappedEvent += this.OnInputMapped;
			this.inputMapper_keyboard.StoppedEvent += this.OnStopped;
			this.inputMapper_mouse.InputMappedEvent += this.OnInputMapped;
			this.inputMapper_mouse.StoppedEvent += this.OnStopped;
			this.InitializeUI();
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x0011CA31 File Offset: 0x0011AC31
		private void OnDisable()
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			this.inputMapper_keyboard.RemoveAllEventListeners();
			this.inputMapper_mouse.RemoveAllEventListeners();
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x0011CA60 File Offset: 0x0011AC60
		private void RedrawUI()
		{
			this.controllerNameUIText.text = "Keyboard/Mouse";
			for (int i = 0; i < this.rows.Count; i++)
			{
				SimpleCombinedKeyboardMouseRemapping.Row row = this.rows[i];
				InputAction action = this.rows[i].action;
				string text = string.Empty;
				int actionElementMapId = -1;
				for (int j = 0; j < 2; j++)
				{
					ControllerType controllerType = (j == 0) ? ControllerType.Keyboard : ControllerType.Mouse;
					foreach (ActionElementMap actionElementMap in this.player.controllers.maps.GetMap(controllerType, 0, "Default", "Default").ElementMapsWithAction(action.id))
					{
						if (actionElementMap.ShowInField(row.actionRange))
						{
							text = actionElementMap.elementIdentifierName;
							actionElementMapId = actionElementMap.id;
							break;
						}
					}
					if (actionElementMapId >= 0)
					{
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

		// Token: 0x0600509B RID: 20635 RVA: 0x0011CBCC File Offset: 0x0011ADCC
		private void ClearUI()
		{
			this.controllerNameUIText.text = string.Empty;
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x0011CC1C File Offset: 0x0011AE1C
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

		// Token: 0x0600509D RID: 20637 RVA: 0x0011CD94 File Offset: 0x0011AF94
		private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
			gameObject.transform.SetParent(this.actionGroupTransform);
			gameObject.transform.SetAsLastSibling();
			gameObject.GetComponent<Text>().text = label;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
			gameObject2.transform.SetParent(this.fieldGroupTransform);
			gameObject2.transform.SetAsLastSibling();
			this.rows.Add(new SimpleCombinedKeyboardMouseRemapping.Row
			{
				action = action,
				actionRange = actionRange,
				button = gameObject2.GetComponent<Button>(),
				text = gameObject2.GetComponentInChildren<Text>()
			});
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x0011CE34 File Offset: 0x0011B034
		private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
		{
			if (index < 0 || index >= this.rows.Count)
			{
				return;
			}
			ControllerMap map = this.player.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
			ControllerMap map2 = this.player.controllers.maps.GetMap(ControllerType.Mouse, 0, "Default", "Default");
			ControllerMap controllerMap;
			if (map.ContainsElementMap(actionElementMapToReplaceId))
			{
				controllerMap = map;
			}
			else if (map2.ContainsElementMap(actionElementMapToReplaceId))
			{
				controllerMap = map2;
			}
			else
			{
				controllerMap = null;
			}
			this._replaceTargetMapping = new SimpleCombinedKeyboardMouseRemapping.TargetMapping
			{
				actionElementMapId = actionElementMapToReplaceId,
				controllerMap = controllerMap
			};
			base.StartCoroutine(this.StartListeningDelayed(index, map, map2, actionElementMapToReplaceId));
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x0011CEE4 File Offset: 0x0011B0E4
		private IEnumerator StartListeningDelayed(int index, ControllerMap keyboardMap, ControllerMap mouseMap, int actionElementMapToReplaceId)
		{
			yield return new WaitForSeconds(0.1f);
			this.inputMapper_keyboard.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = keyboardMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = keyboardMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.inputMapper_mouse.Start(new InputMapper.Context
			{
				actionId = this.rows[index].action.id,
				controllerMap = mouseMap,
				actionRange = this.rows[index].actionRange,
				actionElementMapToReplace = mouseMap.GetElementMap(actionElementMapToReplaceId)
			});
			this.player.controllers.maps.SetMapsEnabled(false, "UI");
			this.statusUIText.text = "Listening...";
			yield break;
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x0011CF10 File Offset: 0x0011B110
		private void OnInputMapped(InputMapper.InputMappedEventData data)
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			if (this._replaceTargetMapping.controllerMap != null && data.actionElementMap.controllerMap != this._replaceTargetMapping.controllerMap)
			{
				this._replaceTargetMapping.controllerMap.DeleteElementMap(this._replaceTargetMapping.actionElementMapId);
			}
			this.RedrawUI();
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x0011CF7A File Offset: 0x0011B17A
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x040042E3 RID: 17123
		private const string category = "Default";

		// Token: 0x040042E4 RID: 17124
		private const string layout = "Default";

		// Token: 0x040042E5 RID: 17125
		private const string uiCategory = "UI";

		// Token: 0x040042E6 RID: 17126
		private InputMapper inputMapper_keyboard = new InputMapper();

		// Token: 0x040042E7 RID: 17127
		private InputMapper inputMapper_mouse = new InputMapper();

		// Token: 0x040042E8 RID: 17128
		public GameObject buttonPrefab;

		// Token: 0x040042E9 RID: 17129
		public GameObject textPrefab;

		// Token: 0x040042EA RID: 17130
		public RectTransform fieldGroupTransform;

		// Token: 0x040042EB RID: 17131
		public RectTransform actionGroupTransform;

		// Token: 0x040042EC RID: 17132
		public Text controllerNameUIText;

		// Token: 0x040042ED RID: 17133
		public Text statusUIText;

		// Token: 0x040042EE RID: 17134
		private List<SimpleCombinedKeyboardMouseRemapping.Row> rows = new List<SimpleCombinedKeyboardMouseRemapping.Row>();

		// Token: 0x040042EF RID: 17135
		private SimpleCombinedKeyboardMouseRemapping.TargetMapping _replaceTargetMapping;

		// Token: 0x02000F0C RID: 3852
		private class Row
		{
			// Token: 0x04005A47 RID: 23111
			public InputAction action;

			// Token: 0x04005A48 RID: 23112
			public AxisRange actionRange;

			// Token: 0x04005A49 RID: 23113
			public Button button;

			// Token: 0x04005A4A RID: 23114
			public Text text;
		}

		// Token: 0x02000F0D RID: 3853
		private struct TargetMapping
		{
			// Token: 0x04005A4B RID: 23115
			public ControllerMap controllerMap;

			// Token: 0x04005A4C RID: 23116
			public int actionElementMapId;
		}
	}
}
