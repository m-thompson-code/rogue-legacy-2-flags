using System;
using System.Collections;
using Steamworks;
using UnityEngine;

// Token: 0x02000B51 RID: 2897
public class StoreAPIManager : MonoBehaviour
{
	// Token: 0x17001D78 RID: 7544
	// (get) Token: 0x06005815 RID: 22549 RVA: 0x0002FE64 File Offset: 0x0002E064
	// (set) Token: 0x06005816 RID: 22550 RVA: 0x0002FE6B File Offset: 0x0002E06B
	public static StoreAPIManager.StoreInitState InitState { get; private set; }

	// Token: 0x17001D79 RID: 7545
	// (get) Token: 0x06005817 RID: 22551 RVA: 0x0002FE73 File Offset: 0x0002E073
	// (set) Token: 0x06005818 RID: 22552 RVA: 0x0002FE7A File Offset: 0x0002E07A
	public static StoreAPIManager Instance { get; private set; }

	// Token: 0x06005819 RID: 22553 RVA: 0x0002FE82 File Offset: 0x0002E082
	private void Awake()
	{
		StoreAPIManager.Instance = this;
		base.StartCoroutine(this.InitializeStoreAPI());
	}

	// Token: 0x0600581A RID: 22554 RVA: 0x0002FE97 File Offset: 0x0002E097
	private IEnumerator InitializeStoreAPI()
	{
		StoreAPIManager.m_storeInstance = new StoreAPIManager.SteamStore();
		yield return StoreAPIManager.m_storeInstance.Init();
		yield break;
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x0002FE9F File Offset: 0x0002E09F
	public static string GetPlatformDirectoryName()
	{
		return "Steam";
	}

	// Token: 0x0600581C RID: 22556 RVA: 0x0002FEA6 File Offset: 0x0002E0A6
	public static float GetInitTimeout()
	{
		return StoreAPIManager.m_storeInstance.InitTimeout;
	}

	// Token: 0x0600581D RID: 22557 RVA: 0x0002FEB2 File Offset: 0x0002E0B2
	public static void GetInitFailureMessage(out string title, out string description)
	{
		StoreAPIManager.m_storeInstance.GetInitFailureMessage(out title, out description);
	}

	// Token: 0x0600581E RID: 22558 RVA: 0x0002FEC0 File Offset: 0x0002E0C0
	public static string GetUserIDString()
	{
		return StoreAPIManager.m_storeInstance.GetUserID();
	}

	// Token: 0x0600581F RID: 22559 RVA: 0x001507AC File Offset: 0x0014E9AC
	public static void GiveAchievement(AchievementType achievementType, StoreType storeMask)
	{
		if (SaveManager.PlayerSaveData.DisableAchievementUnlocks || SaveManager.ModeSaveData.DisableAchievementUnlocks || SaveManager.EquipmentSaveData.DisableAchievementUnlocks)
		{
			return;
		}
		if (AchievementLibrary.GetAchievementData(achievementType) == null)
		{
			return;
		}
		if (!SaveManager.ModeSaveData.GetAchievementUnlocked(achievementType))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(achievementType, true, false);
			StoreAPIManager.m_storeInstance.GiveAchievement(achievementType);
			StoreAPIManager.CheckForAllAchievementsAchievement();
		}
	}

	// Token: 0x06005820 RID: 22560 RVA: 0x0002FECC File Offset: 0x0002E0CC
	public static void GiveAllUnlockedAchievements()
	{
		StoreAPIManager.m_storeInstance.GiveAllUnlockedAchievements();
		StoreAPIManager.CheckForAllAchievementsAchievement();
	}

