using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000786 RID: 1926
public class MergeRoomChunk : MonoBehaviour
{
	// Token: 0x170015C6 RID: 5574
	// (get) Token: 0x06003AFF RID: 15103 RVA: 0x000205D7 File Offset: 0x0001E7D7
	public IRelayLink<Vector2> PlayerEnteredChunkRelay
	{
		get
		{
			return this.m_playerEnteredChunkRelay;
		}
	}

	// Token: 0x06003B00 RID: 15104 RVA: 0x000205DF File Offset: 0x0001E7DF
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
		{
			return;
		}
		this.m_playerEnteredChunkRelay.Dispatch(base.transform.position);
	}

	// Token: 0x04002EF9 RID: 12025
	private Relay<Vector2> m_playerEnteredChunkRelay = new Relay<Vector2>();
}
