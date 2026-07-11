namespace TBotPlus.Web.Models {
	// Mirrors the "type,sender,datetime,message" CSV shape TBot's Serilog CSV sink writes
	// (TBot{yyyyMMdd}.csv) - independently reimplemented here, not shared/copied code, since
	// TBotPlus must not reference the TBot repo.
	public class LogEntry {
		public string Type { get; set; } = "";
		public string Sender { get; set; } = "";
		public string DateTime { get; set; } = "";
		public string Message { get; set; } = "";
	}
}
