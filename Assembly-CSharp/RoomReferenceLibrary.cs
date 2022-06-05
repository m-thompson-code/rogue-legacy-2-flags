using System;
using Rooms;
using UnityEngine;

// Token: 0x02000245 RID: 581
[CreateAssetMenu(menuName = "Custom/Libraries/Room Reference Library")]
public class RoomReferenceLibrary : ScriptableObject
{
	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x06001740 RID: 5952 RVA: 0x000487B6 File Offset: 0x000469B6
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

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x06001741 RID: 5953 RVA: 0x000487DE File Offset: 0x000469DE
	// (set) Token: 0x06001742 RID: 5954 RVA: 0x000487EA File Offset: 0x000469EA
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

	// Token: 0x06001743 RID: 5955 RVA: 0x000487F8 File Offset: 0x000469F8
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

	// Token: 0x06001744 RID: 5956 RVA: 0x00048830 File Offset: 0x00046A30
	public static RoomMetaData GetRoomMetaData(RoomReferenceType roomReferenceType)
	{
		RoomReferenceEntry roomReferenceEntry = RoomReferenceLibrary.GetRoomReferenceEntry(roomReferenceType);
		if (!roomReferenceEntry.IsNativeNull())
		{
			return roomReferenceEntry.RoomMetaData;
		}
		return null;
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x00048854 File Offset: 0x00046A54
	public static string GetRoomMetaDataPath(RoomReferenceType roomReferenceType)
	{
		RoomReferenceEntry roomReferenceEntry = RoomReferenceLibrary.GetRoomReferenceEntry(roomReferenceType);
		if (!roomReferenceEntry.IsNativeNull())
		{
			return roomReferenceEntry.RoomMetaDataPath;
		}
		return null;
	}

	// Token: 0x040016C4 RID: 5828
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RoomReferenceLibrary";

	// Token: 0x040016C5 RID: 5829
	[SerializeField]
	private RoomReferenceEntry[] m_roomReferenceEntryArray;

	// Token: 0x040016C6 RID: 5830
	private static RoomReferenceLibrary m_instance;
}
