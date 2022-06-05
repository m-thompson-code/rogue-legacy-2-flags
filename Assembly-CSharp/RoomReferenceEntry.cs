using System;
using Rooms;
using UnityEngine;

// Token: 0x02000246 RID: 582
[Serializable]
public class RoomReferenceEntry
{
	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06001748 RID: 5960 RVA: 0x00048882 File Offset: 0x00046A82
	// (set) Token: 0x06001749 RID: 5961 RVA: 0x0004888A File Offset: 0x00046A8A
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

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x0600174A RID: 5962 RVA: 0x00048893 File Offset: 0x00046A93
	// (set) Token: 0x0600174B RID: 5963 RVA: 0x0004889B File Offset: 0x00046A9B
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

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x0600174C RID: 5964 RVA: 0x000488A4 File Offset: 0x00046AA4
	public string RoomMetaDataPath
	{
		get
		{
			return this.m_roomMetaDataPath;
		}
	}

	// Token: 0x040016C7 RID: 5831
	[SerializeField]
	private RoomReferenceType m_roomReferenceType;

	// Token: 0x040016C8 RID: 5832
	[SerializeField]
	private RoomMetaData m_roomMetaData;

	// Token: 0x040016C9 RID: 5833
	[SerializeField]
	private string m_roomMetaDataPath;
}
