using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020001FF RID: 511
public static class GameResolutionManager
{
	// Token: 0x0600158E RID: 5518
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	internal static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

	// Token: 0x0600158F RID: 5519
	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();

	// Token: 0x06001590 RID: 5520
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	internal static extern bool GetWindowRect(IntPtr hWnd, ref GameResolutionManager.RECT rect);

	// Token: 0x06001591 RID: 5521
	[DllImport("user32.dll")]
	private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, GameResolutionManager.MonitorEnumDelegate lpfnEnum, IntPtr dwData);

	// Token: 0x06001592 RID: 5522
	[DllImport("User32.dll", CharSet = CharSet.Unicode)]
	public static extern bool GetMonitorInfo(IntPtr hmonitor, [In] [Out] GameResolutionManager.MONITORINFOEX info);

	// Token: 0x06001593 RID: 5523
	[DllImport("User32.dll", ExactSpelling = true)]
	public static extern IntPtr MonitorFromPoint(GameResolutionManager.POINTSTRUCT pt, int flags);

	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x06001594 RID: 5524 RVA: 0x00043240 File Offset: 0x00041440
	// (set) Token: 0x06001595 RID: 5525 RVA: 0x00043247 File Offset: 0x00041447
	public static int ActiveDisplayIndex { get; private set; } = -1;

	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x06001596 RID: 5526 RVA: 0x0004324F File Offset: 0x0004144F
	// (set) Token: 0x06001597 RID: 5527 RVA: 0x00043256 File Offset: 0x00041456
	public static Vector2Int Resolution { get; private set; }

	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x06001598 RID: 5528 RVA: 0x0004325E File Offset: 0x0004145E
	// (set) Token: 0x06001599 RID: 5529 RVA: 0x00043265 File Offset: 0x00041465
	public static FullScreenMode FullscreenMode { get; private set; }

	// Token: 0x0600159A RID: 5530 RVA: 0x00043270 File Offset: 0x00041470
	public static void SetResolution(int xResolution, int yResolution, FullScreenMode fullScreenMode, bool saveConfig = true)
	{
		Screen.SetResolution(xResolution, yResolution, fullScreenMode, 0);
		GameResolutionManager.Resolution = new Vector2Int(xResolution, yResolution);
		GameResolutionManager.FullscreenMode = fullScreenMode;
		SaveManager.ConfigData.ScreenWidth = xResolution;
		SaveManager.ConfigData.ScreenHeight = yResolution;
		SaveManager.ConfigData.ScreenMode = (int)GameResolutionManager.FullscreenMode;
		if (saveConfig)
		{
			SaveManager.SaveConfigFile();
		}
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.ResolutionChanged, null, null);
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x000432CD File Offset: 0x000414CD
	public static void SetVsyncEnable(bool enable)
	{
		if (enable)
		{
			Debug.Log("Enabling Vsync");
			if (QualitySettings.vSyncCount != 1)
			{
				QualitySettings.vSyncCount = 1;
				return;
			}
		}
		else
		{
			Debug.Log("Disabling Vsync");
			if (QualitySettings.vSyncCount != 0)
			{
				QualitySettings.vSyncCount = 0;
			}
		}
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x00043304 File Offset: 0x00041504
	public static void SetPrimaryDisplay(int displayIndex)
	{
		List<GameResolutionManager.DisplayInfo> displays = GameResolutionManager.GetDisplays();
		if (displayIndex >= displays.Count)
		{
			Debug.Log("Failed to set Display Index: " + displayIndex.ToString() + " as primary display. Index out of bounds. Using index 0.");
			displayIndex = 0;
			SaveManager.ConfigData.PrimaryDisplay = displayIndex;
			SaveManager.SaveConfigFile();
		}
		if (GameResolutionManager.ActiveDisplayIndex == -1)
		{
			GameResolutionManager.ActiveDisplayIndex = PlayerPrefs.GetInt("UnitySelectMonitor");
		}
		if (GameResolutionManager.ActiveDisplayIndex == displayIndex)
		{
			return;
		}
		GameResolutionManager.ActiveDisplayIndex = displayIndex;
		PlayerPrefs.SetInt("UnitySelectMonitor", displayIndex);
		if (SaveManager.ConfigData.ScreenMode == 0)
		{
			SharedGameObjects_Loader sharedGameObjects_Loader = UnityEngine.Object.FindObjectOfType<SharedGameObjects_Loader>();
			if (sharedGameObjects_Loader)
			{
				sharedGameObjects_Loader.StartCoroutine(GameResolutionManager.MoveWindowFullscreenCoroutine(displayIndex));
				return;
			}
		}
		else
		{
			GameResolutionManager.DisplayInfo displayInfo = displays[displayIndex];
			int screenWidth = SaveManager.ConfigData.ScreenWidth;
			int screenHeight = SaveManager.ConfigData.ScreenHeight;
			bool flag = false;
			if (displayInfo.ScreenWidth < screenWidth)
			{
				screenWidth = displayInfo.ScreenWidth;
				flag = true;
			}
			if (displayInfo.ScreenHeight < screenHeight)
			{
				screenHeight = displayInfo.ScreenHeight;
				flag = true;
			}
			if (flag)
			{
				SaveManager.ConfigData.ScreenWidth = screenWidth;
				SaveManager.ConfigData.ScreenHeight = screenHeight;
				SaveManager.SaveConfigFile();
			}
			if (SaveManager.ConfigData.ScreenMode == 3)
			{
				int targetX = displayInfo.WorkArea.left + (displayInfo.WorkArea.right - displayInfo.WorkArea.left) / 2 - screenWidth / 2;
				int targetY = displayInfo.WorkArea.top + (displayInfo.WorkArea.bottom - displayInfo.WorkArea.top) / 2 - screenHeight / 2;
				GameResolutionManager.MoveToDisplay(screenWidth, screenHeight, targetX, targetY);
			}
			else
			{
				GameResolutionManager.MoveToDisplay(screenWidth, screenHeight, displayInfo.WorkArea.left, displayInfo.WorkArea.top);
			}
			Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.PrimaryDisplayChanged, null, null);
		}
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000434AC File Offset: 0x000416AC
	private static IEnumerator MoveWindowFullscreenCoroutine(int displayIndex)
	{
		int screenWidth = SaveManager.ConfigData.ScreenWidth;
		int screenHeight = SaveManager.ConfigData.ScreenHeight;
		GameResolutionManager.SetResolution(screenWidth, screenHeight, FullScreenMode.FullScreenWindow, false);
		float delay = Time.unscaledTime + 1f;
		while (Time.unscaledTime < delay)
		{
			yield return null;
		}
		GameResolutionManager.DisplayInfo displayInfo = GameResolutionManager.GetDisplays()[displayIndex];
		bool saveConfig = false;
		if (displayInfo.ScreenWidth < screenWidth)
		{
			screenWidth = displayInfo.ScreenWidth;
			saveConfig = true;
		}
		if (displayInfo.ScreenHeight < screenHeight)
		{
			screenHeight = displayInfo.ScreenHeight;
			saveConfig = true;
		}
		GameResolutionManager.MoveToDisplay(screenWidth, screenHeight, displayInfo.WorkArea.left, displayInfo.WorkArea.top);
		GameResolutionManager.SetResolution(screenWidth, screenHeight, FullScreenMode.ExclusiveFullScreen, saveConfig);
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.PrimaryDisplayChanged, null, null);
		yield break;
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x000434BC File Offset: 0x000416BC
	public static List<GameResolutionManager.DisplayInfo> GetDisplays()
	{
		GameResolutionManager.m_displayInfoList.Clear();
		GameResolutionManager.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref GameResolutionManager.RECT lprcMonitor, IntPtr dwData)
		{
			GameResolutionManager.MONITORINFOEX monitorinfoex = new GameResolutionManager.MONITORINFOEX();
			monitorinfoex.cbSize = Marshal.SizeOf<GameResolutionManager.MONITORINFOEX>(monitorinfoex);
			if (GameResolutionManager.GetMonitorInfo(hMonitor, monitorinfoex))
			{
				GameResolutionManager.DisplayInfo displayInfo = new GameResolutionManager.DisplayInfo();
				displayInfo.ScreenWidth = monitorinfoex.rcMonitor.right - monitorinfoex.rcMonitor.left;
				displayInfo.ScreenHeight = monitorinfoex.rcMonitor.bottom - monitorinfoex.rcMonitor.top;
				displayInfo.MonitorArea = monitorinfoex.rcMonitor;
				displayInfo.WorkArea = monitorinfoex.rcWork;
				displayInfo.Availability = monitorinfoex.dwFlags.ToString();
				GameResolutionManager.m_displayInfoList.Add(displayInfo);
			}
			return true;
		}, IntPtr.Zero);
		return GameResolutionManager.m_displayInfoList;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x0004350C File Offset: 0x0004170C
	private static void MoveToDisplay(int newWidth, int newHeight, int targetX, int targetY)
	{
		GameResolutionManager.MoveWindow(GameResolutionManager.GetActiveWindow(), targetX, targetY, newWidth, newHeight, true);
	}

	// Token: 0x040014D3 RID: 5331
	private static List<GameResolutionManager.DisplayInfo> m_displayInfoList = new List<GameResolutionManager.DisplayInfo>();

	// Token: 0x02000B1E RID: 2846
	// (Invoke) Token: 0x06005BC8 RID: 23496
	private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref GameResolutionManager.RECT lprcMonitor, IntPtr dwData);

	// Token: 0x02000B1F RID: 2847
	public struct RECT
	{
		// Token: 0x04004B64 RID: 19300
		public int left;

		// Token: 0x04004B65 RID: 19301
		public int top;

		// Token: 0x04004B66 RID: 19302
		public int right;

		// Token: 0x04004B67 RID: 19303
		public int bottom;
	}

	// Token: 0x02000B20 RID: 2848
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	public class MONITORINFOEX
	{
		// Token: 0x04004B68 RID: 19304
		public int cbSize = Marshal.SizeOf(typeof(GameResolutionManager.MONITORINFOEX));

		// Token: 0x04004B69 RID: 19305
		public GameResolutionManager.RECT rcMonitor;

		// Token: 0x04004B6A RID: 19306
		public GameResolutionManager.RECT rcWork;

		// Token: 0x04004B6B RID: 19307
		public int dwFlags;

		// Token: 0x04004B6C RID: 19308
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
		public char[] szDevice = new char[32];
	}

	// Token: 0x02000B21 RID: 2849
	public struct POINTSTRUCT
	{
		// Token: 0x06005BCC RID: 23500 RVA: 0x0015B1CE File Offset: 0x001593CE
		public POINTSTRUCT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04004B6D RID: 19309
		public int x;

		// Token: 0x04004B6E RID: 19310
		public int y;
	}

	// Token: 0x02000B22 RID: 2850
	public class DisplayInfo
	{
		// Token: 0x17001E4B RID: 7755
		// (get) Token: 0x06005BCD RID: 23501 RVA: 0x0015B1DE File Offset: 0x001593DE
		// (set) Token: 0x06005BCE RID: 23502 RVA: 0x0015B1E6 File Offset: 0x001593E6
		public string Availability { get; set; }

		// Token: 0x17001E4C RID: 7756
		// (get) Token: 0x06005BCF RID: 23503 RVA: 0x0015B1EF File Offset: 0x001593EF
		// (set) Token: 0x06005BD0 RID: 23504 RVA: 0x0015B1F7 File Offset: 0x001593F7
		public int ScreenHeight { get; set; }

		// Token: 0x17001E4D RID: 7757
		// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x0015B200 File Offset: 0x00159400
		// (set) Token: 0x06005BD2 RID: 23506 RVA: 0x0015B208 File Offset: 0x00159408
		public int ScreenWidth { get; set; }

		// Token: 0x17001E4E RID: 7758
		// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x0015B211 File Offset: 0x00159411
		// (set) Token: 0x06005BD4 RID: 23508 RVA: 0x0015B219 File Offset: 0x00159419
		public GameResolutionManager.RECT MonitorArea { get; set; }

		// Token: 0x17001E4F RID: 7759
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x0015B222 File Offset: 0x00159422
		// (set) Token: 0x06005BD6 RID: 23510 RVA: 0x0015B22A File Offset: 0x0015942A
		public GameResolutionManager.RECT WorkArea { get; set; }
	}
}
