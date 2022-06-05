using System;
using System.Collections;

namespace RLWorldCreation
{
	// Token: 0x02000DB8 RID: 3512
	public class MergeRoomStrategy_Cave : MergeRoomStrategy
	{
		// Token: 0x06006312 RID: 25362 RVA: 0x00036963 File Offset: 0x00034B63
		public IEnumerator Run(BiomeController biomeController)
		{
			this.MergeTopOfCave(biomeController);
			yield break;
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x00036979 File Offset: 0x00034B79
		private void MergeTopOfCave(BiomeController biomeController)
		{
			MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.Cave);
		}
	}
}
