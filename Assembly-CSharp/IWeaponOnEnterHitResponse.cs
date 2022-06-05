using System;

// Token: 0x020009EE RID: 2542
public interface IWeaponOnEnterHitResponse : IHitResponse
{
	// Token: 0x06004CC7 RID: 19655
	void WeaponOnEnterHitResponse(IHitboxController otherHBController);
}
