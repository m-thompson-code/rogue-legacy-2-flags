using System;
using System.Linq;

// Token: 0x02000493 RID: 1171
public class ApplySporeBurstOnPlatformProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B1B RID: 11035 RVA: 0x00092215 File Offset: 0x00090415
	protected override void Awake()
	{
		base.Awake();
		this.m_onExitLanded = new Action<CorgiController_RL>(this.OnExitLanded);
		this.m_onEnterLanded = new Action<CorgiController_RL>(this.OnEnterLanded);
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x00092241 File Offset: 0x00090441
	private void OnEnable()
	{
		if (base.SourceProjectile.IsPersistentProjectile)
		{
			PlayerManager.GetPlayerController().ControllerCorgi.OnCorgiLandedEnterRelay.AddListener(this.m_onEnterLanded, false);
		}
		this.PerformSporeBurstCheck();
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x00092272 File Offset: 0x00090472
	private void OnDisable()
	{
		if (!GameManager.IsApplicationClosing && base.SourceProjectile.IsPersistentProjectile)
		{
			PlayerManager.GetPlayerController().ControllerCorgi.OnCorgiLandedEnterRelay.RemoveListener(this.m_onEnterLanded);
		}
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x000922A4 File Offset: 0x000904A4
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

	// Token: 0x06002B1F RID: 11039 RVA: 0x00092344 File Offset: 0x00090544
	private void OnEnterLanded(CorgiController_RL corgi)
	{
		this.PerformSporeBurstCheck();
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x0009234C File Offset: 0x0009054C
	private void OnExitLanded(CorgiController_RL corgi)
	{
		PlayerManager.GetPlayerController().ControllerCorgi.OnCorgiLandedExitRelay.RemoveListener(this.m_onExitLanded);
		base.SourceProjectile.RemoveStatusEffect(StatusEffectType.Enemy_SporeBurst);
	}

	// Token: 0x0400231A RID: 8986
	private static string[] m_platformExclusionArray = new string[]
	{
		"CrowsNestPlatformTalentProjectile"
	};

	// Token: 0x0400231B RID: 8987
	private Action<CorgiController_RL> m_onExitLanded;

	// Token: 0x0400231C RID: 8988
	private Action<CorgiController_RL> m_onEnterLanded;
}
