using System;

// Token: 0x02000BC0 RID: 3008
public enum CameraLayer
{
	// Token: 0x0400456E RID: 17774
	None,
	// Token: 0x0400456F RID: 17775
	Game,
	// Token: 0x04004570 RID: 17776
	Foreground_PERSP,
	// Token: 0x04004571 RID: 17777
	Foreground_ORTHO = 16,
	// Token: 0x04004572 RID: 17778
	Background_ORTHO = 32,
	// Token: 0x04004573 RID: 17779
	Background_Near_PERSP = 64,
	// Token: 0x04004574 RID: 17780
	Background_Wall = 128,
	// Token: 0x04004575 RID: 17781
	Background_Far_PERSP = 4,
	// Token: 0x04004576 RID: 17782
	ForegroundLights = 8,
	// Token: 0x04004577 RID: 17783
	Any = -1
}
