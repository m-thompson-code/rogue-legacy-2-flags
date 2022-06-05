using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLWorldCreation;
using UnityEngine;

// Token: 0x02000686 RID: 1670
public class WorldBuilder : MonoBehaviour
{
	// Token: 0x170014FC RID: 5372
	// (get) Token: 0x06003C37 RID: 15415 RVA: 0x000D0435 File Offset: 0x000CE635
	public List<BiomeType> BiomeBuildOrder
	{
		get
		{
			return this.m_biomeBuildOrder;
		}
	}

	// Token: 0x170014FD RID: 5373
	// (get) Token: 0x06003C38 RID: 15416 RVA: 0x000D043D File Offset: 0x000CE63D
	public static Dictionary<BiomeType, BiomeController> BiomeControllers
	{
		get
		{
			return WorldBuilder.Instance.m_biomeControllers;
		}
	}

	// Token: 0x170014FE RID: 5374
	// (get) Token: 0x06003C39 RID: 15417 RVA: 0x000D0449 File Offset: 0x000CE649
	public static bool DeactivateRoomGameObjects
	{
		get
		{
			return WorldBuilder.Instance.m_deactivateRoomGameObjects;
		}
	}

	// Token: 0x170014FF RID: 5375
	// (get) Token: 0x06003C3A RID: 15418 RVA: 0x000D0455 File Offset: 0x000CE655
	// (set) Token: 0x06003C3B RID: 15419 RVA: 0x000D045D File Offset: 0x000CE65D
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

	// Token: 0x17001500 RID: 5376
	// (get) Token: 0x06003C3C RID: 15420 RVA: 0x000D0466 File Offset: 0x000CE666
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

	// Token: 0x17001501 RID: 5377
	// (get) Token: 0x06003C3D RID: 15421 RVA: 0x000D0493 File Offset: 0x000CE693
	// (set) Token: 0x06003C3E RID: 15422 RVA: 0x000D049A File Offset: 0x000CE69A
	public static WorldBuilder Instance { get; private set; }

	// Token: 0x17001502 RID: 5378
	// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000D04A2 File Offset: 0x000CE6A2
	public static bool IsInstantiated
	{
		get
		{
			return WorldBuilder.Instance != null;
		}
	}

	// Token: 0x17001503 RID: 5379
	// (get) Token: 0x06003C40 RID: 15424 RVA: 0x000D04AF File Offset: 0x000CE6AF
	// (set) Token: 0x06003C41 RID: 15425 RVA: 0x000D04B6 File Offset: 0x000CE6B6
	public static BiomeBuildStateID State { get; private set; }

	// Token: 0x17001504 RID: 5380
	// (get) Token: 0x06003C42 RID: 15426 RVA: 0x000D04BE File Offset: 0x000CE6BE
	public static bool CreateReports
	{
		get
		{
			return WorldBuilder.Instance.LevelManager.Mode == LevelManagerMode.Debug;
		}
	}

	// Token: 0x17001505 RID: 5381
	// (get) Token: 0x06003C43 RID: 15427 RVA: 0x000D04D2 File Offset: 0x000CE6D2
	// (set) Token: 0x06003C44 RID: 15428 RVA: 0x000D04D9 File Offset: 0x000CE6D9
	public static BiomeType FirstBiomeOverride { get; set; }

	// Token: 0x17001506 RID: 5382
	// (get) Token: 0x06003C45 RID: 15429 RVA: 0x000D04E1 File Offset: 0x000CE6E1
	// (set) Token: 0x06003C46 RID: 15430 RVA: 0x000D04E9 File Offset: 0x000CE6E9
	public LevelManager_RL LevelManager { get; private set; }

	// Token: 0x06003C47 RID: 15431 RVA: 0x000D04F4 File Offset: 0x000CE6F4
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

	// Token: 0x06003C48 RID: 15432 RVA: 0x000D0540 File Offset: 0x000CE740
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

