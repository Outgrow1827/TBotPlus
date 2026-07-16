namespace TBotPlus.Web.Models {
	// Mirrors the "type,sender,datetime,message" CSV shape of TBot's daily log files.
	public class LogEntry {
		public string Type { get; set; } = "";
		public string Sender { get; set; } = "";
		public string DateTime { get; set; } = "";
		public string Message { get; set; } = "";
	}
}
