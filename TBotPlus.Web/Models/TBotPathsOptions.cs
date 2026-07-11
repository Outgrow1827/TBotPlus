namespace TBotPlus.Web.Models {
	// Points TBotPlus at wherever the user's TBot "bin/" output actually lives (locally, on a VM, or
	// elsewhere) - configured via appsettings.json, never hardcoded, since TBotPlus must not depend on
	// or reference the TBot repo/build in any way.
	public class TBotPathsOptions {
		public const string SectionName = "TBotPaths";

		// Folder containing settings.json, instance_settings.json (or other alias-named instance
		// files) and the TBot{yyyyMMdd}.csv log files - i.e. TBot's published "bin" output directory.
		public string BotOutputPath { get; set; } = "";
	}
}
