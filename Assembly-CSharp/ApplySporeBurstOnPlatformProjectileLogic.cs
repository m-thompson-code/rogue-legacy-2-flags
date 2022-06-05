using System;
using System.Linq;

// Token: 0x02000796 RID: 1942
public class ApplySporeBurstOnPlatformProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B64 RID: 15204 RVA: 0x0002094C File Offset: 0x0001EB4C
	protected override void Awake()
	{
		base.Awake();
		this.m_onExitLanded = new Action<CorgiController_RL>(this.OnExitLanded);
		this.m_onEnterLanded = new Action<CorgiController_RL>(this.OnEnterLanded);
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x00020978 File Offset: 0x0001EB78
	private void OnEnable()
	{
		if (base.SourceProjectile.IsPersistentProjectile)
		{
			PlayerManager.GetPlayerController().ControllerCorgi.OnCorgiLandedEnterRelay.AddListener(this.m_onEnterLanded, false);
		}
		this.PerformSporeBurstCheck();
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x000209A9 File Offset: 0x0001EBA9
	private void OnDisable()
	{
		if (!GameManager.IsApplicationClosing && base.SourceProjectile.IsPersistentProjectile)
		{
			PlayerManager.GetPlayerController().ControllerCorgi.OnCorgiLandedEnterRelay.RemoveListener(this.m_onEnterLanded);
		}
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x000F3D78 File Offset: 0x000F1F78
	private void PerformSporeBurstCheck()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.IsGrounded && playerController.ControllerCorgi.StandingOn && playerController.ControllerCorgi.StandingOn.CompareTag("PlayerProjectile") && !ApplySporeBurstOnPlatformProjectileLogic.m_platformExclusionArray.Contains(playerController.ControllerCorgi.StandingOn.GetRoot(false).name))
		{
			if (base.SourceProjectile.IsPersistentProjectile)
			{
				playerController.ControllerCorgi.OnCorgiLandedExitRelay.AddListener(this.m_onExitLanded, false);
			}
			base.SourceProjectile.AttachStatusEffect(StatusEffectType.Enemy_SporeBurst, 0f);
		}
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x000209DA File Offset: 0x0001EBDA
	private void OnEnterLanded(CorgiController_RL corgi)
	{
		this.PerformSporeBurstCheck();
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x000209E2 File Offset: 0x0001EBE2
	private void OnExitLanded(CorgiController_RL corgi)
	{
		PlayerManager.GetPlayerController().ControllerCorgi.OnCorgiLandedExitRelay.RemoveListener(this.m_onExitLanded);
		base.SourceProjectile.RemoveStatusEffect(StatusEffectType.Enemy_SporeBurst);
	}

	// Token: 0x04002F2E RID: 12078
	private static string[] m_platformExclusionArray = new string[]
	{
		"CrowsNestPlatformTalentProjectile"
	};

	// Token: 0x04002F2F RID: 12079
	private Action<CorgiController_RL> m_onExitLanded;

	// Token: 0x04002F30 RID: 12080
	private Action<CorgiController_RL> m_onEnterLanded;
}
