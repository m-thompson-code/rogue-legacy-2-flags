using System;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class DashFlipEFfectCheck : MonoBehaviour
{
	// Token: 0x060026A7 RID: 9895 RVA: 0x00080052 File Offset: 0x0007E252
	private void Awake()
	{
		this.m_effect = base.GetComponent<BaseEffect>();
	}

	// Token: 0x060026A8 RID: 9896 RVA: 0x00080060 File Offset: 0x0007E260
	private void FixedUpdate()
	{
		if (PlayerManager.IsInstantiated)
		{
			float x = PlayerManager.GetPlayerController().Pivot.transform.localScale.x;
			if (this.m_effect && ((x > 0f && this.m_effect.transform.localScale.x < 0f) || (x <= 0f && this.m_effect.transform.localScale.x > 0f)))
			{
				this.m_effect.transform.SetLocalScaleX(this.m_effect.transform.localScale.x * -1f);
			}
		}
	}

	// Token: 0x04002066 RID: 8294
	private BaseEffect m_effect;
}
