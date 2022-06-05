using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class ExplosiveHands_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002D849 File Offset: 0x0002BA49
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0002D850 File Offset: 0x0002BA50
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0002D857 File Offset: 0x0002BA57
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0002D85E File Offset: 0x0002BA5E
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0002D865 File Offset: 0x0002BA65
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0002D86C File Offset: 0x0002BA6C
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0002D873 File Offset: 0x0002BA73
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0002D87A File Offset: 0x0002BA7A
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0002D881 File Offset: 0x0002BA81
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0002D888 File Offset: 0x0002BA88
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0002D88F File Offset: 0x0002BA8F
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.m_useAltCast)
			{
				return this.m_altSpellCast;
			}
			return base.AbilityTellIntroName;
		}
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0002D8A6 File Offset: 0x0002BAA6
	public override void PreCastAbility()
	{
		if (Time.time - this.m_timeSinceLastCast < 1f)
		{
			this.m_useAltCast = !this.m_useAltCast;
		}
		else
		{
			this.m_useAltCast = false;
		}
		this.m_timeSinceLastCast = Time.time;
		base.PreCastAbility();
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0002D8E4 File Offset: 0x0002BAE4
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, this.m_projectileMatchFacing, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x04001172 RID: 4466
	[SerializeField]
	private string m_altSpellCast;

	// Token: 0x04001173 RID: 4467
	[SerializeField]
	private bool m_projectileMatchFacing = true;

	// Token: 0x04001174 RID: 4468
	[SerializeField]
	private float m_fireAngle;

	// Token: 0x04001175 RID: 4469
	private bool m_useAltCast;

	// Token: 0x04001176 RID: 4470
	private float m_timeSinceLastCast;
}
