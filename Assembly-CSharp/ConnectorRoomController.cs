using System;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class ConnectorRoomController : MonoBehaviour
{
	// Token: 0x17000F44 RID: 3908
	// (get) Token: 0x06002605 RID: 9733 RVA: 0x0007DA3E File Offset: 0x0007BC3E
	// (set) Token: 0x06002606 RID: 9734 RVA: 0x0007DA46 File Offset: 0x0007BC46
	public TunnelSpawnController TunnelSpawnController
	{
		get
		{
			return this.m_tunnelSpawnController;
		}
		set
		{
			this.m_tunnelSpawnController = value;
		}
	}

	// Token: 0x17000F45 RID: 3909
	// (get) Token: 0x06002607 RID: 9735 RVA: 0x0007DA4F File Offset: 0x0007BC4F
	// (set) Token: 0x06002608 RID: 9736 RVA: 0x0007DA57 File Offset: 0x0007BC57
	public Room Room
	{
		get
		{
			return this.m_room;
		}
		set
		{
			this.m_room = value;
		}
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x0007DA60 File Offset: 0x0007BC60
	private void Awake()
	{
		this.TunnelSpawnController.GameObject.SetActive(false);
	}

	// Token: 0x04001FC8 RID: 8136
	[SerializeField]
	private Room m_room;

	// Token: 0x04001FC9 RID: 8137
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawnController;
}
