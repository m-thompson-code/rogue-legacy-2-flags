using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200067E RID: 1662
public class RelicHUDIconController : MonoBehaviour
{
	// Token: 0x17001371 RID: 4977
	// (get) Token: 0x060032BE RID: 12990 RVA: 0x0001BC11 File Offset: 0x00019E11
	// (set) Token: 0x060032BF RID: 12991 RVA: 0x0001BC19 File Offset: 0x00019E19
	public RelicType RelicType { get; private set; }

	// Token: 0x060032C0 RID: 12992 RVA: 0x000D9DC8 File Offset: 0x000D7FC8
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

	// Token: 0x060032C1 RID: 12993 RVA: 0x000D9E04 File Offset: 0x000D8004
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

	// Token: 0x060032C2 RID: 12994 RVA: 0x000D9F14 File Offset: 0x000D8114
	public void PlayPurifyEffect()
	{
		EffectManager.PlayEffect(base.gameObject, null, "RelicPurifiedUI_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(base.transform, false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "RelicPurified_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(playerController.transform, true);
		TextPopupManager.DisplayTextDefaultPos(TextPopupType.DownstrikeAmmoGain, LocalizationManager.GetString("LOC_ID_RELIC_UI_RELIC_PURIFIED_1", false, false), playerController, true, true);
	}

	// Token: 0x04002987 RID: 10631
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04002988 RID: 10632
	[SerializeField]
	private TMP_Text m_intValueText;

	// Token: 0x04002989 RID: 10633
	[SerializeField]
	private Image m_sprite;
}
