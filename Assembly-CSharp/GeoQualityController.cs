using System;
using UnityEngine;

// Token: 0x02000A38 RID: 2616
public class GeoQualityController : MonoBehaviour
{
	// Token: 0x06004EE4 RID: 20196 RVA: 0x0002B071 File Offset: 0x00029271
	private void Awake()
	{
		this.m_onQualityChanged = new Action<MonoBehaviour, EventArgs>(this.OnQualityChanged);
	}

	// Token: 0x06004EE5 RID: 20197 RVA: 0x0002B085 File Offset: 0x00029285
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
		this.OnQualityChanged(null, null);
	}

	// Token: 0x06004EE6 RID: 20198 RVA: 0x0002B09B File Offset: 0x0002929B
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x0012E734 File Offset: 0x0012C934
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

	// Token: 0x04003BF1 RID: 15345
	[SerializeField]
	private QualitySetting m_qualitySetting;

	// Token: 0x04003BF2 RID: 15346
	[SerializeField]
	[Tooltip("Resets the enabled states for the GameObjects if you are NOT running on the specified quality setting")]
	private bool m_resetGOStatesOnOtherSettings = true;

	// Token: 0x04003BF3 RID: 15347
	[SerializeField]
	private bool m_enableForAllLowerQualitySettings;

	// Token: 0x04003BF4 RID: 15348
	[SerializeField]
	private MeshFilter[] m_geoToChange;

	// Token: 0x04003BF5 RID: 15349
	[SerializeField]
	private MeshFilter m_originalGeo;

	// Token: 0x04003BF6 RID: 15350
	[SerializeField]
	private MeshFilter m_geoToChangeTo;

	// Token: 0x04003BF7 RID: 15351
	private bool m_usingChangedGeo;

	// Token: 0x04003BF8 RID: 15352
	private Action<MonoBehaviour, EventArgs> m_onQualityChanged;
}
