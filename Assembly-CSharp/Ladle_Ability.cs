using System;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class Ladle_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x060017B9 RID: 6073 RVA: 0x0000BFF5 File Offset: 0x0000A1F5
	protected override void Awake()
	{
		base.Awake();
		this.m_onProjectileCollision = new Action<Projectile_RL, GameObject>(this.OnProjectileCollision);
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x060017BA RID: 6074 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x060017BB RID: 6075 RVA: 0x0000C00F File Offset: 0x0000A20F
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x060017BC RID: 6076 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x060017BD RID: 6077 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x060017BE RID: 6078 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x060017BF RID: 6079 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x060017C0 RID: 6080 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x060017C2 RID: 6082 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x0000C016 File Offset: 0x0000A216
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		abilityController.Animator.SetBool("FryingPan_UsePirateAudio", false);
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x0000C031 File Offset: 0x0000A231
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_onProjectileCollision, false);
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x0008C4C8 File Offset: 0x0008A6C8
	private void OnProjectileCollision(Projectile_RL proj, GameObject colliderObj)
	{
		if (CollisionType_RL.IsProjectile(colliderObj))
		{
			this.m_abilityController.PlayerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, 3f, null);
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
			}
		}
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x0000C051 File Offset: 0x0000A251
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001748 RID: 5960
	private Action<Projectile_RL, GameObject> m_onProjectileCollision;
}
