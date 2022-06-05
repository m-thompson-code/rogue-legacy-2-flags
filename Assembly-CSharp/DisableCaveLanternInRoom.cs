using System;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class DisableCaveLanternInRoom : MonoBehaviour
{
	// Token: 0x06001D83 RID: 7555 RVA: 0x0000F3B1 File Offset: 0x0000D5B1
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
		this.m_onAwake = new Action(this.OnAwake);
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x0000F3E9 File Offset: 0x0000D5E9
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x0000F404 File Offset: 0x0000D604
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		if (CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.Instance.enabled = false;
			return;
		}
		CaveLanternPostProcessingController.OnAwakeRelay.AddListener(this.m_onAwake, false);
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x0000F430 File Offset: 0x0000D630
	private void OnAwake()
	{
		CaveLanternPostProcessingController.Instance.enabled = false;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x0000F43D File Offset: 0x0000D63D
	private void OnPlayerExitRoom(MonoBehaviour sender, EventArgs args)
	{
		if (CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.Instance.enabled = true;
		}
		CaveLanternPostProcessingController.OnAwakeRelay.RemoveListener(this.m_onAwake);
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x0000F467 File Offset: 0x0000D667
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x04001ACF RID: 6863
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001AD0 RID: 6864
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04001AD1 RID: 6865
	private Action m_onAwake;
}
