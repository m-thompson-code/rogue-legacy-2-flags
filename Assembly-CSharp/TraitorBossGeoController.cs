using System;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class TraitorBossGeoController : MonoBehaviour
{
	// Token: 0x170011B0 RID: 4528
	// (get) Token: 0x06002C01 RID: 11265 RVA: 0x00018758 File Offset: 0x00016958
	public GameObject ArmillarySphere
	{
		get
		{
			return this.m_armillarySphere;
		}
	}

	// Token: 0x170011B1 RID: 4529
	// (get) Token: 0x06002C02 RID: 11266 RVA: 0x00018760 File Offset: 0x00016960
	public GameObject Arrow
	{
		get
		{
			return this.m_arrow;
		}
	}

	// Token: 0x170011B2 RID: 4530
	// (get) Token: 0x06002C03 RID: 11267 RVA: 0x00018768 File Offset: 0x00016968
	public GameObject Axe
	{
		get
		{
			return this.m_axe;
		}
	}

	// Token: 0x170011B3 RID: 4531
	// (get) Token: 0x06002C04 RID: 11268 RVA: 0x00018770 File Offset: 0x00016970
	public GameObject Bow
	{
		get
		{
			return this.m_bow;
		}
	}

	// Token: 0x170011B4 RID: 4532
	// (get) Token: 0x06002C05 RID: 11269 RVA: 0x00018778 File Offset: 0x00016978
	public GameObject DualBladesL
	{
		get
		{
			return this.m_dualBladesL;
		}
	}

	// Token: 0x170011B5 RID: 4533
	// (get) Token: 0x06002C06 RID: 11270 RVA: 0x00018780 File Offset: 0x00016980
	public GameObject DualBladesR
	{
		get
		{
			return this.m_dualBladesR;
		}
	}

	// Token: 0x170011B6 RID: 4534
	// (get) Token: 0x06002C07 RID: 11271 RVA: 0x00018788 File Offset: 0x00016988
	public GameObject Katana
	{
		get
		{
			return this.m_katana;
		}
	}

	// Token: 0x170011B7 RID: 4535
	// (get) Token: 0x06002C08 RID: 11272 RVA: 0x00018790 File Offset: 0x00016990
	public GameObject Ladle
	{
		get
		{
			return this.m_ladle;
		}
	}

	// Token: 0x170011B8 RID: 4536
	// (get) Token: 0x06002C09 RID: 11273 RVA: 0x00018798 File Offset: 0x00016998
	public GameObject Pizza
	{
		get
		{
			return this.m_pizza;
		}
	}

	// Token: 0x170011B9 RID: 4537
	// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000187A0 File Offset: 0x000169A0
	public GameObject Scythe
	{
		get
		{
			return this.m_scythe;
		}
	}

	// Token: 0x170011BA RID: 4538
	// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000187A8 File Offset: 0x000169A8
	public GameObject Staff
	{
		get
		{
			return this.m_staff;
		}
	}

	// Token: 0x170011BB RID: 4539
	// (get) Token: 0x06002C0C RID: 11276 RVA: 0x000187B0 File Offset: 0x000169B0
	public GameObject Sword
	{
		get
		{
			return this.m_sword;
		}
	}

	// Token: 0x170011BC RID: 4540
	// (get) Token: 0x06002C0D RID: 11277 RVA: 0x000C5080 File Offset: 0x000C3280
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

	// Token: 0x04002536 RID: 9526
	[SerializeField]
	private GameObject m_armillarySphere;

	// Token: 0x04002537 RID: 9527
	[SerializeField]
	private GameObject m_arrow;

	// Token: 0x04002538 RID: 9528
	[SerializeField]
	private GameObject m_axe;

	// Token: 0x04002539 RID: 9529
	[SerializeField]
	private GameObject m_bow;

	// Token: 0x0400253A RID: 9530
	[SerializeField]
	private GameObject m_dualBladesL;

	// Token: 0x0400253B RID: 9531
	[SerializeField]
	private GameObject m_dualBladesR;

	// Token: 0x0400253C RID: 9532
	[SerializeField]
	private GameObject m_katana;

	// Token: 0x0400253D RID: 9533
	[SerializeField]
	private GameObject m_ladle;

	// Token: 0x0400253E RID: 9534
	[SerializeField]
	private GameObject m_pizza;

	// Token: 0x0400253F RID: 9535
	[SerializeField]
	private GameObject m_scythe;

	// Token: 0x04002540 RID: 9536
	[SerializeField]
	private GameObject m_staff;

	// Token: 0x04002541 RID: 9537
	[SerializeField]
	private GameObject m_sword;

	// Token: 0x04002542 RID: 9538
	private GameObject[] m_allGeo;
}
