using System;

// Token: 0x020004D0 RID: 1232
[Serializable]
public class RoomSaveData
{
	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x060027E8 RID: 10216 RVA: 0x000166D9 File Offset: 0x000148D9
	public bool IsEmpty
	{
		get
		{
			return this.BreakableStates == null || this.ChestStates == null || this.EnemyStates == null || this.DecoBreakableStates == null;
		}
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000BC1E0 File Offset: 0x000BA3E0
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

	// Token: 0x0400231A RID: 8986
	public string RoomName;

	// Token: 0x0400231B RID: 8987
	public BiomeType BiomeType;

	// Token: 0x0400231C RID: 8988
	public bool RoomVisited;

	// Token: 0x0400231D RID: 8989
	public bool IsRoomComplete;

	// Token: 0x0400231E RID: 8990
	public int RoomSeed;

	// Token: 0x0400231F RID: 8991
	public RoomID RoomID;

	// Token: 0x04002320 RID: 8992
	public int GridCoordinatesX;

	// Token: 0x04002321 RID: 8993
	public int GridCoordinatesY;

	// Token: 0x04002322 RID: 8994
	public bool IsMerged;

	// Token: 0x04002323 RID: 8995
	public int RoomNumber;

	// Token: 0x04002324 RID: 8996
	public int[] MergedWithRoomNumbers;

	// Token: 0x04002325 RID: 8997
	public int[] RoomCompleteBiomeControllerIndexOverrides;

	// Token: 0x04002326 RID: 8998
	public string RoomMiscData;

	// Token: 0x04002327 RID: 8999
	public RLBreakableSaveState[] BreakableStates;

	// Token: 0x04002328 RID: 9000
	public RLBreakableSaveState[] DecoBreakableStates;

	// Token: 0x04002329 RID: 9001
	public RLSaveState[] ChestStates;

	// Token: 0x0400232A RID: 9002
	public RLSaveState[] EnemyStates;

	// Token: 0x0400232B RID: 9003
	public RoomType RoomType;

	// Token: 0x0400232C RID: 9004
	public bool IsMirrored;

	// Token: 0x0400232D RID: 9005
	public bool IsTunnelDestination;

	// Token: 0x0400232E RID: 9006
	public int BiomeControllerIndex;
}
