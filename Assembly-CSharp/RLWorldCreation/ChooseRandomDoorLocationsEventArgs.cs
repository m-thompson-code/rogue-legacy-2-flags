using System;
using System.Collections.Generic;

namespace RLWorldCreation
{
	// Token: 0x0200088B RID: 2187
	public class ChooseRandomDoorLocationsEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060047CA RID: 18378 RVA: 0x001021AA File Offset: 0x001003AA
		public ChooseRandomDoorLocationsEventArgs(GridPointManager room, IEnumerable<DoorLocation> potentialLocations, IEnumerable<DoorLocation> chosenLocations)
		{
			this.Initialize(room, potentialLocations, chosenLocations);
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x001021BB File Offset: 0x001003BB
		public void Initialize(GridPointManager room, IEnumerable<DoorLocation> potentialLocations, IEnumerable<DoorLocation> chosenLocations)
		{
			this.Room = room;
			this.PotentialLocations = potentialLocations;
			this.ChosenLocations = chosenLocations;
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x001021D2 File Offset: 0x001003D2
		// (set) Token: 0x060047CD RID: 18381 RVA: 0x001021DA File Offset: 0x001003DA
		public GridPointManager Room { get; private set; }

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x060047CE RID: 18382 RVA: 0x001021E3 File Offset: 0x001003E3
		// (set) Token: 0x060047CF RID: 18383 RVA: 0x001021EB File Offset: 0x001003EB
		public IEnumerable<DoorLocation> PotentialLocations { get; private set; }

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x060047D0 RID: 18384 RVA: 0x001021F4 File Offset: 0x001003F4
		// (set) Token: 0x060047D1 RID: 18385 RVA: 0x001021FC File Offset: 0x001003FC
		public IEnumerable<DoorLocation> ChosenLocations { get; private set; }
	}
}
