using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class BaseSpecialRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001199 RID: 4505
	// (get) Token: 0x06002F19 RID: 12057 RVA: 0x000A0BA1 File Offset: 0x0009EDA1
	public IRelayLink RoomCompletedRelay
	{
		get
		{
			return this.m_roomCompletedRelay.link;
		}
	}

	// Token: 0x1700119A RID: 4506
	// (get) Token: 0x06002F1A RID: 12058 RVA: 0x000A0BAE File Offset: 0x0009EDAE
	public IRelayLink SpecialRoomInitializedRelay
	{
		get
		{
			return this.m_roomInitializedRelay.link;
		}
	}

	// Token: 0x1700119B RID: 4507
	// (get) Token: 0x06002F1B RID: 12059 RVA: 0x000A0BBB File Offset: 0x0009EDBB
	public virtual SpecialRoomType SpecialRoomType
	{
		get
		{
			return this.m_specialRoomType;
		}
	}

	// Token: 0x1700119C RID: 4508
	// (get) Token: 0x06002F1C RID: 12060 RVA: 0x000A0BC3 File Offset: 0x0009EDC3
	// (set) Token: 0x06002F1D RID: 12061 RVA: 0x000A0BCB File Offset: 0x0009EDCB
	public BaseRoom Room { get; private set; }

	// Token: 0x1700119D RID: 4509
	// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000A0BD4 File Offset: 0x0009EDD4
	// (set) Token: 0x06002F1F RID: 12063 RVA: 0x000A0BDC File Offset: 0x0009EDDC
	public bool IsRoomComplete { get; protected set; }

	// Token: 0x1700119E RID: 4510
	// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000A0BE5 File Offset: 0x0009EDE5
	// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000A0BED File Offset: 0x0009EDED
	public RoomSaveData RoomSaveData { get; private set; }

	// Token: 0x06002F22 RID: 12066 RVA: 0x000A0BF8 File Offset: 0x0009EDF8
	protected virtual void Awake()
	{
		this.m_roomCompletedEventArgs = new SpecialRoomCompletedEventArgs(this);
		this.m_onRoomDataLoaded = new Action(this.OnRoomDataLoaded);
		this.m_onRoomDataSaved = new Action<bool>(this.OnRoomDataSaved);
		this.m_onPlayerExitRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom);
		this.m_playerEnterRoomInitialization = new Action<object, RoomViaDoorEventArgs>(this.PlayerEnterRoomInitialization);
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x000A0C5C File Offset: 0x0009EE5C
	private void SaveRoomMiscData()
	{
		if (this.m_roomMiscDataTable != null)
		{
			string text = "";
			foreach (KeyValuePair<string, string> keyValuePair in this.m_roomMiscDataTable)
			{
				text += string.Format("<{0}:{1}>", keyValuePair.Key, keyValuePair.Value);
			}
			this.RoomSaveData.RoomMiscData = text;
		}
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x000A0CE4 File Offset: 0x0009EEE4
	private void LoadRoomMiscData()
	{
		if (!string.IsNullOrEmpty(this.RoomSaveData.RoomMiscData) && this.m_roomMiscDataTable == null)
		{
			this.m_roomMiscDataTable = new Dictionary<string, string>();
		}
		if (this.m_roomMiscDataTable != null)
		{
			this.m_roomMiscDataTable.Clear();
			if (!string.IsNullOrEmpty(this.RoomSaveData.RoomMiscData))
			{
				string roomMiscData = this.RoomSaveData.RoomMiscData;
				int num = 0;
				int num2 = 0;
				while (num != -1 && num2 != -1)
				{
					num = roomMiscData.IndexOf('<', num2);
					if (num != -1)
					{
						num2 = roomMiscData.IndexOf('>', num);
						int num3 = roomMiscData.IndexOf(':', num);
						int length = num3 - (num + 1);
						int length2 = num2 - (num3 + 1);
						string key = roomMiscData.Substring(num + 1, length);
						string value = roomMiscData.Substring(num3 + 1, length2);
						if (this.m_roomMiscDataTable.ContainsKey(key))
						{
							this.m_roomMiscDataTable[key] = value;
						}
						else
						{
							this.m_roomMiscDataTable.Add(key, value);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x000A0DD8 File Offset: 0x0009EFD8
	public string GetRoomMiscData(string id)
	{
		string result;
		if (this.m_roomMiscDataTable != null && this.m_roomMiscDataTable.TryGetValue(id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x000A0E00 File Offset: 0x0009F000
	public void SetRoomMiscData(string id, string value)
	{
		if (this.m_roomMiscDataTable == null)
		{
			this.m_roomMiscDataTable = new Dictionary<string, string>();
		}
		if (this.m_roomMiscDataTable.ContainsKey(id))
		{
			this.m_roomMiscDataTable[id] = value;
			return;
		}
		this.m_roomMiscDataTable.Add(id, value);
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x000A0E3E File Offset: 0x0009F03E
	public virtual void RoomCompleted()
	{
		if (this.Room != null)
		{
			this.IsRoomComplete = true;
			this.m_roomCompletedRelay.Dispatch();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SpecialRoomCompleted, this, this.m_roomCompletedEventArgs);
		}
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x000A0E6E File Offset: 0x0009F06E
	protected void ForceRoomCompleted()
	{
		if (this.Room != null)
		{
			this.IsRoomComplete = true;
		}
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x000A0E88 File Offset: 0x0009F088
	public virtual void SetRoom(BaseRoom room)
	{
		this.Room = room;
		if (this.Room)
		{
			Room room2 = room as Room;
			if (room2)
			{
				room2.SetCanMerge(false);
			}
			this.Room.PlayerEnterRelay.AddListener(this.m_playerEnterRoomInitialization, false);
			this.Room.PlayerExitRelay.AddListener(this.m_onPlayerExitRoom, false);
			this.Room.SaveController.OnRoomDataLoadedRelay.AddListener(this.m_onRoomDataLoaded, false);
			this.Room.SaveController.OnRoomDataSavedRelay.AddListener(this.m_onRoomDataSaved, false);
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Room Field is null. Did you forget to connect the Room Component to the Room Field in the Inspector?</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x000A0F44 File Offset: 0x0009F144
	protected virtual void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_playerEnterRoomInitialization);
			this.Room.PlayerExitRelay.RemoveListener(this.m_onPlayerExitRoom);
			this.Room.SaveController.OnRoomDataLoadedRelay.RemoveListener(this.m_onRoomDataLoaded);
			this.Room.SaveController.OnRoomDataSavedRelay.RemoveListener(this.m_onRoomDataSaved);
		}
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x000A0FC4 File Offset: 0x0009F1C4
	private void PlayerEnterRoomInitialization(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.OnPlayerEnterRoom(sender, eventArgs);
		this.m_roomInitializedRelay.Dispatch();
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x000A0FDC File Offset: 0x0009F1DC
	protected virtual void OnRoomDataLoaded()
	{
		if (this.Room == null)
		{
			return;
		}
		if (this.Room.BiomeControllerIndex == -1)
		{
			return;
		}
		this.RoomSaveData = SaveManager.StageSaveData.GetRoomSaveData(this.Room.BiomeType, this.Room.BiomeControllerIndex);
		if (!this.RoomSaveData.IsNativeNull())
		{
			this.LoadRoomMiscData();
			this.IsRoomComplete = this.RoomSaveData.IsRoomComplete;
		}
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x000A1051 File Offset: 0x0009F251
	protected virtual void OnRoomDataSaved(bool exitingToMainMenu)
	{
		if (!this.RoomSaveData.IsNativeNull())
		{
			this.SaveRoomMiscData();
			this.RoomSaveData.IsRoomComplete = this.IsRoomComplete;
		}
	}

	// Token: 0x06002F2E RID: 12078 RVA: 0x000A1077 File Offset: 0x0009F277
	protected virtual void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
	}

	// Token: 0x06002F2F RID: 12079 RVA: 0x000A1079 File Offset: 0x0009F279
	protected virtual void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
	}

	// Token: 0x0400258B RID: 9611
	private SpecialRoomCompletedEventArgs m_roomCompletedEventArgs;

	// Token: 0x0400258C RID: 9612
	private Relay m_roomCompletedRelay = new Relay();

	// Token: 0x0400258D RID: 9613
	private Relay m_roomInitializedRelay = new Relay();

	// Token: 0x0400258E RID: 9614
	private Action m_onRoomDataLoaded;

	// Token: 0x0400258F RID: 9615
	private Action<bool> m_onRoomDataSaved;

	// Token: 0x04002590 RID: 9616
	private Action<object, RoomViaDoorEventArgs> m_onPlayerExitRoom;

	// Token: 0x04002591 RID: 9617
	private Action<object, RoomViaDoorEventArgs> m_playerEnterRoomInitialization;

	// Token: 0x04002592 RID: 9618
	protected Dictionary<string, string> m_roomMiscDataTable;

	// Token: 0x04002593 RID: 9619
	[SerializeField]
	private SpecialRoomType m_specialRoomType;
}
