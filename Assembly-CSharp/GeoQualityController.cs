using System;
using UnityEngine;

// Token: 0x02000616 RID: 1558
public class GeoQualityController : MonoBehaviour
{
	// Token: 0x06003847 RID: 14407 RVA: 0x000C0331 File Offset: 0x000BE531
	private void Awake()
	{
		this.m_onQualityChanged = new Action<MonoBehaviour, EventArgs>(this.OnQualityChanged);
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x000C0345 File Offset: 0x000BE545
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
		this.OnQualityChanged(null, null);
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x000C035B File Offset: 0x000BE55B
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x000C036C File Offset: 0x000BE56C
	private void OnQualityChanged(object sender, EventArgs args)
	{
		if (this.m_geoToChange == null)
		{
			return;
		}
		if (SaveManager.ConfigData.QualitySetting == (int)this.m_qualitySetting || (this.m_enableForAllLowerQualitySettings && SaveManager.ConfigData.QualitySetting <= (int)this.m_qualitySetting))
		{
			if (!this.m_usingChangedGeo)
			{
				this.m_usingChangedGeo = true;
				MeshFilter[] geoToChange = this.m_geoToChange;
				for (int i = 0; i < geoToChange.Length; i++)
				{
					geoToChange[i].sharedMesh = this.m_geoToChangeTo.sharedMesh;
				}
				return;
			}
		}
		else if (this.m_resetGOStatesOnOtherSettings && this.m_usingChangedGeo)
		{
			this.m_usingChangedGeo = false;
			MeshFilter[] geoToChange = this.m_geoToChange;
			for (int i = 0; i < geoToChange.Length; i++)
			{
				geoToChange[i].sharedMesh = this.m_originalGeo.sharedMesh;
			}
		}
	}

	// Token: 0x04002B8A RID: 11146
	[SerializeField]
	private QualitySetting m_qualitySetting;

	// Token: 0x04002B8B RID: 11147
	[SerializeField]
	[Tooltip("Resets the enabled states for the GameObjects if you are NOT running on the specified quality setting")]
	private bool m_resetGOStatesOnOtherSettings = true;

	// Token: 0x04002B8C RID: 11148
	[SerializeField]
	private bool m_enableForAllLowerQualitySettings;

	// Token: 0x04002B8D RID: 11149
	[SerializeField]
	private MeshFilter[] m_geoToChange;

	// Token: 0x04002B8E RID: 11150
	[SerializeField]
	private MeshFilter m_originalGeo;

	// Token: 0x04002B8F RID: 11151
	[SerializeField]
	private MeshFilter m_geoToChangeTo;

	// Token: 0x04002B90 RID: 11152
	private bool m_usingChangedGeo;

	// Token: 0x04002B91 RID: 11153
	private Action<MonoBehaviour, EventArgs> m_onQualityChanged;
}
