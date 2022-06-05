using System;

// Token: 0x020005E7 RID: 1511
public interface IWeaponOnStayHitResponse : IHitResponse
{
	// Token: 0x060036B6 RID: 14006
	void WeaponOnStayHitResponse(IHitboxController otherHBController);
}
