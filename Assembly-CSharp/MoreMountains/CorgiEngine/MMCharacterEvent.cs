using System;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x0200096A RID: 2410
	public struct MMCharacterEvent
	{
		// Token: 0x06005194 RID: 20884 RVA: 0x00120144 File Offset: 0x0011E344
		public MMCharacterEvent(Character character, MMCharacterEventTypes eventType)
		{
			this.TargetCharacter = character;
			this.EventType = eventType;
		}

		// Token: 0x0400439A RID: 17306
		public Character TargetCharacter;

		// Token: 0x0400439B RID: 17307
		public MMCharacterEventTypes EventType;
	}
}
