using System;
using System.Globalization;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class RerollAbilityComponent : BaseSpecialPropController
{
	// Token: 0x06002692 RID: 9874 RVA: 0x000B6A14 File Offset: 0x000B4C14
	protected override void Awake()
	{
		base.Awake();
		this.m_startingY = this.m_rerollIcon.transform.localPosition.y;
		this.m_rerollIcon.SetActive(false);
		this.m_swapAbilityRoomProp.DisableSpecialPropRelay.AddListener(new Action<bool>(this.DisableProp), false);
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x00015848 File Offset: 0x00013A48
	private void OnDestroy()
	{
		this.m_swapAbilityRoomProp.DisableSpecialPropRelay.RemoveListener(new Action<bool>(this.DisableProp));
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x00015868 File Offset: 0x00013A68
	protected override void InitializePooledPropOnEnter()
	{
		this.UpdateRolls();
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x00015870 File Offset: 0x00013A70
	public void RerollAbility()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.RerollAbilityTransitionOut), TransitionID.QuickSwipe, false);
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x0001588B File Offset: 0x00013A8B
	private void RerollAbilityTransitionOut()
	{
		this.m_swapAbilityRoomProp.RollAbilities(1, true);
		PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
		playerSaveData.TimesRolledRelic += 1;
		this.UpdateRolls();
		RewiredMapController.SetCurrentMapEnabled(true);
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x000B6A70 File Offset: 0x000B4C70
	private void UpdateRolls()
	{
		int num = (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Reroll_Relic).CurrentStatGain - (int)SaveManager.PlayerSaveData.TimesRolledRelic;
		int num2 = (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Reroll_Relic_Room_Cap).CurrentStatGain + 1;
		int num3 = 1;
		int num4;
		if (int.TryParse(base.GetRoomMiscData("TotalAbilityRolls"), NumberStyles.Any, SaveManager.CultureInfo, out num4))
		{
			num3 = num4;
		}
		this.m_rerollText.text = string.Format(LocalizationManager.GetString("LOC_ID_FORMATTER_REROLLS_REMAINING_1", false, false), num, num2 - num3);
		if (num <= 0 || base.SpecialRoomController.IsRoomComplete || num3 > num2)
		{
			this.DisableProp(true);
			return;
		}
		this.m_rerollIcon.SetActive(true);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x000B6B30 File Offset: 0x000B4D30
	private void Update()
	{
		float y = this.m_startingY + Mathf.Sin(Time.timeSinceLevelLoad * 2f) / 8f;
		Vector3 localPosition = this.m_rerollIcon.transform.localPosition;
		localPosition.y = y;
		this.m_rerollIcon.transform.localPosition = localPosition;
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x000158B9 File Offset: 0x00013AB9
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_rerollIcon.SetActive(false);
	}

	// Token: 0x04002164 RID: 8548
	[SerializeField]
	private SwapAbilityRoomPropController m_swapAbilityRoomProp;

	// Token: 0x04002165 RID: 8549
	[SerializeField]
	private GameObject m_rerollIcon;

	// Token: 0x04002166 RID: 8550
	[SerializeField]
	private TMP_Text m_rerollText;

	// Token: 0x04002167 RID: 8551
	private float m_startingY;
}
