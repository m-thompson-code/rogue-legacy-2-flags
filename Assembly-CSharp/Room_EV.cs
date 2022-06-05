using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class Room_EV : MonoBehaviour
{
	// Token: 0x040004C4 RID: 1220
	public const float MAX_CAMERA_ZOOM_DEFAULT = 1f;

	// Token: 0x040004C5 RID: 1221
	public const int ROOM_UNIT_WIDTH = 32;

	// Token: 0x040004C6 RID: 1222
	public const int ROOM_UNIT_HEIGHT = 18;

	// Token: 0x040004C7 RID: 1223
	public const float WALL_WIDTH = 1f;

	// Token: 0x040004C8 RID: 1224
	public const int UPPER_LOWER_DOOR_UNIT_SIZE = 6;

	// Token: 0x040004C9 RID: 1225
	public const int SIDE_DOOR_UNIT_SIZE = 5;

	// Token: 0x040004CA RID: 1226
	public static float TRANSITION_LEFT_RIGHT_DISTANCE = 0.3f;

	// Token: 0x040004CB RID: 1227
	public static float TRANSITION_TOP_DISTANCE = -0.9f;

	// Token: 0x040004CC RID: 1228
	public static float TRANSITION_BOTTOM_DISTANCE = -1.15f;

	// Token: 0x040004CD RID: 1229
	public static float TRANSITION_PLAYER_POSITION_LEFT_RIGHT_X_OFFSET = -0.6f;

	// Token: 0x040004CE RID: 1230
	public static float TRANSITION_PLAYER_POSITION_BOTTOM_Y_OFFSET = -1f;

	// Token: 0x040004CF RID: 1231
	public static float TRANSITION_PLAYER_POSITION_TOP_Y_OFFSET = -0.25f;

	// Token: 0x040004D0 RID: 1232
	public static float TRANSITION_MINIMUM_SPEED_ON_ENTER_BOTTOM = 19.5f;

	// Token: 0x040004D1 RID: 1233
	public static float DISABLE_BRAKING_FORCE_TIME = 0.125f;
}
