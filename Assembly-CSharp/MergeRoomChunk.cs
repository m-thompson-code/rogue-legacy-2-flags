using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class MergeRoomChunk : MonoBehaviour
{
	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06002ACB RID: 10955 RVA: 0x00090E76 File Offset: 0x0008F076
	public IRelayLink<Vector2> PlayerEnteredChunkRelay
	{
		get
		{
			return this.m_playerEnteredChunkRelay;
		}
	}

	// Token: 0x06002ACC RID: 10956 RVA: 0x00090E7E File Offset: 0x0008F07E
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
		{
			return;
		}
		this.m_playerEnteredChunkRelay.Dispatch(base.transform.position);
	}

	// Token: 0x040022F3 RID: 8947
	private Relay<Vector2> m_playerEnteredChunkRelay = new Relay<Vector2>();
}
