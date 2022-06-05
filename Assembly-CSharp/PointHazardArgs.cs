using System;

// Token: 0x020005B5 RID: 1461
public class PointHazardArgs : HazardArgs
{
	// Token: 0x06003644 RID: 13892 RVA: 0x000BC255 File Offset: 0x000BA455
	public PointHazardArgs(StateID initialState, float radius, float rotationSpeed, float expansionDuration) : base(initialState)
	{
		this.Radius = radius;
		this.RotationSpeed = rotationSpeed;
		this.ExpansionDuration = expansionDuration;
	}

	// Token: 0x17001328 RID: 4904
	// (get) Token: 0x06003645 RID: 13893 RVA: 0x000BC274 File Offset: 0x000BA474
	public float Radius { get; }

	// Token: 0x17001329 RID: 4905
	// (get) Token: 0x06003646 RID: 13894 RVA: 0x000BC27C File Offset: 0x000BA47C
	public float RotationSpeed { get; }

	// Token: 0x1700132A RID: 4906
	// (get) Token: 0x06003647 RID: 13895 RVA: 0x000BC284 File Offset: 0x000BA484
	public float ExpansionDuration { get; }
}
