using System;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public class TiledPropCollider : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001482 RID: 5250
	// (get) Token: 0x06003A1E RID: 14878 RVA: 0x000C57B9 File Offset: 0x000C39B9
	// (set) Token: 0x06003A1F RID: 14879 RVA: 0x000C57C1 File Offset: 0x000C39C1
	public BaseRoom Room { get; private set; }

	// Token: 0x06003A20 RID: 14880 RVA: 0x000C57CA File Offset: 0x000C39CA
	private void Awake()
	{
		this.m_spriteRenderer = base.GetComponentInChildren<SpriteRenderer>();
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x000C57D8 File Offset: 0x000C39D8
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		}
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x000C580D File Offset: 0x000C3A0D
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x000C583C File Offset: 0x000C3A3C
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_collider.size = this.m_spriteRenderer.size;
		this.m_collider.offset = new Vector2(0f, this.m_spriteRenderer.size.y / 2f);
	}

	// Token: 0x04002C95 RID: 11413
	[SerializeField]
	private BoxCollider2D m_collider;

	// Token: 0x04002C96 RID: 11414
	private SpriteRenderer m_spriteRenderer;
}
