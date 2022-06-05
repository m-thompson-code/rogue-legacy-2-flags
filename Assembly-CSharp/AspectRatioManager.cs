using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class AspectRatioManager : MonoBehaviour
{
	// Token: 0x17001CCE RID: 7374
	// (get) Token: 0x060054D5 RID: 21717 RVA: 0x0002E09C File Offset: 0x0002C29C
	public static AspectRatioManager Instance
	{
		get
		{
			if (!AspectRatioManager.m_instance)
			{
				AspectRatioManager.m_instance = CDGHelper.FindStaticInstance<AspectRatioManager>(false);
				AspectRatioManager.IsInitialized = true;
			}
			return AspectRatioManager.m_instance;
		}
	}

	// Token: 0x17001CCF RID: 7375
	// (get) Token: 0x060054D6 RID: 21718 RVA: 0x0002E0C0 File Offset: 0x0002C2C0
	public static bool Disable_16_9_Aspect
	{
		get
		{
			return !AspectRatioManager.ForceEnable_16_9 && SaveManager.ConfigData.Disable_16_9;
		}
	}

	// Token: 0x17001CD0 RID: 7376
	// (get) Token: 0x060054D7 RID: 21719 RVA: 0x0002E0D5 File Offset: 0x0002C2D5
	public static float CurrentGameAspectRatio
	{
		get
		{
			return AspectRatioManager.Instance.m_currentAspectRatio;
		}
	}

	// Token: 0x17001CD1 RID: 7377
	// (get) Token: 0x060054D8 RID: 21720 RVA: 0x00140F84 File Offset: 0x0013F184
	public static float CurrentScreenAspectRatio
	{
		get
		{
			return (float)GameResolutionManager.Resolution.x / (float)GameResolutionManager.Resolution.y;
		}
	}

	// Token: 0x17001CD2 RID: 7378
	// (get) Token: 0x060054D9 RID: 21721 RVA: 0x00140FB0 File Offset: 0x0013F1B0
	public static bool IsScreen_16_9_AspectRatio
	{
		get
		{
			float b = (float)((int)(AspectRatioManager.CurrentScreenAspectRatio * 100f)) / 100f;
			return Mathf.Approximately(1.77f, b);
		}
	}

	// Token: 0x17001CD3 RID: 7379
	// (get) Token: 0x060054DA RID: 21722 RVA: 0x0002E0E1 File Offset: 0x0002C2E1
	// (set) Token: 0x060054DB RID: 21723 RVA: 0x0002E0E8 File Offset: 0x0002C2E8
	public static bool ForceEnable_16_9 { get; set; }

	// Token: 0x17001CD4 RID: 7380
	// (get) Token: 0x060054DC RID: 21724 RVA: 0x0002E0F0 File Offset: 0x0002C2F0
	// (set) Token: 0x060054DD RID: 21725 RVA: 0x0002E0F7 File Offset: 0x0002C2F7
	public static bool IsInitialized { get; private set; }

	// Token: 0x060054DE RID: 21726 RVA: 0x0002E0FF File Offset: 0x0002C2FF
	private void Awake()
	{
		this.m_onResolutionChanged = new Action<MonoBehaviour, EventArgs>(this.OnResolutionChanged);
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x0002E113 File Offset: 0x0002C313
	private void OnDestroy()
	{
		AspectRatioManager.IsInitialized = false;
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x0002E11B File Offset: 0x0002C31B
	private IEnumerator Start()
	{
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		this.OnResolutionChanged(null, null);
		yield break;
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x0002E12A File Offset: 0x0002C32A
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.ResolutionChanged, this.m_onResolutionChanged);
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x0002E138 File Offset: 0x0002C338
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.ResolutionChanged, this.m_onResolutionChanged);
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x00140FDC File Offset: 0x0013F1DC
	private void OnResolutionChanged(object sender, EventArgs args)
	{
		if (!CameraController.IsInstantiated)
		{
			return;
		}
		float a = (float)Screen.width / (float)Screen.height;
		float num = (float)GameResolutionManager.Resolution.x / (float)GameResolutionManager.Resolution.y;
		float num2 = (!AspectRatioManager.Disable_16_9_Aspect) ? 1.7777778f : num;
		if (Mathf.Approximately(a, num) && Mathf.Approximately(this.m_currentAspectRatio, num2))
		{
			return;
		}
		Rect rect = new Rect(0f, 0f, 1f, 1f);
		if (num < num2)
		{
			float num3 = num / num2;
			float y = (1f - num3) / 2f;
			rect.y = y;
			rect.height = num3;
		}
		else if (num > num2)
		{
			float num4 = num2 / num;
			float x = (1f - num4) / 2f;
			rect.x = x;
			rect.width = num4;
		}
		foreach (Camera camera in CameraController.Cameras)
		{
			if (AspectRatioManager.Disable_16_9_Aspect || camera != CameraController.TraitMaskCam)
			{
				camera.aspect = num2;
				camera.rect = rect;
			}
		}
		CameraController.GameCamera.aspect = num2;
		CameraController.GameCamera.rect = rect;
		CameraController.UICamera.aspect = num2;
		CameraController.UICamera.rect = rect;
		CameraController.SoloCam.Camera.aspect = num2;
		CameraController.SoloCam.Camera.rect = rect;
		this.m_currentAspectRatio = num2;
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.AspectRatioChanged, this, null);
	}

	// Token: 0x04003F2B RID: 16171
	public const float STANDARD_16_9_ASPECT_RATIO = 1.7777778f;

	// Token: 0x04003F2C RID: 16172
	private Camera m_camera;

	// Token: 0x04003F2D RID: 16173
	private float m_currentAspectRatio;

	// Token: 0x04003F2E RID: 16174
	private Action<MonoBehaviour, EventArgs> m_onResolutionChanged;

	// Token: 0x04003F2F RID: 16175
	private static AspectRatioManager m_instance;
}
