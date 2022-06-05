using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
public class ForceCastAbilityTypeProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003BA1 RID: 15265 RVA: 0x00020C99 File Offset: 0x0001EE99
	private void OnEnable()
	{
		base.StartCoroutine(this.SetCastAbilityType());
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x00020CA8 File Offset: 0x0001EEA8
	private IEnumerator SetCastAbilityType()
	{
		yield return null;
		base.SourceProjectile.CastAbilityType = this.m_forcedCastAbilityType;
		yield break;
	}

	// Token: 0x04002F5C RID: 12124
	[SerializeField]
	private CastAbilityType m_forcedCastAbilityType;
}
