using System;
using Sdl.Community.DeepLMTProvider;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace SDLTranslationMTProvider
{

    [TranslationProviderFactory(Id = "DeepLMtTranslationProviderFactory",
                             Name = "DeepLMtTranslationProviderFactory",
                             Description = "DeepL Mt Translation Provider")]
    public class MyMtTranslationProviderFactory : ITranslationProviderFactory
    {
        public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            var uri = new Uri("deeplprovider:///");
            var options = new MyMtTranslationOptions();

            if (credentialStore.GetCredential(uri) != null)
            {
                var credentials = credentialStore.GetCredential(uri);
                options.ApiKey = credentials.Credential;
            }
            return new MyMtTranslationProvider(options);
        }

        public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
        {
            var info = new TranslationProviderInfo
            {
                TranslationMethod = TranslationMethod.MachineTranslation,
                //Name = PluginResources.Plugin_NiceName
                Name = "dddfd"
            };
            return info;
        }

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException(nameof(translationProviderUri));
            }

            var supportsProvider = string.Equals(translationProviderUri.Scheme, MyMtTranslationProvider.ListTranslationProviderScheme,
                StringComparison.OrdinalIgnoreCase);
            return supportsProvider;
        }
    }
}
