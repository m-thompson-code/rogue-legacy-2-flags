using System;
using UnityEngine;

// Token: 0x020006AC RID: 1708
public class DisablePlatformOverride : MonoBehaviour
{
	// Token: 0x0600348E RID: 13454 RVA: 0x0001CD73 File Offset: 0x0001AF73
	private void Awake()
	{
		this.m_propSpawnController = base.GetComponent<PropSpawnController>();
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0001CDA5 File Offset: 0x0001AFA5
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x000DDAA8 File Offset: 0x000DBCA8
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		Prop propInstance = this.m_propSpawnController.PropInstance;
		if (propInstance && propInstance.HitboxController != null)
		{
			propInstance.HitboxController.SetHitboxActiveState(HitboxType.Platform, false);
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x000DDAE0 File Offset: 0x000DBCE0
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		Prop propInstance = this.m_propSpawnController.PropInstance;
		if (propInstance && propInstance.HitboxController != null)
		{
			propInstance.HitboxController.SetHitboxActiveState(HitboxType.Platform, true);
		}
	}

	// Token: 0x04002A7E RID: 10878
	private PropSpawnController m_propSpawnController;

	// Token: 0x04002A7F RID: 10879
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002A80 RID: 10880
	private Action<object, EventArgs> m_onPlayerExitRoom;
}
