using System;
using UnityEngine;

// Token: 0x020006DA RID: 1754
public class EffectGroundCheck : MonoBehaviour
{
	// Token: 0x060035C6 RID: 13766 RVA: 0x0001D797 File Offset: 0x0001B997
	private void Awake()
	{
		this.m_effect = base.GetComponent<BaseEffect>();
		this.m_effect.OnPlayRelay.AddListener(new Action<BaseEffect>(this.PerformGroundCheck), false);
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x000E26C4 File Offset: 0x000E08C4
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

	// Token: 0x060035C8 RID: 13768 RVA: 0x0001D7C3 File Offset: 0x0001B9C3
	private void StopEffect()
	{
		this.m_effect.Stop(EffectStopType.Immediate);
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x0001D7D1 File Offset: 0x0001B9D1
	private void OnDestroy()
	{
		if (this.m_effect != null)
		{
			this.m_effect.OnPlayRelay.RemoveListener(new Action<BaseEffect>(this.PerformGroundCheck));
		}
	}

	// Token: 0x04002BAC RID: 11180
	[SerializeField]
	private GameObject m_raycastCheckPos;

	// Token: 0x04002BAD RID: 11181
	private BaseEffect m_effect;
}
