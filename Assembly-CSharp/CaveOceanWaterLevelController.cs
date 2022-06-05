using System;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public class CaveOceanWaterLevelController : MonoBehaviour
{
	// Token: 0x06003F02 RID: 16130 RVA: 0x00022DA8 File Offset: 0x00020FA8
	private void Awake()
	{
		this.m_propSpawnController = base.GetComponent<PropSpawnController>();
		this.m_propSpawnController.OnPropInstanceInitializedRelay.AddListener(new Action(this.PlaceOcean), false);
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x00022DD4 File Offset: 0x00020FD4
	private void OnDestroy()
	{
		if (this.m_propSpawnController)
		{
			this.m_propSpawnController.OnPropInstanceInitializedRelay.RemoveListener(new Action(this.PlaceOcean));
		}
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x000FBF78 File Offset: 0x000FA178
	private void PlaceOcean()
	{
		int num = 0;
		if (BossID_RL.IsBossBeaten(BossID.Castle_Boss))
		{
			num++;
		}
		if (BossID_RL.IsBossBeaten(BossID.Bridge_Boss))
		{
			num++;
		}
		if (BossID_RL.IsBossBeaten(BossID.Forest_Boss))
		{
			num++;
		}
		if (BossID_RL.IsBossBeaten(BossID.Study_Boss))
		{
			num++;
		}
		if (BossID_RL.IsBossBeaten(BossID.Tower_Boss))
		{
			num++;
		}
		if (BossID_RL.IsBossBeaten(BossID.Cave_Boss))
		{
			num++;
		}
		Vector3 position = this.m_propSpawnController.PropInstance.transform.position;
		position.y += 2.5f * (float)num;
		this.m_propSpawnController.PropInstance.transform.position = position;
	}

	// Token: 0x04003152 RID: 12626
	private PropSpawnController m_propSpawnController;
}
