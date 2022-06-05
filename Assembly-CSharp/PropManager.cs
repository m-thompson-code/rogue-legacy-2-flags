using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020006A4 RID: 1700
public class PropManager : MonoBehaviour
{
	// Token: 0x17001564 RID: 5476
	// (get) Token: 0x06003E40 RID: 15936 RVA: 0x000DAFE6 File Offset: 0x000D91E6
	// (set) Token: 0x06003E41 RID: 15937 RVA: 0x000DAFED File Offset: 0x000D91ED
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

	// Token: 0x17001565 RID: 5477
	// (get) Token: 0x06003E42 RID: 15938 RVA: 0x000DAFF5 File Offset: 0x000D91F5
	// (set) Token: 0x06003E43 RID: 15939 RVA: 0x000DAFFC File Offset: 0x000D91FC
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003E44 RID: 15940 RVA: 0x000DB004 File Offset: 0x000D9204
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

	// Token: 0x06003E45 RID: 15941 RVA: 0x000DB03B File Offset: 0x000D923B
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (PropManager.IsInitialized)
		{
			PropManager.DisableAllProps();
		}
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x000DB04C File Offset: 0x000D924C
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

	// Token: 0x06003E47 RID: 15943 RVA: 0x000DB0FE File Offset: 0x000D92FE
	private GenericPool_RL<Prop> CreatePool(Prop prefab, int poolSize)
	{
		GenericPool_RL<Prop> genericPool_RL = new GenericPool_RL<Prop>();
		genericPool_RL.Initialize(prefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x000DB110 File Offset: 0x000D9310
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

	// Token: 0x06003E49 RID: 15945 RVA: 0x000DB155 File Offset: 0x000D9355
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

	// Token: 0x06003E4A RID: 15946 RVA: 0x000DB18F File Offset: 0x000D938F
	private void Initialize()
	{
		PropManager.m_propTable = new Dictionary<int, GenericPool_RL<Prop>>();
		this.m_refreshCullingGroup = new Action<MonoBehaviour, EventArgs>(this.RefreshCullingGroup);
		PropManager.IsInitialized = true;
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x000DB1B4 File Offset: 0x000D93B4
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

	// Token: 0x06003E4C RID: 15948 RVA: 0x000DB2D4 File Offset: 0x000D94D4
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

	// Token: 0x06003E4D RID: 15949 RVA: 0x000DB43E File Offset: 0x000D963E
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

	// Token: 0x06003E4E RID: 15950 RVA: 0x000DB450 File Offset: 0x000D9650
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

	// Token: 0x06003E4F RID: 15951 RVA: 0x000DB4B0 File Offset: 0x000D96B0
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

	// Token: 0x06003E50 RID: 15952 RVA: 0x000DB61C File Offset: 0x000D981C
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

	// Token: 0x06003E51 RID: 15953 RVA: 0x000DB76C File Offset: 0x000D996C
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

	// Token: 0x06003E52 RID: 15954 RVA: 0x000DB7CC File Offset: 0x000D99CC
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

	// Token: 0x06003E53 RID: 15955 RVA: 0x000DB8A8 File Offset: 0x000D9AA8
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

	// Token: 0x06003E54 RID: 15956 RVA: 0x000DBC20 File Offset: 0x000D9E20
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

	// Token: 0x06003E55 RID: 15957 RVA: 0x000DBCAC File Offset: 0x000D9EAC
	public static void CreateBiomePools(BiomeType biome)
	{
		PropManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x06003E56 RID: 15958 RVA: 0x000DBCBC File Offset: 0x000D9EBC
	public static void DisableAllProps()
	{
		foreach (KeyValuePair<int, GenericPool_RL<Prop>> keyValuePair in PropManager.m_propTable)
		{
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x04002E55 RID: 11861
	private const int DEFAULT_CULLING_GROUP_SIZE = 3000;

	// Token: 0x04002E56 RID: 11862
	private const float CULLING_DEACTIVATION_RADIUS = 16f;

	// Token: 0x04002E57 RID: 11863
	[SerializeField]
	private int m_poolSize = 5;

	// Token: 0x04002E58 RID: 11864
	private CullingGroup m_cullingGroup;

	// Token: 0x04002E59 RID: 11865
	private BoundingSphere[] m_cullingSpheres;

	// Token: 0x04002E5A RID: 11866
	private static PropManager m_instance;

	// Token: 0x04002E5B RID: 11867
	private static Dictionary<int, GenericPool_RL<Prop>> m_propTable;

	// Token: 0x04002E5C RID: 11868
	private List<Prop> m_cullingPropList = new List<Prop>();

	// Token: 0x04002E5D RID: 11869
	private Dictionary<Prop, int> m_cullingGravityPropTable = new Dictionary<Prop, int>();

	// Token: 0x04002E5E RID: 11870
	private int m_highestPropCount;

	// Token: 0x04002E5F RID: 11871
	private int m_cullingGroupSize;

	// Token: 0x04002E60 RID: 11872
	private bool m_cullingGroupsInitialized;

	// Token: 0x04002E61 RID: 11873
	private Action<MonoBehaviour, EventArgs> m_refreshCullingGroup;
}
