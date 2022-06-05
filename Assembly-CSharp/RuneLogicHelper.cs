using System;
using UnityEngine;

// Token: 0x020002C5 RID: 709
public static class RuneLogicHelper
{
	// Token: 0x06001C35 RID: 7221 RVA: 0x0005B66C File Offset: 0x0005986C
	public static int GetExtraJumps()
	{
		return RuneManager.GetRuneEquippedLevel(RuneType.DoubleJump);
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x0005B675 File Offset: 0x00059875
	public static int GetExtraDashes()
	{
		return RuneManager.GetRuneEquippedLevel(RuneType.Dash);
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x0005B680 File Offset: 0x00059880
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

	// Token: 0x06001C38 RID: 7224 RVA: 0x0005B6B8 File Offset: 0x000598B8
	public static int GetNumLifeStealRunes()
	{
		int runeEquippedLevel = RuneManager.GetRuneEquippedLevel(RuneType.Lifesteal);
		int num = (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.LifeSteal);
		return runeEquippedLevel + num;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x0005B6D8 File Offset: 0x000598D8
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

	// Token: 0x06001C3A RID: 7226 RVA: 0x0005B714 File Offset: 0x00059914
	public static int GetNumSoulStealRunes()
	{
		int runeEquippedLevel = RuneManager.GetRuneEquippedLevel(RuneType.SoulSteal);
		int num = (int)EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.SoulSteal);
		return runeEquippedLevel + num;
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x0005B739 File Offset: 0x00059939
	public static float GetGoldGainPercent()
	{
		return RuneManager.GetRune(RuneType.GoldGain).CurrentStatModTotal_1;
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x0005B747 File Offset: 0x00059947
	public static float GetManaRegenPercent()
	{
		return RuneManager.GetRune(RuneType.ManaRegen).CurrentStatModTotal_1;
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x0005B758 File Offset: 0x00059958
	public static float GetDamageReturnPercent(int numRunes)
	{
		if (numRunes <= 0)
		{
			return 0f;
		}
		RuneData runeData = RuneLibrary.GetRuneData(RuneType.ReturnDamage);
		return runeData.StatMod01 + (float)(numRunes - 1) * runeData.ScalingStatMod01;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x0005B789 File Offset: 0x00059989
	public static int GetMaxManaFlat()
	{
		return Mathf.RoundToInt(RuneManager.GetRune(RuneType.MaxMana).CurrentStatModTotal_1);
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x0005B79F File Offset: 0x0005999F
	public static float GetHastePercent()
	{
		return RuneManager.GetRune(RuneType.Haste).CurrentStatModTotal_1;
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x0005B7AD File Offset: 0x000599AD
	public static float GetOreGainPercent()
	{
		return RuneManager.GetRune(RuneType.OreGain).CurrentStatModTotal_1;
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x0005B7BE File Offset: 0x000599BE
	public static float GetRuneOreGainPercent()
	{
		return RuneManager.GetRune(RuneType.RuneOreGain).CurrentStatModTotal_1;
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x0005B7CF File Offset: 0x000599CF
	public static float GetMagnetDistance()
	{
		return RuneManager.GetRune(RuneType.Magnet).CurrentStatModTotal_1;
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x0005B7DD File Offset: 0x000599DD
	public static int GetManaRegenOnSpinKick()
	{
		return (int)RuneManager.GetRune(RuneType.ManaOnSpinKick).CurrentStatModTotal_1;
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x0005B7EF File Offset: 0x000599EF
	public static float GetStatusEffectDurationMod()
	{
		return RuneManager.GetRune(RuneType.StatusEffectDuration).CurrentStatModTotal_1;
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x0005B800 File Offset: 0x00059A00
	public static float GetArmorRegenMod()
	{
		return RuneManager.GetRune(RuneType.ArmorRegen).CurrentStatModTotal_1;
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x0005B80E File Offset: 0x00059A0E
	public static float GetArmorMinBlockAdd()
	{
		return RuneManager.GetRune(RuneType.ArmorMinBlock).CurrentStatModTotal_1;
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x0005B81F File Offset: 0x00059A1F
	public static float GetResolveAdd()
	{
		return RuneManager.GetRune(RuneType.ResolveGain).CurrentStatModTotal_1;
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x0005B830 File Offset: 0x00059A30
	public static float GetWeaponCritChanceAdd()
	{
		return RuneManager.GetRune(RuneType.WeaponCritChanceAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x0005B841 File Offset: 0x00059A41
	public static float GetMagicCritChanceAdd()
	{
		return RuneManager.GetRune(RuneType.MagicCritChanceAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x0005B852 File Offset: 0x00059A52
	public static float GetWeaponCritDamageAdd()
	{
		return RuneManager.GetRune(RuneType.WeaponCritDamageAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x0005B863 File Offset: 0x00059A63
	public static float GetMagicCritDamageAdd()
	{
		return RuneManager.GetRune(RuneType.MagicCritDamageAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x0005B874 File Offset: 0x00059A74
	public static float GetSuperCritChanceAdd()
	{
		return RuneManager.GetRune(RuneType.SuperCritChanceAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x0005B885 File Offset: 0x00059A85
	public static float GetSuperCritDamageAdd()
	{
		return RuneManager.GetRune(RuneType.SuperCritDamageAdd).CurrentStatModTotal_1;
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x0005B896 File Offset: 0x00059A96
	public static float GetArmorHealthMod()
	{
		return RuneManager.GetRune(RuneType.ArmorHealth).CurrentStatModTotal_1;
	}
}
