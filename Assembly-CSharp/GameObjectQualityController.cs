using System;
using UnityEngine;

// Token: 0x02000A37 RID: 2615
public class GameObjectQualityController : MonoBehaviour
{
	// Token: 0x06004EDF RID: 20191 RVA: 0x0002B02A File Offset: 0x0002922A
	private void Awake()
	{
		this.m_onQualityChanged = new Action<MonoBehaviour, EventArgs>(this.OnQualityChanged);
	}

	// Token: 0x06004EE0 RID: 20192 RVA: 0x0002B03E File Offset: 0x0002923E
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
		this.OnQualityChanged(null, null);
	}

	// Token: 0x06004EE1 RID: 20193 RVA: 0x0002B054 File Offset: 0x00029254
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x0012E628 File Offset: 0x0012C828
	private void OnQualityChanged(object sender, EventArgs args)
	{
		if (SaveManager.ConfigData.QualitySetting == (int)this.m_qualitySetting || (this.m_enableForAllLowerQualitySettings && SaveManager.ConfigData.QualitySetting <= (int)this.m_qualitySetting))
		{
			foreach (GameObject gameObject in this.m_gosToDisable)
			{
				if (gameObject && gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
			foreach (GameObject gameObject2 in this.m_gosToEnable)
			{
				if (gameObject2 && !gameObject2.activeSelf)
				{
					gameObject2.SetActive(true);
				}
			}
			return;
		}
		if (this.m_resetGOStatesOnOtherSettings)
		{
			foreach (GameObject gameObject3 in this.m_gosToDisable)
			{
				if (gameObject3 && !gameObject3.activeSelf)
				{
					gameObject3.SetActive(true);
				}
			}
			foreach (GameObject gameObject4 in this.m_gosToEnable)
			{
				if (gameObject4 && gameObject4.activeSelf)
				{
					gameObject4.SetActive(false);
				}
			}
		}
	}

	// Token: 0x04003BEB RID: 15339
	[SerializeField]
	private QualitySetting m_qualitySetting;

	// Token: 0x04003BEC RID: 15340
	[SerializeField]
	[Tooltip("Resets the enabled states for the GameObjects if you are NOT running on the specified quality setting")]
	private bool m_resetGOStatesOnOtherSettings = true;

	// Token: 0x04003BED RID: 15341
	[SerializeField]
	private bool m_enableForAllLowerQualitySettings;

	// Token: 0x04003BEE RID: 15342
	[SerializeField]
	private GameObject[] m_gosToDisable;

	// Token: 0x04003BEF RID: 15343
	[SerializeField]
	private GameObject[] m_gosToEnable;

	// Token: 0x04003BF0 RID: 15344
	private Action<MonoBehaviour, EventArgs> m_onQualityChanged;
}
