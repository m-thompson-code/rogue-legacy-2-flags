using System;
using FMOD.Studio;

namespace RLAudio
{
	// Token: 0x02000E49 RID: 3657
	public static class AnimBehaviourEventEmitterUtility
	{
		// Token: 0x0600671F RID: 26399 RVA: 0x0017C838 File Offset: 0x0017AA38
		public static PARAMETER_ID GetParameterID(EventDescription eventDescription, string parameterName)
		{
			PARAMETER_DESCRIPTION parameter_DESCRIPTION;
			eventDescription.getParameterDescriptionByName(parameterName, out parameter_DESCRIPTION);
			return parameter_DESCRIPTION.id;
		}
	}
}
