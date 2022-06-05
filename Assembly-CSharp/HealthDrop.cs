using System;
using TMPro;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class HealthDrop : BaseItemDrop
{
	// Token: 0x1700103F RID: 4159
	// (get) Token: 0x060029C8 RID: 10696 RVA: 0x00089FC1 File Offset: 0x000881C1
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.HealthDrop;
		}
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x00089FC5 File Offset: 0x000881C5
	protected override void Collect(GameObject collector)
	{
		this.GainHealth(0f);
		base.Collect(collector);
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x00089FDC File Offset: 0x000881DC
	protected virtual void GainHealth(float hpGain = 0f)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController)
		{
			if (hpGain == 0f)
			{
				float num = (float)SaveManager.PlayerSaveData.GetRelic(RelicType.FoodHealsMore).Level * 0.08f;
				hpGain = (float)playerController.ActualMaxHealth * (0f + num);
				float num2 = playerController.ActualMagic * (2f + SkillTreeLogicHelper.GetPotionMods());
				hpGain += num2;
			}
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.MeatMaxHealth);
			if (relic.IntValue < 3 * relic.Level && playerController.CurrentHealth >= (float)playerController.ActualMaxHealth)
			{
				float num3 = 0.1f;
				int actualMaxHealth = playerController.ActualMaxHealth;
				SaveManager.PlayerSaveData.TemporaryMaxHealthMods += num3;
				playerController.InitializeHealthMods();
				int actualMaxHealth2 = playerController.ActualMaxHealth;
				playerController.SetHealth((float)playerController.ActualMaxHealth, false, true);
				HealthDrop.m_healthChangeArgs_STATIC.Initialise((float)actualMaxHealth2, (float)actualMaxHealth);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerMaxHealthChange, this, HealthDrop.m_healthChangeArgs_STATIC);
				relic.SetIntValue(1, true, true);
			}
			HealthDrop.IncrementFoodChallengeRelicCounter(this);
			bool flag = true;
			BaseAbility_RL ability = playerController.CastAbility.GetAbility(CastAbilityType.Talent, false);
			if (TraitManager.IsTraitActive(TraitType.NoMeat) && this.ItemDropType != ItemDropType.MushroomDrop && this.ItemDropType != ItemDropType.CandyDrop && this.ItemDropType != ItemDropType.CookieDrop)
			{
				playerController.CharacterHitResponse.StartHitResponse(playerController.gameObject, playerController, hpGain * 1f, false, true);
				flag = false;
			}
			Vector2 absPos = playerController.Midpoint;
			absPos.y += playerController.CollisionBounds.height / 2f;
			if (ability && ability.AbilityType == AbilityType.CookingTalent && ability.CurrentAmmo < ability.MaxAmmo)
			{
				int num4 = 1;
				string text;
				if (ability.CurrentAmmo < ability.MaxAmmo - num4)
				{
					text = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_COOK_TALENT_CHARGE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num4);
				}
				else
				{
					text = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_COOK_TALENT_CHARGE_MAX_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				}
				TextPopupManager.DisplayTextAtAbsPos(TextPopupType.DownstrikeAmmoGain, text, absPos, null, TextAlignmentOptions.Center);
				absPos.y += playerController.CollisionBounds.height / 2f;
			}
			if (flag)
			{
				hpGain = (float)Mathf.CeilToInt(hpGain);
				if (TraitManager.IsTraitActive(TraitType.MegaHealth))
				{
					hpGain = 0f;
				}
				playerController.SetHealth(hpGain, true, true);
				string text2 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)hpGain);
				TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, text2, absPos, null, TextAlignmentOptions.Center);
			}
		}
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x0008A280 File Offset: 0x00088480
	public static void IncrementFoodChallengeRelicCounter(MonoBehaviour itemDrop)
	{
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallenge);
		if (relic.Level > 0)
		{
			relic.SetIntValue(1, true, true);
			if (relic.IntValue >= 1)
			{
				PlayerController playerController = PlayerManager.GetPlayerController();
				float num = (float)playerController.ActualMaxHealth;
				relic.SetIntValue(0, false, true);
				relic.SetLevel(-1, true, true);
				RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallengeUsed);
				relic2.SetLevel(1, true, true);
				HealthDrop.m_relicChangedEventArgs_STATIC.Initialize(RelicType.FoodChallengeUsed);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicPurified, itemDrop, HealthDrop.m_relicChangedEventArgs_STATIC);
				relic2.SetFloatValue((float)playerController.ActualMaxHealth - num, false, true);
			}
		}
	}

	// Token: 0x04002247 RID: 8775
	private static MaxHealthChangeEventArgs m_healthChangeArgs_STATIC = new MaxHealthChangeEventArgs(0f, 0f);

	// Token: 0x04002248 RID: 8776
	private static RelicChangedEventArgs m_relicChangedEventArgs_STATIC = new RelicChangedEventArgs(RelicType.None);
}
