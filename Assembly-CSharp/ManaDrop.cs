using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000473 RID: 1139
public class ManaDrop : BaseItemDrop
{
	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x060029D2 RID: 10706 RVA: 0x0008A363 File Offset: 0x00088563
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.ManaDrop;
		}
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x0008A368 File Offset: 0x00088568
	protected override void Collect(GameObject collector)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController != null)
		{
			float num = 0.5f;
			int num2 = Mathf.CeilToInt((float)playerController.ActualMaxMana * num);
			if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Potions_Free_Cast_Up) > 0)
			{
				playerController.SpellOrbs = 1;
			}
			playerController.SetMana((float)num2, true, true, false);
			Vector2 absPos = playerController.Midpoint;
			absPos.y += playerController.CollisionBounds.height / 2f;
			string text;
			if (playerController.CurrentManaAsInt >= playerController.ActualMaxMana)
			{
				if (this.m_maxManaMessageShownUnityEvent != null)
				{
					this.m_maxManaMessageShownUnityEvent.Invoke();
				}
				text = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MAX_MANA_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
			else
			{
				text = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MANA_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num2);
			}
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.MPGained, text, absPos, null, TextAlignmentOptions.Center);
			float armorHealthMod = RuneLogicHelper.GetArmorHealthMod();
			if (armorHealthMod > 0f)
			{
				int num3 = Mathf.CeilToInt(armorHealthMod * (float)playerController.ActualArmor);
				int num4 = Mathf.CeilToInt(armorHealthMod * (float)playerController.ActualMaxHealth);
				int num5 = playerController.ActualArmor - playerController.CurrentArmor;
				playerController.CurrentArmor += num3;
				playerController.SetHealth((float)num4, true, true);
				if (TraitManager.IsTraitActive(TraitType.MegaHealth))
				{
					num4 = 0;
				}
				string text2 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num4);
				if (num5 > 0)
				{
					num3 = Mathf.Min(num3, num5);
					text2 += string.Format(" <size=75%><color=grey>(" + LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_ARMOR_RESTORED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false) + ")</color></size>", num3);
				}
				absPos.y += 0.75f;
				TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, text2, absPos, null, TextAlignmentOptions.Center);
			}
			if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Potion_Recharge_Talent) > 0)
			{
				string text3 = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_COOLDOWN_CHARGED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				BaseAbility_RL ability = playerController.CastAbility.GetAbility(CastAbilityType.Talent, false);
				if (ability && ability.AbilityType == AbilityType.CookingTalent && ability.CurrentAmmo < ability.MaxAmmo)
				{
					int num6 = 1;
					if (ability.CurrentAmmo < ability.MaxAmmo - num6)
					{
						text3 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_COOK_TALENT_CHARGE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num6);
					}
					else
					{
						text3 = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_COOK_TALENT_CHARGE_MAX_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					}
				}
				playerController.CastAbility.ResetAbilityCooldowns(CastAbilityType.Talent, false);
				absPos.y += 0.75f;
				TextPopupManager.DisplayTextAtAbsPos(TextPopupType.DownstrikeAmmoGain, text3, absPos, null, TextAlignmentOptions.Center);
			}
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.ManaDamageReduction);
			if (relic.Level > 0 && relic.IntValue > 0)
			{
				relic.SetIntValue(-1, true, true);
			}
			HealthDrop.IncrementFoodChallengeRelicCounter(this);
		}
		base.Collect(collector);
	}

	// Token: 0x0400224A RID: 8778
	private static HealthChangeEventArgs m_healthChangeArgs_STATIC;

	// Token: 0x0400224B RID: 8779
	[SerializeField]
	private UnityEvent m_maxManaMessageShownUnityEvent;
}
