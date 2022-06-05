using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007BE RID: 1982
public class TimedCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C36 RID: 15414 RVA: 0x000213E1 File Offset: 0x0001F5E1
	private void OnEnable()
	{
		this.m_critApplied = false;
		base.StartCoroutine(this.TimingWindowCoroutine());
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x000213F7 File Offset: 0x0001F5F7
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

	// Token: 0x04002FC4 RID: 12228
	[SerializeField]
	private Vector2 m_critTimingWindow;

	// Token: 0x04002FC5 RID: 12229
	private bool m_critApplied;
}
