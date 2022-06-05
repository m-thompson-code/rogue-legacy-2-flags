using System;
using UnityEngine;

// Token: 0x02000A26 RID: 2598
[Serializable]
public struct DoorLocation : IEquatable<DoorLocation>
{
	// Token: 0x06004E8E RID: 20110 RVA: 0x0002AC4D File Offset: 0x00028E4D
	public DoorLocation(RoomSide roomSide, int doorNumber)
	{
		this.m_roomSide = roomSide;
		this.m_doorNumber = doorNumber;
	}

	// Token: 0x17001B28 RID: 6952
	// (get) Token: 0x06004E8F RID: 20111 RVA: 0x0002AC5D File Offset: 0x00028E5D
	public static DoorLocation Empty
	{
		get
		{
			return new DoorLocation(RoomSide.None, -1);
		}
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x0002AC66 File Offset: 0x00028E66
	public static bool operator ==(DoorLocation doorLocation01, DoorLocation doorLocation02)
	{
		return doorLocation01.Equals(doorLocation02);
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x0002AC70 File Offset: 0x00028E70
	public static bool operator !=(DoorLocation doorLocation01, DoorLocation doorLocation02)
	{
		return !(doorLocation01 == doorLocation02);
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x0002AC7C File Offset: 0x00028E7C
	public override string ToString()
	{
		return string.Format("({0}, {1})", this.RoomSide, this.DoorNumber);
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x0012DDC0 File Offset: 0x0012BFC0
	public override bool Equals(object obj)
	{
		if (!(obj is DoorLocation))
		{
			return false;
		}
		DoorLocation doorLocation = (DoorLocation)obj;
		return this.RoomSide == doorLocation.RoomSide && this.DoorNumber == doorLocation.DoorNumber;
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x0002AC9E File Offset: 0x00028E9E
	public bool Equals(DoorLocation other)
	{
		return this.RoomSide == other.RoomSide && this.DoorNumber == other.DoorNumber;
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x0012DE00 File Offset: 0x0012C000
	public override int GetHashCode()
	{
		return (-785574603 * -1521134295 + this.RoomSide.GetHashCode()) * -1521134295 + this.DoorNumber.GetHashCode();
	}

	// Token: 0x17001B29 RID: 6953
	// (get) Token: 0x06004E96 RID: 20118 RVA: 0x0002ACC0 File Offset: 0x00028EC0
	// (set) Token: 0x06004E97 RID: 20119 RVA: 0x0002ACC8 File Offset: 0x00028EC8
	public RoomSide RoomSide
	{
		get
		{
			return this.m_roomSide;
		}
		private set
		{
			this.m_roomSide = value;
		}
	}

	// Token: 0x17001B2A RID: 6954
	// (get) Token: 0x06004E98 RID: 20120 RVA: 0x0002ACD1 File Offset: 0x00028ED1
	// (set) Token: 0x06004E99 RID: 20121 RVA: 0x0002ACD9 File Offset: 0x00028ED9
	public int DoorNumber
	{
		get
		{
			return this.m_doorNumber;
		}
		private set
		{
			this.m_doorNumber = value;
		}
	}

	// Token: 0x04003B32 RID: 15154
	[SerializeField]
	private RoomSide m_roomSide;

	// Token: 0x04003B33 RID: 15155
	[SerializeField]
	private int m_doorNumber;
}
