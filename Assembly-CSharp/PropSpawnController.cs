using System;
using System.Collections.Generic;
using System.Linq;
using Sigtrap.Relays;
using Spawn;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class PropSpawnController : MonoBehaviour, ISimpleSpawnController, ISpawnController, ICameraLayerController, ISetSpawnType, IMirror
{
	// Token: 0x0600390A RID: 14602 RVA: 0x000C297C File Offset: 0x000C0B7C
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

	// Token: 0x1700141A RID: 5146
	// (get) Token: 0x0600390B RID: 14603 RVA: 0x000C29AE File Offset: 0x000C0BAE
	public SpriteDrawMode SpriteRendererDrawMode
	{
		get
		{
			return this.m_spriteRendererDrawMode;
		}
	}

	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x0600390C RID: 14604 RVA: 0x000C29B6 File Offset: 0x000C0BB6
	public Vector2 SpriteRendererSize
	{
		get
		{
			return this.m_spriteRendererSize;
		}
	}

	// Token: 0x1700141C RID: 5148
	// (get) Token: 0x0600390D RID: 14605 RVA: 0x000C29BE File Offset: 0x000C0BBE
	public DecoSpawnData[][] DecoSpawnData
	{
		get
		{
			return this.m_decoSpawnDataArray;
		}
	}

	// Token: 0x1700141D RID: 5149
	// (get) Token: 0x0600390E RID: 14606 RVA: 0x000C29C6 File Offset: 0x000C0BC6
	public IRelayLink BeforePropInstanceInitializedRelay
	{
		get
		{
			return this.m_beforePropInstanceInitializedRelay.link;
		}
	}

	// Token: 0x1700141E RID: 5150
	// (get) Token: 0x0600390F RID: 14607 RVA: 0x000C29D3 File Offset: 0x000C0BD3
	public IRelayLink OnPropInstanceInitializedRelay
	{
		get
		{
			return this.m_onPropInstanceInitializedRelay.link;
		}
	}

	// Token: 0x1700141F RID: 5151
	// (get) Token: 0x06003910 RID: 14608 RVA: 0x000C29E0 File Offset: 0x000C0BE0
	// (set) Token: 0x06003911 RID: 14609 RVA: 0x000C29E8 File Offset: 0x000C0BE8
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

	// Token: 0x17001420 RID: 5152
	// (get) Token: 0x06003912 RID: 14610 RVA: 0x000C29F1 File Offset: 0x000C0BF1
	public bool IsBreakable
	{
		get
		{
			return this.PropInstance && this.PropInstance.Breakable;
		}
	}

	// Token: 0x17001421 RID: 5153
	// (get) Token: 0x06003913 RID: 14611 RVA: 0x000C2A12 File Offset: 0x000C0C12
	public int ActualSubLayer
	{
		get
		{
			return this.SubLayer + this.SubLayerMod;
		}
	}

	// Token: 0x17001422 RID: 5154
	// (get) Token: 0x06003914 RID: 14612 RVA: 0x000C2A21 File Offset: 0x000C0C21
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

	// Token: 0x17001423 RID: 5155
	// (get) Token: 0x06003915 RID: 14613 RVA: 0x000C2A3D File Offset: 0x000C0C3D
	// (set) Token: 0x06003916 RID: 14614 RVA: 0x000C2A45 File Offset: 0x000C0C45
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

	// Token: 0x17001424 RID: 5156
	// (get) Token: 0x06003917 RID: 14615 RVA: 0x000C2A4E File Offset: 0x000C0C4E
	// (set) Token: 0x06003918 RID: 14616 RVA: 0x000C2A56 File Offset: 0x000C0C56
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

	// Token: 0x17001425 RID: 5157
	// (get) Token: 0x06003919 RID: 14617 RVA: 0x000C2A5F File Offset: 0x000C0C5F
	public bool ShouldSpawn
	{
		get
		{
			return !this.SpawnLogicController || (this.SpawnLogicController.ShouldSpawn && base.gameObject.activeSelf);
		}
	}

	// Token: 0x17001426 RID: 5158
	// (get) Token: 0x0600391A RID: 14618 RVA: 0x000C2A8A File Offset: 0x000C0C8A
	// (set) Token: 0x0600391B RID: 14619 RVA: 0x000C2A92 File Offset: 0x000C0C92
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

	// Token: 0x17001427 RID: 5159
	// (get) Token: 0x0600391C RID: 14620 RVA: 0x000C2AA2 File Offset: 0x000C0CA2
	// (set) Token: 0x0600391D RID: 14621 RVA: 0x000C2AAA File Offset: 0x000C0CAA
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

	// Token: 0x17001428 RID: 5160
	// (get) Token: 0x0600391E RID: 14622 RVA: 0x000C2ABA File Offset: 0x000C0CBA
	// (set) Token: 0x0600391F RID: 14623 RVA: 0x000C2AC2 File Offset: 0x000C0CC2
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

	// Token: 0x17001429 RID: 5161
	// (get) Token: 0x06003920 RID: 14624 RVA: 0x000C2ACB File Offset: 0x000C0CCB
	// (set) Token: 0x06003921 RID: 14625 RVA: 0x000C2AD3 File Offset: 0x000C0CD3
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

	// Token: 0x06003922 RID: 14626 RVA: 0x000C2B0D File Offset: 0x000C0D0D
	public void ForcePropPrefab(Prop propPrefab)
	{
		this.PropPrefab = propPrefab;
	}

	// Token: 0x1700142A RID: 5162
	// (get) Token: 0x06003923 RID: 14627 RVA: 0x000C2B16 File Offset: 0x000C0D16
	public Prop[] BreakableDecoInstances
	{
		get
		{
			return this.m_breakableDecoPropInstances;
		}
	}

	// Token: 0x1700142B RID: 5163
	// (get) Token: 0x06003924 RID: 14628 RVA: 0x000C2B1E File Offset: 0x000C0D1E
	public BiomePropsEntry[] PropTable
	{
		get
		{
			return this.Data.PropsPerBiome;
		}
	}

	// Token: 0x1700142C RID: 5164
	// (get) Token: 0x06003925 RID: 14629 RVA: 0x000C2B2B File Offset: 0x000C0D2B
	public Prop[] DefaultProps
	{
		get
		{
			return this.Data.DefaultProps;
		}
	}

	// Token: 0x1700142D RID: 5165
	// (get) Token: 0x06003926 RID: 14630 RVA: 0x000C2B38 File Offset: 0x000C0D38
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x1700142E RID: 5166
	// (get) Token: 0x06003927 RID: 14631 RVA: 0x000C2B40 File Offset: 0x000C0D40
	// (set) Token: 0x06003928 RID: 14632 RVA: 0x000C2B63 File Offset: 0x000C0D63
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

	// Token: 0x1700142F RID: 5167
	// (get) Token: 0x06003929 RID: 14633 RVA: 0x000C2B6C File Offset: 0x000C0D6C
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

	// Token: 0x17001430 RID: 5168
	// (get) Token: 0x0600392A RID: 14634 RVA: 0x000C2B88 File Offset: 0x000C0D88
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

	// Token: 0x17001431 RID: 5169
	// (get) Token: 0x0600392B RID: 14635 RVA: 0x000C2BA4 File Offset: 0x000C0DA4
	// (set) Token: 0x0600392C RID: 14636 RVA: 0x000C2BAC File Offset: 0x000C0DAC
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

	// Token: 0x17001432 RID: 5170
	// (get) Token: 0x0600392D RID: 14637 RVA: 0x000C2BB5 File Offset: 0x000C0DB5
	// (set) Token: 0x0600392E RID: 14638 RVA: 0x000C2BBD File Offset: 0x000C0DBD
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

	// Token: 0x17001433 RID: 5171
	// (get) Token: 0x0600392F RID: 14639 RVA: 0x000C2BC6 File Offset: 0x000C0DC6
	// (set) Token: 0x06003930 RID: 14640 RVA: 0x000C2BCE File Offset: 0x000C0DCE
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

	// Token: 0x17001434 RID: 5172
	// (get) Token: 0x06003931 RID: 14641 RVA: 0x000C2BD7 File Offset: 0x000C0DD7
	// (set) Token: 0x06003932 RID: 14642 RVA: 0x000C2BDF File Offset: 0x000C0DDF
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

	// Token: 0x17001435 RID: 5173
	// (get) Token: 0x06003933 RID: 14643 RVA: 0x000C2BE8 File Offset: 0x000C0DE8
	// (set) Token: 0x06003934 RID: 14644 RVA: 0x000C2BF0 File Offset: 0x000C0DF0
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

	// Token: 0x17001436 RID: 5174
	// (get) Token: 0x06003935 RID: 14645 RVA: 0x000C2BF9 File Offset: 0x000C0DF9
	// (set) Token: 0x06003936 RID: 14646 RVA: 0x000C2C01 File Offset: 0x000C0E01
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

	// Token: 0x17001437 RID: 5175
	// (get) Token: 0x06003937 RID: 14647 RVA: 0x000C2C0A File Offset: 0x000C0E0A
	// (set) Token: 0x06003938 RID: 14648 RVA: 0x000C2C12 File Offset: 0x000C0E12
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

	// Token: 0x17001438 RID: 5176
	// (get) Token: 0x06003939 RID: 14649 RVA: 0x000C2C1B File Offset: 0x000C0E1B
	// (set) Token: 0x0600393A RID: 14650 RVA: 0x000C2C23 File Offset: 0x000C0E23
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

	// Token: 0x0600393B RID: 14651 RVA: 0x000C2C2C File Offset: 0x000C0E2C
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

	// Token: 0x0600393C RID: 14652 RVA: 0x000C2C84 File Offset: 0x000C0E84
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

	// Token: 0x0600393D RID: 14653 RVA: 0x000C2D94 File Offset: 0x000C0F94
	public void Mirror()
	{
		this.IsMirrored = true;
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x000C2DA0 File Offset: 0x000C0FA0
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

	// Token: 0x0600393F RID: 14655 RVA: 0x000C2F60 File Offset: 0x000C1160
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

	// Token: 0x06003940 RID: 14656 RVA: 0x000C2FCB File Offset: 0x000C11CB
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

	// Token: 0x06003941 RID: 14657 RVA: 0x000C3008 File Offset: 0x000C1208
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

	// Token: 0x06003942 RID: 14658 RVA: 0x000C30C0 File Offset: 0x000C12C0
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

	// Token: 0x06003943 RID: 14659 RVA: 0x000C31DC File Offset: 0x000C13DC
	public void SetCameraLayer(CameraLayer value)
	{
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x000C31DE File Offset: 0x000C13DE
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x000C31E7 File Offset: 0x000C13E7
	public void SetSubLayer(int subLayer, bool isDeco = false)
	{
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x000C321D File Offset: 0x000C141D
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002C01 RID: 11265
	[SerializeField]
	private bool m_override;

	// Token: 0x04002C02 RID: 11266
	[SerializeField]
	private Prop m_overrideProp;

	// Token: 0x04002C03 RID: 11267
	[SerializeField]
	private PropSpawnControllerData m_spawnControllerData;

	// Token: 0x04002C04 RID: 11268
	[SerializeField]
	private bool m_isOverrideSubLayer;

	// Token: 0x04002C05 RID: 11269
	[SerializeField]
	private int m_subLayerOverride;

	// Token: 0x04002C06 RID: 11270
	[SerializeField]
	private bool m_isOverrideSubLayerMod;

	// Token: 0x04002C07 RID: 11271
	[SerializeField]
	private int m_subLayerModOverride;

	// Token: 0x04002C08 RID: 11272
	[SerializeField]
	private bool m_isOverrideCameraLayer;

	// Token: 0x04002C09 RID: 11273
	[SerializeField]
	private bool m_isOverrideZPosition;

	// Token: 0x04002C0A RID: 11274
	[SerializeField]
	private float m_zPositionOverride;

	// Token: 0x04002C0B RID: 11275
	[SerializeField]
	private CameraLayer m_cameraLayerOverride;

	// Token: 0x04002C0C RID: 11276
	[SerializeField]
	private bool m_disableCulling;

	// Token: 0x04002C0D RID: 11277
	[SerializeField]
	private SpriteDrawMode m_spriteRendererDrawMode;

	// Token: 0x04002C0E RID: 11278
	[SerializeField]
	private Vector2 m_spriteRendererSize;

	// Token: 0x04002C0F RID: 11279
	private static List<Prop> m_breakableDecoPropsHelper_STATIC = new List<Prop>();

	// Token: 0x04002C10 RID: 11280
	private Prop m_propInstance;

	// Token: 0x04002C11 RID: 11281
	private Prop m_propPrefab;

	// Token: 0x04002C12 RID: 11282
	private int m_cachedPropPrefabNameHash;

	// Token: 0x04002C13 RID: 11283
	private Prop[] m_breakableDecoPropInstances;

	// Token: 0x04002C14 RID: 11284
	private bool m_breakableDecoPropsArrayInitialized;

	// Token: 0x04002C15 RID: 11285
	private BaseRoom m_room;

	// Token: 0x04002C16 RID: 11286
	private SpawnLogicController m_spawnLogicController;

	// Token: 0x04002C17 RID: 11287
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04002C18 RID: 11288
	private bool m_isMirrored;

	// Token: 0x04002C19 RID: 11289
	private Relay m_onPropInstanceInitializedRelay = new Relay();

	// Token: 0x04002C1A RID: 11290
	private Relay m_beforePropInstanceInitializedRelay = new Relay();

	// Token: 0x04002C1B RID: 11291
	private bool m_hasCheckedForSpawnLogicController;

	// Token: 0x04002C1C RID: 11292
	private DecoSpawnData[][] m_decoSpawnDataArray;

	// Token: 0x04002C1D RID: 11293
	private static Dictionary<Prop, DecoController[]> m_propPrefabDecoControllers_STATIC = new Dictionary<Prop, DecoController[]>();
}
