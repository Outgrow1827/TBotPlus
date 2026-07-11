namespace TBotPlus.Web.Models;

// A single boolean flag inside instance_settings.json that the UI lets the user flip.
// Path is a Newtonsoft JSONPath expression relative to the settings root (e.g. "Brain.AutoDefence.Active").
public sealed class FeatureToggle
{
	public string Path { get; init; } = string.Empty;
	public string Label { get; init; } = string.Empty;
}
