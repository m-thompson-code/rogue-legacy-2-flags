using System;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x0200096C RID: 2412
	public class CharacterStates
	{
		// Token: 0x02000F1B RID: 3867
		public enum CharacterConditions
		{
			// Token: 0x04005A7F RID: 23167
			Normal,
			// Token: 0x04005A80 RID: 23168
			ControlledMovement,
			// Token: 0x04005A81 RID: 23169
			Frozen,
			// Token: 0x04005A82 RID: 23170
			Paused,
			// Token: 0x04005A83 RID: 23171
			Stunned,
			// Token: 0x04005A84 RID: 23172
			DisableHorizontalMovement,
			// Token: 0x04005A85 RID: 23173
			Dead
		}

		// Token: 0x02000F1C RID: 3868
		public enum MovementStates
		{
			// Token: 0x04005A87 RID: 23175
			Null,
			// Token: 0x04005A88 RID: 23176
			Idle,
			// Token: 0x04005A89 RID: 23177
			Walking,
			// Token: 0x04005A8A RID: 23178
			Falling,
			// Token: 0x04005A8B RID: 23179
			Running,
			// Token: 0x04005A8C RID: 23180
			Dashing,
			// Token: 0x04005A8D RID: 23181
			DownStriking,
			// Token: 0x04005A8E RID: 23182
			Jumping,
			// Token: 0x04005A8F RID: 23183
			Pushing,
			// Token: 0x04005A90 RID: 23184
			DoubleJumping,
			// Token: 0x04005A91 RID: 23185
			WallJumping,
			// Token: 0x04005A92 RID: 23186
			Flying
		}
	}
}
