using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class Fart_Trait : BaseTrait, IAudioEventEmitter
{
	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x06002010 RID: 8208 RVA: 0x00066176 File Offset: 0x00064376
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Fart;
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x06002011 RID: 8209 RVA: 0x0006617D File Offset: 0x0006437D
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

	// Token: 0x06002012 RID: 8210 RVA: 0x0006619E File Offset: 0x0006439E
	protected override void Awake()
	{
		base.Awake();
		this.m_onAbilityCast = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCast);
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000661B8 File Offset: 0x000643B8
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDash, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onAbilityCast);
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000661D4 File Offset: 0x000643D4
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDash, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onAbilityCast);
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000661F0 File Offset: 0x000643F0
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

	// Token: 0x04001C48 RID: 7240
	private string m_description = string.Empty;

	// Token: 0x04001C49 RID: 7241
	private Action<MonoBehaviour, EventArgs> m_onAbilityCast;
}
