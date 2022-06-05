using System;
using UnityEngine;

// Token: 0x02000622 RID: 1570
public class PlayerRoomSpawn : MonoBehaviour
{
	// Token: 0x060038C9 RID: 14537 RVA: 0x000C1DC9 File Offset: 0x000BFFC9
	private void Start()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
	}
}
