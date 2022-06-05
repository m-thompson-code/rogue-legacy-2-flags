using System;
using UnityEngine;

// Token: 0x02000C83 RID: 3203
public class SkyLightChangedEventArgs : EventArgs
{
	// Token: 0x06005C16 RID: 23574 RVA: 0x00032825 File Offset: 0x00030A25
	public SkyLightChangedEventArgs(Color newSkyLightColor, float lerp)
	{
		this.Initialize(newSkyLightColor, lerp);
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x00032835 File Offset: 0x00030A35
	public void Initialize(Color newSkyLightColor, float lerp)
	{
		this.NewSkyLightColor = newSkyLightColor;
		this.Lerp = lerp;
	}

	// Token: 0x17001E86 RID: 7814
	// (get) Token: 0x06005C18 RID: 23576 RVA: 0x00032845 File Offset: 0x00030A45
	// (set) Token: 0x06005C19 RID: 23577 RVA: 0x0003284D File Offset: 0x00030A4D
	public Color NewSkyLightColor { get; private set; }

	// Token: 0x17001E87 RID: 7815
	// (get) Token: 0x06005C1A RID: 23578 RVA: 0x00032856 File Offset: 0x00030A56
	// (set) Token: 0x06005C1B RID: 23579 RVA: 0x0003285E File Offset: 0x00030A5E
	public float Lerp { get; private set; }
}
