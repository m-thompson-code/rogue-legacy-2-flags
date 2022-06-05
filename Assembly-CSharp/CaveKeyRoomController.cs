using System;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public class CaveKeyRoomController : BaseSpecialRoomController
{
	// Token: 0x060041A6 RID: 16806 RVA: 0x000245A0 File Offset: 0x000227A0
	protected override void Awake()
	{
		base.Awake();
		this.m_onRelicChanged = new Action<MonoBehaviour, EventArgs>(this.OnRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x000245C7 File Offset: 0x000227C7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x000245DC File Offset: 0x000227DC
	private void OnRelicChanged(object sender, EventArgs args)
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveTuningForkTriggered))
		{
			MapController.SetCaveWhitePipVisibility(true);
		}
	}

	// Token: 0x04003369 RID: 13161
	private Action<MonoBehaviour, EventArgs> m_onRelicChanged;
}
