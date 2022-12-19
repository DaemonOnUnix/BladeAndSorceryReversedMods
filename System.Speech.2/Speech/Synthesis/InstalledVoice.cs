using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.Synthesis;

namespace System.Speech.Synthesis
{
	// Token: 0x02000007 RID: 7
	[DebuggerDisplay("{VoiceInfo.Name} [{Enabled ? \"Enabled\" : \"Disabled\"}]")]
	public class InstalledVoice
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002245 File Offset: 0x00000445
		internal InstalledVoice(VoiceSynthesis voiceSynthesizer, VoiceInfo voice)
		{
			this._voiceSynthesizer = voiceSynthesizer;
			this._voice = voice;
			this._enabled = true;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002262 File Offset: 0x00000462
		public VoiceInfo VoiceInfo
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000226A File Offset: 0x0000046A
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002272 File Offset: 0x00000472
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

		// Token: 0x0600000E RID: 14 RVA: 0x0000227C File Offset: 0x0000047C
		public override bool Equals(object obj)
		{
			InstalledVoice installedVoice = obj as InstalledVoice;
			return installedVoice != null && (this.VoiceInfo.Name == installedVoice.VoiceInfo.Name && this.VoiceInfo.Age == installedVoice.VoiceInfo.Age && this.VoiceInfo.Gender == installedVoice.VoiceInfo.Gender) && this.VoiceInfo.Culture.Equals(installedVoice.VoiceInfo.Culture);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022FF File Offset: 0x000004FF
		public override int GetHashCode()
		{
			return this.VoiceInfo.Name.GetHashCode();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002314 File Offset: 0x00000514
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

		// Token: 0x06000011 RID: 17 RVA: 0x00002378 File Offset: 0x00000578
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

		// Token: 0x06000012 RID: 18 RVA: 0x000023E8 File Offset: 0x000005E8
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

		// Token: 0x0400018A RID: 394
		private VoiceInfo _voice;

		// Token: 0x0400018B RID: 395
		private bool _enabled;

		// Token: 0x0400018C RID: 396
		private VoiceSynthesis _voiceSynthesizer;
	}
}
