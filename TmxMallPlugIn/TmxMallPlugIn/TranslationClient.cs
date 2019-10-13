using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
namespace TmxMallPlugIn
{
	public class TranslationClient
	{
		private Dictionary<string, List<TranslationResult>> _cache;
		private CultureInfo _sourceLanguage;
		private CultureInfo _targetLanguage;
		private TmxMallOptions _options;
		private const int MAX_CACHE = 10000;
		private object SR = new object();
		public static bool _lastError = false;
		private string[] glsNum = new string[5];
		public TranslationClient(CultureInfo sourceLanguage, CultureInfo targetLanguage, TmxMallOptions options)
		{
			this._sourceLanguage = sourceLanguage;
			this._targetLanguage = targetLanguage;
			this._options = options;
			this._cache = new Dictionary<string, List<TranslationResult>>();
			this.glsNum[0] = "【1】";
			this.glsNum[1] = "【2】";
			this.glsNum[2] = "【3】";
			this.glsNum[3] = "【4】";
			this.glsNum[4] = "【5】";
		}
		private string BuildGetTranslationUrl(string segment, bool concordance)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = string.IsNullOrEmpty(this._options.ProxyIP);
			if (flag)
			{
				stringBuilder.Append(TmxMallOptions.getTranslationUrl + "?text=");
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(this._options.ProxyPort);
				if (flag2)
				{
					stringBuilder.Append("http://" + this._options.ProxyIP + "/v1/http/translate?text=");
				}
				else
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						"http://",
						this._options.ProxyIP,
						":",
						this._options.ProxyPort,
						"/v1/http/translate?text="
					}));
				}
			}
			string value = HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(segment));
			stringBuilder.Append(value);
			stringBuilder.Append("&from=");
			stringBuilder.Append(this._sourceLanguage.TwoLetterISOLanguageName);
			stringBuilder.Append("&to=");
			stringBuilder.Append(this._targetLanguage.TwoLetterISOLanguageName);
			stringBuilder.Append("&gls_num=");
			stringBuilder.Append(this._options.GlsNum);
			stringBuilder.Append("&client_id=");
			stringBuilder.Append(this._options.TmxMallClientID);
			stringBuilder.Append("&user_name=");
			stringBuilder.Append(this._options.TmxMallUserName);
			stringBuilder.Append("&fuzzy_threshold=");
			bool flag3 = !concordance;
			if (flag3)
			{
				stringBuilder.Append(float.Parse(this._options.Fuzzy) / 100f);
			}
			else
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append("&tu_num=");
			stringBuilder.Append(this._options.Num);
			stringBuilder.Append("&de=trados&version=" + TmxMallOptions.tradosVersion);
			return stringBuilder.ToString();
		}
		private string BuildSetTranslationUrl(string sourceText, string targetText)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = string.IsNullOrEmpty(this._options.ProxyIP);
			if (flag)
			{
				stringBuilder.Append(TmxMallOptions.setTranslationUrl + "?seg=");
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(this._options.ProxyPort);
				if (flag2)
				{
					stringBuilder.Append("http://" + this._options.ProxyIP + "/v1/http/set?seg=");
				}
				else
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						"http://",
						this._options.ProxyIP,
						":",
						this._options.ProxyPort,
						"/v1/http/set?seg="
					}));
				}
			}
			string value = HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(sourceText));
			stringBuilder.Append(value);
			stringBuilder.Append("&from=");
			stringBuilder.Append(this._sourceLanguage.TwoLetterISOLanguageName);
			stringBuilder.Append("&to=");
			stringBuilder.Append(this._targetLanguage.TwoLetterISOLanguageName);
			stringBuilder.Append("&tra=");
			value = HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(targetText));
			stringBuilder.Append(value);
			stringBuilder.Append("&client_id=");
			stringBuilder.Append(this._options.TmxMallClientID);
			stringBuilder.Append("&user_name=");
			stringBuilder.Append(this._options.TmxMallUserName);
			stringBuilder.Append("&de=trados&version=" + TmxMallOptions.tradosVersion);
			return stringBuilder.ToString(); 

        }
		private TranslationResult GetResult(JObject jsonMatch)
		{
			TranslationResult result;
			try
			{
				TranslationResult translationResult = new TranslationResult
				{
					Source = HttpUtility.HtmlDecode((string)jsonMatch["src"]),
					Translation = HttpUtility.HtmlDecode((string)jsonMatch["tgt"]),
					Createdby = HttpUtility.HtmlDecode((string)jsonMatch["user"]),
					TmName = HttpUtility.HtmlDecode((string)jsonMatch["tmName"]),
					IsMTMatch = (bool)jsonMatch["mt"]
				};
				float match = 0f;
				try
				{
					match = (float)jsonMatch["fuzzy"];
				}
				catch
				{
				}
				translationResult.Match = match;
				TranslationClient._lastError = false;
				result = translationResult;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		private TranslationResult GetGlsResult(JObject jsonMatch)
		{
			TranslationResult result;
			try
			{
				TranslationResult translationResult = new TranslationResult
				{
					Source = HttpUtility.HtmlDecode((string)jsonMatch["src"]),
					Translation = HttpUtility.HtmlDecode((string)jsonMatch["tgt"])
				};
				TranslationClient._lastError = false;
				result = translationResult;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		private List<TranslationResult> GetResultsFromJson(string json)
		{
			List<TranslationResult> list = new List<TranslationResult>();
			JArray jArray = null;
			List<TranslationResult> result;
			try
			{
				jArray = (JArray)JObject.Parse(json)["tu_set"];
			}
			catch
			{
				string text = JObject.Parse(json)["error_code"].ToString();
				string text2 = JObject.Parse(json)["error_msg"].ToString();
				bool flag = !text.Equals("0");
				if (flag)
				{
					bool flag2 = !TranslationClient._lastError;
					if (flag2)
					{
						TranslationClient._lastError = true;
						bool flag3 = string.IsNullOrEmpty(this._options.ProxyIP);
						if (flag3)
						{
							Process.Start(TmxMallOptions.errorHandleUrl + "?errorCode=" + text);
						}
						else
						{
							bool flag4 = string.IsNullOrEmpty(this._options.ProxyPort);
							if (flag4)
							{
								Process.Start("http://" + this._options.ProxyIP + "/static/fail.html?errorCode=" + text);
							}
							else
							{
								Process.Start(string.Concat(new string[]
								{
									"http://",
									this._options.ProxyIP,
									":",
									this._options.ProxyPort,
									"/static/fail.html?errorCode=",
									text
								}));
							}
						}
					}
					result = list;
					return result;
				}
			}
			bool flag5 = jArray != null && jArray.Count > 0;
			if (flag5)
			{
				for (int i = 0; i < jArray.Count; i++)
				{
					JObject jsonMatch = (JObject)jArray[i];
					TranslationResult result2 = this.GetResult(jsonMatch);
					bool flag6 = result2 != null;
					if (flag6)
					{
						list.Add(result2);
					}
				}
			}
			result = list;
			return result;
		}
		public bool Set(string sourceText, string targetText)
		{
			bool result = false;
			object sR;
			Monitor.Enter(sR = this.SR);
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.BuildSetTranslationUrl(sourceText, targetText));
				httpWebRequest.Timeout = 30000;
				httpWebRequest.Proxy = null;
				using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
				{
					streamReader.ReadToEnd();
				}
				result = true;
			}
			catch
			{
			}
			finally
			{
				Monitor.Exit(sR);
			}
			return result;
		}
		private List<TranslationResult> TM_Translate(string segment, bool concordance)
		{
			List<TranslationResult> resultsFromJson;
			try
			{
				string json = null;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.BuildGetTranslationUrl(segment, concordance));
				httpWebRequest.Timeout = 30000;
				httpWebRequest.Proxy = null;
				using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
				{
					json = streamReader.ReadToEnd();
				}
				resultsFromJson = this.GetResultsFromJson(json);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return resultsFromJson;
		}
		public List<TranslationResult> Translate(string segment, bool concordance)
		{
			object sR = this.SR;
			List<TranslationResult> result;
			lock (sR)
			{
				List<TranslationResult> list = this.TM_Translate(segment, concordance);
				bool flag2 = list == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					for (int i = list.Count; i > 0; i--)
					{
						bool flag3 = string.IsNullOrEmpty(list[i - 1].Translation);
						if (flag3)
						{
							list.RemoveAt(i - 1);
						}
					}
					result = list;
				}
			}
			return result;
		}
		public List<TranslationResult> Translate(string segment)
		{
			object sR = this.SR;
			List<TranslationResult> result;
			lock (sR)
			{
				List<TranslationResult> list = this.TM_Translate(segment, false);
				bool flag2 = list == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					for (int i = list.Count; i > 0; i--)
					{
						bool flag3 = string.IsNullOrEmpty(list[i - 1].Translation);
						if (flag3)
						{
							list.RemoveAt(i - 1);
						}
					}
					result = list;
				}
			}
			return result;
		}
		public List<TranslationResult> Translate(string segment, CultureInfo sourceLanguage, CultureInfo targetLanguage)
		{
			int num = (this._sourceLanguage != null) ? this._sourceLanguage.LCID : 0;
			int num2 = (this._targetLanguage != null) ? this._targetLanguage.LCID : 0;
			bool flag = sourceLanguage.LCID != num || targetLanguage.LCID != num2;
			if (flag)
			{
				this._cache.Clear();
			}
			this._sourceLanguage = sourceLanguage;
			this._targetLanguage = targetLanguage;
			return this.Translate(segment, false);
		}
	}
}
