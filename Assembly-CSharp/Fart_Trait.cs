using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200059C RID: 1436
public class Fart_Trait : BaseTrait, IAudioEventEmitter
{
	// Token: 0x1700120A RID: 4618
	// (get) Token: 0x06002D34 RID: 11572 RVA: 0x00017804 File Offset: 0x00015A04
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Fart;
		}
	}

	// Token: 0x1700120B RID: 4619
	// (get) Token: 0x06002D35 RID: 11573 RVA: 0x00018F3B File Offset: 0x0001713B
	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(this.m_description))
			{
				this.m_description = this.ToString();
			}
			return this.m_description;
		}
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x00018F5C File Offset: 0x0001715C
	protected override void Awake()
	{
		base.Awake();
		this.m_onAbilityCast = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCast);
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x00018F76 File Offset: 0x00017176
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDash, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onAbilityCast);
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x00018F92 File Offset: 0x00017192
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDash, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onAbilityCast);
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x000C6CDC File Offset: 0x000C4EDC
	private void OnAbilityCast(MonoBehaviour sender, EventArgs args)
	{
		float num = 0.16f;
		if (UnityEngine.Random.Range(0f, 1f) < num)
		{
			ProjectileManager.Instance.AddProjectileToPool("FartProjectile");
			Component playerController = PlayerManager.GetPlayerController();
			Vector3 fart_POSITION = Trait_EV.FART_POSITION;
			ProjectileManager.FireProjectile(playerController.gameObject, "FartProjectile", fart_POSITION, true, 0f, 1f, false, true, true, true);
		}
	}

	// Token: 0x040025AC RID: 9644
	private string m_description = string.Empty;

	// Token: 0x040025AD RID: 9645
	private Action<MonoBehaviour, EventArgs> m_onAbilityCast;
}
