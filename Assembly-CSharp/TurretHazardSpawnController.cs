using System;
using UnityEngine;

// Token: 0x02000A7D RID: 2685
public class TurretHazardSpawnController : LineHazardSpawnController, IHazardSpawnController, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray, IWidthConsumer, IMirror, IPivotConsumer, ITurretSettingsConsumer
{
	// Token: 0x17001BFC RID: 7164
	// (get) Token: 0x0600512D RID: 20781 RVA: 0x000054AD File Offset: 0x000036AD
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Turret;
		}
	}

	// Token: 0x17001BFD RID: 7165
	// (get) Token: 0x0600512E RID: 20782 RVA: 0x0002C565 File Offset: 0x0002A765
	public float InitialFireDelay
	{
		get
		{
			return this.m_initialFireDelay;
		}
	}

	// Token: 0x17001BFE RID: 7166
	// (get) Token: 0x0600512F RID: 20783 RVA: 0x0002C56D File Offset: 0x0002A76D
	public TurretLogicType LogicType
	{
		get
		{
			return this.m_logicType;
		}
	}

	// Token: 0x17001BFF RID: 7167
	// (get) Token: 0x06005130 RID: 20784 RVA: 0x0002C575 File Offset: 0x0002A775
	public float LoopFireDelay
	{
		get
		{
			return this.m_loopFireDelay;
		}
	}

	// Token: 0x17001C00 RID: 7168
	// (get) Token: 0x06005131 RID: 20785 RVA: 0x0002C57D File Offset: 0x0002A77D
	public float ProjectileSpeedMod
	{
		get
		{
			return this.m_projectileSpeedMod;
		}
	}

	// Token: 0x17001C01 RID: 7169
	// (get) Token: 0x06005132 RID: 20786 RVA: 0x0002C585 File Offset: 0x0002A785
	public override string SpriteSheetPath
	{
		get
		{
			return "Assets/Content/Textures/Hazards/SharedHazards_Packed/TurretHazard_Editor_Texture.png";
		}
	}

	// Token: 0x17001C02 RID: 7170
	// (get) Token: 0x06005133 RID: 20787 RVA: 0x0002C58C File Offset: 0x0002A78C
	public bool UseHalfLoopDelay
	{
		get
		{
			return this.m_useHalfLoopDelay;
		}
	}

	// Token: 0x06005134 RID: 20788 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetInitialFireDelay(float value)
	{
	}

	// Token: 0x06005135 RID: 20789 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetLogic(TurretLogicType value)
	{
	}

	// Token: 0x06005136 RID: 20790 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetLoopFireDelay(float value)
	{
	}

	// Token: 0x06005137 RID: 20791 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetProjectileSpeedMod(float value)
	{
	}

	// Token: 0x06005138 RID: 20792 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetUseHalfLoopDelay(bool value)
	{
	}

	// Token: 0x06005139 RID: 20793 RVA: 0x001339D0 File Offset: 0x00131BD0
	protected override void Spawn()
	{
		if (base.Type == HazardType.None)
		{
			return;
		}
		base.CreateHazard();
		if (this.m_hazardArgs == null)
		{
			this.m_hazardArgs = new TurretHazardArgs(base.InitialState, this.LogicType, this.UseHalfLoopDelay, this.InitialFireDelay, this.LoopFireDelay, this.ProjectileSpeedMod);
		}
		(base.Hazard as Multi_Hazard).Initialize(base.PivotPoint, base.Width, this.m_hazardArgs);
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003D52 RID: 15698
	[SerializeField]
	private bool m_useHalfLoopDelay;

	// Token: 0x04003D53 RID: 15699
	[SerializeField]
	private TurretLogicType m_logicType;

	// Token: 0x04003D54 RID: 15700
	[SerializeField]
	private float m_initialFireDelay;

	// Token: 0x04003D55 RID: 15701
	[SerializeField]
	private float m_loopFireDelay = 1f;

	// Token: 0x04003D56 RID: 15702
	[SerializeField]
	private float m_projectileSpeedMod = 1f;
}
