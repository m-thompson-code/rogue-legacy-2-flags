using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class CaveLanternPostProcessingController : MonoBehaviour
{
	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0000DFF8 File Offset: 0x0000C1F8
	public static IRelayLink OnAwakeRelay
	{
		get
		{
			return CaveLanternPostProcessingController.m_onAwakeRelay;
		}
	}

	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x06001B07 RID: 6919 RVA: 0x0000DFFF File Offset: 0x0000C1FF
	// (set) Token: 0x06001B08 RID: 6920 RVA: 0x0000E006 File Offset: 0x0000C206
	public static CaveLanternPostProcessingController Instance { get; private set; }

	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00094020 File Offset: 0x00092220
	private float DarknessAmountWhenFullyLit
	{
		get
		{
			float num = 0.45f;
			float num2 = Mathf.Min((float)SaveManager.PlayerSaveData.GetBurden(BurdenType.EnemyProjectiles).CurrentLevel * 0.05f, 0.25f);
			return num / (1f + num2);
		}
	}

	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x06001B0A RID: 6922 RVA: 0x00094060 File Offset: 0x00092260
	private float SoftnessWhenFullyLit
	{
		get
		{
			float num = 0.31f;
			float num2 = Mathf.Min((float)SaveManager.PlayerSaveData.GetBurden(BurdenType.EnemyProjectiles).CurrentLevel * 0.05f, 0.25f);
			return num / (1f + num2);
		}
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000940A0 File Offset: 0x000922A0
	private void Awake()
	{
		if (!CaveLanternPostProcessingController.Instance)
		{
			CaveLanternPostProcessingController.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.m_profile = ScriptableObject.CreateInstance<MobilePostProcessingProfile>();
		this.m_profile.EnableCircularDarknessEffect = true;
		this.m_profile.OverrideCircDarknessColor = true;
		this.m_profile.OverrideCircDarknessAmount = true;
		this.m_profile.OverrideCircDarknessSoftness = true;
		CaveLanternPostProcessingController.m_onAwakeRelay.Dispatch();
		CaveLanternPostProcessingController.m_onAwakeRelay.RemoveAll(true, true);
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x00094120 File Offset: 0x00092320
	private void OnEnable()
	{
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.CaveLantern) == 0)
		{
			this.m_profile.CircDarknessColor = Heirloom_EV.CAVE_LANTERN_DARKNESS_COLOR_DIM;
			this.m_profile.CircDarknessAmount = 0.73f;
			this.m_profile.CircDarknessSoftness = 0.13f;
		}
		else
		{
			this.m_profile.CircDarknessColor = Heirloom_EV.CAVE_LANTERN_DARKNESS_COLOR_LIT;
			this.m_profile.CircDarknessAmount = this.DarknessAmountWhenFullyLit;
			this.m_profile.CircDarknessSoftness = this.SoftnessWhenFullyLit;
		}
		CameraController.ForegroundPostProcessing.AddDimensionOverride(this.m_profile);
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x0000E00E File Offset: 0x0000C20E
	private void OnDisable()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing)
		{
			CameraController.ForegroundPostProcessing.RemoveDimensionOverride(this.m_profile);
		}
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x0000E033 File Offset: 0x0000C233
	private void OnDestroy()
	{
		if (CaveLanternPostProcessingController.Instance == this)
		{
			CaveLanternPostProcessingController.Instance = null;
		}
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x0000E048 File Offset: 0x0000C248
	public static void EnableCaveLanternEffect()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing)
		{
			CameraController.ForegroundPostProcessing.AddDimensionOverride(CaveLanternPostProcessingController.Instance.m_profile);
		}
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x0000E071 File Offset: 0x0000C271
	public static void DisableCaveLanternEffect()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing)
		{
			CameraController.ForegroundPostProcessing.RemoveDimensionOverride(CaveLanternPostProcessingController.Instance.m_profile);
		}
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x000941B0 File Offset: 0x000923B0
	public static void SetDimnessPercent(float percent)
	{
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.CaveLantern) == 0)
		{
			CaveLanternPostProcessingController.Instance.m_profile.CircDarknessColor = Heirloom_EV.CAVE_LANTERN_DARKNESS_COLOR_DIM * percent;
			CaveLanternPostProcessingController.Instance.m_profile.CircDarknessAmount = 0.73f * percent;
			CaveLanternPostProcessingController.Instance.m_profile.CircDarknessSoftness = 0.13f * percent;
		}
		else
		{
			CaveLanternPostProcessingController.Instance.m_profile.CircDarknessColor = Heirloom_EV.CAVE_LANTERN_DARKNESS_COLOR_LIT * percent;
			CaveLanternPostProcessingController.Instance.m_profile.CircDarknessAmount = CaveLanternPostProcessingController.Instance.DarknessAmountWhenFullyLit * percent;
			CaveLanternPostProcessingController.Instance.m_profile.CircDarknessSoftness = CaveLanternPostProcessingController.Instance.SoftnessWhenFullyLit * percent;
		}
		CameraController.ForegroundPostProcessing.ForceDirty();
	}

	// Token: 0x04001929 RID: 6441
	private MobilePostProcessingProfile m_profile;

	// Token: 0x0400192A RID: 6442
	private static Relay m_onAwakeRelay = new Relay();
}
