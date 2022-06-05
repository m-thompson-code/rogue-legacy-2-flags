using System;
using FMOD.Studio;

namespace RLAudio
{
	// Token: 0x020008DB RID: 2267
	public static class AnimBehaviourEventEmitterUtility
	{
		// Token: 0x06004A6F RID: 19055 RVA: 0x0010BFD4 File Offset: 0x0010A1D4
		public static PARAMETER_ID GetParameterID(EventDescription eventDescription, string parameterName)
		{
			PARAMETER_DESCRIPTION parameter_DESCRIPTION;
			eventDescription.getParameterDescriptionByName(parameterName, out parameter_DESCRIPTION);
			return parameter_DESCRIPTION.id;
		}
	}
}
