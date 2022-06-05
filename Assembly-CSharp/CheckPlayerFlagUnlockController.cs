using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class CheckPlayerFlagUnlockController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001243 RID: 4675
	// (get) Token: 0x060031D3 RID: 12755 RVA: 0x000A8704 File Offset: 0x000A6904
	// (set) Token: 0x060031D4 RID: 12756 RVA: 0x000A870C File Offset: 0x000A690C
	public BaseRoom Room { get; private set; }

	// Token: 0x060031D5 RID: 12757 RVA: 0x000A8715 File Offset: 0x000A6915
	private void Awake()
	{
		this.m_spawner = base.GetComponent<ISpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x000A8735 File Offset: 0x000A6935
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x000A8769 File Offset: 0x000A6969
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x000A87A4 File Offset: 0x000A69A4
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		bool flag = SaveManager.PlayerSaveData.GetFlag(this.m_playerFlagToCheck);
		GameObject gameObject = base.gameObject;
		if (!this.m_spawner.IsNativeNull())
		{
			bool flag2 = false;
			PropSpawnController propSpawnController = this.m_spawner as PropSpawnController;
			if (propSpawnController && propSpawnController.PropInstance)
			{
				gameObject = propSpawnController.PropInstance.gameObject;
				flag2 = true;
			}
			EnemySpawnController enemySpawnController = this.m_spawner as EnemySpawnController;
			if (!flag2 && enemySpawnController && enemySpawnController.EnemyInstance)
			{
				gameObject = enemySpawnController.EnemyInstance.gameObject;
				flag2 = true;
			}
			IHazardSpawnController hazardSpawnController = this.m_spawner as IHazardSpawnController;
			if (!flag2 && hazardSpawnController != null && !hazardSpawnController.Hazard.IsNativeNull())
			{
				gameObject = hazardSpawnController.Hazard.gameObject;
				flag2 = true;
			}
			SpecialPlatformSpawnController specialPlatformSpawnController = this.m_spawner as SpecialPlatformSpawnController;
			if (!flag2 && specialPlatformSpawnController && specialPlatformSpawnController.SpecialPlatformInstance)
			{
				gameObject = specialPlatformSpawnController.SpecialPlatformInstance.gameObject;
			}
		}
		if (flag)
		{
			if (!this.m_disableWhenUnlocked)
			{
				gameObject.SetActive(true);
				return;
			}
			gameObject.SetActive(false);
			return;
		}
		else
		{
			if (!this.m_disableWhenUnlocked)
			{
				gameObject.SetActive(false);
				return;
			}
			gameObject.SetActive(true);
			return;
		}
	}

	// Token: 0x0400273C RID: 10044
	[SerializeField]
	private PlayerSaveFlag m_playerFlagToCheck;

	// Token: 0x0400273D RID: 10045
	[SerializeField]
	private bool m_disableWhenUnlocked;

	// Token: 0x0400273E RID: 10046
	[SerializeField]
	[Tooltip("If enabled, will be considered unlocked if Global_EV.UNLOCK_ALL_SHOPS == true.")]
	private bool m_isShop;

	// Token: 0x04002740 RID: 10048
	private ISpawnController m_spawner;

	// Token: 0x04002741 RID: 10049
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
