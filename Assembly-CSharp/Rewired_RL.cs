using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

// Token: 0x02000765 RID: 1893
public class Rewired_RL
{
	// Token: 0x17001640 RID: 5696
	// (get) Token: 0x0600412D RID: 16685 RVA: 0x000E72B9 File Offset: 0x000E54B9
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

	// Token: 0x17001641 RID: 5697
	// (get) Token: 0x0600412E RID: 16686 RVA: 0x000E72CF File Offset: 0x000E54CF
	public static bool IsGamepadConnected
	{
		get
		{
			return ReInput.isReady && Rewired_RL.Player.controllers.joystickCount > 0;
		}
	}

	// Token: 0x0600412F RID: 16687 RVA: 0x000E72EC File Offset: 0x000E54EC
	public static int GetMapCategoryID(Rewired_RL.MapCategoryType categoryType)
	{
		return ReInput.mapping.GetMapCategoryId(Rewired_RL.GetMapCategoryName(categoryType));
	}

	// Token: 0x17001642 RID: 5698
	// (get) Token: 0x06004130 RID: 16688 RVA: 0x000E72FE File Offset: 0x000E54FE
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

	// Token: 0x17001643 RID: 5699
	// (get) Token: 0x06004131 RID: 16689 RVA: 0x000E7325 File Offset: 0x000E5525
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

	// Token: 0x06004132 RID: 16690 RVA: 0x000E734C File Offset: 0x000E554C
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

	// Token: 0x06004133 RID: 16691 RVA: 0x000E7694 File Offset: 0x000E5894
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

	// Token: 0x06004134 RID: 16692 RVA: 0x000E7948 File Offset: 0x000E5B48
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

	// Token: 0x06004135 RID: 16693 RVA: 0x000E7A40 File Offset: 0x000E5C40
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

	// Token: 0x06004136 RID: 16694 RVA: 0x000E7B10 File Offset: 0x000E5D10
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

	// Token: 0x06004137 RID: 16695 RVA: 0x000E7B70 File Offset: 0x000E5D70
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

	// Token: 0x06004138 RID: 16696 RVA: 0x000E7C78 File Offset: 0x000E5E78
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

	// Token: 0x06004139 RID: 16697 RVA: 0x000E7D97 File Offset: 0x000E5F97
	public static bool IgnoreNullAemOnGamepad(string actionName)
	{
		return actionName != null && (actionName == "UserReport" || actionName == "Window_UserReport");
	}

	// Token: 0x0600413A RID: 16698 RVA: 0x000E7DB9 File Offset: 0x000E5FB9
	public static bool IsStandardJoystick(Controller controller)
	{
		return ReInput.players.GetPlayer(0).controllers.maps.GetFirstElementMapWithAction(controller, 4, false) != null;
	}

	// Token: 0x0600413B RID: 16699 RVA: 0x000E7DDC File Offset: 0x000E5FDC
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

	// Token: 0x0600413C RID: 16700 RVA: 0x000E7E7C File Offset: 0x000E607C
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

	// Token: 0x0600413D RID: 16701 RVA: 0x000E7EBC File Offset: 0x000E60BC
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

	// Token: 0x0600413E RID: 16702 RVA: 0x000E7EF8 File Offset: 0x000E60F8
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

	// Token: 0x0600413F RID: 16703 RVA: 0x000E80AC File Offset: 0x000E62AC
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

	// Token: 0x06004140 RID: 16704 RVA: 0x000E80E5 File Offset: 0x000E62E5
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

	// Token: 0x06004141 RID: 16705 RVA: 0x000E8109 File Offset: 0x000E6309
	private static void ResetControllerMap(ControllerType controllerType)
	{
		ReInput.players.GetPlayer(0).controllers.maps.LoadDefaultMaps(controllerType);
	}

	// Token: 0x040035FF RID: 13823
	private static List<ActionElementMap> m_reusableAemList = new List<ActionElementMap>();

	// Token: 0x04003600 RID: 13824
	private static List<ControllerType> m_controllerTypeHelperList = new List<ControllerType>();

	// Token: 0x04003601 RID: 13825
	public const string ACTION_MAPCATEGORY_NAME = "Player";

	// Token: 0x04003602 RID: 13826
	public const string ACTION_REMAPPABLE_MAPCATEGORY_NAME = "PlayerRemappable";

	// Token: 0x04003603 RID: 13827
	public const string WINDOW_MAPCATEGORY_NAME = "Window";

