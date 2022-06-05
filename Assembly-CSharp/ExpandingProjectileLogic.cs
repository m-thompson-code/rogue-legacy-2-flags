using System;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
public class ExpandingProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B9E RID: 15262 RVA: 0x000F463C File Offset: 0x000F283C
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

	// Token: 0x06003B9F RID: 15263 RVA: 0x00020C60 File Offset: 0x0001EE60
	private void OnDisable()
	{
		if (this.m_scaleTween)
		{
			this.m_scaleTween.StopTweenWithConditionChecks(false, base.SourceProjectile, "ExpandProjectile");
		}
	}

	// Token: 0x04002F59 RID: 12121
	[SerializeField]
	private float m_shoutScale = 10f;

	// Token: 0x04002F5A RID: 12122
	[SerializeField]
	private bool m_scaleRelative;

	// Token: 0x04002F5B RID: 12123
	private Tween m_scaleTween;
}
