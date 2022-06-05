using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A69 RID: 2665
public class Door_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BC5 RID: 7109
	// (get) Token: 0x06005091 RID: 20625 RVA: 0x0002BFCA File Offset: 0x0002A1CA
	// (set) Token: 0x06005092 RID: 20626 RVA: 0x0002BFD2 File Offset: 0x0002A1D2
	public override bool IsTrue { get; protected set; }

	// Token: 0x17001BC6 RID: 7110
	// (get) Token: 0x06005093 RID: 20627 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.Door;
		}
	}

	// Token: 0x17001BC7 RID: 7111
	// (get) Token: 0x06005094 RID: 20628 RVA: 0x00132B1C File Offset: 0x00130D1C
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

	// Token: 0x06005095 RID: 20629 RVA: 0x0002BFDB File Offset: 0x0002A1DB
	public Door_SpawnScenario() : this(RoomSide.Top, -1, InclusiveOrExclusive.Inclusive, DoorState.Open)
	{
	}

	// Token: 0x06005096 RID: 20630 RVA: 0x00132BC4 File Offset: 0x00130DC4
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

	// Token: 0x06005097 RID: 20631 RVA: 0x00132C14 File Offset: 0x00130E14
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

	// Token: 0x06005098 RID: 20632 RVA: 0x00132D50 File Offset: 0x00130F50
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

	// Token: 0x04003CFD RID: 15613
	public InclusiveOrExclusive InclusiveOrExclusive;

	// Token: 0x04003CFE RID: 15614
	public int Number = -1;

	// Token: 0x04003CFF RID: 15615
	public RoomSide Side = RoomSide.None;

	// Token: 0x04003D00 RID: 15616
	public DoorState State;
}
