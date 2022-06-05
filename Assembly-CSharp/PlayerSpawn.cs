using System;
using UnityEngine;

// Token: 0x02000627 RID: 1575
public class PlayerSpawn : MonoBehaviour, IPlayerSpawn
{
	// Token: 0x060038D2 RID: 14546 RVA: 0x000C1DE7 File Offset: 0x000BFFE7
	private void Start()
	{
		this.m_sprite.enabled = false;
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x000C1DF5 File Offset: 0x000BFFF5
	public void SpawnPlayer()
	{
		GameObject player = PlayerManager.GetPlayer();
		player.SetActive(true);
		player.transform.SetParent(null);
		player.transform.position = base.transform.position;
	}

	// Token: 0x04002BDB RID: 11227
	[SerializeField]
	private SpriteRenderer m_sprite;
}
