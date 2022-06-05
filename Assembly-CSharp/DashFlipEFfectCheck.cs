using System;
using UnityEngine;

// Token: 0x020006C1 RID: 1729
public class DashFlipEFfectCheck : MonoBehaviour
{
	// Token: 0x06003556 RID: 13654 RVA: 0x0001D423 File Offset: 0x0001B623
	private void Awake()
	{
		this.m_effect = base.GetComponent<BaseEffect>();
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x000E0788 File Offset: 0x000DE988
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

	// Token: 0x04002B46 RID: 11078
	private BaseEffect m_effect;
}
