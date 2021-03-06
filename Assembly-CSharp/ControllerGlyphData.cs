using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine;

// Token: 0x020001DA RID: 474
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Controller/ControllerGlyphData")]
[Serializable]
public class ControllerGlyphData : ScriptableObject
{
	// Token: 0x060013AA RID: 5034 RVA: 0x0003B86C File Offset: 0x00039A6C
	public string GetGlyphName(int elementIdentifierId, AxisRange axisRange)
	{
		if (this.Glyphs == null)
		{
			return null;
		}
		foreach (ControllerGlyphData.GlyphEntry glyphEntry in this.Glyphs)
		{
			if (glyphEntry != null && glyphEntry.elementIdentifierId == elementIdentifierId)
			{
				return glyphEntry.GetGlyphName(axisRange);
			}
		}
		return null;
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x0003B8DC File Offset: 0x00039ADC
	private static List<ControllerGlyphData.GlyphEntry> GenerateDefaultGlyphs()
	{
		return new List<ControllerGlyphData.GlyphEntry>
		{
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 0,
				elementName = "Left Stick X",
				glyphNameFull = "X1_LeftStick_Glyph",
				glyphNameNeg = "X1_LeftStickLeft_Glyph",
				glyphNamePos = "X1_LeftStickRight_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 1,
				elementName = "Left Stick Y",
				glyphNameFull = "X1_LeftStick_Glyph",
				glyphNameNeg = "X1_LeftStickDown_Glyph",
				glyphNamePos = "X1_LeftStickUp_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 2,
				elementName = "Right Stick X",
				glyphNameFull = "X1_RightStick_Glyph",
				glyphNameNeg = "X1_RightStickLeft_Glyph",
				glyphNamePos = "X1_RightStickRight_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 3,
				elementName = "Right Stick Y",
				glyphNameFull = "X1_RightStick_Glyph",
				glyphNameNeg = "X1_RightStickDown_Glyph",
				glyphNamePos = "X1_RightStickUp_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 4,
				elementName = "Button A (Bottom Row 1)",
				glyphNameFull = "X1_ButtonA_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 5,
				elementName = "Button B (Bottom Row 2)",
				glyphNameFull = "X1_ButtonB_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 7,
				elementName = "Button X (Top Row 1)",
				glyphNameFull = "X1_ButtonX_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 8,
				elementName = "Button Y (Top Row 2)",
				glyphNameFull = "X1_ButtonY_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 10,
				elementName = "Left Shoulder Digital",
				glyphNameFull = "X1_LeftShoulderDigital_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 11,
				elementName = "Left Shoulder Analog",
				glyphNameFull = "X1_LeftShoulderAnalog_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 12,
				elementName = "Right Shoulder Digital",
				glyphNameFull = "X1_RightShoulderDigital_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 13,
				elementName = "Right Shoulder Analog",
				glyphNameFull = "X1_RightShoulderAnalog_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 14,
				elementName = "Back Button (Centre Row 1)",
				glyphNameFull = "X1_ButtonBack_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 15,
				elementName = "Start Button (Centre Row 2)",
				glyphNameFull = "X1_ButtonStart_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 16,
				elementName = "Guide Button (Centre Row 3)",
				glyphNameFull = "X1_ButtonGuide_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 17,
				elementName = "Left Stick Button",
				glyphNameFull = "X1_LeftStickIn_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 18,
				elementName = "Right Stick Button",
				glyphNameFull = "X1_RightStickIn_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 19,
				elementName = "Dpad Up",
				glyphNameFull = "X1_DpadUp_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 20,
				elementName = "Dpad Right",
				glyphNameFull = "X1_DpadRight_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 21,
				elementName = "Dpad Down",
				glyphNameFull = "X1_DpadDown_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 22,
				elementName = "Dpad Left",
				glyphNameFull = "X1_DpadLeft_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 23,
				elementName = "Left Stick All",
				glyphNameFull = "X1_LeftStick_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 24,
				elementName = "Right Stick All",
				glyphNameFull = "X1_RightStick_Glyph"
			},
			new ControllerGlyphData.GlyphEntry
			{
				elementIdentifierId = 25,
				elementName = "Dpad All",
				glyphNameFull = "X1_DpadAll_Glyph"
			}
		};
	}

	// Token: 0x04001386 RID: 4998
	public const int LEFTSTICK_ALL_ELEMENT_ID = 23;

	// Token: 0x04001387 RID: 4999
	public const int RIGHTSTICK_ALL_ELEMENT_ID = 24;

	// Token: 0x04001388 RID: 5000
	public const int DPAD_ALL_ELEMENT_ID = 25;

	// Token: 0x04001389 RID: 5001
	public GamepadType GamepadType;

	// Token: 0x0400138A RID: 5002
	public string SpriteAssetName;

	// Token: 0x0400138B RID: 5003
	public HardwareJoystickMap[] JoystickMaps;

	// Token: 0x0400138C RID: 5004
	public TextAsset ElementIDTextAsset;

	// Token: 0x0400138D RID: 5005
	public List<ControllerGlyphData.GlyphEntry> Glyphs = ControllerGlyphData.GenerateDefaultGlyphs();

	// Token: 0x02000AFB RID: 2811
	[Serializable]
	public class GlyphEntry
	{
		// Token: 0x06005B06 RID: 23302 RVA: 0x00157F6C File Offset: 0x0015616C
		public string GetGlyphName(AxisRange axisRange)
		{
			switch (axisRange)
			{
			case AxisRange.Full:
				return this.glyphNameFull;
			case AxisRange.Positive:
				if (!(this.glyphNamePos != ""))
				{
					return this.glyphNameFull;
				}
				return this.glyphNamePos;
			case AxisRange.Negative:
				if (!(this.glyphNameNeg != ""))
				{
					return this.glyphNameFull;
				}
				return this.glyphNameNeg;
			default:
				return "";
			}
		}

		// Token: 0x04004AE1 RID: 19169
		public int elementIdentifierId = -1;

		// Token: 0x04004AE2 RID: 19170
		public string elementName = "";

		// Token: 0x04004AE3 RID: 19171
		public string glyphNameFull = "";

		// Token: 0x04004AE4 RID: 19172
		public string glyphNamePos = "";

		// Token: 0x04004AE5 RID: 19173
		public string glyphNameNeg = "";
	}
}
