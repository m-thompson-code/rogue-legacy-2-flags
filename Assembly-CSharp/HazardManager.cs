using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200069A RID: 1690
public class HazardManager : MonoBehaviour
{
	// Token: 0x1700154A RID: 5450
	// (get) Token: 0x06003DAA RID: 15786 RVA: 0x000D7086 File Offset: 0x000D5286
	// (set) Token: 0x06003DAB RID: 15787 RVA: 0x000D708D File Offset: 0x000D528D
	private static HazardManager Instance
	{
		get
		{
			return HazardManager.m_instance;
		}
		set
		{
			HazardManager.m_instance = value;
		}
	}

	// Token: 0x1700154B RID: 5451
	// (get) Token: 0x06003DAC RID: 15788 RVA: 0x000D7095 File Offset: 0x000D5295
	// (set) Token: 0x06003DAD RID: 15789 RVA: 0x000D709C File Offset: 0x000D529C
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003DAE RID: 15790 RVA: 0x000D70A4 File Offset: 0x000D52A4
	private void Awake()
	{
		if (!HazardManager.Instance)
		{
			HazardManager.Instance = this;
			this.Initialize();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			Debug.Log("<color=green>Creating Hazard Manager...</color>");
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x000D70F0 File Offset: 0x000D52F0
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (HazardManager.IsInitialized)
		{
			HazardManager.DisableAllHazards();
		}
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x000D7100 File Offset: 0x000D5300
	private void OnDestroy()
	{
		HazardManager.DestroyPools();
		HazardManager.m_instance = null;
		HazardManager.m_hazardTable = null;
		HazardManager.IsInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		this.m_cullingGroup.Dispose();
		this.m_cullingGroup = null;
		Array.Clear(this.m_cullingSpheres, 0, this.m_cullingSpheres.Length);
		this.m_cullingSpheres = null;
		this.m_cullingHazardList.Clear();
		this.m_cullingHazardList = null;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x000D7180 File Offset: 0x000D5380
	private void Initialize()
	{
		HazardManager.m_hazardTable = new Dictionary<HazardType, GenericPool_RL<Hazard>>();
		this.m_refreshCullingGroup = new Action<MonoBehaviour, EventArgs>(this.RefreshCullingGroup);
		HazardManager.IsInitialized = true;
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x000D71A4 File Offset: 0x000D53A4
	private IEnumerator Start()
	{
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		this.InitializeCullingGroup();
		yield break;
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x000D71B4 File Offset: 0x000D53B4
	private void InitializeCullingGroup()
	{
		this.m_cullingGroup = new CullingGroup();
		this.m_cullingSpheres = new BoundingSphere[100];
		for (int i = 0; i < this.m_cullingSpheres.Length; i++)
		{
			this.m_cullingSpheres[i] = new BoundingSphere(Vector3.zero, 55f);
		}
		this.m_cullingGroup.SetBoundingSpheres(this.m_cullingSpheres);
		this.m_cullingGroup.SetBoundingSphereCount(100);
		this.m_cullingGroup.targetCamera = CameraController.GameCamera;
		CullingGroup cullingGroup = this.m_cullingGroup;
		cullingGroup.onStateChanged = (CullingGroup.StateChanged)Delegate.Combine(cullingGroup.onStateChanged, new CullingGroup.StateChanged(this.CullingSphereStateChanged));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x000D7268 File Offset: 0x000D5468
	private void RefreshCullingGroup(object sender, EventArgs args)
	{
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		this.m_cullingHazardList.Clear();
		foreach (IHazardSpawnController hazardSpawnController in currentPlayerRoom.SpawnControllerManager.HazardSpawnControllers)
		{
			if (hazardSpawnController.ShouldSpawn && hazardSpawnController.Hazard != null)
			{
				this.m_cullingHazardList.Add(hazardSpawnController.Hazard);
			}
		}
		if (this.m_cullingHazardList.Count > 100)
		{
			throw new Exception("Culling group size for hazards is " + 100.ToString() + ".  Please increase HazardManager.CULLING_GROUP_SIZE to fix this error.");
		}
		this.m_cullingGroup.SetBoundingSphereCount(this.m_cullingHazardList.Count);
		for (int j = 0; j < this.m_cullingHazardList.Count; j++)
		{
			IHazard hazard = this.m_cullingHazardList[j];
			this.m_cullingSpheres[j].position = hazard.gameObject.transform.localPosition;
			hazard.SetIsCulled(false);
		}
		base.StartCoroutine(this.UpdateStartingCullStatesCoroutine());
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x000D7362 File Offset: 0x000D5562
	private IEnumerator UpdateStartingCullStatesCoroutine()
	{
		yield return null;
		for (int i = 0; i < this.m_cullingHazardList.Count; i++)
		{
			if (!this.m_cullingGroup.IsVisible(i))
			{
				this.m_cullingHazardList[i].SetIsCulled(true);
			}
		}
		yield break;
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x000D7374 File Offset: 0x000D5574
	private void CullingSphereStateChanged(CullingGroupEvent evt)
	{
		if (this.m_cullingHazardList.Count > evt.index)
		{
			IHazard hazard = this.m_cullingHazardList[evt.index];
			if (hazard == null)
			{
				return;
			}
			if (evt.hasBecomeVisible)
			{
				hazard.SetIsCulled(false);
				return;
			}
			if (evt.hasBecomeInvisible)
			{
				hazard.SetIsCulled(true);
			}
		}
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x000D73D0 File Offset: 0x000D55D0
	private void Internal_CreateBiomePools(BiomeType biomeType)
	{
		Dictionary<HazardType, int> dictionary = new Dictionary<HazardType, int>();
		BiomeController biomeController;
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		else
		{
			biomeController = WorldBuilder.GetBiomeController(biomeType);
		}
		if (biomeController)
		{
			Dictionary<HazardType, int> dictionary2 = new Dictionary<HazardType, int>();
			foreach (BaseRoom baseRoom in biomeController.Rooms)
			{
				dictionary2.Clear();
				foreach (ISpawnController spawnController in baseRoom.SpawnControllerManager.SpawnControllers)
				{
					IHazardSpawnController hazardSpawnController = spawnController as IHazardSpawnController;
					if (hazardSpawnController != null)
					{
						HazardType type = hazardSpawnController.Type;
						if (spawnController as Ferr2DHazardSpawnController)
						{
							if (!dictionary2.ContainsKey(type))
							{
								dictionary2.Add(type, 1);
							}
						}
						else
						{
							LineHazardSpawnController lineHazardSpawnController = spawnController as LineHazardSpawnController;
							int num = 1;
							if (lineHazardSpawnController != null)
							{
								num = lineHazardSpawnController.Width;
								HazardType lineHazardType = lineHazardSpawnController.GetLineHazardType(type);
								if (dictionary2.ContainsKey(lineHazardType))
								{
									Dictionary<HazardType, int> dictionary3 = dictionary2;
									HazardType key = lineHazardType;
									dictionary3[key]++;
								}
								else
								{
									dictionary2.Add(lineHazardType, 1);
								}
							}
							else
							{
								if (hazardSpawnController.Type == HazardType.Triple_Orbiter)
								{
									if (dictionary2.ContainsKey(HazardType.Orbiter))
									{
										Dictionary<HazardType, int> dictionary3 = dictionary2;
										dictionary3[HazardType.Orbiter] = dictionary3[HazardType.Orbiter] + 3;
									}
									else
									{
										dictionary2.Add(HazardType.Orbiter, 3);
									}
								}
								if (hazardSpawnController.Type == HazardType.SentryWithIce)
								{
									if (dictionary2.ContainsKey(HazardType.Sentry))
									{
										Dictionary<HazardType, int> dictionary3 = dictionary2;
										dictionary3[HazardType.Sentry] = dictionary3[HazardType.Sentry] + 1;
									}
									else
									{
										dictionary2.Add(HazardType.Sentry, 1);
									}
									if (dictionary2.ContainsKey(HazardType.IceCrystal))
									{
										Dictionary<HazardType, int> dictionary3 = dictionary2;
										dictionary3[HazardType.IceCrystal] = dictionary3[HazardType.IceCrystal] + 1;
									}
									else
									{
										dictionary2.Add(HazardType.IceCrystal, 1);
									}
								}
							}
							if (dictionary2.ContainsKey(type))
							{
								Dictionary<HazardType, int> dictionary3 = dictionary2;
								HazardType key = type;
								dictionary3[key] += num;
							}
							else
							{
								dictionary2.Add(type, num);
							}
						}
					}
				}
				foreach (KeyValuePair<HazardType, int> keyValuePair in dictionary2)
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						if (dictionary[keyValuePair.Key] < dictionary2[keyValuePair.Key])
						{
							dictionary[keyValuePair.Key] = dictionary2[keyValuePair.Key];
						}
					}
					else
					{
						dictionary.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}
		List<HazardType> list = HazardManager.m_hazardTable.Keys.ToList<HazardType>();
		int num2 = 0;
		int num3 = 0;
		foreach (KeyValuePair<HazardType, int> keyValuePair2 in dictionary)
		{
			if (keyValuePair2.Key != HazardType.None)
			{
				list.Remove(keyValuePair2.Key);
				if (!HazardManager.m_hazardTable.ContainsKey(keyValuePair2.Key))
				{
					Hazard prefab = HazardLibrary.GetPrefab(keyValuePair2.Key);
					HazardManager.m_hazardTable.Add(keyValuePair2.Key, this.CreatePool(prefab, keyValuePair2.Value));
					num3++;
				}
				else
				{
					HazardManager.m_hazardTable[keyValuePair2.Key].ResizePool(keyValuePair2.Value);
					num2++;
				}
			}
		}
		foreach (HazardType key2 in list)
		{
			if (HazardManager.m_hazardTable.ContainsKey(key2))
			{
				HazardManager.m_hazardTable[key2].DestroyPool();
				HazardManager.m_hazardTable.Remove(key2);
			}
		}
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x000D7830 File Offset: 0x000D5A30
	private GenericPool_RL<Hazard> CreatePool(Hazard prefab, int poolSize)
	{
		if (!prefab)
		{
			return null;
		}
		GenericPool_RL<Hazard> genericPool_RL = new GenericPool_RL<Hazard>();
		if (prefab is IFerr2DHazard)
		{
			genericPool_RL.Initialize(prefab, poolSize, true, true);
		}
		else
		{
			genericPool_RL.Initialize(prefab, poolSize, false, true);
		}
		return genericPool_RL;
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x000D786C File Offset: 0x000D5A6C
	public static IHazard GetHazard(HazardType hazardType)
	{
		if (HazardManager.m_hazardTable.ContainsKey(hazardType))
		{
			return HazardManager.m_hazardTable[hazardType].GetFreeObj();
		}
		Debug.LogFormat("<color=red>| HazardManager | Hazard Object Pool Table does not contain an entry for Hazard Type ({0})</color>", new object[]
		{
			hazardType
		});
		return null;
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x000D78A8 File Offset: 0x000D5AA8
	public static void DestroyPools()
	{
		foreach (KeyValuePair<HazardType, GenericPool_RL<Hazard>> keyValuePair in HazardManager.m_hazardTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		HazardManager.m_hazardTable.Clear();
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x000D790C File Offset: 0x000D5B0C
	public static void CreateBiomePools(BiomeType biome)
	{
		HazardManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x000D791C File Offset: 0x000D5B1C
	public static void DisableAllHazards()
	{
		foreach (KeyValuePair<HazardType, GenericPool_RL<Hazard>> keyValuePair in HazardManager.m_hazardTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04002DFE RID: 11774
	private const int CULLING_GROUP_SIZE = 100;

	// Token: 0x04002DFF RID: 11775
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04002E00 RID: 11776
	private static HazardManager m_instance;

	// Token: 0x04002E01 RID: 11777
	private static Dictionary<HazardType, GenericPool_RL<Hazard>> m_hazardTable;

	// Token: 0x04002E02 RID: 11778
	private CullingGroup m_cullingGroup;

	// Token: 0x04002E03 RID: 11779
	private BoundingSphere[] m_cullingSpheres;

	// Token: 0x04002E04 RID: 11780
	private List<IHazard> m_cullingHazardList = new List<IHazard>();

	// Token: 0x04002E05 RID: 11781
	private Action<MonoBehaviour, EventArgs> m_refreshCullingGroup;

	// Token: 0x04002E06 RID: 11782
	public const string RESOURCES_PATH = "Prefabs/Managers/HazardManager";
}
