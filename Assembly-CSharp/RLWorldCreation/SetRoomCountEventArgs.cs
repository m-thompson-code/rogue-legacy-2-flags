using System;

namespace RLWorldCreation
{
	// Token: 0x02000888 RID: 2184
	public class SetRoomCountEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047BC RID: 18364 RVA: 0x001020E9 File Offset: 0x001002E9
		public SetRoomCountEventArgs(int standardCount, int fairyCount, int trapCount, int bonusCount, int mandatoryCount)
		{
			this.StandardCount = standardCount;
			this.FairyCount = fairyCount;
			this.TrapCount = trapCount;
			this.BonusCount = bonusCount;
			this.MandatoryCount = mandatoryCount;
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x060047BD RID: 18365 RVA: 0x00102116 File Offset: 0x00100316
		public int StandardCount { get; }

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x060047BE RID: 18366 RVA: 0x0010211E File Offset: 0x0010031E
		public int FairyCount { get; }

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x060047BF RID: 18367 RVA: 0x00102126 File Offset: 0x00100326
		public int TrapCount { get; }

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x060047C0 RID: 18368 RVA: 0x0010212E File Offset: 0x0010032E
		public int BonusCount { get; }

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x060047C1 RID: 18369 RVA: 0x00102136 File Offset: 0x00100336
		public int MandatoryCount { get; }
	}
}
