using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public class EasyBreakables_Trait : BaseTrait
{
	// Token: 0x170011F9 RID: 4601
	// (get) Token: 0x06002CEB RID: 11499 RVA: 0x00017640 File Offset: 0x00015840
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EasyBreakables;
		}
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x00018D7E File Offset: 0x00016F7E
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

	// Token: 0x06002CED RID: 11501 RVA: 0x000C666C File Offset: 0x000C486C
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

	// Token: 0x06002CEE RID: 11502 RVA: 0x00018D8D File Offset: 0x00016F8D
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_breakablesHitbox);
	}

	// Token: 0x04002595 RID: 9621
	private GameObject m_breakablesHitbox;
}
