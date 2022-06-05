using System;

namespace GameEventTracking
{
	// Token: 0x02000DCD RID: 3533
	public interface IGameEventData
	{
		// Token: 0x1700200F RID: 8207
		// (get) Token: 0x0600635A RID: 25434
		float TimeStamp { get; }

		// Token: 0x17002010 RID: 8208
		// (get) Token: 0x0600635B RID: 25435
		int TimesLoaded { get; }
	}
}
