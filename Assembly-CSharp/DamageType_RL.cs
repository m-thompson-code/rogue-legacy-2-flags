using System;

// Token: 0x02000BD5 RID: 3029
public static class DamageType_RL
{
	// Token: 0x06005A3A RID: 23098 RVA: 0x0003161E File Offset: 0x0002F81E
	public static DamageType ToEnum(string damageType)
	{
		if (damageType == null || damageType == "Strength" || !(damageType == "Magic"))
		{
			return DamageType.Strength;
		}
		return DamageType.Magic;
	}

	// Token: 0x04004606 RID: 17926
	public const string Strength_String = "Strength";

	// Token: 0x04004607 RID: 17927
	public const string Magic_String = "Magic";
}
