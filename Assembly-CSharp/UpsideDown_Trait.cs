using System;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class UpsideDown_Trait : BaseTrait
{
	// Token: 0x17000DF8 RID: 3576
	// (get) Token: 0x060020B2 RID: 8370 RVA: 0x00066E45 File Offset: 0x00065045
	public override TraitType TraitType
	{
		get
		{
			return TraitType.UpsideDown;
		}
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x00066E4C File Offset: 0x0006504C
	public override void DisableOnDeath()
	{
		base.DisableOnDeath();
		PlayerController playerController = PlayerManager.GetPlayerController();
		Vector3 midpoint = playerController.Midpoint;
		float num = CameraController.GameCamera.transform.position.y - midpoint.y;
		midpoint.y += num * 2f;
		playerController.transform.position = midpoint;
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x00066EA4 File Offset: 0x000650A4
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

	// Token: 0x04001C6A RID: 7274
	private Vector3 m_storedDeathDefyPos;
}
