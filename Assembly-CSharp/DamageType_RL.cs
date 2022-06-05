using System;

// Token: 0x02000722 RID: 1826
public static class DamageType_RL
{
	// Token: 0x060040F0 RID: 16624 RVA: 0x000E5FB2 File Offset: 0x000E41B2
	public static DamageType ToEnum(string damageType)
	{
		if (damageType == null || damageType == "Strength" || !(damageType == "Magic"))
		{
			return DamageType.Strength;
		}
		return DamageType.Magic;
	}

	// Token: 0x0400338A RID: 13194
	public const string Strength_String = "Strength";

	// Token: 0x0400338B RID: 13195
	public const string Magic_String = "Magic";
}
