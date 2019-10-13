using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
namespace TmxMallPlugIn
{
	[TranslationProviderFactory(Id = "TmxMallPlugInFactory", Name = "TmxMallPlugInFactory", Description = "TmxMallPlugInFactory")]
	public class TmxMallPlugInFactory : ITranslationProviderFactory
	{
		public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
		{
			bool flag = !this.SupportsTranslationProviderUri(translationProviderUri);
			if (flag)
			{
				throw new Exception("Cannot handle URI.");
			}
			return new TmxMallTranslationProvider(new TmxMallOptions(translationProviderUri));
		}
		public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
		{
			TranslationProviderInfo translationProviderInfo = new TranslationProviderInfo();
			translationProviderInfo.TranslationMethod= TranslationMethod.TranslationMemory;
			translationProviderInfo.Name=PluginResources.Plugin_NiceName;
			return translationProviderInfo;
		}
		public bool SupportsTranslationProviderUri(Uri translationProviderUri)
		{
			bool flag = translationProviderUri == null;
			if (flag)
			{
				throw new ArgumentNullException("Translation provider URI not supported.");
			}
			return string.Equals(translationProviderUri.Scheme, "tmxmallprovider", StringComparison.OrdinalIgnoreCase);
		}
	}
}
