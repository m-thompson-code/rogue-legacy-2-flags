using System;

// Token: 0x02000C35 RID: 3125
public class SongType_RL
{
	// Token: 0x17001E44 RID: 7748
	// (get) Token: 0x06005ACC RID: 23244 RVA: 0x00031D13 File Offset: 0x0002FF13
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

	// Token: 0x04004A06 RID: 18950
	private static SongID[] m_typeArray;
}
