using System;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public abstract class SceneSetupConsumer : MonoBehaviour, ISceneSetupConsumer
{
	// Token: 0x17001B6A RID: 7018
	// (get) Token: 0x06004FA8 RID: 20392
	// (set) Token: 0x06004FA9 RID: 20393
	public abstract ISceneSetup SceneSetup { get; set; }
}
