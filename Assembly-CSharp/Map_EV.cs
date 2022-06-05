using System;

// Token: 0x02000071 RID: 113
public static class Map_EV
{
	// Token: 0x040003A2 RID: 930
	public const float PAN_SPEED = 14f;

	// Token: 0x040003A3 RID: 931
	public const float ZOOM_SPEED = 14f;

	// Token: 0x040003A4 RID: 932
	public const float MIN_ZOOM = 6f;

	// Token: 0x040003A5 RID: 933
	public const float MAX_ZOOM = 24f;

	// Token: 0x040003A6 RID: 934
	public const float ROOM_SCALE = 0.06666667f;

	// Token: 0x040003A7 RID: 935
	public const bool RECAP_ZOOM_OUT_BEFORE_PANNING = false;

	// Token: 0x040003A8 RID: 936
	public const float RECAP_PAN_TWEEN_TIME = 0.5f;

	// Token: 0x040003A9 RID: 937
	public const float RECAP_ZOOM_TWEEN_TIME = 0.5f;

	// Token: 0x040003AA RID: 938
	public const float RECAP_INITIAL_ZOOM = 3f;

	// Token: 0x040003AB RID: 939
	public const float RECAP_MAX_ZOOM = 6f;

	// Token: 0x040003AC RID: 940
	public const float RECAP_MAX_FREE_ZOOM = 12f;

	// Token: 0x040003AD RID: 941
	public static BiomeType[] BIOME_DISPLAY_ORDER = new BiomeType[]
	{
		BiomeType.Castle,
		BiomeType.Stone,
		BiomeType.Forest,
		BiomeType.Study,
		BiomeType.Tower,
		BiomeType.Cave
	};
}
