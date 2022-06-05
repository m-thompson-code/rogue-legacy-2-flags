using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200048A RID: 1162
public class ClownNPCController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x1700108A RID: 4234
	// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000913BD File Offset: 0x0008F5BD
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x000913C8 File Offset: 0x0008F5C8
	public void SetRoom(BaseRoom room)
	{
		this.m_room = room;
		ClownEntranceRoomController component = this.m_room.gameObject.GetComponent<ClownEntranceRoomController>();
		if (component != null)
		{
			this.m_interact.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(component.TriggerDialogue));
		}
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x00091412 File Offset: 0x0008F612
	private void Awake()
	{
		this.m_interact = base.GetComponent<Interactable>();
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x00091420 File Offset: 0x0008F620
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

	// Token: 0x04002302 RID: 8962
	private Interactable m_interact;

	// Token: 0x04002303 RID: 8963
	private BaseRoom m_room;
}
