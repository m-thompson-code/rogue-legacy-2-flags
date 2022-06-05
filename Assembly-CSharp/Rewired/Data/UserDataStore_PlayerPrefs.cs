using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x02000EB5 RID: 3765
	public class UserDataStore_PlayerPrefs : UserDataStore
	{
		// Token: 0x170023AA RID: 9130
		// (get) Token: 0x06006C50 RID: 27728 RVA: 0x0003B400 File Offset: 0x00039600
		// (set) Token: 0x06006C51 RID: 27729 RVA: 0x0003B408 File Offset: 0x00039608
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
			}
		}

		// Token: 0x170023AB RID: 9131
		// (get) Token: 0x06006C52 RID: 27730 RVA: 0x0003B411 File Offset: 0x00039611
		// (set) Token: 0x06006C53 RID: 27731 RVA: 0x0003B419 File Offset: 0x00039619
		public bool LoadDataOnStart
		{
			get
			{
				return this.loadDataOnStart;
			}
			set
			{
				this.loadDataOnStart = value;
			}
		}

		// Token: 0x170023AC RID: 9132
		// (get) Token: 0x06006C54 RID: 27732 RVA: 0x0003B422 File Offset: 0x00039622
		// (set) Token: 0x06006C55 RID: 27733 RVA: 0x0003B42A File Offset: 0x0003962A
		public bool LoadJoystickAssignments
		{
			get
			{
				return this.loadJoystickAssignments;
			}
			set
			{
				this.loadJoystickAssignments = value;
			}
		}

		// Token: 0x170023AD RID: 9133
		// (get) Token: 0x06006C56 RID: 27734 RVA: 0x0003B433 File Offset: 0x00039633
		// (set) Token: 0x06006C57 RID: 27735 RVA: 0x0003B43B File Offset: 0x0003963B
		public bool LoadKeyboardAssignments
		{
			get
			{
				return this.loadKeyboardAssignments;
			}
			set
			{
				this.loadKeyboardAssignments = value;
			}
		}

		// Token: 0x170023AE RID: 9134
		// (get) Token: 0x06006C58 RID: 27736 RVA: 0x0003B444 File Offset: 0x00039644
		// (set) Token: 0x06006C59 RID: 27737 RVA: 0x0003B44C File Offset: 0x0003964C
		public bool LoadMouseAssignments
		{
			get
			{
				return this.loadMouseAssignments;
			}
			set
			{
				this.loadMouseAssignments = value;
			}
		}

		// Token: 0x170023AF RID: 9135
		// (get) Token: 0x06006C5A RID: 27738 RVA: 0x0003B455 File Offset: 0x00039655
		// (set) Token: 0x06006C5B RID: 27739 RVA: 0x0003B45D File Offset: 0x0003965D
		public string PlayerPrefsKeyPrefix
		{
			get
			{
				return this.playerPrefsKeyPrefix;
			}
			set
			{
				this.playerPrefsKeyPrefix = value;
			}
		}

		// Token: 0x170023B0 RID: 9136
		// (get) Token: 0x06006C5C RID: 27740 RVA: 0x0003B466 File Offset: 0x00039666
		private string playerPrefsKey_controllerAssignments
		{
			get
			{
				return string.Format("{0}_{1}", this.playerPrefsKeyPrefix, "ControllerAssignments");
			}
		}

		// Token: 0x170023B1 RID: 9137
		// (get) Token: 0x06006C5D RID: 27741 RVA: 0x0003B47D File Offset: 0x0003967D
		private bool loadControllerAssignments
		{
			get
			{
				return this.loadKeyboardAssignments || this.loadMouseAssignments || this.loadJoystickAssignments;
			}
		}

		// Token: 0x170023B2 RID: 9138
		// (get) Token: 0x06006C5E RID: 27742 RVA: 0x00182FE4 File Offset: 0x001811E4
		private List<int> allActionIds
		{
			get
			{
				if (this.__allActionIds != null)
				{
					return this.__allActionIds;
				}
				List<int> list = new List<int>();
				IList<InputAction> actions = ReInput.mapping.Actions;
				for (int i = 0; i < actions.Count; i++)
				{
					list.Add(actions[i].id);
				}
				this.__allActionIds = list;
				return list;
			}
		}

		// Token: 0x170023B3 RID: 9139
		// (get) Token: 0x06006C5F RID: 27743 RVA: 0x0018303C File Offset: 0x0018123C
		private string allActionIdsString
		{
			get
			{
				if (!string.IsNullOrEmpty(this.__allActionIdsString))
				{
					return this.__allActionIdsString;
				}
				StringBuilder stringBuilder = new StringBuilder();
				List<int> allActionIds = this.allActionIds;
				for (int i = 0; i < allActionIds.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(allActionIds[i]);
				}
				this.__allActionIdsString = stringBuilder.ToString();
				return this.__allActionIdsString;
			}
		}

		// Token: 0x06006C60 RID: 27744 RVA: 0x0003B497 File Offset: 0x00039697
		public override void Save()
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		// Token: 0x06006C61 RID: 27745 RVA: 0x0003B4B3 File Offset: 0x000396B3
		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06006C62 RID: 27746 RVA: 0x0003B4D2 File Offset: 0x000396D2
		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06006C63 RID: 27747 RVA: 0x0003B4F0 File Offset: 0x000396F0
		public override void SavePlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x0003B50D File Offset: 0x0003970D
		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x0003B52B File Offset: 0x0003972B
		public override void Load()
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadAll();
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x0003B548 File Offset: 0x00039748
		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06006C67 RID: 27751 RVA: 0x0003B568 File Offset: 0x00039768
		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06006C68 RID: 27752 RVA: 0x0003B587 File Offset: 0x00039787
		public override void LoadPlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadPlayerDataNow(playerId);
		}

		// Token: 0x06006C69 RID: 27753 RVA: 0x0003B5A5 File Offset: 0x000397A5
		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x0003B5C4 File Offset: 0x000397C4
		protected override void OnInitialize()
		{
			if (this.loadDataOnStart)
			{
				this.Load();
				if (this.loadControllerAssignments && ReInput.controllers.joystickCount > 0)
				{
					this.wasJoystickEverDetected = true;
					this.SaveControllerAssignments();
				}
			}
		}

		// Token: 0x06006C6B RID: 27755 RVA: 0x001830AC File Offset: 0x001812AC
		protected override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.LoadJoystickData(args.controllerId);
				if (this.loadDataOnStart && this.loadJoystickAssignments && !this.wasJoystickEverDetected)
				{
					base.StartCoroutine(this.LoadJoystickAssignmentsDeferred());
				}
				if (this.loadJoystickAssignments && !this.deferredJoystickAssignmentLoadPending)
				{
					this.SaveControllerAssignments();
				}
				this.wasJoystickEverDetected = true;
			}
		}

		// Token: 0x06006C6C RID: 27756 RVA: 0x0003B5F7 File Offset: 0x000397F7
		protected override void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickData(args.controllerId);
			}
		}

		// Token: 0x06006C6D RID: 27757 RVA: 0x0003B617 File Offset: 0x00039817
		protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x0018311C File Offset: 0x0018131C
		public override void SaveControllerMap(int playerId, ControllerMap controllerMap)
		{
			if (controllerMap == null)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			this.SaveControllerMap(player, controllerMap);
		}

		// Token: 0x06006C6F RID: 27759 RVA: 0x00183148 File Offset: 0x00181348
		public override ControllerMap LoadControllerMap(int playerId, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return null;
			}
			return this.LoadControllerMap(player, controllerIdentifier, categoryId, layoutId);
		}

		// Token: 0x06006C70 RID: 27760 RVA: 0x00183174 File Offset: 0x00181374
		private int LoadAll()
		{
			int num = 0;
			if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
			{
				num++;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				num += this.LoadPlayerDataNow(allPlayers[i]);
			}
			return num + this.LoadAllJoystickCalibrationData();
		}

		// Token: 0x06006C71 RID: 27761 RVA: 0x0003B631 File Offset: 0x00039831
		private int LoadPlayerDataNow(int playerId)
		{
			return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
		}

		// Token: 0x06006C72 RID: 27762 RVA: 0x001831D0 File Offset: 0x001813D0
		private int LoadPlayerDataNow(Player player)
		{
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			num += this.LoadInputBehaviors(player.id);
			num += this.LoadControllerMaps(player.id, ControllerType.Keyboard, 0);
			num += this.LoadControllerMaps(player.id, ControllerType.Mouse, 0);
			foreach (Joystick joystick in player.controllers.Joysticks)
			{
				num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystick.id);
			}
			this.RefreshLayoutManager(player.id);
			return num;
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x00183278 File Offset: 0x00181478
		private int LoadAllJoystickCalibrationData()
		{
			int num = 0;
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				num += this.LoadJoystickCalibrationData(joysticks[i]);
			}
			return num;
		}

		// Token: 0x06006C74 RID: 27764 RVA: 0x0003B644 File Offset: 0x00039844
		private int LoadJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return 0;
			}
			if (!joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick)))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06006C75 RID: 27765 RVA: 0x0003B65D File Offset: 0x0003985D
		private int LoadJoystickCalibrationData(int joystickId)
		{
			return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06006C76 RID: 27766 RVA: 0x001832B4 File Offset: 0x001814B4
		private int LoadJoystickData(int joystickId)
		{
			int num = 0;
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
				{
					num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystickId);
					this.RefreshLayoutManager(player.id);
				}
			}
			return num + this.LoadJoystickCalibrationData(joystickId);
		}

		// Token: 0x06006C77 RID: 27767 RVA: 0x0003B670 File Offset: 0x00039870
		private int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0 + this.LoadControllerMaps(playerId, controllerType, controllerId);
			this.RefreshLayoutManager(playerId);
			return num + this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06006C78 RID: 27768 RVA: 0x00183320 File Offset: 0x00181520
		private int LoadControllerDataNow(ControllerType controllerType, int controllerId)
		{
			int num = 0;
			if (controllerType == ControllerType.Joystick)
			{
				num += this.LoadJoystickCalibrationData(controllerId);
			}
			return num;
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x00183340 File Offset: 0x00181540
		private int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return num;
			}
			Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
			if (controller == null)
			{
				return num;
			}
			IList<InputMapCategory> mapCategories = ReInput.mapping.MapCategories;
			for (int i = 0; i < mapCategories.Count; i++)
			{
				InputMapCategory inputMapCategory = mapCategories[i];
				if (inputMapCategory.userAssignable)
				{
					IList<InputLayout> list = ReInput.mapping.MapLayouts(controller.type);
					for (int j = 0; j < list.Count; j++)
					{
						InputLayout inputLayout = list[j];
						ControllerMap controllerMap = this.LoadControllerMap(player, controller.identifier, inputMapCategory.id, inputLayout.id);
						if (controllerMap != null)
						{
							player.controllers.maps.AddMap(controller, controllerMap);
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x00183418 File Offset: 0x00181618
		private ControllerMap LoadControllerMap(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			if (player == null)
			{
				return null;
			}
			string controllerMapXml = this.GetControllerMapXml(player, controllerIdentifier, categoryId, layoutId);
			if (string.IsNullOrEmpty(controllerMapXml))
			{
				return null;
			}
			ControllerMap controllerMap = ControllerMap.CreateFromXml(controllerIdentifier.controllerType, controllerMapXml);
			if (controllerMap == null)
			{
				return null;
			}
			List<int> controllerMapKnownActionIds = this.GetControllerMapKnownActionIds(player, controllerIdentifier, categoryId, layoutId);
			this.AddDefaultMappingsForNewActions(controllerIdentifier, controllerMap, controllerMapKnownActionIds);
			return controllerMap;
		}

		// Token: 0x06006C7B RID: 27771 RVA: 0x0018346C File Offset: 0x0018166C
		private int LoadInputBehaviors(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			IList<InputBehavior> inputBehaviors = ReInput.mapping.GetInputBehaviors(player.id);
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				num += this.LoadInputBehaviorNow(player, inputBehaviors[i]);
			}
			return num;
		}

		// Token: 0x06006C7C RID: 27772 RVA: 0x001834C0 File Offset: 0x001816C0
		private int LoadInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return 0;
			}
			return this.LoadInputBehaviorNow(player, inputBehavior);
		}

		// Token: 0x06006C7D RID: 27773 RVA: 0x001834F8 File Offset: 0x001816F8
		private int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return 0;
			}
			string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.id);
			if (inputBehaviorXml == null || inputBehaviorXml == string.Empty)
			{
				return 0;
			}
			if (!inputBehavior.ImportXmlString(inputBehaviorXml))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06006C7E RID: 27774 RVA: 0x0018353C File Offset: 0x0018173C
		private bool LoadControllerAssignmentsNow()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = this.LoadControllerAssignmentData();
				if (controllerAssignmentSaveInfo == null)
				{
					return false;
				}
				if (this.loadKeyboardAssignments || this.loadMouseAssignments)
				{
					this.LoadKeyboardAndMouseAssignmentsNow(controllerAssignmentSaveInfo);
				}
				if (this.loadJoystickAssignments)
				{
					this.LoadJoystickAssignmentsNow(controllerAssignmentSaveInfo);
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06006C7F RID: 27775 RVA: 0x00183598 File Offset: 0x00181798
		private bool LoadKeyboardAndMouseAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player.id)];
						if (this.loadKeyboardAssignments)
						{
							player.controllers.hasKeyboard = playerInfo.hasKeyboard;
						}
						if (this.loadMouseAssignments)
						{
							player.controllers.hasMouse = playerInfo.hasMouse;
						}
					}
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x00183660 File Offset: 0x00181860
		private bool LoadJoystickAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (ReInput.controllers.joystickCount == 0)
				{
					return false;
				}
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					player.controllers.ClearControllersOfType(ControllerType.Joystick);
				}
				List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo> list = this.loadJoystickAssignments ? new List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>() : null;
				foreach (Player player2 in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player2.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player2.id)];
						for (int i = 0; i < playerInfo.joystickCount; i++)
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo2 = playerInfo.joysticks[i];
							if (joystickInfo2 != null)
							{
								Joystick joystick = this.FindJoystickPrecise(joystickInfo2);
								if (joystick != null)
								{
									if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == joystick) == null)
									{
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystickInfo2.id));
									}
									player2.controllers.AddController(joystick, false);
								}
							}
						}
					}
				}
				if (this.allowImpreciseJoystickAssignmentMatching)
				{
					foreach (Player player3 in ReInput.players.AllPlayers)
					{
						if (data.ContainsPlayer(player3.id))
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo2 = data.players[data.IndexOfPlayer(player3.id)];
							for (int j = 0; j < playerInfo2.joystickCount; j++)
							{
								UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerInfo2.joysticks[j];
								if (joystickInfo != null)
								{
									Joystick joystick2 = null;
									int num = list.FindIndex((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.oldJoystickId == joystickInfo.id);
									if (num >= 0)
									{
										joystick2 = list[num].joystick;
									}
									else
									{
										List<Joystick> list2;
										if (!this.TryFindJoysticksImprecise(joystickInfo, out list2))
										{
											goto IL_298;
										}
										using (List<Joystick>.Enumerator enumerator2 = list2.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												Joystick match = enumerator2.Current;
												if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == match) == null)
												{
													joystick2 = match;
													break;
												}
											}
										}
										if (joystick2 == null)
										{
											goto IL_298;
										}
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
									}
									player3.controllers.AddController(joystick2, false);
								}
								IL_298:;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (ReInput.configuration.autoAssignJoysticks)
			{
				ReInput.controllers.AutoAssignJoysticks();
			}
			return true;
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x001839CC File Offset: 0x00181BCC
		private UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
		{
			UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo result;
			try
			{
				if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
				{
					result = null;
				}
				else
				{
					string @string = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
					if (string.IsNullOrEmpty(@string))
					{
						result = null;
					}
					else
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo>(@string);
						if (controllerAssignmentSaveInfo == null || controllerAssignmentSaveInfo.playerCount == 0)
						{
							result = null;
						}
						else
						{
							result = controllerAssignmentSaveInfo;
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06006C82 RID: 27778 RVA: 0x0003B68D File Offset: 0x0003988D
		private IEnumerator LoadJoystickAssignmentsDeferred()
		{
			this.deferredJoystickAssignmentLoadPending = true;
			yield return new WaitForEndOfFrame();
			if (!ReInput.isReady)
			{
				yield break;
			}
			this.LoadJoystickAssignmentsNow(null);
			this.SaveControllerAssignments();
			this.deferredJoystickAssignmentLoadPending = false;
			yield break;
		}

		// Token: 0x06006C83 RID: 27779 RVA: 0x00183A34 File Offset: 0x00181C34
		private void SaveAll()
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.SavePlayerDataNow(allPlayers[i]);
			}
			this.SaveAllJoystickCalibrationData();
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06006C84 RID: 27780 RVA: 0x0003B69C File Offset: 0x0003989C
		private void SavePlayerDataNow(int playerId)
		{
			this.SavePlayerDataNow(ReInput.players.GetPlayer(playerId));
			PlayerPrefs.Save();
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x00183A84 File Offset: 0x00181C84
		private void SavePlayerDataNow(Player player)
		{
			if (player == null)
			{
				return;
			}
			PlayerSaveData saveData = player.GetSaveData(true);
			this.SaveInputBehaviors(player, saveData);
			this.SaveControllerMaps(player, saveData);
		}

		// Token: 0x06006C86 RID: 27782 RVA: 0x00183AB0 File Offset: 0x00181CB0
		private void SaveAllJoystickCalibrationData()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		// Token: 0x06006C87 RID: 27783 RVA: 0x0003B6B4 File Offset: 0x000398B4
		private void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x00183AE8 File Offset: 0x00181CE8
		private void SaveJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			PlayerPrefs.SetString(this.GetJoystickCalibrationMapPlayerPrefsKey(joystick), calibrationMapSaveData.map.ToXmlString());
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x00183B18 File Offset: 0x00181D18
		private void SaveJoystickData(int joystickId)
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
				{
					this.SaveControllerMaps(player.id, ControllerType.Joystick, joystickId);
				}
			}
			this.SaveJoystickCalibrationData(joystickId);
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x0003B6C7 File Offset: 0x000398C7
		private void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerDataNow(controllerType, controllerId);
			PlayerPrefs.Save();
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x0003B6DF File Offset: 0x000398DF
		private void SaveControllerDataNow(ControllerType controllerType, int controllerId)
		{
			if (controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06006C8C RID: 27788 RVA: 0x00183B70 File Offset: 0x00181D70
		private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
		{
			foreach (ControllerMapSaveData controllerMapSaveData in playerSaveData.AllControllerMapSaveData)
			{
				this.SaveControllerMap(player, controllerMapSaveData.map);
			}
		}

		// Token: 0x06006C8D RID: 27789 RVA: 0x00183BC4 File Offset: 0x00181DC4
		private void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(controllerType, controllerId))
			{
				return;
			}
			ControllerMapSaveData[] mapSaveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
			if (mapSaveData == null)
			{
				return;
			}
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				this.SaveControllerMap(player, mapSaveData[i].map);
			}
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x00183C28 File Offset: 0x00181E28
		private void SaveControllerMap(Player player, ControllerMap controllerMap)
		{
			PlayerPrefs.SetString(this.GetControllerMapPlayerPrefsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 2), controllerMap.ToXmlString());
			PlayerPrefs.SetString(this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 2), this.allActionIdsString);
		}

		// Token: 0x06006C8F RID: 27791 RVA: 0x00183C8C File Offset: 0x00181E8C
		private void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData)
		{
			if (player == null)
			{
				return;
			}
			InputBehavior[] inputBehaviors = playerSaveData.inputBehaviors;
			for (int i = 0; i < inputBehaviors.Length; i++)
			{
				this.SaveInputBehaviorNow(player, inputBehaviors[i]);
			}
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x00183CC0 File Offset: 0x00181EC0
		private void SaveInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			this.SaveInputBehaviorNow(player, inputBehavior);
			PlayerPrefs.Save();
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x0003B6F1 File Offset: 0x000398F1
		private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			PlayerPrefs.SetString(this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.id), inputBehavior.ToXmlString());
		}

		// Token: 0x06006C92 RID: 27794 RVA: 0x00183CFC File Offset: 0x00181EFC
		private bool SaveControllerAssignments()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
				for (int i = 0; i < ReInput.players.allPlayerCount; i++)
				{
					Player player = ReInput.players.AllPlayers[i];
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
					controllerAssignmentSaveInfo.players[i] = playerInfo;
					playerInfo.id = player.id;
					playerInfo.hasKeyboard = player.controllers.hasKeyboard;
					playerInfo.hasMouse = player.controllers.hasMouse;
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] array = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[player.controllers.joystickCount];
					playerInfo.joysticks = array;
					for (int j = 0; j < player.controllers.joystickCount; j++)
					{
						Joystick joystick = player.controllers.Joysticks[j];
						array[j] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo
						{
							instanceGuid = joystick.deviceInstanceGuid,
							id = joystick.id,
							hardwareIdentifier = joystick.hardwareIdentifier
						};
					}
				}
				PlayerPrefs.SetString(this.playerPrefsKey_controllerAssignments, JsonWriter.ToJson(controllerAssignmentSaveInfo));
				PlayerPrefs.Save();
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06006C93 RID: 27795 RVA: 0x0003B712 File Offset: 0x00039912
		private bool ControllerAssignmentSaveDataExists()
		{
			return PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments) && !string.IsNullOrEmpty(PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments));
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x0003B738 File Offset: 0x00039938
		private string GetBasePlayerPrefsKey(Player player)
		{
			return this.playerPrefsKeyPrefix + "|playerName=" + player.name;
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x0003B750 File Offset: 0x00039950
		private string GetControllerMapPlayerPrefsKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			return this.GetBasePlayerPrefsKey(player) + "|dataType=ControllerMap" + UserDataStore_PlayerPrefs.GetControllerMapPlayerPrefsKeyCommonSuffix(player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x0003B774 File Offset: 0x00039974
		private string GetControllerMapKnownActionIdsPlayerPrefsKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			return this.GetBasePlayerPrefsKey(player) + "|dataType=ControllerMap_KnownActionIds" + UserDataStore_PlayerPrefs.GetControllerMapPlayerPrefsKeyCommonSuffix(player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
		}

		// Token: 0x06006C97 RID: 27799 RVA: 0x00183E3C File Offset: 0x0018203C
		private static string GetControllerMapPlayerPrefsKeyCommonSuffix(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion)
		{
			string text = "";
			if (ppKeyVersion >= 2)
			{
				text = text + "|kv=" + ppKeyVersion.ToString();
			}
			text = text + "|controllerMapType=" + UserDataStore_PlayerPrefs.GetControllerMapType(controllerIdentifier.controllerType).Name;
			text = string.Concat(new string[]
			{
				text,
				"|categoryId=",
				categoryId.ToString(),
				"|layoutId=",
				layoutId.ToString()
			});
			if (ppKeyVersion >= 2)
			{
				text = text + "|hardwareGuid=" + controllerIdentifier.hardwareTypeGuid.ToString();
				if (controllerIdentifier.hardwareTypeGuid == Guid.Empty)
				{
					text = text + "|hardwareIdentifier=" + controllerIdentifier.hardwareIdentifier;
				}
				if (controllerIdentifier.controllerType == ControllerType.Joystick)
				{
					text = text + "|duplicate=" + UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controllerIdentifier).ToString();
				}
			}
			else
			{
				text = text + "|hardwareIdentifier=" + controllerIdentifier.hardwareIdentifier;
				if (controllerIdentifier.controllerType == ControllerType.Joystick)
				{
					text = text + "|hardwareGuid=" + controllerIdentifier.hardwareTypeGuid.ToString();
					if (ppKeyVersion >= 1)
					{
						text = text + "|duplicate=" + UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controllerIdentifier).ToString();
					}
				}
			}
			return text;
		}

		// Token: 0x06006C98 RID: 27800 RVA: 0x00183F8C File Offset: 0x0018218C
		private string GetJoystickCalibrationMapPlayerPrefsKey(Joystick joystick)
		{
			return this.playerPrefsKeyPrefix + "|dataType=CalibrationMap" + "|controllerType=" + joystick.type.ToString() + "|hardwareIdentifier=" + joystick.hardwareIdentifier + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x0003B798 File Offset: 0x00039998
		private string GetInputBehaviorPlayerPrefsKey(Player player, int inputBehaviorId)
		{
			return this.GetBasePlayerPrefsKey(player) + "|dataType=InputBehavior" + "|id=" + inputBehaviorId.ToString();
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x00183FF8 File Offset: 0x001821F8
		private string GetControllerMapXml(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			for (int i = 2; i >= 0; i--)
			{
				string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controllerIdentifier, categoryId, layoutId, i);
				if (PlayerPrefs.HasKey(controllerMapPlayerPrefsKey))
				{
					return PlayerPrefs.GetString(controllerMapPlayerPrefsKey);
				}
			}
			return null;
		}

		// Token: 0x06006C9B RID: 27803 RVA: 0x00184030 File Offset: 0x00182230
		private List<int> GetControllerMapKnownActionIds(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			List<int> list = new List<int>();
			string key = null;
			bool flag = false;
			for (int i = 2; i >= 0; i--)
			{
				key = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controllerIdentifier, categoryId, layoutId, i);
				if (PlayerPrefs.HasKey(key))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return list;
			}
			string @string = PlayerPrefs.GetString(key);
			if (string.IsNullOrEmpty(@string))
			{
				return list;
			}
			string[] array = @string.Split(new char[]
			{
				','
			});
			for (int j = 0; j < array.Length; j++)
			{
				int item;
				if (!string.IsNullOrEmpty(array[j]) && int.TryParse(array[j], out item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x001840D0 File Offset: 0x001822D0
		private string GetJoystickCalibrationMapXml(Joystick joystick)
		{
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
			if (!PlayerPrefs.HasKey(joystickCalibrationMapPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(joystickCalibrationMapPlayerPrefsKey);
		}

		// Token: 0x06006C9D RID: 27805 RVA: 0x001840FC File Offset: 0x001822FC
		private string GetInputBehaviorXml(Player player, int id)
		{
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, id);
			if (!PlayerPrefs.HasKey(inputBehaviorPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(inputBehaviorPlayerPrefsKey);
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x00184128 File Offset: 0x00182328
		private void AddDefaultMappingsForNewActions(ControllerIdentifier controllerIdentifier, ControllerMap controllerMap, List<int> knownActionIds)
		{
			if (controllerMap == null || knownActionIds == null)
			{
				return;
			}
			if (knownActionIds == null || knownActionIds.Count == 0)
			{
				return;
			}
			ControllerMap controllerMapInstance = ReInput.mapping.GetControllerMapInstance(controllerIdentifier, controllerMap.categoryId, controllerMap.layoutId);
			if (controllerMapInstance == null)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (int item in this.allActionIds)
			{
				if (!knownActionIds.Contains(item))
				{
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			foreach (ActionElementMap actionElementMap in controllerMapInstance.AllMaps)
			{
				if (list.Contains(actionElementMap.actionId) && !controllerMap.DoesElementAssignmentConflict(actionElementMap))
				{
					ElementAssignment elementAssignment = new ElementAssignment(controllerMap.controllerType, actionElementMap.elementType, actionElementMap.elementIdentifierId, actionElementMap.axisRange, actionElementMap.keyCode, actionElementMap.modifierKeyFlags, actionElementMap.actionId, actionElementMap.axisContribution, actionElementMap.invert);
					controllerMap.CreateElementMap(elementAssignment);
				}
			}
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x00184264 File Offset: 0x00182464
		private Joystick FindJoystickPrecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
		{
			if (joystickInfo == null)
			{
				return null;
			}
			if (joystickInfo.instanceGuid == Guid.Empty)
			{
				return null;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].deviceInstanceGuid == joystickInfo.instanceGuid)
				{
					return joysticks[i];
				}
			}
			return null;
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x001842C8 File Offset: 0x001824C8
		private bool TryFindJoysticksImprecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo, out List<Joystick> matches)
		{
			matches = null;
			if (joystickInfo == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
			{
				return false;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (string.Equals(joysticks[i].hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase))
				{
					if (matches == null)
					{
						matches = new List<Joystick>();
					}
					matches.Add(joysticks[i]);
				}
			}
			return matches != null;
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x00184340 File Offset: 0x00182540
		private static int GetDuplicateIndex(Player player, ControllerIdentifier controllerIdentifier)
		{
			Controller controller = ReInput.controllers.GetController(controllerIdentifier);
			if (controller == null)
			{
				return 0;
			}
			int num = 0;
			foreach (Controller controller2 in player.controllers.Controllers)
			{
				if (controller2.type == controller.type)
				{
					bool flag = false;
					if (controller.type == ControllerType.Joystick)
					{
						if ((controller2 as Joystick).hardwareTypeGuid != controller.hardwareTypeGuid)
						{
							continue;
						}
						if (controller.hardwareTypeGuid != Guid.Empty)
						{
							flag = true;
						}
					}
					if (flag || !(controller2.hardwareIdentifier != controller.hardwareIdentifier))
					{
						if (controller2 == controller)
						{
							return num;
						}
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006CA2 RID: 27810 RVA: 0x00184410 File Offset: 0x00182610
		private void RefreshLayoutManager(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			player.controllers.maps.layoutManager.Apply();
		}

		// Token: 0x06006CA3 RID: 27811 RVA: 0x00184444 File Offset: 0x00182644
		private static Type GetControllerMapType(ControllerType controllerType)
		{
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				return typeof(KeyboardMap);
			case ControllerType.Mouse:
				return typeof(MouseMap);
			case ControllerType.Joystick:
				return typeof(JoystickMap);
			default:
				if (controllerType == ControllerType.Custom)
				{
					return typeof(CustomControllerMap);
				}
				Debug.LogWarning("Rewired: Unknown ControllerType " + controllerType.ToString());
				return null;
			}
		}

		// Token: 0x0400579F RID: 22431
		private const string thisScriptName = "UserDataStore_PlayerPrefs";

		// Token: 0x040057A0 RID: 22432
		private const string logPrefix = "Rewired: ";

		// Token: 0x040057A1 RID: 22433
		private const string editorLoadedMessage = "\n***IMPORTANT:*** Changes made to the Rewired Input Manager configuration after the last time XML data was saved WILL NOT be used because the loaded old saved data has overwritten these values. If you change something in the Rewired Input Manager such as a Joystick Map or Input Behavior settings, you will not see these changes reflected in the current configuration. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component.";

		// Token: 0x040057A2 RID: 22434
		private const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";

		// Token: 0x040057A3 RID: 22435
		private const int controllerMapPPKeyVersion_original = 0;

		// Token: 0x040057A4 RID: 22436
		private const int controllerMapPPKeyVersion_includeDuplicateJoystickIndex = 1;

		// Token: 0x040057A5 RID: 22437
		private const int controllerMapPPKeyVersion_supportDisconnectedControllers = 2;

		// Token: 0x040057A6 RID: 22438
		private const int controllerMapPPKeyVersion_includeFormatVersion = 2;

		// Token: 0x040057A7 RID: 22439
		private const int controllerMapPPKeyVersion = 2;

		// Token: 0x040057A8 RID: 22440
		[Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
		[SerializeField]
		private bool isEnabled = true;

		// Token: 0x040057A9 RID: 22441
		[Tooltip("Should saved data be loaded on start?")]
		[SerializeField]
		private bool loadDataOnStart = true;

		// Token: 0x040057AA RID: 22442
		[Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
		[SerializeField]
		private bool loadJoystickAssignments = true;

		// Token: 0x040057AB RID: 22443
		[Tooltip("Should Player Keyboard assignments be saved and loaded?")]
		[SerializeField]
		private bool loadKeyboardAssignments = true;

		// Token: 0x040057AC RID: 22444
		[Tooltip("Should Player Mouse assignments be saved and loaded?")]
		[SerializeField]
		private bool loadMouseAssignments = true;

		// Token: 0x040057AD RID: 22445
		[Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")]
		[SerializeField]
		private string playerPrefsKeyPrefix = "RewiredSaveData";

		// Token: 0x040057AE RID: 22446
		[NonSerialized]
		private bool allowImpreciseJoystickAssignmentMatching = true;

		// Token: 0x040057AF RID: 22447
		[NonSerialized]
		private bool deferredJoystickAssignmentLoadPending;

		// Token: 0x040057B0 RID: 22448
		[NonSerialized]
		private bool wasJoystickEverDetected;

		// Token: 0x040057B1 RID: 22449
		[NonSerialized]
		private List<int> __allActionIds;

		// Token: 0x040057B2 RID: 22450
		[NonSerialized]
		private string __allActionIdsString;

		// Token: 0x02000EB6 RID: 3766
		private class ControllerAssignmentSaveInfo
		{
			// Token: 0x170023B4 RID: 9140
			// (get) Token: 0x06006CA5 RID: 27813 RVA: 0x0003B7F9 File Offset: 0x000399F9
			public int playerCount
			{
				get
				{
					if (this.players == null)
					{
						return 0;
					}
					return this.players.Length;
				}
			}

			// Token: 0x06006CA6 RID: 27814 RVA: 0x00002AD6 File Offset: 0x00000CD6
			public ControllerAssignmentSaveInfo()
			{
			}

			// Token: 0x06006CA7 RID: 27815 RVA: 0x001844B4 File Offset: 0x001826B4
			public ControllerAssignmentSaveInfo(int playerCount)
			{
				this.players = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
				for (int i = 0; i < playerCount; i++)
				{
					this.players[i] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
				}
			}

			// Token: 0x06006CA8 RID: 27816 RVA: 0x001844EC File Offset: 0x001826EC
			public int IndexOfPlayer(int playerId)
			{
				for (int i = 0; i < this.playerCount; i++)
				{
					if (this.players[i] != null && this.players[i].id == playerId)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06006CA9 RID: 27817 RVA: 0x0003B80D File Offset: 0x00039A0D
			public bool ContainsPlayer(int playerId)
			{
				return this.IndexOfPlayer(playerId) >= 0;
			}

			// Token: 0x040057B3 RID: 22451
			public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[] players;

			// Token: 0x02000EB7 RID: 3767
			public class PlayerInfo
			{
				// Token: 0x170023B5 RID: 9141
				// (get) Token: 0x06006CAA RID: 27818 RVA: 0x0003B81C File Offset: 0x00039A1C
				public int joystickCount
				{
					get
					{
						if (this.joysticks == null)
						{
							return 0;
						}
						return this.joysticks.Length;
					}
				}

				// Token: 0x06006CAB RID: 27819 RVA: 0x00184528 File Offset: 0x00182728
				public int IndexOfJoystick(int joystickId)
				{
					for (int i = 0; i < this.joystickCount; i++)
					{
						if (this.joysticks[i] != null && this.joysticks[i].id == joystickId)
						{
							return i;
						}
					}
					return -1;
				}

				// Token: 0x06006CAC RID: 27820 RVA: 0x0003B830 File Offset: 0x00039A30
				public bool ContainsJoystick(int joystickId)
				{
					return this.IndexOfJoystick(joystickId) >= 0;
				}

				// Token: 0x040057B4 RID: 22452
				public int id;

				// Token: 0x040057B5 RID: 22453
				public bool hasKeyboard;

				// Token: 0x040057B6 RID: 22454
				public bool hasMouse;

				// Token: 0x040057B7 RID: 22455
				public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;
			}

			// Token: 0x02000EB8 RID: 3768
			public class JoystickInfo
			{
				// Token: 0x040057B8 RID: 22456
				public Guid instanceGuid;

				// Token: 0x040057B9 RID: 22457
				public string hardwareIdentifier;

				// Token: 0x040057BA RID: 22458
				public int id;
			}
		}

		// Token: 0x02000EB9 RID: 3769
		private class JoystickAssignmentHistoryInfo
		{
			// Token: 0x06006CAF RID: 27823 RVA: 0x0003B83F File Offset: 0x00039A3F
			public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
			{
				if (joystick == null)
				{
					throw new ArgumentNullException("joystick");
				}
				this.joystick = joystick;
				this.oldJoystickId = oldJoystickId;
			}

			// Token: 0x040057BB RID: 22459
			public readonly Joystick joystick;

			// Token: 0x040057BC RID: 22460
			public readonly int oldJoystickId;
		}
	}
}
