using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class CaveLanternPostProcessingController : MonoBehaviour
{
	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x06001288 RID: 4744 RVA: 0x0003681D File Offset: 0x00034A1D
	public static IRelayLink OnAwakeRelay
	{
		get
		{
			return CaveLanternPostProcessingController.m_onAwakeRelay;
		}
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06001289 RID: 4745 RVA: 0x00036824 File Offset: 0x00034A24
	// (set) Token: 0x0600128A RID: 4746 RVA: 0x0003682B File Offset: 0x00034A2B
	public static CaveLanternPostProcessingController Instance { get; private set; }

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x0600128B RID: 4747 RVA: 0x00036834 File Offset: 0x00034A34
	private float DarknessAmountWhenFullyLit
	{
		get
		{
			float num = 0.45f;
			float num2 = Mathf.Min((float)SaveManager.PlayerSaveData.GetBurden(BurdenType.EnemyProjectiles).CurrentLevel * 0.05f, 0.25f);
			return num / (1f + num2);
		}
	}

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x0600128C RID: 4748 RVA: 0x00036874 File Offset: 0x00034A74
	private float SoftnessWhenFullyLit
	{
		get
		{
			float num = 0.31f;
			float num2 = Mathf.Min((float)SaveManager.PlayerSaveData.GetBurden(BurdenType.EnemyProjectiles).CurrentLevel * 0.05f, 0.25f);
			return num / (1f + num2);
		}
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x000368B4 File Offset: 0x00034AB4
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

	// Token: 0x0600128E RID: 4750 RVA: 0x00036934 File Offset: 0x00034B34
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

	// Token: 0x0600128F RID: 4751 RVA: 0x000369C3 File Offset: 0x00034BC3
	private void OnDisable()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing)
		{
			CameraController.ForegroundPostProcessing.RemoveDimensionOverride(this.m_profile);
		}
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x000369E8 File Offset: 0x00034BE8
	private void OnDestroy()
	{
		if (CaveLanternPostProcessingController.Instance == this)
		{
			CaveLanternPostProcessingController.Instance = null;
		}
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x000369FD File Offset: 0x00034BFD
	public static void EnableCaveLanternEffect()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing)
		{
			CameraController.ForegroundPostProcessing.AddDimensionOverride(CaveLanternPostProcessingController.Instance.m_profile);
		}
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x00036A26 File Offset: 0x00034C26
	public static void DisableCaveLanternEffect()
	{
		if (CameraController.IsInstantiated && CameraController.ForegroundPostProcessing)
		{
			CameraController.ForegroundPostProcessing.RemoveDimensionOverride(CaveLanternPostProcessingController.Instance.m_profile);
		}
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x00036A50 File Offset: 0x00034C50
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

	// Token: 0x040012F6 RID: 4854
	private MobilePostProcessingProfile m_profile;

	// Token: 0x040012F7 RID: 4855
	private static Relay m_onAwakeRelay = new Relay();
}
