using System;
using System.Collections.Generic;
using Rewired;

// Token: 0x02000CD8 RID: 3288
public static class RewiredMapController
{
	// Token: 0x17001EF1 RID: 7921
	// (get) Token: 0x06005DB0 RID: 23984 RVA: 0x000338D6 File Offset: 0x00031AD6
	// (set) Token: 0x06005DB1 RID: 23985 RVA: 0x000338DD File Offset: 0x00031ADD
	public static bool IsInCutscene { get; private set; }

	// Token: 0x06005DB2 RID: 23986 RVA: 0x000338E5 File Offset: 0x00031AE5
	private static Rewired_RL.MapCategoryType[] GetMapCategoryTypes(GameInputMode inputMode)
	{
		return RewiredMapController.m_gameModeCategoryDict[inputMode];
	}

	// Token: 0x17001EF2 RID: 7922
	// (get) Token: 0x06005DB3 RID: 23987 RVA: 0x000338F2 File Offset: 0x00031AF2
	public static GameInputMode CurrentGameInputMode
	{
		get
		{
			return RewiredMapController.m_currentGameInputMode;
		}
	}

	// Token: 0x06005DB4 RID: 23988 RVA: 0x000338F9 File Offset: 0x00031AF9
	public static void SetMap(GameInputMode inputMode)
	{
		if (ReInput.isReady)
		{
			RewiredMapController.m_currentGameInputMode = inputMode;
			ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(false);
			RewiredMapController.SetMapEnabled(RewiredMapController.m_currentGameInputMode, true);
		}
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x0003392F File Offset: 0x00031B2F
	public static void DisableAllMap()
	{
		if (ReInput.isReady)
		{
			ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(false);
		}
	}

	// Token: 0x06005DB6 RID: 23990 RVA: 0x0015EE70 File Offset: 0x0015D070
	public static void SetMapEnabled(GameInputMode inputMode, bool enabled)
	{
		if (ReInput.isReady)
		{
			if (inputMode == GameInputMode.Game)
			{
				RewiredMapController.m_shouldGameInputBeEnabled = enabled;
			}
			if (inputMode != GameInputMode.Game || (inputMode == GameInputMode.Game && !RewiredMapController.IsInCutscene))
			{
				foreach (Rewired_RL.MapCategoryType categoryType in RewiredMapController.GetMapCategoryTypes(inputMode))
				{
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(enabled, Rewired_RL.GetMapCategoryID(categoryType));
				}
			}
		}
	}

	// Token: 0x06005DB7 RID: 23991 RVA: 0x0015EEDC File Offset: 0x0015D0DC
	public static void SetIsInCutscene(bool inCutscene)
	{
		if (inCutscene)
		{
			foreach (Rewired_RL.MapCategoryType categoryType in RewiredMapController.GetMapCategoryTypes(GameInputMode.Game))
			{
				ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, Rewired_RL.GetMapCategoryID(categoryType));
			}
		}
		else if (RewiredMapController.m_shouldGameInputBeEnabled)
		{
			foreach (Rewired_RL.MapCategoryType categoryType2 in RewiredMapController.GetMapCategoryTypes(GameInputMode.Game))
			{
				ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(true, Rewired_RL.GetMapCategoryID(categoryType2));
			}
		}
		RewiredMapController.IsInCutscene = inCutscene;
	}

	// Token: 0x06005DB8 RID: 23992 RVA: 0x00033954 File Offset: 0x00031B54
	public static void SetCurrentMapEnabled(bool enabled)
	{
		RewiredMapController.SetMapEnabled(RewiredMapController.m_currentGameInputMode, enabled);
	}

	// Token: 0x06005DB9 RID: 23993 RVA: 0x00033961 File Offset: 0x00031B61
	public static void SetMouseEnabled(bool enabled)
	{
		ReInput.controllers.Mouse.enabled = enabled;
	}

	// Token: 0x17001EF3 RID: 7923
	// (get) Token: 0x06005DBA RID: 23994 RVA: 0x00033973 File Offset: 0x00031B73
	public static bool IsCurrentMapEnabled
	{
		get
		{
			return RewiredMapController.IsMapEnabled(RewiredMapController.m_currentGameInputMode);
		}
	}

	// Token: 0x06005DBB RID: 23995 RVA: 0x0015EF74 File Offset: 0x0015D174
	public static bool IsMapEnabled(GameInputMode inputMode)
	{
		if (!ReInput.isReady)
		{
			return false;
		}
		Rewired_RL.MapCategoryType categoryType = RewiredMapController.GetMapCategoryTypes(inputMode)[0];
		ControllerType controllerType;
		if (ReInput.players.GetPlayer(0).controllers.Keyboard != null)
		{
			controllerType = ControllerType.Keyboard;
		}
		else
		{
			controllerType = ControllerType.Joystick;
		}
		return ReInput.players.GetPlayer(0).controllers.maps.GetMap(controllerType, 0, Rewired_RL.GetMapCategoryID(categoryType), 0).enabled;
	}

	// Token: 0x04004D1D RID: 19741
	private static bool m_shouldGameInputBeEnabled = false;

	// Token: 0x04004D1E RID: 19742
	private static GameInputMode m_currentGameInputMode = GameInputMode.Game;

	// Token: 0x04004D1F RID: 19743
	private static Dictionary<GameInputMode, Rewired_RL.MapCategoryType[]> m_gameModeCategoryDict = new Dictionary<GameInputMode, Rewired_RL.MapCategoryType[]>
	{
		{
			GameInputMode.Game,
			new Rewired_RL.MapCategoryType[]
			{
				Rewired_RL.MapCategoryType.Action,
				Rewired_RL.MapCategoryType.ActionRemappable
			}
		},
		{
			GameInputMode.Window,
			new Rewired_RL.MapCategoryType[]
			{
				Rewired_RL.MapCategoryType.Window,
				Rewired_RL.MapCategoryType.WindowRemappable
			}
		}
	};
}
