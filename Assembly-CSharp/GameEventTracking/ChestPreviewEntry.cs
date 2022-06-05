using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DEC RID: 3564
	[Serializable]
	public class ChestPreviewEntry
	{
		// Token: 0x0600642E RID: 25646 RVA: 0x000375B4 File Offset: 0x000357B4
		public ChestPreviewEntry(ChestType chestType)
		{
			this.ChestType = chestType;
		}

		// Token: 0x040051AB RID: 20907
		[ReadOnly]
		public ChestType ChestType;

		// Token: 0x040051AC RID: 20908
		public Sprite PreviewImage;
	}
}
