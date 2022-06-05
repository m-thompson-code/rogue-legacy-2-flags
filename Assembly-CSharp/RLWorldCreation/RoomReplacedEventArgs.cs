using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000DAC RID: 3500
	public class RoomReplacedEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062D0 RID: 25296 RVA: 0x001709C0 File Offset: 0x0016EBC0
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

		// Token: 0x040050BB RID: 20667
		private RoomMetaData OriginalMetaData;

		// Token: 0x040050BC RID: 20668
		private bool OriginalIsMirrored;

		// Token: 0x040050BD RID: 20669
		private Vector2Int OriginalSize;

		// Token: 0x040050BE RID: 20670
		private List<DoorLocation> OriginalDoorLocations;

		// Token: 0x040050BF RID: 20671
		private RoomMetaData ReplacementMetaData;

		// Token: 0x040050C0 RID: 20672
		private bool ReplacementIsMirrored;

		// Token: 0x040050C1 RID: 20673
		private Vector2Int ReplacementSize;

		// Token: 0x040050C2 RID: 20674
		private IEnumerable<DoorLocation> ReplacementDoorLocations;
	}
}
