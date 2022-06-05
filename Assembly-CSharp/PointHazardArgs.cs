using System;

// Token: 0x020009BD RID: 2493
public class PointHazardArgs : HazardArgs
{
	// Token: 0x06004C56 RID: 19542 RVA: 0x00029B32 File Offset: 0x00027D32
	public PointHazardArgs(StateID initialState, float radius, float rotationSpeed, float expansionDuration) : base(initialState)
	{
		this.Radius = radius;
		this.RotationSpeed = rotationSpeed;
		this.ExpansionDuration = expansionDuration;
	}

	// Token: 0x17001A55 RID: 6741
	// (get) Token: 0x06004C57 RID: 19543 RVA: 0x00029B51 File Offset: 0x00027D51
	public float Radius { get; }

	// Token: 0x17001A56 RID: 6742
	// (get) Token: 0x06004C58 RID: 19544 RVA: 0x00029B59 File Offset: 0x00027D59
	public float RotationSpeed { get; }

	// Token: 0x17001A57 RID: 6743
	// (get) Token: 0x06004C59 RID: 19545 RVA: 0x00029B61 File Offset: 0x00027D61
	public float ExpansionDuration { get; }
}
