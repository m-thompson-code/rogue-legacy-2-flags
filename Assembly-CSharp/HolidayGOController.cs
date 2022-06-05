using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003BA RID: 954
public class HolidayGOController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x06001F7A RID: 8058 RVA: 0x00010816 File Offset: 0x0000EA16
	// (set) Token: 0x06001F7B RID: 8059 RVA: 0x0001081E File Offset: 0x0000EA1E
	public BaseRoom Room { get; private set; }

	// Token: 0x06001F7C RID: 8060 RVA: 0x00010827 File Offset: 0x0000EA27
	public void SetRoom(BaseRoom room)
	{
		if (this.m_triggerOnRoomEnter)
		{
			this.Room = room;
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnter, false);
		}
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x00010850 File Offset: 0x0000EA50
	private void Awake()
	{
		if (this.m_triggerOnSceneLoaded)
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}
		this.m_onPlayerEnter = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter);
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x0001087D File Offset: 0x0000EA7D
	private void OnDisable()
	{
		if (this.m_triggerOnRoomEnter && this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnter);
		}
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000108AB File Offset: 0x0000EAAB
	private void OnDestroy()
	{
		if (this.m_triggerOnSceneLoaded)
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x000108C6 File Offset: 0x0000EAC6
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		this.UpdateGO();
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x000108C6 File Offset: 0x0000EAC6
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.UpdateGO();
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000A2F7C File Offset: 0x000A117C
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

	// Token: 0x04001C17 RID: 7191
	[SerializeField]
	private bool m_triggerOnSceneLoaded;

	// Token: 0x04001C18 RID: 7192
	[SerializeField]
	private bool m_triggerOnRoomEnter;

	// Token: 0x04001C19 RID: 7193
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04001C1A RID: 7194
	[SerializeField]
	private bool m_enableOnActive = true;

	// Token: 0x04001C1B RID: 7195
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnter;
}
