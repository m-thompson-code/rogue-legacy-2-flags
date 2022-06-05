using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000971 RID: 2417
	[AddComponentMenu("Corgi Engine/Environment/Character Jump Override")]
	public class CharacterJumpOverride : MonoBehaviour
	{
		// Token: 0x0600520E RID: 21006 RVA: 0x00122D68 File Offset: 0x00120F68
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

		// Token: 0x0600520F RID: 21007 RVA: 0x00122E08 File Offset: 0x00121008
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

		// Token: 0x04004410 RID: 17424
		[Header("Jumps")]
		public float JumpHeight = 3.025f;

		// Token: 0x04004411 RID: 17425
		public float JumpMinimumAirTime = 0.1f;

		// Token: 0x04004412 RID: 17426
		public int NumberOfJumps = 3;

		// Token: 0x04004413 RID: 17427
		public CharacterJump.JumpBehavior JumpRestrictions;

		// Token: 0x04004414 RID: 17428
		public bool ResetNumberOfJumpsLeft = true;

		// Token: 0x04004415 RID: 17429
		protected float _previousJumpHeight;

		// Token: 0x04004416 RID: 17430
		protected float _previousJumpMinimumAirTime;

		// Token: 0x04004417 RID: 17431
		protected int _previousNumberOfJumps;

		// Token: 0x04004418 RID: 17432
		protected int _previousNumberOfJumpsLeft;

		// Token: 0x04004419 RID: 17433
		protected CharacterJump.JumpBehavior _previousJumpRestrictions;
	}
}
