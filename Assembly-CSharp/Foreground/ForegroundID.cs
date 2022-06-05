using System;

namespace Foreground
{
	// Token: 0x0200089A RID: 2202
	public struct ForegroundID
	{
		// Token: 0x06004811 RID: 18449 RVA: 0x001037AB File Offset: 0x001019AB
		public ForegroundID(ForegroundLocation location, float zoomLevel)
		{
			this.Location = location;
			this.ZoomLevel = zoomLevel;
		}

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x06004812 RID: 18450 RVA: 0x001037BB File Offset: 0x001019BB
		public readonly ForegroundLocation Location { get; }

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x06004813 RID: 18451 RVA: 0x001037C3 File Offset: 0x001019C3
		public readonly float ZoomLevel { get; }
	}
}
