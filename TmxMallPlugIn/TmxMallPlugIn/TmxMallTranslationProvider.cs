using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
namespace TmxMallPlugIn
{
	public class TmxMallTranslationProvider : ITranslationProvider
	{
		public const string ProviderScheme = "tmxmallprovider";
		public const string SupportLanguageCode = "zh_en_ko_ja_fr_de_ru_es_ar_pt_th_vi_fil_my_id_km_lo_ms_el_it_tr_uk_sv_cs_sk_sl_pl_da_nl_fi_hu_hi_he_bn_hy_bo_ug_ii_la_ro_bg_hr_sq_mk_et_lt";
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		public string Name
		{
			get
			{
				return PluginResources.Plugin_NiceName;
			}
		}
		public TmxMallOptions Options
		{
			get;
			set;
		}
		public ProviderStatusInfo StatusInfo
		{
			get
			{
				return new ProviderStatusInfo(true, PluginResources.Plugin_NiceName);
			}
		}
		public bool SupportsConcordanceSearch
		{
			get
			{
				return true;
			}
		}
		public bool SupportsDocumentSearches
		{
			get
			{
				return false;
			}
		}
		public bool SupportsFilters
		{
			get
			{
				return false;
			}
		}
		public bool SupportsFuzzySearch
		{
			get
			{
				return true;
			}
		}
		public bool SupportsMultipleResults
		{
			get
			{
				return true;
			}
		}
		public bool SupportsPenalties
		{
			get
			{
				return false;
			}
		}
		public bool SupportsPlaceables
		{
			get
			{
				return false;
			}
		}
		public bool SupportsScoring
		{
			get
			{
				return true;
			}
		}
		public bool SupportsSearchForTranslationUnits
		{
			get
			{
				return true;
			}
		}
		public bool SupportsSourceConcordanceSearch
		{
			get
			{
				return true;
			}
		}
		public bool SupportsStructureContext
		{
			get
			{
				return false;
			}
		}
		public bool SupportsTaggedInput
		{
			get
			{
				return false;
			}
		}
		public bool SupportsTargetConcordanceSearch
		{
			get
			{
				return true;
			}
		}
		public bool SupportsTranslation
		{
			get
			{
				return true;
			}
		}
		public bool SupportsUpdate
		{
			get
			{
				return true;
			}
		}
		public bool SupportsWordCounts
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public TranslationMethod TranslationMethod
		{
			get
			{
				return TranslationMethod.TranslationMemory;
			}
		}
		public Uri Uri
		{
			get
			{
				return this.Options.Uri;
			}
		}
		public TmxMallTranslationProvider(TmxMallOptions options)
		{
			this.Options = options;
		}
		public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
		{
			return new MyTranslationProviderLanguageDirection(this, languageDirection);
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
			return "zh_en_ko_ja_fr_de_ru_es_ar_pt_th_vi_fil_my_id_km_lo_ms_el_it_tr_uk_sv_cs_sk_sl_pl_da_nl_fi_hu_hi_he_bn_hy_bo_ug_ii_la_ro_bg_hr_sq_mk_et_lt".Contains(languageDirection.SourceCulture.TwoLetterISOLanguageName) && "zh_en_ko_ja_fr_de_ru_es_ar_pt_th_vi_fil_my_id_km_lo_ms_el_it_tr_uk_sv_cs_sk_sl_pl_da_nl_fi_hu_hi_he_bn_hy_bo_ug_ii_la_ro_bg_hr_sq_mk_et_lt".Contains(languageDirection.TargetCulture.TwoLetterISOLanguageName);
		}
		public void UpdateOptions(TmxMallOptions options)
		{
			this.Options = options;
		}
	}
}
