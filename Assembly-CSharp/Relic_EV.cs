using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class Relic_EV
{
	// Token: 0x060001B3 RID: 435 RVA: 0x00010950 File Offset: 0x0000EB50
	public static float GetRelicFormatString(RelicType relicType, int numRelics, out float altValue)
	{
		altValue = -1f;
		if (relicType <= RelicType.FatalBlowDodge)
		{
			if (relicType <= RelicType.RelicAmountDamageUp)
			{
				if (relicType <= RelicType.BonusDamageOnNextHit)
				{
					if (relicType <= RelicType.LowMultiJump)
					{
						if (relicType == RelicType.ExtraLife)
						{
							altValue = (float)Mathf.RoundToInt(50f);
							return (float)numRelics;
						}
						if (relicType == RelicType.ExtraLife_Unity)
						{
							altValue = (float)Mathf.RoundToInt(50f);
							return (float)numRelics;
						}
						if (relicType == RelicType.LowMultiJump)
						{
							return (float)(3 * numRelics);
						}
					}
					else if (relicType <= RelicType.FreeEnemyKill)
					{
						if (relicType == RelicType.BonusDamageCurse)
						{
							return (float)(Mathf.RoundToInt(100f) * numRelics);
						}
						if (relicType == RelicType.FreeEnemyKill)
						{
							return (float)(6 - (numRelics - 1));
						}
					}
					else
					{
						if (relicType == RelicType.TakeNoDamage)
						{
							return (float)(3 * numRelics);
						}
						if (relicType == RelicType.BonusDamageOnNextHit)
						{
							altValue = 6f;
							return (float)(Mathf.RoundToInt(75f) * numRelics);
						}
					}
				}
				else if (relicType <= RelicType.ExtendInvuln)
				{
					if (relicType == RelicType.ChestHealthRestore)
					{
						return (float)(Mathf.RoundToInt(100f) * numRelics);
					}
					if (relicType == RelicType.MeatMaxHealth)
					{
						altValue = (float)Mathf.RoundToInt(10f);
						return (float)(3 * numRelics);
					}
					if (relicType == RelicType.ExtendInvuln)
					{
						return 1.25f * (float)numRelics;
					}
				}
				else if (relicType <= RelicType.DamageBuffStatusEffect)
				{
					if (relicType == RelicType.InvulnDamageBuff)
					{
						return (float)(Mathf.RoundToInt(100f) * numRelics);
					}
					if (relicType == RelicType.DamageBuffStatusEffect)
					{
						return (float)(Mathf.RoundToInt(20f) * numRelics);
					}
				}
				else
				{
					if (relicType == RelicType.AttackCooldown)
					{
						altValue = 2f * (float)numRelics;
						return (float)(Mathf.RoundToInt(100f) * numRelics);
					}
					if (relicType == RelicType.RelicAmountDamageUp)
					{
						return (float)(Mathf.RoundToInt(6f) * numRelics);
					}
				}
			}
			else if (relicType <= RelicType.MagicDamageEnemyCount)
			{
				if (relicType <= RelicType.BonusMana)
				{
					if (relicType == RelicType.WeaponsBurnAdd)
					{
						return 2f * (float)numRelics;
					}
					if (relicType == RelicType.EnemiesDropMeat)
					{
						return (float)(Mathf.RoundToInt(8f) * numRelics);
					}
					if (relicType == RelicType.BonusMana)
					{
						altValue = (float)(Mathf.RoundToInt(8.000004f) * numRelics);
						return (float)(50 * numRelics);
					}
				}
				else if (relicType <= RelicType.NoGoldXPBonus)
				{
					if (relicType == RelicType.SpinKickArmorBreak)
					{
						return 3.5f * (float)numRelics;
					}
					if (relicType == RelicType.NoGoldXPBonus)
					{
						altValue = 3f;
						return (float)Mathf.RoundToInt(Mathf.Min((Economy_EV.GetGoldGainMod() + 1f) * NPC_EV.GetArchitectGoldMod(-1) * 100f, 300f));
					}
				}
				else
				{
					if (relicType == RelicType.ManaRestoreOnHurt)
					{
						return (float)(100 * numRelics);
					}
					if (relicType == RelicType.MagicDamageEnemyCount)
					{
						altValue = (float)Mathf.RoundToInt(10f);
						return (float)(Mathf.RoundToInt(50f) * numRelics);
					}
				}
			}
			else if (relicType <= RelicType.CritKillsHeal)
			{
				if (relicType <= RelicType.ResolveCombatChallenge)
				{
					if (relicType == RelicType.DashStrikeDamageUp)
					{
						return 10f * (0.2f + 0.2f * (float)(numRelics - 1));
					}
					if (relicType == RelicType.ResolveCombatChallenge)
					{
						altValue = (float)Mathf.RoundToInt(Mathf.Abs(RelicLibrary.GetRelicData(RelicType.ResolveCombatChallengeUsed).CostAmount * 100f));
						return 10f;
					}
				}
				else
				{
					if (relicType == RelicType.ResolveCombatChallengeUsed)
					{
						return (float)Mathf.RoundToInt(Mathf.Abs(RelicLibrary.GetRelicData(RelicType.ResolveCombatChallengeUsed).CostAmount * 100f));
					}
					if (relicType == RelicType.CritKillsHeal)
					{
						return (float)(Mathf.RoundToInt(6f) * numRelics);
					}
				}
			}
			else if (relicType <= RelicType.AllCritChanceUp)
			{
				if (relicType == RelicType.AllCritDamageUp)
				{
					return (float)(Mathf.RoundToInt(20f) * numRelics);
				}
				if (relicType == RelicType.AllCritChanceUp)
				{
					return (float)(Mathf.RoundToInt(10f) * numRelics);
				}
			}
			else
			{
				if (relicType == RelicType.MaxManaDamage)
				{
					return 25f * (float)numRelics;
				}
				if (relicType == RelicType.FatalBlowDodge)
				{
					return 25f * (float)numRelics;
				}
			}
		}
		else if (relicType <= RelicType.OnHitAreaDamage)
		{
			if (relicType <= RelicType.SuperCritChanceUp)
			{
				if (relicType <= RelicType.FreeHitRegenerate)
				{
					if (relicType == RelicType.MaxHealthStatBonus)
					{
						altValue = (float)Mathf.RoundToInt(50f);
						return (float)(Mathf.RoundToInt(10f) * numRelics);
					}
					if (relicType == RelicType.LowHealthStatBonus)
					{
						altValue = (float)Mathf.RoundToInt(50f);
						return (float)(Mathf.RoundToInt(20f) * numRelics);
					}
					if (relicType == RelicType.FreeHitRegenerate)
					{
						return (float)(6 - (numRelics - 1));
					}
				}
				else if (relicType <= RelicType.GoldCombatChallenge)
				{
					if (relicType == RelicType.ManaDamageReduction)
					{
						altValue = 150f;
						return (float)(2 * numRelics);
					}
					if (relicType == RelicType.GoldCombatChallenge)
					{
						altValue = (float)Mathf.RoundToInt(20f);
						return 15f;
					}
				}
				else
				{
					if (relicType == RelicType.FoodHealsMore)
					{
						return (float)(Mathf.RoundToInt(8f) * numRelics);
					}
					if (relicType == RelicType.SuperCritChanceUp)
					{
						return (float)(Mathf.RoundToInt(20f) * numRelics);
					}
				}
			}
			else if (relicType <= RelicType.WeaponsPoisonAdd)
			{
				if (relicType <= RelicType.SpellKillMaxMana)
				{
					switch (relicType)
					{
					case RelicType.WeaponSwap:
						return (float)(Mathf.RoundToInt(7.0000052f) * numRelics);
					case RelicType.SpellSwap:
						return (float)(Mathf.RoundToInt(7.0000052f) * numRelics);
					case RelicType.TalentSwap:
						return (float)(Mathf.RoundToInt(0f) * numRelics);
					default:
						if (relicType == RelicType.SpellKillMaxMana)
						{
							altValue = (float)(200 * numRelics);
							return 5f;
						}
						break;
					}
				}
				else
				{
					if (relicType == RelicType.AttackExhaust)
					{
						altValue = 25f;
						return (float)(Mathf.RoundToInt(50f) * numRelics);
					}
					if (relicType == RelicType.WeaponsPoisonAdd)
					{
						return (float)numRelics;
					}
				}
			}
			else if (relicType <= RelicType.LowResolveMagicDamage)
			{
				if (relicType == RelicType.LowResolveWeaponDamage)
				{
					if (PlayerManager.IsInstantiated)
					{
						altValue = (float)Mathf.RoundToInt((float)Mathf.Max(Mathf.RoundToInt((1f - PlayerManager.GetPlayerController().ActualResolve) * 100f), 0) * 0.00999999f * (float)numRelics * 100f);
					}
					return (float)(Mathf.RoundToInt(0.99999905f) * numRelics);
				}
				if (relicType == RelicType.LowResolveMagicDamage)
				{
					if (PlayerManager.IsInstantiated)
					{
						altValue = (float)Mathf.RoundToInt((float)Mathf.Max(Mathf.RoundToInt((1f - PlayerManager.GetPlayerController().ActualResolve) * 100f), 0) * 0.00999999f * (float)numRelics * 100f);
					}
					return (float)(Mathf.RoundToInt(0.99999905f) * numRelics);
				}
			}
			else
			{
				if (relicType == RelicType.GoldCombatChallengeUsed)
				{
					return (float)Mathf.RoundToInt(20f);
				}
				if (relicType == RelicType.OnHitAreaDamage)
				{
					altValue = (float)Mathf.RoundToInt((1.5f + (float)(numRelics - 1) * 0.75f) * 100f);
					return (float)Relic_EV.GetRelicMaxStack(relicType, numRelics);
				}
			}
		}
		else if (relicType <= RelicType.FlightBonusCurse)
		{
			if (relicType <= RelicType.ReplacementRelic)
			{
				if (relicType == RelicType.GroundDamageBonus)
				{
					return (float)Mathf.RoundToInt((float)numRelics * 0.125f * 100f);
				}
				if (relicType == RelicType.ProjectileDashStart)
				{
					altValue = (float)Mathf.RoundToInt(1f + (float)(numRelics - 1) * 1f);
					return 0f;
				}
				if (relicType == RelicType.ReplacementRelic)
				{
					return 10f * (float)numRelics;
				}
			}
			else if (relicType <= RelicType.RangeDamageBonusCurse)
			{
				if (relicType == RelicType.SkillCritBonus)
				{
					return 2.5f + (float)(numRelics - 1) * 2.5f;
				}
				if (relicType == RelicType.RangeDamageBonusCurse)
				{
					altValue = (float)Mathf.Abs(Mathf.RoundToInt(0f));
					return (float)Mathf.RoundToInt((float)(100 * numRelics) * 0.125f);
				}
			}
			else
			{
				if (relicType == RelicType.SpinKickLeavesCaltrops)
				{
					return (float)Relic_EV.GetRelicMaxStack(relicType, numRelics);
				}
				if (relicType == RelicType.FlightBonusCurse)
				{
					return (float)Mathf.RoundToInt(75f);
				}
			}
		}
		else if (relicType <= RelicType.FoodChallengeUsed)
		{
			if (relicType <= RelicType.LandShockwave)
			{
				if (relicType == RelicType.DamageAuraOnHit)
				{
					return 1.5f + (float)(numRelics - 1) * 1.5f;
				}
				if (relicType == RelicType.LandShockwave)
				{
					return (0.75f + (float)(numRelics - 1) * 0.75f) * 100f;
				}
			}
			else
			{
				if (relicType == RelicType.FoodChallenge)
				{
					if (PlayerManager.IsInstantiated)
					{
						PlayerController playerController = PlayerManager.GetPlayerController();
						float num = (float)playerController.ActualMaxHealth;
						SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallengeUsed).SetLevel(1, true, false);
						playerController.InitializeHealthMods();
						float num2 = (float)playerController.ActualMaxHealth - num;
						SaveManager.PlayerSaveData.GetRelic(RelicType.FoodChallengeUsed).SetLevel(-1, true, false);
						playerController.InitializeHealthMods();
						altValue = num2;
					}
					return 1f;
				}
				if (relicType == RelicType.FoodChallengeUsed)
				{
					altValue = 50f;
					return SaveManager.PlayerSaveData.GetRelic(relicType).FloatValue;
				}
			}
		}
		else if (relicType <= RelicType.NoAttackDamageBonus)
		{
			if (relicType == RelicType.FreeCastSpell)
			{
				return (float)Relic_EV.GetRelicMaxStack(relicType, numRelics);
			}
			if (relicType == RelicType.NoAttackDamageBonus)
			{
				altValue = 75f;
				return (float)Relic_EV.GetRelicMaxStack(relicType, numRelics);
			}
		}
		else
		{
			if (relicType == RelicType.DamageNoHitChallenge)
			{
				return (float)(Mathf.RoundToInt(150f) * numRelics);
			}
			if (relicType == RelicType.SpinKickDamageBonus)
			{
				return 0.4f * (float)numRelics * 100f;
			}
		}
		return 0f;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00011214 File Offset: 0x0000F414
	public static int GetRelicMaxStack(RelicType relicType, int numRelics)
	{
		if (relicType <= RelicType.ManaDamageReduction)
		{
			if (relicType <= RelicType.MeatMaxHealth)
			{
				if (relicType == RelicType.FreeEnemyKill)
				{
					return 6 - (numRelics - 1);
				}
				if (relicType == RelicType.BonusDamageOnNextHit)
				{
					return 5;
				}
				if (relicType == RelicType.MeatMaxHealth)
				{
					return 3 * numRelics;
				}
			}
			else if (relicType <= RelicType.ResolveCombatChallenge)
			{
				if (relicType == RelicType.MagicDamageEnemyCount)
				{
					return Mathf.RoundToInt(5f) * numRelics;
				}
				if (relicType == RelicType.ResolveCombatChallenge)
				{
					return 10;
				}
			}
			else
			{
				if (relicType == RelicType.FreeHitRegenerate)
				{
					return 6 - (numRelics - 1);
				}
				if (relicType == RelicType.ManaDamageReduction)
				{
					return 2 * numRelics;
				}
			}
		}
		else if (relicType <= RelicType.ProjectileDashStart)
		{
			if (relicType == RelicType.GoldCombatChallenge)
			{
				return 15;
			}
			if (relicType == RelicType.OnHitAreaDamage)
			{
				return 5 - (numRelics - 1);
			}
			if (relicType == RelicType.ProjectileDashStart)
			{
				return -1;
			}
		}
		else if (relicType <= RelicType.FoodChallenge)
		{
			if (relicType == RelicType.SpinKickLeavesCaltrops)
			{
				return 4 - (numRelics - 1);
			}
			if (relicType == RelicType.FoodChallenge)
			{
				return 1;
			}
		}
		else
		{
			if (relicType == RelicType.FreeCastSpell)
			{
				return 3 - (numRelics - 1);
			}
			if (relicType == RelicType.NoAttackDamageBonus)
			{
				return 5 - (numRelics - 1);
			}
		}
		return -1;
	}

	// Token: 0x04000452 RID: 1106
	public const float RELIC_RARITY_ONE_ODDS = 1f;

	// Token: 0x04000453 RID: 1107
	public const float RELIC_RARITY_TWO_ODDS = 0f;

	// Token: 0x04000454 RID: 1108
	public const float RELIC_RARITY_THREE_ODDS = 0f;

	// Token: 0x04000455 RID: 1109
	public const float RELIC_MOD_CHANCE = 0.2f;

	// Token: 0x04000456 RID: 1110
	public const float EXTRALIFE_RESTORE_HP_MOD = 0.5f;

	// Token: 0x04000457 RID: 1111
	public const float EXTRALIFE_RESTORE_HP_MAGIC_DAMAGE_FLAT = 0f;

	// Token: 0x04000458 RID: 1112
	public const float EXTRALIFE_UNITY_RESTORE_HP_MOD = 0.5f;

	// Token: 0x04000459 RID: 1113
	public const int TAKE_NO_DAMAGE_FREE_HITS = 3;

	// Token: 0x0400045A RID: 1114
	public const int BONUS_DMG_ON_HIT_COUNT = 6;

	// Token: 0x0400045B RID: 1115
	public const float BONUS_DMG_ON_HIT_DAMAGE_MOD = 1.75f;

	// Token: 0x0400045C RID: 1116
	public const float GOD_MODE_DURATION = 25f;

	// Token: 0x0400045D RID: 1117
	public const float EXTEND_INVULN_DURATION_FLAT_ADD = 1.25f;

	// Token: 0x0400045E RID: 1118
	public const int LOW_MULTI_JUMP_EXTRA_JUMPS = 3;

	// Token: 0x0400045F RID: 1119
	public const float FREE_ENEMY_KILL_CHANCE = 0.05f;

	// Token: 0x04000460 RID: 1120
	public const int FREE_ENEMY_KILL_CHANCE_CHARGE = 6;

	// Token: 0x04000461 RID: 1121
	public const float GOLD_DEATH_CURSE_GOLD_GAIN = 1.35f;

	// Token: 0x04000462 RID: 1122
	public const float CHESTS_HEALTH_RESTORE_MAGIC_SCALE = 1f;

	// Token: 0x04000463 RID: 1123
	public const float MEAT_MAX_HEALTH_UP_MOD = 0.1f;

	// Token: 0x04000464 RID: 1124
	public const int MEAT_MAX_HEALTH_UP_STACK = 3;

	// Token: 0x04000465 RID: 1125
	public const float INVULN_DAMAGE_BUFF_MOD = 2f;

	// Token: 0x04000466 RID: 1126
	public const float RELIC_AMOUNT_DAMAGE_UP = 0.06f;

	// Token: 0x04000467 RID: 1127
	public const float WEAPONS_BURN_ADD_DURATION = 2f;

	// Token: 0x04000468 RID: 1128
	public const float WEAPONS_BURN_ADD_DAMAGE_REDUCTION = 0f;

	// Token: 0x04000469 RID: 1129
	public const float WEAPONS_POISON_ADD_DURATION = 2f;

	// Token: 0x0400046A RID: 1130
	public const float ENEMIES_DROP_MEAT_CHANCE = 0.08f;

	// Token: 0x0400046B RID: 1131
	public const float DAMAGE_BUFF_STATUS_EFFECT = 0.2f;

	// Token: 0x0400046C RID: 1132
	public const float ATTACK_COOLDOWN_DURATION = 2f;

	// Token: 0x0400046D RID: 1133
	public const float ATTACK_COOLDOWN_DAMAGE_MOD = 2f;

	// Token: 0x0400046E RID: 1134
	public const float ATTACK_COOLDOWN_GUN_PROJECTILE_LIFESPAN_ADD = 0f;

	// Token: 0x0400046F RID: 1135
	public const float ATTACK_COOLDOWN_RELOAD_ADD = 0f;

	// Token: 0x04000470 RID: 1136
	public const float ATTACK_COOLDOWN_PISTOL_EXIT_DELAY_ADD = 0.125f;

	// Token: 0x04000471 RID: 1137
	public const float ATTACK_COOLDOWN_AXE_SPINNER_MOD = -0.3f;

	// Token: 0x04000472 RID: 1138
	public const int ATTACK_COOLDOWN_ELECTRIC_LUTE_ADD = 1;

	// Token: 0x04000473 RID: 1139
	public const int KILLS_TO_LEVEL_LILIES = 21;

	// Token: 0x04000474 RID: 1140
	public const float BONUS_DAMAGE_CURSE_DAMAGE_MOD = 1f;

	// Token: 0x04000475 RID: 1141
	public const float SPINKICK_ARMOR_BREAK_DURATION_OVERRIDE = 3.5f;

	// Token: 0x04000476 RID: 1142
	public const float MAGIC_DAMAGE_ENEMY_COUNT_INCREASE_PERCENT = 0.1f;

	// Token: 0x04000477 RID: 1143
	public const float MAGIC_DAMAGE_ENEMY_COUNT_MAX_PERCENT = 0.5f;

	// Token: 0x04000478 RID: 1144
	public const int MANA_RESTORE_ON_HURT_FLAT_ADD = 100;

	// Token: 0x04000479 RID: 1145
	public const int BONUS_MANA_FLAT_ADD = 50;

	// Token: 0x0400047A RID: 1146
	public const float BONUS_MANA_MAGIC_DAMAGE_MOD = 1.08f;

	// Token: 0x0400047B RID: 1147
	public const float SPELL_SWAP_MAGIC_DAMAGE_MOD = 1.07f;

	// Token: 0x0400047C RID: 1148
	public const float WEAPON_SWAP_WEAPON_DAMAGE_MOD = 1.07f;

	// Token: 0x0400047D RID: 1149
	public const float TALENT_SWAP_WEAPON_DAMAGE_MOD = 1f;

	// Token: 0x0400047E RID: 1150
	public const float TALENT_SWAP_MAGIC_DAMAGE_MOD = 1f;

	// Token: 0x0400047F RID: 1151
	public const int RESOLVE_COMBAT_CHALLENGE_NUM_KILLS_REQUIRED = 10;

	// Token: 0x04000480 RID: 1152
	public const bool RESOLVE_COMBAT_CHALLENGE_KILL_PLAYER_ON_HIT = true;

	// Token: 0x04000481 RID: 1153
	public const float RESOLVE_COMBAT_CHALLENGE_DAMAGE_MOD = 10f;

	// Token: 0x04000482 RID: 1154
	public const float NO_GOLD_XP_BONUS_CAP = 3f;

	// Token: 0x04000483 RID: 1155
	public const float CRIT_KILLS_HEAL_AMOUNT = 0.06f;

	// Token: 0x04000484 RID: 1156
	public const float CRIT_DMG_UP_AMOUNT = 0.2f;

	// Token: 0x04000485 RID: 1157
	public const float CRIT_CHANCE_UP_AMOUNT = 0.1f;

	// Token: 0x04000486 RID: 1158
	public const float MAX_MANA_DAMAGE_MOD = 0.25f;

	// Token: 0x04000487 RID: 1159
	public const int FREE_HIT_REGENERATE_CHARGE = 6;

	// Token: 0x04000488 RID: 1160
	public const float MAX_HEALTH_STAT_BONUS_MOD = 0.1f;

	// Token: 0x04000489 RID: 1161
	public const float MAX_HEALTH_STAT_BONUS_MAX_HP_AMOUNT = 0.5f;

	// Token: 0x0400048A RID: 1162
	public const float LOW_HEALTH_STAT_BONUS_MAX_HP_AMOUNT = 0.5f;

	// Token: 0x0400048B RID: 1163
	public const float LOW_HEALTH_STAT_BONUS_MOD = 0.2f;

	// Token: 0x0400048C RID: 1164
	public const float AIM_SLOW_TIME_AMOUNT = 0.425f;

	// Token: 0x0400048D RID: 1165
	public const float REPLACEMENT_RELIC_MAX_HEALTH_MOD = 0.1f;

	// Token: 0x0400048E RID: 1166
	public const float REPLACEMENT_RELIC_STRENGTH_MOD = 0.1f;

	// Token: 0x0400048F RID: 1167
	public const float REPLACEMENT_RELIC_MAGIC_MOD = 0.1f;

	// Token: 0x04000490 RID: 1168
	public const float DASH_STRIKE_DAMAGE_UP_MOD = 0.15f;

	// Token: 0x04000491 RID: 1169
	public const float DASH_STRIKE_DISTANCE_ADD = 0.2f;

	// Token: 0x04000492 RID: 1170
	public const float FATAL_BLOW_DODGE_CHANCE = 0.25f;

	// Token: 0x04000493 RID: 1171
	public const int GOLD_COMBAT_CHALLENGE_NUM_KILLS_REQUIRED = 15;

	// Token: 0x04000494 RID: 1172
	public const float GOLD_COMBAT_CHALLENGE_GOLD_MOD = 0.2f;

	// Token: 0x04000495 RID: 1173
	public const float SUPER_CRIT_CHANCE_UP_MOD_AMOUNT = 0.2f;

	// Token: 0x04000496 RID: 1174
	public const float FOOD_HEALS_MORE_MOD_AMOUNT = 0.08f;

	// Token: 0x04000497 RID: 1175
	public const float SPELL_KILL_MAX_MANA_HP_REQUIREMENT = 0.5f;

	// Token: 0x04000498 RID: 1176
	public const int SPELL_KILL_MAX_MANA_MP_CAP = 200;

	// Token: 0x04000499 RID: 1177
	public const int SPELL_KILL_MAX_MANA_ADD_AMOUNT = 5;

	// Token: 0x0400049A RID: 1178
	public const float ATTACK_EXHAUST_BONUS_DAMAGE_MOD = 1.5f;

	// Token: 0x0400049B RID: 1179
	public const int ATTACK_EXHAUST_EXHAUST_ADD_AMOUNT = 25;

	// Token: 0x0400049C RID: 1180
	public const float LOW_RESOLVE_WEAPON_DAMAGE_MOD = 1.01f;

	// Token: 0x0400049D RID: 1181
	public const float LOW_RESOLVE_MAGIC_DAMAGE_MOD = 1.01f;

	// Token: 0x0400049E RID: 1182
	public const float GROUND_DAMAGE_BONUS_DAMAGE_MOD = 0.125f;

	// Token: 0x0400049F RID: 1183
	public const float FLIGHT_BONUS_CURSE_DAMAGE_MOD = 0.75f;

	// Token: 0x040004A0 RID: 1184
	public const float FLIGHT_BONUS_MOVE_SPEED_MOD = 0.1f;

	// Token: 0x040004A1 RID: 1185
	public const float RANGE_DAMAGE_BONUS_CURSE_FAR_DISTANCE = 8.5f;

	// Token: 0x040004A2 RID: 1186
	public const float RANGE_DAMAGE_BONUS_CURSE_FAR_DAMAGE_MOD = 0.125f;

	// Token: 0x040004A3 RID: 1187
	public const float RANGE_DAMAGE_BONUS_CURSE_NEAR_DAMAGE_MOD = 0f;

	// Token: 0x040004A4 RID: 1188
	public const int PROJECTILE_DASH_START_NUM_REQUIRED_DASHES = 0;

	// Token: 0x040004A5 RID: 1189
	public const float PROJECTILE_DASH_START_BASE_LIFESPAWN = 1f;

	// Token: 0x040004A6 RID: 1190
	public const float PROJECTILE_DASH_START_LIFESPAN_ADD = 1f;

	// Token: 0x040004A7 RID: 1191
	public const float DAMAGE_AURA_ON_HIT_LIFESPAN_BASE = 1.5f;

	// Token: 0x040004A8 RID: 1192
	public const float DAMAGE_AURA_ON_HIT_LIFESPAN_ADD = 1.5f;

	// Token: 0x040004A9 RID: 1193
	public const float LAND_SHOCKWAVE_DAMAGE_BASE = 0.75f;

	// Token: 0x040004AA RID: 1194
	public const float LAND_SHOCKWAVE_DAMAGE_MOD_PER_STACK = 0.75f;

	// Token: 0x040004AB RID: 1195
	public const int FOOD_CHALLENGE_NUM_ITEMS_REQUIRED = 1;

	// Token: 0x040004AC RID: 1196
	public const int FOOD_CHALLENGE_BONUS_MANA_FLAT_ADD = 50;

	// Token: 0x040004AD RID: 1197
	public const float FOOD_CHALLENGE_MAX_HEALTH_MOD = 0.3f;

	// Token: 0x040004AE RID: 1198
	public const float DAMAGE_NO_HIT_CHALLENGE_BONUS_DAMAGE_MOD = 1.5f;

	// Token: 0x040004AF RID: 1199
	public const float ON_HIT_AREA_BASE_DAMAGE = 1.5f;

	// Token: 0x040004B0 RID: 1200
	public const float ON_HIT_AREA_DAMAGE_ADD_PER_STACK = 0.75f;

	// Token: 0x040004B1 RID: 1201
	public const int ON_HIT_AREA_DAMAGE_RECHARGE_TIME = 5;

	// Token: 0x040004B2 RID: 1202
	public const int ON_HIT_AREA_DAMAGE_RECHARGE_TIME_REDUCTION_PER_STACK = 1;

	// Token: 0x040004B3 RID: 1203
	public const float MANA_DAMAGE_REDUCTION_MANA_AMOUNT = 150f;

	// Token: 0x040004B4 RID: 1204
	public const float MANA_DAMAGE_REDUCTION_DAMAGE_MOD = 1f;

	// Token: 0x040004B5 RID: 1205
	public const int MANA_DAMAGE_REDUCTION_STARTING_CHARGES = 2;

	// Token: 0x040004B6 RID: 1206
	public const int FREE_CAST_SPELL_REQUIRED_CASTS = 3;

	// Token: 0x040004B7 RID: 1207
	public const int FREE_CAST_SPELL_CAST_REDUCTION_PER_STACK = 1;

	// Token: 0x040004B8 RID: 1208
	public const int SPINKICK_LEAVES_CALTROPS_NUM_KICKS_REQUIRED = 4;

	// Token: 0x040004B9 RID: 1209
	public const int SPINKICK_LEAVES_CALTROPS_KICK_REDUCTION_PER_STACK = 1;

	// Token: 0x040004BA RID: 1210
	public const int NO_ATTACK_DAMAGE_BONUS_RECHARGE_TIME = 5;

	// Token: 0x040004BB RID: 1211
	public const int NO_ATTACK_DAMAGE_BONUS_TIME_REDUCTION_PER_STACK = 1;

	// Token: 0x040004BC RID: 1212
	public const float NO_ATTACK_DAMAGE_BONUS_DAMAGE_MOD = 0.75f;

	// Token: 0x040004BD RID: 1213
	public const float SPINKICK_DAMAGE_BONUS_MOD = 0.4f;

	// Token: 0x040004BE RID: 1214
	public const float SKILL_CRIT_BONUS_BASE_DURATION = 2.5f;

	// Token: 0x040004BF RID: 1215
	public const float SKILL_CRIT_BONUS_DURATION_ADD_PER_STACK = 2.5f;

	// Token: 0x040004C0 RID: 1216
	public const float MEAT_DROP_CHANCE_UP_MOD_ADD = 0.01f;

	// Token: 0x040004C1 RID: 1217
	public const float DAMAGE_REDUCTION_STATUS_EFFECT_AMOUNT = 0.15f;

	// Token: 0x040004C2 RID: 1218
	public const float WEAPON_CRIT_DAMAGE_ADD_AMOUNT = 0.1f;

	// Token: 0x040004C3 RID: 1219
	public const float MAGIC_CRIT_DAMAGE_ADD_AMOUNT = 0.1f;
}
