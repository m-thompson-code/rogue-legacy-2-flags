using System;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public abstract class SceneSetupConsumer : MonoBehaviour, ISceneSetupConsumer
{
	// Token: 0x17001405 RID: 5125
	// (get) Token: 0x060038CF RID: 14543
	// (set) Token: 0x060038D0 RID: 14544
	public abstract ISceneSetup SceneSetup { get; set; }
}
