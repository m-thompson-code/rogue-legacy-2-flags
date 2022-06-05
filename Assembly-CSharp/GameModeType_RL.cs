using System;

// Token: 0x020004C6 RID: 1222
public class GameModeType_RL
{
	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x06002755 RID: 10069 RVA: 0x00016231 File Offset: 0x00014431
	public static GameModeType[] TypeArray
	{
		get
		{
			if (GameModeType_RL.m_typeArray == null)
			{
				GameModeType_RL.m_typeArray = (Enum.GetValues(typeof(GameModeType)) as GameModeType[]);
			}
			return GameModeType_RL.m_typeArray;
		}
	}

	// Token: 0x040021F6 RID: 8694
	private static GameModeType[] m_typeArray;
}
