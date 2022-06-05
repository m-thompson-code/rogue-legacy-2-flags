using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
public abstract class SpecialPlatform : MonoBehaviour, IPlayHitEffect
{
	// Token: 0x06003E9F RID: 16031
	public abstract void SetState(StateID state);

	// Token: 0x170016D3 RID: 5843
	// (get) Token: 0x06003EA0 RID: 16032 RVA: 0x00022A1E File Offset: 0x00020C1E
	// (set) Token: 0x06003EA1 RID: 16033 RVA: 0x00022A26 File Offset: 0x00020C26
	public float Width { get; set; }

	// Token: 0x170016D4 RID: 5844
	// (get) Token: 0x06003EA2 RID: 16034 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public virtual bool PlayDirectionalHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170016D5 RID: 5845
	// (get) Token: 0x06003EA3 RID: 16035 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public virtual bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170016D6 RID: 5846
	// (get) Token: 0x06003EA4 RID: 16036 RVA: 0x0000F49B File Offset: 0x0000D69B
	public virtual string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x00022A2F File Offset: 0x00020C2F
	protected virtual void Awake()
	{
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x00022A3D File Offset: 0x00020C3D
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

	// Token: 0x04003125 RID: 12581
	protected IHitboxController m_hbController;
}
