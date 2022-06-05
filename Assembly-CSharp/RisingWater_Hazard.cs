using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200073B RID: 1851
public class RisingWater_Hazard : Hazard, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17001526 RID: 5414
	// (get) Token: 0x060038A2 RID: 14498 RVA: 0x00004ACB File Offset: 0x00002CCB
	public override float BaseDamage
	{
		get
		{
			return 999999f;
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetIsCulled(bool culled)
	{
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x0001F1B5 File Offset: 0x0001D3B5
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		base.StartCoroutine(this.RiseWaterCoroutine());
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x000E8C8C File Offset: 0x000E6E8C
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") || otherHBController.RootGameObject.CompareTag("Player_Dodging"))
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.StartHitResponse(base.gameObject, this, 999999f, true, true);
		}
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x0001F1CB File Offset: 0x0001D3CB
	private IEnumerator RiseWaterCoroutine()
	{
		float delay = 3f + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		for (;;)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.y += 5f * Time.deltaTime;
			base.transform.localPosition = localPosition;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void ResetHazard()
	{
	}
}