	// Token: 0x06003C49 RID: 15433 RVA: 0x000D0558 File Offset: 0x000CE758
	public static BiomeController CreateBiomeController(BiomeType biome, Transform parent)
	{
		BiomeController biomeController = new GameObject(biome.ToString() + "_BiomeController").AddComponent<BiomeController>();
		biomeController.Initialise(biome);
		biomeController.gameObject.transform.SetParent(parent);
		biomeController.gameObject.transform.position = Vector3.zero;
		return biomeController;
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x000D05B3 File Offset: 0x000CE7B3
	public static void CreateWorld()
	{
		WorldBuilder.State = BiomeBuildStateID.Running;
		if (WorldBuilder.Instance.m_printSeedsToConsole)
		{
			RNGManager.PrintSeedsToConsole();
		}
		WorldBuilder.Instance.StartCoroutine(WorldBuilder.Instance.CreateWorldCoroutine(null));
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x000D05E2 File Offset: 0x000CE7E2
	public static void CreateWorld(StageSaveData stageSaveData)
	{
		if (stageSaveData == null)
		{
			throw new ArgumentNullException("stageSaveData");
		}
		WorldBuilder.State = BiomeBuildStateID.Running;
		WorldBuilder.Instance.StartCoroutine(WorldBuilder.Instance.CreateWorldCoroutine(stageSaveData));
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x000D060E File Offset: 0x000CE80E
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

	// Token: 0x06003C4D RID: 15437 RVA: 0x000D0624 File Offset: 0x000CE824
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

	// Token: 0x06003C4E RID: 15438 RVA: 0x000D0680 File Offset: 0x000CE880
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

	// Token: 0x17001507 RID: 5383
	// (get) Token: 0x06003C4F RID: 15439 RVA: 0x000D06E8 File Offset: 0x000CE8E8
	// (set) Token: 0x06003C50 RID: 15440 RVA: 0x000D06EF File Offset: 0x000CE8EF
	public static BaseRoom OldCastleTransitionRoom { get; private set; }

	// Token: 0x06003C51 RID: 15441 RVA: 0x000D06F7 File Offset: 0x000CE8F7
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

	// Token: 0x06003C52 RID: 15442 RVA: 0x000D0706 File Offset: 0x000CE906
	public static BiomeController GetBiomeController(BiomeType biome)
	{
		if (WorldBuilder.BiomeControllers != null && WorldBuilder.BiomeControllers.ContainsKey(biome))
		{
			return WorldBuilder.BiomeControllers[biome];
		}
		return null;
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x000D0729 File Offset: 0x000CE929
	public void PreserveFirstBiomeOverride()
	{
		if (WorldBuilder.FirstBiomeOverride == BiomeType.None)
		{
			WorldBuilder.FirstBiomeOverride = this.m_firstBiome;
		}
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x000D0740 File Offset: 0x000CE940
	private void OnBiomeCreatorFail()
	{
		WorldBuilder.State = BiomeBuildStateID.Failed;
		WorldBuildCompleteEventArgs eventArgs = new WorldBuildCompleteEventArgs(RNGSeedManager.GetCurrentSeed(base.gameObject.scene.name), RNGManager.GetSeed(RngID.BiomeCreation), WorldBuilder.BiomeControllers.Values.ToList<BiomeController>(), false);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.WorldCreationFailed, this, eventArgs);
		this.PreserveFirstBiomeOverride();
		this.Cleanup();
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x000D07A0 File Offset: 0x000CE9A0
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

	// Token: 0x06003C56 RID: 15446 RVA: 0x000D0824 File Offset: 0x000CEA24
	private void OnDestroy()
	{
		WorldBuilder.Instance = null;
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x000D082C File Offset: 0x000CEA2C
	public static void Reset()
	{
		WorldBuilder.State = BiomeBuildStateID.None;
		WorldBuilder.Instance.Cleanup();
		if (WorldBuilder.Instance.m_biomeCreator != null)
		{
			WorldBuilder.Instance.m_biomeCreator.Reset();
		}
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x000D085F File Offset: 0x000CEA5F
	private IEnumerator RunBuildStage<T>(BiomeController biomeController) where T : IBiomeBuildStage, new()
	{
		T t = Activator.CreateInstance<T>();
		yield return t.Run(biomeController);
		yield break;
	}

	// Token: 0x04002D5F RID: 11615
	private const float ROOM_CONTENT_POSITION_X_MULTIPLIER = 1f;

	// Token: 0x04002D60 RID: 11616
	private const float ROOM_CONTENT_POSITION_Y_MULTIPLIER = 1f;

	// Token: 0x04002D61 RID: 11617
	[SerializeField]
	[HideInInspector]
	private BiomeLayer m_includedBiomes = BiomeLayer.Castle;

	// Token: 0x04002D62 RID: 11618
	[SerializeField]
	private BiomeCreator m_biomeCreator;

	// Token: 0x04002D63 RID: 11619
	[SerializeField]
	private bool m_instantiateRooms = true;

	// Token: 0x04002D64 RID: 11620
	[SerializeField]
	private bool m_deactivateRoomGameObjects = true;

	// Token: 0x04002D65 RID: 11621
	[SerializeField]
	private bool m_printSeedsToConsole = true;

	// Token: 0x04002D66 RID: 11622
	[SerializeField]
	private bool m_mergeRooms = true;

	// Token: 0x04002D67 RID: 11623
	[SerializeField]
	[HideInInspector]
	private List<BiomeType> m_biomeBuildOrder;

	// Token: 0x04002D68 RID: 11624
	private BiomeType m_firstBiome;

	// Token: 0x04002D69 RID: 11625
	private Dictionary<BiomeType, BiomeController> m_biomeControllers;
}
