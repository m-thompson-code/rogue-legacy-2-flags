using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class PointLightController : MonoBehaviour
{
	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x06002605 RID: 9733 RVA: 0x000151DB File Offset: 0x000133DB
	// (set) Token: 0x06002606 RID: 9734 RVA: 0x000151E3 File Offset: 0x000133E3
	public PointLightLocation Location
	{
		get
		{
			return this.m_location;
		}
		private set
		{
			this.m_location = value;
		}
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x000B4964 File Offset: 0x000B2B64
	private void Awake()
	{
		this.m_light = base.GetComponent<Light>();
		this.m_onQualitySettingsChanged = new Action<MonoBehaviour, EventArgs>(this.OnQualitySettingsChanged);
		if (this.m_light && this.m_light.type != LightType.Point)
		{
			Debug.LogFormat("<color=red>| {0} | Light is not a Point Light. If you see this message, please add a bug report to Pivotal.</color>", new object[]
			{
				this
			});
		}
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x000151EC File Offset: 0x000133EC
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
		this.OnQualitySettingsChanged(null, null);
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x00015202 File Offset: 0x00013402
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x00015210 File Offset: 0x00013410
	private void OnQualitySettingsChanged(object sender, EventArgs args)
	{
		if (this.m_light)
		{
			if (SaveManager.ConfigData.QualitySetting == 0)
			{
				this.m_light.enabled = false;
				return;
			}
			this.m_light.enabled = true;
		}
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000B49C0 File Offset: 0x000B2BC0
	public void UpdateLocation(CameraLayer cameraLayer)
	{
		if (this.m_light)
		{
			float z = 0f;
			if (this.m_location == PointLightLocation.Back)
			{
				z = 1f;
			}
			Vector3 position = this.m_light.transform.position;
			position.z = z;
			this.m_light.transform.position = position;
		}
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x00015244 File Offset: 0x00013444
	public void SetLocation(PointLightLocation location)
	{
		this.Location = location;
	}

	// Token: 0x040020F8 RID: 8440
	[SerializeField]
	private PointLightLocation m_location;

	// Token: 0x040020F9 RID: 8441
	public const float DEFAULT_FOREGROUND_Z_POSITION = 0f;

	// Token: 0x040020FA RID: 8442
	public const float DEFAULT_BACKGROUND_Z_POSITION = 1f;

	// Token: 0x040020FB RID: 8443
	private Light m_light;

	// Token: 0x040020FC RID: 8444
	private Action<MonoBehaviour, EventArgs> m_onQualitySettingsChanged;
}
