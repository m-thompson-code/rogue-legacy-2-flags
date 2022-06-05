using System;
using System.Collections.Generic;
using Rewired;

// Token: 0x0200080D RID: 2061
public static class RewiredMapController
{
	// Token: 0x170016F1 RID: 5873
	// (get) Token: 0x0600441F RID: 17439 RVA: 0x000F121F File Offset: 0x000EF41F
	// (set) Token: 0x06004420 RID: 17440 RVA: 0x000F1226 File Offset: 0x000EF426
	public static bool IsInCutscene { get; private set; }

	// Token: 0x06004421 RID: 17441 RVA: 0x000F122E File Offset: 0x000EF42E
	private static Rewired_RL.MapCategoryType[] GetMapCategoryTypes(GameInputMode inputMode)
	{
		return RewiredMapController.m_gameModeCategoryDict[inputMode];
	}

	// Token: 0x170016F2 RID: 5874
	// (get) Token: 0x06004422 RID: 17442 RVA: 0x000F123B File Offset: 0x000EF43B
	public static GameInputMode CurrentGameInputMode
	{
		get
		{
			return RewiredMapController.m_currentGameInputMode;
		}
	}

	// Token: 0x06004423 RID: 17443 RVA: 0x000F1242 File Offset: 0x000EF442
	public static void SetMap(GameInputMode inputMode)
	{
		if (ReInput.isReady)
		{
			RewiredMapController.m_currentGameInputMode = inputMode;
			ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(false);
			RewiredMapController.SetMapEnabled(RewiredMapController.m_currentGameInputMode, true);
		}
	}

	// Token: 0x06004424 RID: 17444 RVA: 0x000F1278 File Offset: 0x000EF478
	public static void DisableAllMap()
	{
		if (ReInput.isReady)
		{
			ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(false);
		}
	}

	// Token: 0x06004425 RID: 17445 RVA: 0x000F12A0 File Offset: 0x000EF4A0
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

	// Token: 0x06004426 RID: 17446 RVA: 0x000F130C File Offset: 0x000EF50C
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

	// Token: 0x06004427 RID: 17447 RVA: 0x000F13A3 File Offset: 0x000EF5A3
	public static void SetCurrentMapEnabled(bool enabled)
	{
		RewiredMapController.SetMapEnabled(RewiredMapController.m_currentGameInputMode, enabled);
	}

	// Token: 0x06004428 RID: 17448 RVA: 0x000F13B0 File Offset: 0x000EF5B0
	public static void SetMouseEnabled(bool enabled)
	{
		ReInput.controllers.Mouse.enabled = enabled;
	}

	// Token: 0x170016F3 RID: 5875
	// (get) Token: 0x06004429 RID: 17449 RVA: 0x000F13C2 File Offset: 0x000EF5C2
	public static bool IsCurrentMapEnabled
	{
		get
		{
			return RewiredMapController.IsMapEnabled(RewiredMapController.m_currentGameInputMode);
		}
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x000F13D0 File Offset: 0x000EF5D0
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

	// Token: 0x04003A41 RID: 14913
	private static bool m_shouldGameInputBeEnabled = false;

	// Token: 0x04003A42 RID: 14914
	private static GameInputMode m_currentGameInputMode = GameInputMode.Game;

	// Token: 0x04003A43 RID: 14915
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
