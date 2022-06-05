using System;
using UnityEngine;

// Token: 0x02000677 RID: 1655
public struct GridPointManagerContentEntry
{
	// Token: 0x06003B9F RID: 15263 RVA: 0x000CD56E File Offset: 0x000CB76E
	public GridPointManagerContentEntry(RoomContentType contentType, Vector2 localPosition, Vector2 worldPosition, bool isSpawned)
	{
		this.ContentType = contentType;
		this.LocalPosition = localPosition;
		this.WorldPosition = worldPosition;
		this.IsSpawned = isSpawned;
	}

	// Token: 0x170014CC RID: 5324
	// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x000CD58D File Offset: 0x000CB78D
	public readonly RoomContentType ContentType { get; }

	// Token: 0x170014CD RID: 5325
	// (get) Token: 0x06003BA1 RID: 15265 RVA: 0x000CD595 File Offset: 0x000CB795
	public readonly Vector2 LocalPosition { get; }

	// Token: 0x170014CE RID: 5326
	// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x000CD59D File Offset: 0x000CB79D
	public readonly Vector2 WorldPosition { get; }

	// Token: 0x170014CF RID: 5327
	// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x000CD5A5 File Offset: 0x000CB7A5
	public readonly bool IsSpawned { get; }
}
