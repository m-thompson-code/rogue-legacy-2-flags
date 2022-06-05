using System;

// Token: 0x02000BF8 RID: 3064
public class GamepadType_RL
{
	// Token: 0x17001E2D RID: 7725
	// (get) Token: 0x06005A7F RID: 23167 RVA: 0x000318C4 File Offset: 0x0002FAC4
	public static GamepadType[] GamepadTypeArray
	{
		get
		{
			if (GamepadType_RL.m_gamepadTypeArray == null)
			{
				GamepadType_RL.m_gamepadTypeArray = (Enum.GetValues(typeof(GamepadType)) as GamepadType[]);
			}
			return GamepadType_RL.m_gamepadTypeArray;
		}
	}

	// Token: 0x040046DE RID: 18142
	private static GamepadType[] m_gamepadTypeArray;
}
