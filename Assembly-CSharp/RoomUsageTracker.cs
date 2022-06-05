using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B97 RID: 2967
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Room Usage Tracker")]
public class RoomUsageTracker : ScriptableObject
{
	// Token: 0x17001DE0 RID: 7648
	// (get) Token: 0x0600595A RID: 22874 RVA: 0x00152AFC File Offset: 0x00150CFC
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

	// Token: 0x17001DE1 RID: 7649
	// (get) Token: 0x0600595B RID: 22875 RVA: 0x00030AE7 File Offset: 0x0002ECE7
	// (set) Token: 0x0600595C RID: 22876 RVA: 0x00030AEF File Offset: 0x0002ECEF
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

	// Token: 0x0600595D RID: 22877 RVA: 0x00030AF8 File Offset: 0x0002ECF8
	public void Reset()
	{
		if (RoomUsageTracker.Instance.Entries != null)
		{
			RoomUsageTracker.Instance.Entries.Clear();
		}
	}

	// Token: 0x0600595E RID: 22878 RVA: 0x00152B5C File Offset: 0x00150D5C
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

	// Token: 0x0400438A RID: 17290
	[SerializeField]
	private List<RoomUsageEntry> m_roomUsageTable;

	// Token: 0x0400438B RID: 17291
	private static RoomUsageTracker m_instance;

	// Token: 0x0400438C RID: 17292
	private static Dictionary<Room, RoomUsageEntry> m_entryTable;

	// Token: 0x0400438D RID: 17293
	private const string RESOURCES_PATH = "Scriptable Objects/RoomUsageTracker";

	// Token: 0x0400438E RID: 17294
	private const string EDITOR_PATH = "Assets/Content/Scriptable Objects/RoomUsageTracker.asset";
}
