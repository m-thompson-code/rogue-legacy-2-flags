using System;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public class SkyLightSetter : MonoBehaviour
{
	// Token: 0x060028FD RID: 10493 RVA: 0x0001722C File Offset: 0x0001542C
	private void Awake()
	{
		this.m_light = base.GetComponent<Light>();
		this.m_setSkyLight = new Action<MonoBehaviour, EventArgs>(this.SetSkyLight);
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x0001724C File Offset: 0x0001544C
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SkyLightColorChanged, this.m_setSkyLight);
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x0001725B File Offset: 0x0001545B
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SkyLightColorChanged, this.m_setSkyLight);
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000BF538 File Offset: 0x000BD738
	private void SetSkyLight(MonoBehaviour sender, EventArgs args)
	{
		SkyLightChangedEventArgs skyLightChangedEventArgs = args as SkyLightChangedEventArgs;
		Color color = this.m_light.color;
		Color newSkyLightColor = skyLightChangedEventArgs.NewSkyLightColor;
		Color color2 = Color.Lerp(color, newSkyLightColor, skyLightChangedEventArgs.Lerp);
		this.m_light.color = color2;
	}

	// Token: 0x040023C3 RID: 9155
	private Light m_light;

	// Token: 0x040023C4 RID: 9156
	private Action<MonoBehaviour, EventArgs> m_setSkyLight;
}
