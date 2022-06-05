using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200063C RID: 1596
public class Door_SpawnScenario : SpawnScenario
{
	// Token: 0x1700145E RID: 5214
	// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000C47A9 File Offset: 0x000C29A9
	// (set) Token: 0x060039B3 RID: 14771 RVA: 0x000C47B1 File Offset: 0x000C29B1
	public override bool IsTrue { get; protected set; }

	// Token: 0x1700145F RID: 5215
	// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000C47BA File Offset: 0x000C29BA
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.Door;
		}
	}

	// Token: 0x17001460 RID: 5216
	// (get) Token: 0x060039B5 RID: 14773 RVA: 0x000C47C0 File Offset: 0x000C29C0
	public override string GizmoDescription
	{
		get
		{
			string text = "";
			if (this.InclusiveOrExclusive == InclusiveOrExclusive.Inclusive)
			{
				text += "*";
			}
			else
			{
				text += "!";
			}
			string text2 = "A";
			if (this.Number != -1)
			{
				text2 = this.Number.ToString();
			}
			char c = this.Side.ToString().First<char>();
			char c2 = this.State.ToString().First<char>();
			return string.Format("{0}({1}{2}){3}", new object[]
			{
				text,
				c,
				text2,
				c2
			});
		}
	}

	// Token: 0x060039B6 RID: 14774 RVA: 0x000C4868 File Offset: 0x000C2A68
	public Door_SpawnScenario() : this(RoomSide.Top, -1, InclusiveOrExclusive.Inclusive, DoorState.Open)
	{
	}

	// Token: 0x060039B7 RID: 14775 RVA: 0x000C4874 File Offset: 0x000C2A74
	public Door_SpawnScenario(RoomSide side, int doorNumber, InclusiveOrExclusive inclusiveOrExclusive, DoorState doorState)
	{
		if (side != RoomSide.Left && side != RoomSide.Right && side != RoomSide.Top && side != RoomSide.Bottom)
		{
			side = RoomSide.Top;
		}
		this.Side = side;
		this.Number = doorNumber;
		this.InclusiveOrExclusive = inclusiveOrExclusive;
		this.State = doorState;
	}

	// Token: 0x060039B8 RID: 14776 RVA: 0x000C48C4 File Offset: 0x000C2AC4
	public override void RunIsTrueCheck(BaseRoom room)
	{
		if (room && room is Room)
		{
			Room room2 = room as Room;
			bool isTrue = true;
			bool flag = this.InclusiveOrExclusive == InclusiveOrExclusive.Exclusive;
			List<Door> doors = room2.Doors;
			List<Door> list = null;
			if (this.Number == -1)
			{
				list = room2.GetDoorsOnSide(this.Side);
			}
			else
			{
				Door door = room2.GetDoor(this.Side, this.Number);
				if (door != null)
				{
					list = new List<Door>
					{
						door
					};
				}
			}
			if (this.State == DoorState.Open)
			{
				if (list != null && list.Count > 0)
				{
					if (flag)
					{
						if (this.Side != RoomSide.Any)
						{
							using (List<Door>.Enumerator enumerator = doors.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Door item = enumerator.Current;
									if (!list.Contains(item))
									{
										isTrue = false;
										break;
									}
								}
								goto IL_101;
							}
						}
						if (list.Count > 1)
						{
							isTrue = false;
						}
					}
				}
				else
				{
					isTrue = false;
				}
			}
			else if (this.State == DoorState.Closed && list != null && list.Count > 0)
			{
				isTrue = false;
			}
			IL_101:
			this.IsTrue = isTrue;
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Room argument is null</color>", new object[]
		{
			this
		});
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x000C4A00 File Offset: 0x000C2C00
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		bool isTrue = true;
		bool flag = this.InclusiveOrExclusive == InclusiveOrExclusive.Exclusive;
		if (gridPointManager.IsRoomMirrored)
		{
			if (this.Number != -1)
			{
				this.Number = RoomUtility.GetMirrorDoorNumber(new DoorLocation(this.Side, this.Number), gridPointManager.Size);
			}
			if (this.Side != RoomSide.Any)
			{
				this.Side = RoomUtility.GetMirrorSide(this.Side);
			}
		}
		List<DoorLocation> list = new List<DoorLocation>();
		if (this.Number == -1)
		{
			using (List<DoorLocation>.Enumerator enumerator = gridPointManager.GetDoorLocationsOnSide(this.Side).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DoorLocation doorLocation = enumerator.Current;
					if (gridPointManager.GetConnectedRoom(doorLocation) != null)
					{
						list.Add(doorLocation);
					}
				}
				goto IL_D6;
			}
		}
		DoorLocation doorLocation2 = gridPointManager.GetDoorLocation(this.Side, this.Number);
		if (gridPointManager.GetConnectedRoom(doorLocation2) != null)
		{
			list.Add(doorLocation2);
		}
		IL_D6:
		if (this.State == DoorState.Open)
		{
			if (list.Count > 0)
			{
				if (flag)
				{
					if (this.Side == RoomSide.Any)
					{
						Debug.LogFormat("<color=red>| Door_SpawnScenario | An exclusive Door Spawn Scenario in Room ({0}) has not has its <b>Side</b> field set.</color>", new object[]
						{
							gridPointManager.RoomMetaData.ID
						});
					}
					List<DoorLocation> list2 = new List<DoorLocation>();
					foreach (DoorLocation doorLocation3 in gridPointManager.DoorLocations)
					{
						if (gridPointManager.GetConnectedRoom(doorLocation3) != null)
						{
							list2.Add(doorLocation3);
						}
					}
					if (list2.Count == 1)
					{
						if (this.Side != RoomSide.Any && (list2[0].RoomSide != this.Side || (this.Number != -1 && list2[0].DoorNumber != this.Number)))
						{
							isTrue = false;
						}
					}
					else
					{
						isTrue = false;
					}
				}
			}
			else
			{
				isTrue = false;
			}
		}
		else if (this.State == DoorState.Closed && list.Count > 0)
		{
			isTrue = false;
		}
		this.IsTrue = isTrue;
	}

	// Token: 0x04002C6B RID: 11371
	public InclusiveOrExclusive InclusiveOrExclusive;

	// Token: 0x04002C6C RID: 11372
	public int Number = -1;

	// Token: 0x04002C6D RID: 11373
	public RoomSide Side = RoomSide.None;

	// Token: 0x04002C6E RID: 11374
	public DoorState State;
}
