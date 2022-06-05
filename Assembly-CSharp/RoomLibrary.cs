using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000400 RID: 1024
public class RoomLibrary : MonoBehaviour
{
	// Token: 0x17000E79 RID: 3705
	// (get) Token: 0x060020DE RID: 8414 RVA: 0x00011709 File Offset: 0x0000F909
	// (set) Token: 0x060020DF RID: 8415 RVA: 0x00011710 File Offset: 0x0000F910
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

	// Token: 0x17000E7A RID: 3706
	// (get) Token: 0x060020E0 RID: 8416 RVA: 0x00011718 File Offset: 0x0000F918
	// (set) Token: 0x060020E1 RID: 8417 RVA: 0x0001171F File Offset: 0x0000F91F
	private static RoomLibrary Instance { get; set; }

	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x060020E2 RID: 8418 RVA: 0x00011727 File Offset: 0x0000F927
	public static bool IsInstantiated
	{
		get
		{
			return RoomLibrary.Instance != null;
		}
	}

	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00011734 File Offset: 0x0000F934
	// (set) Token: 0x060020E4 RID: 8420 RVA: 0x0001173B File Offset: 0x0000F93B
	public static bool IsLoaded { get; private set; }

	// Token: 0x17000E7D RID: 3709
	// (get) Token: 0x060020E5 RID: 8421 RVA: 0x00011743 File Offset: 0x0000F943
	// (set) Token: 0x060020E6 RID: 8422 RVA: 0x0001174A File Offset: 0x0000F94A
	public static Vector2Int MaxRoomSize { get; private set; }

	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x060020E7 RID: 8423 RVA: 0x00011752 File Offset: 0x0000F952
	// (set) Token: 0x060020E8 RID: 8424 RVA: 0x00011759 File Offset: 0x0000F959
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

	// Token: 0x060020E9 RID: 8425 RVA: 0x00011761 File Offset: 0x0000F961
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

	// Token: 0x060020EA RID: 8426 RVA: 0x000117A1 File Offset: 0x0000F9A1
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (!scene.name.StartsWith("Level"))
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			this.LoadCompiledRooms();
		}
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x000117CD File Offset: 0x0000F9CD
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

	// Token: 0x060020EC RID: 8428 RVA: 0x000A5FC4 File Offset: 0x000A41C4
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

	// Token: 0x060020ED RID: 8429 RVA: 0x00011809 File Offset: 0x0000FA09
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

	// Token: 0x060020EE RID: 8430 RVA: 0x000A6034 File Offset: 0x000A4234
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

	// Token: 0x060020EF RID: 8431 RVA: 0x0001182A File Offset: 0x0000FA2A
	public static RoomSetCollection GetSetCollection(BiomeType biome)
	{
		if (!RoomLibrary.IsLoaded)
		{
			RoomLibrary.Instance.LoadCompiledRooms();
		}
		return RoomLibrary.BiomeToSetCollectionTable[biome];
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x000A6228 File Offset: 0x000A4428
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

	// Token: 0x04001DBB RID: 7611
	[SerializeField]
	private string m_biomeRoomLibraryPath = "Scriptable Objects/RoomPool";

	// Token: 0x04001DBC RID: 7612
	private RoomPool m_biomeRoomLibrary;

	// Token: 0x04001DBD RID: 7613
	[SerializeField]
	private bool m_releaseRoomPoolAfterLoad = true;

	// Token: 0x04001DBE RID: 7614
	public const string COMPILED_ROOMS_SUB_DIRECTORY = "Prefabs/Compiled Rooms";

	// Token: 0x04001DBF RID: 7615
	public const string STORAGE_PATH = "Assets/Content/Prefabs/Compiled Rooms";

	// Token: 0x04001DC0 RID: 7616
	public const string PREFAB_RESOURCES_PATH = "Prefabs/Managers/RoomLibrary";

	// Token: 0x04001DC1 RID: 7617
	private static Dictionary<BiomeType, RoomSetCollection> m_biomeToSetCollectionTable = null;

	// Token: 0x04001DC2 RID: 7618
	private static Dictionary<string, CompiledScene_ScriptableObject> m_compiledSceneTable = new Dictionary<string, CompiledScene_ScriptableObject>();
}
