using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008AF RID: 2223
[Serializable]
public class SpawnChest_SummonRule : BaseSummonRule
{
	// Token: 0x17001838 RID: 6200
	// (get) Token: 0x060043D5 RID: 17365 RVA: 0x00025685 File Offset: 0x00023885
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SpawnChest;
		}
	}

	// Token: 0x17001839 RID: 6201
	// (get) Token: 0x060043D6 RID: 17366 RVA: 0x0002568C File Offset: 0x0002388C
	public override string RuleLabel
	{
		get
		{
			return "Spawn Chest";
		}
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x00025693 File Offset: 0x00023893
	public override void Initialize(SummonRuleController summonController)
	{
		base.Initialize(summonController);
		this.m_onPlayerEnter = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnter);
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x000256AE File Offset: 0x000238AE
	public override void OnEnable()
	{
		base.OnEnable();
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnter);
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x000256C2 File Offset: 0x000238C2
	public override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnter);
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x0010D6C4 File Offset: 0x0010B8C4
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

	// Token: 0x060043DB RID: 17371 RVA: 0x000256D6 File Offset: 0x000238D6
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

	// Token: 0x040034BD RID: 13501
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnter;
}
