using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B22 RID: 2850
public class EnemyManager : MonoBehaviour
{
	// Token: 0x060055F4 RID: 22004 RVA: 0x00144BF8 File Offset: 0x00142DF8
	private void Awake()
	{
		this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
		this.m_refreshCullingGroup = new Action<MonoBehaviour, EventArgs>(this.RefreshCullingGroup);
		this.Initialize();
		Debug.Log("<color=green>Creating Enemy Manager...</color>");
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x060055F5 RID: 22005 RVA: 0x0002EC25 File Offset: 0x0002CE25
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		base.StopAllCoroutines();
		EnemyManager.DisableAllSummonedEnemies();
	}

	// Token: 0x060055F6 RID: 22006 RVA: 0x0002EC32 File Offset: 0x0002CE32
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (EnemyManager.IsInitialized)
		{
			base.StopAllCoroutines();
			EnemyManager.DisableAllEnemies();
			EnemyManager.DisableAllSummonedEnemies();
		}
	}

	// Token: 0x060055F7 RID: 22007 RVA: 0x0002EC4B File Offset: 0x0002CE4B
	private IEnumerator Start()
	{
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		this.InitializeCullingGroup();
		yield break;
	}

	// Token: 0x060055F8 RID: 22008 RVA: 0x00144C58 File Offset: 0x00142E58
	private void InitializeCullingGroup()
	{
		this.m_cullingGroup = new CullingGroup();
		this.m_cullingSpheres = new BoundingSphere[100];
		for (int i = 0; i < this.m_cullingSpheres.Length; i++)
		{
			this.m_cullingSpheres[i] = new BoundingSphere(Vector3.zero, 18f);
		}
		this.m_cullingGroup.SetBoundingSpheres(this.m_cullingSpheres);
		this.m_cullingGroup.SetBoundingSphereCount(100);
		this.m_cullingGroup.targetCamera = CameraController.GameCamera;
		CullingGroup cullingGroup = this.m_cullingGroup;
		cullingGroup.onStateChanged = (CullingGroup.StateChanged)Delegate.Combine(cullingGroup.onStateChanged, new CullingGroup.StateChanged(this.CullingSphereStateChanged));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
	}

	// Token: 0x060055F9 RID: 22009 RVA: 0x00144D0C File Offset: 0x00142F0C
	private void RefreshCullingGroup(object sender, EventArgs args)
	{
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		this.m_cullingEnemyList.Clear();
		foreach (EnemySpawnController enemySpawnController in currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.ShouldSpawn && !enemySpawnController.IsDead && enemySpawnController.EnemyInstance != null && !enemySpawnController.EnemyInstance.Equals(null))
			{
				this.m_cullingEnemyList.Add(enemySpawnController.EnemyInstance);
			}
		}
		if (this.m_cullingEnemyList.Count > 100)
		{
			throw new Exception("Culling group size for enemies is " + 100.ToString() + ".  Please increase EnemyManager.CULLING_GROUP_SIZE to fix this error.");
		}
		this.m_cullingGroup.SetBoundingSphereCount(this.m_cullingEnemyList.Count);
		for (int j = 0; j < this.m_cullingEnemyList.Count; j++)
		{
			EnemyController enemyController = this.m_cullingEnemyList[j];
			this.m_cullingSpheres[j].position = enemyController.transform.localPosition;
			base.StartCoroutine(EnemyManager.SetEnemyCulled(enemyController, false, false));
		}
		base.StartCoroutine(this.UpdateStartingCullStatesCoroutine());
	}

	// Token: 0x060055FA RID: 22010 RVA: 0x0002EC5A File Offset: 0x0002CE5A
	private IEnumerator UpdateStartingCullStatesCoroutine()
	{
		yield return null;
		for (int i = 0; i < this.m_cullingEnemyList.Count; i++)
		{
			if (!this.m_cullingGroup.IsVisible(i))
			{
				base.StartCoroutine(EnemyManager.SetEnemyCulled(this.m_cullingEnemyList[i], true, false));
			}
		}
		yield break;
	}

	// Token: 0x060055FB RID: 22011 RVA: 0x00144E28 File Offset: 0x00143028
	private void CullingSphereStateChanged(CullingGroupEvent evt)
	{
		if (this.m_cullingEnemyList.Count > evt.index)
		{
			EnemyController enemy = this.m_cullingEnemyList[evt.index];
			if (evt.hasBecomeVisible)
			{
				base.StartCoroutine(EnemyManager.SetEnemyCulled(enemy, false, false));
				return;
			}
			if (evt.hasBecomeInvisible)
			{
				base.StartCoroutine(EnemyManager.SetEnemyCulled(enemy, true, false));
			}
		}
	}

	// Token: 0x060055FC RID: 22012 RVA: 0x00144E90 File Offset: 0x00143090
	private void FixedUpdate()
	{
		for (int i = 0; i < this.m_cullingEnemyList.Count; i++)
		{
			if (this.m_cullingEnemyList[i])
			{
				this.m_cullingSpheres[i].position = this.m_cullingEnemyList[i].gameObject.transform.localPosition;
			}
		}
	}

	// Token: 0x060055FD RID: 22013 RVA: 0x0002EC69 File Offset: 0x0002CE69
	public static IEnumerator SetEnemyCulled(EnemyController enemy, bool culled, bool isSpawning = false)
	{
		if (!enemy.IsCulled && (enemy.DisableCulling || enemy.IsBoss))
		{
			yield break;
		}
		if (enemy.IsDead)
		{
			yield break;
		}
		if (enemy.gameObject.activeSelf)
		{
			while (!enemy.IsInitialized)
			{
				yield return null;
			}
			if (!culled)
			{
				if (!enemy.IsCulled)
				{
					yield break;
				}
				foreach (GameObject gameObject in enemy.CulledObjectsArray)
				{
					if (!gameObject.activeSelf)
					{
						gameObject.SetActive(true);
					}
				}
				enemy.HitboxController.SetCulledState(false, true);
				enemy.VisualBoundsObj.Rigidbody.simulated = true;
				enemy.CharacterCorgi.enabled = true;
				enemy.ControllerCorgi.enabled = true;
				enemy.Animator.enabled = true;
				enemy.LogicController.LogicScript.enabled = true;
				enemy.LogicController.enabled = true;
				if (isSpawning)
				{
					enemy.Visuals.SetActive(false);
				}
				if (enemy.ResetToNeutralWhenUnculling)
				{
					enemy.Animator.Play(EnemyManager.NEUTRAL_ANIM_PARAM, 0, 1f);
				}
				if (enemy.PreventPlatformDropObj && !enemy.FallLedge)
				{
					enemy.PreventPlatformDropObj.enabled = true;
				}
				enemy.IsCulled = false;
			}
			else if (culled)
			{
				if (enemy.IsCulled)
				{
					yield break;
				}
				foreach (GameObject gameObject2 in enemy.CulledObjectsArray)
				{
					if (gameObject2.activeSelf)
					{
						gameObject2.SetActive(false);
					}
				}
				enemy.HitboxController.SetCulledState(true, true);
				enemy.VisualBoundsObj.Rigidbody.simulated = false;
				enemy.CharacterCorgi.enabled = false;
				enemy.ControllerCorgi.enabled = false;
				enemy.Animator.enabled = false;
				enemy.LogicController.LogicScript.enabled = false;
				enemy.LogicController.enabled = false;
				if (enemy.PreventPlatformDropObj)
				{
					enemy.PreventPlatformDropObj.enabled = false;
				}
				enemy.IsCulled = true;
			}
		}
		yield break;
	}

	// Token: 0x060055FE RID: 22014 RVA: 0x00144EF4 File Offset: 0x001430F4
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		EnemyManager.DestroyPools();
		EnemyManager.m_enemyManager = null;
		EnemyManager.m_isInitialized = false;
		if (this.m_cullingGroup != null)
		{
			this.m_cullingGroup.Dispose();
		}
		this.m_cullingGroup = null;
		if (this.m_cullingSpheres != null)
		{
			Array.Clear(this.m_cullingSpheres, 0, this.m_cullingSpheres.Length);
		}
		this.m_cullingSpheres = null;
		if (this.m_cullingEnemyList != null)
		{
			this.m_cullingEnemyList.Clear();
		}
		this.m_cullingEnemyList = null;
		this.m_refreshCullingGroup = null;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x060055FF RID: 22015 RVA: 0x0002EC86 File Offset: 0x0002CE86
	private void Initialize()
	{
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		this.m_enemySummonedArgs = new EnemySummonedEventArgs(null, null);
		this.m_enemyPoolTable = new Dictionary<EnemyType, Dictionary<EnemyRank, GenericPool_RL<EnemyController>>>();
		EnemyManager.m_isInitialized = true;
	}

	// Token: 0x06005600 RID: 22016 RVA: 0x00144F9C File Offset: 0x0014319C
	private void Internal_CreateBiomePools(BiomeType biomeType)
	{
		Dictionary<EnemyController, int> dictionary = new Dictionary<EnemyController, int>();
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
			bool flag = BurdenManager.GetBurdenLevel(BurdenType.EnemyEvolve) > 0;
			Dictionary<EnemyController, int> dictionary2 = new Dictionary<EnemyController, int>();
			List<SummonRuleController> list = new List<SummonRuleController>();
			List<EnemyTypeAndRank> list2 = new List<EnemyTypeAndRank>();
			List<EnemyTypeAndRank> list3 = new List<EnemyTypeAndRank>();
			foreach (BaseRoom baseRoom in biomeController.Rooms)
			{
				dictionary2.Clear();
				foreach (EnemySpawnController enemySpawnController in baseRoom.SpawnControllerManager.EnemySpawnControllers)
				{
					if (enemySpawnController.ShouldSpawn)
					{
						EnemySpawnController enemySpawnController2 = enemySpawnController;
						EnemyController enemyPrefab = EnemyLibrary.GetEnemyPrefab(enemySpawnController2.Type, enemySpawnController2.Rank);
						if (enemyPrefab)
						{
							if (dictionary2.ContainsKey(enemyPrefab))
							{
								Dictionary<EnemyController, int> dictionary3 = dictionary2;
								EnemyController key = enemyPrefab;
								dictionary3[key]++;
							}
							else
							{
								dictionary2.Add(enemyPrefab, 1);
							}
						}
					}
				}
				list.Clear();
				baseRoom.gameObject.GetComponentsInChildren<SummonRuleController>(true, list);
				EnemyRank enemyRank = EnemyRank.None;
				foreach (SummonRuleController summonRuleController in list)
				{
					foreach (BaseSummonRule baseSummonRule in summonRuleController.SummonRuleArray)
					{
						if (baseSummonRule.RuleType == SummonRuleType.SetSummonPoolDifficulty)
						{
							enemyRank = (baseSummonRule as SetSummonPoolDifficulty_SummonRule).SummonPoolDifficulty;
						}
						else if (baseSummonRule.RuleType == SummonRuleType.SetSummonPool)
						{
							SetSummonPool_SummonRule setSummonPool_SummonRule = baseSummonRule as SetSummonPool_SummonRule;
							list2.Clear();
							if (setSummonPool_SummonRule.PoolIsBiomeSpecific)
							{
								list2.AddRange(EnemyUtility.GetAllEnemiesInBiome(baseRoom.BiomeType, false));
								for (int j = 0; j < list2.Count; j++)
								{
									if (SetSummonPool_SummonRule.SummonExceptionArray.Contains(list2[j].Type))
									{
										list2.RemoveAt(j);
										j--;
									}
								}
								if (flag && enemyRank == EnemyRank.None)
								{
									list3.Clear();
									for (int k = 0; k < list2.Count; k++)
									{
										EnemyTypeAndRank enemyTypeAndRank = list2[k];
										if (enemyTypeAndRank.Rank < EnemyRank.Advanced)
										{
											EnemyTypeAndRank item = new EnemyTypeAndRank(enemyTypeAndRank.Type, EnemyRank.Advanced);
											if (!list2.Contains(item) && !list3.Contains(item))
											{
												list3.Add(item);
											}
										}
										if (enemyTypeAndRank.Rank < EnemyRank.Expert)
										{
											EnemyTypeAndRank item2 = new EnemyTypeAndRank(enemyTypeAndRank.Type, EnemyRank.Expert);
											if (!list2.Contains(item2) && !list3.Contains(item2))
											{
												list3.Add(item2);
											}
										}
									}
									list2.AddRange(list3);
								}
							}
							else
							{
								list2.AddRange(setSummonPool_SummonRule.EnemiesToSummonArray);
							}
							foreach (EnemyTypeAndRank enemyTypeAndRank2 in list2)
							{
								EnemyRank enemyRank2 = enemyTypeAndRank2.Rank;
								if (enemyRank != EnemyRank.None && enemyRank != EnemyRank.Any)
								{
									enemyRank2 = enemyRank;
								}
								EnemyController enemyPrefab2 = EnemyLibrary.GetEnemyPrefab(enemyTypeAndRank2.Type, enemyRank2);
								if (enemyPrefab2)
								{
									if (dictionary2.ContainsKey(enemyPrefab2))
									{
										Dictionary<EnemyController, int> dictionary3 = dictionary2;
										EnemyController key = enemyPrefab2;
										dictionary3[key]++;
									}
									else
									{
										dictionary2.Add(enemyPrefab2, 1);
									}
								}
							}
						}
					}
				}
				foreach (KeyValuePair<EnemyController, int> keyValuePair in dictionary2)
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
		Dictionary<EnemyController, int> dictionary4 = new Dictionary<EnemyController, int>(dictionary);
		foreach (KeyValuePair<EnemyController, int> keyValuePair2 in dictionary)
		{
			SummonEnemyController component = keyValuePair2.Key.GetComponent<SummonEnemyController>();
			if (component)
			{
				foreach (SummonEnemyController.SummonEnemyEntry summonEntry in component.EnemiesToSummon)
				{
					this.RecursivelyAddSummonedEnemies(dictionary4, summonEntry);
				}
			}
		}
		List<Vector2Int> list4 = new List<Vector2Int>();
		foreach (KeyValuePair<EnemyType, Dictionary<EnemyRank, GenericPool_RL<EnemyController>>> keyValuePair3 in this.m_enemyPoolTable)
		{
			foreach (KeyValuePair<EnemyRank, GenericPool_RL<EnemyController>> keyValuePair4 in keyValuePair3.Value)
			{
				list4.Add(new Vector2Int((int)keyValuePair3.Key, (int)keyValuePair4.Key));
			}
		}
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<EnemyController, int> keyValuePair5 in dictionary4)
		{
			EnemyController key2 = keyValuePair5.Key;
			if (key2)
			{
				list4.Remove(new Vector2Int((int)key2.EnemyType, (int)key2.EnemyRank));
				if (this.m_enemyPoolTable.ContainsKey(key2.EnemyType))
				{
					Dictionary<EnemyRank, GenericPool_RL<EnemyController>> dictionary5 = this.m_enemyPoolTable[key2.EnemyType];
					if (!dictionary5.ContainsKey(key2.EnemyRank))
					{
						dictionary5.Add(key2.EnemyRank, this.CreateEnemyPool(key2, keyValuePair5.Value));
						num2++;
					}
					else
					{
						dictionary5[key2.EnemyRank].ResizePool(keyValuePair5.Value);
						num++;
					}
				}
				else
				{
					Dictionary<EnemyRank, GenericPool_RL<EnemyController>> dictionary6 = new Dictionary<EnemyRank, GenericPool_RL<EnemyController>>();
					dictionary6.Add(key2.EnemyRank, this.CreateEnemyPool(key2, keyValuePair5.Value));
					this.m_enemyPoolTable.Add(key2.EnemyType, dictionary6);
					num2++;
				}
			}
		}
		foreach (Vector2Int vector2Int in list4)
		{
			EnemyType x = (EnemyType)vector2Int.x;
			EnemyRank y = (EnemyRank)vector2Int.y;
			if (this.m_enemyPoolTable.ContainsKey(x))
			{
				Dictionary<EnemyRank, GenericPool_RL<EnemyController>> dictionary7 = this.m_enemyPoolTable[x];
				if (dictionary7.ContainsKey(y))
				{
					dictionary7[y].DestroyPool();
					dictionary7.Remove(y);
				}
				if (dictionary7.Count <= 0)
				{
					this.m_enemyPoolTable.Remove(x);
				}
			}
		}
		ProjectileManager.Instance.CreateEnemyProjectilePools(dictionary4.Keys.ToList<EnemyController>());
	}

	// Token: 0x06005601 RID: 22017 RVA: 0x00145768 File Offset: 0x00143968
	private void RecursivelyAddSummonedEnemies(Dictionary<EnemyController, int> enemyCountDict, SummonEnemyController.SummonEnemyEntry summonEntry)
	{
		if (summonEntry.NumSummons <= 0)
		{
			return;
		}
		EnemyController enemyPrefab = EnemyLibrary.GetEnemyPrefab(summonEntry.EnemyType, summonEntry.EnemyRank);
		if (enemyPrefab)
		{
			if (!enemyCountDict.ContainsKey(enemyPrefab))
			{
				enemyCountDict.Add(enemyPrefab, summonEntry.NumSummons);
				SummonEnemyController component = enemyPrefab.GetComponent<SummonEnemyController>();
				if (component)
				{
					foreach (SummonEnemyController.SummonEnemyEntry summonEntry2 in component.EnemiesToSummon)
					{
						this.RecursivelyAddSummonedEnemies(enemyCountDict, summonEntry2);
					}
					return;
				}
			}
			else if (enemyCountDict.ContainsKey(enemyPrefab) && enemyCountDict[enemyPrefab] < summonEntry.NumSummons)
			{
				enemyCountDict[enemyPrefab] = summonEntry.NumSummons;
			}
		}
	}

	// Token: 0x06005602 RID: 22018 RVA: 0x0002ECB1 File Offset: 0x0002CEB1
	private GenericPool_RL<EnemyController> CreateEnemyPool(EnemyController prefab, int poolSize)
	{
		if (!prefab)
		{
			return null;
		}
		GenericPool_RL<EnemyController> genericPool_RL = new GenericPool_RL<EnemyController>();
		genericPool_RL.Initialize(prefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x17001D0E RID: 7438
	// (get) Token: 0x06005603 RID: 22019 RVA: 0x0002ECCC File Offset: 0x0002CECC
	public static List<EnemyController> SummonedEnemyList
	{
		get
		{
			return EnemyManager.Instance.m_summonedEnemyList;
		}
	}

	// Token: 0x17001D0F RID: 7439
	// (get) Token: 0x06005604 RID: 22020 RVA: 0x0002ECD8 File Offset: 0x0002CED8
	private static EnemyManager Instance
	{
		get
		{
			if (!EnemyManager.m_enemyManager)
			{
				EnemyManager.m_enemyManager = CDGHelper.FindStaticInstance<EnemyManager>(false);
			}
			return EnemyManager.m_enemyManager;
		}
	}

	// Token: 0x06005605 RID: 22021 RVA: 0x0002ECF6 File Offset: 0x0002CEF6
	public static bool Contains(EnemyType enemyType, EnemyRank enemyRank)
	{
		return !string.IsNullOrEmpty(EnemyLibrary.GetEnemyPrefabPath(enemyType, enemyRank));
	}

	// Token: 0x17001D10 RID: 7440
	// (get) Token: 0x06005606 RID: 22022 RVA: 0x0014580C File Offset: 0x00143A0C
	public static int NumActiveSummonedEnemies
	{
		get
		{
			int num = 0;
			foreach (EnemyController enemyController in EnemyManager.Instance.m_summonedEnemyList)
			{
				if (!enemyController.IsDead || enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_DeathDelay))
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x06005607 RID: 22023 RVA: 0x00145880 File Offset: 0x00143A80
	public static int GetNumActiveSummonedEnemiesOfType(EnemyType enemyType, EnemyRank enemyRank)
	{
		int num = 0;
		foreach (EnemyController enemyController in EnemyManager.Instance.m_summonedEnemyList)
		{
			if ((!enemyController.IsDead || enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_DeathDelay)) && (enemyController.EnemyType == enemyType || enemyType == EnemyType.Any) && (enemyController.EnemyRank == enemyRank || enemyRank == EnemyRank.Any))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x17001D11 RID: 7441
	// (get) Token: 0x06005608 RID: 22024 RVA: 0x00145910 File Offset: 0x00143B10
	public static int NumActiveEnemies
	{
		get
		{
			int num = 0;
			if (PlayerManager.IsInstantiated)
			{
				BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
				if (currentPlayerRoom)
				{
					EnemySpawnController[] enemySpawnControllers = currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers;
					for (int i = 0; i < enemySpawnControllers.Length; i++)
					{
						EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
						if (enemyInstance && (!enemyInstance.IsDead || enemyInstance.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_DeathDelay)))
						{
							num++;
						}
					}
				}
			}
			return num;
		}
	}

	// Token: 0x06005609 RID: 22025 RVA: 0x00145984 File Offset: 0x00143B84
	public static int GetNumActiveEnemiesOfType(EnemyType enemyType, EnemyRank enemyRank)
	{
		int num = 0;
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			if (currentPlayerRoom != null)
			{
				foreach (EnemySpawnController enemySpawnController in currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers)
				{
					if (!enemySpawnController.EnemyInstance.IsNativeNull() && (!enemySpawnController.EnemyInstance.IsDead || enemySpawnController.EnemyInstance.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_DeathDelay)) && (enemySpawnController.EnemyInstance.EnemyType == enemyType || enemyType == EnemyType.Any) && (enemySpawnController.EnemyInstance.EnemyRank == enemyRank || enemyRank == EnemyRank.Any))
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x17001D12 RID: 7442
	// (get) Token: 0x0600560A RID: 22026 RVA: 0x0002ED07 File Offset: 0x0002CF07
	public static bool IsInitialized
	{
		get
		{
			return EnemyManager.m_isInitialized;
		}
	}

	// Token: 0x0600560B RID: 22027 RVA: 0x00145A34 File Offset: 0x00143C34
	public static EnemyController GetEnemyFromPool(EnemyType enemyType, EnemyRank enemyRank)
	{
		if (!EnemyManager.IsInitialized)
		{
			Debug.LogFormat("<color=yellow>You tried to get an Enemy from the Enemy Object Pool before it was initialised<color>", Array.Empty<object>());
			return null;
		}
		Dictionary<EnemyRank, GenericPool_RL<EnemyController>> dictionary = null;
		if (!EnemyManager.Instance.m_enemyPoolTable.TryGetValue(enemyType, out dictionary))
		{
			Debug.LogFormat("Unable to find entry for Enemy Type ({0}) in Enemy Manager", new object[]
			{
				enemyType
			});
			return null;
		}
		GenericPool_RL<EnemyController> genericPool_RL = null;
		if (dictionary.TryGetValue(enemyRank, out genericPool_RL))
		{
			EnemyController freeObj = genericPool_RL.GetFreeObj();
			freeObj.gameObject.SetActive(true);
			return freeObj;
		}
		Debug.LogFormat("Unable to find entry for Enemy Rank ({0}) for Enemy Type ({1}) in Enemy Pool", new object[]
		{
			enemyRank,
			enemyType
		});
		return null;
	}

	// Token: 0x0600560C RID: 22028 RVA: 0x00145AD0 File Offset: 0x00143CD0
	public static EnemyController SummonEnemy(ISummoner summoner, EnemyType enemyType, EnemyRank enemyRank, Vector2 spawnPosOffset, bool useAbsPos = false, bool runSummonAnim = true, float speedMod = 1f, float hpMod = 1f)
	{
		EnemyController enemyFromPool = EnemyManager.GetEnemyFromPool(enemyType, enemyRank);
		if (enemyFromPool)
		{
			enemyFromPool.Summoner = summoner;
			if (summoner != null)
			{
				enemyFromPool.SetLevel(summoner.Level);
			}
			else
			{
				enemyFromPool.SetLevel(PlayerManager.GetCurrentPlayerRoom().Level);
			}
			enemyFromPool.SetRoom(PlayerManager.GetCurrentPlayerRoom());
			enemyFromPool.EnemySpawnController = null;
			Vector3 localPosition = spawnPosOffset;
			if (!useAbsPos && summoner != null)
			{
				localPosition.x += summoner.gameObject.transform.position.x;
				localPosition.y += summoner.gameObject.transform.position.y;
			}
			enemyFromPool.gameObject.transform.localPosition = localPosition;
			enemyFromPool.ForceFaceTarget();
			enemyFromPool.IsCommander = false;
			EnemyManager.Instance.m_enemySummonedArgs.Initialize(enemyFromPool, summoner);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemySummoned, EnemyManager.Instance, EnemyManager.Instance.m_enemySummonedArgs);
			if (runSummonAnim)
			{
				EnemyManager.Instance.StartCoroutine(EnemyManager.Instance.Internal_RunSummonAnimCoroutine(enemyFromPool, speedMod, false, hpMod));
			}
		}
		return enemyFromPool;
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x0002ED0E File Offset: 0x0002CF0E
	public static IEnumerator RunSummonAnimCoroutine(EnemyController enemy, float speedMod = 1f, bool disableEnemyOffset = false, float hpMod = 1f)
	{
		yield return EnemyManager.Instance.Internal_RunSummonAnimCoroutine(enemy, speedMod, disableEnemyOffset, hpMod);
		yield break;
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x0002ED32 File Offset: 0x0002CF32
	private IEnumerator Internal_RunSummonAnimCoroutine(EnemyController enemy, float speedMod = 1f, bool disableEnemyOffset = false, float hpMod = 1f)
	{
		enemy.ResetCharacter();
		enemy.HitboxController.DisableAllCollisions = true;
		EffectManager.AddAnimatorToDisableList(enemy.Animator);
		enemy.gameObject.SetActive(true);
		enemy.Visuals.SetActive(false);
		EnemyManager.Instance.m_summonedEnemyList.Add(enemy);
		while (!enemy.IsInitialized)
		{
			yield return null;
		}
		enemy.IsBeingSummoned = true;
		enemy.InitializeCommanderStatusEffects();
		yield return EnemyManager.SetEnemyCulled(enemy, false, true);
		if (!disableEnemyOffset && !enemy.ForceDisableSummonOffset)
		{
			Vector3 localPosition = enemy.gameObject.transform.localPosition;
			localPosition.y -= enemy.Midpoint.y - enemy.gameObject.transform.localPosition.y;
			enemy.gameObject.transform.localPosition = localPosition;
		}
		enemy.OnPositionedForSummoningRelay.Dispatch(null, null);
		enemy.UpdateBounds();
		enemy.ResetCollisionState();
		enemy.ControllerCorgi.enabled = false;
		float duration = 1.2f / speedMod;
		BaseEffect baseEffect = EffectManager.PlayEffect(null, null, "SpellCircle_Effect", enemy.Midpoint, duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect.GetComponent<Animator>().speed = speedMod;
		Bounds visualBounds = enemy.VisualBounds;
		bool flag = visualBounds.size.y > visualBounds.size.x;
		Bounds bounds = new Bounds(baseEffect.transform.localPosition, new Vector3(2f, 2f, 2f));
		float num = flag ? (visualBounds.size.y / bounds.size.y) : (visualBounds.size.x / bounds.size.x);
		num *= 1f;
		num = Mathf.Clamp(num, 1.25f, 999f);
		baseEffect.transform.localScale = new Vector3(num, num, num);
		List<Renderer> rendererArray = enemy.RendererArray;
		if (rendererArray != null)
		{
			foreach (Renderer renderer in rendererArray)
			{
				renderer.GetPropertyBlock(this.m_matPropertyBlock);
				this.m_matPropertyBlock.SetColor(ShaderID_RL._AlphaBlendColor, Color.white);
				renderer.SetPropertyBlock(this.m_matPropertyBlock);
			}
		}
		EnemyManager.EnemyPreSummonedState preSummonedState = default(EnemyManager.EnemyPreSummonedState);
		Vector3 localScale = enemy.gameObject.transform.localScale;
		preSummonedState.StoredLocalScaleX = localScale.x;
		localScale.x = 0.01f;
		enemy.gameObject.transform.localScale = localScale;
		enemy.SetHealth(Mathf.Max((float)enemy.ActualMaxHealth * hpMod, 1f), false, false);
		preSummonedState.StoredLockFlip = enemy.LockFlip;
		enemy.Animator.enabled = false;
		enemy.LogicController.enabled = false;
		enemy.LockFlip = true;
		enemy.SetVelocity(0f, 0f, false);
		preSummonedState.StoredActivationState = enemy.LogicController.DisableLogicActivationByDistance;
		enemy.LogicController.DisableLogicActivationByDistance = true;
		enemy.PreSummonedState = preSummonedState;
		float yieldDuration = Time.time + 0.6f / speedMod;
		while (Time.time < yieldDuration)
		{
			yield return null;
		}
		if (!enemy.InvisibleDuringSummonAnim)
		{
			enemy.Visuals.SetActive(true);
		}
		enemy.Animator.enabled = true;
		enemy.Animator.Play(EnemyManager.NEUTRAL_ANIM_PARAM, 0, 1f);
		yield return null;
		enemy.Animator.enabled = false;
		yield return TweenManager.TweenTo(enemy.gameObject.transform, 0.2f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			preSummonedState.StoredLocalScaleX
		});
		yieldDuration = Time.time + 0.6f / speedMod;
		while (Time.time < yieldDuration)
		{
			yield return null;
		}
		EnemyManager.ResetEnemySummonState(enemy);
		enemy.LogicController.TriggerAggro(null, null);
		yield break;
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x00145BE0 File Offset: 0x00143DE0
	public static void KillAllSummonedEnemies()
	{
		foreach (EnemyController enemyController in EnemyManager.Instance.m_summonedEnemyList)
		{
			if (enemyController && !enemyController.IsDead)
			{
				enemyController.KillCharacter(PlayerManager.GetPlayerController().gameObject, true);
			}
		}
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x00145C54 File Offset: 0x00143E54
	public static void DisableAllSummonedEnemies()
	{
		foreach (EnemyController enemyController in EnemyManager.Instance.m_summonedEnemyList)
		{
			if (enemyController)
			{
				EnemyManager.ResetEnemySummonState(enemyController);
				enemyController.gameObject.SetActive(false);
			}
		}
		EnemyManager.Instance.m_summonedEnemyList.Clear();
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x00145CD0 File Offset: 0x00143ED0
	public static void KillAllNonSummonedEnemies()
	{
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			if (currentPlayerRoom)
			{
				EnemySpawnController[] enemySpawnControllers = currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers;
				for (int i = 0; i < enemySpawnControllers.Length; i++)
				{
					EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
					if (enemyInstance && !enemyInstance.IsDead)
					{
						enemyInstance.KillCharacter(PlayerManager.GetPlayerController().gameObject, true);
					}
				}
			}
		}
	}

	// Token: 0x06005612 RID: 22034 RVA: 0x00145D38 File Offset: 0x00143F38
	private static void ResetEnemySummonState(EnemyController enemy)
	{
		if (enemy.IsBeingSummoned)
		{
			enemy.transform.SetLocalScaleX(enemy.PreSummonedState.StoredLocalScaleX);
			enemy.HitboxController.DisableAllCollisions = false;
			enemy.LogicController.DisableLogicActivationByDistance = enemy.PreSummonedState.StoredActivationState;
			enemy.Animator.enabled = true;
			enemy.LogicController.AssignAnimParamRank();
			enemy.LogicController.enabled = true;
			enemy.StatusEffectController.SetAllStatusEffectsHidden(false);
			enemy.ControllerCorgi.enabled = true;
			enemy.LockFlip = enemy.PreSummonedState.StoredLockFlip;
			enemy.IsBeingSummoned = false;
			if (!enemy.Visuals.activeSelf)
			{
				enemy.Visuals.SetActive(true);
			}
			EffectManager.RemoveAnimatorFromDisableList(enemy.Animator);
			enemy.UseOverrideDefaultTint = false;
			enemy.ResetRendererArrayColor();
			if (TraitManager.IsTraitActive(TraitType.EnemiesBlackFill))
			{
				enemy.BlinkPulseEffect.ResetAllBlackFills();
				enemy.BlinkPulseEffect.ActivateBlackFill(BlackFillType.EnemiesBlackFill_Trait, 0f);
			}
		}
	}

	// Token: 0x06005613 RID: 22035 RVA: 0x00145E34 File Offset: 0x00144034
	public static void DisableAllEnemies()
	{
		foreach (KeyValuePair<EnemyType, Dictionary<EnemyRank, GenericPool_RL<EnemyController>>> keyValuePair in EnemyManager.Instance.m_enemyPoolTable)
		{
			foreach (KeyValuePair<EnemyRank, GenericPool_RL<EnemyController>> keyValuePair2 in keyValuePair.Value)
			{
				keyValuePair2.Value.DisableAll();
			}
		}
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x00145ED0 File Offset: 0x001440D0
	private void LateUpdate()
	{
		for (int i = 0; i < this.m_summonedEnemyList.Count; i++)
		{
			if (this.m_summonedEnemyList[i].IsNativeNull() || !this.m_summonedEnemyList[i].gameObject.activeInHierarchy)
			{
				EnemyManager.ResetEnemySummonState(this.m_summonedEnemyList[i]);
				this.m_summonedEnemyList.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06005615 RID: 22037 RVA: 0x00145F40 File Offset: 0x00144140
	public static void DestroyPools()
	{
		if (!GameManager.IsApplicationClosing)
		{
			if (!EnemyManager.IsInitialized)
			{
				return;
			}
			if (EnemyManager.Instance.m_cullingEnemyList != null)
			{
				EnemyManager.Instance.m_cullingEnemyList.Clear();
			}
			foreach (KeyValuePair<EnemyType, Dictionary<EnemyRank, GenericPool_RL<EnemyController>>> keyValuePair in EnemyManager.Instance.m_enemyPoolTable)
			{
				foreach (KeyValuePair<EnemyRank, GenericPool_RL<EnemyController>> keyValuePair2 in keyValuePair.Value)
				{
					keyValuePair2.Value.DestroyPool();
				}
				keyValuePair.Value.Clear();
			}
			EnemyManager.Instance.m_enemyPoolTable.Clear();
		}
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x0002ED5E File Offset: 0x0002CF5E
	public static void CreateBiomePools(BiomeType biome)
	{
		EnemyManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x04003FB5 RID: 16309
	private const int CULLING_GROUP_SIZE = 100;

	// Token: 0x04003FB6 RID: 16310
	private const string MANAGER_NAME = "EnemyManager";

	// Token: 0x04003FB7 RID: 16311
	private const string RESOURCE_PATH = "Prefabs/Managers/EnemyManager";

	// Token: 0x04003FB8 RID: 16312
	private static readonly int NEUTRAL_ANIM_PARAM = Animator.StringToHash("Neutral");

	// Token: 0x04003FB9 RID: 16313
	private Dictionary<EnemyType, Dictionary<EnemyRank, GenericPool_RL<EnemyController>>> m_enemyPoolTable;

	// Token: 0x04003FBA RID: 16314
	private List<EnemyController> m_summonedEnemyList = new List<EnemyController>();

	// Token: 0x04003FBB RID: 16315
	private CullingGroup m_cullingGroup;

	// Token: 0x04003FBC RID: 16316
	private List<EnemyController> m_cullingEnemyList = new List<EnemyController>();

	// Token: 0x04003FBD RID: 16317
	private BoundingSphere[] m_cullingSpheres;

	// Token: 0x04003FBE RID: 16318
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x04003FBF RID: 16319
	private EnemySummonedEventArgs m_enemySummonedArgs;

	// Token: 0x04003FC0 RID: 16320
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04003FC1 RID: 16321
	private Action<MonoBehaviour, EventArgs> m_refreshCullingGroup;

	// Token: 0x04003FC2 RID: 16322
	private static bool m_isInitialized;

	// Token: 0x04003FC3 RID: 16323
	private static EnemyManager m_enemyManager = null;

	// Token: 0x02000B23 RID: 2851
	public struct EnemyPreSummonedState
	{
		// Token: 0x04003FC4 RID: 16324
		public bool StoredActivationState;

		// Token: 0x04003FC5 RID: 16325
		public bool StoredLockFlip;

		// Token: 0x04003FC6 RID: 16326
		public float StoredLocalScaleX;
	}
}
