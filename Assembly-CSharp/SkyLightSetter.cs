using System;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class SkyLightSetter : MonoBehaviour
{
	// Token: 0x06001DB8 RID: 7608 RVA: 0x00061D1D File Offset: 0x0005FF1D
	private void Awake()
	{
		this.m_light = base.GetComponent<Light>();
		this.m_setSkyLight = new Action<MonoBehaviour, EventArgs>(this.SetSkyLight);
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x00061D3D File Offset: 0x0005FF3D
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SkyLightColorChanged, this.m_setSkyLight);
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x00061D4C File Offset: 0x0005FF4C
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SkyLightColorChanged, this.m_setSkyLight);
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x00061D5C File Offset: 0x0005FF5C
	private void SetSkyLight(MonoBehaviour sender, EventArgs args)
	{
		SkyLightChangedEventArgs skyLightChangedEventArgs = args as SkyLightChangedEventArgs;
		Color color = this.m_light.color;
		Color newSkyLightColor = skyLightChangedEventArgs.NewSkyLightColor;
		Color color2 = Color.Lerp(color, newSkyLightColor, skyLightChangedEventArgs.Lerp);
		this.m_light.color = color2;
	}

	// Token: 0x04001B78 RID: 7032
	private Light m_light;

	// Token: 0x04001B79 RID: 7033
	private Action<MonoBehaviour, EventArgs> m_setSkyLight;
}
