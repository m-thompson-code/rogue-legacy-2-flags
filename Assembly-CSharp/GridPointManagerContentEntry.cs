using System;
using UnityEngine;

// Token: 0x02000AEE RID: 2798
public struct GridPointManagerContentEntry
{
	// Token: 0x060053DC RID: 21468 RVA: 0x0002D808 File Offset: 0x0002BA08
	public GridPointManagerContentEntry(RoomContentType contentType, Vector2 localPosition, Vector2 worldPosition, bool isSpawned)
	{
		this.ContentType = contentType;
		this.LocalPosition = localPosition;
		this.WorldPosition = worldPosition;
		this.IsSpawned = isSpawned;
	}

	// Token: 0x17001C88 RID: 7304
	// (get) Token: 0x060053DD RID: 21469 RVA: 0x0002D827 File Offset: 0x0002BA27
	public readonly RoomContentType ContentType { get; }

	// Token: 0x17001C89 RID: 7305
	// (get) Token: 0x060053DE RID: 21470 RVA: 0x0002D82F File Offset: 0x0002BA2F
	public readonly Vector2 LocalPosition { get; }

	// Token: 0x17001C8A RID: 7306
	// (get) Token: 0x060053DF RID: 21471 RVA: 0x0002D837 File Offset: 0x0002BA37
	public readonly Vector2 WorldPosition { get; }

	// Token: 0x17001C8B RID: 7307
	// (get) Token: 0x060053E0 RID: 21472 RVA: 0x0002D83F File Offset: 0x0002BA3F
	public readonly bool IsSpawned { get; }
}
