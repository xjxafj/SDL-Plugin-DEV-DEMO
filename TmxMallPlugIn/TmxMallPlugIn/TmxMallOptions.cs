using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
namespace TmxMallPlugIn
{
	public class TmxMallOptions
	{
		private TranslationProviderUriBuilder _uriBuilder;
		private static Uri _uri = new TranslationProviderUriBuilder("tmxmallprovider").Uri;
		public static string registerUrl = "http://www.tmxmall.com/user/register";
		public static string verifyUrl = "http://api.tmxmall.com/v1/http/clientIdVerify";
		public static string getTranslationUrl = "http://api.tmxmall.com/v1/http/translate";
		public static string setTranslationUrl = "http://www.tmxmall.com/v1/http/set";
		public static string errorHandleUrl = "http://www.tmxmall.com/static/fail.html";
		public static string tradosVersion = "2019";
		public string TmxMallUserName
		{
			get
			{
                //付加待定
                return "xjxafj@126.com";
                return this.GetStringParameter("TmxMallUserName");
			}
			set
			{
				this.SetStringParameter("TmxMallUserName", value);
			}
		}
		public string TmxMallClientID
		{
			get
			{
                //付加待定
                return "fe2c6538587419c9ebcc516b3bb380f8";
				return this.GetStringParameter("TmxMallClientID");
			}
			set
			{
				this.SetStringParameter("TmxMallClientID", value);
			}
		}
		public string Num
		{
			get
			{
                //付加待定
                return "1";
                return this.GetStringParameter("Num");
			}
			set
			{
				this.SetStringParameter("Num", value);
			}
		}
        /// <summary>
        /// 1-5的匹配结果数量
        /// </summary>
		public string GlsNum
		{
			get
			{
                //付加待定
                return "1";
                return this.GetStringParameter("GlsNum");
			}
			set
			{
				this.SetStringParameter("GlsNum", value);
			}
		}
        /// <summary>
        /// 最小匹配率
        /// </summary>
		public string Fuzzy
		{
			get
			{
                return "1";
				return this.GetStringParameter("Fuzzy");
			}
			set
			{
				this.SetStringParameter("Fuzzy", value);
			}
		}
		public string ProxyIP
		{
			get
			{
				return this.GetStringParameter("ProxyIP");
			}
			set
			{
				this.SetStringParameter("ProxyIP", value);
			}
		}
		public string ProxyPort
		{
			get
			{
				return this.GetStringParameter("ProxyPort");
			}
			set
			{
				this.SetStringParameter("ProxyPort", value);
			}
		}
		public string LastError
		{
			get
			{
				return this.GetStringParameter("LastError");
			}
			set
			{
				this.SetStringParameter("LastError", value);
			}
		}
		public Uri Uri
		{
			get
			{
				return this._uriBuilder.Uri;
			}
		}
		public TmxMallOptions()
		{
			this._uriBuilder = new TranslationProviderUriBuilder("tmxmallprovider");
		}
		public TmxMallOptions(Uri uri)
		{
			this._uriBuilder = new TranslationProviderUriBuilder(uri);
		}
		private bool GetBoolParameter(string p)
		{
            //付加待定
            //bool flag = !(this._uriBuilder.Item(p) == "true");
            bool flag = false; 
                //= !(this._uriBuilder.);
            return true;
		}
		private string GetStringParameter(string p)
		{
            //付加待定
            return "";
            //return this._uriBuilder.Item(p);
		}
		private void SetBoolParameter(string p, bool value)
		{
            //付加待定

           // this._uriBuilder.Item(p, value ? "true" : "false");
           //this.SetBoolParameter
		}
		private void SetStringParameter(string p, string value)
		{
            //付加待定
            ///this._uriBuilder.Item(p, value);
            //this._uriBuilder.UserName
            //this.TmxMallUserName = value;
		}
		public void ToCredentialStore(ITranslationProviderCredentialStore credentialStore)
		{
			bool flag = credentialStore.GetCredential(TmxMallOptions._uri) != null;
			if (flag)
			{
				credentialStore.RemoveCredential(TmxMallOptions._uri);
			}
			string text = string.Concat(new string[]
			{
				this.TmxMallUserName,
				":",
				this.TmxMallClientID,
				":",
				this.Num,
				":",
				this.Fuzzy,
				":",
				this.GlsNum,
				":",
				this.LastError,
				":",
				this.ProxyIP,
				":",
				this.ProxyPort
			});
			TranslationProviderCredential translationProviderCredential = new TranslationProviderCredential(text, true);
			credentialStore.AddCredential(TmxMallOptions._uri, translationProviderCredential);
		}
		public static TmxMallOptions FromCredentialStore(ITranslationProviderCredentialStore credentialStore)
		{
			TranslationProviderCredential credential = credentialStore.GetCredential(TmxMallOptions._uri);
			bool flag = credential == null;
			TmxMallOptions result;
			if (flag)
			{
				result = new TmxMallOptions();
			}
			else
			{
				string credential2 = credential.Credential;
				bool flag2 = string.IsNullOrEmpty(credential2);
				if (flag2)
				{
					result = new TmxMallOptions();
				}
				else
				{
					string[] array = credential2.Split(new char[]
					{
						':'
					});
					TmxMallOptions tmxMallOptions = new TmxMallOptions();
					bool flag3 = array.Length == 8;
					if (flag3)
					{
						tmxMallOptions.TmxMallUserName = array[0];
						tmxMallOptions.TmxMallClientID = array[1];
						tmxMallOptions.Num = array[2];
						tmxMallOptions.Fuzzy = array[3];
						tmxMallOptions.GlsNum = array[4];
						tmxMallOptions.LastError = array[5];
						tmxMallOptions.ProxyIP = array[6];
						tmxMallOptions.ProxyPort = array[7];
					}
					result = tmxMallOptions;
				}
			}
			return result;
		}
	}
}
