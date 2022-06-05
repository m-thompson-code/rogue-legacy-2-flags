using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F26 RID: 3878
	[AddComponentMenu("Corgi Engine/Environment/Character Jump Override")]
	public class CharacterJumpOverride : MonoBehaviour
	{
		// Token: 0x06007021 RID: 28705 RVA: 0x00190A30 File Offset: 0x0018EC30
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			CharacterJump component = collider.GetComponent<CharacterJump>();
			if (component == null)
			{
				return;
			}
			this._previousJumpHeight = component.JumpHeight;
			this._previousJumpMinimumAirTime = component.JumpMinimumAirTime;
			this._previousNumberOfJumps = component.NumberOfJumps;
			this._previousJumpRestrictions = component.JumpRestrictions;
			this._previousNumberOfJumpsLeft = component.NumberOfJumpsLeft;
			component.JumpHeight = this.JumpHeight;
			component.JumpMinimumAirTime = this.JumpMinimumAirTime;
			component.NumberOfJumps = this.NumberOfJumps;
			component.JumpRestrictions = this.JumpRestrictions;
			if (this.ResetNumberOfJumpsLeft)
			{
				component.SetNumberOfJumpsLeft(this.NumberOfJumps);
			}
		}

		// Token: 0x06007022 RID: 28706 RVA: 0x00190AD0 File Offset: 0x0018ECD0
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			CharacterJump component = collider.GetComponent<CharacterJump>();
			if (component == null)
			{
				return;
			}
			component.JumpHeight = this._previousJumpHeight;
			component.JumpMinimumAirTime = this._previousJumpMinimumAirTime;
			component.NumberOfJumps = this._previousNumberOfJumps;
			component.JumpRestrictions = this._previousJumpRestrictions;
			if (this.ResetNumberOfJumpsLeft)
			{
				component.SetNumberOfJumpsLeft(component.NumberOfJumps);
				return;
			}
			component.SetNumberOfJumpsLeft(this._previousNumberOfJumpsLeft);
		}

		// Token: 0x04005A56 RID: 23126
		[Header("Jumps")]
		public float JumpHeight = 3.025f;

		// Token: 0x04005A57 RID: 23127
		public float JumpMinimumAirTime = 0.1f;

		// Token: 0x04005A58 RID: 23128
		public int NumberOfJumps = 3;

		// Token: 0x04005A59 RID: 23129
		public CharacterJump.JumpBehavior JumpRestrictions;

		// Token: 0x04005A5A RID: 23130
		public bool ResetNumberOfJumpsLeft = true;

		// Token: 0x04005A5B RID: 23131
		protected float _previousJumpHeight;

		// Token: 0x04005A5C RID: 23132
		protected float _previousJumpMinimumAirTime;

		// Token: 0x04005A5D RID: 23133
		protected int _previousNumberOfJumps;

		// Token: 0x04005A5E RID: 23134
		protected int _previousNumberOfJumpsLeft;

		// Token: 0x04005A5F RID: 23135
		protected CharacterJump.JumpBehavior _previousJumpRestrictions;
	}
}
