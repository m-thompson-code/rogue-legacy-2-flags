using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000371 RID: 881
public class ClownHUDController : MonoBehaviour
{
	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x06002105 RID: 8453 RVA: 0x00067A8D File Offset: 0x00065C8D
	// (set) Token: 0x06002106 RID: 8454 RVA: 0x00067A95 File Offset: 0x00065C95
	public ClownRoomController ClownRoomController { get; private set; }

	// Token: 0x06002107 RID: 8455 RVA: 0x00067AA0 File Offset: 0x00065CA0
	private void Awake()
	{
		this.m_onEnterClownRoom = new Action<MonoBehaviour, EventArgs>(this.OnEnterClownRoom);
		this.m_onExitClownRoom = new Action<MonoBehaviour, EventArgs>(this.OnExitClownRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterClownRoom, this.m_onEnterClownRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitClownRoom, this.m_onExitClownRoom);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x00067AF7 File Offset: 0x00065CF7
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterClownRoom, this.m_onEnterClownRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitClownRoom, this.m_onExitClownRoom);
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x00067B14 File Offset: 0x00065D14
	private void OnEnterClownRoom(object sender, EventArgs args)
	{
		ClownRoomEnteredEventArgs clownRoomEnteredEventArgs = args as ClownRoomEnteredEventArgs;
		this.ClownRoomController = clownRoomEnteredEventArgs.ClownRoomController;
		ClownRoomController clownRoomController = this.ClownRoomController;
		clownRoomController.CurrentAmmoChangedEvent = (ClownRoomEventHandler)Delegate.Combine(clownRoomController.CurrentAmmoChangedEvent, new ClownRoomEventHandler(this.OnAmmoChanged));
		ClownRoomController clownRoomController2 = this.ClownRoomController;
		clownRoomController2.BronzeGoalReachedEvent = (ClownRoomEventHandler)Delegate.Combine(clownRoomController2.BronzeGoalReachedEvent, new ClownRoomEventHandler(this.OnBronzeGoalReached));
		ClownRoomController clownRoomController3 = this.ClownRoomController;
		clownRoomController3.SilverGoalReachedEvent = (ClownRoomEventHandler)Delegate.Combine(clownRoomController3.SilverGoalReachedEvent, new ClownRoomEventHandler(this.OnSilverGoalReached));
		ClownRoomController clownRoomController4 = this.ClownRoomController;
		clownRoomController4.GoldGoalReachedEvent = (ClownRoomEventHandler)Delegate.Combine(clownRoomController4.GoldGoalReachedEvent, new ClownRoomEventHandler(this.OnGoldGoalReached));
		ClownRoomController clownRoomController5 = this.ClownRoomController;
		clownRoomController5.CurrentGoalAmountChangedEvent = (ClownRoomEventHandler)Delegate.Combine(clownRoomController5.CurrentGoalAmountChangedEvent, new ClownRoomEventHandler(this.OnCurrentGoalAmountChanged));
		this.ResetHUD();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x00067C0C File Offset: 0x00065E0C
	private void OnExitClownRoom(object sender, EventArgs args)
	{
		if (this.ClownRoomController != null)
		{
			ClownRoomController clownRoomController = this.ClownRoomController;
			clownRoomController.CurrentAmmoChangedEvent = (ClownRoomEventHandler)Delegate.Remove(clownRoomController.CurrentAmmoChangedEvent, new ClownRoomEventHandler(this.OnAmmoChanged));
			ClownRoomController clownRoomController2 = this.ClownRoomController;
			clownRoomController2.BronzeGoalReachedEvent = (ClownRoomEventHandler)Delegate.Remove(clownRoomController2.BronzeGoalReachedEvent, new ClownRoomEventHandler(this.OnBronzeGoalReached));
			ClownRoomController clownRoomController3 = this.ClownRoomController;
			clownRoomController3.SilverGoalReachedEvent = (ClownRoomEventHandler)Delegate.Remove(clownRoomController3.SilverGoalReachedEvent, new ClownRoomEventHandler(this.OnSilverGoalReached));
			ClownRoomController clownRoomController4 = this.ClownRoomController;
			clownRoomController4.GoldGoalReachedEvent = (ClownRoomEventHandler)Delegate.Remove(clownRoomController4.GoldGoalReachedEvent, new ClownRoomEventHandler(this.OnGoldGoalReached));
			ClownRoomController clownRoomController5 = this.ClownRoomController;
			clownRoomController5.CurrentGoalAmountChangedEvent = (ClownRoomEventHandler)Delegate.Remove(clownRoomController5.CurrentGoalAmountChangedEvent, new ClownRoomEventHandler(this.OnCurrentGoalAmountChanged));
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x00067CF9 File Offset: 0x00065EF9
	private void OnAmmoChanged()
	{
		this.UpdateHUD();
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x00067D01 File Offset: 0x00065F01
	private void OnCurrentGoalAmountChanged()
	{
		this.UpdateHUD();
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x00067D09 File Offset: 0x00065F09
	private void OnBronzeGoalReached()
	{
		this.m_bronzeBalloon.enabled = true;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x00067D17 File Offset: 0x00065F17
	private void OnSilverGoalReached()
	{
		this.m_silverBalloon.enabled = true;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x00067D25 File Offset: 0x00065F25
	private void OnGoldGoalReached()
	{
		this.m_goldBalloon.enabled = true;
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x00067D34 File Offset: 0x00065F34
	private void UpdateHUD()
	{
		this.m_ammoText.text = string.Format("{0}/{1}", this.ClownRoomController.CurrentAmmo, this.ClownRoomController.StartingAmmo);
		this.m_hitsText.text = this.ClownRoomController.CurrentGoalAmount.ToString();
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x00067D94 File Offset: 0x00065F94
	private void ResetHUD()
	{
		this.m_ammoText.text = string.Format("{0}/{1}", this.ClownRoomController.CurrentAmmo, this.ClownRoomController.StartingAmmo);
		this.m_hitsText.text = this.ClownRoomController.CurrentGoalAmount.ToString();
		this.m_bronzeBalloon.enabled = false;
		this.m_silverBalloon.enabled = false;
		this.m_goldBalloon.enabled = false;
		this.m_bronzeText.text = this.ClownRoomController.BronzeGoal.ToString();
		this.m_silverText.text = this.ClownRoomController.SilverGoal.ToString();
		this.m_goldText.text = this.ClownRoomController.GoldGoal.ToString();
	}

	// Token: 0x04001C8F RID: 7311
	[SerializeField]
	private TMP_Text m_bronzeText;

	// Token: 0x04001C90 RID: 7312
	[SerializeField]
	private Image m_bronzeBalloon;

	// Token: 0x04001C91 RID: 7313
	[SerializeField]
	private TMP_Text m_silverText;

	// Token: 0x04001C92 RID: 7314
	[SerializeField]
	private Image m_silverBalloon;

	// Token: 0x04001C93 RID: 7315
	[SerializeField]
	private TMP_Text m_goldText;

	// Token: 0x04001C94 RID: 7316
	[SerializeField]
	private Image m_goldBalloon;

	// Token: 0x04001C95 RID: 7317
	[SerializeField]
	private TMP_Text m_ammoText;

	// Token: 0x04001C96 RID: 7318
	[SerializeField]
	private TMP_Text m_hitsText;

	// Token: 0x04001C98 RID: 7320
	private Action<MonoBehaviour, EventArgs> m_onEnterClownRoom;

	// Token: 0x04001C99 RID: 7321
	private Action<MonoBehaviour, EventArgs> m_onExitClownRoom;
}
