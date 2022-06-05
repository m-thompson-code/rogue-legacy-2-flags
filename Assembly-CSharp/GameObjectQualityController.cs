using System;
using UnityEngine;

// Token: 0x02000615 RID: 1557
public class GameObjectQualityController : MonoBehaviour
{
	// Token: 0x06003842 RID: 14402 RVA: 0x000C01E0 File Offset: 0x000BE3E0
	private void Awake()
	{
		this.m_onQualityChanged = new Action<MonoBehaviour, EventArgs>(this.OnQualityChanged);
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x000C01F4 File Offset: 0x000BE3F4
	private void OnEnable()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
		this.OnQualityChanged(null, null);
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x000C020A File Offset: 0x000BE40A
	private void OnDisable()
	{
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.QualitySettingsChanged, this.m_onQualityChanged);
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x000C0218 File Offset: 0x000BE418
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

	// Token: 0x04002B84 RID: 11140
	[SerializeField]
	private QualitySetting m_qualitySetting;

	// Token: 0x04002B85 RID: 11141
	[SerializeField]
	[Tooltip("Resets the enabled states for the GameObjects if you are NOT running on the specified quality setting")]
	private bool m_resetGOStatesOnOtherSettings = true;

	// Token: 0x04002B86 RID: 11142
	[SerializeField]
	private bool m_enableForAllLowerQualitySettings;

	// Token: 0x04002B87 RID: 11143
	[SerializeField]
	private GameObject[] m_gosToDisable;

	// Token: 0x04002B88 RID: 11144
	[SerializeField]
	private GameObject[] m_gosToEnable;

	// Token: 0x04002B89 RID: 11145
	private Action<MonoBehaviour, EventArgs> m_onQualityChanged;
}
