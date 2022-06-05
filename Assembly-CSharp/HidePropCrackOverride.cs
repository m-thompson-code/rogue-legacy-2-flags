using System;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class HidePropCrackOverride : MonoBehaviour
{
	// Token: 0x060015E3 RID: 5603 RVA: 0x00044474 File Offset: 0x00042674
	private void Awake()
	{
		this.m_propSpawner = base.GetComponent<PropSpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x00044494 File Offset: 0x00042694
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x000444A2 File Offset: 0x000426A2
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x000444B0 File Offset: 0x000426B0
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (this.m_propSpawner != null && this.m_propSpawner.PropInstance != null && this.m_propSpawner.IsBreakable && !this.m_propSpawner.PropInstance.Breakable.IsBroken)
		{
			this.m_propSpawner.PropInstance.Breakable.SpriteRenderer.enabled = false;
		}
	}

	// Token: 0x04001516 RID: 5398
	private PropSpawnController m_propSpawner;

	// Token: 0x04001517 RID: 5399
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
