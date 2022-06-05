using System;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class DragonPistolWeapon_Ability : PistolWeapon_Ability
{
	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x0600171B RID: 5915 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool CanManuallyReload
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x0600171C RID: 5916 RVA: 0x0000BB56 File Offset: 0x00009D56
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

	// Token: 0x0600171D RID: 5917 RVA: 0x0000BB7E File Offset: 0x00009D7E
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_fireballProjectileName,
			this.m_emptyClipProjectileName
		};
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0008BE70 File Offset: 0x0008A070
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

	// Token: 0x0600171F RID: 5919 RVA: 0x0000BBA7 File Offset: 0x00009DA7
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		base.CurrentAmmo = base.MaxAmmo;
	}

	// Token: 0x04001711 RID: 5905
	[SerializeField]
	private string m_fireballProjectileName;
}
