using System;
using UnityEngine;

// Token: 0x02000A53 RID: 2643
public class PlayerSpawn : MonoBehaviour, IPlayerSpawn
{
	// Token: 0x06004FAB RID: 20395 RVA: 0x0002B752 File Offset: 0x00029952
	private void Start()
	{
		this.m_sprite.enabled = false;
	}

	// Token: 0x06004FAC RID: 20396 RVA: 0x0002B760 File Offset: 0x00029960
	public void SpawnPlayer()
	{
		GameObject player = PlayerManager.GetPlayer();
		player.SetActive(true);
		player.transform.SetParent(null);
		player.transform.position = base.transform.position;
	}

	// Token: 0x04003C6A RID: 15466
	[SerializeField]
	private SpriteRenderer m_sprite;
}
