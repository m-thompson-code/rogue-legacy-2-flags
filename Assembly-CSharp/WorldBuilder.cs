using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLWorldCreation;
using UnityEngine;

// Token: 0x02000B06 RID: 2822
public class WorldBuilder : MonoBehaviour
{
	// Token: 0x17001CBA RID: 7354
	// (get) Token: 0x06005496 RID: 21654 RVA: 0x0002DDC6 File Offset: 0x0002BFC6
	public List<BiomeType> BiomeBuildOrder
	{
		get
		{
			return this.m_biomeBuildOrder;
		}
	}

	// Token: 0x17001CBB RID: 7355
	// (get) Token: 0x06005497 RID: 21655 RVA: 0x0002DDCE File Offset: 0x0002BFCE
	public static Dictionary<BiomeType, BiomeController> BiomeControllers
	{
		get
		{
			return WorldBuilder.Instance.m_biomeControllers;
		}
	}

	// Token: 0x17001CBC RID: 7356
	// (get) Token: 0x06005498 RID: 21656 RVA: 0x0002DDDA File Offset: 0x0002BFDA
	public static bool DeactivateRoomGameObjects
	{
		get
		{
			return WorldBuilder.Instance.m_deactivateRoomGameObjects;
		}
	}

	// Token: 0x17001CBD RID: 7357
	// (get) Token: 0x06005499 RID: 21657 RVA: 0x0002DDE6 File Offset: 0x0002BFE6
	// (set) Token: 0x0600549A RID: 21658 RVA: 0x0002DDEE File Offset: 0x0002BFEE
	public BiomeLayer IncludedBiomes
	{
		get
		{
			return this.m_includedBiomes;
		}
		set
		{
			this.m_includedBiomes = value;
		}
	}

	// Token: 0x17001CBE RID: 7358
	// (get) Token: 0x0600549B RID: 21659 RVA: 0x0002DDF7 File Offset: 0x0002BFF7
	public BaseRoom StartingRoom
	{
		get
		{
			if (WorldBuilder.BiomeControllers != null && WorldBuilder.BiomeControllers.Count > 0)
			{
				return WorldBuilder.BiomeControllers[this.m_firstBiome].TransitionRoom;
			}
			return null;
		}
	}

	// Token: 0x17001CBF RID: 7359
	// (get) Token: 0x0600549C RID: 21660 RVA: 0x0002DE24 File Offset: 0x0002C024
	// (set) Token: 0x0600549D RID: 21661 RVA: 0x0002DE2B File Offset: 0x0002C02B
	public static WorldBuilder Instance { get; private set; }

	// Token: 0x17001CC0 RID: 7360
	// (get) Token: 0x0600549E RID: 21662 RVA: 0x0002DE33 File Offset: 0x0002C033
	public static bool IsInstantiated
	{
		get
		{
			return WorldBuilder.Instance != null;
		}
	}

	// Token: 0x17001CC1 RID: 7361
	// (get) Token: 0x0600549F RID: 21663 RVA: 0x0002DE40 File Offset: 0x0002C040
	// (set) Token: 0x060054A0 RID: 21664 RVA: 0x0002DE47 File Offset: 0x0002C047
	public static BiomeBuildStateID State { get; private set; }

	// Token: 0x17001CC2 RID: 7362
	// (get) Token: 0x060054A1 RID: 21665 RVA: 0x0002DE4F File Offset: 0x0002C04F
	public static bool CreateReports
	{
		get
		{
			return WorldBuilder.Instance.LevelManager.Mode == LevelManagerMode.Debug;
		}
	}

	// Token: 0x17001CC3 RID: 7363
	// (get) Token: 0x060054A2 RID: 21666 RVA: 0x0002DE63 File Offset: 0x0002C063
	// (set) Token: 0x060054A3 RID: 21667 RVA: 0x0002DE6A File Offset: 0x0002C06A
	public static BiomeType FirstBiomeOverride { get; set; }

	// Token: 0x17001CC4 RID: 7364
	// (get) Token: 0x060054A4 RID: 21668 RVA: 0x0002DE72 File Offset: 0x0002C072
	// (set) Token: 0x060054A5 RID: 21669 RVA: 0x0002DE7A File Offset: 0x0002C07A
	public LevelManager_RL LevelManager { get; private set; }

