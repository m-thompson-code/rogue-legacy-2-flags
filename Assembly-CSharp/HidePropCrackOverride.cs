using System;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class HidePropCrackOverride : MonoBehaviour
{
	// Token: 0x06001F73 RID: 8051 RVA: 0x000107DA File Offset: 0x0000E9DA
	private void Awake()
	{
		this.m_propSpawner = base.GetComponent<PropSpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x000107FA File Offset: 0x0000E9FA
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x00010808 File Offset: 0x0000EA08
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x000A2EB0 File Offset: 0x000A10B0
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (this.m_propSpawner != null && this.m_propSpawner.PropInstance != null && this.m_propSpawner.IsBreakable && !this.m_propSpawner.PropInstance.Breakable.IsBroken)
		{
			this.m_propSpawner.PropInstance.Breakable.SpriteRenderer.enabled = false;
		}
	}

	// Token: 0x04001C13 RID: 7187
	private PropSpawnController m_propSpawner;

	// Token: 0x04001C14 RID: 7188
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
