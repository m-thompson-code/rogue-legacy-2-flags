using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class DragonPistolWeapon_Ability : PistolWeapon_Ability
{
	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002D59C File Offset: 0x0002B79C
	protected override bool CanManuallyReload
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0002D59F File Offset: 0x0002B79F
	public override string ProjectileName
	{
		get
		{
			if (base.CurrentAmmo > 5)
			{
				return base.ProjectileName;
			}
			if (base.CurrentAmmo > 0)
			{
				return this.m_fireballProjectileName;
			}
			return this.m_emptyClipProjectileName;
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0002D5C7 File Offset: 0x0002B7C7
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_fireballProjectileName,
			this.m_emptyClipProjectileName
		};
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0002D5F0 File Offset: 0x0002B7F0
	protected override void FireProjectile()
	{
		if (base.CurrentAmmo > 5)
		{
			this.m_shootAudioEmitter.Play();
		}
		else if (base.CurrentAmmo <= 0)
		{
			this.m_emptyClipAudioEmitter.Play();
		}
		base.Base_FireProjectile();
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
		if (level > 0)
		{
			this.m_firedProjectile.LifespanTimer += (float)level * 0f;
		}
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x0002D660 File Offset: 0x0002B860
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		base.CurrentAmmo = base.MaxAmmo;
	}

	// Token: 0x04001168 RID: 4456
	[SerializeField]
	private string m_fireballProjectileName;
}
