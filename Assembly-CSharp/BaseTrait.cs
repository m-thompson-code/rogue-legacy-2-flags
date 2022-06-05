using System;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public abstract class BaseTrait : MonoBehaviour
{
	// Token: 0x170011C5 RID: 4549
	// (get) Token: 0x06002C2D RID: 11309 RVA: 0x000188A5 File Offset: 0x00016AA5
	// (set) Token: 0x06002C2E RID: 11310 RVA: 0x000188AD File Offset: 0x00016AAD
	public bool IsFirstTrait { get; set; }

	// Token: 0x170011C6 RID: 4550
	// (get) Token: 0x06002C2F RID: 11311
	public abstract TraitType TraitType { get; }

	// Token: 0x170011C7 RID: 4551
	// (get) Token: 0x06002C30 RID: 11312 RVA: 0x000188B6 File Offset: 0x00016AB6
	public SpriteRenderer TraitMask
	{
		get
		{
			return this.m_traitMask;
		}
	}

	// Token: 0x170011C8 RID: 4552
	// (get) Token: 0x06002C31 RID: 11313 RVA: 0x000188BE File Offset: 0x00016ABE
	public TraitType[] IncompatibleTraits
	{
		get
		{
			return this.m_incompatibleTraits;
		}
	}

	// Token: 0x170011C9 RID: 4553
	// (get) Token: 0x06002C32 RID: 11314 RVA: 0x000188C6 File Offset: 0x00016AC6
	public TraitData TraitData
	{
		get
		{
			return this.m_traitData;
		}
	}

	// Token: 0x170011CA RID: 4554
	// (get) Token: 0x06002C33 RID: 11315 RVA: 0x000188CE File Offset: 0x00016ACE
	public MobilePostProcessOverrideController PostProcessOverrideController
	{
		get
		{
			return this.m_postProcessOverrideController;
		}
	}

	// Token: 0x170011CB RID: 4555
	// (get) Token: 0x06002C34 RID: 11316 RVA: 0x000188D6 File Offset: 0x00016AD6
	// (set) Token: 0x06002C35 RID: 11317 RVA: 0x000188DE File Offset: 0x00016ADE
	public bool IsPaused { get; private set; }

	// Token: 0x06002C36 RID: 11318 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void Awake()
	{
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void DisableOnDeath()
	{
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void DisableOnCutscene()
	{
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x000188E7 File Offset: 0x00016AE7
	public virtual void AssignGreenMask()
	{
		if (this.m_traitMask)
		{
			this.m_traitMask.color = Color.green;
		}
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x00018906 File Offset: 0x00016B06
	public virtual void SetPaused(bool paused)
	{
		this.IsPaused = paused;
	}

	// Token: 0x04002551 RID: 9553
	[SerializeField]
	private TraitData m_traitData;

	// Token: 0x04002552 RID: 9554
	[SerializeField]
	private TraitType[] m_incompatibleTraits;

	// Token: 0x04002553 RID: 9555
	[SerializeField]
	protected MobilePostProcessOverrideController m_postProcessOverrideController;

	// Token: 0x04002554 RID: 9556
	[SerializeField]
	protected SpriteRenderer m_traitMask;
}
