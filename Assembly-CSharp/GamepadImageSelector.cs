using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000292 RID: 658
public class GamepadImageSelector : MonoBehaviour
{
	// Token: 0x17000BF7 RID: 3063
	// (get) Token: 0x060019BB RID: 6587 RVA: 0x000508B1 File Offset: 0x0004EAB1
	public GamepadType GamepadType
	{
		get
		{
			return this.m_gamepadType;
		}
	}

	// Token: 0x17000BF8 RID: 3064
	// (get) Token: 0x060019BC RID: 6588 RVA: 0x000508B9 File Offset: 0x0004EAB9
	// (set) Token: 0x060019BD RID: 6589 RVA: 0x000508C1 File Offset: 0x0004EAC1
	public int ControllerID { get; set; }

	// Token: 0x17000BF9 RID: 3065
	// (get) Token: 0x060019BE RID: 6590 RVA: 0x000508CA File Offset: 0x0004EACA
	public bool IsXboxSeries
	{
		get
		{
			return this.m_isXboxSeries;
		}
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x000508D2 File Offset: 0x0004EAD2
	private void Awake()
	{
		this.DeactivateAllImages();
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000508DC File Offset: 0x0004EADC
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

	// Token: 0x060019C1 RID: 6593 RVA: 0x000509E4 File Offset: 0x0004EBE4
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

	// Token: 0x060019C2 RID: 6594 RVA: 0x00050C8C File Offset: 0x0004EE8C
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

	// Token: 0x04001848 RID: 6216
	private static List<ActionElementMap> m_aemListHelper = new List<ActionElementMap>();

	// Token: 0x04001849 RID: 6217
	private const string DpadUp_Name = "DpadUp";

	// Token: 0x0400184A RID: 6218
	private const string DpadDown_Name = "DpadDown";

	// Token: 0x0400184B RID: 6219
	private const string DpadLeft_Name = "DpadLeft";

	// Token: 0x0400184C RID: 6220
	private const string DpadRight_Name = "DpadRight";

	// Token: 0x0400184D RID: 6221
	private const string L3_Name = "L3";

	// Token: 0x0400184E RID: 6222
	private const string R3_Name = "R3";

	// Token: 0x0400184F RID: 6223
	private const string LB_Name = "Left Button";

	// Token: 0x04001850 RID: 6224
	private const string LT_Name = "Left Trigger";

	// Token: 0x04001851 RID: 6225
	private const string RB_Name = "Right Button";

	// Token: 0x04001852 RID: 6226
	private const string RT_Name = "Right Trigger";

	// Token: 0x04001853 RID: 6227
	private const string X_Name = "X";

	// Token: 0x04001854 RID: 6228
	private const string A_Name = "A";

	// Token: 0x04001855 RID: 6229
	private const string B_Name = "B";

	// Token: 0x04001856 RID: 6230
	private const string Y_Name = "Y";

	// Token: 0x04001857 RID: 6231
	private const string TouchPad_Name = "TouchPad";

	// Token: 0x04001858 RID: 6232
	[SerializeField]
	private GamepadType m_gamepadType;

	// Token: 0x04001859 RID: 6233
	[SerializeField]
	private Image m_rbButtonImage;

	// Token: 0x0400185A RID: 6234
	[SerializeField]
	private Image m_lbButtonImage;

	// Token: 0x0400185B RID: 6235
	[SerializeField]
	private Image m_rtButtonImage;

	// Token: 0x0400185C RID: 6236
	[SerializeField]
	private Image m_ltButtonImage;

	// Token: 0x0400185D RID: 6237
	[SerializeField]
	private Image m_l3ButtonImage;

	// Token: 0x0400185E RID: 6238
	[SerializeField]
	private Image m_r3ButtonImage;

	// Token: 0x0400185F RID: 6239
	[SerializeField]
	private Image m_xButtonImage;

	// Token: 0x04001860 RID: 6240
	[SerializeField]
	private Image m_bButtonImage;

	// Token: 0x04001861 RID: 6241
	[SerializeField]
	private Image m_yButtonImage;

	// Token: 0x04001862 RID: 6242
	[SerializeField]
	private Image m_aButtonImage;

	// Token: 0x04001863 RID: 6243
	[SerializeField]
	private Image m_dpadButtonImage;

	// Token: 0x04001864 RID: 6244
	[SerializeField]
	private Image m_touchPadImage;

	// Token: 0x04001865 RID: 6245
	[SerializeField]
	private bool m_isXboxSeries;
}
