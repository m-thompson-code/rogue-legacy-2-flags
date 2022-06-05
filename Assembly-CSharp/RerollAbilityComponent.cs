using System;
using System.Globalization;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x020002BC RID: 700
public class RerollAbilityComponent : BaseSpecialPropController
{
	// Token: 0x06001BDC RID: 7132 RVA: 0x00059D94 File Offset: 0x00057F94
	protected override void Awake()
	{
		base.Awake();
		this.m_startingY = this.m_rerollIcon.transform.localPosition.y;
		this.m_rerollIcon.SetActive(false);
		this.m_swapAbilityRoomProp.DisableSpecialPropRelay.AddListener(new Action<bool>(this.DisableProp), false);
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00059DED File Offset: 0x00057FED
	private void OnDestroy()
	{
		this.m_swapAbilityRoomProp.DisableSpecialPropRelay.RemoveListener(new Action<bool>(this.DisableProp));
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x00059E0D File Offset: 0x0005800D
	protected override void InitializePooledPropOnEnter()
	{
		this.UpdateRolls();
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00059E15 File Offset: 0x00058015
	public void RerollAbility()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.RerollAbilityTransitionOut), TransitionID.QuickSwipe, false);
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00059E30 File Offset: 0x00058030
	private void RerollAbilityTransitionOut()
	{
		this.m_swapAbilityRoomProp.RollAbilities(1, true);
		PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
		playerSaveData.TimesRolledRelic += 1;
		this.UpdateRolls();
		RewiredMapController.SetCurrentMapEnabled(true);
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x00059E60 File Offset: 0x00058060
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

	// Token: 0x06001BE2 RID: 7138 RVA: 0x00059F20 File Offset: 0x00058120
	private void Update()
	{
		float y = this.m_startingY + Mathf.Sin(Time.timeSinceLevelLoad * 2f) / 8f;
		Vector3 localPosition = this.m_rerollIcon.transform.localPosition;
		localPosition.y = y;
		this.m_rerollIcon.transform.localPosition = localPosition;
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x00059F75 File Offset: 0x00058175
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_rerollIcon.SetActive(false);
	}

	// Token: 0x04001974 RID: 6516
	[SerializeField]
	private SwapAbilityRoomPropController m_swapAbilityRoomProp;

	// Token: 0x04001975 RID: 6517
	[SerializeField]
	private GameObject m_rerollIcon;

	// Token: 0x04001976 RID: 6518
	[SerializeField]
	private TMP_Text m_rerollText;

	// Token: 0x04001977 RID: 6519
	private float m_startingY;
}
