using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200067D RID: 1661
[CreateAssetMenu(menuName = "Custom/Level Editor/Room Content")]
public class RoomContentMetaData : ScriptableObject
{
	// Token: 0x170014EC RID: 5356
	// (get) Token: 0x06003C0B RID: 15371 RVA: 0x000CFEA0 File Offset: 0x000CE0A0
	// (set) Token: 0x06003C0C RID: 15372 RVA: 0x000CFEA8 File Offset: 0x000CE0A8
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

	// Token: 0x06003C0D RID: 15373 RVA: 0x000CFEB4 File Offset: 0x000CE0B4
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

	// Token: 0x06003C0E RID: 15374 RVA: 0x000CFF05 File Offset: 0x000CE105
	public void ClearEntries()
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.ContentEntries = new RoomContentEntry[0];
	}

	// Token: 0x04002D41 RID: 11585
	[SerializeField]
	private RoomContentEntry[] m_contentEntries;
}
