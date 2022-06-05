using System;
using UnityEngine;

// Token: 0x02000A20 RID: 2592
public class DecoController : MonoBehaviour
{
	// Token: 0x17001B17 RID: 6935
	// (get) Token: 0x06004E5E RID: 20062 RVA: 0x0002AA56 File Offset: 0x00028C56
	// (set) Token: 0x06004E5F RID: 20063 RVA: 0x0002AA5E File Offset: 0x00028C5E
	public DecoLocation[] DecoLocations
	{
		get
		{
			return this.m_decoLocations;
		}
		set
		{
			this.m_decoLocations = value;
		}
	}

	// Token: 0x17001B18 RID: 6936
	// (get) Token: 0x06004E60 RID: 20064 RVA: 0x0002AA67 File Offset: 0x00028C67
	public Prop OwnerProp
	{
		get
		{
			if (!this.m_ownerProp)
			{
				this.m_ownerProp = base.GetComponent<Prop>();
			}
			return this.m_ownerProp;
		}
	}

	// Token: 0x06004E61 RID: 20065 RVA: 0x0012D85C File Offset: 0x0012BA5C
	public void Initialize(DecoSpawnData[] decoLocationSpawnData)
	{
		int num = this.DecoLocations.Length;
		for (int i = 0; i < num; i++)
		{
			DecoLocation decoLocation = this.DecoLocations[i];
			decoLocation.SetDecoController(this);
			DecoSpawnData decoSpawnData = decoLocationSpawnData[i];
			if (decoSpawnData.ShouldSpawn)
			{
				DecoEntry decoEntry = decoLocation.PotentialDecos[(int)decoSpawnData.DecoPropIndex];
				bool isFlipped = false;
				Deco decoPrefab = null;
				if (decoEntry != null)
				{
					decoPrefab = decoEntry.Deco;
					isFlipped = (decoEntry.CanFlip && decoSpawnData.IsFlipped);
				}
				decoLocation.Initialize(this, decoPrefab, isFlipped);
			}
		}
	}

	// Token: 0x04003B19 RID: 15129
	[SerializeField]
	private DecoLocation[] m_decoLocations;

	// Token: 0x04003B1A RID: 15130
	private Prop m_ownerProp;
}
