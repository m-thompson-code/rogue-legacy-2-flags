using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020002D9 RID: 729
[Serializable]
public class CachedPlayerData
{
	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x0005F4E8 File Offset: 0x0005D6E8
	public int CurrentExhaust
	{
		get
		{
			return this.m_currentExhaust;
		}
	}

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x06001CEA RID: 7402 RVA: 0x0005F4F0 File Offset: 0x0005D6F0
	public int CurrentSpellAmmo
	{
		get
		{
			return this.m_currentSpellAmmo;
		}
	}

	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x06001CEB RID: 7403 RVA: 0x0005F4F8 File Offset: 0x0005D6F8
	public int CurrentTalentAmmo
	{
		get
		{
			return this.m_currentTalentAmmo;
		}
	}

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0005F500 File Offset: 0x0005D700
	public int CurrentWeaponAmmo
	{
		get
		{
			return this.m_currentWeaponAmmo;
		}
	}

	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x06001CED RID: 7405 RVA: 0x0005F508 File Offset: 0x0005D708
	public float ActualResolve
	{
		get
		{
			return this.m_actualResolve;
		}
	}

	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0005F510 File Offset: 0x0005D710
	public int CurrentHealth
	{
		get
		{
			return this.m_currentHealth;
		}
	}

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x06001CEF RID: 7407 RVA: 0x0005F518 File Offset: 0x0005D718
	public int CurrentMana
	{
		get
		{
			return this.m_currentMana;
		}
	}

