using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000AFC RID: 2812
[CreateAssetMenu(menuName = "Custom/Level Editor/Room Content")]
public class RoomContentMetaData : ScriptableObject
{
	// Token: 0x17001CAA RID: 7338
	// (get) Token: 0x06005467 RID: 21607 RVA: 0x0002DB91 File Offset: 0x0002BD91
	// (set) Token: 0x06005468 RID: 21608 RVA: 0x0002DB99 File Offset: 0x0002BD99
	public RoomContentEntry[] ContentEntries
	{
		get
		{
			return this.m_contentEntries;
		}
		private set
		{
			this.m_contentEntries = value;
		}
	}

	// Token: 0x06005469 RID: 21609 RVA: 0x0013FDD0 File Offset: 0x0013DFD0
	public void AddEntry(RoomContentType contentType, Vector2 localPosition, SpawnConditionsEntry[] spawnConditions)
	{
		if (Application.isPlaying)
		{
			return;
		}
		RoomContentEntry item = new RoomContentEntry(contentType, localPosition, spawnConditions);
		if (this.ContentEntries == null)
		{
			this.ContentEntries = new RoomContentEntry[0];
		}
		List<RoomContentEntry> list = this.ContentEntries.ToList<RoomContentEntry>();
		list.Add(item);
		this.ContentEntries = list.ToArray();
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x0002DBA2 File Offset: 0x0002BDA2
	public void ClearEntries()
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.ContentEntries = new RoomContentEntry[0];
	}

	// Token: 0x04003EE8 RID: 16104
	[SerializeField]
	private RoomContentEntry[] m_contentEntries;
}
