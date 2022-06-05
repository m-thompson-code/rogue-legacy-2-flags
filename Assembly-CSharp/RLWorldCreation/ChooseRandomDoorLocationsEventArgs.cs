using System;
using System.Collections.Generic;

namespace RLWorldCreation
{
	// Token: 0x02000DAB RID: 3499
	public class ChooseRandomDoorLocationsEventArgs : BiomeCreationEventArgs
	{
		// Token: 0x060062C8 RID: 25288 RVA: 0x00036724 File Offset: 0x00034924
		public ChooseRandomDoorLocationsEventArgs(GridPointManager room, IEnumerable<DoorLocation> potentialLocations, IEnumerable<DoorLocation> chosenLocations)
		{
			this.Initialize(room, potentialLocations, chosenLocations);
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x00036735 File Offset: 0x00034935
		public void Initialize(GridPointManager room, IEnumerable<DoorLocation> potentialLocations, IEnumerable<DoorLocation> chosenLocations)
		{
			this.Room = room;
			this.PotentialLocations = potentialLocations;
			this.ChosenLocations = chosenLocations;
		}

		// Token: 0x17001FF1 RID: 8177
		// (get) Token: 0x060062CA RID: 25290 RVA: 0x0003674C File Offset: 0x0003494C
		// (set) Token: 0x060062CB RID: 25291 RVA: 0x00036754 File Offset: 0x00034954
		public GridPointManager Room { get; private set; }

		// Token: 0x17001FF2 RID: 8178
		// (get) Token: 0x060062CC RID: 25292 RVA: 0x0003675D File Offset: 0x0003495D
		// (set) Token: 0x060062CD RID: 25293 RVA: 0x00036765 File Offset: 0x00034965
		public IEnumerable<DoorLocation> PotentialLocations { get; private set; }

		// Token: 0x17001FF3 RID: 8179
		// (get) Token: 0x060062CE RID: 25294 RVA: 0x0003676E File Offset: 0x0003496E
		// (set) Token: 0x060062CF RID: 25295 RVA: 0x00036776 File Offset: 0x00034976
		public IEnumerable<DoorLocation> ChosenLocations { get; private set; }
	}
}
