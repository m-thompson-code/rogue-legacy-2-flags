using System;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class DecoController : MonoBehaviour
{
	// Token: 0x170013C4 RID: 5060
	// (get) Token: 0x060037CD RID: 14285 RVA: 0x000BF04C File Offset: 0x000BD24C
	// (set) Token: 0x060037CE RID: 14286 RVA: 0x000BF054 File Offset: 0x000BD254
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

	// Token: 0x170013C5 RID: 5061
	// (get) Token: 0x060037CF RID: 14287 RVA: 0x000BF05D File Offset: 0x000BD25D
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

	// Token: 0x060037D0 RID: 14288 RVA: 0x000BF080 File Offset: 0x000BD280
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

	// Token: 0x04002AC1 RID: 10945
	[SerializeField]
	private DecoLocation[] m_decoLocations;

	// Token: 0x04002AC2 RID: 10946
	private Prop m_ownerProp;
}
