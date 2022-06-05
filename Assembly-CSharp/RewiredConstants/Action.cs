using System;
using Rewired.Dev;

namespace RewiredConstants
{
	// Token: 0x02000D9A RID: 3482
	public static class Action
	{
		// Token: 0x0400506F RID: 20591
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Attack")]
		public const int Attack = 1;

		// Token: 0x04005070 RID: 20592
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Jump")]
		public const int Jump = 2;

		// Token: 0x04005071 RID: 20593
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "MoveHorizontal")]
		public const int MoveHorizontal = 3;

		// Token: 0x04005072 RID: 20594
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "MoveVertical")]
		public const int MoveVertical = 8;

		// Token: 0x04005073 RID: 20595
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Dash")]
		public const int Dash = 4;

		// Token: 0x04005074 RID: 20596
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Spell")]
		public const int Spell = 5;

		// Token: 0x04005075 RID: 20597
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Talent")]
		public const int Talent = 16;

		// Token: 0x04005076 RID: 20598
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "DownStrike")]
		public const int DownStrike = 6;

		// Token: 0x04005077 RID: 20599
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "Interact")]
		public const int Interact = 10;

		// Token: 0x04005078 RID: 20600
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "MoveLeft")]
		public const int MoveLeft = 12;

		// Token: 0x04005079 RID: 20601
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "MoveRight")]
		public const int MoveRight = 13;

		// Token: 0x0400507A RID: 20602
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "MoveUp")]
		public const int MoveUp = 14;

		// Token: 0x0400507B RID: 20603
		[ActionIdFieldInfo(categoryName = "PlayerActions", friendlyName = "MoveDown")]
		public const int MoveDown = 15;

		// Token: 0x0400507C RID: 20604
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "Confirm")]
		public const int Confirm = 23;

		// Token: 0x0400507D RID: 20605
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "Cancel")]
		public const int Cancel = 24;

		// Token: 0x0400507E RID: 20606
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "MenuLeft")]
		public const int MenuLeft = 25;

		// Token: 0x0400507F RID: 20607
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "MenuRight")]
		public const int MenuRight = 26;

		// Token: 0x04005080 RID: 20608
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "MenuUp")]
		public const int MenuUp = 27;

		// Token: 0x04005081 RID: 20609
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "MenuDown")]
		public const int MenuDown = 28;

		// Token: 0x04005082 RID: 20610
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "Start")]
		public const int Start = 29;

		// Token: 0x04005083 RID: 20611
		[ActionIdFieldInfo(categoryName = "Menu", friendlyName = "Select")]
		public const int Select = 30;

		// Token: 0x04005084 RID: 20612
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "Action0")]
		public const int UIHorizontal = 33;

		// Token: 0x04005085 RID: 20613
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIVertical")]
		public const int UIVertical = 34;

		// Token: 0x04005086 RID: 20614
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UICancel")]
		public const int UISubmit = 35;

		// Token: 0x04005087 RID: 20615
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UICancel")]
		public const int UICancel = 36;

		// Token: 0x04005088 RID: 20616
		[ActionIdFieldInfo(categoryName = "UI", friendlyName = "UIUpgrade")]
		public const int UIUpgrade = 37;

		// Token: 0x04005089 RID: 20617
		[ActionIdFieldInfo(categoryName = "Pause Window", friendlyName = "Pause Window Start")]
		public const int Pause_Window_Start = 38;

		// Token: 0x0400508A RID: 20618
		[ActionIdFieldInfo(categoryName = "Pause Window", friendlyName = "Pause Window Select")]
		public const int Pause_Window_Select = 39;
	}
}
