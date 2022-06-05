using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

// Token: 0x02000C23 RID: 3107
public class Rewired_RL
{
	// Token: 0x17001E3C RID: 7740
	// (get) Token: 0x06005AAA RID: 23210 RVA: 0x00031B49 File Offset: 0x0002FD49
	public static Player Player
	{
		get
		{
			if (!ReInput.isReady)
			{
				return null;
			}
			return ReInput.players.GetPlayer(0);
		}
	}

	// Token: 0x17001E3D RID: 7741
	// (get) Token: 0x06005AAB RID: 23211 RVA: 0x00031B5F File Offset: 0x0002FD5F
	public static bool IsGamepadConnected
	{
		get
		{
			return ReInput.isReady && Rewired_RL.Player.controllers.joystickCount > 0;
		}
	}

	// Token: 0x06005AAC RID: 23212 RVA: 0x00031B7C File Offset: 0x0002FD7C
	public static int GetMapCategoryID(Rewired_RL.MapCategoryType categoryType)
	{
		return ReInput.mapping.GetMapCategoryId(Rewired_RL.GetMapCategoryName(categoryType));
	}

	// Token: 0x17001E3E RID: 7742
	// (get) Token: 0x06005AAD RID: 23213 RVA: 0x00031B8E File Offset: 0x0002FD8E
	public static Rewired_RL.InputActionType[] InputActionTypeArray
	{
		get
		{
			if (Rewired_RL.m_inputActionTypeArray == null)
			{
				Rewired_RL.m_inputActionTypeArray = (Enum.GetValues(typeof(Rewired_RL.InputActionType)) as Rewired_RL.InputActionType[]);
			}
			return Rewired_RL.m_inputActionTypeArray;
		}
	}

	// Token: 0x17001E3F RID: 7743
	// (get) Token: 0x06005AAE RID: 23214 RVA: 0x00031BB5 File Offset: 0x0002FDB5
	public static Rewired_RL.WindowInputActionType[] WindowInputActionTypeArray
	{
		get
		{
			if (Rewired_RL.m_windowInputActionTypeArray == null)
			{
				Rewired_RL.m_windowInputActionTypeArray = (Enum.GetValues(typeof(Rewired_RL.WindowInputActionType)) as Rewired_RL.WindowInputActionType[]);
			}
			return Rewired_RL.m_windowInputActionTypeArray;
		}
	}

