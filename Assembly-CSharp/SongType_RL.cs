using System;

// Token: 0x02000774 RID: 1908
public class SongType_RL
{
	// Token: 0x17001648 RID: 5704
	// (get) Token: 0x0600414F RID: 16719 RVA: 0x000E8386 File Offset: 0x000E6586
	public static SongID[] TypeArray
	{
		get
		{
			if (SongType_RL.m_typeArray == null)
			{
				SongType_RL.m_typeArray = (Enum.GetValues(typeof(SongID)) as SongID[]);
			}
			return SongType_RL.m_typeArray;
		}
	}

	// Token: 0x04003756 RID: 14166
	private static SongID[] m_typeArray;
}
