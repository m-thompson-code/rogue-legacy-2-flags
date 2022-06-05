using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public class IcePlatform : SpecialPlatform, ITerrainOnStayHitResponse, IHitResponse, ITerrainOnExitHitResponse
{
	// Token: 0x06002D59 RID: 11609 RVA: 0x00099850 File Offset: 0x00097A50
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

	// Token: 0x06002D5A RID: 11610 RVA: 0x0009985F File Offset: 0x00097A5F
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player"))
		{
			PlayerManager.GetPlayerController().DisableFriction = true;
		}
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x0009987E File Offset: 0x00097A7E
	public void TerrainOnExitHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player"))
		{
			PlayerManager.GetPlayerController().DisableFriction = false;
		}
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x0009989D File Offset: 0x00097A9D
	public override void SetState(StateID state)
	{
	}

	// Token: 0x0400245F RID: 9311
	protected BoxCollider2D m_terrainCollider;

	// Token: 0x04002460 RID: 9312
	protected BoxCollider2D m_platformCollider;
}
