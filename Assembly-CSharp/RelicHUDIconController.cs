using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003DE RID: 990
public class RelicHUDIconController : MonoBehaviour
{
	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x06002482 RID: 9346 RVA: 0x00079A69 File Offset: 0x00077C69
	// (set) Token: 0x06002483 RID: 9347 RVA: 0x00079A71 File Offset: 0x00077C71
	public RelicType RelicType { get; private set; }

	// Token: 0x06002484 RID: 9348 RVA: 0x00079A7C File Offset: 0x00077C7C
	public void SetRelicType(RelicType relicType)
	{
		if (relicType != RelicType.None)
		{
			Sprite relicSprite = IconLibrary.GetRelicSprite(relicType, false);
			this.m_sprite.sprite = relicSprite;
		}
		else
		{
			this.m_sprite.sprite = null;
		}
		this.RelicType = relicType;
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x00079AB8 File Offset: 0x00077CB8
	public void UpdateRelic()
	{
		if (this.RelicType == RelicType.None)
		{
			return;
		}
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(this.RelicType);
		int level = relic.Level;
		this.m_levelText.text = level.ToString();
		int relicMaxStack = Relic_EV.GetRelicMaxStack(this.RelicType, level);
		int intValue = relic.IntValue;
		if (relic.CountsBackwards)
		{
			this.m_intValueText.text = (relicMaxStack - intValue).ToString();
		}
		else
		{
			this.m_intValueText.text = intValue.ToString();
		}
		if (relicMaxStack == -1)
		{
			if (this.m_intValueText.gameObject.activeSelf)
			{
				this.m_intValueText.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			if (!this.m_intValueText.gameObject.activeSelf)
			{
				this.m_intValueText.gameObject.SetActive(true);
			}
			if (intValue >= relicMaxStack)
			{
				if (relic.CountsBackwards)
				{
					this.m_intValueText.color = Color.red;
					return;
				}
				this.m_intValueText.color = Color.yellow;
				return;
			}
			else
			{
				this.m_intValueText.color = Color.white;
			}
		}
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x00079BC8 File Offset: 0x00077DC8
	public void PlayPurifyEffect()
	{
		EffectManager.PlayEffect(base.gameObject, null, "RelicPurifiedUI_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(base.transform, false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "RelicPurified_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(playerController.transform, true);
		TextPopupManager.DisplayTextDefaultPos(TextPopupType.DownstrikeAmmoGain, LocalizationManager.GetString("LOC_ID_RELIC_UI_RELIC_PURIFIED_1", false, false), playerController, true, true);
	}

	// Token: 0x04001F0F RID: 7951
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001F10 RID: 7952
	[SerializeField]
	private TMP_Text m_intValueText;

	// Token: 0x04001F11 RID: 7953
	[SerializeField]
	private Image m_sprite;
}
