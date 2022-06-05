using System;
using System.Text;
using Rewired.UI;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000EBF RID: 3775
	public class PlayerPointerEventData : PointerEventData
	{
		// Token: 0x170023BA RID: 9146
		// (get) Token: 0x06006CC0 RID: 27840 RVA: 0x0003B8BD File Offset: 0x00039ABD
		// (set) Token: 0x06006CC1 RID: 27841 RVA: 0x0003B8C5 File Offset: 0x00039AC5
		public int playerId { get; set; }

		// Token: 0x170023BB RID: 9147
		// (get) Token: 0x06006CC2 RID: 27842 RVA: 0x0003B8CE File Offset: 0x00039ACE
		// (set) Token: 0x06006CC3 RID: 27843 RVA: 0x0003B8D6 File Offset: 0x00039AD6
		public int inputSourceIndex { get; set; }

		// Token: 0x170023BC RID: 9148
		// (get) Token: 0x06006CC4 RID: 27844 RVA: 0x0003B8DF File Offset: 0x00039ADF
		// (set) Token: 0x06006CC5 RID: 27845 RVA: 0x0003B8E7 File Offset: 0x00039AE7
		public IMouseInputSource mouseSource { get; set; }

		// Token: 0x170023BD RID: 9149
		// (get) Token: 0x06006CC6 RID: 27846 RVA: 0x0003B8F0 File Offset: 0x00039AF0
		// (set) Token: 0x06006CC7 RID: 27847 RVA: 0x0003B8F8 File Offset: 0x00039AF8
		public ITouchInputSource touchSource { get; set; }

		// Token: 0x170023BE RID: 9150
		// (get) Token: 0x06006CC8 RID: 27848 RVA: 0x0003B901 File Offset: 0x00039B01
		// (set) Token: 0x06006CC9 RID: 27849 RVA: 0x0003B909 File Offset: 0x00039B09
		public PointerEventType sourceType { get; set; }

		// Token: 0x170023BF RID: 9151
		// (get) Token: 0x06006CCA RID: 27850 RVA: 0x0003B912 File Offset: 0x00039B12
		// (set) Token: 0x06006CCB RID: 27851 RVA: 0x0003B91A File Offset: 0x00039B1A
		public int buttonIndex { get; set; }

		// Token: 0x06006CCC RID: 27852 RVA: 0x0003B923 File Offset: 0x00039B23
		public PlayerPointerEventData(EventSystem eventSystem) : base(eventSystem)
		{
			this.playerId = -1;
			this.inputSourceIndex = -1;
			this.buttonIndex = -1;
		}

		// Token: 0x06006CCD RID: 27853 RVA: 0x0018471C File Offset: 0x0018291C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Player Id</b>: " + this.playerId.ToString());
			string str = "<b>Mouse Source</b>: ";
			IMouseInputSource mouseSource = this.mouseSource;
			stringBuilder.AppendLine(str + ((mouseSource != null) ? mouseSource.ToString() : null));
			stringBuilder.AppendLine("<b>Input Source Index</b>: " + this.inputSourceIndex.ToString());
			string str2 = "<b>Touch Source/b>: ";
			ITouchInputSource touchSource = this.touchSource;
			stringBuilder.AppendLine(str2 + ((touchSource != null) ? touchSource.ToString() : null));
			stringBuilder.AppendLine("<b>Source Type</b>: " + this.sourceType.ToString());
			stringBuilder.AppendLine("<b>Button Index</b>: " + this.buttonIndex.ToString());
			stringBuilder.Append(base.ToString());
			return stringBuilder.ToString();
		}
	}
}
