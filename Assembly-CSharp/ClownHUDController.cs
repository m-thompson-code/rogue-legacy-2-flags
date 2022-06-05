using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005F1 RID: 1521
public class ClownHUDController : MonoBehaviour
{
	// Token: 0x17001286 RID: 4742
	// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x0001997C File Offset: 0x00017B7C
	// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x00019984 File Offset: 0x00017B84
	public ClownRoomController ClownRoomController { get; private set; }

	// Token: 0x06002ED6 RID: 11990 RVA: 0x000C8A0C File Offset: 0x000C6C0C
	private void Awake()
	{
		this.m_onEnterClownRoom = new Action<MonoBehaviour, EventArgs>(this.OnEnterClownRoom);
		this.m_onExitClownRoom = new Action<MonoBehaviour, EventArgs>(this.OnExitClownRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterClownRoom, this.m_onEnterClownRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitClownRoom, this.m_onExitClownRoom);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x0001998D File Offset: 0x00017B8D
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterClownRoom, this.m_onEnterClownRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitClownRoom, this.m_onExitClownRoom);
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x000C8A64 File Offset: 0x000C6C64
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

	// Token: 0x06002ED9 RID: 11993 RVA: 0x000C8B5C File Offset: 0x000C6D5C
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

	// Token: 0x06002EDA RID: 11994 RVA: 0x000199A9 File Offset: 0x00017BA9
	private void OnAmmoChanged()
	{
		this.UpdateHUD();
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x000199A9 File Offset: 0x00017BA9
	private void OnCurrentGoalAmountChanged()
	{
		this.UpdateHUD();
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x000199B1 File Offset: 0x00017BB1
	private void OnBronzeGoalReached()
	{
		this.m_bronzeBalloon.enabled = true;
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x000199BF File Offset: 0x00017BBF
	private void OnSilverGoalReached()
	{
		this.m_silverBalloon.enabled = true;
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x000199CD File Offset: 0x00017BCD
	private void OnGoldGoalReached()
	{
		this.m_goldBalloon.enabled = true;
	}

	// Token: 0x06002EDF RID: 11999 RVA: 0x000C8C4C File Offset: 0x000C6E4C
	private void UpdateHUD()
	{
		this.m_ammoText.text = string.Format("{0}/{1}", this.ClownRoomController.CurrentAmmo, this.ClownRoomController.StartingAmmo);
		this.m_hitsText.text = this.ClownRoomController.CurrentGoalAmount.ToString();
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x000C8CAC File Offset: 0x000C6EAC
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

	// Token: 0x04002647 RID: 9799
	[SerializeField]
	private TMP_Text m_bronzeText;

	// Token: 0x04002648 RID: 9800
	[SerializeField]
	private Image m_bronzeBalloon;

	// Token: 0x04002649 RID: 9801
	[SerializeField]
	private TMP_Text m_silverText;

	// Token: 0x0400264A RID: 9802
	[SerializeField]
	private Image m_silverBalloon;

	// Token: 0x0400264B RID: 9803
	[SerializeField]
	private TMP_Text m_goldText;

	// Token: 0x0400264C RID: 9804
	[SerializeField]
	private Image m_goldBalloon;

	// Token: 0x0400264D RID: 9805
	[SerializeField]
	private TMP_Text m_ammoText;

	// Token: 0x0400264E RID: 9806
	[SerializeField]
	private TMP_Text m_hitsText;

	// Token: 0x04002650 RID: 9808
	private Action<MonoBehaviour, EventArgs> m_onEnterClownRoom;

	// Token: 0x04002651 RID: 9809
	private Action<MonoBehaviour, EventArgs> m_onExitClownRoom;
}
