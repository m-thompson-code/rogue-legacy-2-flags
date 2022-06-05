using System;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class CaveKeyRoomController : BaseSpecialRoomController
{
	// Token: 0x06002F7F RID: 12159 RVA: 0x000A295B File Offset: 0x000A0B5B
	protected override void Awake()
	{
		base.Awake();
		this.m_onRelicChanged = new Action<MonoBehaviour, EventArgs>(this.OnRelicChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x000A2982 File Offset: 0x000A0B82
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RelicLevelChanged, this.m_onRelicChanged);
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x000A2997 File Offset: 0x000A0B97
	private void OnRelicChanged(object sender, EventArgs args)
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveTuningForkTriggered))
		{
			MapController.SetCaveWhitePipVisibility(true);
		}
	}

	// Token: 0x040025E2 RID: 9698
	private Action<MonoBehaviour, EventArgs> m_onRelicChanged;
}
