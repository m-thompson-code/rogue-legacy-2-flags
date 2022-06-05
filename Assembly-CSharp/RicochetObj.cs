using System;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class RicochetObj : MonoBehaviour, IRicochetObj, IRootObj
{
	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x060026CA RID: 9930 RVA: 0x00015B7B File Offset: 0x00013D7B
	// (set) Token: 0x060026CB RID: 9931 RVA: 0x00015B83 File Offset: 0x00013D83
	public Vector2 ExternalRicochetKnockbackAmount
	{
		get
		{
			return this.m_externalRicochetKnockbackAmount;
		}
		set
		{
			this.m_externalRicochetKnockbackAmount = value;
		}
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002183 RID: 8579
	[SerializeField]
	private Vector2 m_externalRicochetKnockbackAmount = Player_EV.PLAYER_BASE_KNOCKBACK_DISTANCE;
}
