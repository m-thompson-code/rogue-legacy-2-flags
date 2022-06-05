using System;
using UnityEngine;

// Token: 0x0200071C RID: 1820
public class CollisionType_RL
{
	// Token: 0x060040E7 RID: 16615 RVA: 0x000E5B20 File Offset: 0x000E3D20
	public static TagType GetEquivalentTag(CollisionType collisionType)
	{
		if (collisionType <= CollisionType.Chest)
		{
			if (collisionType <= CollisionType.Breakable)
			{
				switch (collisionType)
				{
				case CollisionType.All:
				case CollisionType.None:
					return TagType.Untagged;
				case CollisionType.Player:
					return TagType.Player;
				case CollisionType.PlayerProjectile:
					return TagType.PlayerProjectile;
				case CollisionType.Player | CollisionType.PlayerProjectile:
				case CollisionType.Player | CollisionType.Enemy:
				case CollisionType.PlayerProjectile | CollisionType.Enemy:
				case CollisionType.Player | CollisionType.PlayerProjectile | CollisionType.Enemy:
					break;
				case CollisionType.Enemy:
					return TagType.Enemy;
				case CollisionType.EnemyProjectile:
					return TagType.EnemyProjectile;
				default:
					if (collisionType == CollisionType.ItemDrop)
					{
						return TagType.ItemDrop;
					}
					if (collisionType == CollisionType.Breakable)
					{
						return TagType.Breakable;
					}
					break;
				}
			}
			else
			{
				if (collisionType == CollisionType.Hazard)
				{
					return TagType.Hazard;
				}
				if (collisionType == CollisionType.Platform)
				{
					return TagType.Platform;
				}
				if (collisionType == CollisionType.Chest)
				{
					return TagType.Chest;
				}
			}
		}
		else if (collisionType <= CollisionType.Player_Dodging)
		{
			if (collisionType == CollisionType.NPC)
			{
				return TagType.NPC;
			}
			if (collisionType == CollisionType.TriggerHazard)
			{
				return TagType.TriggerHazard;
			}
			if (collisionType == CollisionType.Player_Dodging)
			{
				return TagType.Player_Dodging;
			}
		}
		else
		{
			if (collisionType == CollisionType.FlimsyBreakable)
			{
				return TagType.FlimsyBreakable;
			}
			if (collisionType == CollisionType.Generic_Bounceable)
			{
				return TagType.Generic_Bounceable;
			}
			if (collisionType == CollisionType.NonResonant_Bounceable)
			{
				return TagType.NonResonant_Bounceable;
			}
		}
		throw new Exception("Cannot find Tag from CollisionType: " + collisionType.ToString() + ". Please make sure any tags related to collision are added to CollisionType_RL.");
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x000E5C1C File Offset: 0x000E3E1C
	public static CollisionType GetEquivalentCollisionType(TagType tagType)
	{
		if (tagType != TagType.Untagged)
		{
			switch (tagType)
			{
			case TagType.Player:
				return CollisionType.Player;
			case TagType.GameController:
			case TagType.LevelBounds:
			case TagType.Music:
			case TagType.Background:
			case TagType.Door:
			case TagType.Room:
			case TagType.Projectile:
				return CollisionType.None;
			case TagType.Platform:
			case TagType.OneWay:
				break;
			case TagType.Enemy:
				return CollisionType.Enemy;
			case TagType.Breakable:
				return CollisionType.Breakable;
			case TagType.ItemDrop:
				return CollisionType.ItemDrop;
			case TagType.Hazard:
				return CollisionType.Hazard;
			case TagType.PlayerProjectile:
				return CollisionType.PlayerProjectile;
			case TagType.EnemyProjectile:
				return CollisionType.EnemyProjectile;
			case TagType.Chest:
				return CollisionType.Chest;
			default:
				switch (tagType)
				{
				case TagType.NPC:
					return CollisionType.NPC;
				case TagType.Barricade:
				case TagType.Effect:
				case TagType.SpawnController:
				case TagType.TeleporterTube:
				case TagType.PlayerStatusEffect:
				case TagType.EnemyStatusEffect:
					return CollisionType.None;
				case TagType.TriggerHazard:
					return CollisionType.TriggerHazard;
				case TagType.Player_Dodging:
					return CollisionType.Player_Dodging;
				case TagType.FlimsyBreakable:
					return CollisionType.FlimsyBreakable;
				case TagType.Generic_Bounceable:
					return CollisionType.Generic_Bounceable;
				case TagType.MagicPlatform:
					break;
				case TagType.NonResonant_Bounceable:
					return CollisionType.NonResonant_Bounceable;
				default:
					return CollisionType.None;
				}
				break;
			}
			return CollisionType.Platform;
		}
		return CollisionType.None;
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x000E5CFC File Offset: 0x000E3EFC
	public static bool IsProjectile(GameObject obj)
	{
		return obj.CompareTag("Projectile") || obj.CompareTag("PlayerProjectile") || obj.CompareTag("EnemyProjectile");
	}

	// Token: 0x1700162B RID: 5675
	// (get) Token: 0x060040EA RID: 16618 RVA: 0x000E5D28 File Offset: 0x000E3F28
	public static CollisionType[] TypeArray
	{
		get
		{
			if (CollisionType_RL.m_typeArray == null)
			{
				CollisionType_RL.m_typeArray = (Enum.GetValues(typeof(CollisionType)) as CollisionType[]);
			}
			return CollisionType_RL.m_typeArray;
		}
	}

	// Token: 0x04003364 RID: 13156
	private static CollisionType[] m_typeArray;
}