	// Token: 0x04003604 RID: 13828
	public const string WINDOW_REMAPPABLE_MAPCATEGORY_NAME = "WindowRemappable";

	// Token: 0x04003605 RID: 13829
	public const string ILLEGAL_ASSIGNMENTS_MAPCATEGORY_NAME = "IllegalAssignments";

	// Token: 0x04003606 RID: 13830
	public const string GAMEPAD_GLYPHS_CATEGORY_NAME = "GamepadGlyphs";

	// Token: 0x04003607 RID: 13831
	public const string LAYOUT_NAME = "Default";

	// Token: 0x04003608 RID: 13832
	public const string Action_Jump = "Jump";

	// Token: 0x04003609 RID: 13833
	public const string Action_MoveHorizontal = "MoveHorizontal";

	// Token: 0x0400360A RID: 13834
	public const string Action_MoveVertical = "MoveVertical";

	// Token: 0x0400360B RID: 13835
	public const string Action_MoveHorizontalR = "MoveHorizontalR";

	// Token: 0x0400360C RID: 13836
	public const string Action_MoveVerticalR = "MoveVerticalR";

	// Token: 0x0400360D RID: 13837
	public const string Action_Attack = "Attack";

	// Token: 0x0400360E RID: 13838
	public const string Action_DashLeft = "DashLeft";

	// Token: 0x0400360F RID: 13839
	public const string Action_DashRight = "DashRight";

	// Token: 0x04003610 RID: 13840
	public const string Action_Spell = "Spell";

	// Token: 0x04003611 RID: 13841
	public const string Action_Talent = "Talent";

	// Token: 0x04003612 RID: 13842
	public const string Action_Interact = "Interact";

	// Token: 0x04003613 RID: 13843
	public const string Action_Downstrike = "Downstrike";

	// Token: 0x04003614 RID: 13844
	public const string Action_FreeLook = "FreeLook";

	// Token: 0x04003615 RID: 13845
	public const string Action_Flight = "Flight";

	// Token: 0x04003616 RID: 13846
	public const string Action_Start = "Start";

	// Token: 0x04003617 RID: 13847
	public const string Action_Select = "Select";

	// Token: 0x04003618 RID: 13848
	public const string Action_Pause = "Pause";

	// Token: 0x04003619 RID: 13849
	public const string Action_UserReport = "UserReport";

	// Token: 0x0400361A RID: 13850
	public const string Action_DebugLB = "DEBUG_LB";

	// Token: 0x0400361B RID: 13851
	public const string Action_DebugRB = "DEBUG_RB";

	// Token: 0x0400361C RID: 13852
	public const string Action_DebugL3 = "DEBUG_L3";

	// Token: 0x0400361D RID: 13853
	public const string Action_DebugR3 = "DEBUG_R3";

	// Token: 0x0400361E RID: 13854
	public const string Window_Horizontal = "Window_Horizontal";

	// Token: 0x0400361F RID: 13855
	public const string Window_Vertical = "Window_Vertical";

	// Token: 0x04003620 RID: 13856
	public const string Window_Start = "Window_Start";

	// Token: 0x04003621 RID: 13857
	public const string Window_Select = "Window_Select";

	// Token: 0x04003622 RID: 13858
	public const string Window_Confirm = "Window_Confirm";

	// Token: 0x04003623 RID: 13859
	public const string Window_Cancel = "Window_Cancel";

	// Token: 0x04003624 RID: 13860
	public const string Window_X = "Window_X";

	// Token: 0x04003625 RID: 13861
	public const string Window_Y = "Window_Y";

	// Token: 0x04003626 RID: 13862
	public const string Window_LB = "Window_LB";

	// Token: 0x04003627 RID: 13863
	public const string Window_RB = "Window_RB";

	// Token: 0x04003628 RID: 13864
	public const string Window_LT = "Window_LT";

	// Token: 0x04003629 RID: 13865
	public const string Window_RT = "Window_RT";

	// Token: 0x0400362A RID: 13866
	public const string Window_Horizontal_RStick = "Window_Horizontal_RStick";

	// Token: 0x0400362B RID: 13867
	public const string Window_Vertical_RStick = "Window_Vertical_RStick";

	// Token: 0x0400362C RID: 13868
	public const string Window_UserReport = "Window_UserReport";

	// Token: 0x0400362D RID: 13869
	public const string Window_R = "Window_R";

