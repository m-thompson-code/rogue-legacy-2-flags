using System;
using UnityEngine;

// Token: 0x0200042C RID: 1068
[Serializable]
public struct EnemyTypeAndRank
{
	// Token: 0x06002753 RID: 10067 RVA: 0x00082F9B File Offset: 0x0008119B
	public EnemyTypeAndRank(EnemyType enemyType, EnemyRank enemyLevel)
	{
		this.m_type = enemyType;
		this.m_rank = enemyLevel;
	}

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x06002754 RID: 10068 RVA: 0x00082FAB File Offset: 0x000811AB
	public EnemyType Type
	{
		get
		{
			return this.m_type;
		}
	}

	// Token: 0x17000F92 RID: 3986
	// (get) Token: 0x06002755 RID: 10069 RVA: 0x00082FB3 File Offset: 0x000811B3
	public EnemyRank Rank
	{
		get
		{
			return this.m_rank;
		}
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x00082FBB File Offset: 0x000811BB
	public override bool Equals(object obj)
	{
		return obj is EnemyTypeAndRank && this == (EnemyTypeAndRank)obj;
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x00082FD8 File Offset: 0x000811D8
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x00082FEA File Offset: 0x000811EA
	public override string ToString()
	{
		return string.Format("{0} {1}", this.m_rank, this.m_type);
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x0008300C File Offset: 0x0008120C
	public static bool operator ==(EnemyTypeAndRank x, EnemyTypeAndRank y)
	{
		return x.Rank == y.Rank && x.Type == y.Type;
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x00083030 File Offset: 0x00081230
	public static bool operator !=(EnemyTypeAndRank x, EnemyTypeAndRank y)
	{
		return !(x == y);
	}

	// Token: 0x040020FC RID: 8444
	[SerializeField]
	private EnemyType m_type;

	// Token: 0x040020FD RID: 8445
	[SerializeField]
	private EnemyRank m_rank;
}
