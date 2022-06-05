using System;

// Token: 0x020002D1 RID: 721
public class GameModeType_RL
{
	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06001C8B RID: 7307 RVA: 0x0005CD00 File Offset: 0x0005AF00
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

	// Token: 0x040019EF RID: 6639
	private static GameModeType[] m_typeArray;
}
