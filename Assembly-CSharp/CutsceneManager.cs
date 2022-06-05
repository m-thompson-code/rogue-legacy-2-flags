using System;
using System.Collections.Generic;

// Token: 0x02000B1B RID: 2843
public class CutsceneManager
{
	// Token: 0x17001D01 RID: 7425
	// (get) Token: 0x060055AB RID: 21931 RVA: 0x0002E8AF File Offset: 0x0002CAAF
	public static bool IsCutsceneActive
	{
		get
		{
			return CutsceneManager.m_playCutscene_STATIC;
		}
	}

	// Token: 0x17001D02 RID: 7426
	// (get) Token: 0x060055AC RID: 21932 RVA: 0x0002E8B6 File Offset: 0x0002CAB6
	public static Tunnel ExitRoomTunnel
	{
		get
		{
			return CutsceneManager.m_exitRoomTunnel_STATIC;
		}
	}

	// Token: 0x17001D03 RID: 7427
	// (get) Token: 0x060055AD RID: 21933 RVA: 0x0002E8BD File Offset: 0x0002CABD
	public static PlayerSaveFlag CutsceneSaveFlag
	{
		get
		{
			return CutsceneManager.m_cutsceneSaveFlag_STATIC;
		}
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
	public static void InitializeCutscene(PlayerSaveFlag saveFlag, Tunnel exitRoomTunnel)
	{
		CutsceneManager.m_playCutscene_STATIC = true;
		CutsceneManager.m_cutsceneSaveFlag_STATIC = saveFlag;
		CutsceneManager.m_exitRoomTunnel_STATIC = exitRoomTunnel;
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x0002E8D8 File Offset: 0x0002CAD8
	public static void ResetCutscene()
	{
		CutsceneManager.m_exitRoomTunnel_STATIC = null;
		CutsceneManager.m_playCutscene_STATIC = false;
		CutsceneManager.m_cutsceneSaveFlag_STATIC = PlayerSaveFlag.None;
	}

	// Token: 0x060055B0 RID: 21936 RVA: 0x00143E48 File Offset: 0x00142048
	public static void SetTraitsEnabled(bool enabled)
	{
		if (!enabled)
		{
			using (List<BaseTrait>.Enumerator enumerator = TraitManager.ActiveTraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BaseTrait baseTrait = enumerator.Current;
					if (baseTrait.TraitMask)
					{
						baseTrait.DisableOnCutscene();
						baseTrait.gameObject.SetActive(false);
					}
				}
				return;
			}
		}
		foreach (BaseTrait baseTrait2 in TraitManager.ActiveTraitList)
		{
			if (!baseTrait2.gameObject.activeSelf)
			{
				baseTrait2.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04003F95 RID: 16277
	private static Tunnel m_exitRoomTunnel_STATIC;

	// Token: 0x04003F96 RID: 16278
	private static PlayerSaveFlag m_cutsceneSaveFlag_STATIC;

	// Token: 0x04003F97 RID: 16279
	private static bool m_playCutscene_STATIC;
}
