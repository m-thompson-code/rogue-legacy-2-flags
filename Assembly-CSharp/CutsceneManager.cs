using System;
using System.Collections.Generic;

// Token: 0x02000692 RID: 1682
public class CutsceneManager
{
	// Token: 0x17001535 RID: 5429
	// (get) Token: 0x06003D16 RID: 15638 RVA: 0x000D3D68 File Offset: 0x000D1F68
	public static bool IsCutsceneActive
	{
		get
		{
			return CutsceneManager.m_playCutscene_STATIC;
		}
	}

	// Token: 0x17001536 RID: 5430
	// (get) Token: 0x06003D17 RID: 15639 RVA: 0x000D3D6F File Offset: 0x000D1F6F
	public static Tunnel ExitRoomTunnel
	{
		get
		{
			return CutsceneManager.m_exitRoomTunnel_STATIC;
		}
	}

	// Token: 0x17001537 RID: 5431
	// (get) Token: 0x06003D18 RID: 15640 RVA: 0x000D3D76 File Offset: 0x000D1F76
	public static PlayerSaveFlag CutsceneSaveFlag
	{
		get
		{
			return CutsceneManager.m_cutsceneSaveFlag_STATIC;
		}
	}

	// Token: 0x06003D19 RID: 15641 RVA: 0x000D3D7D File Offset: 0x000D1F7D
	public static void InitializeCutscene(PlayerSaveFlag saveFlag, Tunnel exitRoomTunnel)
	{
		CutsceneManager.m_playCutscene_STATIC = true;
		CutsceneManager.m_cutsceneSaveFlag_STATIC = saveFlag;
		CutsceneManager.m_exitRoomTunnel_STATIC = exitRoomTunnel;
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x000D3D91 File Offset: 0x000D1F91
	public static void ResetCutscene()
	{
		CutsceneManager.m_exitRoomTunnel_STATIC = null;
		CutsceneManager.m_playCutscene_STATIC = false;
		CutsceneManager.m_cutsceneSaveFlag_STATIC = PlayerSaveFlag.None;
	}

	// Token: 0x06003D1B RID: 15643 RVA: 0x000D3DA8 File Offset: 0x000D1FA8
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

	// Token: 0x04002DCA RID: 11722
	private static Tunnel m_exitRoomTunnel_STATIC;

	// Token: 0x04002DCB RID: 11723
	private static PlayerSaveFlag m_cutsceneSaveFlag_STATIC;

	// Token: 0x04002DCC RID: 11724
	private static bool m_playCutscene_STATIC;
}
