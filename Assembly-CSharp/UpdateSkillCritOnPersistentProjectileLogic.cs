using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
public class UpdateSkillCritOnPersistentProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C4D RID: 15437 RVA: 0x00021540 File Offset: 0x0001F740
	protected override void Awake()
	{
		base.Awake();
		base.SourceProjectile.IsPersistentProjectile = true;
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x000F6434 File Offset: 0x000F4634
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

	// Token: 0x04002FD5 RID: 12245
	[SerializeField]
	private bool m_critsWhenDashing;
}
