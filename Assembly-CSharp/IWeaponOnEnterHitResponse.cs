using System;

// Token: 0x020005E6 RID: 1510
public interface IWeaponOnEnterHitResponse : IHitResponse
{
	// Token: 0x060036B5 RID: 14005
	void WeaponOnEnterHitResponse(IHitboxController otherHBController);
}
