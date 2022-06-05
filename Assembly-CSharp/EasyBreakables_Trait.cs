using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class EasyBreakables_Trait : BaseTrait
{
	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06001FEA RID: 8170 RVA: 0x00065C40 File Offset: 0x00063E40
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EasyBreakables;
		}
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x00065C44 File Offset: 0x00063E44
	public IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		IHitboxController hbController = PlayerManager.GetPlayerController().HitboxController;
		if (!hbController.IsInitialized)
		{
			yield return new WaitUntil(() => hbController.IsInitialized);
		}
		Collider2D collider = hbController.GetCollider(HitboxType.Terrain);
		this.AddBreakablesHitbox(collider, hbController as HitboxController);
		yield break;
	}

	// Token: 0x06001FEC RID: 8172 RVA: 0x00065C54 File Offset: 0x00063E54
	private void AddBreakablesHitbox(Collider2D hbCollider, HitboxController hbController)
	{
		CollisionType collisionType = CollisionType.None;
		collisionType |= CollisionType.FlimsyBreakable;
		collisionType |= CollisionType.Breakable;
		LayerType value = LayerType.Weapon_Hitbox;
		GameObject gameObject = new GameObject();
		Collider2D collider = hbController.GetCollider(HitboxType.Body);
		gameObject.transform.SetParent(collider.transform.parent, false);
		BoxCollider2D boxCollider2D = (BoxCollider2D)CDGHelper.CopyComponent<Collider2D>(hbCollider, gameObject, true);
		Vector2 size = boxCollider2D.size;
		size.x += 1f;
		boxCollider2D.size = size;
		HitboxInfo hitboxInfo = gameObject.AddComponent<HitboxInfo>();
		hitboxInfo.HitboxController = hbController;
		hitboxInfo.CollidesWithType = collisionType;
		TagType equivalentTag = CollisionType_RL.GetEquivalentTag(hbController.CollisionType);
		hitboxInfo.tag = TagType_RL.ToString(equivalentTag);
		hitboxInfo.SetCollider(boxCollider2D);
		gameObject.layer = LayerMask.NameToLayer(LayerType_RL.ToString(value));
		gameObject.name = hbCollider.name + " - (EasyBreakable Trait)";
		Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
		rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
		rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
		this.m_breakablesHitbox = gameObject;
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x00065D3F File Offset: 0x00063F3F
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_breakablesHitbox);
	}

	// Token: 0x04001C44 RID: 7236
	private GameObject m_breakablesHitbox;
}
