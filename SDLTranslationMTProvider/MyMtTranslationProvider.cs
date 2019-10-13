using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Community.DeepLMTProvider;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using SDLTranslationMTProvider.Code.Config;

namespace SDLTranslationMTProvider
{

    public class MyMtTranslationProvider : ITranslationProvider
    {

        public static readonly string ListTranslationProviderScheme = "deepltranslationprovider";

        public MyMtTranslationOptions Options
        {
            get;
            set;
        }

        public MyMtTranslationProvider(MyMtTranslationOptions options)
        {
            Options = options;
        }

        public ProviderStatusInfo StatusInfo => new ProviderStatusInfo(true, "Deepl");

        public Uri Uri => new TranslationProviderUriBuilder(ListTranslationProviderScheme).Uri;

        public string Name => "DeepL Translator provider using DeepL Translator ";

        public bool SupportsTaggedInput => true;

        public bool SupportsScoring => false;

        public bool SupportsSearchForTranslationUnits => true;

        public bool SupportsMultipleResults => false;

        public bool SupportsFilters => false;

        public bool SupportsPenalties => true;

        public bool SupportsStructureContext => false;

        public bool SupportsDocumentSearches => false;

        public bool SupportsUpdate => false;

        public bool SupportsPlaceables => false;

        public bool SupportsTranslation => true;

        public bool SupportsFuzzySearch => false;

        public bool SupportsConcordanceSearch => false;

        public bool SupportsSourceConcordanceSearch => false;

        public bool SupportsTargetConcordanceSearch => false;

        public bool SupportsWordCounts => false;

        public TranslationMethod TranslationMethod => TranslationMethod.MachineTranslation;

        public bool IsReadOnly => true;

        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            return new MyMtTranslationProviderLanguageDirection(this, languageDirection);
        }

        public void LoadState(string translationProviderState)
        {

        }

        public void RefreshStatusInfo()
        {

        }

        public string SerializeState()
        {
            return null;
        }

        public bool SupportsLanguageDirection(LanguagePair languageDirection)
        {

            return true;
            //return
            //    Helpers.IsSuportedLanguagePair(languageDirection.SourceCulture.TwoLetterISOLanguageName.ToUpper(),
            //    languageDirection.TargetCulture.TwoLetterISOLanguageName.ToUpper());
        }
    }

}

