using System;
using UnityEngine;

// Token: 0x02000421 RID: 1057
public class EffectGroundCheck : MonoBehaviour
{
	// Token: 0x0600270B RID: 9995 RVA: 0x000823F2 File Offset: 0x000805F2
	private void Awake()
	{
		this.m_effect = base.GetComponent<BaseEffect>();
		this.m_effect.OnPlayRelay.AddListener(new Action<BaseEffect>(this.PerformGroundCheck), false);
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x00082420 File Offset: 0x00080620
	private void PerformGroundCheck(BaseEffect effect)
	{
		if (PlayerManager.IsInstantiated)
		{
			Vector2 b = new Vector2((this.m_raycastCheckPos.transform.position.x - base.transform.position.x) * 2f, 0f);
			RaycastHit2D hit;
			if (PlayerManager.GetPlayerController().IsFacingRight)
			{
				hit = Physics2D.Raycast(this.m_raycastCheckPos.transform.position, Vector2.down, 0.2f, PlayerManager.GetPlayerController().ControllerCorgi.SavedPlatformMask);
			}
			else
			{
				hit = Physics2D.Raycast(this.m_raycastCheckPos.transform.position - b, Vector2.down, 0.2f, PlayerManager.GetPlayerController().ControllerCorgi.SavedPlatformMask);
			}
			if (!hit)
			{
				base.Invoke("StopEffect", 0f);
			}
		}
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x0008250D File Offset: 0x0008070D
	private void StopEffect()
	{
		this.m_effect.Stop(EffectStopType.Immediate);
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x0008251B File Offset: 0x0008071B
	private void OnDestroy()
	{
		if (this.m_effect != null)
		{
			this.m_effect.OnPlayRelay.RemoveListener(new Action<BaseEffect>(this.PerformGroundCheck));
		}
	}

	// Token: 0x040020C6 RID: 8390
	[SerializeField]
	private GameObject m_raycastCheckPos;

	// Token: 0x040020C7 RID: 8391
	private BaseEffect m_effect;
}
