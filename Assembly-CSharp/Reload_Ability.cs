using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class Reload_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0002AD49 File Offset: 0x00028F49
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0002AD50 File Offset: 0x00028F50
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0002AD57 File Offset: 0x00028F57
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0002AD5E File Offset: 0x00028F5E
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0002AD65 File Offset: 0x00028F65
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x0002AD6C File Offset: 0x00028F6C
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0002AD73 File Offset: 0x00028F73
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0002AD7C File Offset: 0x00028F7C
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

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0002ADE8 File Offset: 0x00028FE8
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x06000DED RID: 3565 RVA: 0x0002ADEF File Offset: 0x00028FEF
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0002ADF6 File Offset: 0x00028FF6
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_reloadLoopEmitter.Play();
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x0002AE0C File Offset: 0x0002900C
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

	// Token: 0x04001102 RID: 4354
	[SerializeField]
	private StudioEventEmitter m_reloadSuccessEmitter;

	// Token: 0x04001103 RID: 4355
	[SerializeField]
	private StudioEventEmitter m_reloadLoopEmitter;
}
