using System;
using UnityEngine;

// Token: 0x0200078F RID: 1935
public class Platform : MonoBehaviour, IPlayHitEffect
{
	// Token: 0x170015D3 RID: 5587
	// (get) Token: 0x06003B3A RID: 15162 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170015D4 RID: 5588
	// (get) Token: 0x06003B3B RID: 15163 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170015D5 RID: 5589
	// (get) Token: 0x06003B3C RID: 15164 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}
}
