using System;
using System.Globalization;
using RLAudio;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public class RerollRelicComponent : BaseSpecialPropController
{
	// Token: 0x0600269B RID: 9883 RVA: 0x000B6B88 File Offset: 0x000B4D88
	protected override void Awake()
	{
		base.Awake();
		this.m_startingY = this.m_rerollIcon.transform.localPosition.y;
		this.m_rerollIcon.SetActive(false);
		this.m_relicRoomProp.DisableSpecialPropRelay.AddListener(new Action<bool>(this.DisableProp), false);
	}

	// Token: 0x0600269C RID: 9884 RVA: 0x000158CE File Offset: 0x00013ACE
	private void OnDestroy()
	{
		this.m_relicRoomProp.DisableSpecialPropRelay.RemoveListener(new Action<bool>(this.DisableProp));
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x000158EE File Offset: 0x00013AEE
	protected override void InitializePooledPropOnEnter()
	{
		this.UpdateRolls();
	}

	// Token: 0x0600269E RID: 9886 RVA: 0x000158F6 File Offset: 0x00013AF6
	public void RerollRelic()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		AmbientSoundController.Instance.StopOnTransition = false;
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.RerollRelicTransitionOut), TransitionID.QuickSwipe, false);
		AmbientSoundController.Instance.StopOnTransition = true;
	}

	// Token: 0x0600269F RID: 9887 RVA: 0x00015927 File Offset: 0x00013B27
	private void RerollRelicTransitionOut()
	{
		this.m_relicRoomProp.RollRelics(1, true, true);
		PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
		playerSaveData.TimesRolledRelic += 1;
		this.UpdateRolls();
		RewiredMapController.SetCurrentMapEnabled(true);
	}

	// Token: 0x060026A0 RID: 9888 RVA: 0x000B6BE4 File Offset: 0x000B4DE4
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

	// Token: 0x060026A1 RID: 9889 RVA: 0x000B6CA4 File Offset: 0x000B4EA4
	private void Update()
	{
		float y = this.m_startingY + Mathf.Sin(Time.timeSinceLevelLoad * 2f) / 8f;
		Vector3 localPosition = this.m_rerollIcon.transform.localPosition;
		localPosition.y = y;
		this.m_rerollIcon.transform.localPosition = localPosition;
	}

	// Token: 0x060026A2 RID: 9890 RVA: 0x00015956 File Offset: 0x00013B56
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_rerollIcon.SetActive(false);
	}

	// Token: 0x04002168 RID: 8552
	[SerializeField]
	private RelicRoomPropController m_relicRoomProp;

	// Token: 0x04002169 RID: 8553
	[SerializeField]
	private GameObject m_rerollIcon;

	// Token: 0x0400216A RID: 8554
	[SerializeField]
	private TMP_Text m_rerollText;

	// Token: 0x0400216B RID: 8555
	private float m_startingY;
}
