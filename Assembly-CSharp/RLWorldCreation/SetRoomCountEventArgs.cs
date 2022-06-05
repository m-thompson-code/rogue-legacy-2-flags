using System;

namespace RLWorldCreation
{
	// Token: 0x02000DA8 RID: 3496
	public class SetRoomCountEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062BA RID: 25274 RVA: 0x00036663 File Offset: 0x00034863
		public SetRoomCountEventArgs(int standardCount, int fairyCount, int trapCount, int bonusCount, int mandatoryCount)
		{
			this.StandardCount = standardCount;
			this.FairyCount = fairyCount;
			this.TrapCount = trapCount;
			this.BonusCount = bonusCount;
			this.MandatoryCount = mandatoryCount;
		}

		// Token: 0x17001FE6 RID: 8166
		// (get) Token: 0x060062BB RID: 25275 RVA: 0x00036690 File Offset: 0x00034890
		public int StandardCount { get; }

		// Token: 0x17001FE7 RID: 8167
		// (get) Token: 0x060062BC RID: 25276 RVA: 0x00036698 File Offset: 0x00034898
		public int FairyCount { get; }

		// Token: 0x17001FE8 RID: 8168
		// (get) Token: 0x060062BD RID: 25277 RVA: 0x000366A0 File Offset: 0x000348A0
		public int TrapCount { get; }

		// Token: 0x17001FE9 RID: 8169
		// (get) Token: 0x060062BE RID: 25278 RVA: 0x000366A8 File Offset: 0x000348A8
		public int BonusCount { get; }

		// Token: 0x17001FEA RID: 8170
		// (get) Token: 0x060062BF RID: 25279 RVA: 0x000366B0 File Offset: 0x000348B0
		public int MandatoryCount { get; }
	}
}
