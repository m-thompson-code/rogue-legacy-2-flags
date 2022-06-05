using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class UpdateSkillCritOnPersistentProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002BBC RID: 11196 RVA: 0x00094C4A File Offset: 0x00092E4A
	protected override void Awake()
	{
		base.Awake();
		base.SourceProjectile.IsPersistentProjectile = true;
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x00094C60 File Offset: 0x00092E60
	private void FixedUpdate()
	{
		bool flag = false;
		bool flag2 = base.SourceProjectile.ActualCritChance >= 100f;
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_FreeCrit))
		{
			flag = true;
		}
		if (!flag && this.m_critsWhenDashing && playerController.MovementState == CharacterStates.MovementStates.Dashing)
		{
			flag = true;
		}
		if (!flag && playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Combo))
		{
			BaseStatusEffect statusEffect = playerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_Combo);
			if (statusEffect && statusEffect.IsPlaying && statusEffect.TimesStacked >= 15)
			{
				flag = true;
			}
		}
		if (flag && !flag2)
		{
			base.SourceProjectile.ActualCritChance += 100f;
			base.SourceProjectile.ChangeSpriteRendererColor(ProjectileLibrary.GetBuffColor(ProjectileBuffType.PlayerDashAttack));
			return;
		}
		if (!flag && flag2)
		{
			base.SourceProjectile.ActualCritChance -= 100f;
			base.SourceProjectile.ResetSpriteRendererColor();
		}
	}

	// Token: 0x04002388 RID: 9096
	[SerializeField]
	private bool m_critsWhenDashing;
}
