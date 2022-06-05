using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[Serializable]
public class CachedPlayerData
{
	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x060027B3 RID: 10163 RVA: 0x00016550 File Offset: 0x00014750
	public int CurrentExhaust
	{
		get
		{
			return this.m_currentExhaust;
		}
	}

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x060027B4 RID: 10164 RVA: 0x00016558 File Offset: 0x00014758
	public int CurrentSpellAmmo
	{
		get
		{
			return this.m_currentSpellAmmo;
		}
	}

	// Token: 0x17001037 RID: 4151
	// (get) Token: 0x060027B5 RID: 10165 RVA: 0x00016560 File Offset: 0x00014760
	public int CurrentTalentAmmo
	{
		get
		{
			return this.m_currentTalentAmmo;
		}
	}

	// Token: 0x17001038 RID: 4152
	// (get) Token: 0x060027B6 RID: 10166 RVA: 0x00016568 File Offset: 0x00014768
	public int CurrentWeaponAmmo
	{
		get
		{
			return this.m_currentWeaponAmmo;
		}
	}

	// Token: 0x17001039 RID: 4153
	// (get) Token: 0x060027B7 RID: 10167 RVA: 0x00016570 File Offset: 0x00014770
	public float ActualResolve
	{
		get
		{
			return this.m_actualResolve;
		}
	}

	// Token: 0x1700103A RID: 4154
	// (get) Token: 0x060027B8 RID: 10168 RVA: 0x00016578 File Offset: 0x00014778
	public int CurrentHealth
	{
		get
		{
			return this.m_currentHealth;
		}
	}

	// Token: 0x1700103B RID: 4155
	// (get) Token: 0x060027B9 RID: 10169 RVA: 0x00016580 File Offset: 0x00014780
	public int CurrentMana
	{
		get
		{
			return this.m_currentMana;
		}
	}

	// Token: 0x1700103C RID: 4156
	// (get) Token: 0x060027BA RID: 10170 RVA: 0x00016588 File Offset: 0x00014788
	public int SpellOrbs
	{
		get
		{
			return this.m_spellOrbs;
		}
	}

	// Token: 0x1700103D RID: 4157
	// (get) Token: 0x060027BB RID: 10171 RVA: 0x00016590 File Offset: 0x00014790
	public int CurrentArmor
	{
		get
		{
			return this.m_shields;
		}
	}

	// Token: 0x1700103E RID: 4158
	// (get) Token: 0x060027BC RID: 10172 RVA: 0x00016598 File Offset: 0x00014798
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x1700103F RID: 4159
	// (get) Token: 0x060027BD RID: 10173 RVA: 0x000165A0 File Offset: 0x000147A0
	public int VitalityStat
	{
		get
		{
			return this.m_actualVitality;
		}
	}

	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x060027BE RID: 10174 RVA: 0x000165A8 File Offset: 0x000147A8
	public int ModdedVitalityStat
	{
		get
		{
			return this.m_moddedVitality;
		}
	}

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x060027BF RID: 10175 RVA: 0x000165B0 File Offset: 0x000147B0
	public float StrengthStat
	{
		get
		{
			return this.m_actualStrength;
		}
	}

	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000165B8 File Offset: 0x000147B8
	public float ModdedStrengthStat
	{
		get
		{
			return this.m_moddedActualStrength;
		}
	}

	// Token: 0x17001043 RID: 4163
	// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000165C0 File Offset: 0x000147C0
	public float MagicStat
	{
		get
		{
			return this.m_actualMagic;
		}
	}

	// Token: 0x17001044 RID: 4164
	// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000165C8 File Offset: 0x000147C8
	public float ModdedMagicStat
	{
		get
		{
			return this.m_moddedActualMagic;
		}
	}

	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000165D0 File Offset: 0x000147D0
	public float MagicCritChance
	{
		get
		{
			return this.m_magicCritChance;
		}
	}

	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x060027C4 RID: 10180 RVA: 0x000165D8 File Offset: 0x000147D8
	public float ModdedMagicCritChance
	{
		get
		{
			return this.m_moddedMagicCritChance;
		}
	}

	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x060027C5 RID: 10181 RVA: 0x000165E0 File Offset: 0x000147E0
	public float MagicCritDmg
	{
		get
		{
			return this.m_magicCritDmg;
		}
	}

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x060027C6 RID: 10182 RVA: 0x000165E8 File Offset: 0x000147E8
	public float ModdedMagicCritDmg
	{
		get
		{
			return this.m_moddedMagicCritDmg;
		}
	}

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000165F0 File Offset: 0x000147F0
	public float CritChance
	{
		get
		{
			return this.m_critChance;
		}
	}

	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x060027C8 RID: 10184 RVA: 0x000165F8 File Offset: 0x000147F8
	public float ModdedCritChance
	{
		get
		{
			return this.m_moddedCritChance;
		}
	}

	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x060027C9 RID: 10185 RVA: 0x00016600 File Offset: 0x00014800
	public float CritDamage
	{
		get
		{
			return this.m_critDamage;
		}
	}

