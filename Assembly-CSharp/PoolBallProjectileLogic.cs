using System;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
[RequireComponent(typeof(TerrainRicochetProjectile_Logic))]
public class PoolBallProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B8F RID: 11151 RVA: 0x00093E8D File Offset: 0x0009208D
	protected override void Awake()
	{
		base.Awake();
		this.m_ricochetLogic = base.gameObject.GetComponent<TerrainRicochetProjectile_Logic>();
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x00093EA8 File Offset: 0x000920A8
	private void OnRicochet(object sender, EventArgs args)
	{
		BaseEffect baseEffect = EffectManager.PlayEffect(base.SourceProjectile.gameObject, base.SourceProjectile.Animator, "PoolBallHit_Effect", base.SourceProjectile.transform.localPosition, -2f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect.transform.SetParent(base.SourceProjectile.gameObject.transform, true);
		baseEffect.AddDetachListener(base.SourceProjectile);
		if (this.m_currentBounces == 0)
		{
			BaseEffect baseEffect2 = EffectManager.PlayEffect(base.SourceProjectile.gameObject, base.SourceProjectile.Animator, "PoolBallTrail_Effect", base.SourceProjectile.transform.localPosition, -2f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			baseEffect2.transform.SetParent(base.SourceProjectile.gameObject.transform, true);
			baseEffect2.AddDetachListener(base.SourceProjectile);
		}
		this.m_currentBounces++;
		if (this.m_currentBounces >= 2 && base.SourceProjectile.ActualCritChance < 100f)
		{
			base.SourceProjectile.ActualCritChance += 100f;
		}
		if (this.m_currentBounces >= 4)
		{
			base.SourceProjectile.FlagForDestruction(null);
			return;
		}
		base.SourceProjectile.Magic = this.m_startingMagic * (1f + Mathf.Min(7f * (float)this.m_currentBounces, 7f));
		base.SourceProjectile.ActualCritDamage = this.m_startingCritDamage * (1f + Mathf.Min(7f * (float)this.m_currentBounces, 7f));
		this.m_poolBallText.text = (this.m_currentBounces + 1).ToString();
		this.RicochetRelay.Dispatch(this.m_currentBounces);
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x0009405C File Offset: 0x0009225C
	public void OnEnable()
	{
		this.m_startingMagic = base.SourceProjectile.Magic;
		this.m_startingCritDamage = base.SourceProjectile.ActualCritDamage;
		this.m_currentBounces = 0;
		this.m_ricochetLogic.OnRicochet -= this.OnRicochet;
		this.m_ricochetLogic.OnRicochet += this.OnRicochet;
		this.m_poolBallText.text = (this.m_currentBounces + 1).ToString();
		base.SourceProjectile.transform.localScale = Vector3.one;
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x000940F0 File Offset: 0x000922F0
	private void OnDestroy()
	{
		this.m_ricochetLogic.OnRicochet -= this.OnRicochet;
	}

	// Token: 0x0400236A RID: 9066
	[SerializeField]
	private TMP_Text m_poolBallText;

	// Token: 0x0400236B RID: 9067
	public Relay<int> RicochetRelay = new Relay<int>();

	// Token: 0x0400236C RID: 9068
	private TerrainRicochetProjectile_Logic m_ricochetLogic;

	// Token: 0x0400236D RID: 9069
	private int m_currentBounces;

	// Token: 0x0400236E RID: 9070
	private float m_startingMagic;

	// Token: 0x0400236F RID: 9071
	private float m_startingCritDamage;
}
