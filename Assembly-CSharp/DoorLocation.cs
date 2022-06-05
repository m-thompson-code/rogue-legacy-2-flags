using System;
using UnityEngine;

// Token: 0x02000605 RID: 1541
[Serializable]
public struct DoorLocation : IEquatable<DoorLocation>
{
	// Token: 0x060037F1 RID: 14321 RVA: 0x000BF573 File Offset: 0x000BD773
	public DoorLocation(RoomSide roomSide, int doorNumber)
	{
		this.m_roomSide = roomSide;
		this.m_doorNumber = doorNumber;
	}

	// Token: 0x170013D1 RID: 5073
	// (get) Token: 0x060037F2 RID: 14322 RVA: 0x000BF583 File Offset: 0x000BD783
	public static DoorLocation Empty
	{
		get
		{
			return new DoorLocation(RoomSide.None, -1);
		}
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x000BF58C File Offset: 0x000BD78C
	public static bool operator ==(DoorLocation doorLocation01, DoorLocation doorLocation02)
	{
		return doorLocation01.Equals(doorLocation02);
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x000BF596 File Offset: 0x000BD796
	public static bool operator !=(DoorLocation doorLocation01, DoorLocation doorLocation02)
	{
		return !(doorLocation01 == doorLocation02);
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x000BF5A2 File Offset: 0x000BD7A2
	public override string ToString()
	{
		return string.Format("({0}, {1})", this.RoomSide, this.DoorNumber);
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x000BF5C4 File Offset: 0x000BD7C4
	public override bool Equals(object obj)
	{
		if (!(obj is DoorLocation))
		{
			return false;
		}
		DoorLocation doorLocation = (DoorLocation)obj;
		return this.RoomSide == doorLocation.RoomSide && this.DoorNumber == doorLocation.DoorNumber;
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x000BF602 File Offset: 0x000BD802
	public bool Equals(DoorLocation other)
	{
		return this.RoomSide == other.RoomSide && this.DoorNumber == other.DoorNumber;
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x000BF624 File Offset: 0x000BD824
	public override int GetHashCode()
	{
		return (-785574603 * -1521134295 + this.RoomSide.GetHashCode()) * -1521134295 + this.DoorNumber.GetHashCode();
	}

	// Token: 0x170013D2 RID: 5074
	// (get) Token: 0x060037F9 RID: 14329 RVA: 0x000BF666 File Offset: 0x000BD866
	// (set) Token: 0x060037FA RID: 14330 RVA: 0x000BF66E File Offset: 0x000BD86E
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

	// Token: 0x170013D3 RID: 5075
	// (get) Token: 0x060037FB RID: 14331 RVA: 0x000BF677 File Offset: 0x000BD877
	// (set) Token: 0x060037FC RID: 14332 RVA: 0x000BF67F File Offset: 0x000BD87F
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

	// Token: 0x04002ACF RID: 10959
	[SerializeField]
	private RoomSide m_roomSide;

	// Token: 0x04002AD0 RID: 10960
	[SerializeField]
	private int m_doorNumber;
}
