using System;

// Token: 0x0200077C RID: 1916
public class StatusEffectType_RL
{
	// Token: 0x06004153 RID: 16723 RVA: 0x000E83E4 File Offset: 0x000E65E4
	public static StatusBarEntryType GetStatusBarType(StatusEffectType statusEffectType)
	{
		if (statusEffectType <= StatusEffectType.Enemy_AppliesInvuln)
		{
			if (statusEffectType <= StatusEffectType.Enemy_ArmorBreak)
			{
				if (statusEffectType <= StatusEffectType.Enemy_Burn_Immunity)
				{
					if (statusEffectType <= StatusEffectType.Enemy_FreezeImmunity)
					{
						if (statusEffectType == StatusEffectType.Enemy_Freeze)
						{
							return StatusBarEntryType.Freeze;
						}
						if (statusEffectType != StatusEffectType.Enemy_FreezeImmunity)
						{
							return StatusBarEntryType.None;
						}
						return StatusBarEntryType.FreezeImmunity;
					}
					else
					{
						if (statusEffectType == StatusEffectType.Enemy_Burn)
						{
							return StatusBarEntryType.Burn;
						}
						if (statusEffectType != StatusEffectType.Enemy_Burn_Immunity)
						{
							return StatusBarEntryType.None;
						}
						return StatusBarEntryType.BurnImmunity;
					}
				}
				else if (statusEffectType <= StatusEffectType.Enemy_ManaBurn)
				{
					if (statusEffectType == StatusEffectType.Enemy_Poison)
					{
						return StatusBarEntryType.Poison;
					}
					if (statusEffectType != StatusEffectType.Enemy_ManaBurn)
					{
						return StatusBarEntryType.None;
					}
					return StatusBarEntryType.ManaBurn;
				}
				else
				{
					if (statusEffectType == StatusEffectType.Enemy_SporeBurst)
					{
						return StatusBarEntryType.SporeBurst;
					}
					if (statusEffectType != StatusEffectType.Enemy_ArmorBreak)
					{
						return StatusBarEntryType.None;
					}
					return StatusBarEntryType.ArmorBreak;
				}
			}
			else if (statusEffectType <= StatusEffectType.Enemy_Dizzy)
			{
				if (statusEffectType <= StatusEffectType.Enemy_Phased)
				{
					if (statusEffectType == StatusEffectType.Enemy_MagicBreak)
					{
						return StatusBarEntryType.MagicBreak;
					}
					if (statusEffectType != StatusEffectType.Enemy_Phased)
					{
						return StatusBarEntryType.None;
					}
					return StatusBarEntryType.Phased;
				}
				else
				{
					if (statusEffectType == StatusEffectType.Enemy_Knockout)
					{
						return StatusBarEntryType.Knockout;
					}
					if (statusEffectType != StatusEffectType.Enemy_Dizzy)
					{
						return StatusBarEntryType.None;
					}
					return StatusBarEntryType.Dizzy;
				}
			}
			else if (statusEffectType <= StatusEffectType.Enemy_Combo)
			{
				if (statusEffectType == StatusEffectType.Enemy_DeathDelay)
				{
					return StatusBarEntryType.DeathDelay;
				}
				if (statusEffectType != StatusEffectType.Enemy_Combo)
				{
					return StatusBarEntryType.None;
				}
			}
			else
			{
				if (statusEffectType == StatusEffectType.Enemy_Size)
				{
					return StatusBarEntryType.Size;
				}
				if (statusEffectType == StatusEffectType.Enemy_Speed)
				{
					return StatusBarEntryType.Speed;
				}
				if (statusEffectType != StatusEffectType.Enemy_AppliesInvuln)
				{
					return StatusBarEntryType.None;
				}
				return StatusBarEntryType.AppliesInvuln;
			}
		}
		else if (statusEffectType <= StatusEffectType.Enemy_Vulnerable)
		{
			if (statusEffectType <= StatusEffectType.Enemy_Curse_Projectile)
			{
				if (statusEffectType <= StatusEffectType.Enemy_FreeHit)
				{
					if (statusEffectType == StatusEffectType.Enemy_Invuln)
					{
						return StatusBarEntryType.Invuln;
					}
					if (statusEffectType != StatusEffectType.Enemy_FreeHit)
					{
						return StatusBarEntryType.None;
					}
					return StatusBarEntryType.FreeHit;
				}
				else
				{
					if (statusEffectType == StatusEffectType.Enemy_Disarm)
					{
						return StatusBarEntryType.Disarm;
					}
					if (statusEffectType != StatusEffectType.Enemy_Curse_Projectile)
					{
						return StatusBarEntryType.None;
					}
					return StatusBarEntryType.CurseProjectile;
				}
			}
			else if (statusEffectType <= StatusEffectType.Enemy_ArmorShred)
			{
				if (statusEffectType == StatusEffectType.Enemy_Explode)
				{
					return StatusBarEntryType.Explode;
				}
				if (statusEffectType != StatusEffectType.Enemy_ArmorShred)
				{
					return StatusBarEntryType.None;
				}
				return StatusBarEntryType.ArmorShred;
			}
			else
			{
				if (statusEffectType == StatusEffectType.Enemy_Bleed)
				{
					return StatusBarEntryType.Bleed;
				}
				switch (statusEffectType)
				{
				case StatusEffectType.Enemy_Bane:
					return StatusBarEntryType.Bane;
				case (StatusEffectType)261:
				case (StatusEffectType)263:
				case (StatusEffectType)265:
				case (StatusEffectType)267:
				case (StatusEffectType)269:
					return StatusBarEntryType.None;
				case StatusEffectType.Enemy_InvulnWindow:
					return StatusBarEntryType.InvulnWindow;
				case StatusEffectType.Enemy_Aggro:
					return StatusBarEntryType.Aggro;
				case StatusEffectType.Enemy_Flamer:
					return StatusBarEntryType.Flamer;
				case StatusEffectType.Enemy_InvulnTimer:
					return StatusBarEntryType.InvulnTimer;
				case StatusEffectType.Enemy_Vulnerable:
					return StatusBarEntryType.Vulnerable;
				default:
					return StatusBarEntryType.None;
				}
			}
		}
		else if (statusEffectType <= StatusEffectType.Player_Exhaust)
		{
			if (statusEffectType <= StatusEffectType.Player_Suave)
			{
				if (statusEffectType == StatusEffectType.Player_GodMode)
				{
					return StatusBarEntryType.GodMode;
				}
				if (statusEffectType != StatusEffectType.Player_Suave)
				{
					return StatusBarEntryType.None;
				}
				return StatusBarEntryType.Suave;
			}
			else
			{
				if (statusEffectType == StatusEffectType.Player_Disarmed)
				{
					return StatusBarEntryType.Disarmed;
				}
				if (statusEffectType != StatusEffectType.Player_Exhaust)
				{
					return StatusBarEntryType.None;
				}
				return StatusBarEntryType.Exhaust;
			}
		}
		else if (statusEffectType <= StatusEffectType.Player_FreeCrit)
		{
			if (statusEffectType == StatusEffectType.Player_Dance)
			{
				return StatusBarEntryType.Dance;
			}
			if (statusEffectType != StatusEffectType.Player_FreeCrit)
			{
				return StatusBarEntryType.None;
			}
			return StatusBarEntryType.FreeCrit;
		}
		else if (statusEffectType != StatusEffectType.Player_Combo)
		{
			if (statusEffectType == StatusEffectType.Player_NoContactDamage)
			{
				return StatusBarEntryType.NoContactDamage;
			}
			if (statusEffectType != StatusEffectType.Player_Cloak)
			{
				return StatusBarEntryType.None;
			}
			return StatusBarEntryType.Cloak;
		}
		return StatusBarEntryType.Combo;
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x000E8688 File Offset: 0x000E6888
	public static StatusEffectType GetStatusEffectType(StatusBarEntryType statusBarType)
	{
		switch (statusBarType)
		{
		case StatusBarEntryType.Cloak:
			return StatusEffectType.Player_Cloak;
		case StatusBarEntryType.Freeze:
			return StatusEffectType.Enemy_Freeze;
		case StatusBarEntryType.FreezeImmunity:
			return StatusEffectType.Enemy_FreezeImmunity;
		case StatusBarEntryType.Burn:
			return StatusEffectType.Enemy_Burn;
		case StatusBarEntryType.BurnImmunity:
			return StatusEffectType.Enemy_Burn_Immunity;
		case StatusBarEntryType.GodMode:
			return StatusEffectType.Player_GodMode;
		case StatusBarEntryType.ManaBurn:
			return StatusEffectType.Enemy_ManaBurn;
		case StatusBarEntryType.SporeBurst:
			return StatusEffectType.Enemy_SporeBurst;
		case StatusBarEntryType.ArmorBreak:
			return StatusEffectType.Enemy_ArmorBreak;
		case StatusBarEntryType.MagicBreak:
			return StatusEffectType.Enemy_MagicBreak;
		case StatusBarEntryType.Suave:
			return StatusEffectType.Player_Suave;
		case StatusBarEntryType.Phased:
			return StatusEffectType.Enemy_Phased;
		case StatusBarEntryType.Knockout:
			return StatusEffectType.Enemy_Knockout;
		case StatusBarEntryType.Dizzy:
			return StatusEffectType.Enemy_Dizzy;
		case StatusBarEntryType.DeathDelay:
			return StatusEffectType.Enemy_DeathDelay;
		case StatusBarEntryType.Combo:
			return StatusEffectType.Enemy_Combo;
		case StatusBarEntryType.Size:
			return StatusEffectType.Enemy_Size;
		case StatusBarEntryType.Speed:
			return StatusEffectType.Enemy_Speed;
		case StatusBarEntryType.AppliesInvuln:
			return StatusEffectType.Enemy_AppliesInvuln;
		case StatusBarEntryType.Invuln:
			return StatusEffectType.Enemy_Invuln;
		case StatusBarEntryType.FreeHit:
			return StatusEffectType.Enemy_FreeHit;
		case StatusBarEntryType.Disarm:
			return StatusEffectType.Enemy_Disarm;
		case StatusBarEntryType.Disarmed:
			return StatusEffectType.Player_Disarmed;
		case StatusBarEntryType.CurseProjectile:
			return StatusEffectType.Enemy_Curse_Projectile;
		case StatusBarEntryType.Exhaust:
			return StatusEffectType.Player_Exhaust;
		case StatusBarEntryType.Poison:
			return StatusEffectType.Enemy_Poison;
		case StatusBarEntryType.Bleed:
			return StatusEffectType.Enemy_Bleed;
		case StatusBarEntryType.Bane:
			return StatusEffectType.Enemy_Bane;
		case StatusBarEntryType.Dance:
			return StatusEffectType.Player_Dance;
		case StatusBarEntryType.Vulnerable:
			return StatusEffectType.Enemy_Vulnerable;
		case StatusBarEntryType.FreeCrit:
			return StatusEffectType.Player_FreeCrit;
		case StatusBarEntryType.NoContactDamage:
			return StatusEffectType.Player_NoContactDamage;
		case StatusBarEntryType.InvulnWindow:
			return StatusEffectType.Enemy_InvulnWindow;
		case StatusBarEntryType.Aggro:
			return StatusEffectType.Enemy_Aggro;
		case StatusBarEntryType.Flamer:
			return StatusEffectType.Enemy_Flamer;
		case StatusBarEntryType.InvulnTimer:
			return StatusEffectType.Enemy_InvulnTimer;
		}
		return StatusEffectType.None;
	}

	// Token: 0x1700164A RID: 5706
	// (get) Token: 0x06004155 RID: 16725 RVA: 0x000E87FE File Offset: 0x000E69FE
	public static StatusEffectType[] TypeArray
	{
		get
		{
			if (StatusEffectType_RL.m_typeArray == null)
			{
				StatusEffectType_RL.m_typeArray = (Enum.GetValues(typeof(StatusEffectType)) as StatusEffectType[]);
			}
			return StatusEffectType_RL.m_typeArray;
		}
	}

	// Token: 0x040037E0 RID: 14304
	private static StatusEffectType[] m_typeArray;
}
