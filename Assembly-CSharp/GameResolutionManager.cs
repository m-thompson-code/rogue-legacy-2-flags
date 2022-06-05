using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public static class GameResolutionManager
{
	// Token: 0x06001EE8 RID: 7912
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	internal static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

	// Token: 0x06001EE9 RID: 7913
	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();

	// Token: 0x06001EEA RID: 7914
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	internal static extern bool GetWindowRect(IntPtr hWnd, ref GameResolutionManager.RECT rect);

	// Token: 0x06001EEB RID: 7915
	[DllImport("user32.dll")]
	private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, GameResolutionManager.MonitorEnumDelegate lpfnEnum, IntPtr dwData);

	// Token: 0x06001EEC RID: 7916
	[DllImport("User32.dll", CharSet = CharSet.Unicode)]
	public static extern bool GetMonitorInfo(IntPtr hmonitor, [In] [Out] GameResolutionManager.MONITORINFOEX info);

	// Token: 0x06001EED RID: 7917
	[DllImport("User32.dll", ExactSpelling = true)]
	public static extern IntPtr MonitorFromPoint(GameResolutionManager.POINTSTRUCT pt, int flags);

	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x06001EEE RID: 7918 RVA: 0x00010379 File Offset: 0x0000E579
	// (set) Token: 0x06001EEF RID: 7919 RVA: 0x00010380 File Offset: 0x0000E580
	public static int ActiveDisplayIndex { get; private set; } = -1;

	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x00010388 File Offset: 0x0000E588
	// (set) Token: 0x06001EF1 RID: 7921 RVA: 0x0001038F File Offset: 0x0000E58F
	public static Vector2Int Resolution { get; private set; }

	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x00010397 File Offset: 0x0000E597
	// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x0001039E File Offset: 0x0000E59E
	public static FullScreenMode FullscreenMode { get; private set; }

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000A17F4 File Offset: 0x0009F9F4
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

	// Token: 0x06001EF5 RID: 7925 RVA: 0x000103A6 File Offset: 0x0000E5A6
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

	// Token: 0x06001EF6 RID: 7926 RVA: 0x000A1854 File Offset: 0x0009FA54
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

	// Token: 0x06001EF7 RID: 7927 RVA: 0x000103DB File Offset: 0x0000E5DB
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

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000A19FC File Offset: 0x0009FBFC
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

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000103EA File Offset: 0x0000E5EA
	private static void MoveToDisplay(int newWidth, int newHeight, int targetX, int targetY)
	{
		GameResolutionManager.MoveWindow(GameResolutionManager.GetActiveWindow(), targetX, targetY, newWidth, newHeight, true);
	}

	// Token: 0x04001BA2 RID: 7074
	private static List<GameResolutionManager.DisplayInfo> m_displayInfoList = new List<GameResolutionManager.DisplayInfo>();

	// Token: 0x020003A5 RID: 933
	// (Invoke) Token: 0x06001EFC RID: 7932
	private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref GameResolutionManager.RECT lprcMonitor, IntPtr dwData);

	// Token: 0x020003A6 RID: 934
	public struct RECT
	{
		// Token: 0x04001BA3 RID: 7075
		public int left;

		// Token: 0x04001BA4 RID: 7076
		public int top;

		// Token: 0x04001BA5 RID: 7077
		public int right;

		// Token: 0x04001BA6 RID: 7078
		public int bottom;
	}

	// Token: 0x020003A7 RID: 935
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	public class MONITORINFOEX
	{
		// Token: 0x04001BA7 RID: 7079
		public int cbSize = Marshal.SizeOf(typeof(GameResolutionManager.MONITORINFOEX));

		// Token: 0x04001BA8 RID: 7080
		public GameResolutionManager.RECT rcMonitor;

		// Token: 0x04001BA9 RID: 7081
		public GameResolutionManager.RECT rcWork;

		// Token: 0x04001BAA RID: 7082
		public int dwFlags;

		// Token: 0x04001BAB RID: 7083
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
		public char[] szDevice = new char[32];
	}

	// Token: 0x020003A8 RID: 936
	public struct POINTSTRUCT
	{
		// Token: 0x06001F00 RID: 7936 RVA: 0x00010437 File Offset: 0x0000E637
		public POINTSTRUCT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04001BAC RID: 7084
		public int x;

		// Token: 0x04001BAD RID: 7085
		public int y;
	}

	// Token: 0x020003A9 RID: 937
	public class DisplayInfo
	{
		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x00010447 File Offset: 0x0000E647
		// (set) Token: 0x06001F02 RID: 7938 RVA: 0x0001044F File Offset: 0x0000E64F
		public string Availability { get; set; }

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x00010458 File Offset: 0x0000E658
		// (set) Token: 0x06001F04 RID: 7940 RVA: 0x00010460 File Offset: 0x0000E660
		public int ScreenHeight { get; set; }

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x00010469 File Offset: 0x0000E669
		// (set) Token: 0x06001F06 RID: 7942 RVA: 0x00010471 File Offset: 0x0000E671
		public int ScreenWidth { get; set; }

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x0001047A File Offset: 0x0000E67A
		// (set) Token: 0x06001F08 RID: 7944 RVA: 0x00010482 File Offset: 0x0000E682
		public GameResolutionManager.RECT MonitorArea { get; set; }

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x0001048B File Offset: 0x0000E68B
		// (set) Token: 0x06001F0A RID: 7946 RVA: 0x00010493 File Offset: 0x0000E693
		public GameResolutionManager.RECT WorkArea { get; set; }
	}
}
