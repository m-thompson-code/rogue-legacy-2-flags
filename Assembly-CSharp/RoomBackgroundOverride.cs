using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class RoomBackgroundOverride : MonoBehaviour
{
	// Token: 0x1700143A RID: 5178
	// (get) Token: 0x0600394D RID: 14669 RVA: 0x000C3246 File Offset: 0x000C1446
	public Background BackgroundOverride
	{
		get
		{
			return this.m_backgroundOverride;
		}
	}

	// Token: 0x1700143B RID: 5179
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x000C324E File Offset: 0x000C144E
	public bool IsTiled
	{
		get
		{
			return this.m_isTiled;
		}
	}

	// Token: 0x04002C1F RID: 11295
	[SerializeField]
	private Background m_backgroundOverride;

	// Token: 0x04002C20 RID: 11296
	[SerializeField]
	private bool m_isTiled;
}
