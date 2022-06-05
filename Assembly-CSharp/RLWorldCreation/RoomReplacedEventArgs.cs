using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x0200088C RID: 2188
	public class RoomReplacedEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047D2 RID: 18386 RVA: 0x00102208 File Offset: 0x00100408
		public RoomReplacedEventArgs(RoomMetaData originalMetaData, bool originalIsMirrored, Vector2Int originalSize, List<DoorLocation> originalDoorLocations, RoomMetaData replacementMetaData, bool replacementIsMirrored, Vector2Int replacementSize, IEnumerable<DoorLocation> replacementDoorLocations)
		{
			this.OriginalMetaData = originalMetaData;
			this.OriginalIsMirrored = originalIsMirrored;
			this.OriginalSize = originalSize;
			this.OriginalDoorLocations = originalDoorLocations;
			this.ReplacementMetaData = replacementMetaData;
			this.ReplacementIsMirrored = replacementIsMirrored;
			this.ReplacementSize = replacementSize;
			this.ReplacementDoorLocations = replacementDoorLocations;
		}

		// Token: 0x04003CB9 RID: 15545
		private RoomMetaData OriginalMetaData;

		// Token: 0x04003CBA RID: 15546
		private bool OriginalIsMirrored;

		// Token: 0x04003CBB RID: 15547
		private Vector2Int OriginalSize;

		// Token: 0x04003CBC RID: 15548
		private List<DoorLocation> OriginalDoorLocations;

		// Token: 0x04003CBD RID: 15549
		private RoomMetaData ReplacementMetaData;

		// Token: 0x04003CBE RID: 15550
		private bool ReplacementIsMirrored;

		// Token: 0x04003CBF RID: 15551
		private Vector2Int ReplacementSize;

		// Token: 0x04003CC0 RID: 15552
		private IEnumerable<DoorLocation> ReplacementDoorLocations;
	}
}
