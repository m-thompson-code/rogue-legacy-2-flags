using System;
using UnityEngine;

// Token: 0x020005DE RID: 1502
public class UpsideDown_Trait : BaseTrait
{
	// Token: 0x17001267 RID: 4711
	// (get) Token: 0x06002E57 RID: 11863 RVA: 0x00017DE0 File Offset: 0x00015FE0
	public override TraitType TraitType
	{
		get
		{
			return TraitType.UpsideDown;
		}
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x000C7D34 File Offset: 0x000C5F34
	public override void DisableOnDeath()
	{
		base.DisableOnDeath();
		PlayerController playerController = PlayerManager.GetPlayerController();
		Vector3 midpoint = playerController.Midpoint;
		float num = CameraController.GameCamera.transform.position.y - midpoint.y;
		midpoint.y += num * 2f;
		playerController.transform.position = midpoint;
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x000C7D8C File Offset: 0x000C5F8C
	public void ApplyDeathDefy(bool reset)
	{
		if (!reset)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			this.m_storedDeathDefyPos = playerController.transform.position;
			Vector3 midpoint = playerController.Midpoint;
			float num = CameraController.GameCamera.transform.position.y - midpoint.y;
			midpoint.y += num * 2f;
			playerController.transform.position = midpoint;
			return;
		}
		PlayerManager.GetPlayerController().transform.position = this.m_storedDeathDefyPos;
	}

	// Token: 0x0400260C RID: 9740
	private Vector3 m_storedDeathDefyPos;
}
