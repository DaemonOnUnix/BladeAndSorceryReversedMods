using System;

namespace System.Speech.Internal
{
	// Token: 0x0200008C RID: 140
	internal interface IAsyncDispatch
	{
		// Token: 0x06000499 RID: 1177
		void Post(object evt);

		// Token: 0x0600049A RID: 1178
		void Post(object[] evt);

		// Token: 0x0600049B RID: 1179
		void PostOperation(Delegate callback, params object[] parameters);
	}
}
