using System;
using UnityEngine;

// Token: 0x020006F2 RID: 1778
[Serializable]
public struct EnemyTypeAndRank
{
	// Token: 0x06003656 RID: 13910 RVA: 0x0001DDCD File Offset: 0x0001BFCD
	public EnemyTypeAndRank(EnemyType enemyType, EnemyRank enemyLevel)
	{
		this.m_type = enemyType;
		this.m_rank = enemyLevel;
	}

	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x06003657 RID: 13911 RVA: 0x0001DDDD File Offset: 0x0001BFDD
	public EnemyType Type
	{
		get
		{
			return this.m_type;
		}
	}

	// Token: 0x17001471 RID: 5233
	// (get) Token: 0x06003658 RID: 13912 RVA: 0x0001DDE5 File Offset: 0x0001BFE5
	public EnemyRank Rank
	{
		get
		{
			return this.m_rank;
		}
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x0001DDED File Offset: 0x0001BFED
	public override bool Equals(object obj)
	{
		return obj is EnemyTypeAndRank && this == (EnemyTypeAndRank)obj;
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x0001DE0A File Offset: 0x0001C00A
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x0001DE1C File Offset: 0x0001C01C
	public override string ToString()
	{
		return string.Format("{0} {1}", this.m_rank, this.m_type);
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x0001DE3E File Offset: 0x0001C03E
	public static bool operator ==(EnemyTypeAndRank x, EnemyTypeAndRank y)
	{
		return x.Rank == y.Rank && x.Type == y.Type;
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x0001DE62 File Offset: 0x0001C062
	public static bool operator !=(EnemyTypeAndRank x, EnemyTypeAndRank y)
	{
		return !(x == y);
	}

	// Token: 0x04002C1F RID: 11295
	[SerializeField]
	private EnemyType m_type;

	// Token: 0x04002C20 RID: 11296
	[SerializeField]
	private EnemyRank m_rank;
}
