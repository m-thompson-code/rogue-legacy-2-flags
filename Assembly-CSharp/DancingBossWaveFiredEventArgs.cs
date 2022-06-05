using System;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class DancingBossWaveFiredEventArgs : EventArgs
{
	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x060013DB RID: 5083 RVA: 0x0003C4AD File Offset: 0x0003A6AD
	// (set) Token: 0x060013DC RID: 5084 RVA: 0x0003C4B5 File Offset: 0x0003A6B5
	public RoomSide Side { get; private set; }

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x060013DD RID: 5085 RVA: 0x0003C4BE File Offset: 0x0003A6BE
	// (set) Token: 0x060013DE RID: 5086 RVA: 0x0003C4C6 File Offset: 0x0003A6C6
	public Vector2 Origin { get; private set; }

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x060013DF RID: 5087 RVA: 0x0003C4CF File Offset: 0x0003A6CF
	// (set) Token: 0x060013E0 RID: 5088 RVA: 0x0003C4D7 File Offset: 0x0003A6D7
	public float Speed { get; private set; }

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0003C4E0 File Offset: 0x0003A6E0
	// (set) Token: 0x060013E2 RID: 5090 RVA: 0x0003C4E8 File Offset: 0x0003A6E8
	public float LifeSpan { get; private set; }

	// Token: 0x060013E3 RID: 5091 RVA: 0x0003C4F1 File Offset: 0x0003A6F1
	public DancingBossWaveFiredEventArgs(RoomSide side, Vector2 origin, float speed, float lifeSpan)
	{
		this.Initialize(side, origin, speed, lifeSpan);
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x0003C504 File Offset: 0x0003A704
	public void Initialize(RoomSide side, Vector2 origin, float speed, float lifeSpan)
	{
		this.Side = side;
		this.Origin = origin;
		this.Speed = speed;
		this.LifeSpan = lifeSpan;
	}
}
