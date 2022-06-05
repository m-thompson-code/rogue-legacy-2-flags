using System;

namespace Foreground
{
	// Token: 0x02000DC7 RID: 3527
	public struct ForegroundID
	{
		// Token: 0x0600634C RID: 25420 RVA: 0x00036B2D File Offset: 0x00034D2D
		public ForegroundID(ForegroundLocation location, float zoomLevel)
		{
			this.Location = location;
			this.ZoomLevel = zoomLevel;
		}

		// Token: 0x17002009 RID: 8201
		// (get) Token: 0x0600634D RID: 25421 RVA: 0x00036B3D File Offset: 0x00034D3D
		public readonly ForegroundLocation Location { get; }

		// Token: 0x1700200A RID: 8202
		// (get) Token: 0x0600634E RID: 25422 RVA: 0x00036B45 File Offset: 0x00034D45
		public readonly float ZoomLevel { get; }
	}
}
