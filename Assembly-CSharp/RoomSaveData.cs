using System;

// Token: 0x020002DB RID: 731
[Serializable]
public class RoomSaveData
{
	// Token: 0x17000CCF RID: 3279
	// (get) Token: 0x06001D1E RID: 7454 RVA: 0x0005FF5F File Offset: 0x0005E15F
	public bool IsEmpty
	{
		get
		{
			return this.BreakableStates == null || this.ChestStates == null || this.EnemyStates == null || this.DecoBreakableStates == null;
		}
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x0005FF84 File Offset: 0x0005E184
	public void Clear()
	{
		this.RoomName = null;
		this.BiomeType = BiomeType.None;
		this.RoomVisited = false;
		this.IsRoomComplete = false;
		this.RoomSeed = 0;
		this.RoomID = default(RoomID);
		this.GridCoordinatesX = 0;
		this.GridCoordinatesY = 0;
		this.IsMerged = false;
		this.RoomNumber = 0;
		this.MergedWithRoomNumbers = null;
		this.RoomMiscData = null;
		this.RoomCompleteBiomeControllerIndexOverrides = null;
		this.BreakableStates = null;
		this.ChestStates = null;
		this.EnemyStates = null;
		this.DecoBreakableStates = null;
		this.RoomType = RoomType.None;
		this.IsMirrored = false;
		this.IsTunnelDestination = false;
		this.BiomeControllerIndex = 0;
	}

	// Token: 0x04001B13 RID: 6931
	public string RoomName;

	// Token: 0x04001B14 RID: 6932
	public BiomeType BiomeType;

	// Token: 0x04001B15 RID: 6933
	public bool RoomVisited;

	// Token: 0x04001B16 RID: 6934
	public bool IsRoomComplete;

	// Token: 0x04001B17 RID: 6935
	public int RoomSeed;

	// Token: 0x04001B18 RID: 6936
	public RoomID RoomID;

	// Token: 0x04001B19 RID: 6937
	public int GridCoordinatesX;

	// Token: 0x04001B1A RID: 6938
	public int GridCoordinatesY;

	// Token: 0x04001B1B RID: 6939
	public bool IsMerged;

	// Token: 0x04001B1C RID: 6940
	public int RoomNumber;

	// Token: 0x04001B1D RID: 6941
	public int[] MergedWithRoomNumbers;

	// Token: 0x04001B1E RID: 6942
	public int[] RoomCompleteBiomeControllerIndexOverrides;

	// Token: 0x04001B1F RID: 6943
	public string RoomMiscData;

	// Token: 0x04001B20 RID: 6944
	public RLBreakableSaveState[] BreakableStates;

	// Token: 0x04001B21 RID: 6945
	public RLBreakableSaveState[] DecoBreakableStates;

	// Token: 0x04001B22 RID: 6946
	public RLSaveState[] ChestStates;

	// Token: 0x04001B23 RID: 6947
	public RLSaveState[] EnemyStates;

	// Token: 0x04001B24 RID: 6948
	public RoomType RoomType;

	// Token: 0x04001B25 RID: 6949
	public bool IsMirrored;

	// Token: 0x04001B26 RID: 6950
	public bool IsTunnelDestination;

	// Token: 0x04001B27 RID: 6951
	public int BiomeControllerIndex;
}