	// Token: 0x0400362E RID: 13870
	public const string Window_AllMovement_LStick = "Window_AllMovement_LStick";

	// Token: 0x0400362F RID: 13871
	public const string Window_AllMovement_RStick = "Window_AllMovement_RStick";

	// Token: 0x04003630 RID: 13872
	private static Rewired_RL.InputActionType[] m_inputActionTypeArray;

	// Token: 0x04003631 RID: 13873
	private static Rewired_RL.WindowInputActionType[] m_windowInputActionTypeArray;

	// Token: 0x02000E32 RID: 3634
	[Flags]
	public enum MapCategoryType
	{
		// Token: 0x040056FF RID: 22271
		None = 0,
		// Token: 0x04005700 RID: 22272
		Action = 1,
		// Token: 0x04005701 RID: 22273
		ActionRemappable = 2,
		// Token: 0x04005702 RID: 22274
		Window = 4,
		// Token: 0x04005703 RID: 22275
		WindowRemappable = 8,
		// Token: 0x04005704 RID: 22276
		IllegalAssignments = 16,
		// Token: 0x04005705 RID: 22277
		GamepadGlyphs = 32
	}

	// Token: 0x02000E33 RID: 3635
	public enum InputActionType
	{
		// Token: 0x04005707 RID: 22279
		None,
		// Token: 0x04005708 RID: 22280
		Action_Jump,
		// Token: 0x04005709 RID: 22281
		Action_MoveHorizontal,
		// Token: 0x0400570A RID: 22282
		Action_MoveVertical,
		// Token: 0x0400570B RID: 22283
		Action_Attack,
		// Token: 0x0400570C RID: 22284
		Action_DashLeft,
		// Token: 0x0400570D RID: 22285
		Action_DashRight,
		// Token: 0x0400570E RID: 22286
		Action_Spell,
		// Token: 0x0400570F RID: 22287
		Action_Talent,
		// Token: 0x04005710 RID: 22288
		Action_Interact,
		// Token: 0x04005711 RID: 22289
		Action_Start,
		// Token: 0x04005712 RID: 22290
		Action_Select,
		// Token: 0x04005713 RID: 22291
		Action_Pause,
		// Token: 0x04005714 RID: 22292
		Action_DebugLB,
		// Token: 0x04005715 RID: 22293
		Action_DebugRB,
		// Token: 0x04005716 RID: 22294
		Action_DebugL3,
		// Token: 0x04005717 RID: 22295
		Action_DebugR3,
		// Token: 0x04005718 RID: 22296
		Action_UserReport,
		// Token: 0x04005719 RID: 22297
		Action_Downstrike,
		// Token: 0x0400571A RID: 22298
		Action_MoveHorizontalR,
		// Token: 0x0400571B RID: 22299
		Action_MoveVerticalR,
		// Token: 0x0400571C RID: 22300
		Action_FreeLook,
		// Token: 0x0400571D RID: 22301
		Action_Flight
	}

	// Token: 0x02000E34 RID: 3636
	public enum WindowInputActionType
	{
		// Token: 0x0400571F RID: 22303
		None,
		// Token: 0x04005720 RID: 22304
		Window_Horizontal,
		// Token: 0x04005721 RID: 22305
		Window_Vertical,
		// Token: 0x04005722 RID: 22306
		Window_Start,
		// Token: 0x04005723 RID: 22307
		Window_Select,
		// Token: 0x04005724 RID: 22308
		Window_Confirm,
		// Token: 0x04005725 RID: 22309
		Window_Cancel,
		// Token: 0x04005726 RID: 22310
		Window_X,
		// Token: 0x04005727 RID: 22311
		Window_Y,
		// Token: 0x04005728 RID: 22312
		Window_LB,
		// Token: 0x04005729 RID: 22313
		Window_RB,
		// Token: 0x0400572A RID: 22314
		Window_LT,
		// Token: 0x0400572B RID: 22315
		Window_RT,
		// Token: 0x0400572C RID: 22316
		Window_Horizontal_RStick,
		// Token: 0x0400572D RID: 22317
		Window_Vertical_RStick,
		// Token: 0x0400572E RID: 22318
		Window_AllMovement_LStick,
		// Token: 0x0400572F RID: 22319
		Window_AllMovement_RStick,
		// Token: 0x04005730 RID: 22320
		Window_UserReport,
		// Token: 0x04005731 RID: 22321
		Window_R
	}
}
