using System;
using UnityEngine;

// Token: 0x02000CFD RID: 3325
public class FramerateMonitor : UserReportingMonitor
{
	// Token: 0x06005ECF RID: 24271 RVA: 0x00034439 File Offset: 0x00032639
	public FramerateMonitor()
	{
		this.MaximumDurationInSeconds = 10f;
		this.MinimumFramerate = 15f;
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x00162F78 File Offset: 0x00161178
	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (1f / deltaTime < this.MinimumFramerate)
		{
			this.duration += deltaTime;
		}
		else
		{
			this.duration = 0f;
		}
		if (this.duration > this.MaximumDurationInSeconds)
		{
			this.duration = 0f;
			base.Trigger();
		}
	}

	// Token: 0x04004DD9 RID: 19929
	private float duration;

	// Token: 0x04004DDA RID: 19930
	public float MaximumDurationInSeconds;

	// Token: 0x04004DDB RID: 19931
	public float MinimumFramerate;
}