	// Token: 0x1700104C RID: 4172
	// (get) Token: 0x060027CA RID: 10186 RVA: 0x00016608 File Offset: 0x00014808
	public float ModdedCritDamage
	{
		get
		{
			return this.m_moddedCritDamage;
		}
	}

	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x060027CB RID: 10187 RVA: 0x00016610 File Offset: 0x00014810
	public float DexterityStat
	{
		get
		{
			return this.m_dexterity;
		}
	}

	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x060027CC RID: 10188 RVA: 0x00016618 File Offset: 0x00014818
	public float ModdedDexterityStat
	{
		get
		{
			return this.m_moddedDexterity;
		}
	}

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x060027CD RID: 10189 RVA: 0x00016620 File Offset: 0x00014820
	public float FocusStat
	{
		get
		{
			return this.m_focus;
		}
	}

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x060027CE RID: 10190 RVA: 0x00016628 File Offset: 0x00014828
	public float ModdedFocusStat
	{
		get
		{
			return this.m_moddedFocus;
		}
	}

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x060027CF RID: 10191 RVA: 0x00016630 File Offset: 0x00014830
	public int Weight
	{
		get
		{
			return this.m_weight;
		}
	}

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x060027D0 RID: 10192 RVA: 0x00016638 File Offset: 0x00014838
	public int RuneWeight
	{
		get
		{
			return this.m_runeWeight;
		}
	}

	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x060027D1 RID: 10193 RVA: 0x00016640 File Offset: 0x00014840
	public float Cooldown
	{
		get
		{
			return this.m_cooldown;
		}
	}

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x060027D2 RID: 10194 RVA: 0x00016648 File Offset: 0x00014848
	public int BaseArmor
	{
		get
		{
			return this.m_baseArmor;
		}
	}

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x060027D3 RID: 10195 RVA: 0x00016650 File Offset: 0x00014850
	public int SoulsCollected
	{
		get
		{
			return this.m_soulsCollected;
		}
	}

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x060027D4 RID: 10196 RVA: 0x00016658 File Offset: 0x00014858
	// (set) Token: 0x060027D5 RID: 10197 RVA: 0x00016660 File Offset: 0x00014860
	public int LastCachedUpdate_FrameCount { get; private set; }

