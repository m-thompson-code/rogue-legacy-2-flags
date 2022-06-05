using System;
using UnityEngine;

// Token: 0x020002AF RID: 687
public class PointLightController : MonoBehaviour
{
	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06001B69 RID: 7017 RVA: 0x00057BB4 File Offset: 0x00055DB4
	// (set) Token: 0x06001B6A RID: 7018 RVA: 0x00057BBC File Offset: 0x00055DBC
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

	// Token: 0x06001B6B RID: 7019 RVA: 0x00057BC8 File Offset: 0x00055DC8
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

	// Token: 0x06001B6C RID: 7020 RVA: 0x00057C22 File Offset: 0x00055E22
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
		this.OnQualitySettingsChanged(null, null);
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x00057C38 File Offset: 0x00055E38
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualitySettingsChanged);
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x00057C46 File Offset: 0x00055E46
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

	// Token: 0x06001B6F RID: 7023 RVA: 0x00057C7C File Offset: 0x00055E7C
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

	// Token: 0x06001B70 RID: 7024 RVA: 0x00057CD5 File Offset: 0x00055ED5
	public void SetLocation(PointLightLocation location)
	{
		this.Location = location;
	}

	// Token: 0x0400191C RID: 6428
	[SerializeField]
	private PointLightLocation m_location;

	// Token: 0x0400191D RID: 6429
	public const float DEFAULT_FOREGROUND_Z_POSITION = 0f;

	// Token: 0x0400191E RID: 6430
	public const float DEFAULT_BACKGROUND_Z_POSITION = 1f;

	// Token: 0x0400191F RID: 6431
	private Light m_light;

	// Token: 0x04001920 RID: 6432
	private Action<MonoBehaviour, EventArgs> m_onQualitySettingsChanged;
}
