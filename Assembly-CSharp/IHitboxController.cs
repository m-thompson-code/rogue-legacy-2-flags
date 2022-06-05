using System;
using UnityEngine;

// Token: 0x0200035A RID: 858
public interface IHitboxController
{
	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06001C1F RID: 7199
	GameObject gameObject { get; }

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06001C20 RID: 7200
	GameObject RootGameObject { get; }

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06001C21 RID: 7201
	bool IsInitialized { get; }

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x06001C22 RID: 7202
	IDamageObj DamageObj { get; }

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x06001C23 RID: 7203
	// (set) Token: 0x06001C24 RID: 7204
	CollisionType CollisionType { get; set; }

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x06001C25 RID: 7205
	CollisionType WeaponCollidesWithType { get; }

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x06001C26 RID: 7206
	CollisionType TerrainCollidesWithType { get; }

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x06001C27 RID: 7207
	// (set) Token: 0x06001C28 RID: 7208
	float RepeatHitDuration { get; set; }

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x06001C29 RID: 7209
	// (set) Token: 0x06001C2A RID: 7210
	bool DisableAllCollisions { get; set; }

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x06001C2B RID: 7211
	// (set) Token: 0x06001C2C RID: 7212
	Collider2D LastCollidedWith { get; set; }

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x06001C2D RID: 7213
	bool ResponseMethodsInitialized { get; }

	// Token: 0x06001C2E RID: 7214
	void HandleCollision(HitResponseType hitResponseType, HitboxInfo hbInfo, Collider2D otherCollider);

	// Token: 0x06001C2F RID: 7215
	void InvokeAllBodyHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController);

	// Token: 0x06001C30 RID: 7216
	void InvokeAllWeaponHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController);

	// Token: 0x06001C31 RID: 7217
	void InvokeAllTerrainHitResponseMethods(HitResponseType hitResponseType, IHitboxController otherHBController);

	// Token: 0x06001C32 RID: 7218
	void AddHitboxResponseMethod(IHitResponse hitResponse);

	// Token: 0x06001C33 RID: 7219
	void RemoveHitboxResponseMethod(IHitResponse hitResponse);

	// Token: 0x06001C34 RID: 7220
	GameObject ContainsHitbox(HitboxType hitboxType, string hitboxName);

	// Token: 0x06001C35 RID: 7221
	void SetHitboxActiveState(HitboxType hitboxType, bool active);

	// Token: 0x06001C36 RID: 7222
	bool GetHitboxActiveState(HitboxType hitboxType);

	// Token: 0x06001C37 RID: 7223
	Collider2D GetCollider(HitboxType hitboxType);

	// Token: 0x06001C38 RID: 7224
	void ChangeCollisionType(HitboxType hitboxType, CollisionType collisionType);

	// Token: 0x06001C39 RID: 7225
	void ChangeCanCollideWith(HitboxType hitboxType, CollisionType collisionType);

	// Token: 0x06001C3A RID: 7226
	void RealignHitboxColliders();

	// Token: 0x06001C3B RID: 7227
	void Initialize();

	// Token: 0x06001C3C RID: 7228
	void ResetRepeatHitChecks();

	// Token: 0x06001C3D RID: 7229
	void RemoveFromRepeatHitChecks(GameObject obj);

	// Token: 0x06001C3E RID: 7230
	void ResetAllHitboxStates();

	// Token: 0x06001C3F RID: 7231
	void SetCulledState(bool cull, bool includeRigidbody);
}
