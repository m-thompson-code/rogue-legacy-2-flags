using System;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class RicochetObj : MonoBehaviour, IRicochetObj, IRootObj
{
	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x06001C08 RID: 7176 RVA: 0x0005A720 File Offset: 0x00058920
	// (set) Token: 0x06001C09 RID: 7177 RVA: 0x0005A728 File Offset: 0x00058928
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

	// Token: 0x06001C0B RID: 7179 RVA: 0x0005A744 File Offset: 0x00058944
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001988 RID: 6536
	[SerializeField]
	private Vector2 m_externalRicochetKnockbackAmount = Player_EV.PLAYER_BASE_KNOCKBACK_DISTANCE;
}