	// Token: 0x060054A6 RID: 21670 RVA: 0x0014012C File Offset: 0x0013E32C
	private void Awake()
	{
		if (WorldBuilder.Instance == null)
		{
			WorldBuilder.Instance = this;
			this.LevelManager = base.GetComponent<LevelManager_RL>();
			return;
		}
		Debug.LogFormat("<color=red>|{0}| Multiple WorldBuilder's found in Scene. WorldBuilder is a Singleton. Destroying additional WorldBuilder</color>", new object[]
		{
			this
		});
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060054A7 RID: 21671 RVA: 0x0002DE83 File Offset: 0x0002C083
	private IEnumerator CreateBiome(BiomeType biome)
	{
		BiomeController biomeController = WorldBuilder.CreateBiomeController(biome, base.transform);
		base.StartCoroutine(this.m_biomeCreator.CreateBiome(biomeController));
		while (this.m_biomeCreator.State != BiomeBuildStateID.Complete && this.m_biomeCreator.State != BiomeBuildStateID.Failed)
		{
			yield return null;
		}
		if (this.m_biomeCreator.State == BiomeBuildStateID.Complete)
		{
			WorldBuilder.BiomeControllers.Add(biome, biomeController);
		}
		else if (this.m_biomeCreator.State == BiomeBuildStateID.Failed)
		{
			this.OnBiomeCreatorFail();
		}
		yield break;
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x00140178 File Offset: 0x0013E378
	public static BiomeController CreateBiomeController(BiomeType biome, Transform parent)
	{
		BiomeController biomeController = new GameObject(biome.ToString() + "_BiomeController").AddComponent<BiomeController>();
		biomeController.Initialise(biome);
		biomeController.gameObject.transform.SetParent(parent);
		biomeController.gameObject.transform.position = Vector3.zero;
		return biomeController;
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x0002DE99 File Offset: 0x0002C099
	public static void CreateWorld()
	{
		WorldBuilder.State = BiomeBuildStateID.Running;
		if (WorldBuilder.Instance.m_printSeedsToConsole)
		{
			RNGManager.PrintSeedsToConsole();
		}
		WorldBuilder.Instance.StartCoroutine(WorldBuilder.Instance.CreateWorldCoroutine(null));
	}

	// Token: 0x060054AA RID: 21674 RVA: 0x0002DEC8 File Offset: 0x0002C0C8
	public static void CreateWorld(StageSaveData stageSaveData)
	{
		if (stageSaveData == null)
		{
			throw new ArgumentNullException("stageSaveData");
		}
		WorldBuilder.State = BiomeBuildStateID.Running;
		WorldBuilder.Instance.StartCoroutine(WorldBuilder.Instance.CreateWorldCoroutine(stageSaveData));
	}

	// Token: 0x060054AB RID: 21675 RVA: 0x0002DEF4 File Offset: 0x0002C0F4
	private IEnumerator CreateWorldCoroutine(StageSaveData stageSaveData = null)
	{
		while (!SharedWorldObjects_Loader.IsInitialized)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.transform.SetLocalPositionX(0f);
		playerController.transform.SetLocalPositionY(0f);
		playerController.ControllerCorgi.SetRaysParameters();
		playerController.ControllerCorgi.GravityActive(false);
		this.m_biomeControllers = new Dictionary<BiomeType, BiomeController>();
		this.m_firstBiome = WorldBuilder.FirstBiomeOverride;
		WorldBuilder.FirstBiomeOverride = BiomeType.None;
		if (stageSaveData != null)
		{
			using (Dictionary<BiomeType, List<RoomSaveData>>.Enumerator enumerator = stageSaveData.RoomSaveDataDict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<BiomeType, List<RoomSaveData>> keyValuePair2 = enumerator.Current;
					BiomeType key = keyValuePair2.Key;
					if (this.m_firstBiome == BiomeType.None)
					{
						this.m_firstBiome = key;
					}
					BiomeController biomeController = WorldBuilder.CreateBiomeController(key, base.transform);
					WorldBuilder.BiomeControllers.Add(key, biomeController);
					List<RoomSaveData> value = keyValuePair2.Value;
					this.m_biomeCreator.CreateBiomeFromSaveData(biomeController, value);
				}
				goto IL_2CA;
			}
		}
		int num = 0;
		using (List<BiomeType>.Enumerator enumerator2 = this.BiomeBuildOrder.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (BiomeUtility.IsBiomeInBiomeLayerMask(enumerator2.Current, this.IncludedBiomes))
				{
					num++;
				}
			}
		}
		if (num == 0)
		{
			throw new Exception(string.Format("|{0}| No Biomes in Included Biomes Field", this));
		}
		foreach (BiomeType biomeType in this.BiomeBuildOrder)
		{
			if (BiomeUtility.IsBiomeInBiomeLayerMask(biomeType, this.IncludedBiomes))
			{
				BiomeType connectsToBiome = BiomeUtility.GetConnectsToBiome(biomeType);
				if (this.m_firstBiome == BiomeType.None || connectsToBiome == BiomeType.None || WorldBuilder.BiomeControllers.ContainsKey(connectsToBiome))
				{
					if (this.m_firstBiome == BiomeType.None)
					{
						this.m_firstBiome = biomeType;
					}
					base.StartCoroutine(this.CreateBiome(biomeType));
					while (this.m_biomeCreator.State != BiomeBuildStateID.Complete && this.m_biomeCreator.State != BiomeBuildStateID.Failed)
					{
						yield return null;
					}
					if (this.m_biomeCreator.State == BiomeBuildStateID.Failed)
					{
						yield break;
					}
				}
				else
				{
					Debug.LogFormat("<color=red>|{0}| Can't build <b>{1}</b> Biome because the Biome that it connects to (<b>{2}</b>) has not been built.</color>", new object[]
					{
						this,
						biomeType,
						BiomeUtility.GetConnectsToBiome(biomeType)
					});
				}
			}
		}
		List<BiomeType>.Enumerator enumerator3 = default(List<BiomeType>.Enumerator);
		IL_2CA:
		if (stageSaveData == null)
		{
			foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
			{
				yield return WorldBuilder.Instance.RunBuildStage<GenerateRoomSeeds_BuildStage>(keyValuePair.Value);
				if (WorldBuilder.Instance.m_mergeRooms)
				{
					yield return WorldBuilder.Instance.RunBuildStage<CreateMergeRooms_BuildStage>(keyValuePair.Value);
				}
				if (this.m_biomeCreator.State == BiomeBuildStateID.Failed)
				{
					yield break;
				}
				keyValuePair = default(KeyValuePair<BiomeType, BiomeController>);
			}
			Dictionary<BiomeType, BiomeController>.Enumerator enumerator4 = default(Dictionary<BiomeType, BiomeController>.Enumerator);
		}
		if (this.LevelManager && this.LevelManager.Mode != LevelManagerMode.Debug)
		{
			foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair3 in WorldBuilder.BiomeControllers)
			{
				foreach (GridPointManager gridPointManager in keyValuePair3.Value.GridPointManager.GridPointManagers)
				{
					RoomContentMetaData content = gridPointManager.RoomMetaData.GetContent();
					if (content != null)
					{
						gridPointManager.Content = new GridPointManagerContentEntry[content.ContentEntries.Length];
						for (int i = 0; i < content.ContentEntries.Length; i++)
						{
							RoomContentEntry roomContentEntry = content.ContentEntries[i];
							bool isSpawned = roomContentEntry.GetIsSpawned(gridPointManager);
							Vector2 roomContentWorldPosition = this.GetRoomContentWorldPosition(gridPointManager, roomContentEntry.LocalPosition);
							gridPointManager.Content[i] = new GridPointManagerContentEntry(roomContentEntry.ContentType, roomContentEntry.LocalPosition, roomContentWorldPosition, isSpawned);
						}
					}
				}
			}
			if (MapController.IsInitialized)
			{
				MapController.Reset();
			}
			MapController.Initialize();
			if (ItemDropManager.IsInitialized)
			{
				ItemDropManager.DisableAllItemDrops();
			}
			if (EnemyManager.IsInitialized)
			{
				EnemyManager.DisableAllEnemies();
				EnemyManager.DisableAllSummonedEnemies();
			}
		}
		if (this.m_instantiateRooms)
		{
			yield return WorldBuilder.InstantiateBiome(this.m_firstBiome);
			BiomeTransitionController.UpdateObjectPools(BiomeType.None, this.m_firstBiome);
			this.DeactivateRooms(this.m_firstBiome);
		}
		WorldBuildCompleteEventArgs eventArgs = new WorldBuildCompleteEventArgs(RNGSeedManager.GetCurrentSeed(base.gameObject.scene.name), RNGManager.GetSeed(RngID.BiomeCreation), WorldBuilder.BiomeControllers.Values.ToList<BiomeController>(), true);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.WorldCreationComplete, this, eventArgs);
		WorldBuilder.State = BiomeBuildStateID.Complete;
		yield break;
		yield break;
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x001401D4 File Offset: 0x0013E3D4
	private Vector2 GetRoomContentWorldPosition(GridPointManager room, Vector2 roomContentLocalPosition)
	{
		Vector2 worldPositionFromRoomCoordinates = GridController.GetWorldPositionFromRoomCoordinates(GridController.GetRoomCoordinatesFromGridCoordinates(room.GridCoordinates), room.Size);
		Vector2 a = new Vector2(1f, 1f);
		if (room.IsRoomMirrored)
		{
			a = new Vector2(-1f, 1f);
		}
		return worldPositionFromRoomCoordinates + a * roomContentLocalPosition;
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x00140230 File Offset: 0x0013E430
	private void DeactivateRooms(BiomeType biome)
	{
		if (WorldBuilder.DeactivateRoomGameObjects)
		{
			foreach (BaseRoom baseRoom in WorldBuilder.BiomeControllers[biome].Rooms)
			{
				baseRoom.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x17001CC5 RID: 7365
	// (get) Token: 0x060054AE RID: 21678 RVA: 0x0002DF0A File Offset: 0x0002C10A
	// (set) Token: 0x060054AF RID: 21679 RVA: 0x0002DF11 File Offset: 0x0002C111
	public static BaseRoom OldCastleTransitionRoom { get; private set; }

	// Token: 0x060054B0 RID: 21680 RVA: 0x0002DF19 File Offset: 0x0002C119
	public static IEnumerator InstantiateBiome(BiomeType biome)
	{
		if (biome == BiomeType.Castle)
		{
			BiomeController biomeController = WorldBuilder.BiomeControllers[biome];
			List<BaseRoom> rooms = biomeController.Rooms;
			if (rooms != null && rooms.Count == 1 && rooms[0].RoomType == RoomType.Transition)
			{
				WorldBuilder.OldCastleTransitionRoom = rooms[0];
				biomeController.Rooms.Clear();
				biomeController.StandaloneRooms.Clear();
			}
		}
		MergeRoomTools.ResetMergeRoomCounter();
		yield return WorldBuilder.Instance.RunBuildStage<InstantiateRooms_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return new WaitForFixedUpdate();
		yield return WorldBuilder.Instance.RunBuildStage<CreateOneWays_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CloseDoors_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CreateTunnelRooms_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<RunIsSpawnedCheck_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CreateRoomBackgrounds_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<SetSpawnTypes_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		if (WorldBuilder.Instance.m_mergeRooms)
		{
			yield return WorldBuilder.Instance.RunBuildStage<CreateMergeRoomInstances_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		}
		yield return WorldBuilder.Instance.RunBuildStage<SpawnConditionalObjects_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<MergeTerrain_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CreateCameras_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CreateSky_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CreateWeather_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<CreateTransitionPoints_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		yield return WorldBuilder.Instance.RunBuildStage<MoveRoomsToRoot_BuildStage>(WorldBuilder.BiomeControllers[biome]);
		foreach (BaseRoom baseRoom in WorldBuilder.BiomeControllers[biome].Rooms)
		{
			baseRoom.gameObject.SetActive(false);
		}
		BiomeEventArgs eventArgs = new BiomeEventArgs(biome);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.BiomeCreationComplete, WorldBuilder.Instance, eventArgs);
		yield break;
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x0002DF28 File Offset: 0x0002C128
	public static BiomeController GetBiomeController(BiomeType biome)
	{
		if (WorldBuilder.BiomeControllers != null && WorldBuilder.BiomeControllers.ContainsKey(biome))
		{
			return WorldBuilder.BiomeControllers[biome];
		}
		return null;
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x0002DF4B File Offset: 0x0002C14B
	public void PreserveFirstBiomeOverride()
	{
		if (WorldBuilder.FirstBiomeOverride == BiomeType.None)
		{
			WorldBuilder.FirstBiomeOverride = this.m_firstBiome;
		}
	}

	// Token: 0x060054B3 RID: 21683 RVA: 0x00140298 File Offset: 0x0013E498
	private void OnBiomeCreatorFail()
	{
		WorldBuilder.State = BiomeBuildStateID.Failed;
		WorldBuildCompleteEventArgs eventArgs = new WorldBuildCompleteEventArgs(RNGSeedManager.GetCurrentSeed(base.gameObject.scene.name), RNGManager.GetSeed(RngID.BiomeCreation), WorldBuilder.BiomeControllers.Values.ToList<BiomeController>(), false);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.WorldCreationFailed, this, eventArgs);
		this.PreserveFirstBiomeOverride();
		this.Cleanup();
	}

	// Token: 0x060054B4 RID: 21684 RVA: 0x001402F8 File Offset: 0x0013E4F8
	private void Cleanup()
	{
		base.StopAllCoroutines();
		if (WorldBuilder.BiomeControllers != null)
		{
			foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
			{
				if (keyValuePair.Value.gameObject)
				{
					UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_biomeControllers = null;
		}
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x0002DF5F File Offset: 0x0002C15F
	private void OnDestroy()
	{
		WorldBuilder.Instance = null;
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x0002DF67 File Offset: 0x0002C167
	public static void Reset()
	{
		WorldBuilder.State = BiomeBuildStateID.None;
		WorldBuilder.Instance.Cleanup();
		if (WorldBuilder.Instance.m_biomeCreator != null)
		{
			WorldBuilder.Instance.m_biomeCreator.Reset();
		}
	}

	// Token: 0x060054B7 RID: 21687 RVA: 0x0002DF9A File Offset: 0x0002C19A
	private IEnumerator RunBuildStage<T>(BiomeController biomeController) where T : IBiomeBuildStage, new()
	{
		T t = Activator.CreateInstance<T>();
		yield return t.Run(biomeController);
		yield break;
	}

	// Token: 0x04003F08 RID: 16136
	private const float ROOM_CONTENT_POSITION_X_MULTIPLIER = 1f;

	// Token: 0x04003F09 RID: 16137
	private const float ROOM_CONTENT_POSITION_Y_MULTIPLIER = 1f;

	// Token: 0x04003F0A RID: 16138
	[SerializeField]
	[HideInInspector]
	private BiomeLayer m_includedBiomes = BiomeLayer.Castle;

	// Token: 0x04003F0B RID: 16139
	[SerializeField]
	private BiomeCreator m_biomeCreator;

	// Token: 0x04003F0C RID: 16140
	[SerializeField]
	private bool m_instantiateRooms = true;

	// Token: 0x04003F0D RID: 16141
	[SerializeField]
	private bool m_deactivateRoomGameObjects = true;

	// Token: 0x04003F0E RID: 16142
	[SerializeField]
	private bool m_printSeedsToConsole = true;

	// Token: 0x04003F0F RID: 16143
	[SerializeField]
	private bool m_mergeRooms = true;

	// Token: 0x04003F10 RID: 16144
	[SerializeField]
	[HideInInspector]
	private List<BiomeType> m_biomeBuildOrder;

	// Token: 0x04003F11 RID: 16145
	private BiomeType m_firstBiome;

	// Token: 0x04003F12 RID: 16146
	private Dictionary<BiomeType, BiomeController> m_biomeControllers;
}
