using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009BC RID: 2492
public class Ferr2DHazardArgs : HazardArgs
{
	// Token: 0x06004C53 RID: 19539 RVA: 0x00029B0B File Offset: 0x00027D0B
	public Ferr2DHazardArgs(StateID initialState, Vector3 localPosition, List<Vector2> points) : base(initialState)
	{
		this.LocalPosition = localPosition;
		this.Points = points;
	}

	// Token: 0x17001A53 RID: 6739
	// (get) Token: 0x06004C54 RID: 19540 RVA: 0x00029B22 File Offset: 0x00027D22
	public Vector3 LocalPosition { get; }

	// Token: 0x17001A54 RID: 6740
	// (get) Token: 0x06004C55 RID: 19541 RVA: 0x00029B2A File Offset: 0x00027D2A
	public List<Vector2> Points { get; }
}
