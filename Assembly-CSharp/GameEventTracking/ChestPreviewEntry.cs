using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008B5 RID: 2229
	[Serializable]
	public class ChestPreviewEntry
	{
		// Token: 0x060048AE RID: 18606 RVA: 0x00104C70 File Offset: 0x00102E70
		public ChestPreviewEntry(ChestType chestType)
		{
			this.ChestType = chestType;
		}

		// Token: 0x04003D51 RID: 15697
		[ReadOnly]
		public ChestType ChestType;

		// Token: 0x04003D52 RID: 15698
		public Sprite PreviewImage;
	}
}
