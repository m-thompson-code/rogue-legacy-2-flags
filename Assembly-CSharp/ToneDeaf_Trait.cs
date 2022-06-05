using System;
using FMODUnity;

// Token: 0x020005DB RID: 1499
public class ToneDeaf_Trait : BaseTrait
{
	// Token: 0x17001262 RID: 4706
	// (get) Token: 0x06002E45 RID: 11845 RVA: 0x00019532 File Offset: 0x00017732
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ToneDeaf;
		}
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x000C7C4C File Offset: 0x000C5E4C
	private void Start()
	{
		RuntimeManager.StudioSystem.setParameterByName("traitTonedeaf", 1f, false);
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x000C7C74 File Offset: 0x000C5E74
	private void OnDestroy()
	{
		RuntimeManager.StudioSystem.setParameterByName("traitTonedeaf", 0f, false);
	}
}
