using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public abstract class HazardSpawnControllerBase : ComplexSpawnController, IHazardSpawnController, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray
{
	// Token: 0x170013E6 RID: 5094
	// (get) Token: 0x06003850 RID: 14416 RVA: 0x000C0644 File Offset: 0x000BE844
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

	// Token: 0x170013E7 RID: 5095
	// (get) Token: 0x06003851 RID: 14417
	public abstract HazardCategory Category { get; }

	// Token: 0x170013E8 RID: 5096
	// (get) Token: 0x06003852 RID: 14418 RVA: 0x000C06D1 File Offset: 0x000BE8D1
	// (set) Token: 0x06003853 RID: 14419 RVA: 0x000C06D9 File Offset: 0x000BE8D9
	public IHazard Hazard { get; protected set; }

	// Token: 0x170013E9 RID: 5097
	// (get) Token: 0x06003854 RID: 14420 RVA: 0x000C06E2 File Offset: 0x000BE8E2
	// (set) Token: 0x06003855 RID: 14421 RVA: 0x000C06EA File Offset: 0x000BE8EA
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

	// Token: 0x170013EA RID: 5098
	// (get) Token: 0x06003856 RID: 14422 RVA: 0x000C06F3 File Offset: 0x000BE8F3
	// (set) Token: 0x06003857 RID: 14423 RVA: 0x000C06FB File Offset: 0x000BE8FB
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

	// Token: 0x06003858 RID: 14424 RVA: 0x000C0704 File Offset: 0x000BE904
	private void Start()
	{
		SpriteRenderer component = base.gameObject.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x000C0730 File Offset: 0x000BE930
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

	// Token: 0x0600385A RID: 14426 RVA: 0x000C07FD File Offset: 0x000BE9FD
	public override void SetRoom(BaseRoom room)
	{
		base.SetRoom(room);
		if (GameUtility.IsInLevelEditor)
		{
			this.SetSpawnType();
		}
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x000C0814 File Offset: 0x000BEA14
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

	// Token: 0x0600385C RID: 14428 RVA: 0x000C08E4 File Offset: 0x000BEAE4
	protected virtual IHazard GetHazard(HazardType hazardType)
	{
		return HazardManager.GetHazard(this.Type);
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x000C08F1 File Offset: 0x000BEAF1
	public void SetInitialState(StateID state)
	{
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x000C08F3 File Offset: 0x000BEAF3
	public void SetType(HazardType hazardType)
	{
		this.Type = hazardType;
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x000C0904 File Offset: 0x000BEB04
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002B93 RID: 11155
	[SerializeField]
	[ReadOnly]
	private HazardType m_type;

	// Token: 0x04002B94 RID: 11156
	[SerializeField]
	[ReadOnly]
	private StateID m_initialState;

	// Token: 0x04002B95 RID: 11157
	protected HazardArgs m_hazardArgs;

	// Token: 0x04002B96 RID: 11158
	[NonSerialized]
	private string[] m_projectileNameArray;
}
