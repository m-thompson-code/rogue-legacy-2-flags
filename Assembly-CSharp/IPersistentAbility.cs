using System;

// Token: 0x0200059E RID: 1438
public interface IPersistentAbility
{
	// Token: 0x170012FF RID: 4863
	// (get) Token: 0x0600360D RID: 13837
	bool IsPersistentActive { get; }

	// Token: 0x0600360E RID: 13838
	void StopPersistentAbility();
}
