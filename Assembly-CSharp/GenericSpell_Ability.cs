using System;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class GenericSpell_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x06001436 RID: 5174 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x06001437 RID: 5175 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06001438 RID: 5176 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06001439 RID: 5177 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x0600143A RID: 5178 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x0600143B RID: 5179 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x0600143C RID: 5180 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x0600143D RID: 5181 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x0600143E RID: 5182 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x0600143F RID: 5183 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x00086B6C File Offset: 0x00084D6C
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, this.m_projectileMatchFacing, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			if (!this.m_projectileMatchFacing && this.m_firedProjectile.transform.parent == this.m_abilityController.PlayerController.Pivot.transform)
			{
				this.m_firedProjectile.transform.SetParent(this.m_abilityController.PlayerController.transform, false);
			}
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x04001605 RID: 5637
	[SerializeField]
	private bool m_projectileMatchFacing = true;

	// Token: 0x04001606 RID: 5638
	[SerializeField]
	private float m_fireAngle;
}
