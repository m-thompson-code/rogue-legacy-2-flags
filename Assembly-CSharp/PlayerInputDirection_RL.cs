using System;
using UnityEngine;

// Token: 0x0200075E RID: 1886
public static class PlayerInputDirection_RL
{
	// Token: 0x06004123 RID: 16675 RVA: 0x000E7094 File Offset: 0x000E5294
	public static Vector2Int GetInputDirectionVector(PlayerInputDirection direction)
	{
		switch (direction)
		{
		default:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_DOWN;
		case PlayerInputDirection.DownLeft:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_DOWN_LEFT;
		case PlayerInputDirection.Left:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_LEFT;
		case PlayerInputDirection.UpLeft:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_UP_LEFT;
		case PlayerInputDirection.Up:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_UP;
		case PlayerInputDirection.UpRight:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_UP_RIGHT;
		case PlayerInputDirection.Right:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_RIGHT;
		case PlayerInputDirection.DownRight:
			return PlayerInputDirection_RL.CDG_8_DIRECTION_ANGLE_DOWN_RIGHT;
		}
	}

	// Token: 0x06004124 RID: 16676 RVA: 0x000E70F8 File Offset: 0x000E52F8
	public static bool InputDirectionPressed(float degrees, PlayerInputDirection direction)
	{
		degrees = CDGHelper.WrapAngleDegrees(degrees, false);
		Vector2Int inputDirectionVector = PlayerInputDirection_RL.GetInputDirectionVector(direction);
		if (inputDirectionVector.x > inputDirectionVector.y)
		{
			return degrees >= (float)inputDirectionVector.x || degrees <= (float)inputDirectionVector.y;
		}
		return degrees >= (float)inputDirectionVector.x && degrees <= (float)inputDirectionVector.y;
	}

	// Token: 0x06004125 RID: 16677 RVA: 0x000E7160 File Offset: 0x000E5360
	public static PlayerInputDirection GetInputDirection(float degrees)
	{
		foreach (PlayerInputDirection playerInputDirection in PlayerInputDirection_RL.TypeArray)
		{
			if (PlayerInputDirection_RL.InputDirectionPressed(degrees, playerInputDirection))
			{
				return playerInputDirection;
			}
		}
		return PlayerInputDirection.Down;
	}

	// Token: 0x1700163D RID: 5693
	// (get) Token: 0x06004126 RID: 16678 RVA: 0x000E7191 File Offset: 0x000E5391
	public static PlayerInputDirection[] TypeArray
	{
		get
		{
			if (PlayerInputDirection_RL.m_typeArray == null)
			{
				PlayerInputDirection_RL.m_typeArray = (Enum.GetValues(typeof(PlayerInputDirection)) as PlayerInputDirection[]);
			}
			return PlayerInputDirection_RL.m_typeArray;
		}
	}

	// Token: 0x04003598 RID: 13720
	private static Vector2Int CDG_8_DIRECTION_ANGLE_DOWN = new Vector2Int(254, 285);

	// Token: 0x04003599 RID: 13721
	private static Vector2Int CDG_8_DIRECTION_ANGLE_DOWN_LEFT = new Vector2Int(210, 253);

	// Token: 0x0400359A RID: 13722
	private static Vector2Int CDG_8_DIRECTION_ANGLE_LEFT = new Vector2Int(150, 209);

	// Token: 0x0400359B RID: 13723
	private static Vector2Int CDG_8_DIRECTION_ANGLE_UP_LEFT = new Vector2Int(106, 149);

	// Token: 0x0400359C RID: 13724
	private static Vector2Int CDG_8_DIRECTION_ANGLE_UP = new Vector2Int(74, 105);

	// Token: 0x0400359D RID: 13725
	private static Vector2Int CDG_8_DIRECTION_ANGLE_UP_RIGHT = new Vector2Int(30, 73);

	// Token: 0x0400359E RID: 13726
	private static Vector2Int CDG_8_DIRECTION_ANGLE_RIGHT = new Vector2Int(330, 29);

	// Token: 0x0400359F RID: 13727
	private static Vector2Int CDG_8_DIRECTION_ANGLE_DOWN_RIGHT = new Vector2Int(286, 329);

	// Token: 0x040035A0 RID: 13728
	private static PlayerInputDirection[] m_typeArray;
}
