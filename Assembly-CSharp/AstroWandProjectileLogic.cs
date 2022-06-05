using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class AstroWandProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B23 RID: 11043 RVA: 0x00092393 File Offset: 0x00090593
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerCastingAstroWand = new Action<MonoBehaviour, EventArgs>(this.OnPlayerCastingAstroWand);
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x000923AD File Offset: 0x000905AD
	private void OnEnable()
	{
		base.SourceProjectile.DontFireDeathRelay = false;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerCastingAstroWand, this.m_onPlayerCastingAstroWand);
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x000923C8 File Offset: 0x000905C8
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerCastingAstroWand, this.m_onPlayerCastingAstroWand);
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x000923D7 File Offset: 0x000905D7
	private void OnPlayerCastingAstroWand(MonoBehaviour sender, EventArgs args)
	{
		base.SourceProjectile.DontFireDeathRelay = true;
		base.SourceProjectile.FlagForDestruction(null);
	}

	// Token: 0x0400231D RID: 8989
	private Action<MonoBehaviour, EventArgs> m_onPlayerCastingAstroWand;
}
