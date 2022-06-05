using System;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F24 RID: 3876
	public class CorgiControllerState
	{
		// Token: 0x17002461 RID: 9313
		// (get) Token: 0x06006FFD RID: 28669 RVA: 0x0003DC95 File Offset: 0x0003BE95
		// (set) Token: 0x06006FFE RID: 28670 RVA: 0x0003DC9D File Offset: 0x0003BE9D
		public bool IsCollidingRight { get; set; }

		// Token: 0x17002462 RID: 9314
		// (get) Token: 0x06006FFF RID: 28671 RVA: 0x0003DCA6 File Offset: 0x0003BEA6
		// (set) Token: 0x06007000 RID: 28672 RVA: 0x0003DCAE File Offset: 0x0003BEAE
		public bool IsCollidingLeft { get; set; }

		// Token: 0x17002463 RID: 9315
		// (get) Token: 0x06007001 RID: 28673 RVA: 0x0003DCB7 File Offset: 0x0003BEB7
		// (set) Token: 0x06007002 RID: 28674 RVA: 0x0003DCBF File Offset: 0x0003BEBF
		public bool IsCollidingAbove { get; set; }

		// Token: 0x17002464 RID: 9316
		// (get) Token: 0x06007003 RID: 28675 RVA: 0x0003DCC8 File Offset: 0x0003BEC8
		// (set) Token: 0x06007004 RID: 28676 RVA: 0x0003DCD0 File Offset: 0x0003BED0
		public bool IsCollidingBelow { get; set; }

		// Token: 0x17002465 RID: 9317
		// (get) Token: 0x06007005 RID: 28677 RVA: 0x0003DCD9 File Offset: 0x0003BED9
		public bool HasCollisions
		{
			get
			{
				return this.IsCollidingRight || this.IsCollidingLeft || this.IsCollidingAbove || this.IsCollidingBelow;
			}
		}

		// Token: 0x17002466 RID: 9318
		// (get) Token: 0x06007006 RID: 28678 RVA: 0x0003DCFB File Offset: 0x0003BEFB
		// (set) Token: 0x06007007 RID: 28679 RVA: 0x0003DD03 File Offset: 0x0003BF03
		public float LateralSlopeAngle { get; set; }

		// Token: 0x17002467 RID: 9319
		// (get) Token: 0x06007008 RID: 28680 RVA: 0x0003DD0C File Offset: 0x0003BF0C
		// (set) Token: 0x06007009 RID: 28681 RVA: 0x0003DD14 File Offset: 0x0003BF14
		public float BelowSlopeAngle { get; set; }

		// Token: 0x17002468 RID: 9320
		// (get) Token: 0x0600700A RID: 28682 RVA: 0x0003DD1D File Offset: 0x0003BF1D
		// (set) Token: 0x0600700B RID: 28683 RVA: 0x0003DD25 File Offset: 0x0003BF25
		public bool SlopeAngleOK { get; set; }

		// Token: 0x17002469 RID: 9321
		// (get) Token: 0x0600700C RID: 28684 RVA: 0x0003DD2E File Offset: 0x0003BF2E
		// (set) Token: 0x0600700D RID: 28685 RVA: 0x0003DD36 File Offset: 0x0003BF36
		public bool OnAMovingPlatform { get; set; }

		// Token: 0x1700246A RID: 9322
		// (get) Token: 0x0600700E RID: 28686 RVA: 0x0003DD3F File Offset: 0x0003BF3F
		public bool IsGrounded
		{
			get
			{
				return this.IsCollidingBelow && !this.JustStartedJump;
			}
		}

		// Token: 0x1700246B RID: 9323
		// (get) Token: 0x0600700F RID: 28687 RVA: 0x0003DD54 File Offset: 0x0003BF54
		// (set) Token: 0x06007010 RID: 28688 RVA: 0x0003DD5C File Offset: 0x0003BF5C
		public bool IsFalling { get; set; }

		// Token: 0x1700246C RID: 9324
		// (get) Token: 0x06007011 RID: 28689 RVA: 0x0003DD65 File Offset: 0x0003BF65
		// (set) Token: 0x06007012 RID: 28690 RVA: 0x0003DD6D File Offset: 0x0003BF6D
		public bool WasGroundedLastFrame { get; set; }

		// Token: 0x1700246D RID: 9325
		// (get) Token: 0x06007013 RID: 28691 RVA: 0x0003DD76 File Offset: 0x0003BF76
		// (set) Token: 0x06007014 RID: 28692 RVA: 0x0003DD7E File Offset: 0x0003BF7E
		public bool WasTouchingTheCeilingLastFrame { get; set; }

		// Token: 0x1700246E RID: 9326
		// (get) Token: 0x06007015 RID: 28693 RVA: 0x0003DD87 File Offset: 0x0003BF87
		// (set) Token: 0x06007016 RID: 28694 RVA: 0x0003DD8F File Offset: 0x0003BF8F
		public bool JustGotGrounded { get; set; }

		// Token: 0x1700246F RID: 9327
		// (get) Token: 0x06007017 RID: 28695 RVA: 0x0003DD98 File Offset: 0x0003BF98
		// (set) Token: 0x06007018 RID: 28696 RVA: 0x0003DDA0 File Offset: 0x0003BFA0
		public bool ColliderResized { get; set; }

		// Token: 0x17002470 RID: 9328
		// (get) Token: 0x06007019 RID: 28697 RVA: 0x0003DDA9 File Offset: 0x0003BFA9
		// (set) Token: 0x0600701A RID: 28698 RVA: 0x0003DDB1 File Offset: 0x0003BFB1
		public bool JustStartedJump { get; set; }

		// Token: 0x0600701B RID: 28699 RVA: 0x001908F8 File Offset: 0x0018EAF8
		public virtual void Reset()
		{
			this.IsCollidingLeft = false;
			this.IsCollidingRight = false;
			this.IsCollidingAbove = false;
			this.DistanceToLeftCollider = -1f;
			this.DistanceToRightCollider = -1f;
			this.SlopeAngleOK = false;
			this.JustGotGrounded = false;
			this.IsFalling = true;
			this.LateralSlopeAngle = 0f;
			this.JustStartedJump = false;
		}

		// Token: 0x0600701C RID: 28700 RVA: 0x00190958 File Offset: 0x0018EB58
		public override string ToString()
		{
			return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}", new object[]
			{
				this.IsCollidingRight,
				this.IsCollidingLeft,
				this.IsCollidingAbove,
				this.IsCollidingBelow,
				this.LateralSlopeAngle,
				this.JustStartedJump
			});
		}

		// Token: 0x04005A48 RID: 23112
		public float DistanceToLeftCollider;

		// Token: 0x04005A49 RID: 23113
		public float DistanceToRightCollider;
	}
}
