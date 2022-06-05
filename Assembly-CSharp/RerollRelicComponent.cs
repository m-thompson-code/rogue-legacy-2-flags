using System;
using System.Globalization;
using RLAudio;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class RerollRelicComponent : BaseSpecialPropController
{
	// Token: 0x06001BE5 RID: 7141 RVA: 0x00059F94 File Offset: 0x00058194
	protected override void Awake()
	{
		base.Awake();
		this.m_startingY = this.m_rerollIcon.transform.localPosition.y;
		this.m_rerollIcon.SetActive(false);
		this.m_relicRoomProp.DisableSpecialPropRelay.AddListener(new Action<bool>(this.DisableProp), false);
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x00059FED File Offset: 0x000581ED
	private void OnDestroy()
	{
		this.m_relicRoomProp.DisableSpecialPropRelay.RemoveListener(new Action<bool>(this.DisableProp));
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x0005A00D File Offset: 0x0005820D
	protected override void InitializePooledPropOnEnter()
	{
		this.UpdateRolls();
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x0005A015 File Offset: 0x00058215
	public void RerollRelic()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		AmbientSoundController.Instance.StopOnTransition = false;
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.RerollRelicTransitionOut), TransitionID.QuickSwipe, false);
		AmbientSoundController.Instance.StopOnTransition = true;
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x0005A046 File Offset: 0x00058246
	private void RerollRelicTransitionOut()
	{
		this.m_relicRoomProp.RollRelics(1, true, true);
		PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
		playerSaveData.TimesRolledRelic += 1;
		this.UpdateRolls();
		RewiredMapController.SetCurrentMapEnabled(true);
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x0005A078 File Offset: 0x00058278
	private void UpdateRolls()
	{
		int num = (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Reroll_Relic).CurrentStatGain - (int)SaveManager.PlayerSaveData.TimesRolledRelic;
		int num2 = (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Reroll_Relic_Room_Cap).CurrentStatGain + 1;
		int num3 = 1;
		int num4;
		if (int.TryParse(base.GetRoomMiscData("TotalRelicRolls"), NumberStyles.Any, SaveManager.CultureInfo, out num4))
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

	// Token: 0x06001BEB RID: 7147 RVA: 0x0005A138 File Offset: 0x00058338
	private void Update()
	{
		float y = this.m_startingY + Mathf.Sin(Time.timeSinceLevelLoad * 2f) / 8f;
		Vector3 localPosition = this.m_rerollIcon.transform.localPosition;
		localPosition.y = y;
		this.m_rerollIcon.transform.localPosition = localPosition;
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x0005A18D File Offset: 0x0005838D
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_rerollIcon.SetActive(false);
	}

	// Token: 0x04001978 RID: 6520
	[SerializeField]
	private RelicRoomPropController m_relicRoomProp;

	// Token: 0x04001979 RID: 6521
	[SerializeField]
	private GameObject m_rerollIcon;

	// Token: 0x0400197A RID: 6522
	[SerializeField]
	private TMP_Text m_rerollText;

	// Token: 0x0400197B RID: 6523
	private float m_startingY;
}
