using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006E9 RID: 1769
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Room Usage Tracker")]
public class RoomUsageTracker : ScriptableObject
{
	// Token: 0x170015E8 RID: 5608
	// (get) Token: 0x0600401D RID: 16413 RVA: 0x000E3228 File Offset: 0x000E1428
	private static RoomUsageTracker Instance
	{
		get
		{
			if (RoomUsageTracker.m_instance == null)
			{
				if (Application.isPlaying)
				{
					RoomUsageTracker.m_instance = CDGResources.Load<RoomUsageTracker>("Scriptable Objects/RoomUsageTracker", "", true);
				}
				if (RoomUsageTracker.m_instance == null)
				{
					Debug.LogFormat("<color=red>| CameraLayerUtility | Failed to find Room Usage Tracker SO at Path ({0})</color>", new object[]
					{
						"Scriptable Objects/RoomUsageTracker"
					});
				}
			}
			return RoomUsageTracker.m_instance;
		}
	}

	// Token: 0x170015E9 RID: 5609
	// (get) Token: 0x0600401E RID: 16414 RVA: 0x000E3288 File Offset: 0x000E1488
	// (set) Token: 0x0600401F RID: 16415 RVA: 0x000E3290 File Offset: 0x000E1490
	public List<RoomUsageEntry> Entries
	{
		get
		{
			return this.m_roomUsageTable;
		}
		set
		{
			this.m_roomUsageTable = value;
		}
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x000E3299 File Offset: 0x000E1499
	public void Reset()
	{
		if (RoomUsageTracker.Instance.Entries != null)
		{
			RoomUsageTracker.Instance.Entries.Clear();
		}
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x000E32B8 File Offset: 0x000E14B8
	public static void TrackRoomPrefab(Room roomPrefab)
	{
		if (RoomUsageTracker.m_entryTable == null)
		{
			RoomUsageTracker.m_entryTable = new Dictionary<Room, RoomUsageEntry>();
			for (int i = 0; i < RoomUsageTracker.Instance.Entries.Count; i++)
			{
				RoomUsageTracker.m_entryTable.Add(RoomUsageTracker.Instance.Entries[i].RoomPrefab, RoomUsageTracker.Instance.Entries[i]);
			}
		}
		if (!RoomUsageTracker.m_entryTable.ContainsKey(roomPrefab))
		{
			RoomUsageEntry roomUsageEntry = new RoomUsageEntry(roomPrefab);
			RoomUsageTracker.Instance.Entries.Add(roomUsageEntry);
			RoomUsageTracker.m_entryTable.Add(roomPrefab, roomUsageEntry);
		}
		RoomUsageTracker.m_entryTable[roomPrefab].Count++;
	}

	// Token: 0x04003138 RID: 12600
	[SerializeField]
	private List<RoomUsageEntry> m_roomUsageTable;

	// Token: 0x04003139 RID: 12601
	private static RoomUsageTracker m_instance;

	// Token: 0x0400313A RID: 12602
	private static Dictionary<Room, RoomUsageEntry> m_entryTable;

	// Token: 0x0400313B RID: 12603
	private const string RESOURCES_PATH = "Scriptable Objects/RoomUsageTracker";

	// Token: 0x0400313C RID: 12604
	private const string EDITOR_PATH = "Assets/Content/Scriptable Objects/RoomUsageTracker.asset";
}
