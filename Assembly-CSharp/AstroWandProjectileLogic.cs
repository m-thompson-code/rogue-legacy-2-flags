using System;
using UnityEngine;

// Token: 0x02000797 RID: 1943
public class AstroWandProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B6C RID: 15212 RVA: 0x00020A29 File Offset: 0x0001EC29
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerCastingAstroWand = new Action<MonoBehaviour, EventArgs>(this.OnPlayerCastingAstroWand);
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x00020A43 File Offset: 0x0001EC43
	private void OnEnable()
	{
		base.SourceProjectile.DontFireDeathRelay = false;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerCastingAstroWand, this.m_onPlayerCastingAstroWand);
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x00020A5E File Offset: 0x0001EC5E
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerCastingAstroWand, this.m_onPlayerCastingAstroWand);
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x00020A6D File Offset: 0x0001EC6D
	private void OnPlayerCastingAstroWand(MonoBehaviour sender, EventArgs args)
	{
		base.SourceProjectile.DontFireDeathRelay = true;
		base.SourceProjectile.FlagForDestruction(null);
	}

	// Token: 0x04002F31 RID: 12081
	private Action<MonoBehaviour, EventArgs> m_onPlayerCastingAstroWand;
}
