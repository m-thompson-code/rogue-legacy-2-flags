using System;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class TraitorBossGeoController : MonoBehaviour
{
	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x06001F66 RID: 8038 RVA: 0x00064B84 File Offset: 0x00062D84
	public GameObject ArmillarySphere
	{
		get
		{
			return this.m_armillarySphere;
		}
	}

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x06001F67 RID: 8039 RVA: 0x00064B8C File Offset: 0x00062D8C
	public GameObject Arrow
	{
		get
		{
			return this.m_arrow;
		}
	}

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x06001F68 RID: 8040 RVA: 0x00064B94 File Offset: 0x00062D94
	public GameObject Axe
	{
		get
		{
			return this.m_axe;
		}
	}

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x06001F69 RID: 8041 RVA: 0x00064B9C File Offset: 0x00062D9C
	public GameObject Bow
	{
		get
		{
			return this.m_bow;
		}
	}

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x06001F6A RID: 8042 RVA: 0x00064BA4 File Offset: 0x00062DA4
	public GameObject DualBladesL
	{
		get
		{
			return this.m_dualBladesL;
		}
	}

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x06001F6B RID: 8043 RVA: 0x00064BAC File Offset: 0x00062DAC
	public GameObject DualBladesR
	{
		get
		{
			return this.m_dualBladesR;
		}
	}

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x06001F6C RID: 8044 RVA: 0x00064BB4 File Offset: 0x00062DB4
	public GameObject Katana
	{
		get
		{
			return this.m_katana;
		}
	}

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x06001F6D RID: 8045 RVA: 0x00064BBC File Offset: 0x00062DBC
	public GameObject Ladle
	{
		get
		{
			return this.m_ladle;
		}
	}

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x06001F6E RID: 8046 RVA: 0x00064BC4 File Offset: 0x00062DC4
	public GameObject Pizza
	{
		get
		{
			return this.m_pizza;
		}
	}

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x06001F6F RID: 8047 RVA: 0x00064BCC File Offset: 0x00062DCC
	public GameObject Scythe
	{
		get
		{
			return this.m_scythe;
		}
	}

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06001F70 RID: 8048 RVA: 0x00064BD4 File Offset: 0x00062DD4
	public GameObject Staff
	{
		get
		{
			return this.m_staff;
		}
	}

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x06001F71 RID: 8049 RVA: 0x00064BDC File Offset: 0x00062DDC
	public GameObject Sword
	{
		get
		{
			return this.m_sword;
		}
	}

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x06001F72 RID: 8050 RVA: 0x00064BE4 File Offset: 0x00062DE4
	public GameObject[] AllWeaponGeo
	{
		get
		{
			if (this.m_allGeo == null)
			{
				this.m_allGeo = new GameObject[]
				{
					this.ArmillarySphere,
					this.Arrow,
					this.Axe,
					this.Bow,
					this.DualBladesL,
					this.DualBladesR,
					this.Katana,
					this.Ladle,
					this.Pizza,
					this.Scythe,
					this.Staff,
					this.Sword
				};
			}
			return this.m_allGeo;
		}
	}

	// Token: 0x04001C18 RID: 7192
	[SerializeField]
	private GameObject m_armillarySphere;

	// Token: 0x04001C19 RID: 7193
	[SerializeField]
	private GameObject m_arrow;

	// Token: 0x04001C1A RID: 7194
	[SerializeField]
	private GameObject m_axe;

	// Token: 0x04001C1B RID: 7195
	[SerializeField]
	private GameObject m_bow;

	// Token: 0x04001C1C RID: 7196
	[SerializeField]
	private GameObject m_dualBladesL;

	// Token: 0x04001C1D RID: 7197
	[SerializeField]
	private GameObject m_dualBladesR;

	// Token: 0x04001C1E RID: 7198
	[SerializeField]
	private GameObject m_katana;

	// Token: 0x04001C1F RID: 7199
	[SerializeField]
	private GameObject m_ladle;

	// Token: 0x04001C20 RID: 7200
	[SerializeField]
	private GameObject m_pizza;

	// Token: 0x04001C21 RID: 7201
	[SerializeField]
	private GameObject m_scythe;

	// Token: 0x04001C22 RID: 7202
	[SerializeField]
	private GameObject m_staff;

	// Token: 0x04001C23 RID: 7203
	[SerializeField]
	private GameObject m_sword;

	// Token: 0x04001C24 RID: 7204
	private GameObject[] m_allGeo;
}
