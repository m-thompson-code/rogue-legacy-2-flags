using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public class Ferr2DHazardArgs : HazardArgs
{
	// Token: 0x06003641 RID: 13889 RVA: 0x000BC22E File Offset: 0x000BA42E
	public Ferr2DHazardArgs(StateID initialState, Vector3 localPosition, List<Vector2> points) : base(initialState)
	{
		this.LocalPosition = localPosition;
		this.Points = points;
	}

	// Token: 0x17001326 RID: 4902
	// (get) Token: 0x06003642 RID: 13890 RVA: 0x000BC245 File Offset: 0x000BA445
	public Vector3 LocalPosition { get; }

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x06003643 RID: 13891 RVA: 0x000BC24D File Offset: 0x000BA44D
	public List<Vector2> Points { get; }
}
