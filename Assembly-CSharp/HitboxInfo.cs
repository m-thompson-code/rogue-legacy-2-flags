using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
[Serializable]
public class HitboxInfo : MonoBehaviour
{
	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x06001379 RID: 4985 RVA: 0x0003B794 File Offset: 0x00039994
	// (set) Token: 0x0600137A RID: 4986 RVA: 0x0003B79C File Offset: 0x0003999C
	public Collider2D Collider { get; private set; }

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x0600137B RID: 4987 RVA: 0x0003B7A5 File Offset: 0x000399A5
	// (set) Token: 0x0600137C RID: 4988 RVA: 0x0003B7AD File Offset: 0x000399AD
	public CollisionType CollidesWithType { get; set; }

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x0600137D RID: 4989 RVA: 0x0003B7B6 File Offset: 0x000399B6
	// (set) Token: 0x0600137E RID: 4990 RVA: 0x0003B7BE File Offset: 0x000399BE
	public IHitboxController HitboxController { get; set; }

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x0600137F RID: 4991 RVA: 0x0003B7C7 File Offset: 0x000399C7
	public GameObject RootGameObj
	{
		get
		{
			return this.HitboxController.RootGameObject;
		}
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x0003B7D4 File Offset: 0x000399D4
	public void SetCollider(Collider2D collider)
	{
		this.Collider = collider;
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0003B7DD File Offset: 0x000399DD
	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		this.OnTriggerHandler(HitResponseType.OnEnter, otherCollider);
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0003B7E7 File Offset: 0x000399E7
	private void OnTriggerStay2D(Collider2D otherCollider)
	{
		this.OnTriggerHandler(HitResponseType.OnStay, otherCollider);
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0003B7F1 File Offset: 0x000399F1
	private void OnTriggerExit2D(Collider2D otherCollider)
	{
		this.OnTriggerHandler(HitResponseType.OnExit, otherCollider);
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x0003B7FC File Offset: 0x000399FC
	private void OnTriggerHandler(HitResponseType hitResponseType, Collider2D otherCollider)
	{
		IHitboxController hitboxController = this.HitboxController;
		if (hitboxController != null)
		{
			if (hitboxController.DisableAllCollisions)
			{
				return;
			}
			if (!CDGHelper.DoCollisionTypesCollide_V2(this.CollidesWithType, otherCollider.gameObject))
			{
				return;
			}
			if (otherCollider.CompareTag("Generic_Bounceable") && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) <= 0)
			{
				return;
			}
			hitboxController.HandleCollision(hitResponseType, this, otherCollider);
		}
	}
}
