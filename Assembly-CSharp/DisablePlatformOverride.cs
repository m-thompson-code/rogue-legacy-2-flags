using System;
using UnityEngine;

// Token: 0x020003FF RID: 1023
public class DisablePlatformOverride : MonoBehaviour
{
	// Token: 0x0600260F RID: 9743 RVA: 0x0007DA7B File Offset: 0x0007BC7B
	private void Awake()
	{
		this.m_propSpawnController = base.GetComponent<PropSpawnController>();
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x0007DAAD File Offset: 0x0007BCAD
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x0007DAC8 File Offset: 0x0007BCC8
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x0007DAE4 File Offset: 0x0007BCE4
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		Prop propInstance = this.m_propSpawnController.PropInstance;
		if (propInstance && propInstance.HitboxController != null)
		{
			propInstance.HitboxController.SetHitboxActiveState(HitboxType.Platform, false);
		}
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x0007DB1C File Offset: 0x0007BD1C
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		Prop propInstance = this.m_propSpawnController.PropInstance;
		if (propInstance && propInstance.HitboxController != null)
		{
			propInstance.HitboxController.SetHitboxActiveState(HitboxType.Platform, true);
		}
	}

	// Token: 0x04001FCA RID: 8138
	private PropSpawnController m_propSpawnController;

	// Token: 0x04001FCB RID: 8139
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001FCC RID: 8140
	private Action<object, EventArgs> m_onPlayerExitRoom;
}
