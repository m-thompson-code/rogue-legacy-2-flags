using System;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class FramerateMonitor : UserReportingMonitor
{
	// Token: 0x060044F7 RID: 17655 RVA: 0x000F5350 File Offset: 0x000F3550
	public FramerateMonitor()
	{
		this.MaximumDurationInSeconds = 10f;
		this.MinimumFramerate = 15f;
	}

	// Token: 0x060044F8 RID: 17656 RVA: 0x000F5370 File Offset: 0x000F3570
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

	// Token: 0x04003ACD RID: 15053
	private float duration;

	// Token: 0x04003ACE RID: 15054
	public float MaximumDurationInSeconds;

	// Token: 0x04003ACF RID: 15055
	public float MinimumFramerate;
}
