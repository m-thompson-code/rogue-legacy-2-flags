using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class TurretHazardSpawnController : LineHazardSpawnController, IHazardSpawnController, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray, IWidthConsumer, IMirror, IPivotConsumer, ITurretSettingsConsumer
{
	// Token: 0x17001495 RID: 5269
	// (get) Token: 0x06003A48 RID: 14920 RVA: 0x000C5C9D File Offset: 0x000C3E9D
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Turret;
		}
	}

	// Token: 0x17001496 RID: 5270
	// (get) Token: 0x06003A49 RID: 14921 RVA: 0x000C5CA1 File Offset: 0x000C3EA1
	public float InitialFireDelay
	{
		get
		{
			return this.m_initialFireDelay;
		}
	}

	// Token: 0x17001497 RID: 5271
	// (get) Token: 0x06003A4A RID: 14922 RVA: 0x000C5CA9 File Offset: 0x000C3EA9
	public TurretLogicType LogicType
	{
		get
		{
			return this.m_logicType;
		}
	}

	// Token: 0x17001498 RID: 5272
	// (get) Token: 0x06003A4B RID: 14923 RVA: 0x000C5CB1 File Offset: 0x000C3EB1
	public float LoopFireDelay
	{
		get
		{
			return this.m_loopFireDelay;
		}
	}

	// Token: 0x17001499 RID: 5273
	// (get) Token: 0x06003A4C RID: 14924 RVA: 0x000C5CB9 File Offset: 0x000C3EB9
	public float ProjectileSpeedMod
	{
		get
		{
			return this.m_projectileSpeedMod;
		}
	}

	// Token: 0x1700149A RID: 5274
	// (get) Token: 0x06003A4D RID: 14925 RVA: 0x000C5CC1 File Offset: 0x000C3EC1
	public override string SpriteSheetPath
	{
		get
		{
			return "Assets/Content/Textures/Hazards/SharedHazards_Packed/TurretHazard_Editor_Texture.png";
		}
	}

	// Token: 0x1700149B RID: 5275
	// (get) Token: 0x06003A4E RID: 14926 RVA: 0x000C5CC8 File Offset: 0x000C3EC8
	public bool UseHalfLoopDelay
	{
		get
		{
			return this.m_useHalfLoopDelay;
		}
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x000C5CD0 File Offset: 0x000C3ED0
	public void SetInitialFireDelay(float value)
	{
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x000C5CD2 File Offset: 0x000C3ED2
	public void SetLogic(TurretLogicType value)
	{
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x000C5CD4 File Offset: 0x000C3ED4
	public void SetLoopFireDelay(float value)
	{
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x000C5CD6 File Offset: 0x000C3ED6
	public void SetProjectileSpeedMod(float value)
	{
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x000C5CD8 File Offset: 0x000C3ED8
	public void SetUseHalfLoopDelay(bool value)
	{
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x000C5CDC File Offset: 0x000C3EDC
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

	// Token: 0x06003A56 RID: 14934 RVA: 0x000C5D74 File Offset: 0x000C3F74
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002CBC RID: 11452
	[SerializeField]
	private bool m_useHalfLoopDelay;

	// Token: 0x04002CBD RID: 11453
	[SerializeField]
	private TurretLogicType m_logicType;

	// Token: 0x04002CBE RID: 11454
	[SerializeField]
	private float m_initialFireDelay;

	// Token: 0x04002CBF RID: 11455
	[SerializeField]
	private float m_loopFireDelay = 1f;

	// Token: 0x04002CC0 RID: 11456
	[SerializeField]
	private float m_projectileSpeedMod = 1f;
}
