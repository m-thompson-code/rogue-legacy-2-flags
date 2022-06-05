using System;
using UnityEngine;

// Token: 0x02000900 RID: 2304
public class CheckPlayerFlagUnlockController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170018D2 RID: 6354
	// (get) Token: 0x06004605 RID: 17925 RVA: 0x000266F0 File Offset: 0x000248F0
	// (set) Token: 0x06004606 RID: 17926 RVA: 0x000266F8 File Offset: 0x000248F8
	public BaseRoom Room { get; private set; }

	// Token: 0x06004607 RID: 17927 RVA: 0x00026701 File Offset: 0x00024901
	private void Awake()
	{
		this.m_spawner = base.GetComponent<ISpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x00026721 File Offset: 0x00024921
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06004609 RID: 17929 RVA: 0x00026755 File Offset: 0x00024955
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x00112730 File Offset: 0x00110930
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

	// Token: 0x04003611 RID: 13841
	[SerializeField]
	private PlayerSaveFlag m_playerFlagToCheck;

	// Token: 0x04003612 RID: 13842
	[SerializeField]
	private bool m_disableWhenUnlocked;

	// Token: 0x04003613 RID: 13843
	[SerializeField]
	[Tooltip("If enabled, will be considered unlocked if Global_EV.UNLOCK_ALL_SHOPS == true.")]
	private bool m_isShop;

	// Token: 0x04003615 RID: 13845
	private ISpawnController m_spawner;

	// Token: 0x04003616 RID: 13846
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
