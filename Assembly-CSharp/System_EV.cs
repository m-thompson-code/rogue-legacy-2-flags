using System;

// Token: 0x02000080 RID: 128
public static class System_EV
{
	// Token: 0x060001D0 RID: 464 RVA: 0x00011C08 File Offset: 0x0000FE08
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

	// Token: 0x04000569 RID: 1385
	public const float FIXED_DELTA_TIME = 0.016666668f;

	// Token: 0x0400056A RID: 1386
	public const float MAXIMUM_DELTA_TIME = 0.033333335f;

	// Token: 0x0400056B RID: 1387
	public const int DEFAULT_TARGET_FRAME_RATE = 120;

	// Token: 0x0400056C RID: 1388
	public static readonly int[] MAX_FPS_OPTIONS = new int[]
	{
		60,
		120,
		144,
		240
	};

	// Token: 0x0400056D RID: 1389
	public const byte NUM_SAVE_BACKUPS = 5;

	// Token: 0x0400056E RID: 1390
	public const float SAVE_RESTRICTION_DURATION = 10f;

	// Token: 0x0400056F RID: 1391
	public const int NUM_PROFILES = 5;

	// Token: 0x04000570 RID: 1392
	public const int MIN_NUMBER_VERTICAL_RAYS = 2;

	// Token: 0x04000571 RID: 1393
	public const int MAX_NUMBER_VERTICAL_RAYS = 6;

	// Token: 0x04000572 RID: 1394
	public const int MIN_NUMBER_HORIZONTAL_RAYS = 2;

	// Token: 0x04000573 RID: 1395
	public const int MAX_NUMBER_HORIZONTAL_RAYS = 6;

	// Token: 0x04000574 RID: 1396
	public const float DISTANCE_BETWEEN_VERTICAL_RAYS = 0.5f;

	// Token: 0x04000575 RID: 1397
	public const float DISTANCE_BETWEEN_HORIZONTAL_RAYS = 0.5f;

	// Token: 0x04000576 RID: 1398
	public const bool STICK_TO_SLOPES = true;

	// Token: 0x04000577 RID: 1399
	public const float BELOW_RAYCAST_LENGTH = 2f;

	// Token: 0x04000578 RID: 1400
	public const int MAX_SLOPE_ANGLE = 46;

	// Token: 0x04000579 RID: 1401
	public const float TYPEWRITER_SPEED = 0.015f;

	// Token: 0x0400057A RID: 1402
	public const float TYPEWRITER_LONG_SPEED = 0.1f;

	// Token: 0x0400057B RID: 1403
	public const int MAJOR_VERSION = 1;

	// Token: 0x0400057C RID: 1404
	public const int MINOR_VERSION = 0;

	// Token: 0x0400057D RID: 1405
	public const int PATCH_VERSION = 2;

	// Token: 0x0400057E RID: 1406
	public const string HOTPATCH_VERSION = "c";

	// Token: 0x0400057F RID: 1407
	public const string PLATFORM_VERSION = "steam";

	// Token: 0x04000580 RID: 1408
	public const int PLAYER_SAVE_REVISION_NUMBER = 13;

	// Token: 0x04000581 RID: 1409
	public const int EQUIPMENT_SAVE_REVISION_NUMBER = 8;

	// Token: 0x04000582 RID: 1410
	public const int LINEAGE_SAVE_REVISION_NUMBER = 1;

	// Token: 0x04000583 RID: 1411
	public const int STAGE_SAVE_REVISION_NUMBER = 14;

	// Token: 0x04000584 RID: 1412
	public const int MODE_SAVE_REVISION_NUMBER = 2;

	// Token: 0x04000585 RID: 1413
	public const int CONFIG_SAVE_REVISION_NUMBER = 3;

	// Token: 0x04000586 RID: 1414
	public const int PROFILE_CONFIG_SAVE_REVISION_NUMBER = 1;

	// Token: 0x04000587 RID: 1415
	public static DateTime NEXT_PATCH_DATE = new DateTime(2020, 10, 27, 17, 0, 0, DateTimeKind.Utc);

	// Token: 0x04000588 RID: 1416
	public const string PATCH_TIMER_TEXT = "TBD";

	// Token: 0x04000589 RID: 1417
	public const int UPDATES_SINCE_EA_LAUNCH = 32;
}
