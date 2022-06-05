using System;
using FMODUnity;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class Cooking_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00029504 File Offset: 0x00027704
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0002950B File Offset: 0x0002770B
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00029512 File Offset: 0x00027712
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00029519 File Offset: 0x00027719
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00029520 File Offset: 0x00027720
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00029527 File Offset: 0x00027727
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06000D8B RID: 3467 RVA: 0x0002952E File Offset: 0x0002772E
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00029535 File Offset: 0x00027735
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x06000D8D RID: 3469 RVA: 0x0002953C File Offset: 0x0002773C
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00029543 File Offset: 0x00027743
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0002954C File Offset: 0x0002774C
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		if (this.m_abilityController.PlayerController.CharacterClass.WeaponAbilityType != AbilityType.SpoonsWeapon)
		{
			if (this.m_abilityController.PlayerController.CharacterClass.WeaponAbilityType != AbilityType.BoxingGloveWeapon && this.m_abilityController.PlayerController.CharacterClass.WeaponAbilityType != AbilityType.ExplosiveHandsWeapon && this.m_abilityController.PlayerController.LookController.CurrentWeaponGeo)
			{
				this.m_abilityController.PlayerController.LookController.CurrentWeaponGeo.gameObject.SetActive(false);
			}
			this.m_abilityController.PlayerController.LookController.LadleGeo.gameObject.SetActive(true);
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		this.m_abilityController.PlayerController.ControllerCorgi.RayLengthAdd += 0.05f;
		this.m_cookingEmitter.Play();
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00029677 File Offset: 0x00027877
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00029690 File Offset: 0x00027890
	protected override void OnEnterExitLogic()
	{
		Cooking_Ability.HealPlayer();
		this.ApplyAbilityCosts();
		base.OnEnterExitLogic();
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x000296A4 File Offset: 0x000278A4
	public static void HealPlayer()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		float num = (float)Mathf.CeilToInt(playerController.ActualMagic * 2f);
		float num2 = (float)Mathf.CeilToInt(playerController.ActualMagic * 0f + 100f);
		playerController.SetHealth(num, true, true);
		playerController.SetMana(num2, true, true, false);
		Vector2 absPos = playerController.Midpoint;
		absPos.y += playerController.CollisionBounds.height / 2f;
		if (TraitManager.IsTraitActive(TraitType.MegaHealth))
		{
			num = 0f;
		}
		string str = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num);
		string str2 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MANA_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num2);
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, str + "\n<color=#47FFFB>" + str2 + "</color>", absPos, null, TextAlignmentOptions.Center);
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x000297A8 File Offset: 0x000279A8
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		this.m_cookingEmitter.Stop();
		this.m_abilityController.PlayerController.ControllerCorgi.RayLengthAdd -= 0.05f;
		if (this.m_abilityController.PlayerController.CharacterClass.WeaponAbilityType != AbilityType.SpoonsWeapon)
		{
			if (this.m_abilityController.PlayerController.LookController.CurrentWeaponGeo)
			{
				this.m_abilityController.PlayerController.LookController.CurrentWeaponGeo.gameObject.SetActive(true);
			}
			this.m_abilityController.PlayerController.LookController.LadleGeo.gameObject.SetActive(false);
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x040010E2 RID: 4322
	[SerializeField]
	private StudioEventEmitter m_cookingEmitter;
}
