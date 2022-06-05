using System;
using FMODUnity;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class Reload_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06001565 RID: 5477 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06001566 RID: 5478 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06001567 RID: 5479 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06001568 RID: 5480 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06001569 RID: 5481 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x0600156A RID: 5482 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x0600156B RID: 5483 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x0600156C RID: 5484 RVA: 0x00089E70 File Offset: 0x00088070
	protected override float AttackAnimExitDelay
	{
		get
		{
			float num = 0.6f;
			BaseAbility_RL ability = this.m_abilityController.PlayerController.CastAbility.GetAbility(CastAbilityType.Weapon, true);
			if (ability && ability.AbilityType == AbilityType.PistolWeapon)
			{
				int currentAmmo = ability.CurrentAmmo;
				int maxAmmo = ability.MaxAmmo;
				if (maxAmmo > 0)
				{
					float num2 = 1f - (float)currentAmmo / (float)maxAmmo;
					num = Mathf.Max(0.1f, num * num2);
				}
			}
			return num;
		}
	}

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x0600156D RID: 5485 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x0600156E RID: 5486 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0000AA65 File Offset: 0x00008C65
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_reloadLoopEmitter.Play();
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00089EDC File Offset: 0x000880DC
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		this.m_reloadLoopEmitter.Stop();
		if (!abilityInterrupted)
		{
			this.m_abilityController.ResetAbilityAmmo(CastAbilityType.Weapon, true);
			this.m_abilityController.PlayerController.BlinkPulseEffect.StartSingleBlinkEffect(Color.white);
			this.m_reloadSuccessEmitter.Play();
			string @string;
			if (UnityEngine.Random.Range(0f, 1f) < 0.025f)
			{
				@string = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_RELOAD_2", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
			else
			{
				@string = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_RELOAD_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
			TextPopupManager.DisplayTextDefaultPos(TextPopupType.DownstrikeAmmoGain, @string, this.m_abilityController.PlayerController, true, true);
		}
	}

	// Token: 0x04001677 RID: 5751
	[SerializeField]
	private StudioEventEmitter m_reloadSuccessEmitter;

	// Token: 0x04001678 RID: 5752
	[SerializeField]
	private StudioEventEmitter m_reloadLoopEmitter;
}
