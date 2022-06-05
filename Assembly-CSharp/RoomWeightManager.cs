using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000681 RID: 1665
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Room Weight Manager")]
public class RoomWeightManager : ScriptableObject
{
	// Token: 0x170014F0 RID: 5360
	// (get) Token: 0x06003C18 RID: 15384 RVA: 0x000D0185 File Offset: 0x000CE385
	// (set) Token: 0x06003C19 RID: 15385 RVA: 0x000D018D File Offset: 0x000CE38D
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

	// Token: 0x170014F1 RID: 5361
	// (get) Token: 0x06003C1A RID: 15386 RVA: 0x000D0196 File Offset: 0x000CE396
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

	// Token: 0x06003C1B RID: 15387 RVA: 0x000D01C0 File Offset: 0x000CE3C0
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

	// Token: 0x04002D4E RID: 11598
	[SerializeField]
	private List<RoomWeightEntry> m_entries;

	// Token: 0x04002D4F RID: 11599
	[SerializeField]
	private bool m_isEnabled = true;

	// Token: 0x04002D50 RID: 11600
	[SerializeField]
	private bool m_treatMirroredAsDistinct = true;

	// Token: 0x04002D51 RID: 11601
	private static Dictionary<int, int> m_roomRNGWeightTable;

	// Token: 0x04002D52 RID: 11602
	private static RoomWeightManager m_instance;

	// Token: 0x04002D53 RID: 11603
	private const string RESOURCES_PATH = "Scriptable Objects/RoomWeightManager";
}
