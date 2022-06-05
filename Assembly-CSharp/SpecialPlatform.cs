using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public abstract class SpecialPlatform : MonoBehaviour, IPlayHitEffect
{
	// Token: 0x06002D99 RID: 11673
	public abstract void SetState(StateID state);

	// Token: 0x1700114E RID: 4430
	// (get) Token: 0x06002D9A RID: 11674 RVA: 0x0009A1E9 File Offset: 0x000983E9
	// (set) Token: 0x06002D9B RID: 11675 RVA: 0x0009A1F1 File Offset: 0x000983F1
	public float Width { get; set; }

	// Token: 0x1700114F RID: 4431
	// (get) Token: 0x06002D9C RID: 11676 RVA: 0x0009A1FA File Offset: 0x000983FA
	public virtual bool PlayDirectionalHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001150 RID: 4432
	// (get) Token: 0x06002D9D RID: 11677 RVA: 0x0009A1FD File Offset: 0x000983FD
	public virtual bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001151 RID: 4433
	// (get) Token: 0x06002D9E RID: 11678 RVA: 0x0009A200 File Offset: 0x00098400
	public virtual string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x0009A203 File Offset: 0x00098403
	protected virtual void Awake()
	{
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x0009A211 File Offset: 0x00098411
	protected virtual IEnumerator Start()
	{
		if (!this.m_hbController.IsNativeNull())
		{
			while (!this.m_hbController.IsInitialized)
			{
				yield return null;
			}
			Collider2D collider = this.m_hbController.GetCollider(HitboxType.Platform);
			if (collider)
			{
				collider.tag = "TriggerHazard";
			}
		}
		yield break;
	}

	// Token: 0x0400248B RID: 9355
	protected IHitboxController m_hbController;
}
