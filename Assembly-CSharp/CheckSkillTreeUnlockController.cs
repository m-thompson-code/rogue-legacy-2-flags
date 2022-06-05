using System;
using UnityEngine;

// Token: 0x02000901 RID: 2305
public class CheckSkillTreeUnlockController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170018D3 RID: 6355
	// (get) Token: 0x0600460C RID: 17932 RVA: 0x0002678E File Offset: 0x0002498E
	// (set) Token: 0x0600460D RID: 17933 RVA: 0x00026796 File Offset: 0x00024996
	public BaseRoom Room { get; private set; }

	// Token: 0x0600460E RID: 17934 RVA: 0x0002679F File Offset: 0x0002499F
	private void Awake()
	{
		this.m_spawner = base.GetComponent<ISpawnController>();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x0600460F RID: 17935 RVA: 0x000267BF File Offset: 0x000249BF
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06004610 RID: 17936 RVA: 0x000267F3 File Offset: 0x000249F3
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterHubTown, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06004611 RID: 17937 RVA: 0x00112860 File Offset: 0x00110A60
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

	// Token: 0x04003617 RID: 13847
	[SerializeField]
	private SkillTreeType m_skillToCheckUnlock;

	// Token: 0x04003618 RID: 13848
	[SerializeField]
	private bool m_disableWhenUnlocked;

	// Token: 0x04003619 RID: 13849
	[SerializeField]
	[Tooltip("If enabled, will be considered unlocked if Global_EV.UNLOCK_ALL_SHOPS == true.")]
	private bool m_isShop;

	// Token: 0x0400361B RID: 13851
	private ISpawnController m_spawner;

	// Token: 0x0400361C RID: 13852
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
