using System;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class CaveLanternFadeInController : MonoBehaviour
{
	// Token: 0x06001B02 RID: 6914 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
	private void Awake()
	{
		this.m_collider = base.GetComponent<BoxCollider2D>();
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x00093F9C File Offset: 0x0009219C
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

	// Token: 0x06001B04 RID: 6916 RVA: 0x0000DFD6 File Offset: 0x0000C1D6
	private void OnDisable()
	{
		if (this.m_effectEnabled && CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.DisableCaveLanternEffect();
		}
		this.m_effectEnabled = false;
	}

	// Token: 0x04001927 RID: 6439
	private BoxCollider2D m_collider;

	// Token: 0x04001928 RID: 6440
	private bool m_effectEnabled;
}
