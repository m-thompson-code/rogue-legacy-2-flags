using System;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class DisableCaveLanternInRoom : MonoBehaviour
{
	// Token: 0x06001462 RID: 5218 RVA: 0x0003DE2D File Offset: 0x0003C02D
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
		this.m_onAwake = new Action(this.OnAwake);
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x0003DE65 File Offset: 0x0003C065
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x0003DE80 File Offset: 0x0003C080
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		if (CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.Instance.enabled = false;
			return;
		}
		CaveLanternPostProcessingController.OnAwakeRelay.AddListener(this.m_onAwake, false);
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x0003DEAC File Offset: 0x0003C0AC
	private void OnAwake()
	{
		CaveLanternPostProcessingController.Instance.enabled = false;
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x0003DEB9 File Offset: 0x0003C0B9
	private void OnPlayerExitRoom(MonoBehaviour sender, EventArgs args)
	{
		if (CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.Instance.enabled = true;
		}
		CaveLanternPostProcessingController.OnAwakeRelay.RemoveListener(this.m_onAwake);
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x0003DEE3 File Offset: 0x0003C0E3
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x04001428 RID: 5160
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001429 RID: 5161
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x0400142A RID: 5162
	private Action m_onAwake;
}
