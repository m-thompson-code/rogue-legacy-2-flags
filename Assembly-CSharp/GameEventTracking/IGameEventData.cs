using System;

namespace GameEventTracking
{
	// Token: 0x020008A0 RID: 2208
	public interface IGameEventData
	{
		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x0600481F RID: 18463
		float TimeStamp { get; }

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06004820 RID: 18464
		int TimesLoaded { get; }
	}
}
