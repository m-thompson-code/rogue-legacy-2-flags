using System;
using System.Text;
using Rewired.UI;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000935 RID: 2357
	public class PlayerPointerEventData : PointerEventData
	{
		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x06004F56 RID: 20310 RVA: 0x00115697 File Offset: 0x00113897
		// (set) Token: 0x06004F57 RID: 20311 RVA: 0x0011569F File Offset: 0x0011389F
		public int playerId { get; set; }

		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x06004F58 RID: 20312 RVA: 0x001156A8 File Offset: 0x001138A8
		// (set) Token: 0x06004F59 RID: 20313 RVA: 0x001156B0 File Offset: 0x001138B0
		public int inputSourceIndex { get; set; }

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x06004F5A RID: 20314 RVA: 0x001156B9 File Offset: 0x001138B9
		// (set) Token: 0x06004F5B RID: 20315 RVA: 0x001156C1 File Offset: 0x001138C1
		public IMouseInputSource mouseSource { get; set; }

		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x06004F5C RID: 20316 RVA: 0x001156CA File Offset: 0x001138CA
		// (set) Token: 0x06004F5D RID: 20317 RVA: 0x001156D2 File Offset: 0x001138D2
		public ITouchInputSource touchSource { get; set; }

		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x06004F5E RID: 20318 RVA: 0x001156DB File Offset: 0x001138DB
		// (set) Token: 0x06004F5F RID: 20319 RVA: 0x001156E3 File Offset: 0x001138E3
		public PointerEventType sourceType { get; set; }

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x06004F60 RID: 20320 RVA: 0x001156EC File Offset: 0x001138EC
		// (set) Token: 0x06004F61 RID: 20321 RVA: 0x001156F4 File Offset: 0x001138F4
		public int buttonIndex { get; set; }

		// Token: 0x06004F62 RID: 20322 RVA: 0x001156FD File Offset: 0x001138FD
		public PlayerPointerEventData(EventSystem eventSystem) : base(eventSystem)
		{
			this.playerId = -1;
			this.inputSourceIndex = -1;
			this.buttonIndex = -1;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x0011571C File Offset: 0x0011391C
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
