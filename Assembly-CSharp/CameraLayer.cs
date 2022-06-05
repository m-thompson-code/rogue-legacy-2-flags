using System;

// Token: 0x0200070D RID: 1805
public enum CameraLayer
{
	// Token: 0x040032F3 RID: 13043
	None,
	// Token: 0x040032F4 RID: 13044
	Game,
	// Token: 0x040032F5 RID: 13045
	Foreground_PERSP,
	// Token: 0x040032F6 RID: 13046
	Foreground_ORTHO = 16,
	// Token: 0x040032F7 RID: 13047
	Background_ORTHO = 32,
	// Token: 0x040032F8 RID: 13048
	Background_Near_PERSP = 64,
	// Token: 0x040032F9 RID: 13049
	Background_Wall = 128,
	// Token: 0x040032FA RID: 13050
	Background_Far_PERSP = 4,
	// Token: 0x040032FB RID: 13051
	ForegroundLights = 8,
	// Token: 0x040032FC RID: 13052
	Any = -1
}