	// Token: 0x17000CB3 RID: 3251
	// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0005F520 File Offset: 0x0005D720
	public int SpellOrbs
	{
		get
		{
			return this.m_spellOrbs;
		}
	}

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x0005F528 File Offset: 0x0005D728
	public int CurrentArmor
	{
		get
		{
			return this.m_shields;
		}
	}

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0005F530 File Offset: 0x0005D730
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x0005F538 File Offset: 0x0005D738
	public int VitalityStat
	{
		get
		{
			return this.m_actualVitality;
		}
	}

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x0005F540 File Offset: 0x0005D740
	public int ModdedVitalityStat
	{
		get
		{
			return this.m_moddedVitality;
		}
	}

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x0005F548 File Offset: 0x0005D748
	public float StrengthStat
	{
		get
		{
			return this.m_actualStrength;
		}
	}

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0005F550 File Offset: 0x0005D750
	public float ModdedStrengthStat
	{
		get
		{
			return this.m_moddedActualStrength;
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x0005F558 File Offset: 0x0005D758
	public float MagicStat
	{
		get
		{
			return this.m_actualMagic;
		}
	}

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0005F560 File Offset: 0x0005D760
	public float ModdedMagicStat
	{
		get
		{
			return this.m_moddedActualMagic;
		}
	}

	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0005F568 File Offset: 0x0005D768
	public float MagicCritChance
	{
		get
		{
			return this.m_magicCritChance;
		}
	}

	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0005F570 File Offset: 0x0005D770
	public float ModdedMagicCritChance
	{
		get
		{
			return this.m_moddedMagicCritChance;
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x06001CFB RID: 7419 RVA: 0x0005F578 File Offset: 0x0005D778
	public float MagicCritDmg
	{
		get
		{
			return this.m_magicCritDmg;
		}
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0005F580 File Offset: 0x0005D780
	public float ModdedMagicCritDmg
	{
		get
		{
			return this.m_moddedMagicCritDmg;
		}
	}

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x06001CFD RID: 7421 RVA: 0x0005F588 File Offset: 0x0005D788
	public float CritChance
	{
		get
		{
			return this.m_critChance;
		}
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0005F590 File Offset: 0x0005D790
	public float ModdedCritChance
	{
		get
		{
			return this.m_moddedCritChance;
		}
	}

	// Token: 0x17000CC2 RID: 3266
	// (get) Token: 0x06001CFF RID: 7423 RVA: 0x0005F598 File Offset: 0x0005D798
	public float CritDamage
	{
		get
		{
			return this.m_critDamage;
		}
	}

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x06001D00 RID: 7424 RVA: 0x0005F5A0 File Offset: 0x0005D7A0
	public float ModdedCritDamage
	{
		get
		{
			return this.m_moddedCritDamage;
		}
	}

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x06001D01 RID: 7425 RVA: 0x0005F5A8 File Offset: 0x0005D7A8
	public float DexterityStat
	{
		get
		{
			return this.m_dexterity;
		}
	}

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0005F5B0 File Offset: 0x0005D7B0
	public float ModdedDexterityStat
	{
		get
		{
			return this.m_moddedDexterity;
		}
	}

	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x06001D03 RID: 7427 RVA: 0x0005F5B8 File Offset: 0x0005D7B8
	public float FocusStat
	{
		get
		{
			return this.m_focus;
		}
	}

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x06001D04 RID: 7428 RVA: 0x0005F5C0 File Offset: 0x0005D7C0
	public float ModdedFocusStat
	{
		get
		{
			return this.m_moddedFocus;
		}
	}

	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x06001D05 RID: 7429 RVA: 0x0005F5C8 File Offset: 0x0005D7C8
	public int Weight
	{
		get
		{
			return this.m_weight;
		}
	}

	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x06001D06 RID: 7430 RVA: 0x0005F5D0 File Offset: 0x0005D7D0
	public int RuneWeight
	{
		get
		{
			return this.m_runeWeight;
		}
	}

	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x06001D07 RID: 7431 RVA: 0x0005F5D8 File Offset: 0x0005D7D8
	public float Cooldown
	{
		get
		{
			return this.m_cooldown;
		}
	}

	// Token: 0x17000CCB RID: 3275
	// (get) Token: 0x06001D08 RID: 7432 RVA: 0x0005F5E0 File Offset: 0x0005D7E0
	public int BaseArmor
	{
		get
		{
			return this.m_baseArmor;
		}
	}

	// Token: 0x17000CCC RID: 3276
	// (get) Token: 0x06001D09 RID: 7433 RVA: 0x0005F5E8 File Offset: 0x0005D7E8
	public int SoulsCollected
	{
		get
		{
			return this.m_soulsCollected;
		}
	}

	// Token: 0x17000CCD RID: 3277
	// (get) Token: 0x06001D0A RID: 7434 RVA: 0x0005F5F0 File Offset: 0x0005D7F0
	// (set) Token: 0x06001D0B RID: 7435 RVA: 0x0005F5F8 File Offset: 0x0005D7F8
	public int LastCachedUpdate_FrameCount { get; private set; }

	// Token: 0x06001D0C RID: 7436 RVA: 0x0005F604 File Offset: 0x0005D804
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

	// Token: 0x06001D0D RID: 7437 RVA: 0x0005F648 File Offset: 0x0005D848
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

	// Token: 0x06001D0E RID: 7438 RVA: 0x0005F9BF File Offset: 0x0005DBBF
	public CachedPlayerData Clone()
	{
		return base.MemberwiseClone() as CachedPlayerData;
	}

	// Token: 0x04001AE3 RID: 6883
	private int m_currentExhaust;

	// Token: 0x04001AE4 RID: 6884
	private int m_currentSpellAmmo;

	// Token: 0x04001AE5 RID: 6885
	private int m_currentTalentAmmo;

	// Token: 0x04001AE6 RID: 6886
	private int m_currentWeaponAmmo;

	// Token: 0x04001AE7 RID: 6887
	private float m_actualResolve;

	// Token: 0x04001AE8 RID: 6888
	private int m_currentHealth;

	// Token: 0x04001AE9 RID: 6889
	private int m_currentMana;

	// Token: 0x04001AEA RID: 6890
	private int m_spellOrbs;

	// Token: 0x04001AEB RID: 6891
	private int m_shields;

	// Token: 0x04001AEC RID: 6892
	private int m_level;

	// Token: 0x04001AED RID: 6893
	private int m_actualVitality;

	// Token: 0x04001AEE RID: 6894
	private int m_moddedVitality;

	// Token: 0x04001AEF RID: 6895
	private float m_actualStrength;

	// Token: 0x04001AF0 RID: 6896
	private float m_moddedActualStrength;

	// Token: 0x04001AF1 RID: 6897
	private float m_actualMagic;

	// Token: 0x04001AF2 RID: 6898
	private float m_moddedActualMagic;

	// Token: 0x04001AF3 RID: 6899
	private float m_magicCritChance;

	// Token: 0x04001AF4 RID: 6900
	private float m_moddedMagicCritChance;

	// Token: 0x04001AF5 RID: 6901
	private float m_magicCritDmg;

	// Token: 0x04001AF6 RID: 6902
	private float m_moddedMagicCritDmg;

	// Token: 0x04001AF7 RID: 6903
	private float m_critChance;

	// Token: 0x04001AF8 RID: 6904
	private float m_moddedCritChance;

	// Token: 0x04001AF9 RID: 6905
	private float m_critDamage;

	// Token: 0x04001AFA RID: 6906
	private float m_moddedCritDamage;

	// Token: 0x04001AFB RID: 6907
	private float m_dexterity;

	// Token: 0x04001AFC RID: 6908
	private float m_moddedDexterity;

	// Token: 0x04001AFD RID: 6909
	private float m_focus;

	// Token: 0x04001AFE RID: 6910
	private float m_moddedFocus;

	// Token: 0x04001AFF RID: 6911
	private int m_weight;

	// Token: 0x04001B00 RID: 6912
	private int m_runeWeight;

	// Token: 0x04001B01 RID: 6913
	private float m_cooldown;

	// Token: 0x04001B02 RID: 6914
	private int m_baseArmor;

	// Token: 0x04001B03 RID: 6915
	private int m_soulsCollected;
}
