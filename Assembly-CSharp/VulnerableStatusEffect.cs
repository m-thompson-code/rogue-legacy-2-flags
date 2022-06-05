using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class VulnerableStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x06001F54 RID: 8020 RVA: 0x00064853 File Offset: 0x00062A53
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Vulnerable;
		}
	}

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x06001F55 RID: 8021 RVA: 0x0006485A File Offset: 0x00062A5A
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x00064861 File Offset: 0x00062A61
	protected override void Awake()
	{
		base.Awake();
		this.m_onCharacterHit = new Action<object, CharacterHitEventArgs>(this.OnCharacterHit);
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x0006487B File Offset: 0x00062A7B
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

	// Token: 0x06001F58 RID: 8024 RVA: 0x0006488A File Offset: 0x00062A8A
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x00064893 File Offset: 0x00062A93
	private void OnCharacterHit(object sender, CharacterHitEventArgs args)
	{
		this.StopEffect(false);
	}

	// Token: 0x04001C0D RID: 7181
	private Action<object, CharacterHitEventArgs> m_onCharacterHit;
}
