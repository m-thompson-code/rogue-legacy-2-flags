using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Room Weight Manager")]
public class RoomWeightManager : ScriptableObject
{
	// Token: 0x17001CAE RID: 7342
	// (get) Token: 0x06005477 RID: 21623 RVA: 0x0002DC3C File Offset: 0x0002BE3C
	// (set) Token: 0x06005478 RID: 21624 RVA: 0x0002DC44 File Offset: 0x0002BE44
	public List<RoomWeightEntry> Entries
	{
		get
		{
			return this.m_entries;
		}
		private set
		{
			this.m_entries = value;
		}
	}

	// Token: 0x17001CAF RID: 7343
	// (get) Token: 0x06005479 RID: 21625 RVA: 0x0002DC4D File Offset: 0x0002BE4D
	private static RoomWeightManager Instance
	{
		get
		{
			if (RoomWeightManager.m_instance == null)
			{
				RoomWeightManager.m_instance = CDGResources.Load<RoomWeightManager>("Scriptable Objects/RoomWeightManager", "", true);
			}
			return RoomWeightManager.m_instance;
		}
	}

	// Token: 0x0600547A RID: 21626 RVA: 0x00140010 File Offset: 0x0013E210
	public static int GetWeight(RoomID roomID, bool isMirrored)
	{
		if (!RoomWeightManager.Instance.m_treatMirroredAsDistinct)
		{
			isMirrored = false;
		}
		if (RoomWeightManager.m_roomRNGWeightTable == null)
		{
			RoomWeightManager.m_roomRNGWeightTable = new Dictionary<int, int>();
			foreach (RoomWeightEntry roomWeightEntry in RoomWeightManager.Instance.Entries)
			{
				int hashedRoomID = RoomUtility.GetHashedRoomID(roomWeightEntry.RoomID, roomWeightEntry.IsMirrored);
				if (!RoomWeightManager.m_roomRNGWeightTable.ContainsKey(hashedRoomID))
				{
					int rngweight = roomWeightEntry.RNGWeight;
					RoomWeightManager.m_roomRNGWeightTable.Add(hashedRoomID, rngweight);
				}
			}
		}
		int hashedRoomID2 = RoomUtility.GetHashedRoomID(roomID, isMirrored);
		int result;
		if (RoomWeightManager.m_roomRNGWeightTable.TryGetValue(hashedRoomID2, out result))
		{
			return result;
		}
		return BiomeCreation_EV.DEFAULT_ROOM_RNG_WEIGHT;
	}

	// Token: 0x04003EF7 RID: 16119
	[SerializeField]
	private List<RoomWeightEntry> m_entries;

	// Token: 0x04003EF8 RID: 16120
	[SerializeField]
	private bool m_isEnabled = true;

	// Token: 0x04003EF9 RID: 16121
	[SerializeField]
	private bool m_treatMirroredAsDistinct = true;

	// Token: 0x04003EFA RID: 16122
	private static Dictionary<int, int> m_roomRNGWeightTable;

	// Token: 0x04003EFB RID: 16123
	private static RoomWeightManager m_instance;

	// Token: 0x04003EFC RID: 16124
	private const string RESOURCES_PATH = "Scriptable Objects/RoomWeightManager";
}
