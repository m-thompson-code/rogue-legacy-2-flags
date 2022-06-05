using System;
using UnityEngine;

// Token: 0x02000C1C RID: 3100
public static class PlayerInputDirection_RL
{
	// Token: 0x06005AA0 RID: 23200 RVA: 0x00156834 File Offset: 0x00154A34
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

	// Token: 0x06005AA1 RID: 23201 RVA: 0x00156898 File Offset: 0x00154A98
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

	// Token: 0x06005AA2 RID: 23202 RVA: 0x00156900 File Offset: 0x00154B00
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

	// Token: 0x17001E39 RID: 7737
	// (get) Token: 0x06005AA3 RID: 23203 RVA: 0x00031AD4 File Offset: 0x0002FCD4
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

	// Token: 0x04004814 RID: 18452
	private static Vector2Int CDG_8_DIRECTION_ANGLE_DOWN = new Vector2Int(254, 285);

	// Token: 0x04004815 RID: 18453
	private static Vector2Int CDG_8_DIRECTION_ANGLE_DOWN_LEFT = new Vector2Int(210, 253);

	// Token: 0x04004816 RID: 18454
	private static Vector2Int CDG_8_DIRECTION_ANGLE_LEFT = new Vector2Int(150, 209);

	// Token: 0x04004817 RID: 18455
	private static Vector2Int CDG_8_DIRECTION_ANGLE_UP_LEFT = new Vector2Int(106, 149);

	// Token: 0x04004818 RID: 18456
	private static Vector2Int CDG_8_DIRECTION_ANGLE_UP = new Vector2Int(74, 105);

	// Token: 0x04004819 RID: 18457
	private static Vector2Int CDG_8_DIRECTION_ANGLE_UP_RIGHT = new Vector2Int(30, 73);

	// Token: 0x0400481A RID: 18458
	private static Vector2Int CDG_8_DIRECTION_ANGLE_RIGHT = new Vector2Int(330, 29);

	// Token: 0x0400481B RID: 18459
	private static Vector2Int CDG_8_DIRECTION_ANGLE_DOWN_RIGHT = new Vector2Int(286, 329);

	// Token: 0x0400481C RID: 18460
	private static PlayerInputDirection[] m_typeArray;
}
