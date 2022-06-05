using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class TownSceneSetup : SceneSetup, ISceneSetupBroker
{
	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x0600281F RID: 10271 RVA: 0x0001689C File Offset: 0x00014A9C
	// (set) Token: 0x06002820 RID: 10272 RVA: 0x000168A4 File Offset: 0x00014AA4
	public override bool IsComplete { get; protected set; }

	// Token: 0x06002821 RID: 10273 RVA: 0x000168AD File Offset: 0x00014AAD
	private void Start()
	{
		this.InjectSceneSetup();
		Debug.LogFormat("<color=blue>{0}: Town Scene Setup Complete!</color>", new object[]
		{
			Time.frameCount
		});
		this.IsComplete = true;
		base.OnComplete(EventArgs.Empty);
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000BCD60 File Offset: 0x000BAF60
	public void InjectSceneSetup()
	{
		foreach (SceneSetupConsumer sceneSetupConsumer in this.m_sceneSetupConsumers)
		{
			((ISceneSetupConsumer)sceneSetupConsumer).SceneSetup = this;
		}
	}

	// Token: 0x04002354 RID: 9044
	[SerializeField]
	private List<SceneSetupConsumer> m_sceneSetupConsumers;
}
