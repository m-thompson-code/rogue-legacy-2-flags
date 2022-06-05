using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class Room_EV : MonoBehaviour
{
	// Token: 0x040004E5 RID: 1253
	public const float MAX_CAMERA_ZOOM_DEFAULT = 1f;

	// Token: 0x040004E6 RID: 1254
	public const int ROOM_UNIT_WIDTH = 32;

	// Token: 0x040004E7 RID: 1255
	public const int ROOM_UNIT_HEIGHT = 18;

	// Token: 0x040004E8 RID: 1256
	public const float WALL_WIDTH = 1f;

	// Token: 0x040004E9 RID: 1257
	public const int UPPER_LOWER_DOOR_UNIT_SIZE = 6;

	// Token: 0x040004EA RID: 1258
	public const int SIDE_DOOR_UNIT_SIZE = 5;

	// Token: 0x040004EB RID: 1259
	public static float TRANSITION_LEFT_RIGHT_DISTANCE = 0.3f;

	// Token: 0x040004EC RID: 1260
	public static float TRANSITION_TOP_DISTANCE = -0.9f;

	// Token: 0x040004ED RID: 1261
	public static float TRANSITION_BOTTOM_DISTANCE = -1.15f;

	// Token: 0x040004EE RID: 1262
	public static float TRANSITION_PLAYER_POSITION_LEFT_RIGHT_X_OFFSET = -0.6f;

	// Token: 0x040004EF RID: 1263
	public static float TRANSITION_PLAYER_POSITION_BOTTOM_Y_OFFSET = -1f;

	// Token: 0x040004F0 RID: 1264
	public static float TRANSITION_PLAYER_POSITION_TOP_Y_OFFSET = -0.25f;

	// Token: 0x040004F1 RID: 1265
	public static float TRANSITION_MINIMUM_SPEED_ON_ENTER_BOTTOM = 19.5f;

	// Token: 0x040004F2 RID: 1266
	public static float DISABLE_BRAKING_FORCE_TIME = 0.125f;
}