	// Token: 0x06005821 RID: 22561 RVA: 0x00150814 File Offset: 0x0014EA14
	private static void CheckForAllAchievementsAchievement()
	{
		bool flag = true;
		foreach (AchievementType achievementType in AchievementType_RL.TypeArray)
		{
			if (achievementType != AchievementType.None && achievementType != AchievementType.AllAchievements && AchievementLibrary.GetAchievementData(achievementType) != null && !SaveManager.ModeSaveData.GetAchievementUnlocked(achievementType))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.AllAchievements, true, false);
			StoreAPIManager.m_storeInstance.GiveAchievement(AchievementType.AllAchievements);
		}
	}

	// Token: 0x04004116 RID: 16662
	private static StoreAPIManager.IStoreAPI m_storeInstance;

	// Token: 0x02000B52 RID: 2898
	public enum StoreInitState
	{
		// Token: 0x04004118 RID: 16664
		LoggingIn,
		// Token: 0x04004119 RID: 16665
		Succeeded,
		// Token: 0x0400411A RID: 16666
		Failed
	}

	// Token: 0x02000B53 RID: 2899
	private interface IStoreAPI
	{
		// Token: 0x17001D7A RID: 7546
		// (get) Token: 0x06005823 RID: 22563
		StoreType StoreType { get; }

		// Token: 0x17001D7B RID: 7547
		// (get) Token: 0x06005824 RID: 22564
		float InitTimeout { get; }

		// Token: 0x06005825 RID: 22565
		IEnumerator Init();

		// Token: 0x06005826 RID: 22566
		void GetInitFailureMessage(out string title, out string description);

		// Token: 0x06005827 RID: 22567
		string GetUserID();

		// Token: 0x06005828 RID: 22568
		void GiveAchievement(AchievementType achievementType);

		// Token: 0x06005829 RID: 22569
		void GiveAllUnlockedAchievements();
	}

	// Token: 0x02000B54 RID: 2900
	private class NoStore : StoreAPIManager.IStoreAPI
	{
		// Token: 0x17001D7C RID: 7548
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public StoreType StoreType
		{
			get
			{
				return StoreType.None;
			}
		}

		// Token: 0x17001D7D RID: 7549
		// (get) Token: 0x0600582B RID: 22571 RVA: 0x00003CCB File Offset: 0x00001ECB
		public float InitTimeout
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x0002FEDD File Offset: 0x0002E0DD
		public IEnumerator Init()
		{
			StoreAPIManager.InitState = StoreAPIManager.StoreInitState.Succeeded;
			yield break;
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x0002FEE5 File Offset: 0x0002E0E5
		public void GetInitFailureMessage(out string title, out string description)
		{
			throw new Exception("This should never get called");
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x0002FEF1 File Offset: 0x0002E0F1
		public string GetUserID()
		{
			return string.Empty;
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x00002FCA File Offset: 0x000011CA
		public void GiveAchievement(AchievementType achievementType)
		{
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x00002FCA File Offset: 0x000011CA
		public void GiveAllUnlockedAchievements()
		{
		}
	}

	// Token: 0x02000B56 RID: 2902
	private class SteamStore : StoreAPIManager.IStoreAPI
	{
		// Token: 0x17001D80 RID: 7552
		// (get) Token: 0x06005838 RID: 22584 RVA: 0x00003DA1 File Offset: 0x00001FA1
		public StoreType StoreType
		{
			get
			{
				return StoreType.Steam;
			}
		}

		// Token: 0x17001D81 RID: 7553
		// (get) Token: 0x06005839 RID: 22585 RVA: 0x00003C54 File Offset: 0x00001E54
		public float InitTimeout
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x0002FF0F File Offset: 0x0002E10F
		public IEnumerator Init()
		{
			while (!SteamManager.Initialized)
			{
				yield return null;
			}
			this.m_userStatsRecievedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnStatsReceived));
			this.m_userStatsStoredCallback = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnStatsStored));
			this.m_userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
			if (!SteamUserStats.RequestCurrentStats())
			{
				Debug.Log("RequestCurrentStats failed");
			}
			StoreAPIManager.InitState = StoreAPIManager.StoreInitState.Succeeded;
			yield break;
		}

		// Token: 0x0600583B RID: 22587 RVA: 0x001508AC File Offset: 0x0014EAAC
		private void OnStatsReceived(UserStatsReceived_t data)
		{
			if (data.m_nGameID == 1253920UL)
			{
				if (data.m_eResult == EResult.k_EResultOK)
				{
					this.m_statsReceieved = true;
					Debug.Log("Steam Stats Received");
					return;
				}
				Debug.LogFormat("RequestCurrentStats failed with error code: {0}", new object[]
				{
					data.m_eResult
				});
			}
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x0002FF1E File Offset: 0x0002E11E
		private void OnStatsStored(UserStatsStored_t data)
		{
			if (data.m_nGameID == 1253920UL && data.m_eResult != EResult.k_EResultOK)
			{
				Debug.LogFormat("StoreStats callback failed with error code {0}", new object[]
				{
					data.m_eResult
				});
			}
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x00002FCA File Offset: 0x000011CA
		private void OnUserAchievementStored(UserAchievementStored_t data)
		{
		}

		// Token: 0x0600583E RID: 22590 RVA: 0x0002FF55 File Offset: 0x0002E155
		public void GetInitFailureMessage(out string title, out string description)
		{
			title = LocalizationManager.GetString("LOC_ID_CONFIRM_MENU_STEAM_FAIL_TITLE_1", false, false);
			description = LocalizationManager.GetString("LOC_ID_CONFIRM_MENU_STEAM_FAIL_DESCRIPTION_1", false, false);
		}

		// Token: 0x0600583F RID: 22591 RVA: 0x00150900 File Offset: 0x0014EB00
		public string GetUserID()
		{
			return SteamUser.GetSteamID().ToString();
		}

		// Token: 0x06005840 RID: 22592 RVA: 0x0002FF73 File Offset: 0x0002E173
		public void GiveAchievement(AchievementType achievementType)
		{
			if (SteamManager.Initialized && this.m_statsReceieved)
			{
				SteamUserStats.SetAchievement(achievementType.ToString());
				if (!SteamUserStats.StoreStats())
				{
					Debug.Log("Failed to StoreStats");
				}
			}
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x00150920 File Offset: 0x0014EB20
		public void GiveAllUnlockedAchievements()
		{
			if (SteamManager.Initialized && this.m_statsReceieved)
			{
				bool flag = false;
				foreach (AchievementType achievementType in AchievementType_RL.TypeArray)
				{
					if (achievementType != AchievementType.None && AchievementLibrary.GetAchievementData(achievementType) != null && SaveManager.ModeSaveData.GetAchievementUnlocked(achievementType))
					{
						SteamUserStats.SetAchievement(achievementType.ToString());
						flag = true;
					}
				}
				if (flag && !SteamUserStats.StoreStats())
				{
					Debug.Log("Failed to StoreStats");
				}
			}
		}

		// Token: 0x0400411D RID: 16669
		private Callback<UserStatsReceived_t> m_userStatsRecievedCallback;

		// Token: 0x0400411E RID: 16670
		private Callback<UserStatsStored_t> m_userStatsStoredCallback;

		// Token: 0x0400411F RID: 16671
		private Callback<UserAchievementStored_t> m_userAchievementStoredCallback;

		// Token: 0x04004120 RID: 16672
		private bool m_statsReceieved;

		// Token: 0x04004121 RID: 16673
		private const ulong GAME_ID = 1253920UL;
	}
}
