using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class GroundBowProjectile_RL : Projectile_RL
{
	// Token: 0x06002B19 RID: 11033 RVA: 0x000920AC File Offset: 0x000902AC
	public override void WeaponOnEnterHitResponse(IHitboxController otherHBController)
	{
		bool flag = false;
		if (CollisionType_RL.IsProjectile(otherHBController.RootGameObject))
		{
			Projectile_RL component = otherHBController.RootGameObject.GetComponent<Projectile_RL>();
			if (!component || !Projectile_RL.CollisionFlagAllowed(this, component))
			{
				return;
			}
			if (component && base.CompareTag("PlayerProjectile") && component.CompareTag("EnemyProjectile") && component.DieOnCharacterCollision)
			{
				flag = true;
			}
		}
		if (base.Owner == otherHBController.RootGameObject && !this.CanHitOwner)
		{
			return;
		}
		this.m_onCollisionRelay.Dispatch(this, otherHBController.RootGameObject);
		if (this.DieOnCharacterCollision)
		{
			bool flag2 = true;
			if (otherHBController.CollisionType == CollisionType.FlimsyBreakable)
			{
				flag2 = false;
			}
			if (flag)
			{
				flag2 = false;
			}
			if (flag2)
			{
				this.m_collidedObj = otherHBController.RootGameObject;
				base.FlagForDestruction(null);
				this.m_destructionType = Projectile_RL.ProjectileDestructionType.CharacterCollision;
				if (otherHBController.RootGameObject.CompareTag("Player") && PlayerManager.GetPlayerController().CharacterDash.IsDashing && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0)
				{
					this.m_destructionType = Projectile_RL.ProjectileDestructionType.VoidDashCollision;
				}
			}
		}
		if (base.Owner && PlayerManager.GetPlayerController())
		{
			GameObject rootGameObject = otherHBController.RootGameObject;
			EnemyController component2 = rootGameObject.GetComponent<EnemyController>();
			if (component2 && (component2.RicochetsAttackerOnHit || base.RicochetsOwnerWhenHits) && base.CanBeRicocheted)
			{
				base.RicochetPlayer(null, rootGameObject);
			}
		}
		base.PerformHitEffectCheck(otherHBController);
	}
}
