using System;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class CheckSkillTreeUnlockController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001244 RID: 4676
	// (get) Token: 0x060031DA RID: 12762 RVA: 0x000A88D9 File Offset: 0x000A6AD9
	// (set) Token: 0x060031DB RID: 12763 RVA: 0x000A88E1 File Offset: 0x000A6AE1
	public BaseRoom Room { get; private set; }

	// Token: 0x060031DC RID: 12764 RVA: 0x000A88EA File Offset: 0x000A6AEA
	private void Awake()
	{
		this.m_spawner = base.GetComponent<ISpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x000A890A File Offset: 0x000A6B0A
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x000A893E File Offset: 0x000A6B3E
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x000A8978 File Offset: 0x000A6B78
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		bool flag = SkillTreeManager.GetSkillObjLevel(this.m_skillToCheckUnlock) > 0;
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

	// Token: 0x04002742 RID: 10050
	[SerializeField]
	private SkillTreeType m_skillToCheckUnlock;

	// Token: 0x04002743 RID: 10051
	[SerializeField]
	private bool m_disableWhenUnlocked;

	// Token: 0x04002744 RID: 10052
	[SerializeField]
	[Tooltip("If enabled, will be considered unlocked if Global_EV.UNLOCK_ALL_SHOPS == true.")]
	private bool m_isShop;

	// Token: 0x04002746 RID: 10054
	private ISpawnController m_spawner;

	// Token: 0x04002747 RID: 10055
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
