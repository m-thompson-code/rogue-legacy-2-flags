using System;
using Rooms;
using UnityEngine;

// Token: 0x02000402 RID: 1026
[CreateAssetMenu(menuName = "Custom/Libraries/Room Reference Library")]
public class RoomReferenceLibrary : ScriptableObject
{
	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x060020F3 RID: 8435 RVA: 0x00011874 File Offset: 0x0000FA74
	public static RoomReferenceLibrary Instance
	{
		get
		{
			if (!RoomReferenceLibrary.m_instance)
			{
				RoomReferenceLibrary.m_instance = CDGResources.Load<RoomReferenceLibrary>("Scriptable Objects/Libraries/RoomReferenceLibrary", "", true);
			}
			return RoomReferenceLibrary.m_instance;
		}
	}

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x060020F4 RID: 8436 RVA: 0x0001189C File Offset: 0x0000FA9C
	// (set) Token: 0x060020F5 RID: 8437 RVA: 0x000118A8 File Offset: 0x0000FAA8
	public static RoomReferenceEntry[] RoomReferenceArray
	{
		get
		{
			return RoomReferenceLibrary.Instance.m_roomReferenceEntryArray;
		}
		set
		{
			RoomReferenceLibrary.Instance.m_roomReferenceEntryArray = value;
		}
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000A62EC File Offset: 0x000A44EC
	public static RoomReferenceEntry GetRoomReferenceEntry(RoomReferenceType roomReferenceType)
	{
		foreach (RoomReferenceEntry roomReferenceEntry in RoomReferenceLibrary.Instance.m_roomReferenceEntryArray)
		{
			if (roomReferenceEntry.RoomReferenceType == roomReferenceType)
			{
				return roomReferenceEntry;
			}
		}
		return null;
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000A6324 File Offset: 0x000A4524
	public static RoomMetaData GetRoomMetaData(RoomReferenceType roomReferenceType)
	{
		RoomReferenceEntry roomReferenceEntry = RoomReferenceLibrary.GetRoomReferenceEntry(roomReferenceType);
		if (!roomReferenceEntry.IsNativeNull())
		{
			return roomReferenceEntry.RoomMetaData;
		}
		return null;
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x000A6348 File Offset: 0x000A4548
	public static string GetRoomMetaDataPath(RoomReferenceType roomReferenceType)
	{
		RoomReferenceEntry roomReferenceEntry = RoomReferenceLibrary.GetRoomReferenceEntry(roomReferenceType);
		if (!roomReferenceEntry.IsNativeNull())
		{
			return roomReferenceEntry.RoomMetaDataPath;
		}
		return null;
	}

	// Token: 0x04001DDC RID: 7644
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RoomReferenceLibrary";

	// Token: 0x04001DDD RID: 7645
	[SerializeField]
	private RoomReferenceEntry[] m_roomReferenceEntryArray;

	// Token: 0x04001DDE RID: 7646
	private static RoomReferenceLibrary m_instance;
}
