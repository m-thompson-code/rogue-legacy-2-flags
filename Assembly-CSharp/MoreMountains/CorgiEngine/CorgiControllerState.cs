using System;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x0200096F RID: 2415
	public class CorgiControllerState
	{
		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x060051EA RID: 20970 RVA: 0x00122AEC File Offset: 0x00120CEC
		// (set) Token: 0x060051EB RID: 20971 RVA: 0x00122AF4 File Offset: 0x00120CF4
		public bool IsCollidingRight { get; set; }

		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x060051EC RID: 20972 RVA: 0x00122AFD File Offset: 0x00120CFD
		// (set) Token: 0x060051ED RID: 20973 RVA: 0x00122B05 File Offset: 0x00120D05
		public bool IsCollidingLeft { get; set; }

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x060051EE RID: 20974 RVA: 0x00122B0E File Offset: 0x00120D0E
		// (set) Token: 0x060051EF RID: 20975 RVA: 0x00122B16 File Offset: 0x00120D16
		public bool IsCollidingAbove { get; set; }

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x00122B1F File Offset: 0x00120D1F
		// (set) Token: 0x060051F1 RID: 20977 RVA: 0x00122B27 File Offset: 0x00120D27
		public bool IsCollidingBelow { get; set; }

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x060051F2 RID: 20978 RVA: 0x00122B30 File Offset: 0x00120D30
		public bool HasCollisions
		{
			get
			{
				return this.IsCollidingRight || this.IsCollidingLeft || this.IsCollidingAbove || this.IsCollidingBelow;
			}
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x060051F3 RID: 20979 RVA: 0x00122B52 File Offset: 0x00120D52
		// (set) Token: 0x060051F4 RID: 20980 RVA: 0x00122B5A File Offset: 0x00120D5A
		public float LateralSlopeAngle { get; set; }

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x060051F5 RID: 20981 RVA: 0x00122B63 File Offset: 0x00120D63
		// (set) Token: 0x060051F6 RID: 20982 RVA: 0x00122B6B File Offset: 0x00120D6B
		public float BelowSlopeAngle { get; set; }

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x00122B74 File Offset: 0x00120D74
		// (set) Token: 0x060051F8 RID: 20984 RVA: 0x00122B7C File Offset: 0x00120D7C
		public bool SlopeAngleOK { get; set; }

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x060051F9 RID: 20985 RVA: 0x00122B85 File Offset: 0x00120D85
		// (set) Token: 0x060051FA RID: 20986 RVA: 0x00122B8D File Offset: 0x00120D8D
		public bool OnAMovingPlatform { get; set; }

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x060051FB RID: 20987 RVA: 0x00122B96 File Offset: 0x00120D96
		public bool IsGrounded
		{
			get
			{
				return this.IsCollidingBelow && !this.JustStartedJump;
			}
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x060051FC RID: 20988 RVA: 0x00122BAB File Offset: 0x00120DAB
		// (set) Token: 0x060051FD RID: 20989 RVA: 0x00122BB3 File Offset: 0x00120DB3
		public bool IsFalling { get; set; }

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x060051FE RID: 20990 RVA: 0x00122BBC File Offset: 0x00120DBC
		// (set) Token: 0x060051FF RID: 20991 RVA: 0x00122BC4 File Offset: 0x00120DC4
		public bool WasGroundedLastFrame { get; set; }

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x00122BCD File Offset: 0x00120DCD
		// (set) Token: 0x06005201 RID: 20993 RVA: 0x00122BD5 File Offset: 0x00120DD5
		public bool WasTouchingTheCeilingLastFrame { get; set; }

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x00122BDE File Offset: 0x00120DDE
		// (set) Token: 0x06005203 RID: 20995 RVA: 0x00122BE6 File Offset: 0x00120DE6
		public bool JustGotGrounded { get; set; }

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x00122BEF File Offset: 0x00120DEF
		// (set) Token: 0x06005205 RID: 20997 RVA: 0x00122BF7 File Offset: 0x00120DF7
		public bool ColliderResized { get; set; }

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x06005206 RID: 20998 RVA: 0x00122C00 File Offset: 0x00120E00
		// (set) Token: 0x06005207 RID: 20999 RVA: 0x00122C08 File Offset: 0x00120E08
		public bool JustStartedJump { get; set; }

		// Token: 0x06005208 RID: 21000 RVA: 0x00122C14 File Offset: 0x00120E14
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

		// Token: 0x06005209 RID: 21001 RVA: 0x00122C74 File Offset: 0x00120E74
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

		// Token: 0x04004402 RID: 17410
		public float DistanceToLeftCollider;

		// Token: 0x04004403 RID: 17411
		public float DistanceToRightCollider;
	}
}
