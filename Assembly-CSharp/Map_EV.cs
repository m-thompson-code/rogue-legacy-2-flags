using System;

// Token: 0x02000079 RID: 121
public static class Map_EV
{
	// Token: 0x040003C3 RID: 963
	public const float PAN_SPEED = 14f;

	// Token: 0x040003C4 RID: 964
	public const float ZOOM_SPEED = 14f;

	// Token: 0x040003C5 RID: 965
	public const float MIN_ZOOM = 6f;

	// Token: 0x040003C6 RID: 966
	public const float MAX_ZOOM = 24f;

	// Token: 0x040003C7 RID: 967
	public const float ROOM_SCALE = 0.06666667f;

	// Token: 0x040003C8 RID: 968
	public const bool RECAP_ZOOM_OUT_BEFORE_PANNING = false;

	// Token: 0x040003C9 RID: 969
	public const float RECAP_PAN_TWEEN_TIME = 0.5f;

	// Token: 0x040003CA RID: 970
	public const float RECAP_ZOOM_TWEEN_TIME = 0.5f;

	// Token: 0x040003CB RID: 971
	public const float RECAP_INITIAL_ZOOM = 3f;

	// Token: 0x040003CC RID: 972
	public const float RECAP_MAX_ZOOM = 6f;

	// Token: 0x040003CD RID: 973
	public const float RECAP_MAX_FREE_ZOOM = 12f;

	// Token: 0x040003CE RID: 974
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
