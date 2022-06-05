using System;
using UnityEngine;

// Token: 0x02000359 RID: 857
[Serializable]
public class HitboxInfo : MonoBehaviour
{
	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06001C12 RID: 7186 RVA: 0x0000E94E File Offset: 0x0000CB4E
	// (set) Token: 0x06001C13 RID: 7187 RVA: 0x0000E956 File Offset: 0x0000CB56
	public Collider2D Collider { get; private set; }

	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x06001C14 RID: 7188 RVA: 0x0000E95F File Offset: 0x0000CB5F
	// (set) Token: 0x06001C15 RID: 7189 RVA: 0x0000E967 File Offset: 0x0000CB67
	public CollisionType CollidesWithType { get; set; }

	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0000E970 File Offset: 0x0000CB70
	// (set) Token: 0x06001C17 RID: 7191 RVA: 0x0000E978 File Offset: 0x0000CB78
	public IHitboxController HitboxController { get; set; }

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0000E981 File Offset: 0x0000CB81
	public GameObject RootGameObj
	{
		get
		{
			return this.HitboxController.RootGameObject;
		}
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x0000E98E File Offset: 0x0000CB8E
	public void SetCollider(Collider2D collider)
	{
		this.Collider = collider;
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x0000E997 File Offset: 0x0000CB97
	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		this.OnTriggerHandler(HitResponseType.OnEnter, otherCollider);
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x0000E9A1 File Offset: 0x0000CBA1
	private void OnTriggerStay2D(Collider2D otherCollider)
	{
		this.OnTriggerHandler(HitResponseType.OnStay, otherCollider);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x0000E9AB File Offset: 0x0000CBAB
	private void OnTriggerExit2D(Collider2D otherCollider)
	{
		this.OnTriggerHandler(HitResponseType.OnExit, otherCollider);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00098868 File Offset: 0x00096A68
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
