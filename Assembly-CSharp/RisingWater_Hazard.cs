using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class RisingWater_Hazard : Hazard, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x060028F8 RID: 10488 RVA: 0x0008771F File Offset: 0x0008591F
	public override float BaseDamage
	{
		get
		{
			return 999999f;
		}
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x00087726 File Offset: 0x00085926
	public override void SetIsCulled(bool culled)
	{
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x00087728 File Offset: 0x00085928
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		base.StartCoroutine(this.RiseWaterCoroutine());
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x00087740 File Offset: 0x00085940
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") || otherHBController.RootGameObject.CompareTag("Player_Dodging"))
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.StartHitResponse(base.gameObject, this, 999999f, true, true);
		}
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x0008778E File Offset: 0x0008598E
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

	// Token: 0x060028FD RID: 10493 RVA: 0x0008779D File Offset: 0x0008599D
	public override void ResetHazard()
	{
	}
}
