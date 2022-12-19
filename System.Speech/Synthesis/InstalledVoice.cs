using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.Synthesis;

namespace System.Speech.Synthesis
{
	// Token: 0x0200013C RID: 316
	[DebuggerDisplay("{VoiceInfo.Name} [{Enabled ? \"Enabled\" : \"Disabled\"}]")]
	public class InstalledVoice
	{
		// Token: 0x06000865 RID: 2149 RVA: 0x00025BAA File Offset: 0x00024BAA
		internal InstalledVoice(VoiceSynthesis voiceSynthesizer, VoiceInfo voice)
		{
			this._voiceSynthesizer = voiceSynthesizer;
			this._voice = voice;
			this._enabled = true;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x00025BC7 File Offset: 0x00024BC7
		public VoiceInfo VoiceInfo
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00025BCF File Offset: 0x00024BCF
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x00025BD7 File Offset: 0x00024BD7
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this.SetEnabledFlag(value, true);
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00025BE4 File Offset: 0x00024BE4
		public override bool Equals(object obj)
		{
			InstalledVoice installedVoice = obj as InstalledVoice;
			return installedVoice != null && (this.VoiceInfo.Name == installedVoice.VoiceInfo.Name && this.VoiceInfo.Age == installedVoice.VoiceInfo.Age && this.VoiceInfo.Gender == installedVoice.VoiceInfo.Gender) && this.VoiceInfo.Culture.Equals(installedVoice.VoiceInfo.Culture);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00025C67 File Offset: 0x00024C67
		public override int GetHashCode()
		{
			return this.VoiceInfo.Name.GetHashCode();
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00025C7C File Offset: 0x00024C7C
		internal static InstalledVoice Find(List<InstalledVoice> list, VoiceInfo voiceId)
		{
			foreach (InstalledVoice installedVoice in list)
			{
				if (installedVoice.Enabled && installedVoice.VoiceInfo.Equals(voiceId))
				{
					return installedVoice;
				}
			}
			return null;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00025CE0 File Offset: 0x00024CE0
		internal static InstalledVoice FirstEnabled(List<InstalledVoice> list, CultureInfo culture)
		{
			InstalledVoice installedVoice = null;
			foreach (InstalledVoice installedVoice2 in list)
			{
				if (installedVoice2.Enabled)
				{
					if (Helpers.CompareInvariantCulture(installedVoice2.VoiceInfo.Culture, culture))
					{
						return installedVoice2;
					}
					if (installedVoice == null)
					{
						installedVoice = installedVoice2;
					}
				}
			}
			return installedVoice;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00025D50 File Offset: 0x00024D50
		internal void SetEnabledFlag(bool value, bool switchContext)
		{
			try
			{
				if (this._enabled != value)
				{
					this._enabled = value;
					if (!this._enabled)
					{
						if (this._voice.Equals(this._voiceSynthesizer.CurrentVoice(switchContext).VoiceInfo))
						{
							this._voiceSynthesizer.Voice = null;
						}
					}
					else
					{
						this._voiceSynthesizer.Voice = null;
					}
				}
			}
			catch (InvalidOperationException)
			{
				this._voiceSynthesizer.Voice = null;
			}
		}

		// Token: 0x040005F3 RID: 1523
		private VoiceInfo _voice;

		// Token: 0x040005F4 RID: 1524
		private bool _enabled;

		// Token: 0x040005F5 RID: 1525
		private VoiceSynthesis _voiceSynthesizer;
	}
}
