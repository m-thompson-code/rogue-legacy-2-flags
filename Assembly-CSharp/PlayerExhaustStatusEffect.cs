using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000310 RID: 784
[Obsolete("Replaced with CurrentExhaust directly on the PlayerController.")]
public class PlayerExhaustStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06001EFB RID: 7931 RVA: 0x00063DA3 File Offset: 0x00061FA3
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Exhaust;
		}
	}

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06001EFC RID: 7932 RVA: 0x00063DAA File Offset: 0x00061FAA
	public override float StartingDurationOverride
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x00063DB1 File Offset: 0x00061FB1
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x00063DCB File Offset: 0x00061FCB
	public void SetTimesStacked(int timesStacked)
	{
		base.TimesStacked = Mathf.Clamp(timesStacked, 0, int.MaxValue);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Exhaust, base.Duration, int.MaxValue, base.TimesStacked);
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x00063E02 File Offset: 0x00062002
	public override void StartEffect(float duration, IDamageObj caster)
	{
		base.StartEffect(duration, caster);
		if (!this.m_hitListenerAdded)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
			this.m_hitListenerAdded = true;
		}
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x00063E27 File Offset: 0x00062027
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

	// Token: 0x06001F01 RID: 7937 RVA: 0x00063E36 File Offset: 0x00062036
	private void OnPlayerHit(object sender, EventArgs args)
	{
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x00063E38 File Offset: 0x00062038
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		this.m_hitListenerAdded = false;
	}

	// Token: 0x04001BEA RID: 7146
	private bool m_hitListenerAdded;

	// Token: 0x04001BEB RID: 7147
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;
}
