using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B3F RID: 2879
public class PropManager : MonoBehaviour
{
	// Token: 0x17001D4C RID: 7500
	// (get) Token: 0x06005732 RID: 22322 RVA: 0x0002F757 File Offset: 0x0002D957
	// (set) Token: 0x06005733 RID: 22323 RVA: 0x0002F75E File Offset: 0x0002D95E
	private static PropManager Instance
	{
		get
		{
			return PropManager.m_instance;
		}
		set
		{
			PropManager.m_instance = value;
		}
	}

	// Token: 0x17001D4D RID: 7501
	// (get) Token: 0x06005734 RID: 22324 RVA: 0x0002F766 File Offset: 0x0002D966
	// (set) Token: 0x06005735 RID: 22325 RVA: 0x0002F76D File Offset: 0x0002D96D
	public static bool IsInitialized { get; private set; }

	// Token: 0x06005736 RID: 22326 RVA: 0x0002F775 File Offset: 0x0002D975
	private void Awake()
	{
		if (!PropManager.Instance)
		{
			PropManager.Instance = this;
			this.Initialize();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06005737 RID: 22327 RVA: 0x0002F7AC File Offset: 0x0002D9AC
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (PropManager.IsInitialized)
		{
			PropManager.DisableAllProps();
		}
	}

	// Token: 0x06005738 RID: 22328 RVA: 0x0014B64C File Offset: 0x0014984C
	private void OnDestroy()
	{
		PropManager.DestroyPools();
		if (this.m_cullingGroup != null)
		{
			this.m_cullingGroup.Dispose();
		}
		this.m_cullingGroup = null;
		if (this.m_cullingPropList != null)
		{
			this.m_cullingPropList.Clear();
		}
		if (this.m_cullingGravityPropTable != null)
		{
			this.m_cullingGravityPropTable.Clear();
		}
		this.m_cullingPropList = null;
		if (this.m_cullingSpheres != null)
		{
			Array.Clear(this.m_cullingSpheres, 0, this.m_cullingSpheres.Length);
		}
		this.m_cullingSpheres = null;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		PropManager.m_instance = null;
		PropManager.m_propTable = null;
		PropManager.IsInitialized = false;
		this.m_cullingGroupsInitialized = false;
	}

	// Token: 0x06005739 RID: 22329 RVA: 0x0002F7BA File Offset: 0x0002D9BA
	private GenericPool_RL<Prop> CreatePool(Prop prefab, int poolSize)
	{
		GenericPool_RL<Prop> genericPool_RL = new GenericPool_RL<Prop>();
		genericPool_RL.Initialize(prefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x0014B700 File Offset: 0x00149900
	public static void AddPropToPool(Prop propPrefab, int poolSize)
	{
		if (propPrefab)
		{
			int key = Animator.StringToHash(propPrefab.name);
			if (!PropManager.m_propTable.ContainsKey(key))
			{
				PropManager.m_propTable.Add(key, PropManager.Instance.CreatePool(propPrefab, poolSize));
			}
		}
	}

	// Token: 0x0600573B RID: 22331 RVA: 0x0002F7CB File Offset: 0x0002D9CB
	public static Prop GetProp(Prop prop, int propNameHash)
	{
		if (PropManager.m_propTable.ContainsKey(propNameHash))
		{
			return PropManager.m_propTable[propNameHash].GetFreeObj();
		}
		Debug.LogFormat("<color=red>| PropManager | Prop Object Pool Table does not contain an entry for Prop named ({0})</color>", new object[]
		{
			prop.name
		});
		return null;
	}

	// Token: 0x0600573C RID: 22332 RVA: 0x0002F805 File Offset: 0x0002DA05
	private void Initialize()
	{
		PropManager.m_propTable = new Dictionary<int, GenericPool_RL<Prop>>();
		this.m_refreshCullingGroup = new Action<MonoBehaviour, EventArgs>(this.RefreshCullingGroup);
		PropManager.IsInitialized = true;
	}

	// Token: 0x0600573D RID: 22333 RVA: 0x0014B748 File Offset: 0x00149948
	private void InitializeCullingGroup(int groupSize)
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
		if (this.m_cullingGroup != null)
		{
			this.m_cullingGroup.Dispose();
			this.m_cullingGroup = null;
		}
		if (this.m_cullingSpheres != null)
		{
			Array.Clear(this.m_cullingSpheres, 0, this.m_cullingSpheres.Length);
			this.m_cullingSpheres = null;
		}
		if (groupSize <= 0)
		{
			groupSize = 3000;
		}
		this.m_cullingGroupSize = groupSize;
		this.m_cullingGroup = new CullingGroup();
		this.m_cullingSpheres = new BoundingSphere[this.m_cullingGroupSize];
		for (int i = 0; i < this.m_cullingSpheres.Length; i++)
		{
			this.m_cullingSpheres[i] = new BoundingSphere(Vector3.zero, 16f);
		}
		this.m_cullingGroup.SetBoundingSpheres(this.m_cullingSpheres);
		this.m_cullingGroup.SetBoundingSphereCount(this.m_cullingGroupSize);
		this.m_cullingGroup.targetCamera = CameraController.GameCamera;
		CullingGroup cullingGroup = this.m_cullingGroup;
		cullingGroup.onStateChanged = (CullingGroup.StateChanged)Delegate.Combine(cullingGroup.onStateChanged, new CullingGroup.StateChanged(this.CullingSphereStateChanged));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_refreshCullingGroup);
		this.m_cullingGroupsInitialized = true;
	}

	// Token: 0x0600573E RID: 22334 RVA: 0x0014B868 File Offset: 0x00149A68
	private void RefreshCullingGroup(object sender, EventArgs args)
	{
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		this.m_cullingPropList.Clear();
		this.m_cullingGravityPropTable.Clear();
		foreach (PropSpawnController propSpawnController in currentPlayerRoom.SpawnControllerManager.PropSpawnControllers)
		{
			if (propSpawnController.ShouldSpawn && propSpawnController.PropInstance && !propSpawnController.DisableCulling)
			{
				this.m_cullingPropList.Add(propSpawnController.PropInstance);
				if (propSpawnController.CameraLayer == CameraLayer.Game && propSpawnController.PropInstance.CorgiController && !this.m_cullingGravityPropTable.ContainsKey(propSpawnController.PropInstance))
				{
					this.m_cullingGravityPropTable.Add(propSpawnController.PropInstance, this.m_cullingPropList.Count - 1);
				}
			}
		}
		if (this.m_cullingPropList.Count > this.m_cullingGroupSize)
		{
			throw new Exception("Culling group size for props is " + this.m_cullingGroupSize.ToString() + ".  Look into the max props code or increase PropManager.DEFAULT_CULLING_GROUP_SIZE to fix this error.");
		}
		this.m_cullingGroup.SetBoundingSphereCount(this.m_cullingPropList.Count);
		for (int j = 0; j < this.m_cullingPropList.Count; j++)
		{
			Prop prop = this.m_cullingPropList[j];
			this.m_cullingSpheres[j].position = prop.gameObject.transform.localPosition;
			this.SetPropCullState(prop, false);
		}
		base.StartCoroutine(this.UpdateStartingCullStatesCoroutine());
	}

	// Token: 0x0600573F RID: 22335 RVA: 0x0002F829 File Offset: 0x0002DA29
	private IEnumerator UpdateStartingCullStatesCoroutine()
	{
		yield return null;
		for (int i = 0; i < this.m_cullingPropList.Count; i++)
		{
			if (!this.m_cullingGroup.IsVisible(i))
			{
				this.SetPropCullState(this.m_cullingPropList[i], true);
			}
		}
		yield break;
	}

	// Token: 0x06005740 RID: 22336 RVA: 0x0014B9D4 File Offset: 0x00149BD4
	private void CullingSphereStateChanged(CullingGroupEvent evt)
	{
		if (this.m_cullingPropList.Count > evt.index)
		{
			Prop prop = this.m_cullingPropList[evt.index];
			if (!prop)
			{
				return;
			}
			if (evt.hasBecomeVisible)
			{
				this.SetPropCullState(prop, false);
				return;
			}
			if (evt.hasBecomeInvisible)
			{
				this.SetPropCullState(prop, true);
			}
		}
	}

	// Token: 0x06005741 RID: 22337 RVA: 0x0014BA34 File Offset: 0x00149C34
	private void SetDecoCullStates(Prop prop, bool cull)
	{
		DecoController[] decoControllers = prop.DecoControllers;
		for (int i = 0; i < decoControllers.Length; i++)
		{
			foreach (DecoLocation decoLocation in decoControllers[i].DecoLocations)
			{
				if (decoLocation.DecoInstance)
				{
					foreach (Prop prop2 in decoLocation.DecoInstance.Props)
					{
						if (prop2.CameraLayerController.CameraLayer == CameraLayer.Game && prop2.HitboxController != null && prop2.HitboxesDisabled != cull)
						{
							prop2.HitboxController.SetCulledState(cull, true);
							prop2.HitboxesDisabled = cull;
						}
						if (prop2.IsCulled != cull)
						{
							if (prop2.Animators != null)
							{
								Animator[] animators = prop2.Animators;
								for (int l = 0; l < animators.Length; l++)
								{
									animators[l].enabled = !cull;
								}
							}
							if (prop2.Lights != null)
							{
								if (!cull)
								{
									if (QualitySettings.GetQualityLevel() != 0)
									{
										Light[] lights = prop.Lights;
										for (int l = 0; l < lights.Length; l++)
										{
											lights[l].enabled = true;
										}
									}
								}
								else
								{
									Light[] lights = prop.Lights;
									for (int l = 0; l < lights.Length; l++)
									{
										lights[l].enabled = false;
									}
								}
							}
							prop2.IsCulled = cull;
						}
					}
				}
			}
		}
	}

	// Token: 0x06005742 RID: 22338 RVA: 0x0014BBA0 File Offset: 0x00149DA0
	private void SetPropCullState(Prop prop, bool cull)
	{
		if (prop.CameraLayerController.CameraLayer == CameraLayer.Game && prop.HitboxController != null)
		{
			if (!cull)
			{
				if (prop.HitboxesDisabled)
				{
					prop.HitboxController.SetCulledState(false, true);
					prop.HitboxesDisabled = false;
				}
			}
			else if (!prop.HitboxesDisabled)
			{
				bool includeRigidbody = !prop.CorgiController && !prop.HitboxController.GetCollider(HitboxType.Platform);
				prop.HitboxController.SetCulledState(true, includeRigidbody);
				prop.HitboxesDisabled = true;
			}
		}
		this.SetDecoCullStates(prop, cull);
		if (!cull)
		{
			if (!prop.IsCulled)
			{
				return;
			}
			if (prop.Animators != null)
			{
				Animator[] animators = prop.Animators;
				for (int i = 0; i < animators.Length; i++)
				{
					animators[i].enabled = true;
				}
			}
			if (prop.Lights != null && QualitySettings.GetQualityLevel() != 0)
			{
				Light[] lights = prop.Lights;
				for (int i = 0; i < lights.Length; i++)
				{
					lights[i].enabled = true;
				}
			}
			prop.IsCulled = false;
			return;
		}
		else
		{
			if (prop.IsCulled)
			{
				return;
			}
			if (prop.Animators != null)
			{
				Animator[] animators = prop.Animators;
				for (int i = 0; i < animators.Length; i++)
				{
					animators[i].enabled = false;
				}
			}
			if (prop.Lights != null)
			{
				Light[] lights = prop.Lights;
				for (int i = 0; i < lights.Length; i++)
				{
					lights[i].enabled = false;
				}
			}
			prop.IsCulled = true;
			return;
		}
	}

	// Token: 0x06005743 RID: 22339 RVA: 0x0014BCF0 File Offset: 0x00149EF0
	private void SetPropCorgiControllerCullState(Prop prop, bool cull)
	{
		prop.HitboxController.SetCulledState(cull, true);
		if (prop.CorgiController.PermanentlyDisabled)
		{
			return;
		}
		if (cull)
		{
			if (prop.CorgiController.enabled)
			{
				prop.CorgiController.enabled = false;
				return;
			}
		}
		else if (!prop.CorgiController.enabled)
		{
			prop.CorgiController.enabled = true;
		}
	}

	// Token: 0x06005744 RID: 22340 RVA: 0x0014BD50 File Offset: 0x00149F50
	private void FixedUpdate()
	{
		if (this.m_cullingGroupsInitialized)
		{
			Vector3 localPosition = this.m_cullingGroup.targetCamera.transform.localPosition;
			float num = 32f * CameraController.ZoomLevel;
			foreach (KeyValuePair<Prop, int> keyValuePair in this.m_cullingGravityPropTable)
			{
				Vector3 localPosition2 = keyValuePair.Key.gameObject.transform.localPosition;
				this.m_cullingSpheres[keyValuePair.Value].position = localPosition2;
				float num2 = localPosition.x - localPosition2.x;
				bool cull = num2 < -num || num2 > num;
				this.SetPropCorgiControllerCullState(keyValuePair.Key, cull);
			}
		}
	}

	// Token: 0x06005745 RID: 22341 RVA: 0x0014BE2C File Offset: 0x0014A02C
	private void Internal_CreateBiomePools(BiomeType biomeType)
	{
		this.m_highestPropCount = 0;
		Dictionary<Prop, int> dictionary = new Dictionary<Prop, int>();
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
			Dictionary<Prop, int> dictionary2 = new Dictionary<Prop, int>();
			List<BaseRoom> list = new List<BaseRoom>();
			list.AddRange(biomeController.Rooms);
			if (biomeType != BiomeType.Castle)
			{
				BiomeController biomeController2 = GameUtility.IsInLevelEditor ? OnPlayManager.BiomeController : WorldBuilder.GetBiomeController(BiomeType.Castle);
				BaseRoom baseRoom = (biomeController2 != null) ? biomeController2.TransitionRoom : null;
				if (baseRoom != null)
				{
					list.Add(baseRoom);
				}
			}
			foreach (BaseRoom baseRoom2 in list)
			{
				int num = 0;
				dictionary2.Clear();
				PropSpawnController[] propSpawnControllers = baseRoom2.SpawnControllerManager.PropSpawnControllers;
				for (int i = 0; i < propSpawnControllers.Length; i++)
				{
					Prop propPrefab = propSpawnControllers[i].PropPrefab;
					if (propPrefab)
					{
						if (dictionary2.ContainsKey(propPrefab))
						{
							Dictionary<Prop, int> dictionary3 = dictionary2;
							Prop key = propPrefab;
							dictionary3[key]++;
						}
						else
						{
							dictionary2.Add(propPrefab, 1);
						}
						num++;
					}
				}
				foreach (KeyValuePair<Prop, int> keyValuePair in dictionary2)
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
				if (num > this.m_highestPropCount)
				{
					this.m_highestPropCount = num;
				}
			}
		}
		List<int> list2 = PropManager.m_propTable.Keys.ToList<int>();
		int num2 = 0;
		int num3 = 0;
		foreach (KeyValuePair<Prop, int> keyValuePair2 in dictionary)
		{
			if (keyValuePair2.Key)
			{
				int num4 = Animator.StringToHash(keyValuePair2.Key.name);
				list2.Remove(num4);
				if (!PropManager.m_propTable.ContainsKey(num4))
				{
					PropManager.m_propTable.Add(num4, this.CreatePool(keyValuePair2.Key, keyValuePair2.Value));
					num3++;
				}
				else
				{
					PropManager.m_propTable[num4].ResizePool(keyValuePair2.Value);
					num2++;
				}
			}
		}
		foreach (int key2 in list2)
		{
			if (PropManager.m_propTable.ContainsKey(key2))
			{
				PropManager.m_propTable[key2].DestroyPool();
				PropManager.m_propTable.Remove(key2);
			}
		}
		this.InitializeCullingGroup(this.m_highestPropCount);
	}

