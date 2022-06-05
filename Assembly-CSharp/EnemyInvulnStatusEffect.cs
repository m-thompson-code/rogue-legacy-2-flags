using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class EnemyInvulnStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D4A RID: 3402
	// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x00063220 File Offset: 0x00061420
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Invuln;
		}
	}

	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x00063227 File Offset: 0x00061427
	public override float StartingDurationOverride
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x0006322E File Offset: 0x0006142E
	// (set) Token: 0x06001EA5 RID: 7845 RVA: 0x00063236 File Offset: 0x00061436
	public GameObject Source { get; private set; }

	// Token: 0x06001EA6 RID: 7846 RVA: 0x0006323F File Offset: 0x0006143F
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

	// Token: 0x06001EA7 RID: 7847 RVA: 0x00063258 File Offset: 0x00061458
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
