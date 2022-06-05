using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class Platform : MonoBehaviour, IPlayHitEffect
{
	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x06002AFD RID: 11005 RVA: 0x00091C97 File Offset: 0x0008FE97
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x06002AFE RID: 11006 RVA: 0x00091C9A File Offset: 0x0008FE9A
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x06002AFF RID: 11007 RVA: 0x00091C9D File Offset: 0x0008FE9D
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}
}
