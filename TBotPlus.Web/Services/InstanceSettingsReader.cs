using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TBotPlus.Web.Models;

namespace TBotPlus.Web.Services {
	// Mostly read-only access to TBot's settings.json / instance_settings.json, straight off disk.
	// Only SetBooleanAsync/SetBooleansAsync write, for the feature Active/Enabled toggles - full field
	// editing is deferred until a safe cross-process coordination story exists.
	public class InstanceSettingsReader {
		private readonly TBotPathsOptions _paths;

		public InstanceSettingsReader(IOptions<TBotPathsOptions> paths) {
			_paths = paths.Value;
		}

		private string GlobalSettingsPath => Path.Combine(_paths.BotOutputPath, "settings.json");

		public async Task<List<InstanceSummary>> GetInstancesAsync() {
			var result = new List<InstanceSummary>();
			if (!File.Exists(GlobalSettingsPath))
				return result;

			var global = JObject.Parse(await File.ReadAllTextAsync(GlobalSettingsPath));
			var instances = global["Instances"] as JArray;
			if (instances == null)
				return result;

			foreach (var instance in instances) {
				string fileName = (string?) instance["Settings"] ?? "";
				result.Add(new InstanceSummary {
					Alias = (string?) instance["Alias"] ?? fileName,
					FileName = fileName,
					Exists = File.Exists(Path.Combine(_paths.BotOutputPath, fileName))
				});
			}

			return result;
		}

		public async Task<JObject?> GetInstanceSettingsAsync(string fileName) {
			string path = Path.Combine(_paths.BotOutputPath, fileName);
			if (!File.Exists(path))
				return null;

			return JObject.Parse(await File.ReadAllTextAsync(path));
		}

		// Flips a single boolean identified by a JSONPath (e.g. "Brain.AutoDefence.Active").
		public Task<bool> SetBooleanAsync(string fileName, string jsonPath, bool value) =>
			SetBooleansAsync(fileName, new Dictionary<string, bool> { [jsonPath] = value });

		// Applies several paths in one read-modify-write, so paired flags never land in an
		// inconsistent combination from two separate writes.
		public async Task<bool> SetBooleansAsync(string fileName, IDictionary<string, bool> values) {
			string path = Path.Combine(_paths.BotOutputPath, fileName);
			if (!File.Exists(path))
				return false;

			var root = JObject.Parse(await File.ReadAllTextAsync(path));
			foreach (var (jsonPath, value) in values) {
				var token = root.SelectToken(jsonPath);
				if (token == null)
					return false;
				token.Replace(new JValue(value));
			}

			string tempPath = path + ".tmp";
			await File.WriteAllTextAsync(tempPath, root.ToString(Formatting.Indented));
			File.Replace(tempPath, path, null);
			return true;
		}
	}
}
