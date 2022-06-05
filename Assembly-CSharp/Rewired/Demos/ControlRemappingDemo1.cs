using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x0200093A RID: 2362
	[AddComponentMenu("")]
	public class ControlRemappingDemo1 : MonoBehaviour
	{
		// Token: 0x06004FE2 RID: 20450 RVA: 0x00117E6C File Offset: 0x0011606C
		private void Awake()
		{
			this.inputMapper.options.timeout = 5f;
			this.inputMapper.options.ignoreMouseXAxis = true;
			this.inputMapper.options.ignoreMouseYAxis = true;
			this.Initialize();
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00117EAB File Offset: 0x001160AB
		private void OnEnable()
		{
			this.Subscribe();
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x00117EB3 File Offset: 0x001160B3
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x00117EBC File Offset: 0x001160BC
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

		// Token: 0x06004FE6 RID: 20454 RVA: 0x00117F44 File Offset: 0x00116144
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

		// Token: 0x06004FE7 RID: 20455 RVA: 0x00117FA3 File Offset: 0x001161A3
		private void Subscribe()
		{
			this.Unsubscribe();
			this.inputMapper.ConflictFoundEvent += this.OnConflictFound;
			this.inputMapper.StoppedEvent += this.OnStopped;
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x00117FD9 File Offset: 0x001161D9
		private void Unsubscribe()
		{
			this.inputMapper.RemoveAllEventListeners();
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00117FE8 File Offset: 0x001161E8
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

		// Token: 0x06004FEA RID: 20458 RVA: 0x00118040 File Offset: 0x00116240
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

		// Token: 0x06004FEB RID: 20459 RVA: 0x0011809D File Offset: 0x0011629D
		private void Close()
		{
			this.ClearWorkingVars();
			this.showMenu = false;
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x001180AC File Offset: 0x001162AC
		private void Open()
		{
			this.showMenu = true;
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001180B8 File Offset: 0x001162B8
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

		// Token: 0x06004FEE RID: 20462 RVA: 0x00118150 File Offset: 0x00116350
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

		// Token: 0x06004FEF RID: 20463 RVA: 0x00118214 File Offset: 0x00116414
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

		// Token: 0x06004FF0 RID: 20464 RVA: 0x00118328 File Offset: 0x00116528
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

		// Token: 0x06004FF1 RID: 20465 RVA: 0x00118444 File Offset: 0x00116644
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

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001185A0 File Offset: 0x001167A0
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

		// Token: 0x06004FF3 RID: 20467 RVA: 0x00118774 File Offset: 0x00116974
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

		// Token: 0x06004FF4 RID: 20468 RVA: 0x00118870 File Offset: 0x00116A70
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

		// Token: 0x06004FF5 RID: 20469 RVA: 0x00118A44 File Offset: 0x00116C44
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

		// Token: 0x06004FF6 RID: 20470 RVA: 0x00118F38 File Offset: 0x00117138
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

		// Token: 0x06004FF7 RID: 20471 RVA: 0x00118FB8 File Offset: 0x001171B8
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

		// Token: 0x06004FF8 RID: 20472 RVA: 0x00119000 File Offset: 0x00117200
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

		// Token: 0x06004FF9 RID: 20473 RVA: 0x00119067 File Offset: 0x00117267
		private void ShowDialog()
		{
			this.dialog.Update();
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x00119074 File Offset: 0x00117274
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

		// Token: 0x06004FFB RID: 20475 RVA: 0x001190E0 File Offset: 0x001172E0
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

		// Token: 0x06004FFC RID: 20476 RVA: 0x0011913C File Offset: 0x0011733C
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

		// Token: 0x06004FFD RID: 20477 RVA: 0x00119248 File Offset: 0x00117448
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

		// Token: 0x06004FFE RID: 20478 RVA: 0x001192D4 File Offset: 0x001174D4
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

		// Token: 0x06004FFF RID: 20479 RVA: 0x00119374 File Offset: 0x00117574
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

		// Token: 0x06005000 RID: 20480 RVA: 0x001193E4 File Offset: 0x001175E4
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

		// Token: 0x06005001 RID: 20481 RVA: 0x001194B4 File Offset: 0x001176B4
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

		// Token: 0x06005002 RID: 20482 RVA: 0x00119954 File Offset: 0x00117B54
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

		// Token: 0x06005003 RID: 20483 RVA: 0x001199C0 File Offset: 0x00117BC0
		private Rect GetScreenCenteredRect(float width, float height)
		{
			return new Rect((float)Screen.width * 0.5f - width * 0.5f, (float)((double)Screen.height * 0.5 - (double)(height * 0.5f)), width, height);
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x001199F8 File Offset: 0x00117BF8
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

		// Token: 0x06005005 RID: 20485 RVA: 0x00119A18 File Offset: 0x00117C18
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

		// Token: 0x06005006 RID: 20486 RVA: 0x00119AD4 File Offset: 0x00117CD4
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

		// Token: 0x06005007 RID: 20487 RVA: 0x00119BEC File Offset: 0x00117DEC
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

		// Token: 0x06005008 RID: 20488 RVA: 0x00119C44 File Offset: 0x00117E44
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

		// Token: 0x06005009 RID: 20489 RVA: 0x00119D18 File Offset: 0x00117F18
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

		// Token: 0x0600500A RID: 20490 RVA: 0x00119DD8 File Offset: 0x00117FD8
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

		// Token: 0x0600500B RID: 20491 RVA: 0x00119F08 File Offset: 0x00118108
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

		// Token: 0x0600500C RID: 20492 RVA: 0x0011A0A4 File Offset: 0x001182A4
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

		// Token: 0x0600500D RID: 20493 RVA: 0x0011A138 File Offset: 0x00118338
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

		// Token: 0x0600500E RID: 20494 RVA: 0x0011A1DA File Offset: 0x001183DA
		private void PlayerSelectionChanged()
		{
			this.ClearControllerSelection();
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x0011A1E2 File Offset: 0x001183E2
		private void ControllerSelectionChanged()
		{
			this.ClearMapSelection();
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x0011A1EA File Offset: 0x001183EA
		private void ClearControllerSelection()
		{
			this.selectedController.Clear();
			this.ClearMapSelection();
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x0011A1FD File Offset: 0x001183FD
		private void ClearMapSelection()
		{
			this.selectedMapCategoryId = -1;
			this.selectedMap = null;
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x0011A20D File Offset: 0x0011840D
		private void ResetAll()
		{
			this.ClearWorkingVars();
			this.initialized = false;
			this.showMenu = false;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x0011A224 File Offset: 0x00118424
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

		// Token: 0x06005014 RID: 20500 RVA: 0x0011A28C File Offset: 0x0011848C
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

		// Token: 0x06005015 RID: 20501 RVA: 0x0011A2FB File Offset: 0x001184FB
		private void SetGUIStateEnd()
		{
			this.guiState = true;
			if (!GUI.enabled)
			{
				GUI.enabled = this.guiState;
			}
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x0011A318 File Offset: 0x00118518
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

		// Token: 0x06005017 RID: 20503 RVA: 0x0011A3D4 File Offset: 0x001185D4
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

		// Token: 0x06005018 RID: 20504 RVA: 0x0011A4C8 File Offset: 0x001186C8
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

		// Token: 0x06005019 RID: 20505 RVA: 0x0011A4E5 File Offset: 0x001186E5
		private void OnConflictFound(InputMapper.ConflictFoundEventData data)
		{
			this.conflictFoundEventData = data;
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x0011A4EE File Offset: 0x001186EE
		private void OnStopped(InputMapper.StoppedEventData data)
		{
			this.conflictFoundEventData = null;
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x0011A4F8 File Offset: 0x001186F8
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

		// Token: 0x0600501C RID: 20508 RVA: 0x0011A578 File Offset: 0x00118778
		protected void CheckRecompile()
		{
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x0011A57A File Offset: 0x0011877A
		private void RecompileWindow(int windowId)
		{
		}

		// Token: 0x0400426A RID: 17002
		private const float defaultModalWidth = 250f;

		// Token: 0x0400426B RID: 17003
		private const float defaultModalHeight = 200f;

		// Token: 0x0400426C RID: 17004
		private const float assignmentTimeout = 5f;

		// Token: 0x0400426D RID: 17005
		private ControlRemappingDemo1.DialogHelper dialog;

		// Token: 0x0400426E RID: 17006
		private InputMapper inputMapper = new InputMapper();

		// Token: 0x0400426F RID: 17007
		private InputMapper.ConflictFoundEventData conflictFoundEventData;

		// Token: 0x04004270 RID: 17008
		private bool guiState;

		// Token: 0x04004271 RID: 17009
		private bool busy;

		// Token: 0x04004272 RID: 17010
		private bool pageGUIState;

		// Token: 0x04004273 RID: 17011
		private Player selectedPlayer;

		// Token: 0x04004274 RID: 17012
		private int selectedMapCategoryId;

		// Token: 0x04004275 RID: 17013
		private ControlRemappingDemo1.ControllerSelection selectedController;

		// Token: 0x04004276 RID: 17014
		private ControllerMap selectedMap;

		// Token: 0x04004277 RID: 17015
		private bool showMenu;

		// Token: 0x04004278 RID: 17016
		private bool startListening;

		// Token: 0x04004279 RID: 17017
		private Vector2 actionScrollPos;

		// Token: 0x0400427A RID: 17018
		private Vector2 calibrateScrollPos;

		// Token: 0x0400427B RID: 17019
		private Queue<ControlRemappingDemo1.QueueEntry> actionQueue;

		// Token: 0x0400427C RID: 17020
		private bool setupFinished;

		// Token: 0x0400427D RID: 17021
		[NonSerialized]
		private bool initialized;

		// Token: 0x0400427E RID: 17022
		private bool isCompiling;

		// Token: 0x0400427F RID: 17023
		private GUIStyle style_wordWrap;

		// Token: 0x04004280 RID: 17024
		private GUIStyle style_centeredBox;

		// Token: 0x02000EFE RID: 3838
		private class ControllerSelection
		{
			// Token: 0x06006FC6 RID: 28614 RVA: 0x0019E6D4 File Offset: 0x0019C8D4
			public ControllerSelection()
			{
				this.Clear();
			}

			// Token: 0x1700244F RID: 9295
			// (get) Token: 0x06006FC7 RID: 28615 RVA: 0x0019E6E2 File Offset: 0x0019C8E2
			// (set) Token: 0x06006FC8 RID: 28616 RVA: 0x0019E6EA File Offset: 0x0019C8EA
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

			// Token: 0x17002450 RID: 9296
			// (get) Token: 0x06006FC9 RID: 28617 RVA: 0x0019E6FF File Offset: 0x0019C8FF
			// (set) Token: 0x06006FCA RID: 28618 RVA: 0x0019E707 File Offset: 0x0019C907
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

			// Token: 0x17002451 RID: 9297
			// (get) Token: 0x06006FCB RID: 28619 RVA: 0x0019E71C File Offset: 0x0019C91C
			public int idPrev
			{
				get
				{
					return this._idPrev;
				}
			}

			// Token: 0x17002452 RID: 9298
			// (get) Token: 0x06006FCC RID: 28620 RVA: 0x0019E724 File Offset: 0x0019C924
			public ControllerType typePrev
			{
				get
				{
					return this._typePrev;
				}
			}

			// Token: 0x17002453 RID: 9299
			// (get) Token: 0x06006FCD RID: 28621 RVA: 0x0019E72C File Offset: 0x0019C92C
			public bool hasSelection
			{
				get
				{
					return this._id >= 0;
				}
			}

			// Token: 0x06006FCE RID: 28622 RVA: 0x0019E73A File Offset: 0x0019C93A
			public void Set(int id, ControllerType type)
			{
				this.id = id;
				this.type = type;
			}

			// Token: 0x06006FCF RID: 28623 RVA: 0x0019E74A File Offset: 0x0019C94A
			public void Clear()
			{
				this._id = -1;
				this._idPrev = -1;
				this._type = ControllerType.Joystick;
				this._typePrev = ControllerType.Joystick;
			}

			// Token: 0x04005A0B RID: 23051
			private int _id;

			// Token: 0x04005A0C RID: 23052
			private int _idPrev;

			// Token: 0x04005A0D RID: 23053
			private ControllerType _type;

			// Token: 0x04005A0E RID: 23054
			private ControllerType _typePrev;
		}

		// Token: 0x02000EFF RID: 3839
		private class DialogHelper
		{
			// Token: 0x17002454 RID: 9300
			// (get) Token: 0x06006FD0 RID: 28624 RVA: 0x0019E768 File Offset: 0x0019C968
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

			// Token: 0x17002455 RID: 9301
			// (get) Token: 0x06006FD1 RID: 28625 RVA: 0x0019E784 File Offset: 0x0019C984
			// (set) Token: 0x06006FD2 RID: 28626 RVA: 0x0019E78C File Offset: 0x0019C98C
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

			// Token: 0x17002456 RID: 9302
			// (get) Token: 0x06006FD3 RID: 28627 RVA: 0x0019E7BF File Offset: 0x0019C9BF
			// (set) Token: 0x06006FD4 RID: 28628 RVA: 0x0019E7D1 File Offset: 0x0019C9D1
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

			// Token: 0x17002457 RID: 9303
			// (get) Token: 0x06006FD5 RID: 28629 RVA: 0x0019E803 File Offset: 0x0019CA03
			public bool busy
			{
				get
				{
					return this._busyTimerRunning;
				}
			}

			// Token: 0x06006FD6 RID: 28630 RVA: 0x0019E80B File Offset: 0x0019CA0B
			public DialogHelper()
			{
				this.drawWindowDelegate = new Action<int>(this.DrawWindow);
				this.drawWindowFunction = new GUI.WindowFunction(this.drawWindowDelegate.Invoke);
			}

			// Token: 0x06006FD7 RID: 28631 RVA: 0x0019E83C File Offset: 0x0019CA3C
			public void StartModal(int queueActionId, ControlRemappingDemo1.DialogHelper.DialogType type, ControlRemappingDemo1.WindowProperties windowProperties, Action<int, ControlRemappingDemo1.UserResponse> resultCallback)
			{
				this.StartModal(queueActionId, type, windowProperties, resultCallback, -1f);
			}

			// Token: 0x06006FD8 RID: 28632 RVA: 0x0019E84E File Offset: 0x0019CA4E
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

			// Token: 0x06006FD9 RID: 28633 RVA: 0x0019E87E File Offset: 0x0019CA7E
			public void Update()
			{
				this.Draw();
				this.UpdateTimers();
			}

			// Token: 0x06006FDA RID: 28634 RVA: 0x0019E88C File Offset: 0x0019CA8C
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

			// Token: 0x06006FDB RID: 28635 RVA: 0x0019E8FE File Offset: 0x0019CAFE
			public void DrawConfirmButton()
			{
				this.DrawConfirmButton("Confirm");
			}

			// Token: 0x06006FDC RID: 28636 RVA: 0x0019E90C File Offset: 0x0019CB0C
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

			// Token: 0x06006FDD RID: 28637 RVA: 0x0019E94F File Offset: 0x0019CB4F
			public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response)
			{
				this.DrawConfirmButton(response, "Confirm");
			}

			// Token: 0x06006FDE RID: 28638 RVA: 0x0019E960 File Offset: 0x0019CB60
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

			// Token: 0x06006FDF RID: 28639 RVA: 0x0019E9A3 File Offset: 0x0019CBA3
			public void DrawCancelButton()
			{
				this.DrawCancelButton("Cancel");
			}

			// Token: 0x06006FE0 RID: 28640 RVA: 0x0019E9B0 File Offset: 0x0019CBB0
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

			// Token: 0x06006FE1 RID: 28641 RVA: 0x0019E9F2 File Offset: 0x0019CBF2
			public void Confirm()
			{
				this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
			}

			// Token: 0x06006FE2 RID: 28642 RVA: 0x0019E9FB File Offset: 0x0019CBFB
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.resultCallback(this.currentActionId, response);
				this.Close();
			}

			// Token: 0x06006FE3 RID: 28643 RVA: 0x0019EA15 File Offset: 0x0019CC15
			public void Cancel()
			{
				this.resultCallback(this.currentActionId, ControlRemappingDemo1.UserResponse.Cancel);
				this.Close();
			}

			// Token: 0x06006FE4 RID: 28644 RVA: 0x0019EA2F File Offset: 0x0019CC2F
			private void DrawWindow(int windowId)
			{
				this.windowProperties.windowDrawDelegate(this.windowProperties.title, this.windowProperties.message);
			}

			// Token: 0x06006FE5 RID: 28645 RVA: 0x0019EA57 File Offset: 0x0019CC57
			private void UpdateTimers()
			{
				if (this._busyTimerRunning && this.busyTimer <= 0f)
				{
					this._busyTimerRunning = false;
				}
			}

			// Token: 0x06006FE6 RID: 28646 RVA: 0x0019EA75 File Offset: 0x0019CC75
			private void StartBusyTimer(float time)
			{
				this._busyTime = time + Time.realtimeSinceStartup;
				this._busyTimerRunning = true;
			}

			// Token: 0x06006FE7 RID: 28647 RVA: 0x0019EA8B File Offset: 0x0019CC8B
			private void Close()
			{
				this.Reset();
				this.StateChanged(0.1f);
			}

			// Token: 0x06006FE8 RID: 28648 RVA: 0x0019EA9E File Offset: 0x0019CC9E
			private void StateChanged(float delay)
			{
				this.StartBusyTimer(delay);
			}

			// Token: 0x06006FE9 RID: 28649 RVA: 0x0019EAA7 File Offset: 0x0019CCA7
			private void Reset()
			{
				this._enabled = false;
				this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
				this.currentActionId = -1;
				this.resultCallback = null;
			}

			// Token: 0x06006FEA RID: 28650 RVA: 0x0019EAC5 File Offset: 0x0019CCC5
			private void ResetTimers()
			{
				this._busyTimerRunning = false;
			}

			// Token: 0x06006FEB RID: 28651 RVA: 0x0019EACE File Offset: 0x0019CCCE
			public void FullReset()
			{
				this.Reset();
				this.ResetTimers();
			}

			// Token: 0x04005A0F RID: 23055
			private const float openBusyDelay = 0.25f;

			// Token: 0x04005A10 RID: 23056
			private const float closeBusyDelay = 0.1f;

			// Token: 0x04005A11 RID: 23057
			private ControlRemappingDemo1.DialogHelper.DialogType _type;

			// Token: 0x04005A12 RID: 23058
			private bool _enabled;

			// Token: 0x04005A13 RID: 23059
			private float _busyTime;

			// Token: 0x04005A14 RID: 23060
			private bool _busyTimerRunning;

			// Token: 0x04005A15 RID: 23061
			private Action<int> drawWindowDelegate;

			// Token: 0x04005A16 RID: 23062
			private GUI.WindowFunction drawWindowFunction;

			// Token: 0x04005A17 RID: 23063
			private ControlRemappingDemo1.WindowProperties windowProperties;

			// Token: 0x04005A18 RID: 23064
			private int currentActionId;

			// Token: 0x04005A19 RID: 23065
			private Action<int, ControlRemappingDemo1.UserResponse> resultCallback;

			// Token: 0x02000F3F RID: 3903
			public enum DialogType
			{
				// Token: 0x04005AE3 RID: 23267
				None,
				// Token: 0x04005AE4 RID: 23268
				JoystickConflict,
				// Token: 0x04005AE5 RID: 23269
				ElementConflict,
				// Token: 0x04005AE6 RID: 23270
				KeyConflict,
				// Token: 0x04005AE7 RID: 23271
				DeleteAssignmentConfirmation = 10,
				// Token: 0x04005AE8 RID: 23272
				AssignElement
			}
		}

		// Token: 0x02000F00 RID: 3840
		private abstract class QueueEntry
		{
			// Token: 0x17002458 RID: 9304
			// (get) Token: 0x06006FEC RID: 28652 RVA: 0x0019EADC File Offset: 0x0019CCDC
			// (set) Token: 0x06006FED RID: 28653 RVA: 0x0019EAE4 File Offset: 0x0019CCE4
			public int id { get; protected set; }

			// Token: 0x17002459 RID: 9305
			// (get) Token: 0x06006FEE RID: 28654 RVA: 0x0019EAED File Offset: 0x0019CCED
			// (set) Token: 0x06006FEF RID: 28655 RVA: 0x0019EAF5 File Offset: 0x0019CCF5
			public ControlRemappingDemo1.QueueActionType queueActionType { get; protected set; }

			// Token: 0x1700245A RID: 9306
			// (get) Token: 0x06006FF0 RID: 28656 RVA: 0x0019EAFE File Offset: 0x0019CCFE
			// (set) Token: 0x06006FF1 RID: 28657 RVA: 0x0019EB06 File Offset: 0x0019CD06
			public ControlRemappingDemo1.QueueEntry.State state { get; protected set; }

			// Token: 0x1700245B RID: 9307
			// (get) Token: 0x06006FF2 RID: 28658 RVA: 0x0019EB0F File Offset: 0x0019CD0F
			// (set) Token: 0x06006FF3 RID: 28659 RVA: 0x0019EB17 File Offset: 0x0019CD17
			public ControlRemappingDemo1.UserResponse response { get; protected set; }

			// Token: 0x1700245C RID: 9308
			// (get) Token: 0x06006FF4 RID: 28660 RVA: 0x0019EB20 File Offset: 0x0019CD20
			protected static int nextId
			{
				get
				{
					int result = ControlRemappingDemo1.QueueEntry.uidCounter;
					ControlRemappingDemo1.QueueEntry.uidCounter++;
					return result;
				}
			}

			// Token: 0x06006FF5 RID: 28661 RVA: 0x0019EB33 File Offset: 0x0019CD33
			public QueueEntry(ControlRemappingDemo1.QueueActionType queueActionType)
			{
				this.id = ControlRemappingDemo1.QueueEntry.nextId;
				this.queueActionType = queueActionType;
			}

			// Token: 0x06006FF6 RID: 28662 RVA: 0x0019EB4D File Offset: 0x0019CD4D
			public void Confirm(ControlRemappingDemo1.UserResponse response)
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Confirmed;
				this.response = response;
			}

			// Token: 0x06006FF7 RID: 28663 RVA: 0x0019EB5D File Offset: 0x0019CD5D
			public void Cancel()
			{
				this.state = ControlRemappingDemo1.QueueEntry.State.Canceled;
			}

			// Token: 0x04005A1E RID: 23070
			private static int uidCounter;

			// Token: 0x02000F40 RID: 3904
			public enum State
			{
				// Token: 0x04005AEA RID: 23274
				Waiting,
				// Token: 0x04005AEB RID: 23275
				Confirmed,
				// Token: 0x04005AEC RID: 23276
				Canceled
			}
		}

		// Token: 0x02000F01 RID: 3841
		private class JoystickAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x1700245D RID: 9309
			// (get) Token: 0x06006FF8 RID: 28664 RVA: 0x0019EB66 File Offset: 0x0019CD66
			// (set) Token: 0x06006FF9 RID: 28665 RVA: 0x0019EB6E File Offset: 0x0019CD6E
			public int playerId { get; private set; }

			// Token: 0x1700245E RID: 9310
			// (get) Token: 0x06006FFA RID: 28666 RVA: 0x0019EB77 File Offset: 0x0019CD77
			// (set) Token: 0x06006FFB RID: 28667 RVA: 0x0019EB7F File Offset: 0x0019CD7F
			public int joystickId { get; private set; }

			// Token: 0x1700245F RID: 9311
			// (get) Token: 0x06006FFC RID: 28668 RVA: 0x0019EB88 File Offset: 0x0019CD88
			// (set) Token: 0x06006FFD RID: 28669 RVA: 0x0019EB90 File Offset: 0x0019CD90
			public bool assign { get; private set; }

			// Token: 0x06006FFE RID: 28670 RVA: 0x0019EB99 File Offset: 0x0019CD99
			public JoystickAssignmentChange(int newPlayerId, int joystickId, bool assign) : base(ControlRemappingDemo1.QueueActionType.JoystickAssignment)
			{
				this.playerId = newPlayerId;
				this.joystickId = joystickId;
				this.assign = assign;
			}
		}

		// Token: 0x02000F02 RID: 3842
		private class ElementAssignmentChange : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x17002460 RID: 9312
			// (get) Token: 0x06006FFF RID: 28671 RVA: 0x0019EBB7 File Offset: 0x0019CDB7
			// (set) Token: 0x06007000 RID: 28672 RVA: 0x0019EBBF File Offset: 0x0019CDBF
			public ControlRemappingDemo1.ElementAssignmentChangeType changeType { get; set; }

			// Token: 0x17002461 RID: 9313
			// (get) Token: 0x06007001 RID: 28673 RVA: 0x0019EBC8 File Offset: 0x0019CDC8
			// (set) Token: 0x06007002 RID: 28674 RVA: 0x0019EBD0 File Offset: 0x0019CDD0
			public InputMapper.Context context { get; private set; }

			// Token: 0x06007003 RID: 28675 RVA: 0x0019EBD9 File Offset: 0x0019CDD9
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType changeType, InputMapper.Context context) : base(ControlRemappingDemo1.QueueActionType.ElementAssignment)
			{
				this.changeType = changeType;
				this.context = context;
			}

			// Token: 0x06007004 RID: 28676 RVA: 0x0019EBF0 File Offset: 0x0019CDF0
			public ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChange other) : this(other.changeType, other.context.Clone())
			{
			}
		}

		// Token: 0x02000F03 RID: 3843
		private class FallbackJoystickIdentification : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x17002462 RID: 9314
			// (get) Token: 0x06007005 RID: 28677 RVA: 0x0019EC09 File Offset: 0x0019CE09
			// (set) Token: 0x06007006 RID: 28678 RVA: 0x0019EC11 File Offset: 0x0019CE11
			public int joystickId { get; private set; }

			// Token: 0x17002463 RID: 9315
			// (get) Token: 0x06007007 RID: 28679 RVA: 0x0019EC1A File Offset: 0x0019CE1A
			// (set) Token: 0x06007008 RID: 28680 RVA: 0x0019EC22 File Offset: 0x0019CE22
			public string joystickName { get; private set; }

			// Token: 0x06007009 RID: 28681 RVA: 0x0019EC2B File Offset: 0x0019CE2B
			public FallbackJoystickIdentification(int joystickId, string joystickName) : base(ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification)
			{
				this.joystickId = joystickId;
				this.joystickName = joystickName;
			}
		}

		// Token: 0x02000F04 RID: 3844
		private class Calibration : ControlRemappingDemo1.QueueEntry
		{
			// Token: 0x17002464 RID: 9316
			// (get) Token: 0x0600700A RID: 28682 RVA: 0x0019EC42 File Offset: 0x0019CE42
			// (set) Token: 0x0600700B RID: 28683 RVA: 0x0019EC4A File Offset: 0x0019CE4A
			public Player player { get; private set; }

			// Token: 0x17002465 RID: 9317
			// (get) Token: 0x0600700C RID: 28684 RVA: 0x0019EC53 File Offset: 0x0019CE53
			// (set) Token: 0x0600700D RID: 28685 RVA: 0x0019EC5B File Offset: 0x0019CE5B
			public ControllerType controllerType { get; private set; }

			// Token: 0x17002466 RID: 9318
			// (get) Token: 0x0600700E RID: 28686 RVA: 0x0019EC64 File Offset: 0x0019CE64
			// (set) Token: 0x0600700F RID: 28687 RVA: 0x0019EC6C File Offset: 0x0019CE6C
			public Joystick joystick { get; private set; }

			// Token: 0x17002467 RID: 9319
			// (get) Token: 0x06007010 RID: 28688 RVA: 0x0019EC75 File Offset: 0x0019CE75
			// (set) Token: 0x06007011 RID: 28689 RVA: 0x0019EC7D File Offset: 0x0019CE7D
			public CalibrationMap calibrationMap { get; private set; }

			// Token: 0x06007012 RID: 28690 RVA: 0x0019EC86 File Offset: 0x0019CE86
			public Calibration(Player player, Joystick joystick, CalibrationMap calibrationMap) : base(ControlRemappingDemo1.QueueActionType.Calibrate)
			{
				this.player = player;
				this.joystick = joystick;
				this.calibrationMap = calibrationMap;
				this.selectedElementIdentifierId = -1;
			}

			// Token: 0x04005A2A RID: 23082
			public int selectedElementIdentifierId;

			// Token: 0x04005A2B RID: 23083
			public bool recording;
		}

		// Token: 0x02000F05 RID: 3845
		private struct WindowProperties
		{
			// Token: 0x04005A2C RID: 23084
			public int windowId;

			// Token: 0x04005A2D RID: 23085
			public Rect rect;

			// Token: 0x04005A2E RID: 23086
			public Action<string, string> windowDrawDelegate;

			// Token: 0x04005A2F RID: 23087
			public string title;

			// Token: 0x04005A30 RID: 23088
			public string message;
		}

		// Token: 0x02000F06 RID: 3846
		private enum QueueActionType
		{
			// Token: 0x04005A32 RID: 23090
			None,
			// Token: 0x04005A33 RID: 23091
			JoystickAssignment,
			// Token: 0x04005A34 RID: 23092
			ElementAssignment,
			// Token: 0x04005A35 RID: 23093
			FallbackJoystickIdentification,
			// Token: 0x04005A36 RID: 23094
			Calibrate
		}

		// Token: 0x02000F07 RID: 3847
		private enum ElementAssignmentChangeType
		{
			// Token: 0x04005A38 RID: 23096
			Add,
			// Token: 0x04005A39 RID: 23097
			Replace,
			// Token: 0x04005A3A RID: 23098
			Remove,
			// Token: 0x04005A3B RID: 23099
			ReassignOrRemove,
			// Token: 0x04005A3C RID: 23100
			ConflictCheck
		}

		// Token: 0x02000F08 RID: 3848
		public enum UserResponse
		{
			// Token: 0x04005A3E RID: 23102
			Confirm,
			// Token: 0x04005A3F RID: 23103
			Cancel,
			// Token: 0x04005A40 RID: 23104
			Custom1,
			// Token: 0x04005A41 RID: 23105
			Custom2
		}
	}
}
