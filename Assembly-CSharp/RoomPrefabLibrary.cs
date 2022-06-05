using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000A62 RID: 2658
[CreateAssetMenu(menuName = "Custom/Room Prefab Library")]
public class RoomPrefabLibrary : ScriptableObject
{
	// Token: 0x17001BA9 RID: 7081
	// (get) Token: 0x06005053 RID: 20563 RVA: 0x0002BD54 File Offset: 0x00029F54
	private static RoomPrefabLibrary Instance
	{
		get
		{
			if (RoomPrefabLibrary.m_instance == null)
			{
				RoomPrefabLibrary.m_instance = CDGResources.Load<RoomPrefabLibrary>("Scriptable Objects/Libraries/RoomPrefabLibrary", "", true);
			}
			return RoomPrefabLibrary.m_instance;
		}
	}

	// Token: 0x17001BAA RID: 7082
	// (get) Token: 0x06005054 RID: 20564 RVA: 0x0002BD7D File Offset: 0x00029F7D
	public static Ferr2DT_PathTerrain BottomDoorOneWayPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_bottomDoorOneWayPrefab;
		}
	}

	// Token: 0x17001BAB RID: 7083
	// (get) Token: 0x06005055 RID: 20565 RVA: 0x0002BD89 File Offset: 0x00029F89
	public static Ferr2DT_PathTerrain DoorSealPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_doorSealPrefab;
		}
	}

	// Token: 0x17001BAC RID: 7084
	// (get) Token: 0x06005056 RID: 20566 RVA: 0x0002BD95 File Offset: 0x00029F95
	public static Ferr2DT_PathTerrain DecoTerrainPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_decoTerrainPrefab;
		}
	}

	// Token: 0x17001BAD RID: 7085
	// (get) Token: 0x06005057 RID: 20567 RVA: 0x0002BDA1 File Offset: 0x00029FA1
	public static CinemachineVirtualCamera CinemachineVirtualCameraPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_cinemachineVirtualCameraPrefab;
		}
	}

	// Token: 0x17001BAE RID: 7086
	// (get) Token: 0x06005058 RID: 20568 RVA: 0x0002BDAD File Offset: 0x00029FAD
	public static Room_DebugUI Room_DebugUI_Prefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_room_DebugUI_Prefab;
		}
	}

	// Token: 0x17001BAF RID: 7087
	// (get) Token: 0x06005059 RID: 20569 RVA: 0x0002BDB9 File Offset: 0x00029FB9
	public static Ferr2DT_Material DefaultDontMergeFerr2DMaterial
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_defaultDontMergeFerr2DMaterial;
		}
	}

	// Token: 0x17001BB0 RID: 7088
	// (get) Token: 0x0600505A RID: 20570 RVA: 0x0002BDC5 File Offset: 0x00029FC5
	public static Ferr2DT_Material StudyDontMergeFerr2DMaterial
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_studyDontMergeFerr2DMaterial;
		}
	}

	// Token: 0x04003CD7 RID: 15575
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RoomPrefabLibrary";

	// Token: 0x04003CD8 RID: 15576
	[SerializeField]
	private Ferr2DT_PathTerrain m_bottomDoorOneWayPrefab;

	// Token: 0x04003CD9 RID: 15577
	[SerializeField]
	private Ferr2DT_PathTerrain m_doorSealPrefab;

	// Token: 0x04003CDA RID: 15578
	[SerializeField]
	private Ferr2DT_PathTerrain m_decoTerrainPrefab;

	// Token: 0x04003CDB RID: 15579
	[SerializeField]
	private CinemachineVirtualCamera m_cinemachineVirtualCameraPrefab;

	// Token: 0x04003CDC RID: 15580
	[SerializeField]
	private Room_DebugUI m_room_DebugUI_Prefab;

	// Token: 0x04003CDD RID: 15581
	[SerializeField]
	private Ferr2DT_Material m_defaultDontMergeFerr2DMaterial;

	// Token: 0x04003CDE RID: 15582
	[SerializeField]
	private Ferr2DT_Material m_studyDontMergeFerr2DMaterial;

	// Token: 0x04003CDF RID: 15583
	private static RoomPrefabLibrary m_instance;
}
