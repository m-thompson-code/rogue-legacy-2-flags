using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000688 RID: 1672
public class AspectRatioManager : MonoBehaviour
{
	// Token: 0x17001508 RID: 5384
	// (get) Token: 0x06003C5C RID: 15452 RVA: 0x000D08D9 File Offset: 0x000CEAD9
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

	// Token: 0x17001509 RID: 5385
	// (get) Token: 0x06003C5D RID: 15453 RVA: 0x000D08FD File Offset: 0x000CEAFD
	public static bool Disable_16_9_Aspect
	{
		get
		{
			return !AspectRatioManager.ForceEnable_16_9 && SaveManager.ConfigData.Disable_16_9;
		}
	}

	// Token: 0x1700150A RID: 5386
	// (get) Token: 0x06003C5E RID: 15454 RVA: 0x000D0912 File Offset: 0x000CEB12
	public static float CurrentGameAspectRatio
	{
		get
		{
			return AspectRatioManager.Instance.m_currentAspectRatio;
		}
	}

	// Token: 0x1700150B RID: 5387
	// (get) Token: 0x06003C5F RID: 15455 RVA: 0x000D0920 File Offset: 0x000CEB20
	public static float CurrentScreenAspectRatio
	{
		get
		{
			return (float)GameResolutionManager.Resolution.x / (float)GameResolutionManager.Resolution.y;
		}
	}

	// Token: 0x1700150C RID: 5388
	// (get) Token: 0x06003C60 RID: 15456 RVA: 0x000D094C File Offset: 0x000CEB4C
	public static bool IsScreen_16_9_AspectRatio
	{
		get
		{
			float b = (float)((int)(AspectRatioManager.CurrentScreenAspectRatio * 100f)) / 100f;
			return Mathf.Approximately(1.77f, b);
		}
	}

	// Token: 0x1700150D RID: 5389
	// (get) Token: 0x06003C61 RID: 15457 RVA: 0x000D0978 File Offset: 0x000CEB78
	// (set) Token: 0x06003C62 RID: 15458 RVA: 0x000D097F File Offset: 0x000CEB7F
	public static bool ForceEnable_16_9 { get; set; }

	// Token: 0x1700150E RID: 5390
	// (get) Token: 0x06003C63 RID: 15459 RVA: 0x000D0987 File Offset: 0x000CEB87
	// (set) Token: 0x06003C64 RID: 15460 RVA: 0x000D098E File Offset: 0x000CEB8E
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003C65 RID: 15461 RVA: 0x000D0996 File Offset: 0x000CEB96
	private void Awake()
	{
		this.m_onResolutionChanged = new Action<MonoBehaviour, EventArgs>(this.OnResolutionChanged);
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x000D09AA File Offset: 0x000CEBAA
	private void OnDestroy()
	{
		AspectRatioManager.IsInitialized = false;
	}

	// Token: 0x06003C67 RID: 15463 RVA: 0x000D09B2 File Offset: 0x000CEBB2
	private IEnumerator Start()
	{
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		this.OnResolutionChanged(null, null);
		yield break;
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x000D09C1 File Offset: 0x000CEBC1
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.ResolutionChanged, this.m_onResolutionChanged);
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x000D09CF File Offset: 0x000CEBCF
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.ResolutionChanged, this.m_onResolutionChanged);
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x000D09E0 File Offset: 0x000CEBE0
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

	// Token: 0x04002D70 RID: 11632
	public const float STANDARD_16_9_ASPECT_RATIO = 1.7777778f;

	// Token: 0x04002D71 RID: 11633
	private Camera m_camera;

	// Token: 0x04002D72 RID: 11634
	private float m_currentAspectRatio;

	// Token: 0x04002D73 RID: 11635
	private Action<MonoBehaviour, EventArgs> m_onResolutionChanged;

	// Token: 0x04002D74 RID: 11636
	private static AspectRatioManager m_instance;
}
