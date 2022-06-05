using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public class EnemyInvulnStatusEffect : BaseStatusEffect
{
	// Token: 0x1700112F RID: 4399
	// (get) Token: 0x06002A83 RID: 10883 RVA: 0x00017CC0 File Offset: 0x00015EC0
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Invuln;
		}
	}

	// Token: 0x17001130 RID: 4400
	// (get) Token: 0x06002A84 RID: 10884 RVA: 0x0000457A File Offset: 0x0000277A
	public override float StartingDurationOverride
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17001131 RID: 4401
	// (get) Token: 0x06002A85 RID: 10885 RVA: 0x00017CC7 File Offset: 0x00015EC7
	// (set) Token: 0x06002A86 RID: 10886 RVA: 0x00017CCF File Offset: 0x00015ECF
	public GameObject Source { get; private set; }

	// Token: 0x06002A87 RID: 10887 RVA: 0x00017CD8 File Offset: 0x00015ED8
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		EnemyController enemyController = this.m_charController as EnemyController;
		this.Source = caster.gameObject;
		bool flag = true;
		if ((enemyController.EnemyType == EnemyType.PaintingEnemy || enemyController.EnemyType == EnemyType.MimicChest) && !enemyController.LogicController.LogicIsActivated)
		{
			flag = false;
		}
		if (!flag)
		{
			goto IL_F7;
		}
		using (List<Renderer>.Enumerator enumerator = this.m_charController.RendererArray.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Renderer renderer = enumerator.Current;
				if (renderer.sharedMaterial.HasProperty(ShaderID_RL._ShieldToggle))
				{
					renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					BaseStatusEffect.m_matBlockHelper_STATIC.SetInt(ShaderID_RL._ShieldToggle, 1);
					renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				}
			}
			goto IL_F7;
		}
		IL_E0:
		yield return null;
		IL_F7:
		if (Time.time >= base.EndTime)
		{
			this.StopEffect(false);
			yield break;
		}
		goto IL_E0;
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x000C1C48 File Offset: 0x000BFE48
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!GameManager.IsApplicationClosing)
		{
			this.Source = null;
			if (this.m_charController && BaseStatusEffect.m_matBlockHelper_STATIC != null && this.m_charController.RendererArray != null)
			{
				foreach (Renderer renderer in this.m_charController.RendererArray)
				{
					if (renderer && renderer.sharedMaterial.HasProperty(ShaderID_RL._ShieldToggle))
					{
						renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
						BaseStatusEffect.m_matBlockHelper_STATIC.SetInt(ShaderID_RL._ShieldToggle, 0);
						renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					}
				}
			}
		}
	}
}
