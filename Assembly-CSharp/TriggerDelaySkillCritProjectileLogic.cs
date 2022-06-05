using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class TriggerDelaySkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002BB4 RID: 11188 RVA: 0x00094B36 File Offset: 0x00092D36
	private void OnEnable()
	{
		if (this.m_spriteRenderer)
		{
			this.m_initialTint = this.m_spriteRenderer.color;
		}
		base.StartCoroutine(this.TriggerSkillCritCoroutine());
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x00094B63 File Offset: 0x00092D63
	private IEnumerator TriggerSkillCritCoroutine()
	{
		float duration = Time.time + this.m_skillCritTriggerDelay;
		if (this.m_onlyCritBeforeDelay)
		{
			this.ApplyCrit();
			this.EnableCritEffects();
		}
		while (Time.time < duration)
		{
			yield return null;
		}
		if (this.m_onlyCritBeforeDelay)
		{
			this.UnapplyCrit();
			this.DisableCritEffects();
		}
		else
		{
			this.ApplyCrit();
			this.EnableCritEffects();
		}
		yield break;
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x00094B72 File Offset: 0x00092D72
	private void ApplyCrit()
	{
		if (base.SourceProjectile.ActualCritChance < 100f)
		{
			base.SourceProjectile.ActualCritChance += 100f;
		}
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x00094B9D File Offset: 0x00092D9D
	private void UnapplyCrit()
	{
		if (base.SourceProjectile.ActualCritChance >= 100f)
		{
			base.SourceProjectile.ActualCritChance -= 100f;
		}
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x00094BC8 File Offset: 0x00092DC8
	private void EnableCritEffects()
	{
		if (this.m_spriteRenderer)
		{
			this.m_spriteRenderer.color = this.m_critTintColor;
		}
		if (this.m_critEffectGO)
		{
			this.m_critEffectGO.SetActive(true);
		}
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x00094C01 File Offset: 0x00092E01
	private void DisableCritEffects()
	{
		if (this.m_spriteRenderer)
		{
			this.m_spriteRenderer.color = this.m_initialTint;
		}
		if (this.m_critEffectGO)
		{
			this.m_critEffectGO.SetActive(false);
		}
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x00094C3A File Offset: 0x00092E3A
	private void OnDisable()
	{
		this.DisableCritEffects();
	}

	// Token: 0x04002382 RID: 9090
	[SerializeField]
	private float m_skillCritTriggerDelay;

	// Token: 0x04002383 RID: 9091
	[SerializeField]
	[Tooltip("Enabling this means that the projectile will crit BEFORE the time delay, and afterwards will only deal regular damage.")]
	private bool m_onlyCritBeforeDelay;

	// Token: 0x04002384 RID: 9092
	[SerializeField]
	private GameObject m_critEffectGO;

	// Token: 0x04002385 RID: 9093
	[SerializeField]
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04002386 RID: 9094
	[SerializeField]
	private Color m_critTintColor;

	// Token: 0x04002387 RID: 9095
	private Color m_initialTint;
}
