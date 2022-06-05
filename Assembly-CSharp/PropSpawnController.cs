using System;
using System.Collections.Generic;
using System.Linq;
using Sigtrap.Relays;
using Spawn;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class PropSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController, ICameraLayerController, ISetSpawnType, IMirror
{
	// Token: 0x06004FE9 RID: 20457 RVA: 0x001313BC File Offset: 0x0012F5BC
	public static DecoController[] GetPrefabDecoControllers(Prop propPrefab)
	{
		DecoController[] components;
		if (PropSpawnController.m_propPrefabDecoControllers_STATIC.TryGetValue(propPrefab, out components))
		{
			return components;
		}
		components = propPrefab.GetComponents<DecoController>();
		PropSpawnController.m_propPrefabDecoControllers_STATIC.Add(propPrefab, components);
		return components;
	}

	// Token: 0x17001B81 RID: 7041
	// (get) Token: 0x06004FEA RID: 20458 RVA: 0x0002B9AD File Offset: 0x00029BAD
	public SpriteDrawMode SpriteRendererDrawMode
	{
		get
		{
			return this.m_spriteRendererDrawMode;
		}
	}

	// Token: 0x17001B82 RID: 7042
	// (get) Token: 0x06004FEB RID: 20459 RVA: 0x0002B9B5 File Offset: 0x00029BB5
	public Vector2 SpriteRendererSize
	{
		get
		{
			return this.m_spriteRendererSize;
		}
	}

	// Token: 0x17001B83 RID: 7043
	// (get) Token: 0x06004FEC RID: 20460 RVA: 0x0002B9BD File Offset: 0x00029BBD
	public DecoSpawnData[][] DecoSpawnData
	{
		get
		{
			return this.m_decoSpawnDataArray;
		}
	}

	// Token: 0x17001B84 RID: 7044
	// (get) Token: 0x06004FED RID: 20461 RVA: 0x0002B9C5 File Offset: 0x00029BC5
	public IRelayLink BeforePropInstanceInitializedRelay
	{
		get
		{
			return this.m_beforePropInstanceInitializedRelay.link;
		}
	}

	// Token: 0x17001B85 RID: 7045
	// (get) Token: 0x06004FEE RID: 20462 RVA: 0x0002B9D2 File Offset: 0x00029BD2
	public IRelayLink OnPropInstanceInitializedRelay
	{
		get
		{
			return this.m_onPropInstanceInitializedRelay.link;
		}
	}

	// Token: 0x17001B86 RID: 7046
	// (get) Token: 0x06004FEF RID: 20463 RVA: 0x0002B9DF File Offset: 0x00029BDF
	// (set) Token: 0x06004FF0 RID: 20464 RVA: 0x0002B9E7 File Offset: 0x00029BE7
	public bool DisableCulling
	{
		get
		{
			return this.m_disableCulling;
		}
		set
		{
			this.m_disableCulling = value;
		}
	}

	// Token: 0x17001B87 RID: 7047
	// (get) Token: 0x06004FF1 RID: 20465 RVA: 0x0002B9F0 File Offset: 0x00029BF0
	public bool IsBreakable
	{
		get
		{
			return this.PropInstance && this.PropInstance.Breakable;
		}
	}

	// Token: 0x17001B88 RID: 7048
	// (get) Token: 0x06004FF2 RID: 20466 RVA: 0x0002BA11 File Offset: 0x00029C11
	public int ActualSubLayer
	{
		get
		{
			return this.SubLayer + this.SubLayerMod;
		}
	}

	// Token: 0x17001B89 RID: 7049
	// (get) Token: 0x06004FF3 RID: 20467 RVA: 0x0002BA20 File Offset: 0x00029C20
	public CameraLayer CameraLayer
	{
		get
		{
			if (this.IsOverrideCameraLayer)
			{
				return this.CameraLayerOverride;
			}
			return this.Data.CameraLayer;
		}
	}

	// Token: 0x17001B8A RID: 7050
	// (get) Token: 0x06004FF4 RID: 20468 RVA: 0x0002BA3C File Offset: 0x00029C3C
	// (set) Token: 0x06004FF5 RID: 20469 RVA: 0x0002BA44 File Offset: 0x00029C44
	public PropSpawnControllerData Data
	{
		get
		{
			return this.m_spawnControllerData;
		}
		set
		{
			this.m_spawnControllerData = value;
		}
	}

	// Token: 0x17001B8B RID: 7051
	// (get) Token: 0x06004FF6 RID: 20470 RVA: 0x0002BA4D File Offset: 0x00029C4D
	// (set) Token: 0x06004FF7 RID: 20471 RVA: 0x0002BA55 File Offset: 0x00029C55
	public bool IsMirrored
	{
		get
		{
			return this.m_isMirrored;
		}
		private set
		{
			this.m_isMirrored = value;
		}
	}

	// Token: 0x17001B8C RID: 7052
	// (get) Token: 0x06004FF8 RID: 20472 RVA: 0x0002BA5E File Offset: 0x00029C5E
	public bool ShouldSpawn
	{
		get
		{
			return !this.SpawnLogicController || (this.SpawnLogicController.ShouldSpawn && base.gameObject.activeSelf);
		}
	}

	// Token: 0x17001B8D RID: 7053
	// (get) Token: 0x06004FF9 RID: 20473 RVA: 0x0002BA89 File Offset: 0x00029C89
	// (set) Token: 0x06004FFA RID: 20474 RVA: 0x0002BA91 File Offset: 0x00029C91
	public bool Override
	{
		get
		{
			return this.m_override;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_override = value;
			}
		}
	}

	// Token: 0x17001B8E RID: 7054
	// (get) Token: 0x06004FFB RID: 20475 RVA: 0x0002BAA1 File Offset: 0x00029CA1
	// (set) Token: 0x06004FFC RID: 20476 RVA: 0x0002BAA9 File Offset: 0x00029CA9
	public Prop OverrideProp
	{
		get
		{
			return this.m_overrideProp;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_overrideProp = value;
			}
		}
	}

	// Token: 0x17001B8F RID: 7055
	// (get) Token: 0x06004FFD RID: 20477 RVA: 0x0002BAB9 File Offset: 0x00029CB9
	// (set) Token: 0x06004FFE RID: 20478 RVA: 0x0002BAC1 File Offset: 0x00029CC1
	public Prop PropInstance
	{
		get
		{
			return this.m_propInstance;
		}
		private set
		{
			this.m_propInstance = value;
		}
	}

	// Token: 0x17001B90 RID: 7056
	// (get) Token: 0x06004FFF RID: 20479 RVA: 0x0002BACA File Offset: 0x00029CCA
	// (set) Token: 0x06005000 RID: 20480 RVA: 0x0002BAD2 File Offset: 0x00029CD2
	public Prop PropPrefab
	{
		get
		{
			return this.m_propPrefab;
		}
		private set
		{
			if (this.m_propPrefab != value)
			{
				this.m_propPrefab = value;
				if (this.m_propPrefab)
				{
					this.m_cachedPropPrefabNameHash = Animator.StringToHash(this.m_propPrefab.name);
				}
			}
		}
	}

	// Token: 0x06005001 RID: 20481 RVA: 0x0002BB0C File Offset: 0x00029D0C
	public void ForcePropPrefab(Prop propPrefab)
	{
		this.PropPrefab = propPrefab;
	}

	// Token: 0x17001B91 RID: 7057
	// (get) Token: 0x06005002 RID: 20482 RVA: 0x0002BB15 File Offset: 0x00029D15
	public Prop[] BreakableDecoInstances
	{
		get
		{
			return this.m_breakableDecoPropInstances;
		}
	}

	// Token: 0x17001B92 RID: 7058
	// (get) Token: 0x06005003 RID: 20483 RVA: 0x0002BB1D File Offset: 0x00029D1D
	public BiomePropsEntry[] PropTable
	{
		get
		{
			return this.Data.PropsPerBiome;
		}
	}

	// Token: 0x17001B93 RID: 7059
	// (get) Token: 0x06005004 RID: 20484 RVA: 0x0002BB2A File Offset: 0x00029D2A
	public Prop[] DefaultProps
	{
		get
		{
			return this.Data.DefaultProps;
		}
	}

	// Token: 0x17001B94 RID: 7060
	// (get) Token: 0x06005005 RID: 20485 RVA: 0x0002BB37 File Offset: 0x00029D37
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x17001B95 RID: 7061
	// (get) Token: 0x06005006 RID: 20486 RVA: 0x0002BB3F File Offset: 0x00029D3F
	// (set) Token: 0x06005007 RID: 20487 RVA: 0x0002BB62 File Offset: 0x00029D62
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			if (!this.m_hasCheckedForSpawnLogicController)
			{
				this.m_spawnLogicController = base.GetComponent<SpawnLogicController>();
				this.m_hasCheckedForSpawnLogicController = true;
			}
			return this.m_spawnLogicController;
		}
		set
		{
			this.m_spawnLogicController = value;
		}
	}

	// Token: 0x17001B96 RID: 7062
	// (get) Token: 0x06005008 RID: 20488 RVA: 0x0002BB6B File Offset: 0x00029D6B
	public int SubLayer
	{
		get
		{
			if (this.IsOverrideSubLayer)
			{
				return this.SubLayerOverride;
			}
			return this.Data.SubLayer;
		}
	}

	// Token: 0x17001B97 RID: 7063
	// (get) Token: 0x06005009 RID: 20489 RVA: 0x0002BB87 File Offset: 0x00029D87
	public int SubLayerMod
	{
		get
		{
			if (this.IsOverrideSubLayerMod)
			{
				return this.SubLayerModOverride;
			}
			return this.Data.SubLayerMod;
		}
	}

	// Token: 0x17001B98 RID: 7064
	// (get) Token: 0x0600500A RID: 20490 RVA: 0x0002BBA3 File Offset: 0x00029DA3
	// (set) Token: 0x0600500B RID: 20491 RVA: 0x0002BBAB File Offset: 0x00029DAB
	public bool IsOverrideSubLayer
	{
		get
		{
			return this.m_isOverrideSubLayer;
		}
		set
		{
			this.m_isOverrideSubLayer = value;
		}
	}

	// Token: 0x17001B99 RID: 7065
	// (get) Token: 0x0600500C RID: 20492 RVA: 0x0002BBB4 File Offset: 0x00029DB4
	// (set) Token: 0x0600500D RID: 20493 RVA: 0x0002BBBC File Offset: 0x00029DBC
	public int SubLayerOverride
	{
		get
		{
			return this.m_subLayerOverride;
		}
		set
		{
			this.m_subLayerOverride = value;
		}
	}

	// Token: 0x17001B9A RID: 7066
	// (get) Token: 0x0600500E RID: 20494 RVA: 0x0002BBC5 File Offset: 0x00029DC5
	// (set) Token: 0x0600500F RID: 20495 RVA: 0x0002BBCD File Offset: 0x00029DCD
	public bool IsOverrideSubLayerMod
	{
		get
		{
			return this.m_isOverrideSubLayerMod;
		}
		set
		{
			this.m_isOverrideSubLayerMod = value;
		}
	}

	// Token: 0x17001B9B RID: 7067
	// (get) Token: 0x06005010 RID: 20496 RVA: 0x0002BBD6 File Offset: 0x00029DD6
	// (set) Token: 0x06005011 RID: 20497 RVA: 0x0002BBDE File Offset: 0x00029DDE
	public int SubLayerModOverride
	{
		get
		{
			return this.m_subLayerModOverride;
		}
		set
		{
			this.m_subLayerModOverride = value;
		}
	}

	// Token: 0x17001B9C RID: 7068
	// (get) Token: 0x06005012 RID: 20498 RVA: 0x0002BBE7 File Offset: 0x00029DE7
	// (set) Token: 0x06005013 RID: 20499 RVA: 0x0002BBEF File Offset: 0x00029DEF
	public bool IsOverrideCameraLayer
	{
		get
		{
			return this.m_isOverrideCameraLayer;
		}
		set
		{
			this.m_isOverrideCameraLayer = value;
		}
	}

	// Token: 0x17001B9D RID: 7069
	// (get) Token: 0x06005014 RID: 20500 RVA: 0x0002BBF8 File Offset: 0x00029DF8
	// (set) Token: 0x06005015 RID: 20501 RVA: 0x0002BC00 File Offset: 0x00029E00
	public CameraLayer CameraLayerOverride
	{
		get
		{
			return this.m_cameraLayerOverride;
		}
		set
		{
			this.m_cameraLayerOverride = value;
		}
	}

	// Token: 0x17001B9E RID: 7070
	// (get) Token: 0x06005016 RID: 20502 RVA: 0x0002BC09 File Offset: 0x00029E09
	// (set) Token: 0x06005017 RID: 20503 RVA: 0x0002BC11 File Offset: 0x00029E11
	public bool IsOverrideZPosition
	{
		get
		{
			return this.m_isOverrideZPosition;
		}
		set
		{
			this.m_isOverrideZPosition = value;
		}
	}

	// Token: 0x17001B9F RID: 7071
	// (get) Token: 0x06005018 RID: 20504 RVA: 0x0002BC1A File Offset: 0x00029E1A
	// (set) Token: 0x06005019 RID: 20505 RVA: 0x0002BC22 File Offset: 0x00029E22
	public float ZPositionOverride
	{
		get
		{
			return this.m_zPositionOverride;
		}
		set
		{
			this.m_zPositionOverride = value;
		}
	}

	// Token: 0x0600501A RID: 20506 RVA: 0x001313F0 File Offset: 0x0012F5F0
	private void Start()
	{
		if (this.Override)
		{
			bool flag = false;
			BiomePropsEntry[] propTable = this.PropTable;
			for (int i = 0; i < propTable.Length; i++)
			{
				if (propTable[i].Props.Contains(this.OverrideProp))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.OverrideProp = null;
				this.Override = false;
			}
		}
	}

	// Token: 0x0600501B RID: 20507 RVA: 0x00131448 File Offset: 0x0012F648
	private void GetPropInstance()
	{
		try
		{
			this.PropInstance = PropManager.GetProp(this.PropPrefab, this.m_cachedPropPrefabNameHash);
		}
		catch (Exception)
		{
			throw new Exception("<color=red>| " + ((this != null) ? this.ToString() : null) + " | An exception occurred while attempting to get a Prop instance.</color>");
		}
		if (this.PropInstance)
		{
			this.PropInstance.ResetInitialization();
			this.PropInstance.gameObject.SetActive(true);
			this.PropInstance.Initialize(this.Room, this);
			for (int i = 0; i < this.PropInstance.DecoControllers.Length; i++)
			{
				this.PropInstance.DecoControllers[i].Initialize(this.m_decoSpawnDataArray[i]);
			}
			IRoomConsumer[] roomConsumers = this.PropInstance.RoomConsumers;
			for (int j = 0; j < roomConsumers.Length; j++)
			{
				roomConsumers[j].SetRoom(this.Room);
			}
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Failed to get instance of Prop ({1})</color>", new object[]
		{
			this,
			base.name
		});
	}

	// Token: 0x0600501C RID: 20508 RVA: 0x0002BC2B File Offset: 0x00029E2B
	public void Mirror()
	{
		this.IsMirrored = true;
	}

	// Token: 0x0600501D RID: 20509 RVA: 0x00131558 File Offset: 0x0012F758
	public void InitializePropInstance()
	{
		if (this.ShouldSpawn && this.PropPrefab && this.PropPrefab.gameObject)
		{
			this.m_beforePropInstanceInitializedRelay.Dispatch();
			this.GetPropInstance();
			if (this.PropInstance)
			{
				PropSpawnController.m_breakableDecoPropsHelper_STATIC.Clear();
				DecoController[] decoControllers = this.PropInstance.DecoControllers;
				for (int i = 0; i < decoControllers.Length; i++)
				{
					foreach (DecoLocation decoLocation in decoControllers[i].DecoLocations)
					{
						if (decoLocation.DecoInstance)
						{
							foreach (Prop prop in decoLocation.DecoInstance.Props)
							{
								if (prop.Breakable)
								{
									PropSpawnController.m_breakableDecoPropsHelper_STATIC.Add(prop);
								}
							}
						}
					}
				}
				if (!this.m_breakableDecoPropsArrayInitialized || ChallengeManager.IsInChallenge)
				{
					this.m_breakableDecoPropInstances = PropSpawnController.m_breakableDecoPropsHelper_STATIC.ToArray();
					this.m_breakableDecoPropsArrayInitialized = true;
				}
				else
				{
					if (this.m_breakableDecoPropInstances.Length != PropSpawnController.m_breakableDecoPropsHelper_STATIC.Count)
					{
						Debug.Log(string.Concat(new string[]
						{
							"Mismatch on : ",
							base.name,
							". DecoPropHelperSize: ",
							PropSpawnController.m_breakableDecoPropsHelper_STATIC.Count.ToString(),
							" propArraySize: ",
							this.m_breakableDecoPropInstances.Length.ToString()
						}));
					}
					for (int l = 0; l < this.m_breakableDecoPropInstances.Length; l++)
					{
						this.m_breakableDecoPropInstances[l] = PropSpawnController.m_breakableDecoPropsHelper_STATIC[l];
					}
				}
			}
			this.m_onPropInstanceInitializedRelay.Dispatch();
		}
	}

	// Token: 0x0600501E RID: 20510 RVA: 0x00131718 File Offset: 0x0012F918
	private void OnDisable()
	{
		if (this.PropInstance)
		{
			DecoController[] decoControllers = this.PropInstance.DecoControllers;
			for (int i = 0; i < decoControllers.Length; i++)
			{
				DecoLocation[] decoLocations = decoControllers[i].DecoLocations;
				for (int j = 0; j < decoLocations.Length; j++)
				{
					decoLocations[j].ReturnDecoToPool();
				}
			}
			this.PropInstance.gameObject.SetActive(false);
			this.PropInstance = null;
		}
	}

	// Token: 0x0600501F RID: 20511 RVA: 0x0002BC34 File Offset: 0x00029E34
	public bool Spawn()
	{
		if (this.Room == null)
		{
			throw new ArgumentNullException("Room");
		}
		if (this.ShouldSpawn && GameUtility.IsInLevelEditor)
		{
			this.SetSpawnType();
		}
		return this.PropPrefab;
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x00131784 File Offset: 0x0012F984
	public void SetSpawnType()
	{
		if (this.Override)
		{
			this.PropPrefab = this.OverrideProp;
		}
		else
		{
			Prop[] array = this.DefaultProps;
			BiomePropsEntry biomePropsEntry = null;
			foreach (BiomePropsEntry biomePropsEntry2 in this.PropTable)
			{
				if (biomePropsEntry2.Biome == this.Room.AppearanceBiomeType)
				{
					biomePropsEntry = biomePropsEntry2;
					break;
				}
			}
			if (biomePropsEntry != null && biomePropsEntry.Props.Length != 0)
			{
				array = biomePropsEntry.Props;
			}
			if (array != null && array.Length != 0)
			{
				int num = 0;
				if (array.Length > 1)
				{
					num = RNGManager.GetRandomNumber(RngID.Prop_RoomSeed, string.Format("PropSpawnController.SetSpawnType()", Array.Empty<object>()), 0, array.Length);
				}
				this.PropPrefab = array[num];
			}
		}
		this.SetDecoTypes(this.PropPrefab);
	}

	// Token: 0x06005021 RID: 20513 RVA: 0x0013183C File Offset: 0x0012FA3C
	private void SetDecoTypes(Prop prop)
	{
		if (!prop)
		{
			return;
		}
		if (!this.PropPrefab)
		{
			return;
		}
		DecoController[] prefabDecoControllers = PropSpawnController.GetPrefabDecoControllers(this.PropPrefab);
		int num = prefabDecoControllers.Length;
		this.m_decoSpawnDataArray = new DecoSpawnData[num][];
		for (int i = 0; i < num; i++)
		{
			DecoController decoController = prefabDecoControllers[i];
			int num2 = decoController.DecoLocations.Length;
			DecoSpawnData[] array = new DecoSpawnData[num2];
			for (int j = 0; j < num2; j++)
			{
				DecoLocation decoLocation = decoController.DecoLocations[j];
				DecoSpawnData decoSpawnData = default(DecoSpawnData);
				bool flag = RNGManager.GetRandomNumber(RngID.Deco_RoomSeed, "Get is Deco empty", 0, 100) > Mathf.RoundToInt(100f * decoLocation.ChanceEmpty);
				int num3 = decoLocation.PotentialDecos.Length;
				if (flag && num3 > 0)
				{
					int randomNumber = RNGManager.GetRandomNumber(RngID.Deco_RoomSeed, "Get Random Deco Index", 0, num3);
					bool isFlipped = RNGManager.GetRandomNumber(RngID.Deco_RoomSeed, "Get Flip Deco", 0, 2) == 1;
					decoSpawnData.ShouldSpawn = true;
					decoSpawnData.DecoPropIndex = (byte)randomNumber;
					decoSpawnData.IsFlipped = isFlipped;
				}
				array[j] = decoSpawnData;
			}
			this.m_decoSpawnDataArray[i] = array;
		}
	}

	// Token: 0x06005022 RID: 20514 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetCameraLayer(CameraLayer value)
	{
	}

	// Token: 0x06005023 RID: 20515 RVA: 0x0002BC6F File Offset: 0x00029E6F
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x06005024 RID: 20516 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetSubLayer(int subLayer, bool isDeco = false)
	{
	}

	// Token: 0x06005027 RID: 20519 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003C93 RID: 15507
	[SerializeField]
	private bool m_override;

	// Token: 0x04003C94 RID: 15508
	[SerializeField]
	private Prop m_overrideProp;

	// Token: 0x04003C95 RID: 15509
	[SerializeField]
	private PropSpawnControllerData m_spawnControllerData;

	// Token: 0x04003C96 RID: 15510
	[SerializeField]
	private bool m_isOverrideSubLayer;

	// Token: 0x04003C97 RID: 15511
	[SerializeField]
	private int m_subLayerOverride;

	// Token: 0x04003C98 RID: 15512
	[SerializeField]
	private bool m_isOverrideSubLayerMod;

	// Token: 0x04003C99 RID: 15513
	[SerializeField]
	private int m_subLayerModOverride;

	// Token: 0x04003C9A RID: 15514
	[SerializeField]
	private bool m_isOverrideCameraLayer;

	// Token: 0x04003C9B RID: 15515
	[SerializeField]
	private bool m_isOverrideZPosition;

	// Token: 0x04003C9C RID: 15516
	[SerializeField]
	private float m_zPositionOverride;

	// Token: 0x04003C9D RID: 15517
	[SerializeField]
	private CameraLayer m_cameraLayerOverride;

	// Token: 0x04003C9E RID: 15518
	[SerializeField]
	private bool m_disableCulling;

	// Token: 0x04003C9F RID: 15519
	[SerializeField]
	private SpriteDrawMode m_spriteRendererDrawMode;

	// Token: 0x04003CA0 RID: 15520
	[SerializeField]
	private Vector2 m_spriteRendererSize;

	// Token: 0x04003CA1 RID: 15521
	private static List<Prop> m_breakableDecoPropsHelper_STATIC = new List<Prop>();

	// Token: 0x04003CA2 RID: 15522
	private Prop m_propInstance;

	// Token: 0x04003CA3 RID: 15523
	private Prop m_propPrefab;

	// Token: 0x04003CA4 RID: 15524
	private int m_cachedPropPrefabNameHash;

	// Token: 0x04003CA5 RID: 15525
	private Prop[] m_breakableDecoPropInstances;

	// Token: 0x04003CA6 RID: 15526
	private bool m_breakableDecoPropsArrayInitialized;

	// Token: 0x04003CA7 RID: 15527
	private BaseRoom m_room;

	// Token: 0x04003CA8 RID: 15528
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04003CA9 RID: 15529
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04003CAA RID: 15530
	private bool m_isMirrored;

	// Token: 0x04003CAB RID: 15531
	private Relay m_onPropInstanceInitializedRelay = new Relay();

	// Token: 0x04003CAC RID: 15532
	private Relay m_beforePropInstanceInitializedRelay = new Relay();

	// Token: 0x04003CAD RID: 15533
	private bool m_hasCheckedForSpawnLogicController;

	// Token: 0x04003CAE RID: 15534
	private DecoSpawnData[][] m_decoSpawnDataArray;

	// Token: 0x04003CAF RID: 15535
	private static Dictionary<Prop, DecoController[]> m_propPrefabDecoControllers_STATIC = new Dictionary<Prop, DecoController[]>();
}
