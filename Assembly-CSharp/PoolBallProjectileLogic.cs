using System;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x020007B5 RID: 1973
[RequireComponent(typeof(TerrainRicochetProjectile_Logic))]
public class PoolBallProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C02 RID: 15362 RVA: 0x000211DE File Offset: 0x0001F3DE
	protected override void Awake()
	{
		base.Awake();
		this.m_ricochetLogic = base.gameObject.GetComponent<TerrainRicochetProjectile_Logic>();
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x000F55C8 File Offset: 0x000F37C8
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

	// Token: 0x06003C04 RID: 15364 RVA: 0x000F577C File Offset: 0x000F397C
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

	// Token: 0x06003C05 RID: 15365 RVA: 0x000211F7 File Offset: 0x0001F3F7
	private void OnDestroy()
	{
		this.m_ricochetLogic.OnRicochet -= this.OnRicochet;
	}

	// Token: 0x04002FA4 RID: 12196
	[SerializeField]
	private TMP_Text m_poolBallText;

	// Token: 0x04002FA5 RID: 12197
	public Relay<int> RicochetRelay = new Relay<int>();

	// Token: 0x04002FA6 RID: 12198
	private TerrainRicochetProjectile_Logic m_ricochetLogic;

	// Token: 0x04002FA7 RID: 12199
	private int m_currentBounces;

	// Token: 0x04002FA8 RID: 12200
	private float m_startingMagic;

	// Token: 0x04002FA9 RID: 12201
	private float m_startingCritDamage;
}
