using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000EEA RID: 3818
	[AddComponentMenu("")]
	public class SimpleCombinedKeyboardMouseRemapping : MonoBehaviour
	{
		// Token: 0x1700240C RID: 9228
		// (get) Token: 0x06006E72 RID: 28274 RVA: 0x0003CC1D File Offset: 0x0003AE1D
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(0);
			}
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x0018AF1C File Offset: 0x0018911C
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

		// Token: 0x06006E74 RID: 28276 RVA: 0x0003CC2A File Offset: 0x0003AE2A
		private void OnDisable()
		{
			this.inputMapper_keyboard.Stop();
			this.inputMapper_mouse.Stop();
			this.inputMapper_keyboard.RemoveAllEventListeners();
			this.inputMapper_mouse.RemoveAllEventListeners();
		}

		// Token: 0x06006E75 RID: 28277 RVA: 0x0018B004 File Offset: 0x00189204
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

		// Token: 0x06006E76 RID: 28278 RVA: 0x0018B170 File Offset: 0x00189370
		private void ClearUI()
		{
			this.controllerNameUIText.text = string.Empty;
			for (int i = 0; i < this.rows.Count; i++)
			{
				this.rows[i].text.text = string.Empty;
			}
		}

		// Token: 0x06006E77 RID: 28279 RVA: 0x0018B1C0 File Offset: 0x001893C0
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

		// Token: 0x06006E78 RID: 28280 RVA: 0x0018B338 File Offset: 0x00189538
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

		// Token: 0x06006E79 RID: 28281 RVA: 0x0018B3D8 File Offset: 0x001895D8
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

		// Token: 0x06006E7A RID: 28282 RVA: 0x0003CC58 File Offset: 0x0003AE58
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

		// Token: 0x06006E7B RID: 28283 RVA: 0x0018B488 File Offset: 0x00189688
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

		// Token: 0x06006E7C RID: 28284 RVA: 0x0003CC84 File Offset: 0x0003AE84
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.statusUIText.text = string.Empty;
			this.player.controllers.maps.SetMapsEnabled(true, "UI");
		}

		// Token: 0x040058C8 RID: 22728
		private const string category = "Default";

		// Token: 0x040058C9 RID: 22729
		private const string layout = "Default";

		// Token: 0x040058CA RID: 22730
		private const string uiCategory = "UI";

		// Token: 0x040058CB RID: 22731
		private InputMapper inputMapper_keyboard = new InputMapper();

		// Token: 0x040058CC RID: 22732
		private InputMapper inputMapper_mouse = new InputMapper();

		// Token: 0x040058CD RID: 22733
		public GameObject buttonPrefab;

		// Token: 0x040058CE RID: 22734
		public GameObject textPrefab;

		// Token: 0x040058CF RID: 22735
		public RectTransform fieldGroupTransform;

		// Token: 0x040058D0 RID: 22736
		public RectTransform actionGroupTransform;

		// Token: 0x040058D1 RID: 22737
		public Text controllerNameUIText;

		// Token: 0x040058D2 RID: 22738
		public Text statusUIText;

		// Token: 0x040058D3 RID: 22739
		private List<SimpleCombinedKeyboardMouseRemapping.Row> rows = new List<SimpleCombinedKeyboardMouseRemapping.Row>();

		// Token: 0x040058D4 RID: 22740
		private SimpleCombinedKeyboardMouseRemapping.TargetMapping _replaceTargetMapping;

		// Token: 0x02000EEB RID: 3819
		private class Row
		{
			// Token: 0x040058D5 RID: 22741
			public InputAction action;

			// Token: 0x040058D6 RID: 22742
			public AxisRange actionRange;

			// Token: 0x040058D7 RID: 22743
			public Button button;

			// Token: 0x040058D8 RID: 22744
			public Text text;
		}

		// Token: 0x02000EEC RID: 3820
		private struct TargetMapping
		{
			// Token: 0x040058D9 RID: 22745
			public ControllerMap controllerMap;

			// Token: 0x040058DA RID: 22746
			public int actionElementMapId;
		}
	}
}
