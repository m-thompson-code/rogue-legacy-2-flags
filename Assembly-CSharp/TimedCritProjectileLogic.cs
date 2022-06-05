using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class TimedCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002BB1 RID: 11185 RVA: 0x00094B09 File Offset: 0x00092D09
	private void OnEnable()
	{
		this.m_critApplied = false;
		base.StartCoroutine(this.TimingWindowCoroutine());
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x00094B1F File Offset: 0x00092D1F
	private IEnumerator TimingWindowCoroutine()
	{
		float critWindow = Time.time + this.m_critTimingWindow.x;
		float endTime = Time.time + this.m_critTimingWindow.y;
		while (Time.time < endTime)
		{
			if (Time.time >= critWindow && base.SourceProjectile.ActualCritChance < 100f && !this.m_critApplied)
			{
				base.SourceProjectile.ActualCritChance += 100f;
				base.SourceProjectile.ChangeSpriteRendererColor(ProjectileLibrary.GetBuffColor(ProjectileBuffType.PlayerDashAttack));
				this.m_critApplied = true;
			}
			yield return null;
		}
		if (this.m_critApplied)
		{
			if (base.SourceProjectile.ActualCritChance >= 100f)
			{
				base.SourceProjectile.ActualCritChance -= 100f;
				base.SourceProjectile.ResetSpriteRendererColor();
			}
			this.m_critApplied = false;
		}
		yield break;
	}

	// Token: 0x04002380 RID: 9088
	[SerializeField]
	private Vector2 m_critTimingWindow;

	// Token: 0x04002381 RID: 9089
	private bool m_critApplied;
}
