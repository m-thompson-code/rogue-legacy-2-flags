using System;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class RangeDamageBonusCurseIndicator : MonoBehaviour
{
	// Token: 0x06002667 RID: 9831 RVA: 0x000B62D8 File Offset: 0x000B44D8
	private void Awake()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerScaleChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x000B6330 File Offset: 0x000B4530
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerScaleChanged, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, new Action<MonoBehaviour, EventArgs>(this.ToggleRangeEffect));
	}

	// Token: 0x06002669 RID: 9833 RVA: 0x00015644 File Offset: 0x00013844
	private void OnEnable()
	{
		if (this.m_playerController && this.m_playerController.IsInitialized)
		{
			this.ToggleRangeEffect(null, null);
		}
	}

	// Token: 0x0600266A RID: 9834 RVA: 0x000B6388 File Offset: 0x000B4588
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

	// Token: 0x0600266B RID: 9835 RVA: 0x00015668 File Offset: 0x00013868
	public void DisableRangeEffect()
	{
		if (this.m_rangeEffect && this.m_rangeEffect.isActiveAndEnabled)
		{
			this.m_rangeEffect.gameObject.SetActive(false);
			this.m_rangeEffect = null;
		}
	}

	// Token: 0x0400214A RID: 8522
	private const string RANGE_EFFECT_NAME = "TelescopeRelicRangeIndicator_Effect";

	// Token: 0x0400214B RID: 8523
	[SerializeField]
	private PlayerController m_playerController;

	// Token: 0x0400214C RID: 8524
	private BaseEffect m_rangeEffect;
}
