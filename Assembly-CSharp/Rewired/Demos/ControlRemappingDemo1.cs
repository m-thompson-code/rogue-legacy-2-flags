using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EC9 RID: 3785
	[AddComponentMenu("")]
	public class ControlRemappingDemo1 : MonoBehaviour
	{
		// Token: 0x06006D6C RID: 28012 RVA: 0x0003C034 File Offset: 0x0003A234
		private void Awake()
		{
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			this.Initialize();
		}

		// Token: 0x06006D6D RID: 28013 RVA: 0x0003C073 File Offset: 0x0003A273
		private void OnEnable()
		{
			this.Subscribe();
		}

		// Token: 0x06006D6E RID: 28014 RVA: 0x0003C07B File Offset: 0x0003A27B
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x06006D6F RID: 28015 RVA: 0x00186A64 File Offset: 0x00184C64
		private void Initialize()
		{
			this.dialog = new ControlRemappingDemo1.DialogHelper();
			this.actionQueue = new Queue<ControlRemappingDemo1.QueueEntry>();
			this.selectedController = new ControlRemappingDemo1.ControllerSelection();
			ReInput.ControllerConnectedEvent += this.JoystickConnected;
			ReInput.ControllerPreDisconnectEvent += this.JoystickPreDisconnect;
			ReInput.ControllerDisconnectedEvent += this.JoystickDisconnected;
			this.ResetAll();
			this.initialized = true;
			ReInput.userDataStore.Load();
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06006D70 RID: 28016 RVA: 0x00186AEC File Offset: 0x00184CEC
		private void Setup()
		{
			if (this.setupFinished)
			{
				return;
			}
			this.style_wordWrap = new GUIStyle(GUI.skin.label);
			this.style_wordWrap.wordWrap = true;
			this.style_centeredBox = new GUIStyle(GUI.skin.box);
			this.style_centeredBox.alignment = TextAnchor.MiddleCenter;
			this.setupFinished = true;
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x0003C083 File Offset: 0x0003A283
		private void Subscribe()
		{
			this.Unsubscribe();
			this.inputMapper.ConflictFoundEvent += this.OnConflictFound;
			this.inputMapper.StoppedEvent += this.OnStopped;
		}

		// Token: 0x06006D72 RID: 28018 RVA: 0x0003C0B9 File Offset: 0x0003A2B9
		private void Unsubscribe()
		{
			this.inputMapper.RemoveAllEventListeners();
		}

		// Token: 0x06006D73 RID: 28019 RVA: 0x00186B4C File Offset: 0x00184D4C
		public void OnGUI()
		{
			if (!this.initialized)
			{
				return;
			}
			this.Setup();
			this.HandleMenuControl();
			if (!this.showMenu)
			{
				this.DrawInitialScreen();
				return;
			}
			this.SetGUIStateStart();
			this.ProcessQueue();
			this.DrawPage();
			this.ShowDialog();
			this.SetGUIStateEnd();
			this.busy = false;
		}

		// Token: 0x06006D74 RID: 28020 RVA: 0x00186BA4 File Offset: 0x00184DA4
		private void HandleMenuControl()
		{
			if (this.dialog.enabled)
			{
				return;
			}
			if (Event.current.type == EventType.Layout && ReInput.players.GetSystemPlayer().GetButtonDown("Menu"))
			{
				if (this.showMenu)
				{
					ReInput.userDataStore.Save();
					this.Close();
					return;
				}
				this.Open();
			}
		}

		// Token: 0x06006D75 RID: 28021 RVA: 0x0003C0C6 File Offset: 0x0003A2C6
		private void Close()
		{
			this.ClearWorkingVars();
			this.showMenu = false;
		}

		// Token: 0x06006D76 RID: 28022 RVA: 0x0003C0D5 File Offset: 0x0003A2D5
		private void Open()
		{
			this.showMenu = true;
		}

		// Token: 0x06006D77 RID: 28023 RVA: 0x00186C04 File Offset: 0x00184E04
		private void DrawInitialScreen()
		{
			ActionElementMap firstElementMapWithAction = ReInput.players.GetSystemPlayer().controllers.maps.GetFirstElementMapWithAction("Menu", true);
			GUIContent content;
			if (firstElementMapWithAction != null)
			{
				content = new GUIContent("Press " + firstElementMapWithAction.elementIdentifierName + " to open the menu.");
			}
			else
			{
				content = new GUIContent("There is no element assigned to open the menu!");
			}
			GUILayout.BeginArea(this.GetScreenCenteredRect(300f, 50f));
			GUILayout.Box(content, this.style_centeredBox, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true),
				GUILayout.ExpandWidth(true)
			});
			GUILayout.EndArea();
		}

		// Token: 0x06006D78 RID: 28024 RVA: 0x00186C9C File Offset: 0x00184E9C
		private void DrawPage()
		{
			if (GUI.enabled != this.pageGUIState)
			{
				GUI.enabled = this.pageGUIState;
			}
			GUILayout.BeginArea(new Rect(((float)Screen.width - (float)Screen.width * 0.9f) * 0.5f, ((float)Screen.height - (float)Screen.height * 0.9f) * 0.5f, (float)Screen.width * 0.9f, (float)Screen.height * 0.9f));
			this.DrawPlayerSelector();
			this.DrawJoystickSelector();
			this.DrawMouseAssignment();
			this.DrawControllerSelector();
			this.DrawCalibrateButton();
			this.DrawMapCategories();
			this.actionScrollPos = GUILayout.BeginScrollView(this.actionScrollPos, Array.Empty<GUILayoutOption>());
			this.DrawCategoryActions();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		// Token: 0x06006D79 RID: 28025 RVA: 0x00186D60 File Offset: 0x00184F60
		private void DrawPlayerSelector()
		{
			if (ReInput.players.allPlayerCount == 0)
			{
				GUILayout.Label("There are no players.", Array.Empty<GUILayoutOption>());
				return;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Players:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (Player player in ReInput.players.GetPlayers(true))
			{
				if (this.selectedPlayer == null)
				{
					this.selectedPlayer = player;
				}
				bool flag = player == this.selectedPlayer;
				bool flag2 = GUILayout.Toggle(flag, (player.descriptiveName != string.Empty) ? player.descriptiveName : player.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (flag2 != flag && flag2)
				{
					this.selectedPlayer = player;
					this.selectedController.Clear();
					this.selectedMapCategoryId = -1;
				}
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x06006D7A RID: 28026 RVA: 0x00186E74 File Offset: 0x00185074
		private void DrawMouseAssignment()
		{
			bool enabled = GUI.enabled;
			if (this.selectedPlayer == null)
			{
				GUI.enabled = false;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Assign Mouse:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.selectedPlayer != null && this.selectedPlayer.controllers.hasMouse;
			bool flag2 = GUILayout.Toggle(flag, "Assign Mouse", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			if (flag2 != flag)
			{
				if (flag2)
				{
					this.selectedPlayer.controllers.hasMouse = true;
					using (IEnumerator<Player> enumerator = ReInput.players.Players.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Player player = enumerator.Current;
							if (player != this.selectedPlayer)
							{
								player.controllers.hasMouse = false;
							}
						}
						goto IL_E9;
					}
				}
				this.selectedPlayer.controllers.hasMouse = false;
			}
			IL_E9:
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D7B RID: 28027 RVA: 0x00186F90 File Offset: 0x00185190
		private void DrawJoystickSelector()
		{
			bool enabled = GUI.enabled;
			if (this.selectedPlayer == null)
			{
				GUI.enabled = false;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Assign Joysticks:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag = this.selectedPlayer == null || this.selectedPlayer.controllers.joystickCount == 0;
			bool flag2 = GUILayout.Toggle(flag, "None", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			if (flag2 != flag)
			{
				this.selectedPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
				this.ControllerSelectionChanged();
			}
			if (this.selectedPlayer != null)
			{
				foreach (Joystick joystick in ReInput.controllers.Joysticks)
				{
					flag = this.selectedPlayer.controllers.ContainsController(joystick);
					flag2 = GUILayout.Toggle(flag, joystick.name, "Button", new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					});
					if (flag2 != flag)
					{
						this.EnqueueAction(new ControlRemappingDemo1.JoystickAssignmentChange(this.selectedPlayer.id, joystick.id, flag2));
					}
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x001870EC File Offset: 0x001852EC
		private void DrawControllerSelector()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(15f);
			GUILayout.Label("Controller to Map:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (!this.selectedController.hasSelection)
			{
				this.selectedController.Set(0, ControllerType.Keyboard);
				this.ControllerSelectionChanged();
			}
			bool flag = this.selectedController.type == ControllerType.Keyboard;
			if (GUILayout.Toggle(flag, "Keyboard", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}) != flag)
			{
				this.selectedController.Set(0, ControllerType.Keyboard);
				this.ControllerSelectionChanged();
			}
			if (!this.selectedPlayer.controllers.hasMouse)
			{
				GUI.enabled = false;
			}
			flag = (this.selectedController.type == ControllerType.Mouse);
			if (GUILayout.Toggle(flag, "Mouse", "Button", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}) != flag)
			{
				this.selectedController.Set(0, ControllerType.Mouse);
				this.ControllerSelectionChanged();
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
			foreach (Joystick joystick in this.selectedPlayer.controllers.Joysticks)
			{
				flag = (this.selectedController.type == ControllerType.Joystick && this.selectedController.id == joystick.id);
				if (GUILayout.Toggle(flag, joystick.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}) != flag)
				{
					this.selectedController.Set(joystick.id, ControllerType.Joystick);
					this.ControllerSelectionChanged();
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D7D RID: 28029 RVA: 0x001872C0 File Offset: 0x001854C0
		private void DrawCalibrateButton()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(10f);
			Controller controller = this.selectedController.hasSelection ? this.selectedPlayer.controllers.GetController(this.selectedController.type, this.selectedController.id) : null;
			if (controller == null || this.selectedController.type != ControllerType.Joystick)
			{
				GUI.enabled = false;
				GUILayout.Button("Select a controller to calibrate", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			else if (GUILayout.Button("Calibrate " + controller.name, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				Joystick joystick = controller as Joystick;
				if (joystick != null)
				{
					CalibrationMap calibrationMap = joystick.calibrationMap;
					if (calibrationMap != null)
					{
						this.EnqueueAction(new ControlRemappingDemo1.Calibration(this.selectedPlayer, joystick, calibrationMap));
					}
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x001873BC File Offset: 0x001855BC
		private void DrawMapCategories()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			if (!this.selectedController.hasSelection)
			{
				return;
			}
			bool enabled = GUI.enabled;
			GUILayout.Space(15f);
			GUILayout.Label("Categories:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			foreach (InputMapCategory inputMapCategory in ReInput.mapping.UserAssignableMapCategories)
			{
				if (!this.selectedPlayer.controllers.maps.ContainsMapInCategory(this.selectedController.type, inputMapCategory.id))
				{
					GUI.enabled = false;
				}
				else if (this.selectedMapCategoryId < 0)
				{
					this.selectedMapCategoryId = inputMapCategory.id;
					this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, inputMapCategory.id);
				}
				bool flag = inputMapCategory.id == this.selectedMapCategoryId;
				if (GUILayout.Toggle(flag, (inputMapCategory.descriptiveName != string.Empty) ? inputMapCategory.descriptiveName : inputMapCategory.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}) != flag)
				{
					this.selectedMapCategoryId = inputMapCategory.id;
					this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, inputMapCategory.id);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			GUILayout.EndHorizontal();
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x00187590 File Offset: 0x00185790
		private void DrawCategoryActions()
		{
			if (this.selectedPlayer == null)
			{
				return;
			}
			if (this.selectedMapCategoryId < 0)
			{
				return;
			}
			bool enabled = GUI.enabled;
			if (this.selectedMap == null)
			{
				return;
			}
			GUILayout.Space(15f);
			GUILayout.Label("Actions:", Array.Empty<GUILayoutOption>());
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(this.selectedMapCategoryId);
			if (mapCategory == null)
			{
				return;
			}
			InputCategory actionCategory = ReInput.mapping.GetActionCategory(mapCategory.name);
			if (actionCategory == null)
			{
				return;
			}
			float width = 150f;
			foreach (InputAction inputAction in ReInput.mapping.ActionsInCategory(actionCategory.id))
			{
				string text = (inputAction.descriptiveName != string.Empty) ? inputAction.descriptiveName : inputAction.name;
				if (inputAction.type == InputActionType.Button)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text, new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap in this.selectedMap.AllMaps)
					{
						if (actionElementMap.actionId == inputAction.id)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap, actionElementMap);
						}
					}
					GUILayout.EndHorizontal();
				}
				else if (inputAction.type == InputActionType.Axis)
				{
					if (this.selectedController.type != ControllerType.Keyboard)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label(text, new GUILayoutOption[]
						{
							GUILayout.Width(width)
						});
						this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Full, this.selectedController, this.selectedMap);
						foreach (ActionElementMap actionElementMap2 in this.selectedMap.AllMaps)
						{
							if (actionElementMap2.actionId == inputAction.id && actionElementMap2.elementType != ControllerElementType.Button && actionElementMap2.axisType != AxisType.Split)
							{
								this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Full, this.selectedController, this.selectedMap, actionElementMap2);
								this.DrawInvertButton(this.selectedPlayer.id, inputAction, Pole.Positive, this.selectedController, this.selectedMap, actionElementMap2);
							}
						}
						GUILayout.EndHorizontal();
					}
					string text2 = (inputAction.positiveDescriptiveName != string.Empty) ? inputAction.positiveDescriptiveName : (inputAction.descriptiveName + " +");
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text2, new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap3 in this.selectedMap.AllMaps)
					{
						if (actionElementMap3.actionId == inputAction.id && actionElementMap3.axisContribution == Pole.Positive && actionElementMap3.axisType != AxisType.Normal)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Positive, this.selectedController, this.selectedMap, actionElementMap3);
						}
					}
					GUILayout.EndHorizontal();
					string text3 = (inputAction.negativeDescriptiveName != string.Empty) ? inputAction.negativeDescriptiveName : (inputAction.descriptiveName + " -");
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(text3, new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					this.DrawAddActionMapButton(this.selectedPlayer.id, inputAction, AxisRange.Negative, this.selectedController, this.selectedMap);
					foreach (ActionElementMap actionElementMap4 in this.selectedMap.AllMaps)
					{
						if (actionElementMap4.actionId == inputAction.id && actionElementMap4.axisContribution == Pole.Negative && actionElementMap4.axisType != AxisType.Normal)
						{
							this.DrawActionAssignmentButton(this.selectedPlayer.id, inputAction, AxisRange.Negative, this.selectedController, this.selectedMap, actionElementMap4);
						}
					}
					GUILayout.EndHorizontal();
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D80 RID: 28032 RVA: 0x00187A84 File Offset: 0x00185C84
		private void DrawActionAssignmentButton(int playerId, InputAction action, AxisRange actionRange, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap)
		{
			if (GUILayout.Button(elementMap.elementIdentifierName, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false),
				GUILayout.MinWidth(30f)
			}))
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = action.id,
					actionRange = actionRange,
					controllerMap = controllerMap,
					actionElementMapToReplace = elementMap
				};
				this.EnqueueAction(new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove, context));
				this.startListening = true;
			}
			GUILayout.Space(4f);
		}

		// Token: 0x06006D81 RID: 28033 RVA: 0x00187B04 File Offset: 0x00185D04
		private void DrawInvertButton(int playerId, InputAction action, Pole actionAxisContribution, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap, ActionElementMap elementMap)
		{
			bool invert = elementMap.invert;
			bool flag = GUILayout.Toggle(invert, "Invert", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			if (flag != invert)
			{
				elementMap.invert = flag;
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06006D82 RID: 28034 RVA: 0x00187B4C File Offset: 0x00185D4C
		private void DrawAddActionMapButton(int playerId, InputAction action, AxisRange actionRange, ControlRemappingDemo1.ControllerSelection controller, ControllerMap controllerMap)
		{
			if (GUILayout.Button("Add...", new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = action.id,
					actionRange = actionRange,
					controllerMap = controllerMap
				};
				this.EnqueueAction(new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.Add, context));
				this.startListening = true;
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06006D83 RID: 28035 RVA: 0x0003C0DE File Offset: 0x0003A2DE
		private void ShowDialog()
		{
			this.dialog.Update();
		}

		// Token: 0x06006D84 RID: 28036 RVA: 0x00187BB4 File Offset: 0x00185DB4
		private void DrawModalWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Okay");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06006D85 RID: 28037 RVA: 0x00187C20 File Offset: 0x00185E20
		private void DrawModalWindow_OkayOnly(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Okay");
			GUILayout.EndHorizontal();
		}

		// Token: 0x06006D86 RID: 28038 RVA: 0x00187C7C File Offset: 0x00185E7C
		private void DrawElementAssignmentWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = this.actionQueue.Peek() as ControlRemappingDemo1.ElementAssignmentChange;
			if (elementAssignmentChange == null)
			{
				this.dialog.Cancel();
				return;
			}
			float num;
			if (!this.dialog.busy)
			{
				if (this.startListening && this.inputMapper.status == InputMapper.Status.Idle)
				{
					this.inputMapper.Start(elementAssignmentChange.context);
					this.startListening = false;
				}
				if (this.conflictFoundEventData != null)
				{
					this.dialog.Confirm();
					return;
				}
				num = this.inputMapper.timeRemaining;
				if (num == 0f)
				{
					this.dialog.Cancel();
					return;
				}
			}
			else
			{
				num = this.inputMapper.options.timeout;
			}
			GUILayout.Label("Assignment will be canceled in " + ((int)Mathf.Ceil(num)).ToString() + "...", this.style_wordWrap, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x06006D87 RID: 28039 RVA: 0x00187D88 File Offset: 0x00185F88
		private void DrawElementAssignmentProtectedConflictWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06006D88 RID: 28040 RVA: 0x00187E14 File Offset: 0x00186014
		private void DrawElementAssignmentNormalConflictWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Confirm, "Replace");
			GUILayout.FlexibleSpace();
			this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x00187EB4 File Offset: 0x001860B4
		private void DrawReassignOrRemoveElementAssignmentWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.dialog.DrawConfirmButton("Reassign");
			GUILayout.FlexibleSpace();
			this.dialog.DrawCancelButton("Remove");
			GUILayout.EndHorizontal();
		}

		// Token: 0x06006D8A RID: 28042 RVA: 0x00187F24 File Offset: 0x00186124
		private void DrawFallbackJoystickIdentificationWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			ControlRemappingDemo1.FallbackJoystickIdentification fallbackJoystickIdentification = this.actionQueue.Peek() as ControlRemappingDemo1.FallbackJoystickIdentification;
			if (fallbackJoystickIdentification == null)
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Press any button or axis on \"" + fallbackJoystickIdentification.joystickName + "\" now.", this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Skip", Array.Empty<GUILayoutOption>()))
			{
				this.dialog.Cancel();
				return;
			}
			if (this.dialog.busy)
			{
				return;
			}
			if (!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(fallbackJoystickIdentification.joystickId, 0.8f, false))
			{
				return;
			}
			this.dialog.Confirm();
		}

		// Token: 0x06006D8B RID: 28043 RVA: 0x00187FF4 File Offset: 0x001861F4
		private void DrawCalibrationWindow(string title, string message)
		{
			if (!this.dialog.enabled)
			{
				return;
			}
			ControlRemappingDemo1.Calibration calibration = this.actionQueue.Peek() as ControlRemappingDemo1.Calibration;
			if (calibration == null)
			{
				this.dialog.Cancel();
				return;
			}
			GUILayout.Space(5f);
			GUILayout.Label(message, this.style_wordWrap, Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool enabled = GUI.enabled;
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			this.calibrateScrollPos = GUILayout.BeginScrollView(this.calibrateScrollPos, Array.Empty<GUILayoutOption>());
			if (calibration.recording)
			{
				GUI.enabled = false;
			}
			IList<ControllerElementIdentifier> axisElementIdentifiers = calibration.joystick.AxisElementIdentifiers;
			for (int i = 0; i < axisElementIdentifiers.Count; i++)
			{
				ControllerElementIdentifier controllerElementIdentifier = axisElementIdentifiers[i];
				bool flag = calibration.selectedElementIdentifierId == controllerElementIdentifier.id;
				bool flag2 = GUILayout.Toggle(flag, controllerElementIdentifier.name, "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (flag != flag2)
				{
					calibration.selectedElementIdentifierId = controllerElementIdentifier.id;
				}
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			if (calibration.selectedElementIdentifierId >= 0)
			{
				float axisRawById = calibration.joystick.GetAxisRawById(calibration.selectedElementIdentifierId);
				GUILayout.Label("Raw Value: " + axisRawById.ToString(), Array.Empty<GUILayoutOption>());
				int axisIndexById = calibration.joystick.GetAxisIndexById(calibration.selectedElementIdentifierId);
				AxisCalibration axis = calibration.calibrationMap.GetAxis(axisIndexById);
				GUILayout.Label("Calibrated Value: " + calibration.joystick.GetAxisById(calibration.selectedElementIdentifierId).ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Zero: " + axis.calibratedZero.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Min: " + axis.calibratedMin.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max: " + axis.calibratedMax.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Dead Zone: " + axis.deadZone.ToString(), Array.Empty<GUILayoutOption>());
				GUILayout.Space(15f);
				bool flag3 = GUILayout.Toggle(axis.enabled, "Enabled", "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (axis.enabled != flag3)
				{
					axis.enabled = flag3;
				}
				GUILayout.Space(10f);
				bool flag4 = GUILayout.Toggle(calibration.recording, "Record Min/Max", "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (flag4 != calibration.recording)
				{
					if (flag4)
					{
						axis.calibratedMax = 0f;
						axis.calibratedMin = 0f;
					}
					calibration.recording = flag4;
				}
				if (calibration.recording)
				{
					axis.calibratedMin = Mathf.Min(new float[]
					{
						axis.calibratedMin,
						axisRawById,
						axis.calibratedMin
					});
					axis.calibratedMax = Mathf.Max(new float[]
					{
						axis.calibratedMax,
						axisRawById,
						axis.calibratedMax
					});
					GUI.enabled = false;
				}
				if (GUILayout.Button("Set Zero", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					axis.calibratedZero = axisRawById;
				}
				if (GUILayout.Button("Set Dead Zone", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					axis.deadZone = axisRawById;
				}
				bool flag5 = GUILayout.Toggle(axis.invert, "Invert", "Button", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
				if (axis.invert != flag5)
				{
					axis.invert = flag5;
				}
				GUILayout.Space(10f);
				if (GUILayout.Button("Reset", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					axis.Reset();
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}
			else
			{
				GUILayout.Label("Select an axis to begin.", Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			if (calibration.recording)
			{
				GUI.enabled = false;
			}
			if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
			{
				this.calibrateScrollPos = default(Vector2);
				this.dialog.Confirm();
			}
			if (GUI.enabled != enabled)
			{
				GUI.enabled = enabled;
			}
		}

		// Token: 0x06006D8C RID: 28044 RVA: 0x00188494 File Offset: 0x00186694
		private void DialogResultCallback(int queueActionId, ControlRemappingDemo1.UserResponse response)
		{
			foreach (ControlRemappingDemo1.QueueEntry queueEntry in this.actionQueue)
			{
				if (queueEntry.id == queueActionId)
				{
					if (response != ControlRemappingDemo1.UserResponse.Cancel)
					{
						queueEntry.Confirm(response);
						break;
					}
					queueEntry.Cancel();
					break;
				}
			}
		}

		// Token: 0x06006D8D RID: 28045 RVA: 0x0003C0EB File Offset: 0x0003A2EB
		private Rect GetScreenCenteredRect(float width, float height)
		{
			return new Rect((float)Screen.width * 0.5f - width * 0.5f, (float)((double)Screen.height * 0.5 - (double)(height * 0.5f)), width, height);
		}

		// Token: 0x06006D8E RID: 28046 RVA: 0x0003C123 File Offset: 0x0003A323
		private void EnqueueAction(ControlRemappingDemo1.QueueEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			this.busy = true;
			GUI.enabled = false;
			this.actionQueue.Enqueue(entry);
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x00188500 File Offset: 0x00186700
		private void ProcessQueue()
		{
			if (this.dialog.enabled)
			{
				return;
			}
			if (this.busy || this.actionQueue.Count == 0)
			{
				return;
			}
			while (this.actionQueue.Count > 0)
			{
				ControlRemappingDemo1.QueueEntry queueEntry = this.actionQueue.Peek();
				bool flag = false;
				switch (queueEntry.queueActionType)
				{
				case ControlRemappingDemo1.QueueActionType.JoystickAssignment:
					flag = this.ProcessJoystickAssignmentChange((ControlRemappingDemo1.JoystickAssignmentChange)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.ElementAssignment:
					flag = this.ProcessElementAssignmentChange((ControlRemappingDemo1.ElementAssignmentChange)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification:
					flag = this.ProcessFallbackJoystickIdentification((ControlRemappingDemo1.FallbackJoystickIdentification)queueEntry);
					break;
				case ControlRemappingDemo1.QueueActionType.Calibrate:
					flag = this.ProcessCalibration((ControlRemappingDemo1.Calibration)queueEntry);
					break;
				}
				if (!flag)
				{
					break;
				}
				this.actionQueue.Dequeue();
			}
		}

		// Token: 0x06006D90 RID: 28048 RVA: 0x001885BC File Offset: 0x001867BC
		private bool ProcessJoystickAssignmentChange(ControlRemappingDemo1.JoystickAssignmentChange entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			Player player = ReInput.players.GetPlayer(entry.playerId);
			if (player == null)
			{
				return true;
			}
			if (!entry.assign)
			{
				player.controllers.RemoveController(ControllerType.Joystick, entry.joystickId);
				this.ControllerSelectionChanged();
				return true;
			}
			if (player.controllers.ContainsController(ControllerType.Joystick, entry.joystickId))
			{
				return true;
			}
			if (!ReInput.controllers.IsJoystickAssigned(entry.joystickId) || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				player.controllers.AddController(ControllerType.Joystick, entry.joystickId, true);
				this.ControllerSelectionChanged();
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Joystick Reassignment",
				message = "This joystick is already assigned to another player. Do you want to reassign this joystick to " + player.descriptiveName + "?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x001886D4 File Offset: 0x001868D4
		private bool ProcessElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			switch (entry.changeType)
			{
			case ControlRemappingDemo1.ElementAssignmentChangeType.Add:
			case ControlRemappingDemo1.ElementAssignmentChangeType.Replace:
				return this.ProcessAddOrReplaceElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.Remove:
				return this.ProcessRemoveElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove:
				return this.ProcessRemoveOrReassignElementAssignment(entry);
			case ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck:
				return this.ProcessElementAssignmentConflictCheck(entry);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x0018872C File Offset: 0x0018692C
		private bool ProcessRemoveOrReassignElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Remove;
				this.actionQueue.Enqueue(elementAssignmentChange);
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange2 = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange2.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Replace;
				this.actionQueue.Enqueue(elementAssignmentChange2);
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
			{
				title = "Reassign or Remove",
				message = "Do you want to reassign or remove this assignment?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawReassignOrRemoveElementAssignmentWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06006D93 RID: 28051 RVA: 0x00188800 File Offset: 0x00186A00
		private bool ProcessRemoveElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				entry.context.controllerMap.DeleteElementMap(entry.context.actionElementMapToReplace.id);
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.DeleteAssignmentConfirmation, new ControlRemappingDemo1.WindowProperties
			{
				title = "Remove Assignment",
				message = "Are you sure you want to remove this assignment?",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06006D94 RID: 28052 RVA: 0x001888C0 File Offset: 0x00186AC0
		private bool ProcessAddOrReplaceElementAssignment(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				this.inputMapper.Stop();
				return true;
			}
			if (entry.state != ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				string text;
				if (entry.context.controllerMap.controllerType == ControllerType.Keyboard)
				{
					if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
					{
						text = "Press any key to assign it to this action. You may also use the modifier keys Command, Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
					}
					else
					{
						text = "Press any key to assign it to this action. You may also use the modifier keys Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
					}
					if (Application.isEditor)
					{
						text += "\n\nNOTE: Some modifier key combinations will not work in the Unity Editor, but they will work in a game build.";
					}
				}
				else if (entry.context.controllerMap.controllerType == ControllerType.Mouse)
				{
					text = "Press any mouse button or axis to assign it to this action.\n\nTo assign mouse movement axes, move the mouse quickly in the direction you want mapped to the action. Slow movements will be ignored.";
				}
				else
				{
					text = "Press any button or axis to assign it to this action.";
				}
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assign",
					message = text,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
				return false;
			}
			if (Event.current.type != EventType.Layout)
			{
				return false;
			}
			if (this.conflictFoundEventData != null)
			{
				ControlRemappingDemo1.ElementAssignmentChange elementAssignmentChange = new ControlRemappingDemo1.ElementAssignmentChange(entry);
				elementAssignmentChange.changeType = ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck;
				this.actionQueue.Enqueue(elementAssignmentChange);
			}
			return true;
		}

		// Token: 0x06006D95 RID: 28053 RVA: 0x001889F0 File Offset: 0x00186BF0
		private bool ProcessElementAssignmentConflictCheck(ControlRemappingDemo1.ElementAssignmentChange entry)
		{
			if (entry.context.controllerMap == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				this.inputMapper.Stop();
				return true;
			}
			if (this.conflictFoundEventData == null)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				if (entry.response == ControlRemappingDemo1.UserResponse.Confirm)
				{
					this.conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Replace);
				}
				else
				{
					if (entry.response != ControlRemappingDemo1.UserResponse.Custom1)
					{
						throw new NotImplementedException();
					}
					this.conflictFoundEventData.responseCallback(InputMapper.ConflictResponse.Add);
				}
				return true;
			}
			if (this.conflictFoundEventData.isProtected)
			{
				string message = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use and is protected from reassignment. You cannot remove the protected assignment, but you can still assign the action to this element. If you do so, the element will trigger multiple actions when activated.";
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assignment Conflict",
					message = message,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentProtectedConflictWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			}
			else
			{
				string message2 = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use. You may replace the other conflicting assignments, add this assignment anyway which will leave multiple actions assigned to this element, or cancel this assignment.";
				this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties
				{
					title = "Assignment Conflict",
					message = message2,
					rect = this.GetScreenCenteredRect(250f, 200f),
					windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentNormalConflictWindow)
				}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			}
			return false;
		}

		// Token: 0x06006D96 RID: 28054 RVA: 0x00188B8C File Offset: 0x00186D8C
		private bool ProcessFallbackJoystickIdentification(ControlRemappingDemo1.FallbackJoystickIdentification entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Joystick Identification Required",
				message = "A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:",
				rect = this.GetScreenCenteredRect(250f, 200f),
				windowDrawDelegate = new Action<string, string>(this.DrawFallbackJoystickIdentificationWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback), 1f);
			return false;
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x00188C20 File Offset: 0x00186E20
		private bool ProcessCalibration(ControlRemappingDemo1.Calibration entry)
		{
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
			{
				return true;
			}
			if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
			{
				return true;
			}
			this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties
			{
				title = "Calibrate Controller",
				message = "Select an axis to calibrate on the " + entry.joystick.name + ".",
				rect = this.GetScreenCenteredRect(450f, 480f),
				windowDrawDelegate = new Action<string, string>(this.DrawCalibrationWindow)
			}, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
			return false;
		}

		// Token: 0x06006D98 RID: 28056 RVA: 0x0003C142 File Offset: 0x0003A342
		private void PlayerSelectionChanged()
		{
			this.ClearControllerSelection();
		}

		// Token: 0x06006D99 RID: 28057 RVA: 0x0003C14A File Offset: 0x0003A34A
		private void ControllerSelectionChanged()
		{
			this.ClearMapSelection();
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x0003C152 File Offset: 0x0003A352
		private void ClearControllerSelection()
		{
			this.selectedController.Clear();
			this.ClearMapSelection();
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x0003C165 File Offset: 0x0003A365
		private void ClearMapSelection()
		{
			this.selectedMapCategoryId = -1;
			this.selectedMap = null;
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x0003C175 File Offset: 0x0003A375
		private void ResetAll()
		{
			this.ClearWorkingVars();
			this.initialized = false;
			this.showMenu = false;
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x00188CC4 File Offset: 0x00186EC4
		private void ClearWorkingVars()
		{
			this.selectedPlayer = null;
			this.ClearMapSelection();
			this.selectedController.Clear();
			this.actionScrollPos = default(Vector2);
			this.dialog.FullReset();
			this.actionQueue.Clear();
			this.busy = false;
			this.startListening = false;
			this.conflictFoundEventData = null;
			this.inputMapper.Stop();
		}

		// Token: 0x06006D9E RID: 28062 RVA: 0x00188D2C File Offset: 0x00186F2C
		private void SetGUIStateStart()
		{
			this.guiState = true;
			if (this.busy)
			{
				this.guiState = false;
			}
			this.pageGUIState = (this.guiState && !this.busy && !this.dialog.enabled && !this.dialog.busy);
			if (GUI.enabled != this.guiState)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06006D9F RID: 28063 RVA: 0x0003C18B File Offset: 0x0003A38B
		private void SetGUIStateEnd()
		{
			this.guiState = true;
			if (!GUI.enabled)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x00188D9C File Offset: 0x00186F9C
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
			{
				using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Player player = enumerator.Current;
						if (player.controllers.ContainsController(args.controllerType, args.controllerId))
						{
							ReInput.userDataStore.LoadControllerData(player.id, args.controllerType, args.controllerId);
						}
					}
					goto IL_90;
				}
			}
			ReInput.userDataStore.LoadControllerData(args.controllerType, args.controllerId);
			IL_90:
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x00188E58 File Offset: 0x00187058
		private void JoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (this.selectedController.hasSelection && args.controllerType == this.selectedController.type && args.controllerId == this.selectedController.id)
			{
				this.ClearControllerSelection();
			}
			if (this.showMenu)
			{
				if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
				{
					using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Player player = enumerator.Current;
							if (player.controllers.ContainsController(args.controllerType, args.controllerId))
							{
								ReInput.userDataStore.SaveControllerData(player.id, args.controllerType, args.controllerId);
							}
						}
						return;
					}
				}
				ReInput.userDataStore.SaveControllerData(args.controllerType, args.controllerId);
			}
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x0003C1A6 File Offset: 0x0003A3A6
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (this.showMenu)
			{
				this.ClearWorkingVars();
			}
			if (ReInput.unityJoystickIdentificationRequired)
			{
				this.IdentifyAllJoysticks();
			}
		}

		// Token: 0x06006DA3 RID: 28067 RVA: 0x0003C1C3 File Offset: 0x0003A3C3
		private void OnConflictFound(InputMapper.ConflictFoundEventData data)
		{
			this.conflictFoundEventData = data;
		}

		// Token: 0x06006DA4 RID: 28068 RVA: 0x0003C1CC File Offset: 0x0003A3CC
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.conflictFoundEventData = null;
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x00188F4C File Offset: 0x0018714C
		public void IdentifyAllJoysticks()
		{
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			this.ClearWorkingVars();
			this.Open();
			foreach (Joystick joystick in ReInput.controllers.Joysticks)
			{
				this.actionQueue.Enqueue(new ControlRemappingDemo1.FallbackJoystickIdentification(joystick.id, joystick.name));
			}
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x00002FCA File Offset: 0x000011CA
		protected void CheckRecompile()
		{
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x00002FCA File Offset: 0x000011CA
		private void RecompileWindow(int windowId)
		{
		}

		// Token: 0x04005808 RID: 22536
		private const float defaultModalWidth = 250f;

		// Token: 0x04005809 RID: 22537
		private const float defaultModalHeight = 200f;

		// Token: 0x0400580A RID: 22538
		private const float assignmentTimeout = 5f;

		// Token: 0x0400580B RID: 22539
		private ControlRemappingDemo1.DialogHelper dialog;

		// Token: 0x0400580C RID: 22540
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x0400580D RID: 22541
		private InputMapper.ConflictFoundEventData conflictFoundEventData;

		// Token: 0x0400580E RID: 22542
		private bool guiState;

		// Token: 0x0400580F RID: 22543
		private bool busy;

		// Token: 0x04005810 RID: 22544
		private bool pageGUIState;

		// Token: 0x04005811 RID: 22545
		private Player selectedPlayer;

		// Token: 0x04005812 RID: 22546
		private int selectedMapCategoryId;

		// Token: 0x04005813 RID: 22547
		private ControlRemappingDemo1.ControllerSelection selectedController;

		// Token: 0x04005814 RID: 22548
		private ControllerMap selectedMap;

		// Token: 0x04005815 RID: 22549
		private bool showMenu;

		// Token: 0x04005816 RID: 22550
		private bool startListening;

		// Token: 0x04005817 RID: 22551
		private Vector2 actionScrollPos;

		// Token: 0x04005818 RID: 22552
		private Vector2 calibrateScrollPos;

		// Token: 0x04005819 RID: 22553
		private Queue<ControlRemappingDemo1.QueueEntry> actionQueue;

		// Token: 0x0400581A RID: 22554
		private bool setupFinished;

		// Token: 0x0400581B RID: 22555
		[NonSerialized]
		private bool initialized;

		// Token: 0x0400581C RID: 22556
		private bool isCompiling;

		// Token: 0x0400581D RID: 22557
		private GUIStyle style_wordWrap;

		// Token: 0x0400581E RID: 22558
		private GUIStyle style_centeredBox;

		// Token: 0x02000ECA RID: 3786
		private class ControllerSelection
		{
			// Token: 0x06006DA9 RID: 28073 RVA: 0x0003C1E8 File Offset: 0x0003A3E8
			public ControllerSelection()
			{
				this.Clear();
			}

			// Token: 0x170023EC RID: 9196
			// (get) Token: 0x06006DAA RID: 28074 RVA: 0x0003C1F6 File Offset: 0x0003A3F6
			// (set) Token: 0x06006DAB RID: 28075 RVA: 0x0003C1FE File Offset: 0x0003A3FE
			public int id
			{
				get
				{
					return this._id;
				}
				set
				{
					this._idPrev = this._id;
					this._id = value;
				}
			}

			// Token: 0x170023ED RID: 9197
			// (get) Token: 0x06006DAC RID: 28076 RVA: 0x0003C213 File Offset: 0x0003A413
			// (set) Token: 0x06006DAD RID: 28077 RVA: 0x0003C21B File Offset: 0x0003A41B
			public ControllerType type
			{
				get
				{
					return this._type;
				}
				set
				{
					this._typePrev = this._type;
					this._type = value;
				}
			}

			// Token: 0x170023EE RID: 9198
			// (get) Token: 0x06006DAE RID: 28078 RVA: 0x0003C230 File Offset: 0x0003A430
			public int idPrev
			{
				get
				{
					return this._idPrev;
				}
			}

			// Token: 0x170023EF RID: 9199
			// (get) Token: 0x06006DAF RID: 28079 RVA: 0x0003C238 File Offset: 0x0003A438
			public ControllerType typePrev
			{
				get
				{
					return this._typePrev;
				}
			}

			// Token: 0x170023F0 RID: 9200
			// (get) Token: 0x06006DB0 RID: 28080 RVA: 0x0003C240 File Offset: 0x0003A440
			public bool hasSelection
			{
				get
				{
					return this._id >= 0;
				}
			}

			// Token: 0x06006DB1 RID: 28081 RVA: 0x0003C24E File Offset: 0x0003A44E
			public void Set(int id, ControllerType type)
			{
				this.id = id;
				this.type = type;
			}

			// Token: 0x06006DB2 RID: 28082 RVA: 0x0003C25E File Offset: 0x0003A45E
			public void Clear()
			{
				this._id = -1;
				this._idPrev = -1;
				this._type = ControllerType.Joystick;
				this._typePrev = ControllerType.Joystick;
			}

			// Token: 0x0400581F RID: 22559
			private int _id;

			// Token: 0x04005820 RID: 22560
			private int _idPrev;

			// Token: 0x04005821 RID: 22561
			private ControllerType _type;

			// Token: 0x04005822 RID: 22562
			private ControllerType _typePrev;
		}

		// Token: 0x02000ECB RID: 3787
		private class DialogHelper
		{
			// Token: 0x170023F1 RID: 9201
			// (get) Token: 0x06006DB3 RID: 28083 RVA: 0x0003C27C File Offset: 0x0003A47C
			private float busyTimer
			{
				get
				{
					if (!this._busyTimerRunning)
					{
						return 0f;
					}
					return this._busyTime - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x170023F2 RID: 9202
			// (get) Token: 0x06006DB4 RID: 28084 RVA: 0x0003C298 File Offset: 0x0003A498
			// (set) Token: 0x06006DB5 RID: 28085 RVA: 0x0003C2A0 File Offset: 0x0003A4A0
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					if (!value)
					{
						this._enabled = value;
						this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
						this.StateChanged(0.1f);
						return;
					}
					if (this._type == ControlRemappingDemo1.DialogHelper.DialogType.None)
					{
						return;
					}
					this.StateChanged(0.25f);
				}
			}

			// Token: 0x170023F3 RID: 9203
			// (get) Token: 0x06006DB6 RID: 28086 RVA: 0x0003C2D3 File Offset: 0x0003A4D3
			// (set) Token: 0x06006DB7 RID: 28087 RVA: 0x0003C2E5 File Offset: 0x0003A4E5
			public ControlRemappingDemo1.DialogHelper.DialogType type
			{
				get
				{
					if (!this._enabled)
					{
						return ControlRemappingDemo1.DialogHelper.DialogType.None;
					}
					return this._type;
				}
				set
				{
					if (value == ControlRemappingDemo1.DialogHelper.DialogType.None)
					{
						this._enabled = false;
						this.StateChanged(0.1f);
					}
					else
					{
						this._enabled = true;
						this.StateChanged(0.25f);
					}
					this._type = value;
				}
			}

			// Token: 0x170023F4 RID: 9204
			// (get) Token: 0x06006DB8 RID: 28088 RVA: 0x0003C317 File Offset: 0x0003A517
			public bool busy
			{
				get
				{
					return this._busyTimerRunning;
				}
			}

			// Token: 0x06006DB9 RID: 28089 RVA: 0x0003C31F File Offset: 0x0003A51F
			public DialogHelper()
			{
				this.drawWindowDelegate = new Action<int>(this.DrawWindow);
				this.drawWindowFunction = new GUI.WindowFunction(this.drawWindowDelegate.Invoke);
			}

			// Token: 0x06006DBA RID: 28090 RVA: 0x0003C350 File Offset: 0x0003A550
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback)
			{
				this.StartModal(queueActionId, type, windowProperties, resultCallback, -1f);
			}

			// Token: 0x06006DBB RID: 28091 RVA: 0x0003C362 File Offset: 0x0003A562
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback, float openBusyDelay)
			{
				this.currentActionId = queueActionId;
				this.windowProperties = windowProperties;
				this.type = type;
				this.resultCallback = resultCallback;
				if (openBusyDelay >= 0f)
				{
					this.StateChanged(openBusyDelay);
				}
			}

			// Token: 0x06006DBC RID: 28092 RVA: 0x0003C392 File Offset: 0x0003A592
			public void Update()
			{
				this.Draw();
				this.UpdateTimers();
			}

			// Token: 0x06006DBD RID: 28093 RVA: 0x00188FCC File Offset: 0x001871CC
			public void Draw()
			{
				if (!this._enabled)
				{
					return;
				}
				bool enabled = GUI.enabled;
				GUI.enabled = true;
				GUILayout.Window(this.windowProperties.windowId, this.windowProperties.rect, this.drawWindowFunction, this.windowProperties.title, Array.Empty<GUILayoutOption>());
				GUI.FocusWindow(this.windowProperties.windowId);
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06006DBE RID: 28094 RVA: 0x0003C3A0 File Offset: 0x0003A5A0
			public void DrawConfirmButton()
			{
				this.DrawConfirmButton("Confirm");
			}

			// Token: 0x06006DBF RID: 28095 RVA: 0x00189040 File Offset: 0x00187240
			public void DrawConfirmButton(string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06006DC0 RID: 28096 RVA: 0x0003C3AD File Offset: 0x0003A5AD
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response)
			{
				this.DrawConfirmButton(response, "Confirm");
			}

			// Token: 0x06006DC1 RID: 28097 RVA: 0x00189084 File Offset: 0x00187284
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response, string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Confirm(response);
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06006DC2 RID: 28098 RVA: 0x0003C3BB File Offset: 0x0003A5BB
			public void DrawCancelButton()
			{
				this.DrawCancelButton("Cancel");
			}

			// Token: 0x06006DC3 RID: 28099 RVA: 0x001890C8 File Offset: 0x001872C8
			public void DrawCancelButton(string title)
			{
				bool enabled = GUI.enabled;
				if (this.busy)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button(title, Array.Empty<GUILayoutOption>()))
				{
					this.Cancel();
				}
				if (GUI.enabled != enabled)
				{
					GUI.enabled = enabled;
				}
			}

			// Token: 0x06006DC4 RID: 28100 RVA: 0x0003C3C8 File Offset: 0x0003A5C8
			public void Confirm()
			{
				this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
			}

			// Token: 0x06006DC5 RID: 28101 RVA: 0x0003C3D1 File Offset: 0x0003A5D1
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.resultCallback(this.currentActionId, response);
				this.Close();
			}

			// Token: 0x06006DC6 RID: 28102 RVA: 0x0003C3EB File Offset: 0x0003A5EB
			public void Cancel()
			{
				this.resultCallback(this.currentActionId, ControlRemappingDemo1.UserResponse.Cancel);
				this.Close();
			}

			// Token: 0x06006DC7 RID: 28103 RVA: 0x0003C405 File Offset: 0x0003A605
			private void DrawWindow(int windowId)
			{
				this.windowProperties.windowDrawDelegate(this.windowProperties.title, this.windowProperties.message);
			}

			// Token: 0x06006DC8 RID: 28104 RVA: 0x0003C42D File Offset: 0x0003A62D
			private void UpdateTimers()
			{
				if (this._busyTimerRunning && this.busyTimer <= 0f)
				{
					this._busyTimerRunning = false;
				}
			}

			// Token: 0x06006DC9 RID: 28105 RVA: 0x0003C44B File Offset: 0x0003A64B
			private void StartBusyTimer(float time)
			{
				this._busyTime = time + Time.realtimeSinceStartup;
				this._busyTimerRunning = true;
			}

			// Token: 0x06006DCA RID: 28106 RVA: 0x0003C461 File Offset: 0x0003A661
			private void Close()
			{
				this.Reset();
				this.StateChanged(0.1f);
			}

			// Token: 0x06006DCB RID: 28107 RVA: 0x0003C474 File Offset: 0x0003A674
			private void StateChanged(float delay)
			{
				this.StartBusyTimer(delay);
			}

			// Token: 0x06006DCC RID: 28108 RVA: 0x0003C47D File Offset: 0x0003A67D
			private void Reset()
			{
				this._enabled = false;
				this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
				this.currentActionId = -1;
				this.resultCallback = null;
			}

			// Token: 0x06006DCD RID: 28109 RVA: 0x0003C49B File Offset: 0x0003A69B
			private void ResetTimers()
			{
				this._busyTimerRunning = false;
			}

			// Token: 0x06006DCE RID: 28110 RVA: 0x0003C4A4 File Offset: 0x0003A6A4
			public void FullReset()
			{
				this.Reset();
				this.ResetTimers();
			}

			// Token: 0x04005823 RID: 22563
			private const float openBusyDelay = 0.25f;

			// Token: 0x04005824 RID: 22564
			private const float closeBusyDelay = 0.1f;

			// Token: 0x04005825 RID: 22565
			private ControlRemappingDemo1.DialogHelper.DialogType _type;

			// Token: 0x04005826 RID: 22566
			private bool _enabled;

			// Token: 0x04005827 RID: 22567
			private float _busyTime;

			// Token: 0x04005828 RID: 22568
			private bool _busyTimerRunning;

			// Token: 0x04005829 RID: 22569
			private Action<int> drawWindowDelegate;

			// Token: 0x0400582A RID: 22570
			private GUI.WindowFunction drawWindowFunction;

			// Token: 0x0400582B RID: 22571
			private ControlRemappingDemo1.WindowProperties windowProperties;

			// Token: 0x0400582C RID: 22572
			private int currentActionId;

			// Token: 0x0400582D RID: 22573
			private Action<int, ControlRemappingDemo1.UserResponse> resultCallback;

			// Token: 0x02000ECC RID: 3788
			public enum DialogType
			{
				// Token: 0x0400582F RID: 22575
				None,
				// Token: 0x04005830 RID: 22576
				JoystickConflict,
				// Token: 0x04005831 RID: 22577
				ElementConflict,
				// Token: 0x04005832 RID: 22578
				KeyConflict,
				// Token: 0x04005833 RID: 22579
				DeleteAssignmentConfirmation = 10,
				// Token: 0x04005834 RID: 22580
				AssignElement
			}
		}

		// Token: 0x02000ECD RID: 3789
		private abstract class QueueEntry
		{
			// Token: 0x170023F5 RID: 9205
			// (get) Token: 0x06006DCF RID: 28111 RVA: 0x0003C4B2 File Offset: 0x0003A6B2
			// (set) Token: 0x06006DD0 RID: 28112 RVA: 0x0003C4BA File Offset: 0x0003A6BA
			public int id { get; protected set; }

			// Token: 0x170023F6 RID: 9206
			// (get) Token: 0x06006DD1 RID: 28113 RVA: 0x0003C4C3 File Offset: 0x0003A6C3
			// (set) Token: 0x06006DD2 RID: 28114 RVA: 0x0003C4CB File Offset: 0x0003A6CB
			public ControlRemappingDemo1.QueueActionType queueActionType { get; protected set; }

			// Token: 0x170023F7 RID: 9207
			// (get) Token: 0x06006DD3 RID: 28115 RVA: 0x0003C4D4 File Offset: 0x0003A6D4
			// (set) Token: 0x06006DD4 RID: 28116 RVA: 0x0003C4DC File Offset: 0x0003A6DC
			public ControlRemappingDemo1.QueueEntry.State state { get; protected set; }

			// Token: 0x170023F8 RID: 9208
			// (get) Token: 0x06006DD5 RID: 28117 RVA: 0x0003C4E5 File Offset: 0x0003A6E5
			// (set) Token: 0x06006DD6 RID: 28118 RVA: 0x0003C4ED File Offset: 0x0003A6ED
			public ControlRemappingDemo1.UserResponse response { get; protected set; }

			// Token: 0x170023F9 RID: 9209
			// (get) Token: 0x06006DD7 RID: 28119 RVA: 0x0003C4F6 File Offset: 0x0003A6F6
			protected static int nextId
			{
				get
				{
					int result = ControlRemappingDemo1.QueueEntry.uidCounter;
					ControlRemappingDemo1.QueueEntry.uidCounter++;
					return result;
				}
			}

			// Token: 0x06006DD8 RID: 28120 RVA: 0x0003C509 File Offset: 0x0003A709
			public QueueEntry(ControlRemappingDemo1.QueueActionType queueActionType)
			{
				this.id = ControlRemappingDemo1.QueueEntry.nextId;
				this.queueActionType = queueActionType;
			}

			// Token: 0x06006DD9 RID: 28121 RVA: 0x0003C523 File Offset: 0x0003A723
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Confirmed;
				this.response = response;
			}

			// Token: 0x06006DDA RID: 28122 RVA: 0x0003C533 File Offset: 0x0003A733
			public void Cancel()
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Canceled;
			}

			// Token: 0x04005839 RID: 22585
			private static int uidCounter;

			// Token: 0x02000ECE RID: 3790
			public enum State
			{
				// Token: 0x0400583B RID: 22587
				Waiting,
				// Token: 0x0400583C RID: 22588
				Confirmed,
				// Token: 0x0400583D RID: 22589
				Canceled
			}
		}

		// Token: 0x02000ECF RID: 3791
		private class JoystickAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170023FA RID: 9210
			// (get) Token: 0x06006DDB RID: 28123 RVA: 0x0003C53C File Offset: 0x0003A73C
			// (set) Token: 0x06006DDC RID: 28124 RVA: 0x0003C544 File Offset: 0x0003A744
			public int playerId { get; private set; }

			// Token: 0x170023FB RID: 9211
			// (get) Token: 0x06006DDD RID: 28125 RVA: 0x0003C54D File Offset: 0x0003A74D
			// (set) Token: 0x06006DDE RID: 28126 RVA: 0x0003C555 File Offset: 0x0003A755
			public int joystickId { get; private set; }

			// Token: 0x170023FC RID: 9212
			// (get) Token: 0x06006DDF RID: 28127 RVA: 0x0003C55E File Offset: 0x0003A75E
			// (set) Token: 0x06006DE0 RID: 28128 RVA: 0x0003C566 File Offset: 0x0003A766
			public bool assign { get; private set; }

			// Token: 0x06006DE1 RID: 28129 RVA: 0x0003C56F File Offset: 0x0003A76F
			public JoystickAssignmentChange(int newPlayerId, int joystickId, bool assign) : base(ControlRemappingDemo1.QueueActionType.JoystickAssignment)
			{
				this.playerId = newPlayerId;
				this.joystickId = joystickId;
				this.assign = assign;
			}
		}

		// Token: 0x02000ED0 RID: 3792
		private class ElementAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170023FD RID: 9213
			// (get) Token: 0x06006DE2 RID: 28130 RVA: 0x0003C58D File Offset: 0x0003A78D
			// (set) Token: 0x06006DE3 RID: 28131 RVA: 0x0003C595 File Offset: 0x0003A795
			public ControlRemappingDemo1.ElementAssignmentChangeType changeType { get; set; }

			// Token: 0x170023FE RID: 9214
			// (get) Token: 0x06006DE4 RID: 28132 RVA: 0x0003C59E File Offset: 0x0003A79E
			// (set) Token: 0x06006DE5 RID: 28133 RVA: 0x0003C5A6 File Offset: 0x0003A7A6
			public InputMapper.Context context { get; private set; }

			// Token: 0x06006DE6 RID: 28134 RVA: 0x0003C5AF File Offset: 0x0003A7AF
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType changeType, InputMapper.Context context) : base(ControlRemappingDemo1.QueueActionType.ElementAssignment)
			{
				this.changeType = changeType;
				this.context = context;
			}

			// Token: 0x06006DE7 RID: 28135 RVA: 0x0003C5C6 File Offset: 0x0003A7C6
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange other) : this(other.changeType, other.context.Clone())
			{
			}
		}

		// Token: 0x02000ED1 RID: 3793
		private class FallbackJoystickIdentification : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x170023FF RID: 9215
			// (get) Token: 0x06006DE8 RID: 28136 RVA: 0x0003C5DF File Offset: 0x0003A7DF
			// (set) Token: 0x06006DE9 RID: 28137 RVA: 0x0003C5E7 File Offset: 0x0003A7E7
			public int joystickId { get; private set; }

			// Token: 0x17002400 RID: 9216
			// (get) Token: 0x06006DEA RID: 28138 RVA: 0x0003C5F0 File Offset: 0x0003A7F0
			// (set) Token: 0x06006DEB RID: 28139 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
			public string joystickName { get; private set; }

			// Token: 0x06006DEC RID: 28140 RVA: 0x0003C601 File Offset: 0x0003A801
			public FallbackJoystickIdentification(int joystickId, string joystickName) : base(ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification)
			{
				this.joystickId = joystickId;
				this.joystickName = joystickName;
			}
		}

		// Token: 0x02000ED2 RID: 3794
		private class Calibration : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x17002401 RID: 9217
			// (get) Token: 0x06006DED RID: 28141 RVA: 0x0003C618 File Offset: 0x0003A818
			// (set) Token: 0x06006DEE RID: 28142 RVA: 0x0003C620 File Offset: 0x0003A820
			public Player player { get; private set; }

			// Token: 0x17002402 RID: 9218
			// (get) Token: 0x06006DEF RID: 28143 RVA: 0x0003C629 File Offset: 0x0003A829
			// (set) Token: 0x06006DF0 RID: 28144 RVA: 0x0003C631 File Offset: 0x0003A831
			public ControllerType controllerType { get; private set; }

			// Token: 0x17002403 RID: 9219
			// (get) Token: 0x06006DF1 RID: 28145 RVA: 0x0003C63A File Offset: 0x0003A83A
			// (set) Token: 0x06006DF2 RID: 28146 RVA: 0x0003C642 File Offset: 0x0003A842
			public Joystick joystick { get; private set; }

			// Token: 0x17002404 RID: 9220
			// (get) Token: 0x06006DF3 RID: 28147 RVA: 0x0003C64B File Offset: 0x0003A84B
			// (set) Token: 0x06006DF4 RID: 28148 RVA: 0x0003C653 File Offset: 0x0003A853
			public CalibrationMap calibrationMap { get; private set; }

			// Token: 0x06006DF5 RID: 28149 RVA: 0x0003C65C File Offset: 0x0003A85C
			public Calibration(Player player, Joystick joystick, CalibrationMap calibrationMap) : base(ControlRemappingDemo1.QueueActionType.Calibrate)
			{
				this.player = player;
				this.joystick = joystick;
				this.calibrationMap = calibrationMap;
				this.selectedElementIdentifierId = -1;
			}

			// Token: 0x04005849 RID: 22601
			public int selectedElementIdentifierId;

			// Token: 0x0400584A RID: 22602
			public bool recording;
		}

		// Token: 0x02000ED3 RID: 3795
		private struct WindowProperties
		{
			// Token: 0x0400584B RID: 22603
			public int windowId;

			// Token: 0x0400584C RID: 22604
			public Rect rect;

			// Token: 0x0400584D RID: 22605
			public Action<string, string> windowDrawDelegate;

			// Token: 0x0400584E RID: 22606
			public string title;

			// Token: 0x0400584F RID: 22607
			public string message;
		}

		// Token: 0x02000ED4 RID: 3796
		private enum QueueActionType
		{
			// Token: 0x04005851 RID: 22609
			None,
			// Token: 0x04005852 RID: 22610
			JoystickAssignment,
			// Token: 0x04005853 RID: 22611
			ElementAssignment,
			// Token: 0x04005854 RID: 22612
			FallbackJoystickIdentification,
			// Token: 0x04005855 RID: 22613
			Calibrate
		}

		// Token: 0x02000ED5 RID: 3797
		private enum ElementAssignmentChangeType
		{
			// Token: 0x04005857 RID: 22615
			Add,
			// Token: 0x04005858 RID: 22616
			Replace,
			// Token: 0x04005859 RID: 22617
			Remove,
			// Token: 0x0400585A RID: 22618
			ReassignOrRemove,
			// Token: 0x0400585B RID: 22619
			ConflictCheck
		}

		// Token: 0x02000ED6 RID: 3798
		public enum UserResponse
		{
			// Token: 0x0400585D RID: 22621
			Confirm,
			// Token: 0x0400585E RID: 22622
			Cancel,
			// Token: 0x0400585F RID: 22623
			Custom1,
			// Token: 0x04005860 RID: 22624
			Custom2
		}
	}
}
