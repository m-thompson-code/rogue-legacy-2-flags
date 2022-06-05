using System;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class SkyLightChangedEventArgs : EventArgs
{
	// Token: 0x0600428D RID: 17037 RVA: 0x000EBE0F File Offset: 0x000EA00F
	public SkyLightChangedEventArgs(Color newSkyLightColor, float lerp)
	{
		this.Initialize(newSkyLightColor, lerp);
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x000EBE1F File Offset: 0x000EA01F
	public void Initialize(Color newSkyLightColor, float lerp)
	{
		this.NewSkyLightColor = newSkyLightColor;
		this.Lerp = lerp;
	}

	// Token: 0x17001688 RID: 5768
	// (get) Token: 0x0600428F RID: 17039 RVA: 0x000EBE2F File Offset: 0x000EA02F
	// (set) Token: 0x06004290 RID: 17040 RVA: 0x000EBE37 File Offset: 0x000EA037
	public Color NewSkyLightColor { get; private set; }

	// Token: 0x17001689 RID: 5769
	// (get) Token: 0x06004291 RID: 17041 RVA: 0x000EBE40 File Offset: 0x000EA040
	// (set) Token: 0x06004292 RID: 17042 RVA: 0x000EBE48 File Offset: 0x000EA048
	public float Lerp { get; private set; }
}
