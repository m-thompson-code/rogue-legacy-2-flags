using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002DF RID: 735
public class TownSceneSetup : SceneSetup, ISceneSetupBroker
{
	// Token: 0x17000CD4 RID: 3284
	// (get) Token: 0x06001D40 RID: 7488 RVA: 0x000607FF File Offset: 0x0005E9FF
	// (set) Token: 0x06001D41 RID: 7489 RVA: 0x00060807 File Offset: 0x0005EA07
	public override bool IsComplete { get; protected set; }

	// Token: 0x06001D42 RID: 7490 RVA: 0x00060810 File Offset: 0x0005EA10
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

	// Token: 0x06001D43 RID: 7491 RVA: 0x00060848 File Offset: 0x0005EA48
	public void InjectSceneSetup()
	{
		foreach (SceneSetupConsumer sceneSetupConsumer in this.m_sceneSetupConsumers)
		{
			((ISceneSetupConsumer)sceneSetupConsumer).SceneSetup = this;
		}
	}

	// Token: 0x04001B3F RID: 6975
	[SerializeField]
	private List<SceneSetupConsumer> m_sceneSetupConsumers;
}
