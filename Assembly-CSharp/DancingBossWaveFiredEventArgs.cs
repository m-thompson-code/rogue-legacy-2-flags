using System;
using UnityEngine;

// Token: 0x02000375 RID: 885
public class DancingBossWaveFiredEventArgs : EventArgs
{
	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x0000EDFC File Offset: 0x0000CFFC
	// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x0000EE04 File Offset: 0x0000D004
	public RoomSide Side { get; private set; }

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x0000EE0D File Offset: 0x0000D00D
	// (set) Token: 0x06001CE9 RID: 7401 RVA: 0x0000EE15 File Offset: 0x0000D015
	public Vector2 Origin { get; private set; }

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x06001CEA RID: 7402 RVA: 0x0000EE1E File Offset: 0x0000D01E
	// (set) Token: 0x06001CEB RID: 7403 RVA: 0x0000EE26 File Offset: 0x0000D026
	public float Speed { get; private set; }

	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0000EE2F File Offset: 0x0000D02F
	// (set) Token: 0x06001CED RID: 7405 RVA: 0x0000EE37 File Offset: 0x0000D037
	public float LifeSpan { get; private set; }

	// Token: 0x06001CEE RID: 7406 RVA: 0x0000EE40 File Offset: 0x0000D040
	public DancingBossWaveFiredEventArgs(RoomSide side, Vector2 origin, float speed, float lifeSpan)
	{
		this.Initialize(side, origin, speed, lifeSpan);
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x0000EE53 File Offset: 0x0000D053
	public void Initialize(RoomSide side, Vector2 origin, float speed, float lifeSpan)
	{
		this.Side = side;
		this.Origin = origin;
		this.Speed = speed;
		this.LifeSpan = lifeSpan;
	}
}
