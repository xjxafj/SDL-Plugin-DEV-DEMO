using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using System.Windows.Forms;
namespace TmxMallPlugIn
{
	[TranslationProviderWinFormsUi(Id = "TmxMallPlugInWinFormsUI", Name = "TmxMallPlugInWinFormsUI", Description = "TmxMallPlugInWinFormsUI")]
	public class TmxMallPlugInWinFormsUI : ITranslationProviderWinFormsUI
	{
		public bool SupportsEditing
		{
			get
			{
				return true;
			}
		}
		public string TypeDescription
		{
			get
			{
				return PluginResources.Plugin_Description;
			}
		}
		public string TypeName
		{
			get
			{
				return PluginResources.Plugin_NiceName;
			}
		}
		public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
		{
			TmxMallOptions tmxMallOptions = TmxMallOptions.FromCredentialStore(credentialStore);
			TmxMallConfDialog tmxMallConfDialog = new TmxMallConfDialog(tmxMallOptions);
			bool flag = tmxMallConfDialog.ShowDialog(owner) == DialogResult.OK;
			ITranslationProvider[] result;
			if (flag)
			{
				TmxMallTranslationProvider tmxMallTranslationProvider = new TmxMallTranslationProvider(tmxMallConfDialog.Options);
				tmxMallTranslationProvider.UpdateOptions(tmxMallOptions);
				tmxMallOptions.ToCredentialStore(credentialStore);
				result = new ITranslationProvider[]
				{
					tmxMallTranslationProvider
				};
			}
			else
			{
				result = null;
			}
			return result;
		}
		public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
		{
			TmxMallTranslationProvider tmxMallTranslationProvider = translationProvider as TmxMallTranslationProvider;
			bool flag = tmxMallTranslationProvider != null;
			bool result;
			if (flag)
			{
				TmxMallOptions tmxMallOptions = TmxMallOptions.FromCredentialStore(credentialStore);
				TmxMallConfDialog tmxMallConfDialog = new TmxMallConfDialog(tmxMallOptions);
				bool flag2 = tmxMallConfDialog.ShowDialog() == DialogResult.OK;
				if (flag2)
				{
					tmxMallTranslationProvider.UpdateOptions(tmxMallOptions);
					tmxMallOptions.ToCredentialStore(credentialStore);
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
		{
			return false;
		}
		public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
		{
			TranslationProviderDisplayInfo translationProviderDisplayInfo = new TranslationProviderDisplayInfo();
			translationProviderDisplayInfo.Name=PluginResources.Plugin_NiceName;
			translationProviderDisplayInfo.TranslationProviderIcon=PluginResources.Icon;
			translationProviderDisplayInfo.SearchResultImage=PluginResources.Icon.ToBitmap();
			return translationProviderDisplayInfo;
		}
		public bool SupportsTranslationProviderUri(Uri translationProviderUri)
		{
			bool flag = translationProviderUri == null;
			if (flag)
			{
				throw new ArgumentNullException("URI not supported by the plug-in.");
			}
			return string.Equals(translationProviderUri.Scheme, "tmxmallprovider", StringComparison.CurrentCultureIgnoreCase);
		}
	}
}
