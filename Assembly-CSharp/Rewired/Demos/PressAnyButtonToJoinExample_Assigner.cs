using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE4 RID: 3812
	[AddComponentMenu("")]
	public class PressAnyButtonToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x06006E57 RID: 28247 RVA: 0x0003CAAF File Offset: 0x0003ACAF
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.AssignJoysticksToPlayers();
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x0018AAAC File Offset: 0x00188CAC
		private void AssignJoysticksToPlayers()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				Joystick joystick = joysticks[i];
				if (!ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id) && joystick.GetAnyButtonDown())
				{
					Player player = this.FindPlayerWithoutJoystick();
					if (player == null)
					{
						return;
					}
					player.controllers.AddController(joystick, false);
				}
			}
			if (this.DoAllPlayersHaveJoysticks())
			{
				ReInput.configuration.autoAssignJoysticks = true;
				base.enabled = false;
			}
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x0018AB30 File Offset: 0x00188D30
		private Player FindPlayerWithoutJoystick()
		{
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].controllers.joystickCount <= 0)
				{
					return players[i];
				}
			}
			return null;
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x0003CABF File Offset: 0x0003ACBF
		private bool DoAllPlayersHaveJoysticks()
		{
			return this.FindPlayerWithoutJoystick() == null;
		}
	}
}
