using System;
using System.Collections;
using Steamworks;
using UnityEngine;

// Token: 0x020006B0 RID: 1712
public class StoreAPIManager : MonoBehaviour
{
	// Token: 0x1700158C RID: 5516
	// (get) Token: 0x06003F0D RID: 16141 RVA: 0x000E04E1 File Offset: 0x000DE6E1
	// (set) Token: 0x06003F0E RID: 16142 RVA: 0x000E04E8 File Offset: 0x000DE6E8
	public static StoreAPIManager.StoreInitState InitState { get; private set; }

	// Token: 0x1700158D RID: 5517
	// (get) Token: 0x06003F0F RID: 16143 RVA: 0x000E04F0 File Offset: 0x000DE6F0
	// (set) Token: 0x06003F10 RID: 16144 RVA: 0x000E04F7 File Offset: 0x000DE6F7
	public static StoreAPIManager Instance { get; private set; }

	// Token: 0x06003F11 RID: 16145 RVA: 0x000E04FF File Offset: 0x000DE6FF
	private void Awake()
	{
		StoreAPIManager.Instance = this;
		base.StartCoroutine(this.InitializeStoreAPI());
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x000E0514 File Offset: 0x000DE714
	private IEnumerator InitializeStoreAPI()
	{
		StoreAPIManager.m_storeInstance = new StoreAPIManager.SteamStore();
		yield return StoreAPIManager.m_storeInstance.Init();
		yield break;
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x000E051C File Offset: 0x000DE71C
	public static string GetPlatformDirectoryName()
	{
		return "Steam";
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x000E0523 File Offset: 0x000DE723
	public static float GetInitTimeout()
	{
		return StoreAPIManager.m_storeInstance.InitTimeout;
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x000E052F File Offset: 0x000DE72F
	public static void GetInitFailureMessage(out string title, out string description)
	{
		StoreAPIManager.m_storeInstance.GetInitFailureMessage(out title, out description);
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x000E053D File Offset: 0x000DE73D
	public static string GetUserIDString()
	{
		return StoreAPIManager.m_storeInstance.GetUserID();
	}

	// Token: 0x06003F17 RID: 16151 RVA: 0x000E054C File Offset: 0x000DE74C
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

	// Token: 0x06003F18 RID: 16152 RVA: 0x000E05B2 File Offset: 0x000DE7B2
	public static void GiveAllUnlockedAchievements()
	{
		StoreAPIManager.m_storeInstance.GiveAllUnlockedAchievements();
		StoreAPIManager.CheckForAllAchievementsAchievement();
	}

	// Token: 0x06003F19 RID: 16153 RVA: 0x000E05C4 File Offset: 0x000DE7C4
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

	// Token: 0x04002EDD RID: 11997
	private static StoreAPIManager.IStoreAPI m_storeInstance;

	// Token: 0x02000E17 RID: 3607
	public enum StoreInitState
	{
		// Token: 0x040056C2 RID: 22210
		LoggingIn,
		// Token: 0x040056C3 RID: 22211
		Succeeded,
		// Token: 0x040056C4 RID: 22212
		Failed
	}

	// Token: 0x02000E18 RID: 3608
	private interface IStoreAPI
	{
		// Token: 0x1700231D RID: 8989
		// (get) Token: 0x06006B61 RID: 27489
		StoreType StoreType { get; }

		// Token: 0x1700231E RID: 8990
		// (get) Token: 0x06006B62 RID: 27490
		float InitTimeout { get; }

		// Token: 0x06006B63 RID: 27491
		IEnumerator Init();

		// Token: 0x06006B64 RID: 27492
		void GetInitFailureMessage(out string title, out string description);

		// Token: 0x06006B65 RID: 27493
		string GetUserID();

		// Token: 0x06006B66 RID: 27494
		void GiveAchievement(AchievementType achievementType);

		// Token: 0x06006B67 RID: 27495
		void GiveAllUnlockedAchievements();
	}

	// Token: 0x02000E19 RID: 3609
	private class NoStore : StoreAPIManager.IStoreAPI
	{
		// Token: 0x1700231F RID: 8991
		// (get) Token: 0x06006B68 RID: 27496 RVA: 0x001916AA File Offset: 0x0018F8AA
		public StoreType StoreType
		{
			get
			{
				return StoreType.None;
			}
		}

		// Token: 0x17002320 RID: 8992
		// (get) Token: 0x06006B69 RID: 27497 RVA: 0x001916AD File Offset: 0x0018F8AD
		public float InitTimeout
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06006B6A RID: 27498 RVA: 0x001916B4 File Offset: 0x0018F8B4
		public IEnumerator Init()
		{
			StoreAPIManager.InitState = StoreAPIManager.StoreInitState.Succeeded;
			yield break;
		}

		// Token: 0x06006B6B RID: 27499 RVA: 0x001916BC File Offset: 0x0018F8BC
		public void GetInitFailureMessage(out string title, out string description)
		{
			throw new Exception("This should never get called");
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x001916C8 File Offset: 0x0018F8C8
		public string GetUserID()
		{
			return string.Empty;
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x001916CF File Offset: 0x0018F8CF
		public void GiveAchievement(AchievementType achievementType)
		{
		}

		// Token: 0x06006B6E RID: 27502 RVA: 0x001916D1 File Offset: 0x0018F8D1
		public void GiveAllUnlockedAchievements()
		{
		}
	}

	// Token: 0x02000E1A RID: 3610
	private class SteamStore : StoreAPIManager.IStoreAPI
	{
		// Token: 0x17002321 RID: 8993
		// (get) Token: 0x06006B70 RID: 27504 RVA: 0x001916DB File Offset: 0x0018F8DB
		public StoreType StoreType
		{
			get
			{
				return StoreType.Steam;
			}
		}

		// Token: 0x17002322 RID: 8994
		// (get) Token: 0x06006B71 RID: 27505 RVA: 0x001916DE File Offset: 0x0018F8DE
		public float InitTimeout
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x06006B72 RID: 27506 RVA: 0x001916E5 File Offset: 0x0018F8E5
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

		// Token: 0x06006B73 RID: 27507 RVA: 0x001916F4 File Offset: 0x0018F8F4
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

		// Token: 0x06006B74 RID: 27508 RVA: 0x00191748 File Offset: 0x0018F948
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

		// Token: 0x06006B75 RID: 27509 RVA: 0x0019177F File Offset: 0x0018F97F
		private void OnUserAchievementStored(UserAchievementStored_t data)
		{
		}

		// Token: 0x06006B76 RID: 27510 RVA: 0x00191781 File Offset: 0x0018F981
		public void GetInitFailureMessage(out string title, out string description)
		{
			title = LocalizationManager.GetString("LOC_ID_CONFIRM_MENU_STEAM_FAIL_TITLE_1", false, false);
			description = LocalizationManager.GetString("LOC_ID_CONFIRM_MENU_STEAM_FAIL_DESCRIPTION_1", false, false);
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x001917A0 File Offset: 0x0018F9A0
		public string GetUserID()
		{
			return SteamUser.GetSteamID().ToString();
		}

		// Token: 0x06006B78 RID: 27512 RVA: 0x001917C0 File Offset: 0x0018F9C0
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

		// Token: 0x06006B79 RID: 27513 RVA: 0x001917F8 File Offset: 0x0018F9F8
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

		// Token: 0x040056C5 RID: 22213
		private Callback<UserStatsReceived_t> m_userStatsRecievedCallback;

		// Token: 0x040056C6 RID: 22214
		private Callback<UserStatsStored_t> m_userStatsStoredCallback;

		// Token: 0x040056C7 RID: 22215
		private Callback<UserAchievementStored_t> m_userAchievementStoredCallback;

		// Token: 0x040056C8 RID: 22216
		private bool m_statsReceieved;

		// Token: 0x040056C9 RID: 22217
		private const ulong GAME_ID = 1253920UL;
	}
}
