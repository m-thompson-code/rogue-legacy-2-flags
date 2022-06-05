using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200083F RID: 2111
public class BaseSpecialRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001772 RID: 6002
	// (get) Token: 0x0600411B RID: 16667 RVA: 0x00023F64 File Offset: 0x00022164
	public IRelayLink RoomCompletedRelay
	{
		get
		{
			return this.m_roomCompletedRelay.link;
		}
	}

	// Token: 0x17001773 RID: 6003
	// (get) Token: 0x0600411C RID: 16668 RVA: 0x00023F71 File Offset: 0x00022171
	public IRelayLink SpecialRoomInitializedRelay
	{
		get
		{
			return this.m_roomInitializedRelay.link;
		}
	}

	// Token: 0x17001774 RID: 6004
	// (get) Token: 0x0600411D RID: 16669 RVA: 0x00023F7E File Offset: 0x0002217E
	public virtual SpecialRoomType SpecialRoomType
	{
		get
		{
			return this.m_specialRoomType;
		}
	}

	// Token: 0x17001775 RID: 6005
	// (get) Token: 0x0600411E RID: 16670 RVA: 0x00023F86 File Offset: 0x00022186
	// (set) Token: 0x0600411F RID: 16671 RVA: 0x00023F8E File Offset: 0x0002218E
	public BaseRoom Room { get; private set; }

	// Token: 0x17001776 RID: 6006
	// (get) Token: 0x06004120 RID: 16672 RVA: 0x00023F97 File Offset: 0x00022197
	// (set) Token: 0x06004121 RID: 16673 RVA: 0x00023F9F File Offset: 0x0002219F
	public bool IsRoomComplete { get; protected set; }

	// Token: 0x17001777 RID: 6007
	// (get) Token: 0x06004122 RID: 16674 RVA: 0x00023FA8 File Offset: 0x000221A8
	// (set) Token: 0x06004123 RID: 16675 RVA: 0x00023FB0 File Offset: 0x000221B0
	public RoomSaveData RoomSaveData { get; private set; }

	// Token: 0x06004124 RID: 16676 RVA: 0x00105AFC File Offset: 0x00103CFC
	protected virtual void Awake()
	{
		this.m_roomCompletedEventArgs = new SpecialRoomCompletedEventArgs(this);
		this.m_onRoomDataLoaded = new Action(this.OnRoomDataLoaded);
		this.m_onRoomDataSaved = new Action<bool>(this.OnRoomDataSaved);
		this.m_onPlayerExitRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom);
		this.m_playerEnterRoomInitialization = new Action<object, RoomViaDoorEventArgs>(this.PlayerEnterRoomInitialization);
	}

	// Token: 0x06004125 RID: 16677 RVA: 0x00105B60 File Offset: 0x00103D60
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

	// Token: 0x06004126 RID: 16678 RVA: 0x00105BE8 File Offset: 0x00103DE8
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

	// Token: 0x06004127 RID: 16679 RVA: 0x00105CDC File Offset: 0x00103EDC
	public string GetRoomMiscData(string id)
	{
		string result;
		if (this.m_roomMiscDataTable != null && this.m_roomMiscDataTable.TryGetValue(id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004128 RID: 16680 RVA: 0x00023FB9 File Offset: 0x000221B9
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

	// Token: 0x06004129 RID: 16681 RVA: 0x00023FF7 File Offset: 0x000221F7
	public virtual void RoomCompleted()
	{
		if (this.Room != null)
		{
			this.IsRoomComplete = true;
			this.m_roomCompletedRelay.Dispatch();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SpecialRoomCompleted, this, this.m_roomCompletedEventArgs);
		}
	}

	// Token: 0x0600412A RID: 16682 RVA: 0x00024027 File Offset: 0x00022227
	protected void ForceRoomCompleted()
	{
		if (this.Room != null)
		{
			this.IsRoomComplete = true;
		}
	}

	// Token: 0x0600412B RID: 16683 RVA: 0x00105D04 File Offset: 0x00103F04
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

	// Token: 0x0600412C RID: 16684 RVA: 0x00105DC0 File Offset: 0x00103FC0
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

	// Token: 0x0600412D RID: 16685 RVA: 0x0002403E File Offset: 0x0002223E
	private void PlayerEnterRoomInitialization(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.OnPlayerEnterRoom(sender, eventArgs);
		this.m_roomInitializedRelay.Dispatch();
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x00105E40 File Offset: 0x00104040
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

	// Token: 0x0600412F RID: 16687 RVA: 0x00024053 File Offset: 0x00022253
	protected virtual void OnRoomDataSaved(bool exitingToMainMenu)
	{
		if (!this.RoomSaveData.IsNativeNull())
		{
			this.SaveRoomMiscData();
			this.RoomSaveData.IsRoomComplete = this.IsRoomComplete;
		}
	}

	// Token: 0x06004130 RID: 16688 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
	}

	// Token: 0x06004131 RID: 16689 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
	}

	// Token: 0x040032F5 RID: 13045
	private SpecialRoomCompletedEventArgs m_roomCompletedEventArgs;

	// Token: 0x040032F6 RID: 13046
	private Relay m_roomCompletedRelay = new Relay();

	// Token: 0x040032F7 RID: 13047
	private Relay m_roomInitializedRelay = new Relay();

	// Token: 0x040032F8 RID: 13048
	private Action m_onRoomDataLoaded;

	// Token: 0x040032F9 RID: 13049
	private Action<bool> m_onRoomDataSaved;

	// Token: 0x040032FA RID: 13050
	private Action<object, RoomViaDoorEventArgs> m_onPlayerExitRoom;

	// Token: 0x040032FB RID: 13051
	private Action<object, RoomViaDoorEventArgs> m_playerEnterRoomInitialization;

	// Token: 0x040032FC RID: 13052
	protected Dictionary<string, string> m_roomMiscDataTable;

	// Token: 0x040032FD RID: 13053
	[SerializeField]
	private SpecialRoomType m_specialRoomType;
}
