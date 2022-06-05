using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000243 RID: 579
public class RoomLibrary : MonoBehaviour
{
	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x0600172B RID: 5931 RVA: 0x00048322 File Offset: 0x00046522
	// (set) Token: 0x0600172C RID: 5932 RVA: 0x00048329 File Offset: 0x00046529
	public static Dictionary<BiomeType, RoomSetCollection> BiomeToSetCollectionTable
	{
		get
		{
			return RoomLibrary.m_biomeToSetCollectionTable;
		}
		private set
		{
			RoomLibrary.m_biomeToSetCollectionTable = value;
		}
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x0600172D RID: 5933 RVA: 0x00048331 File Offset: 0x00046531
	// (set) Token: 0x0600172E RID: 5934 RVA: 0x00048338 File Offset: 0x00046538
	private static RoomLibrary Instance { get; set; }

	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x0600172F RID: 5935 RVA: 0x00048340 File Offset: 0x00046540
	public static bool IsInstantiated
	{
		get
		{
			return RoomLibrary.Instance != null;
		}
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x06001730 RID: 5936 RVA: 0x0004834D File Offset: 0x0004654D
	// (set) Token: 0x06001731 RID: 5937 RVA: 0x00048354 File Offset: 0x00046554
	public static bool IsLoaded { get; private set; }

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x06001732 RID: 5938 RVA: 0x0004835C File Offset: 0x0004655C
	// (set) Token: 0x06001733 RID: 5939 RVA: 0x00048363 File Offset: 0x00046563
	public static Vector2Int MaxRoomSize { get; private set; }

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06001734 RID: 5940 RVA: 0x0004836B File Offset: 0x0004656B
	// (set) Token: 0x06001735 RID: 5941 RVA: 0x00048372 File Offset: 0x00046572
	public static Dictionary<string, CompiledScene_ScriptableObject> CompiledSceneTable
	{
		get
		{
			return RoomLibrary.m_compiledSceneTable;
		}
		private set
		{
			RoomLibrary.m_compiledSceneTable = value;
		}
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x0004837A File Offset: 0x0004657A
	private void Awake()
	{
		if (!(RoomLibrary.Instance == null))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		RoomLibrary.Instance = this;
		if (GameUtility.IsInLevelEditor)
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			return;
		}
		this.LoadCompiledRooms();
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x000483BA File Offset: 0x000465BA
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (!scene.name.StartsWith("Level"))
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			this.LoadCompiledRooms();
		}
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x000483E6 File Offset: 0x000465E6
	private void OnDestroy()
	{
		if (RoomLibrary.Instance != null)
		{
			RoomLibrary.MaxRoomSize = Vector2Int.zero;
			RoomLibrary.IsLoaded = false;
			if (RoomLibrary.BiomeToSetCollectionTable != null)
			{
				RoomLibrary.BiomeToSetCollectionTable.Clear();
				RoomLibrary.BiomeToSetCollectionTable = null;
			}
			RoomLibrary.Instance = null;
		}
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x00048424 File Offset: 0x00046624
	private void LoadCompiledRooms()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.m_biomeRoomLibrary == null)
		{
			this.m_biomeRoomLibrary = CDGResources.Load<RoomPool>(this.m_biomeRoomLibraryPath, "", true);
		}
		foreach (BiomeType biomeType in BiomeType_RL.TypeArray)
		{
			if (BiomeType_RL.IsValidBiome(biomeType))
			{
				this.LoadCompiledRooms(biomeType, this.m_biomeRoomLibrary);
			}
		}
		RoomLibrary.IsLoaded = true;
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x00048491 File Offset: 0x00046691
	public static void LoadCompiledRoomsFromLevelEditor()
	{
		if (!GameUtility.IsInLevelEditor)
		{
			return;
		}
		if (RoomLibrary.Instance != null)
		{
			RoomLibrary.Instance.LoadCompiledRooms();
		}
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x000484B4 File Offset: 0x000466B4
	private void LoadCompiledRooms(BiomeType biome, RoomPool roomPool)
	{
		if (RoomLibrary.BiomeToSetCollectionTable == null)
		{
			RoomLibrary.BiomeToSetCollectionTable = new Dictionary<BiomeType, RoomSetCollection>();
		}
		RoomLibrary.BiomeToSetCollectionTable.Add(biome, new RoomSetCollection(biome, roomPool));
		foreach (object obj in Enum.GetValues(typeof(RoomType)))
		{
			RoomType roomType = (RoomType)obj;
			CompiledScene_ScriptableObject[] compiledScenes = roomPool.GetCompiledScenes(biome, roomType);
			int i = 0;
			while (i < compiledScenes.Length)
			{
				CompiledScene_ScriptableObject compiledScene_ScriptableObject = compiledScenes[i];
				if (!RoomLibrary.CompiledSceneTable.ContainsKey(compiledScene_ScriptableObject.SceneID))
				{
					RoomLibrary.CompiledSceneTable.Add(compiledScene_ScriptableObject.SceneID, compiledScene_ScriptableObject);
				}
				if (compiledScene_ScriptableObject.RoomMetaData != null)
				{
					using (List<RoomMetaData>.Enumerator enumerator2 = compiledScene_ScriptableObject.RoomMetaData.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							RoomMetaData roomMetaData = enumerator2.Current;
							if (!(roomMetaData == null))
							{
								if (roomMetaData.Size.x > RoomLibrary.MaxRoomSize.x)
								{
									RoomLibrary.MaxRoomSize = new Vector2Int(roomMetaData.Size.x, RoomLibrary.MaxRoomSize.y);
								}
								if (roomMetaData.Size.y > RoomLibrary.MaxRoomSize.y)
								{
									RoomLibrary.MaxRoomSize = new Vector2Int(RoomLibrary.MaxRoomSize.x, roomMetaData.Size.y);
								}
							}
						}
						goto IL_183;
					}
					goto IL_165;
				}
				goto IL_165;
				IL_183:
				i++;
				continue;
				IL_165:
				Debug.LogFormat("<color=red>[{0}] Room GameObject List is null on Compiled Scene SO ({1})</color>", new object[]
				{
					this,
					compiledScene_ScriptableObject.name
				});
				goto IL_183;
			}
		}
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x000486A8 File Offset: 0x000468A8
	public static RoomSetCollection GetSetCollection(BiomeType biome)
	{
		if (!RoomLibrary.IsLoaded)
		{
			RoomLibrary.Instance.LoadCompiledRooms();
		}
		return RoomLibrary.BiomeToSetCollectionTable[biome];
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x000486C8 File Offset: 0x000468C8
	public static RoomMetaData GetRoomMetaDataFromID(RoomID roomID)
	{
		string sceneName = roomID.SceneName;
		if (!RoomLibrary.m_compiledSceneTable.ContainsKey(sceneName))
		{
			CompiledScene_ScriptableObject value = CDGResources.Load<CompiledScene_ScriptableObject>("Prefabs/Compiled Rooms/" + sceneName + "_Compiled", "rooms", true);
			RoomLibrary.m_compiledSceneTable.Add(sceneName, value);
		}
		List<RoomMetaData> roomMetaData = RoomLibrary.m_compiledSceneTable[sceneName].RoomMetaData;
		RoomMetaData roomMetaData2 = null;
		for (int i = 0; i < roomMetaData.Count; i++)
		{
			if (roomMetaData[i].ID == roomID)
			{
				roomMetaData2 = roomMetaData[i];
				break;
			}
		}
		if (roomMetaData2 == null)
		{
			throw new InvalidOperationException(string.Format("| RoomLibrary | The Compiled Scene ({0}) does not contain a RoomMetaData object with ID ({1})", RoomLibrary.m_compiledSceneTable[sceneName].name, roomID));
		}
		return roomMetaData2;
	}

	// Token: 0x040016A3 RID: 5795
	[SerializeField]
	private string m_biomeRoomLibraryPath = "Scriptable Objects/RoomPool";

	// Token: 0x040016A4 RID: 5796
	private RoomPool m_biomeRoomLibrary;

	// Token: 0x040016A5 RID: 5797
	[SerializeField]
	private bool m_releaseRoomPoolAfterLoad = true;

	// Token: 0x040016A6 RID: 5798
	public const string COMPILED_ROOMS_SUB_DIRECTORY = "Prefabs/Compiled Rooms";

	// Token: 0x040016A7 RID: 5799
	public const string STORAGE_PATH = "Assets/Content/Prefabs/Compiled Rooms";

	// Token: 0x040016A8 RID: 5800
	public const string PREFAB_RESOURCES_PATH = "Prefabs/Managers/RoomLibrary";

	// Token: 0x040016A9 RID: 5801
	private static Dictionary<BiomeType, RoomSetCollection> m_biomeToSetCollectionTable = null;

	// Token: 0x040016AA RID: 5802
	private static Dictionary<string, CompiledScene_ScriptableObject> m_compiledSceneTable = new Dictionary<string, CompiledScene_ScriptableObject>();
}
