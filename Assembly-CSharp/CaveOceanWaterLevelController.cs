using System;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public class CaveOceanWaterLevelController : MonoBehaviour
{
	// Token: 0x06002DD8 RID: 11736 RVA: 0x0009A7B5 File Offset: 0x000989B5
	private void Awake()
	{
		this.m_propSpawnController = base.GetComponent<PropSpawnController>();
		this.m_propSpawnController.OnPropInstanceInitializedRelay.AddListener(new Action(this.PlaceOcean), false);
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x0009A7E1 File Offset: 0x000989E1
	private void OnDestroy()
	{
		if (this.m_propSpawnController)
		{
			this.m_propSpawnController.OnPropInstanceInitializedRelay.RemoveListener(new Action(this.PlaceOcean));
		}
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x0009A810 File Offset: 0x00098A10
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

	// Token: 0x040024A3 RID: 9379
	private PropSpawnController m_propSpawnController;
}
