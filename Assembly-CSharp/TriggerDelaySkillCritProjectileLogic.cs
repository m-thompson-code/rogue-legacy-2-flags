using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007C0 RID: 1984
public class TriggerDelaySkillCritProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C3F RID: 15423 RVA: 0x0002141D File Offset: 0x0001F61D
	private void OnEnable()
	{
		if (this.m_spriteRenderer)
		{
			this.m_initialTint = this.m_spriteRenderer.color;
		}
		base.StartCoroutine(this.TriggerSkillCritCoroutine());
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x0002144A File Offset: 0x0001F64A
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

	// Token: 0x06003C41 RID: 15425 RVA: 0x00021459 File Offset: 0x0001F659
	private void ApplyCrit()
	{
		if (base.SourceProjectile.ActualCritChance < 100f)
		{
			base.SourceProjectile.ActualCritChance += 100f;
		}
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x00021484 File Offset: 0x0001F684
	private void UnapplyCrit()
	{
		if (base.SourceProjectile.ActualCritChance >= 100f)
		{
			base.SourceProjectile.ActualCritChance -= 100f;
		}
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x000214AF File Offset: 0x0001F6AF
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

	// Token: 0x06003C44 RID: 15428 RVA: 0x000214E8 File Offset: 0x0001F6E8
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

	// Token: 0x06003C45 RID: 15429 RVA: 0x00021521 File Offset: 0x0001F721
	private void OnDisable()
	{
		this.DisableCritEffects();
	}

	// Token: 0x04002FCB RID: 12235
	[SerializeField]
	private float m_skillCritTriggerDelay;

	// Token: 0x04002FCC RID: 12236
	[SerializeField]
	[Tooltip("Enabling this means that the projectile will crit BEFORE the time delay, and afterwards will only deal regular damage.")]
	private bool m_onlyCritBeforeDelay;

	// Token: 0x04002FCD RID: 12237
	[SerializeField]
	private GameObject m_critEffectGO;

	// Token: 0x04002FCE RID: 12238
	[SerializeField]
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04002FCF RID: 12239
	[SerializeField]
	private Color m_critTintColor;

	// Token: 0x04002FD0 RID: 12240
	private Color m_initialTint;
}
