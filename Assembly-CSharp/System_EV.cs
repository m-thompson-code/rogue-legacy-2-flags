using System;

// Token: 0x02000088 RID: 136
public static class System_EV
{
	// Token: 0x060001E4 RID: 484 RVA: 0x00003BDF File Offset: 0x00001DDF
	public static string GetVersionString()
	{
		return string.Format("v{0}.{1}.{2}{3}-{4}", new object[]
		{
			1,
			0,
			2,
			"c",
			"steam"
		});
	}

	// Token: 0x0400058A RID: 1418
	public const float FIXED_DELTA_TIME = 0.016666668f;

	// Token: 0x0400058B RID: 1419
	public const float MAXIMUM_DELTA_TIME = 0.033333335f;

	// Token: 0x0400058C RID: 1420
	public const int DEFAULT_TARGET_FRAME_RATE = 120;

	// Token: 0x0400058D RID: 1421
	public static readonly int[] MAX_FPS_OPTIONS = new int[]
	{
		60,
		120,
		144,
		240
	};

	// Token: 0x0400058E RID: 1422
	public const byte NUM_SAVE_BACKUPS = 5;

	// Token: 0x0400058F RID: 1423
	public const float SAVE_RESTRICTION_DURATION = 10f;

	// Token: 0x04000590 RID: 1424
	public const int NUM_PROFILES = 5;

	// Token: 0x04000591 RID: 1425
	public const int MIN_NUMBER_VERTICAL_RAYS = 2;

	// Token: 0x04000592 RID: 1426
	public const int MAX_NUMBER_VERTICAL_RAYS = 6;

	// Token: 0x04000593 RID: 1427
	public const int MIN_NUMBER_HORIZONTAL_RAYS = 2;

	// Token: 0x04000594 RID: 1428
	public const int MAX_NUMBER_HORIZONTAL_RAYS = 6;

	// Token: 0x04000595 RID: 1429
	public const float DISTANCE_BETWEEN_VERTICAL_RAYS = 0.5f;

	// Token: 0x04000596 RID: 1430
	public const float DISTANCE_BETWEEN_HORIZONTAL_RAYS = 0.5f;

	// Token: 0x04000597 RID: 1431
	public const bool STICK_TO_SLOPES = true;

	// Token: 0x04000598 RID: 1432
	public const float BELOW_RAYCAST_LENGTH = 2f;

	// Token: 0x04000599 RID: 1433
	public const int MAX_SLOPE_ANGLE = 46;

	// Token: 0x0400059A RID: 1434
	public const float TYPEWRITER_SPEED = 0.015f;

	// Token: 0x0400059B RID: 1435
	public const float TYPEWRITER_LONG_SPEED = 0.1f;

	// Token: 0x0400059C RID: 1436
	public const int MAJOR_VERSION = 1;

	// Token: 0x0400059D RID: 1437
	public const int MINOR_VERSION = 0;

	// Token: 0x0400059E RID: 1438
	public const int PATCH_VERSION = 2;

	// Token: 0x0400059F RID: 1439
	public const string HOTPATCH_VERSION = "c";

	// Token: 0x040005A0 RID: 1440
	public const string PLATFORM_VERSION = "steam";

	// Token: 0x040005A1 RID: 1441
	public const int PLAYER_SAVE_REVISION_NUMBER = 13;

	// Token: 0x040005A2 RID: 1442
	public const int EQUIPMENT_SAVE_REVISION_NUMBER = 8;

	// Token: 0x040005A3 RID: 1443
	public const int LINEAGE_SAVE_REVISION_NUMBER = 1;

	// Token: 0x040005A4 RID: 1444
	public const int STAGE_SAVE_REVISION_NUMBER = 14;

	// Token: 0x040005A5 RID: 1445
	public const int MODE_SAVE_REVISION_NUMBER = 2;

	// Token: 0x040005A6 RID: 1446
	public const int CONFIG_SAVE_REVISION_NUMBER = 3;

	// Token: 0x040005A7 RID: 1447
	public const int PROFILE_CONFIG_SAVE_REVISION_NUMBER = 1;

	// Token: 0x040005A8 RID: 1448
	public static DateTime NEXT_PATCH_DATE = new DateTime(2020, 10, 27, 17, 0, 0, DateTimeKind.Utc);

	// Token: 0x040005A9 RID: 1449
	public const string PATCH_TIMER_TEXT = "TBD";

	// Token: 0x040005AA RID: 1450
	public const int UPDATES_SINCE_EA_LAUNCH = 32;
}
