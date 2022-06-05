using System;
using UnityEngine;

// Token: 0x0200069C RID: 1692
public class BreakableDropOverride : MonoBehaviour
{
	// Token: 0x06003403 RID: 13315 RVA: 0x0001C877 File Offset: 0x0001AA77
	private void Awake()
	{
		this.m_propSpawnController = base.GetComponent<PropSpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x0001C897 File Offset: 0x0001AA97
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x0001C8A5 File Offset: 0x0001AAA5
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x000DC244 File Offset: 0x000DA444
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (this.m_propSpawnController && this.m_propSpawnController.ShouldSpawn && this.m_propSpawnController.IsBreakable)
		{
			this.m_propSpawnController.PropInstance.Breakable.ItemDropTypeOverride = this.m_itemDropOverride;
		}
	}

	// Token: 0x04002A31 RID: 10801
	[SerializeField]
	private ItemDropType m_itemDropOverride;

	// Token: 0x04002A32 RID: 10802
	private PropSpawnController m_propSpawnController;

	// Token: 0x04002A33 RID: 10803
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
