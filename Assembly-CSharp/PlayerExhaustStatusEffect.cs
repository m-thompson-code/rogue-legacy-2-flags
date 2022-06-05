using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000546 RID: 1350
[Obsolete("Replaced with CurrentExhaust directly on the PlayerController.")]
public class PlayerExhaustStatusEffect : BaseStatusEffect
{
	// Token: 0x17001171 RID: 4465
	// (get) Token: 0x06002B42 RID: 11074 RVA: 0x000181D9 File Offset: 0x000163D9
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Exhaust;
		}
	}

	// Token: 0x17001172 RID: 4466
	// (get) Token: 0x06002B43 RID: 11075 RVA: 0x00005319 File Offset: 0x00003519
	public override float StartingDurationOverride
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000181E0 File Offset: 0x000163E0
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000181FA File Offset: 0x000163FA
	public void SetTimesStacked(int timesStacked)
	{
		base.TimesStacked = Mathf.Clamp(timesStacked, 0, int.MaxValue);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Exhaust, base.Duration, int.MaxValue, base.TimesStacked);
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x00018231 File Offset: 0x00016431
	public override void StartEffect(float duration, IDamageObj caster)
	{
		base.StartEffect(duration, caster);
		if (!this.m_hitListenerAdded)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
			this.m_hitListenerAdded = true;
		}
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x00018256 File Offset: 0x00016456
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.SetTimesStacked(base.TimesStacked);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnPlayerHit(object sender, EventArgs args)
	{
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x00018265 File Offset: 0x00016465
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		this.m_hitListenerAdded = false;
	}

	// Token: 0x040024D2 RID: 9426
	private bool m_hitListenerAdded;

	// Token: 0x040024D3 RID: 9427
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;
}
