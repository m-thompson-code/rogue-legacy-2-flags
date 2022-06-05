using System;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
public class BreakableDropOverride : MonoBehaviour
{
	// Token: 0x06002595 RID: 9621 RVA: 0x0007C49D File Offset: 0x0007A69D
	private void Awake()
	{
		this.m_propSpawnController = base.GetComponent<PropSpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x0007C4BD File Offset: 0x0007A6BD
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x0007C4CB File Offset: 0x0007A6CB
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x0007C4DC File Offset: 0x0007A6DC
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (this.m_propSpawnController && this.m_propSpawnController.ShouldSpawn && this.m_propSpawnController.IsBreakable)
		{
			this.m_propSpawnController.PropInstance.Breakable.ItemDropTypeOverride = this.m_itemDropOverride;
		}
	}

	// Token: 0x04001F90 RID: 8080
	[SerializeField]
	private ItemDropType m_itemDropOverride;

	// Token: 0x04001F91 RID: 8081
	private PropSpawnController m_propSpawnController;

	// Token: 0x04001F92 RID: 8082
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
