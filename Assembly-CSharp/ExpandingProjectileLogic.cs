using System;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class ExpandingProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B49 RID: 11081 RVA: 0x00092CB4 File Offset: 0x00090EB4
	private void OnEnable()
	{
		Vector3 vector = (base.SourceProjectile.transform.parent != null) ? base.SourceProjectile.transform.parent.lossyScale : Vector3.one;
		float num = Mathf.Abs(this.m_shoutScale / vector.x);
		float num2 = Mathf.Abs(this.m_shoutScale / vector.y);
		if (!this.m_scaleRelative)
		{
			this.m_scaleTween = TweenManager.TweenTo(base.SourceProjectile.transform, base.SourceProjectile.Lifespan, new EaseDelegate(Ease.None), new object[]
			{
				"localScale.x",
				num,
				"localScale.y",
				num2
			});
			return;
		}
		this.m_scaleTween = TweenManager.TweenBy(base.SourceProjectile.transform, base.SourceProjectile.Lifespan, new EaseDelegate(Ease.None), new object[]
		{
			"localScale.x",
			num,
			"localScale.y",
			num2
		});
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x00092DCF File Offset: 0x00090FCF
	private void OnDisable()
	{
		if (this.m_scaleTween)
		{
			this.m_scaleTween.StopTweenWithConditionChecks(false, base.SourceProjectile, "ExpandProjectile");
		}
	}

	// Token: 0x0400233B RID: 9019
	[SerializeField]
	private float m_shoutScale = 10f;

	// Token: 0x0400233C RID: 9020
	[SerializeField]
	private bool m_scaleRelative;

	// Token: 0x0400233D RID: 9021
	private Tween m_scaleTween;
}
