using TBotPlus.Web.Models;

namespace TBotPlus.Web.Services;

public sealed class LocaleService
{
	public const string DefaultCode = "en";

	public static readonly IReadOnlyList<LanguageOption> Catalog = new List<LanguageOption>
	{
		new() { Code = "en", Name = "English", NativeName = "English" },
		new() { Code = "fr", Name = "French", NativeName = "Français" },
		new() { Code = "de", Name = "German", NativeName = "Deutsch" },
		new() { Code = "es", Name = "Spanish", NativeName = "Español" },
		new() { Code = "pt", Name = "Portuguese", NativeName = "Português" },
		new() { Code = "it", Name = "Italian", NativeName = "Italiano" },
		new() { Code = "pl", Name = "Polish", NativeName = "Polski" },
		new() { Code = "nl", Name = "Dutch", NativeName = "Nederlands" },
		new() { Code = "ru", Name = "Russian", NativeName = "Русский" },
		new() { Code = "tr", Name = "Turkish", NativeName = "Türkçe" },
		new() { Code = "gr", Name = "Greek", NativeName = "Ελληνικά" },
		new() { Code = "cz", Name = "Czech", NativeName = "Čeština" },
		new() { Code = "sk", Name = "Slovak", NativeName = "Slovenčina" },
		new() { Code = "ro", Name = "Romanian", NativeName = "Română" },
		new() { Code = "hu", Name = "Hungarian", NativeName = "Magyar" },
		new() { Code = "hr", Name = "Croatian", NativeName = "Hrvatski" },
		new() { Code = "si", Name = "Slovenian", NativeName = "Slovenščina" },
		new() { Code = "dk", Name = "Danish", NativeName = "Dansk" },
		new() { Code = "no", Name = "Norwegian", NativeName = "Norsk" },
		new() { Code = "se", Name = "Swedish", NativeName = "Svenska" },
		new() { Code = "fi", Name = "Finnish", NativeName = "Suomi" },
		new() { Code = "jp", Name = "Japanese", NativeName = "日本語" },
		new() { Code = "tw", Name = "Chinese (Traditional)", NativeName = "繁體中文" },
		new() { Code = "ar", Name = "Arabic", NativeName = "العربية" },
		new() { Code = "mx", Name = "Spanish (Mexico)", NativeName = "Español (México)" },
		new() { Code = "ba", Name = "Bosnian", NativeName = "Bosanski" },
	};

	public string CurrentCode { get; private set; } = DefaultCode;

	public event Action? Changed;

	public LanguageOption CurrentLanguage =>
		Catalog.FirstOrDefault(l => l.Code == CurrentCode) ?? Catalog[0];

	public void SetCurrent(string code)
	{
		if (Catalog.All(l => l.Code != code) || code == CurrentCode)
		{
			return;
		}

		CurrentCode = code;
		Changed?.Invoke();
	}
}
