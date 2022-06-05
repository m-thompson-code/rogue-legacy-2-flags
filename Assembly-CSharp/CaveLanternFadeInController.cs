using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class CaveLanternFadeInController : MonoBehaviour
{
	// Token: 0x06001284 RID: 4740 RVA: 0x00036761 File Offset: 0x00034961
	private void Awake()
	{
		this.m_collider = base.GetComponent<BoxCollider2D>();
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x00036770 File Offset: 0x00034970
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!CaveLanternPostProcessingController.Instance)
		{
			return;
		}
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (!this.m_effectEnabled)
		{
			CaveLanternPostProcessingController.EnableCaveLanternEffect();
			this.m_effectEnabled = true;
		}
		float y = this.m_collider.bounds.max.y;
		float y2 = this.m_collider.size.y;
		CaveLanternPostProcessingController.SetDimnessPercent(Mathf.Abs(PlayerManager.GetPlayerController().Midpoint.y - y) / y2);
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x000367F3 File Offset: 0x000349F3
	private void OnDisable()
	{
		if (this.m_effectEnabled && CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.DisableCaveLanternEffect();
		}
		this.m_effectEnabled = false;
	}

	// Token: 0x040012F4 RID: 4852
	private BoxCollider2D m_collider;

	// Token: 0x040012F5 RID: 4853
	private bool m_effectEnabled;
}
