using System;
using FMODUnity;

// Token: 0x02000364 RID: 868
public class ToneDeaf_Trait : BaseTrait
{
	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x060020A6 RID: 8358 RVA: 0x00066D80 File Offset: 0x00064F80
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ToneDeaf;
		}
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x00066D88 File Offset: 0x00064F88
	private void Start()
	{
		RuntimeManager.StudioSystem.setParameterByName("traitTonedeaf", 1f, false);
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x00066DB0 File Offset: 0x00064FB0
	private void OnDestroy()
	{
		RuntimeManager.StudioSystem.setParameterByName("traitTonedeaf", 0f, false);
	}
}
