using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using TBotPlus.Web.Models;

namespace TBotPlus.Web.Services {
	// Read-only access to TBot's daily CSV log files, straight off disk - no dependency on TBot.WebUI
	// or the SignalR log hub being available, so TBotPlus can show logs whether or not TBot's web
	// server is running.
	public class LogFileReader {
		private readonly TBotPathsOptions _paths;

		public LogFileReader(IOptions<TBotPathsOptions> paths) {
			_paths = paths.Value;
		}

		private string GetLogPath(DateTime date) =>
			Path.Combine(_paths.BotOutputPath, $"TBot{date:yyyyMMdd}.csv");

		public async Task<List<LogEntry>> ReadAsync(DateTime date, string? search = null, string? level = null, string? sender = null) {
			var result = new List<LogEntry>();
			string path = GetLogPath(date);
			if (!File.Exists(path))
				return result;

			using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using var reader = new StreamReader(stream);
			// TBot's CSV header is lower-case ("type,sender,datetime,message"); match case-insensitively
			// against the PascalCase LogEntry properties instead of requiring an exact name match.
			var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
				PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
			};
			using var csv = new CsvReader(reader, config);

			await foreach (var record in csv.GetRecordsAsync<LogEntry>()) {
				if (!string.IsNullOrEmpty(search) && !record.Message.Contains(search, StringComparison.InvariantCultureIgnoreCase))
					continue;
				if (!string.IsNullOrEmpty(level) && !string.Equals(record.Type, level, StringComparison.InvariantCultureIgnoreCase))
					continue;
				if (!string.IsNullOrEmpty(sender) && sender != "All" && !string.Equals(record.Sender, sender, StringComparison.InvariantCultureIgnoreCase))
					continue;
				result.Add(record);
			}

			return result;
		}

		public bool LogFileExists(DateTime date) => File.Exists(GetLogPath(date));
	}
}
