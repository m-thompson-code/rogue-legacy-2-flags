using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000789 RID: 1929
public class ClownNPCController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170015CB RID: 5579
	// (get) Token: 0x06003B13 RID: 15123 RVA: 0x0002070A File Offset: 0x0001E90A
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x000F2EE8 File Offset: 0x000F10E8
	public void SetRoom(BaseRoom room)
	{
		this.m_room = room;
		ClownEntranceRoomController component = this.m_room.gameObject.GetComponent<ClownEntranceRoomController>();
		if (component != null)
		{
			this.m_interact.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(component.TriggerDialogue));
		}
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x00020712 File Offset: 0x0001E912
	private void Awake()
	{
		this.m_interact = base.GetComponent<Interactable>();
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x000F2F34 File Offset: 0x000F1134
	private void OnDestroy()
	{
		if (this.m_room != null)
		{
			ClownEntranceRoomController component = this.m_room.gameObject.GetComponent<ClownEntranceRoomController>();
			if (component != null && this.m_interact != null)
			{
				this.m_interact.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(component.TriggerDialogue));
			}
		}
	}

	// Token: 0x04002F08 RID: 12040
	private Interactable m_interact;

	// Token: 0x04002F09 RID: 12041
	private BaseRoom m_room;
}
