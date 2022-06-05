using System;
using FMODUnity;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class Cooking_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x060014FA RID: 5370 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x060014FB RID: 5371 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x060014FC RID: 5372 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x060014FD RID: 5373 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x060014FE RID: 5374 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x060014FF RID: 5375 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x06001500 RID: 5376 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x06001501 RID: 5377 RVA: 0x000050CB File Offset: 0x000032CB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x06001502 RID: 5378 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x06001503 RID: 5379 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x0008888C File Offset: 0x00086A8C
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

	// Token: 0x06001505 RID: 5381 RVA: 0x0000A2B3 File Offset: 0x000084B3
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x0000A7F5 File Offset: 0x000089F5
	protected override void OnEnterExitLogic()
	{
		Cooking_Ability.HealPlayer();
		this.ApplyAbilityCosts();
		base.OnEnterExitLogic();
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x000889B8 File Offset: 0x00086BB8
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

	// Token: 0x06001508 RID: 5384 RVA: 0x00088ABC File Offset: 0x00086CBC
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

	// Token: 0x04001651 RID: 5713
	[SerializeField]
	private StudioEventEmitter m_cookingEmitter;
}
