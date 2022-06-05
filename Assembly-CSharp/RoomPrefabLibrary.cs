using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000635 RID: 1589
[CreateAssetMenu(menuName = "Custom/Room Prefab Library")]
public class RoomPrefabLibrary : ScriptableObject
{
	// Token: 0x17001442 RID: 5186
	// (get) Token: 0x06003974 RID: 14708 RVA: 0x000C4114 File Offset: 0x000C2314
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

	// Token: 0x17001443 RID: 5187
	// (get) Token: 0x06003975 RID: 14709 RVA: 0x000C413D File Offset: 0x000C233D
	public static Ferr2DT_PathTerrain BottomDoorOneWayPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_bottomDoorOneWayPrefab;
		}
	}

	// Token: 0x17001444 RID: 5188
	// (get) Token: 0x06003976 RID: 14710 RVA: 0x000C4149 File Offset: 0x000C2349
	public static Ferr2DT_PathTerrain DoorSealPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_doorSealPrefab;
		}
	}

	// Token: 0x17001445 RID: 5189
	// (get) Token: 0x06003977 RID: 14711 RVA: 0x000C4155 File Offset: 0x000C2355
	public static Ferr2DT_PathTerrain DecoTerrainPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_decoTerrainPrefab;
		}
	}

	// Token: 0x17001446 RID: 5190
	// (get) Token: 0x06003978 RID: 14712 RVA: 0x000C4161 File Offset: 0x000C2361
	public static CinemachineVirtualCamera CinemachineVirtualCameraPrefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_cinemachineVirtualCameraPrefab;
		}
	}

	// Token: 0x17001447 RID: 5191
	// (get) Token: 0x06003979 RID: 14713 RVA: 0x000C416D File Offset: 0x000C236D
	public static Room_DebugUI Room_DebugUI_Prefab
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_room_DebugUI_Prefab;
		}
	}

	// Token: 0x17001448 RID: 5192
	// (get) Token: 0x0600397A RID: 14714 RVA: 0x000C4179 File Offset: 0x000C2379
	public static Ferr2DT_Material DefaultDontMergeFerr2DMaterial
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_defaultDontMergeFerr2DMaterial;
		}
	}

	// Token: 0x17001449 RID: 5193
	// (get) Token: 0x0600397B RID: 14715 RVA: 0x000C4185 File Offset: 0x000C2385
	public static Ferr2DT_Material StudyDontMergeFerr2DMaterial
	{
		get
		{
			return RoomPrefabLibrary.Instance.m_studyDontMergeFerr2DMaterial;
		}
	}

	// Token: 0x04002C45 RID: 11333
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RoomPrefabLibrary";

	// Token: 0x04002C46 RID: 11334
	[SerializeField]
	private Ferr2DT_PathTerrain m_bottomDoorOneWayPrefab;

	// Token: 0x04002C47 RID: 11335
	[SerializeField]
	private Ferr2DT_PathTerrain m_doorSealPrefab;

	// Token: 0x04002C48 RID: 11336
	[SerializeField]
	private Ferr2DT_PathTerrain m_decoTerrainPrefab;

	// Token: 0x04002C49 RID: 11337
	[SerializeField]
	private CinemachineVirtualCamera m_cinemachineVirtualCameraPrefab;

	// Token: 0x04002C4A RID: 11338
	[SerializeField]
	private Room_DebugUI m_room_DebugUI_Prefab;

	// Token: 0x04002C4B RID: 11339
	[SerializeField]
	private Ferr2DT_Material m_defaultDontMergeFerr2DMaterial;

	// Token: 0x04002C4C RID: 11340
	[SerializeField]
	private Ferr2DT_Material m_studyDontMergeFerr2DMaterial;

	// Token: 0x04002C4D RID: 11341
	private static RoomPrefabLibrary m_instance;
}
