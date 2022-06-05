using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class RangeDamageBonusCurseIndicator : MonoBehaviour
{
	// Token: 0x06001BB3 RID: 7091 RVA: 0x00059484 File Offset: 0x00057684
	private void Awake()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerScaleChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x000594DC File Offset: 0x000576DC
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerScaleChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x00059534 File Offset: 0x00057734
	private void OnEnable()
	{
		if (this.m_playerController && this.m_playerController.IsInitialized)
		{
			this.ToggleRangeEffect(null, null);
		}
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x00059558 File Offset: 0x00057758
	private void ToggleRangeEffect(object sender, EventArgs args)
	{
		float num = 0.9444444f;
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.RangeDamageBonusCurse);
		if (this.m_rangeEffect && this.m_rangeEffect.isActiveAndEnabled)
		{
			this.m_rangeEffect.gameObject.SetActive(false);
			this.m_rangeEffect = null;
		}
		if (PlayerManager.IsInstantiated && PlayerManager.GetPlayerController().IsDead)
		{
			return;
		}
		if (relic != null && relic.Level > 0)
		{
			this.m_playerController.ControllerCorgi.SetRaysParameters();
			this.m_rangeEffect = EffectManager.PlayEffect(base.gameObject, null, "TelescopeRelicRangeIndicator_Effect", this.m_playerController.Midpoint, 0f, EffectStopType.Immediate, EffectTriggerDirection.None);
			num /= this.m_rangeEffect.transform.lossyScale.x;
			this.m_rangeEffect.transform.localScale = new Vector3(num, num, 1f);
			this.m_rangeEffect.transform.SetParent(base.transform, true);
		}
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00059657 File Offset: 0x00057857
	public void DisableRangeEffect()
	{
		if (this.m_rangeEffect && this.m_rangeEffect.isActiveAndEnabled)
		{
			this.m_rangeEffect.gameObject.SetActive(false);
			this.m_rangeEffect = null;
		}
	}

	// Token: 0x0400195D RID: 6493
	private const string RANGE_EFFECT_NAME = "TelescopeRelicRangeIndicator_Effect";

	// Token: 0x0400195E RID: 6494
	[SerializeField]
	private PlayerController m_playerController;

	// Token: 0x0400195F RID: 6495
	private BaseEffect m_rangeEffect;
}
