using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class ForceCastAbilityTypeProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B4C RID: 11084 RVA: 0x00092E08 File Offset: 0x00091008
	private void OnEnable()
	{
		base.StartCoroutine(this.SetCastAbilityType());
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x00092E17 File Offset: 0x00091017
	private IEnumerator SetCastAbilityType()
	{
		yield return null;
		base.SourceProjectile.CastAbilityType = this.m_forcedCastAbilityType;
		yield break;
	}

	// Token: 0x0400233E RID: 9022
	[SerializeField]
	private CastAbilityType m_forcedCastAbilityType;
}