	// Token: 0x060027D6 RID: 10198 RVA: 0x000BB900 File Offset: 0x000B9B00
	public void ForceValue(string fieldName, object value)
	{
		FieldInfo field = base.GetType().GetField(fieldName, BindingFlags.NonPublic);
		if (field != null)
		{
			field.SetValue(this, value);
			return;
		}
		Debug.Log("Failed to force cached player data value: " + fieldName + ". Field not found.");
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x000BB944 File Offset: 0x000B9B44
	public void UpdateData()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.InitializeAllMods(false, false);
			if (playerController.CachedHealthOverride != 0f)
			{
				this.m_currentHealth = (int)playerController.CachedHealthOverride;
			}
			else
			{
				this.m_currentHealth = playerController.CurrentHealthAsInt;
			}
			if (playerController.CachedManaOverride != 0f)
			{
				this.m_currentMana = (int)playerController.CachedManaOverride;
			}
			else
			{
				this.m_currentMana = playerController.CurrentManaAsInt;
			}
			this.m_currentMana = playerController.CurrentManaAsInt;
			this.m_shields = playerController.CurrentArmor;
			this.m_spellOrbs = playerController.SpellOrbs;
			this.m_currentExhaust = playerController.CurrentExhaust;
			if (SkillTreeManager.IsInitialized)
			{
				this.m_level = SkillTreeManager.GetTotalSkillObjLevel();
			}
			else
			{
				Debug.Log("<color=red>Failed to update cached player LEVEL DATA ONLY.  SkillTree is disposed or not instantiated yet.</color>");
			}
			this.m_cooldown = playerController.AbilityCoolDownMod;
			this.m_weight = playerController.ActualAllowedEquipmentWeight;
			this.m_runeWeight = playerController.ActualRuneWeight;
			this.m_baseArmor = playerController.BaseArmor;
			this.m_actualResolve = playerController.ActualResolve;
			this.m_actualMagic = (playerController.BaseMagic + (float)playerController.MagicAdd) * (1f + playerController.MagicMod);
			if (ChallengeManager.IsInChallenge)
			{
				this.m_actualMagic = ChallengeManager.ApplyStatCap(this.m_actualMagic, false);
			}
			this.m_moddedActualMagic = playerController.ActualMagic;
			this.m_actualStrength = (playerController.BaseStrength + (float)playerController.StrengthAdd) * (1f + playerController.StrengthMod);
			if (ChallengeManager.IsInChallenge)
			{
				this.m_actualStrength = ChallengeManager.ApplyStatCap(this.m_actualStrength, false);
			}
			this.m_moddedActualStrength = playerController.ActualStrength;
			this.m_actualVitality = Mathf.CeilToInt((float)(playerController.BaseVitality + playerController.VitalityAdd) * (1f + playerController.VitalityMod));
			if (ChallengeManager.IsInChallenge)
			{
				this.m_actualVitality = (int)ChallengeManager.ApplyStatCap((float)this.m_actualVitality, false);
			}
			this.m_moddedVitality = playerController.ActualVitality;
			this.m_dexterity = (playerController.BaseDexterity + playerController.DexterityAdd) * (1f + playerController.DexterityMod);
			if (ChallengeManager.IsInChallenge)
			{
				this.m_dexterity = ChallengeManager.ApplyStatCap(this.m_dexterity, true);
			}
			this.m_moddedDexterity = playerController.ActualDexterity;
			this.m_critChance = playerController.CritChanceAdd + 0f;
			this.m_moddedCritChance = playerController.ActualCritChance;
			this.m_critDamage = playerController.BaseCritDamage + playerController.CritDamageAdd;
			this.m_moddedCritDamage = playerController.ActualCritDamage;
			this.m_focus = (playerController.BaseFocus + playerController.FocusAdd) * (1f + playerController.FocusMod);
			if (ChallengeManager.IsInChallenge)
			{
				this.m_focus = ChallengeManager.ApplyStatCap(this.m_focus, true);
			}
			this.m_moddedFocus = playerController.ActualFocus;
			this.m_magicCritChance = playerController.MagicCritChanceAdd + 0f;
			this.m_moddedMagicCritChance = playerController.ActualMagicCritChance;
			this.m_magicCritDmg = playerController.BaseMagicCritDamage + playerController.MagicCritDamageAdd;
			this.m_moddedMagicCritDmg = playerController.ActualMagicCritDamage;
			BaseAbility_RL ability = playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
			this.m_currentWeaponAmmo = (ability ? ability.CurrentAmmo : 0);
			BaseAbility_RL ability2 = playerController.CastAbility.GetAbility(CastAbilityType.Spell, false);
			this.m_currentSpellAmmo = (ability2 ? ability2.CurrentAmmo : 0);
			BaseAbility_RL ability3 = playerController.CastAbility.GetAbility(CastAbilityType.Talent, false);
			this.m_currentTalentAmmo = (ability3 ? ability3.CurrentAmmo : 0);
			this.m_soulsCollected = Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true);
			this.LastCachedUpdate_FrameCount = Time.frameCount;
			return;
		}
		Debug.Log("<color=red>Failed to update cached player data.  PlayerManager is disposed or not instantiated yet.</color>");
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x00016669 File Offset: 0x00014869
	public CachedPlayerData Clone()
	{
		return base.MemberwiseClone() as CachedPlayerData;
	}

	// Token: 0x040022EA RID: 8938
	private int m_currentExhaust;

	// Token: 0x040022EB RID: 8939
	private int m_currentSpellAmmo;

	// Token: 0x040022EC RID: 8940
	private int m_currentTalentAmmo;

	// Token: 0x040022ED RID: 8941
	private int m_currentWeaponAmmo;

	// Token: 0x040022EE RID: 8942
	private float m_actualResolve;

	// Token: 0x040022EF RID: 8943
	private int m_currentHealth;

	// Token: 0x040022F0 RID: 8944
	private int m_currentMana;

	// Token: 0x040022F1 RID: 8945
	private int m_spellOrbs;

	// Token: 0x040022F2 RID: 8946
	private int m_shields;

	// Token: 0x040022F3 RID: 8947
	private int m_level;

	// Token: 0x040022F4 RID: 8948
	private int m_actualVitality;

	// Token: 0x040022F5 RID: 8949
	private int m_moddedVitality;

	// Token: 0x040022F6 RID: 8950
	private float m_actualStrength;

	// Token: 0x040022F7 RID: 8951
	private float m_moddedActualStrength;

	// Token: 0x040022F8 RID: 8952
	private float m_actualMagic;

	// Token: 0x040022F9 RID: 8953
	private float m_moddedActualMagic;

	// Token: 0x040022FA RID: 8954
	private float m_magicCritChance;

	// Token: 0x040022FB RID: 8955
	private float m_moddedMagicCritChance;

	// Token: 0x040022FC RID: 8956
	private float m_magicCritDmg;

	// Token: 0x040022FD RID: 8957
	private float m_moddedMagicCritDmg;

	// Token: 0x040022FE RID: 8958
	private float m_critChance;

	// Token: 0x040022FF RID: 8959
	private float m_moddedCritChance;

	// Token: 0x04002300 RID: 8960
	private float m_critDamage;

	// Token: 0x04002301 RID: 8961
	private float m_moddedCritDamage;

	// Token: 0x04002302 RID: 8962
	private float m_dexterity;

	// Token: 0x04002303 RID: 8963
	private float m_moddedDexterity;

	// Token: 0x04002304 RID: 8964
	private float m_focus;

	// Token: 0x04002305 RID: 8965
	private float m_moddedFocus;

	// Token: 0x04002306 RID: 8966
	private int m_weight;

	// Token: 0x04002307 RID: 8967
	private int m_runeWeight;

	// Token: 0x04002308 RID: 8968
	private float m_cooldown;

	// Token: 0x04002309 RID: 8969
	private int m_baseArmor;

	// Token: 0x0400230A RID: 8970
	private int m_soulsCollected;
}
