using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public interface IHitboxController
{
	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x06001386 RID: 4998
	GameObject gameObject { get; }

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x06001387 RID: 4999
	GameObject RootGameObject { get; }

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x06001388 RID: 5000
	bool IsInitialized { get; }

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x06001389 RID: 5001
	IDamageObj DamageObj { get; }

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x0600138A RID: 5002
	// (set) Token: 0x0600138B RID: 5003
	CollisionType CollisionType { get; set; }

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x0600138C RID: 5004
	CollisionType WeaponCollidesWithType { get; }

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x0600138D RID: 5005
	CollisionType TerrainCollidesWithType { get; }

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x0600138E RID: 5006
	// (set) Token: 0x0600138F RID: 5007
	float RepeatHitDuration { get; set; }

	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x06001390 RID: 5008
	// (set) Token: 0x06001391 RID: 5009
	bool DisableAllCollisions { get; set; }

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x06001392 RID: 5010
	// (set) Token: 0x06001393 RID: 5011
	Collider2D LastCollidedWith { get; set; }

	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x06001394 RID: 5012
	bool ResponseMethodsInitialized { get; }

	// Token: 0x06001395 RID: 5013
	void HandleCollision(HitResponseType hitResponseType, HitboxInfo hbInfo, Collider2D otherCollider);

	// Token: 0x06001396 RID: 5014
	void InvokeAllBodyHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController);

	// Token: 0x06001397 RID: 5015
	void InvokeAllWeaponHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController);

	// Token: 0x06001398 RID: 5016
	void InvokeAllTerrainHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController);

	// Token: 0x06001399 RID: 5017
	void AddHitboxResponseMethod(IHitResponse hitResponse);

	// Token: 0x0600139A RID: 5018
	void RemoveHitboxResponseMethod(IHitResponse hitResponse);

	// Token: 0x0600139B RID: 5019
	GameObject ContainsHitbox(HitboxType hitboxType, string hitboxName);

	// Token: 0x0600139C RID: 5020
	void SetHitboxActiveState(HitboxType hitboxType, bool active);

	// Token: 0x0600139D RID: 5021
	bool GetHitboxActiveState(HitboxType hitboxType);

	// Token: 0x0600139E RID: 5022
	Collider2D GetCollider(HitboxType hitboxType);

	// Token: 0x0600139F RID: 5023
	void ChangeCollisionType(HitboxType hitboxType, CollisionType collisionType);

	// Token: 0x060013A0 RID: 5024
	void ChangeCanCollideWith(HitboxType hitboxType, CollisionType collisionType);

	// Token: 0x060013A1 RID: 5025
	void RealignHitboxColliders();

	// Token: 0x060013A2 RID: 5026
	void Initialize();

	// Token: 0x060013A3 RID: 5027
	void ResetRepeatHitChecks();

	// Token: 0x060013A4 RID: 5028
	void RemoveFromRepeatHitChecks(GameObject obj);

	// Token: 0x060013A5 RID: 5029
	void ResetAllHitboxStates();

	// Token: 0x060013A6 RID: 5030
	void SetCulledState(bool cull, bool includeRigidbody);
}
