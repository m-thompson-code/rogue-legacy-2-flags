using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A3B RID: 2619
public abstract class HazardSpawnControllerBase : ComplexSpawnController, IHazardSpawnController, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray
{
	// Token: 0x17001B3D RID: 6973
	// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x0012E9F4 File Offset: 0x0012CBF4
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				List<string> list = new List<string>();
				Hazard prefab = HazardLibrary.GetPrefab(this.Type);
				if (prefab)
				{
					IHasProjectileNameArray[] componentsInChildren = prefab.GetComponentsInChildren<IHasProjectileNameArray>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						foreach (string item in componentsInChildren[i].ProjectileNameArray)
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
				this.m_projectileNameArray = list.ToArray();
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17001B3E RID: 6974
	// (get) Token: 0x06004EF5 RID: 20213
	public abstract HazardCategory Category { get; }

	// Token: 0x17001B3F RID: 6975
	// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x0002B0F7 File Offset: 0x000292F7
	// (set) Token: 0x06004EF7 RID: 20215 RVA: 0x0002B0FF File Offset: 0x000292FF
	public IHazard Hazard { get; protected set; }

	// Token: 0x17001B40 RID: 6976
	// (get) Token: 0x06004EF8 RID: 20216 RVA: 0x0002B108 File Offset: 0x00029308
	// (set) Token: 0x06004EF9 RID: 20217 RVA: 0x0002B110 File Offset: 0x00029310
	public StateID InitialState
	{
		get
		{
			return this.m_initialState;
		}
		private set
		{
			this.m_initialState = value;
		}
	}

	// Token: 0x17001B41 RID: 6977
	// (get) Token: 0x06004EFA RID: 20218 RVA: 0x0002B119 File Offset: 0x00029319
	// (set) Token: 0x06004EFB RID: 20219 RVA: 0x0002B121 File Offset: 0x00029321
	public HazardType Type
	{
		get
		{
			return this.m_type;
		}
		private set
		{
			this.m_type = value;
		}
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x0012EA84 File Offset: 0x0012CC84
	private void Start()
	{
		SpriteRenderer component = base.gameObject.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x0012EAB0 File Offset: 0x0012CCB0
	protected void CreateHazard()
	{
		this.Hazard = this.GetHazard(this.Type);
		if (!this.Hazard.IsNativeNull())
		{
			this.Hazard.gameObject.SetActive(true);
			this.Hazard.gameObject.transform.position = base.transform.position;
			this.Hazard.gameObject.transform.rotation = base.transform.rotation;
			this.Hazard.gameObject.transform.localScale = base.transform.localScale;
			IRoomConsumer[] componentsInChildren = this.Hazard.gameObject.GetComponentsInChildren<IRoomConsumer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetRoom(base.Room);
			}
		}
	}

	// Token: 0x06004EFE RID: 20222 RVA: 0x0002B12A File Offset: 0x0002932A
	public override void SetRoom(BaseRoom room)
	{
		base.SetRoom(room);
		if (GameUtility.IsInLevelEditor)
		{
			this.SetSpawnType();
		}
	}

	// Token: 0x06004EFF RID: 20223 RVA: 0x0012EB80 File Offset: 0x0012CD80
	public void SetSpawnType()
	{
		if (this.Type == HazardType.BiomeSpecific)
		{
			HazardType[] array = HazardLibrary.GetDefaultHazardTypes(this.Category);
			if (HazardLibrary.GetHazardsInBiomeTable(this.Category).ContainsKey(base.Room.AppearanceBiomeType))
			{
				array = HazardLibrary.GetHazardsInBiomeTable(this.Category)[base.Room.AppearanceBiomeType];
			}
			int num = 0;
			if (array.Length > 1)
			{
				num = RNGManager.GetRandomNumber(RngID.Hazards_RoomSeed, "Get Random Hazard Index", 0, array.Length);
			}
			HazardType hazardType = array[num];
			if (hazardType == HazardType.Orbiter && base.Room.AppearanceBiomeType == BiomeType.Castle && BurdenManager.IsBurdenActive(BurdenType.CastleBiomeUp))
			{
				hazardType = HazardType.Triple_Orbiter;
			}
			if (hazardType == HazardType.IceCrystal && base.Room.AppearanceBiomeType == BiomeType.Forest && BurdenManager.IsBurdenActive(BurdenType.ForestBiomeUp))
			{
				hazardType = HazardType.SentryWithIce;
			}
			this.SetType(hazardType);
		}
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x0002B140 File Offset: 0x00029340
	protected virtual IHazard GetHazard(HazardType hazardType)
	{
		return HazardManager.GetHazard(this.Type);
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetInitialState(StateID state)
	{
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x0002B14D File Offset: 0x0002934D
	public void SetType(HazardType hazardType)
	{
		this.Type = hazardType;
	}

	// Token: 0x06004F04 RID: 20228 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003C00 RID: 15360
	[SerializeField]
	[ReadOnly]
	private HazardType m_type;

	// Token: 0x04003C01 RID: 15361
	[SerializeField]
	[ReadOnly]
	private StateID m_initialState;

	// Token: 0x04003C02 RID: 15362
	protected HazardArgs m_hazardArgs;

	// Token: 0x04003C03 RID: 15363
	[NonSerialized]
	private string[] m_projectileNameArray;
}
