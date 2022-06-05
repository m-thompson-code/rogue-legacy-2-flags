using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200045E RID: 1118
public class GamepadImageSelector : MonoBehaviour
{
	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x060023AA RID: 9130 RVA: 0x00013A10 File Offset: 0x00011C10
	public GamepadType GamepadType
	{
		get
		{
			return this.m_gamepadType;
		}
	}

	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x060023AB RID: 9131 RVA: 0x00013A18 File Offset: 0x00011C18
	// (set) Token: 0x060023AC RID: 9132 RVA: 0x00013A20 File Offset: 0x00011C20
	public int ControllerID { get; set; }

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x060023AD RID: 9133 RVA: 0x00013A29 File Offset: 0x00011C29
	public bool IsXboxSeries
	{
		get
		{
			return this.m_isXboxSeries;
		}
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x00013A31 File Offset: 0x00011C31
	private void Awake()
	{
		this.DeactivateAllImages();
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000AD144 File Offset: 0x000AB344
	public void EnableGamepadImage(string actionName, Pole axis, bool disableAllOthers)
	{
		ActionElementMap actionElementMap = Rewired_RL.GetActionElementMap(true, actionName, axis, false, this.ControllerID);
		ControllerMap map = Rewired_RL.Player.controllers.maps.GetMap(ControllerType.Joystick, this.ControllerID, Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.GamepadGlyphs), 0);
		if (actionElementMap != null && map.ContainsElementIdentifier(actionElementMap.elementIdentifierId))
		{
			GamepadImageSelector.m_aemListHelper.Clear();
			map.GetElementMaps(false, GamepadImageSelector.m_aemListHelper);
			using (List<ActionElementMap>.Enumerator enumerator = GamepadImageSelector.m_aemListHelper.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActionElementMap actionElementMap2 = enumerator.Current;
					if (actionElementMap2.elementIdentifierId == actionElementMap.elementIdentifierId)
					{
						if (disableAllOthers)
						{
							this.DeactivateAllImages();
						}
						string name = ReInput.mapping.GetAction(actionElementMap2.actionId).name;
						Image imageFromActionName = this.GetImageFromActionName(name);
						if (imageFromActionName)
						{
							imageFromActionName.gameObject.SetActive(true);
						}
						break;
					}
				}
				return;
			}
		}
		if (disableAllOthers)
		{
			this.DeactivateAllImages();
		}
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000AD24C File Offset: 0x000AB44C
	private Image GetImageFromActionName(string actionName)
	{
		if (actionName != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(actionName);
			if (num <= 2124245956U)
			{
				if (num <= 591316005U)
				{
					if (num != 235163378U)
					{
						if (num != 367118664U)
						{
							if (num == 591316005U)
							{
								if (actionName == "DpadLeft")
								{
									return this.m_dpadButtonImage;
								}
							}
						}
						else if (actionName == "L3")
						{
							return this.m_l3ButtonImage;
						}
					}
					else if (actionName == "R3")
					{
						return this.m_r3ButtonImage;
					}
				}
				else if (num <= 1187421356U)
				{
					if (num != 778272617U)
					{
						if (num == 1187421356U)
						{
							if (actionName == "Left Trigger")
							{
								return this.m_ltButtonImage;
							}
						}
					}
					else if (actionName == "DpadUp")
					{
						return this.m_dpadButtonImage;
					}
				}
				else if (num != 1563367678U)
				{
					if (num == 2124245956U)
					{
						if (actionName == "Left Button")
						{
							return this.m_lbButtonImage;
						}
					}
				}
				else if (actionName == "DpadRight")
				{
					return this.m_dpadButtonImage;
				}
			}
			else if (num <= 3339451269U)
			{
				if (num <= 2862628385U)
				{
					if (num != 2807966800U)
					{
						if (num == 2862628385U)
						{
							if (actionName == "Right Trigger")
							{
								return this.m_rtButtonImage;
							}
						}
					}
					else if (actionName == "DpadDown")
					{
						return this.m_dpadButtonImage;
					}
				}
				else if (num != 3289118412U)
				{
					if (num == 3339451269U)
					{
						if (actionName == "B")
						{
							return this.m_bButtonImage;
						}
					}
				}
				else if (actionName == "A")
				{
					return this.m_aButtonImage;
				}
			}
			else if (num <= 3554079661U)
			{
				if (num != 3353373007U)
				{
					if (num == 3554079661U)
					{
						if (actionName == "TouchPad")
						{
							return this.m_touchPadImage;
						}
					}
				}
				else if (actionName == "Right Button")
				{
					return this.m_rbButtonImage;
				}
			}
			else if (num != 3691781268U)
			{
				if (num == 3708558887U)
				{
					if (actionName == "X")
					{
						return this.m_xButtonImage;
					}
				}
			}
			else if (actionName == "Y")
			{
				return this.m_yButtonImage;
			}
		}
		return null;
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000AD4F4 File Offset: 0x000AB6F4
	private void DeactivateAllImages()
	{
		if (this.m_rbButtonImage.gameObject.activeSelf)
		{
			this.m_rbButtonImage.gameObject.SetActive(false);
		}
		if (this.m_lbButtonImage.gameObject.activeSelf)
		{
			this.m_lbButtonImage.gameObject.SetActive(false);
		}
		if (this.m_rtButtonImage.gameObject.activeSelf)
		{
			this.m_rtButtonImage.gameObject.SetActive(false);
		}
		if (this.m_ltButtonImage.gameObject.activeSelf)
		{
			this.m_ltButtonImage.gameObject.SetActive(false);
		}
		if (this.m_l3ButtonImage.gameObject.activeSelf)
		{
			this.m_l3ButtonImage.gameObject.SetActive(false);
		}
		if (this.m_r3ButtonImage.gameObject.activeSelf)
		{
			this.m_r3ButtonImage.gameObject.SetActive(false);
		}
		if (this.m_xButtonImage.gameObject.activeSelf)
		{
			this.m_xButtonImage.gameObject.SetActive(false);
		}
		if (this.m_bButtonImage.gameObject.activeSelf)
		{
			this.m_bButtonImage.gameObject.SetActive(false);
		}
		if (this.m_yButtonImage.gameObject.activeSelf)
		{
			this.m_yButtonImage.gameObject.SetActive(false);
		}
		if (this.m_aButtonImage.gameObject.activeSelf)
		{
			this.m_aButtonImage.gameObject.SetActive(false);
		}
		if (this.m_dpadButtonImage.gameObject.activeSelf)
		{
			this.m_dpadButtonImage.gameObject.SetActive(false);
		}
		if (this.m_touchPadImage && this.m_touchPadImage.gameObject.activeSelf)
		{
			this.m_touchPadImage.gameObject.SetActive(false);
		}
	}

	// Token: 0x04001F99 RID: 8089
	private static List<ActionElementMap> m_aemListHelper = new List<ActionElementMap>();

	// Token: 0x04001F9A RID: 8090
	private const string DpadUp_Name = "DpadUp";

	// Token: 0x04001F9B RID: 8091
	private const string DpadDown_Name = "DpadDown";

	// Token: 0x04001F9C RID: 8092
	private const string DpadLeft_Name = "DpadLeft";

	// Token: 0x04001F9D RID: 8093
	private const string DpadRight_Name = "DpadRight";

	// Token: 0x04001F9E RID: 8094
	private const string L3_Name = "L3";

	// Token: 0x04001F9F RID: 8095
	private const string R3_Name = "R3";

	// Token: 0x04001FA0 RID: 8096
	private const string LB_Name = "Left Button";

	// Token: 0x04001FA1 RID: 8097
	private const string LT_Name = "Left Trigger";

	// Token: 0x04001FA2 RID: 8098
	private const string RB_Name = "Right Button";

	// Token: 0x04001FA3 RID: 8099
	private const string RT_Name = "Right Trigger";

	// Token: 0x04001FA4 RID: 8100
	private const string X_Name = "X";

	// Token: 0x04001FA5 RID: 8101
	private const string A_Name = "A";

	// Token: 0x04001FA6 RID: 8102
	private const string B_Name = "B";

	// Token: 0x04001FA7 RID: 8103
	private const string Y_Name = "Y";

	// Token: 0x04001FA8 RID: 8104
	private const string TouchPad_Name = "TouchPad";

	// Token: 0x04001FA9 RID: 8105
	[SerializeField]
	private GamepadType m_gamepadType;

	// Token: 0x04001FAA RID: 8106
	[SerializeField]
	private Image m_rbButtonImage;

	// Token: 0x04001FAB RID: 8107
	[SerializeField]
	private Image m_lbButtonImage;

	// Token: 0x04001FAC RID: 8108
	[SerializeField]
	private Image m_rtButtonImage;

	// Token: 0x04001FAD RID: 8109
	[SerializeField]
	private Image m_ltButtonImage;

	// Token: 0x04001FAE RID: 8110
	[SerializeField]
	private Image m_l3ButtonImage;

	// Token: 0x04001FAF RID: 8111
	[SerializeField]
	private Image m_r3ButtonImage;

	// Token: 0x04001FB0 RID: 8112
	[SerializeField]
	private Image m_xButtonImage;

	// Token: 0x04001FB1 RID: 8113
	[SerializeField]
	private Image m_bButtonImage;

	// Token: 0x04001FB2 RID: 8114
	[SerializeField]
	private Image m_yButtonImage;

	// Token: 0x04001FB3 RID: 8115
	[SerializeField]
	private Image m_aButtonImage;

	// Token: 0x04001FB4 RID: 8116
	[SerializeField]
	private Image m_dpadButtonImage;

	// Token: 0x04001FB5 RID: 8117
	[SerializeField]
	private Image m_touchPadImage;

	// Token: 0x04001FB6 RID: 8118
	[SerializeField]
	private bool m_isXboxSeries;
}
