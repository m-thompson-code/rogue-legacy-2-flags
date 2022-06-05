using System;
using UnityEngine;

// Token: 0x02000320 RID: 800
public abstract class BaseTrait : MonoBehaviour
{
	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x06001F86 RID: 8070 RVA: 0x00064F0F File Offset: 0x0006310F
	// (set) Token: 0x06001F87 RID: 8071 RVA: 0x00064F17 File Offset: 0x00063117
	public bool IsFirstTrait { get; set; }

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x06001F88 RID: 8072
	public abstract TraitType TraitType { get; }

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x06001F89 RID: 8073 RVA: 0x00064F20 File Offset: 0x00063120
	public SpriteRenderer TraitMask
	{
		get
		{
			return this.m_traitMask;
		}
	}

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x06001F8A RID: 8074 RVA: 0x00064F28 File Offset: 0x00063128
	public TraitType[] IncompatibleTraits
	{
		get
		{
			return this.m_incompatibleTraits;
		}
	}

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x06001F8B RID: 8075 RVA: 0x00064F30 File Offset: 0x00063130
	public TraitData TraitData
	{
		get
		{
			return this.m_traitData;
		}
	}

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x06001F8C RID: 8076 RVA: 0x00064F38 File Offset: 0x00063138
	public MobilePostProcessOverrideController PostProcessOverrideController
	{
		get
		{
			return this.m_postProcessOverrideController;
		}
	}

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x06001F8D RID: 8077 RVA: 0x00064F40 File Offset: 0x00063140
	// (set) Token: 0x06001F8E RID: 8078 RVA: 0x00064F48 File Offset: 0x00063148
	public bool IsPaused { get; private set; }

	// Token: 0x06001F8F RID: 8079 RVA: 0x00064F51 File Offset: 0x00063151
	protected virtual void Awake()
	{
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x00064F53 File Offset: 0x00063153
	public virtual void DisableOnDeath()
	{
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x00064F55 File Offset: 0x00063155
	public virtual void DisableOnCutscene()
	{
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x00064F57 File Offset: 0x00063157
	public virtual void AssignGreenMask()
	{
		if (this.m_traitMask)
		{
			this.m_traitMask.color = Color.green;
		}
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x00064F76 File Offset: 0x00063176
	public virtual void SetPaused(bool paused)
	{
		this.IsPaused = paused;
	}

	// Token: 0x04001C2D RID: 7213
	[SerializeField]
	private TraitData m_traitData;

	// Token: 0x04001C2E RID: 7214
	[SerializeField]
	private TraitType[] m_incompatibleTraits;

	// Token: 0x04001C2F RID: 7215
	[SerializeField]
	protected MobilePostProcessOverrideController m_postProcessOverrideController;

	// Token: 0x04001C30 RID: 7216
	[SerializeField]
	protected SpriteRenderer m_traitMask;
}
