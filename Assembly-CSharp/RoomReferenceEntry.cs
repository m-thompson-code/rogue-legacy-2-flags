using System;
using Rooms;
using UnityEngine;

// Token: 0x02000403 RID: 1027
[Serializable]
public class RoomReferenceEntry
{
	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x060020FB RID: 8443 RVA: 0x000118B5 File Offset: 0x0000FAB5
	// (set) Token: 0x060020FC RID: 8444 RVA: 0x000118BD File Offset: 0x0000FABD
	public RoomReferenceType RoomReferenceType
	{
		get
		{
			return this.m_roomReferenceType;
		}
		set
		{
			this.m_roomReferenceType = value;
		}
	}

	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x060020FD RID: 8445 RVA: 0x000118C6 File Offset: 0x0000FAC6
	// (set) Token: 0x060020FE RID: 8446 RVA: 0x000118CE File Offset: 0x0000FACE
	public RoomMetaData RoomMetaData
	{
		get
		{
			return this.m_roomMetaData;
		}
		set
		{
			this.m_roomMetaData = value;
		}
	}

	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x060020FF RID: 8447 RVA: 0x000118D7 File Offset: 0x0000FAD7
	public string RoomMetaDataPath
	{
		get
		{
			return this.m_roomMetaDataPath;
		}
	}

	// Token: 0x04001DDF RID: 7647
	[SerializeField]
	private RoomReferenceType m_roomReferenceType;

	// Token: 0x04001DE0 RID: 7648
	[SerializeField]
	private RoomMetaData m_roomMetaData;

	// Token: 0x04001DE1 RID: 7649
	[SerializeField]
	private string m_roomMetaDataPath;
}
