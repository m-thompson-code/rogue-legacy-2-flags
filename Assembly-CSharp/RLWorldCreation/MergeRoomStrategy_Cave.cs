using System;
using System.Collections;

namespace RLWorldCreation
{
	// Token: 0x02000890 RID: 2192
	public class MergeRoomStrategy_Cave : MergeRoomStrategy
	{
		// Token: 0x060047EF RID: 18415 RVA: 0x00102B21 File Offset: 0x00100D21
		public IEnumerator Run(BiomeController biomeController)
		{
			this.MergeTopOfCave(biomeController);
			yield break;
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x00102B37 File Offset: 0x00100D37
		private void MergeTopOfCave(BiomeController biomeController)
		{
			MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.Cave);
		}
	}
}
