namespace TBotPlus.Web.Models {
	// Points TBotPlus at the user's TBot "bin/" output folder, configured via appsettings.json.
	public class TBotPathsOptions {
		public const string SectionName = "TBotPaths";

		// Folder containing settings.json, instance_settings.json and the TBot{yyyyMMdd}.csv log files.
		public string BotOutputPath { get; set; } = "";
	}
}
