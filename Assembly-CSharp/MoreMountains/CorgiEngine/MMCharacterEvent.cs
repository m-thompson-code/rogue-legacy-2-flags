using System;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F18 RID: 3864
	public struct MMCharacterEvent
	{
		// Token: 0x06006F95 RID: 28565 RVA: 0x0003D86C File Offset: 0x0003BA6C
		public MMCharacterEvent(Character character, MMCharacterEventTypes eventType)
		{
			this.TargetCharacter = character;
			this.EventType = eventType;
		}

		// Token: 0x040059B7 RID: 22967
		public Character TargetCharacter;

		// Token: 0x040059B8 RID: 22968
		public MMCharacterEventTypes EventType;
	}
}