	// Token: 0x06005AAF RID: 23215 RVA: 0x001569D0 File Offset: 0x00154BD0
	public static Rewired_RL.InputActionType GetActionInputType(string actionInputName)
	{
		if (actionInputName != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(actionInputName);
			if (num <= 1943884296U)
			{
				if (num <= 596758484U)
				{
					if (num <= 236909357U)
					{
						if (num != 182978943U)
						{
							if (num == 236909357U)
							{
								if (actionInputName == "Jump")
								{
									return Rewired_RL.InputActionType.Action_Jump;
								}
							}
						}
						else if (actionInputName == "Start")
						{
							return Rewired_RL.InputActionType.Action_Start;
						}
					}
					else if (num != 293923304U)
					{
						if (num != 506378735U)
						{
							if (num == 596758484U)
							{
								if (actionInputName == "MoveVerticalR")
								{
									return Rewired_RL.InputActionType.Action_MoveVerticalR;
								}
							}
						}
						else if (actionInputName == "Interact")
						{
							return Rewired_RL.InputActionType.Action_Interact;
						}
					}
					else if (actionInputName == "UserReport")
					{
						return Rewired_RL.InputActionType.Action_UserReport;
					}
				}
				else if (num <= 1049176909U)
				{
					if (num != 794818787U)
					{
						if (num != 945351785U)
						{
							if (num == 1049176909U)
							{
								if (actionInputName == "Select")
								{
									return Rewired_RL.InputActionType.Action_Select;
								}
							}
						}
						else if (actionInputName == "Spell")
						{
							return Rewired_RL.InputActionType.Action_Spell;
						}
					}
					else if (actionInputName == "Flight")
					{
						return Rewired_RL.InputActionType.Action_Flight;
					}
				}
				else if (num != 1157218093U)
				{
					if (num != 1541630844U)
					{
						if (num == 1943884296U)
						{
							if (actionInputName == "FreeLook")
							{
								return Rewired_RL.InputActionType.Action_FreeLook;
							}
						}
					}
					else if (actionInputName == "MoveHorizontal")
					{
						return Rewired_RL.InputActionType.Action_MoveHorizontal;
					}
				}
				else if (actionInputName == "Pause")
				{
					return Rewired_RL.InputActionType.Action_Pause;
				}
			}
			else if (num <= 3004047413U)
			{
				if (num <= 2343121693U)
				{
					if (num != 2219379200U)
					{
						if (num == 2343121693U)
						{
							if (actionInputName == "Attack")
							{
								return Rewired_RL.InputActionType.Action_Attack;
							}
						}
					}
					else if (actionInputName == "DashLeft")
					{
						return Rewired_RL.InputActionType.Action_DashLeft;
					}
				}
				else if (num != 2573627295U)
				{
					if (num != 2908907986U)
					{
						if (num == 3004047413U)
						{
							if (actionInputName == "DashRight")
							{
								return Rewired_RL.InputActionType.Action_DashRight;
							}
						}
					}
					else if (actionInputName == "DEBUG_R3")
					{
						return Rewired_RL.InputActionType.Action_DebugR3;
					}
				}
				else if (actionInputName == "Downstrike")
				{
					return Rewired_RL.InputActionType.Action_Downstrike;
				}
			}
			else if (num <= 3450676494U)
			{
				if (num != 3040863272U)
				{
					if (num != 3326082795U)
					{
						if (num == 3450676494U)
						{
							if (actionInputName == "MoveVertical")
							{
								return Rewired_RL.InputActionType.Action_MoveVertical;
							}
						}
					}
					else if (actionInputName == "DEBUG_LB")
					{
						return Rewired_RL.InputActionType.Action_DebugLB;
					}
				}
				else if (actionInputName == "DEBUG_L3")
				{
					return Rewired_RL.InputActionType.Action_DebugL3;
				}
			}
			else if (num != 3573660010U)
			{
				if (num != 3731011317U)
				{
					if (num == 4184099403U)
					{
						if (actionInputName == "Talent")
						{
							return Rewired_RL.InputActionType.Action_Talent;
						}
					}
				}
				else if (actionInputName == "DEBUG_RB")
				{
					return Rewired_RL.InputActionType.Action_DebugRB;
				}
			}
			else if (actionInputName == "MoveHorizontalR")
			{
				return Rewired_RL.InputActionType.Action_MoveHorizontalR;
			}
		}
		return Rewired_RL.InputActionType.None;
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x00156D18 File Offset: 0x00154F18
	public static Rewired_RL.WindowInputActionType GetWindowInputType(string windowInputName)
	{
		if (windowInputName != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(windowInputName);
			if (num <= 1883140641U)
			{
				if (num <= 1028809144U)
				{
					if (num <= 772891383U)
					{
						if (num != 587439162U)
						{
							if (num == 772891383U)
							{
								if (windowInputName == "Window_UserReport")
								{
									return Rewired_RL.WindowInputActionType.Window_UserReport;
								}
							}
						}
						else if (windowInputName == "Window_Horizontal")
						{
							return Rewired_RL.WindowInputActionType.Window_Horizontal;
						}
					}
					else if (num != 837749246U)
					{
						if (num == 1028809144U)
						{
							if (windowInputName == "Window_RT")
							{
								return Rewired_RL.WindowInputActionType.Window_RT;
							}
						}
					}
					else if (windowInputName == "Window_Select")
					{
						return Rewired_RL.WindowInputActionType.Window_Select;
					}
				}
				else if (num <= 1392388215U)
				{
					if (num != 1094242334U)
					{
						if (num == 1392388215U)
						{
							if (windowInputName == "Window_AllMovement_RStick")
							{
								return Rewired_RL.WindowInputActionType.Window_AllMovement_RStick;
							}
						}
					}
					else if (windowInputName == "Window_LT")
					{
						return Rewired_RL.WindowInputActionType.Window_LT;
					}
				}
				else if (num != 1396239476U)
				{
					if (num != 1397916762U)
					{
						if (num == 1883140641U)
						{
							if (windowInputName == "Window_Horizontal_RStick")
							{
								return Rewired_RL.WindowInputActionType.Window_Horizontal_RStick;
							}
						}
					}
					else if (windowInputName == "Window_RB")
					{
						return Rewired_RL.WindowInputActionType.Window_RB;
					}
				}
				else if (windowInputName == "Window_LB")
				{
					return Rewired_RL.WindowInputActionType.Window_LB;
				}
			}
			else if (num <= 2974169219U)
			{
				if (num <= 2801142332U)
				{
					if (num != 2152151621U)
					{
						if (num == 2801142332U)
						{
							if (windowInputName == "Window_R")
							{
								return Rewired_RL.WindowInputActionType.Window_R;
							}
						}
					}
					else if (windowInputName == "Window_AllMovement_LStick")
					{
						return Rewired_RL.WindowInputActionType.Window_AllMovement_LStick;
					}
				}
				else if (num != 2968918522U)
				{
					if (num == 2974169219U)
					{
						if (windowInputName == "Window_Vertical_RStick")
						{
							return Rewired_RL.WindowInputActionType.Window_Vertical_RStick;
						}
					}
				}
				else if (windowInputName == "Window_X")
				{
					return Rewired_RL.WindowInputActionType.Window_X;
				}
			}
			else if (num <= 3321728476U)
			{
				if (num != 2985696141U)
				{
					if (num == 3321728476U)
					{
						if (windowInputName == "Window_Cancel")
						{
							return Rewired_RL.WindowInputActionType.Window_Cancel;
						}
					}
				}
				else if (windowInputName == "Window_Y")
				{
					return Rewired_RL.WindowInputActionType.Window_Y;
				}
			}
			else if (num != 3423447334U)
			{
				if (num != 3527354476U)
				{
					if (num == 4154718278U)
					{
						if (windowInputName == "Window_Confirm")
						{
							return Rewired_RL.WindowInputActionType.Window_Confirm;
						}
					}
				}
				else if (windowInputName == "Window_Vertical")
				{
					return Rewired_RL.WindowInputActionType.Window_Vertical;
				}
			}
			else if (windowInputName == "Window_Start")
			{
				return Rewired_RL.WindowInputActionType.Window_Start;
			}
		}
		return Rewired_RL.WindowInputActionType.None;
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x00156FCC File Offset: 0x001551CC
	public static string GetString(Rewired_RL.InputActionType actionInputType)
	{
		switch (actionInputType)
		{
		case Rewired_RL.InputActionType.Action_Jump:
			return "Jump";
		case Rewired_RL.InputActionType.Action_MoveHorizontal:
			return "MoveHorizontal";
		case Rewired_RL.InputActionType.Action_MoveVertical:
			return "MoveVertical";
		case Rewired_RL.InputActionType.Action_Attack:
			return "Attack";
		case Rewired_RL.InputActionType.Action_DashLeft:
			return "DashLeft";
		case Rewired_RL.InputActionType.Action_DashRight:
			return "DashRight";
		case Rewired_RL.InputActionType.Action_Spell:
			return "Spell";
		case Rewired_RL.InputActionType.Action_Talent:
			return "Talent";
		case Rewired_RL.InputActionType.Action_Interact:
			return "Interact";
		case Rewired_RL.InputActionType.Action_Start:
			return "Start";
		case Rewired_RL.InputActionType.Action_Select:
			return "Select";
		case Rewired_RL.InputActionType.Action_Pause:
			return "Pause";
		case Rewired_RL.InputActionType.Action_DebugLB:
			return "DEBUG_LB";
		case Rewired_RL.InputActionType.Action_DebugRB:
			return "DEBUG_RB";
		case Rewired_RL.InputActionType.Action_DebugL3:
			return "DEBUG_L3";
		case Rewired_RL.InputActionType.Action_DebugR3:
			return "DEBUG_R3";
		case Rewired_RL.InputActionType.Action_UserReport:
			return "UserReport";
		case Rewired_RL.InputActionType.Action_Downstrike:
			return "Downstrike";
		case Rewired_RL.InputActionType.Action_MoveHorizontalR:
			return "MoveHorizontalR";
		case Rewired_RL.InputActionType.Action_MoveVerticalR:
			return "MoveVerticalR";
		case Rewired_RL.InputActionType.Action_FreeLook:
			return "FreeLook";
		case Rewired_RL.InputActionType.Action_Flight:
			return "Flight";
		default:
			return "";
		}
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x001570C4 File Offset: 0x001552C4
	public static string GetString(Rewired_RL.WindowInputActionType windowInputType)
	{
		switch (windowInputType)
		{
		case Rewired_RL.WindowInputActionType.Window_Horizontal:
			return "Window_Horizontal";
		case Rewired_RL.WindowInputActionType.Window_Vertical:
			return "Window_Vertical";
		case Rewired_RL.WindowInputActionType.Window_Start:
			return "Window_Start";
		case Rewired_RL.WindowInputActionType.Window_Select:
			return "Window_Select";
		case Rewired_RL.WindowInputActionType.Window_Confirm:
			return "Window_Confirm";
		case Rewired_RL.WindowInputActionType.Window_Cancel:
			return "Window_Cancel";
		case Rewired_RL.WindowInputActionType.Window_X:
			return "Window_X";
		case Rewired_RL.WindowInputActionType.Window_Y:
			return "Window_Y";
		case Rewired_RL.WindowInputActionType.Window_LB:
			return "Window_LB";
		case Rewired_RL.WindowInputActionType.Window_RB:
			return "Window_RB";
		case Rewired_RL.WindowInputActionType.Window_LT:
			return "Window_LT";
		case Rewired_RL.WindowInputActionType.Window_RT:
			return "Window_RT";
		case Rewired_RL.WindowInputActionType.Window_Horizontal_RStick:
			return "Window_Horizontal_RStick";
		case Rewired_RL.WindowInputActionType.Window_Vertical_RStick:
			return "Window_Vertical_RStick";
		case Rewired_RL.WindowInputActionType.Window_AllMovement_LStick:
			return "Window_AllMovement_LStick";
		case Rewired_RL.WindowInputActionType.Window_AllMovement_RStick:
			return "Window_AllMovement_RStick";
		case Rewired_RL.WindowInputActionType.Window_UserReport:
			return "Window_UserReport";
		case Rewired_RL.WindowInputActionType.Window_R:
			return "Window_R";
		default:
			return "";
		}
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x00157194 File Offset: 0x00155394
	public static string GetMapCategoryName(Rewired_RL.MapCategoryType categoryType)
	{
		if (categoryType <= Rewired_RL.MapCategoryType.WindowRemappable)
		{
			switch (categoryType)
			{
			case Rewired_RL.MapCategoryType.Action:
				return "Player";
			case Rewired_RL.MapCategoryType.ActionRemappable:
				return "PlayerRemappable";
			case Rewired_RL.MapCategoryType.Action | Rewired_RL.MapCategoryType.ActionRemappable:
				break;
			case Rewired_RL.MapCategoryType.Window:
				return "Window";
			default:
				if (categoryType == Rewired_RL.MapCategoryType.WindowRemappable)
				{
					return "WindowRemappable";
				}
				break;
			}
		}
		else
		{
			if (categoryType == Rewired_RL.MapCategoryType.IllegalAssignments)
			{
				return "IllegalAssignments";
			}
			if (categoryType == Rewired_RL.MapCategoryType.GamepadGlyphs)
			{
				return "GamepadGlyphs";
			}
		}
		return null;
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x001571F4 File Offset: 0x001553F4
	public static bool HasPoleAxes(string actionName)
	{
		if (actionName != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(actionName);
			if (num <= 1883140641U)
			{
				if (num <= 596758484U)
				{
					if (num != 587439162U)
					{
						if (num != 596758484U)
						{
							return false;
						}
						if (!(actionName == "MoveVerticalR"))
						{
							return false;
						}
					}
					else if (!(actionName == "Window_Horizontal"))
					{
						return false;
					}
				}
				else if (num != 1541630844U)
				{
					if (num != 1883140641U)
					{
						return false;
					}
					if (!(actionName == "Window_Horizontal_RStick"))
					{
						return false;
					}
				}
				else if (!(actionName == "MoveHorizontal"))
				{
					return false;
				}
			}
			else if (num <= 3450676494U)
			{
				if (num != 2974169219U)
				{
					if (num != 3450676494U)
					{
						return false;
					}
					if (!(actionName == "MoveVertical"))
					{
						return false;
					}
				}
				else if (!(actionName == "Window_Vertical_RStick"))
				{
					return false;
				}
			}
			else if (num != 3527354476U)
			{
				if (num != 3573660010U)
				{
					return false;
				}
				if (!(actionName == "MoveHorizontalR"))
				{
					return false;
				}
			}
			else if (!(actionName == "Window_Vertical"))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x001572FC File Offset: 0x001554FC
	public static bool IgnoreNullAemOnKeyboard(string actionName)
	{
		if (actionName != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(actionName);
			if (num <= 2974169219U)
			{
				if (num <= 1883140641U)
				{
					if (num != 596758484U)
					{
						if (num != 1883140641U)
						{
							return false;
						}
						if (!(actionName == "Window_Horizontal_RStick"))
						{
							return false;
						}
					}
					else if (!(actionName == "MoveVerticalR"))
					{
						return false;
					}
				}
				else if (num != 2908907986U)
				{
					if (num != 2974169219U)
					{
						return false;
					}
					if (!(actionName == "Window_Vertical_RStick"))
					{
						return false;
					}
				}
				else if (!(actionName == "DEBUG_R3"))
				{
					return false;
				}
			}
			else if (num <= 3326082795U)
			{
				if (num != 3040863272U)
				{
					if (num != 3326082795U)
					{
						return false;
					}
					if (!(actionName == "DEBUG_LB"))
					{
						return false;
					}
				}
				else if (!(actionName == "DEBUG_L3"))
				{
					return false;
				}
			}
			else if (num != 3423447334U)
			{
				if (num != 3573660010U)
				{
					if (num != 3731011317U)
					{
						return false;
					}
					if (!(actionName == "DEBUG_RB"))
					{
						return false;
					}
				}
				else if (!(actionName == "MoveHorizontalR"))
				{
					return false;
				}
			}
			else if (!(actionName == "Window_Start"))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x00031BDC File Offset: 0x0002FDDC
	public static bool IgnoreNullAemOnGamepad(string actionName)
	{
		return actionName != null && (actionName == "UserReport" || actionName == "Window_UserReport");
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x00031BFE File Offset: 0x0002FDFE
	public static bool IsStandardJoystick(Controller controller)
	{
		return ReInput.players.GetPlayer(0).controllers.maps.GetFirstElementMapWithAction(controller, 4, false) != null;
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x0015741C File Offset: 0x0015561C
	public static bool DoesActionExistInMapCategory(string actionName, Rewired_RL.MapCategoryType mapCategory, ControllerType controllerType, Pole axis = Pole.Positive)
	{
		int mapCategoryID = Rewired_RL.GetMapCategoryID(mapCategory);
		Rewired_RL.m_reusableAemList.Clear();
		ReInput.players.GetPlayer(0).controllers.maps.GetElementMapsWithAction(controllerType, actionName, false, Rewired_RL.m_reusableAemList);
		foreach (ActionElementMap actionElementMap in Rewired_RL.m_reusableAemList)
		{
			if (actionElementMap.controllerMap.categoryId == mapCategoryID && actionElementMap.axisContribution == axis)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x001574BC File Offset: 0x001556BC
	public static Rewired_RL.MapCategoryType GetMapCategoryTypeFlags(ControllerType controllerType, string actionName, Pole axis = Pole.Positive)
	{
		Rewired_RL.MapCategoryType mapCategoryType = Rewired_RL.MapCategoryType.ActionRemappable;
		Rewired_RL.MapCategoryType mapCategoryType2 = Rewired_RL.MapCategoryType.Action;
		if (Rewired_RL.GetWindowInputType(actionName) != Rewired_RL.WindowInputActionType.None)
		{
			mapCategoryType = Rewired_RL.MapCategoryType.WindowRemappable;
			mapCategoryType2 = Rewired_RL.MapCategoryType.Window;
		}
		bool flag = Rewired_RL.DoesActionExistInMapCategory(actionName, mapCategoryType, controllerType, axis);
		bool flag2 = Rewired_RL.DoesActionExistInMapCategory(actionName, mapCategoryType2, controllerType, axis);
		Rewired_RL.MapCategoryType mapCategoryType3 = Rewired_RL.MapCategoryType.None;
		if (flag)
		{
			mapCategoryType3 |= mapCategoryType;
		}
		if (flag2)
		{
			mapCategoryType3 |= mapCategoryType2;
		}
		return mapCategoryType3;
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x001574FC File Offset: 0x001556FC
	public static Rewired_RL.MapCategoryType GetMapCategoryTypeFromFlag(Rewired_RL.MapCategoryType flag, bool prioritizeNonMappable)
	{
		Rewired_RL.MapCategoryType mapCategoryType = Rewired_RL.MapCategoryType.None;
		if ((Rewired_RL.MapCategoryType.ActionRemappable & flag) != Rewired_RL.MapCategoryType.None)
		{
			mapCategoryType = Rewired_RL.MapCategoryType.ActionRemappable;
		}
		if (prioritizeNonMappable || (mapCategoryType == Rewired_RL.MapCategoryType.None && (Rewired_RL.MapCategoryType.Action & flag) != Rewired_RL.MapCategoryType.None))
		{
			mapCategoryType = Rewired_RL.MapCategoryType.Action;
		}
		if (mapCategoryType == Rewired_RL.MapCategoryType.None)
		{
			if ((Rewired_RL.MapCategoryType.WindowRemappable & flag) != Rewired_RL.MapCategoryType.None)
			{
				mapCategoryType = Rewired_RL.MapCategoryType.WindowRemappable;
			}
			if (prioritizeNonMappable || (mapCategoryType == Rewired_RL.MapCategoryType.None && (Rewired_RL.MapCategoryType.Window & flag) != Rewired_RL.MapCategoryType.None))
			{
				mapCategoryType = Rewired_RL.MapCategoryType.Window;
			}
		}
		return mapCategoryType;
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x00157538 File Offset: 0x00155738
	public static ActionElementMap GetActionElementMap(bool useGamepad, InputAction inputAction, Pole axis, bool prioritizeNonRemappable, int controllerID = 0)
	{
		ActionElementMap actionElementMap = null;
		Player player = ReInput.players.GetPlayer(0);
		Rewired_RL.m_controllerTypeHelperList.Clear();
		if (useGamepad)
		{
			Rewired_RL.m_controllerTypeHelperList.Add(ControllerType.Joystick);
		}
		else
		{
			Rewired_RL.m_controllerTypeHelperList.Add(ControllerType.Mouse);
			Rewired_RL.m_controllerTypeHelperList.Add(ControllerType.Keyboard);
		}
		foreach (ControllerType controllerType in Rewired_RL.m_controllerTypeHelperList)
		{
			Rewired_RL.MapCategoryType mapCategoryTypeFlags = Rewired_RL.GetMapCategoryTypeFlags(controllerType, inputAction.name, axis);
			if (mapCategoryTypeFlags != Rewired_RL.MapCategoryType.None)
			{
				int mapCategoryID = Rewired_RL.GetMapCategoryID(Rewired_RL.GetMapCategoryTypeFromFlag(mapCategoryTypeFlags, prioritizeNonRemappable));
				ControllerMap map = player.controllers.maps.GetMap(controllerType, controllerID, mapCategoryID, 0);
				Rewired_RL.m_reusableAemList.Clear();
				map.GetElementMapsWithAction(inputAction.id, Rewired_RL.m_reusableAemList);
				if (Rewired_RL.m_reusableAemList.Count > 0)
				{
					if (Rewired_RL.m_reusableAemList.Count <= 1)
					{
						actionElementMap = Rewired_RL.m_reusableAemList[Rewired_RL.m_reusableAemList.Count - 1];
						break;
					}
					if (inputAction.type == Rewired.InputActionType.Axis)
					{
						int index = -1;
						for (int i = 0; i < Rewired_RL.m_reusableAemList.Count; i++)
						{
							if (Rewired_RL.m_reusableAemList[i].axisContribution == axis)
							{
								index = i;
							}
						}
						actionElementMap = Rewired_RL.m_reusableAemList[index];
						break;
					}
				}
			}
		}
		bool flag = (!useGamepad && Rewired_RL.IgnoreNullAemOnKeyboard(inputAction.name)) || (useGamepad && Rewired_RL.IgnoreNullAemOnGamepad(inputAction.name));
		if (actionElementMap == null && !flag)
		{
			Debug.Log("ActionElementMap null. InputAction name: " + inputAction.name + ". Use Gamepad: " + useGamepad.ToString());
		}
		return actionElementMap;
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x001576EC File Offset: 0x001558EC
	public static ActionElementMap GetActionElementMap(bool useGamepad, string actionName, Pole axis, bool prioritizeNonRemappable, int controllerID)
	{
		InputAction action = ReInput.mapping.GetAction(actionName);
		if (action != null)
		{
			return Rewired_RL.GetActionElementMap(useGamepad, action, axis, prioritizeNonRemappable, controllerID);
		}
		Debug.Log("InputAction null on: " + actionName);
		return null;
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x00031C20 File Offset: 0x0002FE20
	public static void ResetControllerMapToDefault(bool resetGamepad)
	{
		ReInput.players.GetPlayer(0);
		if (!resetGamepad)
		{
			Rewired_RL.ResetControllerMap(ControllerType.Mouse);
			Rewired_RL.ResetControllerMap(ControllerType.Keyboard);
			return;
		}
		Rewired_RL.ResetControllerMap(ControllerType.Joystick);
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x00031C44 File Offset: 0x0002FE44
	private static void ResetControllerMap(ControllerType controllerType)
	{
		ReInput.players.GetPlayer(0).controllers.maps.LoadDefaultMaps(controllerType);
	}

	// Token: 0x0400487B RID: 18555
	private static List<ActionElementMap> m_reusableAemList = new List<ActionElementMap>();

	// Token: 0x0400487C RID: 18556
	private static List<ControllerType> m_controllerTypeHelperList = new List<ControllerType>();

	// Token: 0x0400487D RID: 18557
	public const string ACTION_MAPCATEGORY_NAME = "Player";

	// Token: 0x0400487E RID: 18558
	public const string ACTION_REMAPPABLE_MAPCATEGORY_NAME = "PlayerRemappable";

	// Token: 0x0400487F RID: 18559
	public const string WINDOW_MAPCATEGORY_NAME = "Window";

	// Token: 0x04004880 RID: 18560
	public const string WINDOW_REMAPPABLE_MAPCATEGORY_NAME = "WindowRemappable";

	// Token: 0x04004881 RID: 18561
	public const string ILLEGAL_ASSIGNMENTS_MAPCATEGORY_NAME = "IllegalAssignments";

	// Token: 0x04004882 RID: 18562
	public const string GAMEPAD_GLYPHS_CATEGORY_NAME = "GamepadGlyphs";

	// Token: 0x04004883 RID: 18563
	public const string LAYOUT_NAME = "Default";

	// Token: 0x04004884 RID: 18564
	public const string Action_Jump = "Jump";

	// Token: 0x04004885 RID: 18565
	public const string Action_MoveHorizontal = "MoveHorizontal";

	// Token: 0x04004886 RID: 18566
	public const string Action_MoveVertical = "MoveVertical";

	// Token: 0x04004887 RID: 18567
	public const string Action_MoveHorizontalR = "MoveHorizontalR";

	// Token: 0x04004888 RID: 18568
	public const string Action_MoveVerticalR = "MoveVerticalR";

	// Token: 0x04004889 RID: 18569
	public const string Action_Attack = "Attack";

	// Token: 0x0400488A RID: 18570
	public const string Action_DashLeft = "DashLeft";

	// Token: 0x0400488B RID: 18571
	public const string Action_DashRight = "DashRight";

	// Token: 0x0400488C RID: 18572
	public const string Action_Spell = "Spell";

	// Token: 0x0400488D RID: 18573
	public const string Action_Talent = "Talent";

	// Token: 0x0400488E RID: 18574
	public const string Action_Interact = "Interact";

	// Token: 0x0400488F RID: 18575
	public const string Action_Downstrike = "Downstrike";

	// Token: 0x04004890 RID: 18576
	public const string Action_FreeLook = "FreeLook";

	// Token: 0x04004891 RID: 18577
	public const string Action_Flight = "Flight";

	// Token: 0x04004892 RID: 18578
	public const string Action_Start = "Start";

	// Token: 0x04004893 RID: 18579
	public const string Action_Select = "Select";

	// Token: 0x04004894 RID: 18580
	public const string Action_Pause = "Pause";

	// Token: 0x04004895 RID: 18581
	public const string Action_UserReport = "UserReport";

	// Token: 0x04004896 RID: 18582
	public const string Action_DebugLB = "DEBUG_LB";

	// Token: 0x04004897 RID: 18583
	public const string Action_DebugRB = "DEBUG_RB";

	// Token: 0x04004898 RID: 18584
	public const string Action_DebugL3 = "DEBUG_L3";

	// Token: 0x04004899 RID: 18585
	public const string Action_DebugR3 = "DEBUG_R3";

	// Token: 0x0400489A RID: 18586
	public const string Window_Horizontal = "Window_Horizontal";

	// Token: 0x0400489B RID: 18587
	public const string Window_Vertical = "Window_Vertical";

	// Token: 0x0400489C RID: 18588
	public const string Window_Start = "Window_Start";

	// Token: 0x0400489D RID: 18589
	public const string Window_Select = "Window_Select";

	// Token: 0x0400489E RID: 18590
	public const string Window_Confirm = "Window_Confirm";

	// Token: 0x0400489F RID: 18591
	public const string Window_Cancel = "Window_Cancel";

	// Token: 0x040048A0 RID: 18592
	public const string Window_X = "Window_X";

	// Token: 0x040048A1 RID: 18593
	public const string Window_Y = "Window_Y";

	// Token: 0x040048A2 RID: 18594
	public const string Window_LB = "Window_LB";

	// Token: 0x040048A3 RID: 18595
	public const string Window_RB = "Window_RB";

	// Token: 0x040048A4 RID: 18596
	public const string Window_LT = "Window_LT";

	// Token: 0x040048A5 RID: 18597
	public const string Window_RT = "Window_RT";

	// Token: 0x040048A6 RID: 18598
	public const string Window_Horizontal_RStick = "Window_Horizontal_RStick";

	// Token: 0x040048A7 RID: 18599
	public const string Window_Vertical_RStick = "Window_Vertical_RStick";

	// Token: 0x040048A8 RID: 18600
	public const string Window_UserReport = "Window_UserReport";

	// Token: 0x040048A9 RID: 18601
	public const string Window_R = "Window_R";

	// Token: 0x040048AA RID: 18602
	public const string Window_AllMovement_LStick = "Window_AllMovement_LStick";

	// Token: 0x040048AB RID: 18603
	public const string Window_AllMovement_RStick = "Window_AllMovement_RStick";

	// Token: 0x040048AC RID: 18604
	private static Rewired_RL.InputActionType[] m_inputActionTypeArray;

	// Token: 0x040048AD RID: 18605
	private static Rewired_RL.WindowInputActionType[] m_windowInputActionTypeArray;

	// Token: 0x02000C24 RID: 3108
	[Flags]
	public enum MapCategoryType
	{
		// Token: 0x040048AF RID: 18607
		None = 0,
		// Token: 0x040048B0 RID: 18608
		Action = 1,
		// Token: 0x040048B1 RID: 18609
		ActionRemappable = 2,
		// Token: 0x040048B2 RID: 18610
		Window = 4,
		// Token: 0x040048B3 RID: 18611
		WindowRemappable = 8,
		// Token: 0x040048B4 RID: 18612
		IllegalAssignments = 16,
		// Token: 0x040048B5 RID: 18613
		GamepadGlyphs = 32
	}

	// Token: 0x02000C25 RID: 3109
	public enum InputActionType
	{
		// Token: 0x040048B7 RID: 18615
		None,
		// Token: 0x040048B8 RID: 18616
		Action_Jump,
		// Token: 0x040048B9 RID: 18617
		Action_MoveHorizontal,
		// Token: 0x040048BA RID: 18618
		Action_MoveVertical,
		// Token: 0x040048BB RID: 18619
		Action_Attack,
		// Token: 0x040048BC RID: 18620
		Action_DashLeft,
		// Token: 0x040048BD RID: 18621
		Action_DashRight,
		// Token: 0x040048BE RID: 18622
		Action_Spell,
		// Token: 0x040048BF RID: 18623
		Action_Talent,
		// Token: 0x040048C0 RID: 18624
		Action_Interact,
		// Token: 0x040048C1 RID: 18625
		Action_Start,
		// Token: 0x040048C2 RID: 18626
		Action_Select,
		// Token: 0x040048C3 RID: 18627
		Action_Pause,
		// Token: 0x040048C4 RID: 18628
		Action_DebugLB,
		// Token: 0x040048C5 RID: 18629
		Action_DebugRB,
		// Token: 0x040048C6 RID: 18630
		Action_DebugL3,
		// Token: 0x040048C7 RID: 18631
		Action_DebugR3,
		// Token: 0x040048C8 RID: 18632
		Action_UserReport,
		// Token: 0x040048C9 RID: 18633
		Action_Downstrike,
		// Token: 0x040048CA RID: 18634
		Action_MoveHorizontalR,
		// Token: 0x040048CB RID: 18635
		Action_MoveVerticalR,
		// Token: 0x040048CC RID: 18636
		Action_FreeLook,
		// Token: 0x040048CD RID: 18637
		Action_Flight
	}

	// Token: 0x02000C26 RID: 3110
	public enum WindowInputActionType
	{
		// Token: 0x040048CF RID: 18639
		None,
		// Token: 0x040048D0 RID: 18640
		Window_Horizontal,
		// Token: 0x040048D1 RID: 18641
		Window_Vertical,
		// Token: 0x040048D2 RID: 18642
		Window_Start,
		// Token: 0x040048D3 RID: 18643
		Window_Select,
		// Token: 0x040048D4 RID: 18644
		Window_Confirm,
		// Token: 0x040048D5 RID: 18645
		Window_Cancel,
		// Token: 0x040048D6 RID: 18646
		Window_X,
		// Token: 0x040048D7 RID: 18647
		Window_Y,
		// Token: 0x040048D8 RID: 18648
		Window_LB,
		// Token: 0x040048D9 RID: 18649
		Window_RB,
		// Token: 0x040048DA RID: 18650
		Window_LT,
		// Token: 0x040048DB RID: 18651
		Window_RT,
		// Token: 0x040048DC RID: 18652
		Window_Horizontal_RStick,
		// Token: 0x040048DD RID: 18653
		Window_Vertical_RStick,
		// Token: 0x040048DE RID: 18654
		Window_AllMovement_LStick,
		// Token: 0x040048DF RID: 18655
		Window_AllMovement_RStick,
		// Token: 0x040048E0 RID: 18656
		Window_UserReport,
		// Token: 0x040048E1 RID: 18657
		Window_R
	}
}
