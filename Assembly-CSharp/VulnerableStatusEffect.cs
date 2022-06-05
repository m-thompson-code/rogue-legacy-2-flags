using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class VulnerableStatusEffect : BaseStatusEffect
{
	// Token: 0x170011A7 RID: 4519
	// (get) Token: 0x06002BDD RID: 11229 RVA: 0x0001865E File Offset: 0x0001685E
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Vulnerable;
		}
	}

	// Token: 0x170011A8 RID: 4520
	// (get) Token: 0x06002BDE RID: 11230 RVA: 0x00004536 File Offset: 0x00002736
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x00018665 File Offset: 0x00016865
	protected override void Awake()
	{
		base.Awake();
		this.m_onCharacterHit = new Action<object, CharacterHitEventArgs>(this.OnCharacterHit);
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x0001867F File Offset: 0x0001687F
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Vulnerable, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x0001868E File Offset: 0x0001688E
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x00018697 File Offset: 0x00016897
	private void OnCharacterHit(object sender, CharacterHitEventArgs args)
	{
		this.StopEffect(false);
	}

	// Token: 0x0400251D RID: 9501
	private Action<object, CharacterHitEventArgs> m_onCharacterHit;
}
