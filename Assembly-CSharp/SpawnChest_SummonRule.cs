using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052B RID: 1323
[Serializable]
public class SpawnChest_SummonRule : BaseSummonRule
{
	// Token: 0x17001203 RID: 4611
	// (get) Token: 0x060030B9 RID: 12473 RVA: 0x000A5F32 File Offset: 0x000A4132
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SpawnChest;
		}
	}

	// Token: 0x17001204 RID: 4612
	// (get) Token: 0x060030BA RID: 12474 RVA: 0x000A5F39 File Offset: 0x000A4139
	public override string RuleLabel
	{
		get
		{
			return "Spawn Chest";
		}
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x000A5F40 File Offset: 0x000A4140
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_onPlayerEnter = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnter);
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x000A5F5B File Offset: 0x000A415B
	public override void OnEnable()
	{
		base.OnEnable();
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnter);
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x000A5F6F File Offset: 0x000A416F
	public override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnter);
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x000A5F84 File Offset: 0x000A4184
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		ChestSpawnController chestSpawnController = (base.SerializedObject != null) ? (base.SerializedObject as ChestSpawnController) : null;
		if (chestSpawnController)
		{
			ChestObj chestInstance = chestSpawnController.ChestInstance;
			if (chestInstance)
			{
				chestInstance.SetOpacity(0f);
				chestInstance.SetChestLockState(ChestLockState.Locked);
				chestInstance.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x000A5FE3 File Offset: 0x000A41E3
	public override IEnumerator RunSummonRule()
	{
		ChestSpawnController chestSpawnController = (base.SerializedObject != null) ? (base.SerializedObject as ChestSpawnController) : null;
		if (chestSpawnController != null && chestSpawnController.ChestInstance != null)
		{
			ChestObj chest = chestSpawnController.ChestInstance;
			if (chest != null)
			{
				float chestOpacity = 0f;
				chest.gameObject.SetActive(true);
				Vector3 position = chest.transform.position;
				position.y += 1f;
				chest.transform.position = position;
				chest.Interactable.SetIsInteractableActive(false);
				TweenManager.TweenBy(chest.transform, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
				{
					"position.y",
					-1
				});
				while (chestOpacity < 1f)
				{
					chest.SetOpacity(chestOpacity);
					chestOpacity += Time.deltaTime;
					yield return null;
				}
				chest.Interactable.SetIsInteractableActive(true);
				chest.SetChestLockState(ChestLockState.Unlocked);
			}
			chest = null;
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040026A2 RID: 9890
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnter;
}
