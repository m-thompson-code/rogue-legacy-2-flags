using System;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
public class ConnectorRoomController : MonoBehaviour
{
	// Token: 0x170013F7 RID: 5111
	// (get) Token: 0x06003484 RID: 13444 RVA: 0x0001CD3E File Offset: 0x0001AF3E
	// (set) Token: 0x06003485 RID: 13445 RVA: 0x0001CD46 File Offset: 0x0001AF46
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

	// Token: 0x170013F8 RID: 5112
	// (get) Token: 0x06003486 RID: 13446 RVA: 0x0001CD4F File Offset: 0x0001AF4F
	// (set) Token: 0x06003487 RID: 13447 RVA: 0x0001CD57 File Offset: 0x0001AF57
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

	// Token: 0x06003488 RID: 13448 RVA: 0x0001CD60 File Offset: 0x0001AF60
	private void Awake()
	{
		this.TunnelSpawnController.GameObject.SetActive(false);
	}

	// Token: 0x04002A7C RID: 10876
	[SerializeField]
	private Room m_room;

	// Token: 0x04002A7D RID: 10877
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawnController;
}
