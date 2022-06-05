using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
public class IcePlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnExitHitResponse
{
	// Token: 0x06003E2F RID: 15919 RVA: 0x00022678 File Offset: 0x00020878
	protected override IEnumerator Start()
	{
		yield return base.Start();
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_terrainCollider = (this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D);
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		if (base.Width > 0f)
		{
			SpriteRenderer componentInChildren = base.GetComponentInChildren<SpriteRenderer>();
			componentInChildren.size = new Vector2(base.Width, componentInChildren.size.y);
			this.m_terrainCollider.size = new Vector2(base.Width, this.m_terrainCollider.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
		}
		yield break;
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x00022687 File Offset: 0x00020887
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player"))
		{
			PlayerManager.GetPlayerController().DisableFriction = true;
		}
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x000226A6 File Offset: 0x000208A6
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player"))
		{
			PlayerManager.GetPlayerController().DisableFriction = false;
		}
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x040030DD RID: 12509
	protected BoxCollider2D m_terrainCollider;

	// Token: 0x040030DE RID: 12510
	protected BoxCollider2D m_platformCollider;
}
