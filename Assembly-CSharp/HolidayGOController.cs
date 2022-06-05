using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000209 RID: 521
public class HolidayGOController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x060015EA RID: 5610 RVA: 0x0004458A File Offset: 0x0004278A
	// (set) Token: 0x060015EB RID: 5611 RVA: 0x00044592 File Offset: 0x00042792
	public BaseRoom Room { get; private set; }

	// Token: 0x060015EC RID: 5612 RVA: 0x0004459B File Offset: 0x0004279B
	public void SetRoom(BaseRoom room)
	{
		if (this.m_triggerOnRoomEnter)
		{
			this.Room = room;
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnter, false);
		}
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x000445C4 File Offset: 0x000427C4
	private void Awake()
	{
		if (this.m_triggerOnSceneLoaded)
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}
		this.m_onPlayerEnter = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter);
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x000445F1 File Offset: 0x000427F1
	private void OnDisable()
	{
		if (this.m_triggerOnRoomEnter && this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnter);
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x0004461F File Offset: 0x0004281F
	private void OnDestroy()
	{
		if (this.m_triggerOnSceneLoaded)
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x0004463A File Offset: 0x0004283A
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		this.UpdateGO();
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x00044642 File Offset: 0x00042842
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.UpdateGO();
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x0004464C File Offset: 0x0004284C
	private void UpdateGO()
	{
		if (!this)
		{
			return;
		}
		if (HolidayLookController.IsHoliday(this.m_holidayType))
		{
			if (this.m_enableOnActive)
			{
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
			return;
		}
		else
		{
			if (this.m_enableOnActive)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			return;
		}
	}

	// Token: 0x0400151A RID: 5402
	[SerializeField]
	private bool m_triggerOnSceneLoaded;

	// Token: 0x0400151B RID: 5403
	[SerializeField]
	private bool m_triggerOnRoomEnter;

	// Token: 0x0400151C RID: 5404
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x0400151D RID: 5405
	[SerializeField]
	private bool m_enableOnActive = true;

	// Token: 0x0400151E RID: 5406
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnter;
}
