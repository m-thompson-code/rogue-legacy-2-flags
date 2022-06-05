using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public static class RuneLogicHelper
{
	// Token: 0x060026FF RID: 9983 RVA: 0x00015E95 File Offset: 0x00014095
	public static int GetExtraJumps()
	{
		return RuneManager.GetRuneEquippedLevel(RuneType.DoubleJump);
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x00015E9E File Offset: 0x0001409E
	public static int GetExtraDashes()
	{
		return RuneManager.GetRuneEquippedLevel(RuneType.Dash);
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x000B7E80 File Offset: 0x000B6080
	public static float GetLifeStealPercent()
	{
		RuneData runeData = RuneLibrary.GetRuneData(RuneType.Lifesteal);
		int numLifeStealRunes = RuneLogicHelper.GetNumLifeStealRunes();
		if (numLifeStealRunes <= 0)
		{
			return 0f;
		}
		return runeData.StatMod01 + (float)(numLifeStealRunes - 1) * runeData.ScalingStatMod01;
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000B7EB8 File Offset: 0x000B60B8
	public static int GetNumLifeStealRunes()
	{
		int runeEquippedLevel = RuneManager.GetRuneEquippedLevel(RuneType.Lifesteal);
		int num = (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.LifeSteal);
		return runeEquippedLevel + num;
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000B7ED8 File Offset: 0x000B60D8
	public static float GetSoulStealPercent()
	{
		RuneData runeData = RuneLibrary.GetRuneData(RuneType.SoulSteal);
		int numSoulStealRunes = RuneLogicHelper.GetNumSoulStealRunes();
		if (numSoulStealRunes <= 0)
		{
			return 0f;
		}
		return runeData.StatMod01 + (float)(numSoulStealRunes - 1) * runeData.ScalingStatMod01;
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x000B7F14 File Offset: 0x000B6114
	public static int GetNumSoulStealRunes()
	{
		int runeEquippedLevel = RuneManager.GetRuneEquippedLevel(RuneType.SoulSteal);
		int num = (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.SoulSteal);
		return runeEquippedLevel + num;
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x00015EA7 File Offset: 0x000140A7
	public static float GetGoldGainPercent()
	{
		return RuneManager.GetRune(RuneType.GoldGain).CurrentStatModTotal_1;
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x00015EB5 File Offset: 0x000140B5
	public static float GetManaRegenPercent()
	{
		return RuneManager.GetRune(RuneType.ManaRegen).CurrentStatModTotal_1;
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000B7F3C File Offset: 0x000B613C
	public static float GetDamageReturnPercent(int numRunes)
	{
		if (numRunes <= 0)
		{
			return 0f;
		}
		RuneData runeData = RuneLibrary.GetRuneData(RuneType.ReturnDamage);
		return runeData.StatMod01 + (float)(numRunes - 1) * runeData.ScalingStatMod01;
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x00015EC6 File Offset: 0x000140C6
	public static int GetMaxManaFlat()
	{
		return Mathf.RoundToInt(RuneManager.GetRune(RuneType.MaxMana).CurrentStatModTotal_1);
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x00015EDC File Offset: 0x000140DC
	public static float GetHastePercent()
	{
		return RuneManager.GetRune(RuneType.Haste).CurrentStatModTotal_1;
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x00015EEA File Offset: 0x000140EA
	public static float GetOreGainPercent()
	{
		return RuneManager.GetRune(RuneType.OreGain).CurrentStatModTotal_1;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x00015EFB File Offset: 0x000140FB
	public static float GetRuneOreGainPercent()
	{
		return RuneManager.GetRune(RuneType.RuneOreGain).CurrentStatModTotal_1;
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x00015F0C File Offset: 0x0001410C
	public static float GetMagnetDistance()
	{
		return RuneManager.GetRune(RuneType.Magnet).CurrentStatModTotal_1;
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x00015F1A File Offset: 0x0001411A
	public static int GetManaRegenOnSpinKick()
	{
		return (int)RuneManager.GetRune(RuneType.ManaOnSpinKick).CurrentStatModTotal_1;
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x00015F2C File Offset: 0x0001412C
	public static float GetStatusEffectDurationMod()
	{
		return RuneManager.GetRune(RuneType.StatusEffectDuration).CurrentStatModTotal_1;
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x00015F3D File Offset: 0x0001413D
	public static float GetArmorRegenMod()
	{
		return RuneManager.GetRune(RuneType.ArmorRegen).CurrentStatModTotal_1;
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x00015F4B File Offset: 0x0001414B
	public static float GetArmorMinBlockAdd()
	{
		return RuneManager.GetRune(RuneType.ArmorMinBlock).CurrentStatModTotal_1;
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x00015F5C File Offset: 0x0001415C
	public static float GetResolveAdd()
	{
		return RuneManager.GetRune(RuneType.ResolveGain).CurrentStatModTotal_1;
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x00015F6D File Offset: 0x0001416D
	public static float GetWeaponCritChanceAdd()
	{
		return RuneManager.GetRune(RuneType.WeaponCritChanceAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x00015F7E File Offset: 0x0001417E
	public static float GetMagicCritChanceAdd()
	{
		return RuneManager.GetRune(RuneType.MagicCritChanceAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x00015F8F File Offset: 0x0001418F
	public static float GetWeaponCritDamageAdd()
	{
		return RuneManager.GetRune(RuneType.WeaponCritDamageAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x00015FA0 File Offset: 0x000141A0
	public static float GetMagicCritDamageAdd()
	{
		return RuneManager.GetRune(RuneType.MagicCritDamageAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x00015FB1 File Offset: 0x000141B1
	public static float GetSuperCritChanceAdd()
	{
		return RuneManager.GetRune(RuneType.SuperCritChanceAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x00015FC2 File Offset: 0x000141C2
	public static float GetSuperCritDamageAdd()
	{
		return RuneManager.GetRune(RuneType.SuperCritDamageAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x00015FD3 File Offset: 0x000141D3
	public static float GetArmorHealthMod()
	{
		return RuneManager.GetRune(RuneType.ArmorHealth).CurrentStatModTotal_1;
	}
}
