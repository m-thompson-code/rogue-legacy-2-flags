using System;
using UnityEngine;

// Token: 0x02000A78 RID: 2680
public class TiledPropCollider : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001BE9 RID: 7145
	// (get) Token: 0x06005103 RID: 20739 RVA: 0x0002C3AE File Offset: 0x0002A5AE
	// (set) Token: 0x06005104 RID: 20740 RVA: 0x0002C3B6 File Offset: 0x0002A5B6
	public BaseRoom Room { get; private set; }

	// Token: 0x06005105 RID: 20741 RVA: 0x0002C3BF File Offset: 0x0002A5BF
	private void Awake()
	{
		this.m_spriteRenderer = base.GetComponentInChildren<SpriteRenderer>();
	}

	// Token: 0x06005106 RID: 20742 RVA: 0x0002C3CD File Offset: 0x0002A5CD
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		}
	}

	// Token: 0x06005107 RID: 20743 RVA: 0x0002C402 File Offset: 0x0002A602
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06005108 RID: 20744 RVA: 0x001336C0 File Offset: 0x001318C0
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_collider.size = this.m_spriteRenderer.size;
		this.m_collider.offset = new Vector2(0f, this.m_spriteRenderer.size.y / 2f);
	}

	// Token: 0x04003D2B RID: 15659
	[SerializeField]
	private BoxCollider2D m_collider;

	// Token: 0x04003D2C RID: 15660
	private SpriteRenderer m_spriteRenderer;
}