	// Token: 0x06005746 RID: 22342 RVA: 0x0014C1A4 File Offset: 0x0014A3A4
	public static void DestroyPools()
	{
		if (PropManager.Instance.m_cullingGroupsInitialized)
		{
			PropManager.Instance.m_cullingGravityPropTable.Clear();
			PropManager.Instance.m_cullingPropList.Clear();
		}
		foreach (KeyValuePair<int, GenericPool_RL<Prop>> keyValuePair in PropManager.m_propTable)
		{
			keyValuePair.Value.DestroyPool();
		}
		PropManager.m_propTable.Clear();
	}

	// Token: 0x06005747 RID: 22343 RVA: 0x0002F838 File Offset: 0x0002DA38
	public static void CreateBiomePools(BiomeType biome)
	{
		PropManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x0014C230 File Offset: 0x0014A430
	public static void DisableAllProps()
	{
		foreach (KeyValuePair<int, GenericPool_RL<Prop>> keyValuePair in PropManager.m_propTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04004072 RID: 16498
	private const int DEFAULT_CULLING_GROUP_SIZE = 3000;

	// Token: 0x04004073 RID: 16499
	private const float CULLING_DEACTIVATION_RADIUS = 16f;

	// Token: 0x04004074 RID: 16500
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04004075 RID: 16501
	private CullingGroup m_cullingGroup;

	// Token: 0x04004076 RID: 16502
	private BoundingSphere[] m_cullingSpheres;

	// Token: 0x04004077 RID: 16503
	private static PropManager m_instance;

	// Token: 0x04004078 RID: 16504
	private static Dictionary<int, GenericPool_RL<Prop>> m_propTable;

	// Token: 0x04004079 RID: 16505
	private List<Prop> m_cullingPropList = new List<Prop>();

	// Token: 0x0400407A RID: 16506
	private Dictionary<Prop, int> m_cullingGravityPropTable = new Dictionary<Prop, int>();

	// Token: 0x0400407B RID: 16507
	private int m_highestPropCount;

	// Token: 0x0400407C RID: 16508
	private int m_cullingGroupSize;

	// Token: 0x0400407D RID: 16509
	private bool m_cullingGroupsInitialized;

	// Token: 0x0400407E RID: 16510
	private Action<MonoBehaviour, EventArgs> m_refreshCullingGroup;
}
