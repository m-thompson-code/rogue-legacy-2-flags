using System;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F1A RID: 3866
	public class CharacterStates
	{
		// Token: 0x02000F1B RID: 3867
		public enum CharacterConditions
		{
			// Token: 0x040059BF RID: 22975
			Normal,
			// Token: 0x040059C0 RID: 22976
			ControlledMovement,
			// Token: 0x040059C1 RID: 22977
			Frozen,
			// Token: 0x040059C2 RID: 22978
			Paused,
			// Token: 0x040059C3 RID: 22979
			Stunned,
			// Token: 0x040059C4 RID: 22980
			DisableHorizontalMovement,
			// Token: 0x040059C5 RID: 22981
			Dead
		}

		// Token: 0x02000F1C RID: 3868
		public enum MovementStates
		{
			// Token: 0x040059C7 RID: 22983
			Null,
			// Token: 0x040059C8 RID: 22984
			Idle,
			// Token: 0x040059C9 RID: 22985
			Walking,
			// Token: 0x040059CA RID: 22986
			Falling,
			// Token: 0x040059CB RID: 22987
			Running,
			// Token: 0x040059CC RID: 22988
			Dashing,
			// Token: 0x040059CD RID: 22989
			DownStriking,
			// Token: 0x040059CE RID: 22990
			Jumping,
			// Token: 0x040059CF RID: 22991
			Pushing,
			// Token: 0x040059D0 RID: 22992
			DoubleJumping,
			// Token: 0x040059D1 RID: 22993
			WallJumping,
			// Token: 0x040059D2 RID: 22994
			Flying
		}
	}
}
