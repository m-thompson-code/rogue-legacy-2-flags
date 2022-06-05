using System;

// Token: 0x0200073A RID: 1850
public class GamepadType_RL
{
	// Token: 0x17001631 RID: 5681
	// (get) Token: 0x06004102 RID: 16642 RVA: 0x000E6458 File Offset: 0x000E4658
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

	// Token: 0x04003462 RID: 13410
	private static GamepadType[] m_gamepadTypeArray;
}
