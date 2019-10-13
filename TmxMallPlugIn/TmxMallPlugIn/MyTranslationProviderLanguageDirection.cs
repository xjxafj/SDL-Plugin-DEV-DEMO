using Sdl.Core.Globalization;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Action = Sdl.LanguagePlatform.TranslationMemory.Action;

namespace TmxMallPlugIn
{
	public class MyTranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
	{
		private LanguagePair _languages;
		private TmxMallOptions _options;
		private TmxMallTranslationProvider _provider;
		private TranslationClient _translationClient;
		private TmxMallSegmentElementVisitor _visitor;
		public bool CanReverseLanguageDirection
		{
			get
			{
				return true;
			}
		}
		public CultureInfo SourceLanguage
		{
			get
			{
				return this._languages.SourceCulture;
			}
		}
		public CultureInfo TargetLanguage
		{
			get
			{
				return this._languages.TargetCulture;
			}
		}
		public ITranslationProvider TranslationProvider
		{
			get
			{
				return this._provider;
			}
		}
		public MyTranslationProviderLanguageDirection(TmxMallTranslationProvider provider, LanguagePair languages)
		{
			this._languages = languages;
			this._provider = provider;
			this._options = this._provider.Options;
			this._visitor = new TmxMallSegmentElementVisitor();
			this._translationClient = new TranslationClient(languages.SourceCulture, languages.TargetCulture, this._options);
		}
		public ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)
		{
			return this.AddOrUpdateTranslationUnitsMasked(translationUnits, previousTranslationHashes, settings, null);
		}
		public ImportResult[] AddOrUpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, bool[] mask)
		{
			List<ImportResult> list = new List<ImportResult>();
			int num = 0;
			for (int i = 0; i < translationUnits.Length; i++)
			{
				TranslationUnit translationUnit = translationUnits[i];
				bool flag = mask == null || mask[num];
				if (flag)
				{
					bool flag2 = previousTranslationHashes != null && previousTranslationHashes[num] > 0;
					if (flag2)
					{
						list.Add(this.UpdateTranslationUnit(translationUnit));
					}
					else
					{
						list.Add(this.AddTranslationUnit(translationUnit, settings));
					}
				}
				else
				{
					list.Add(null);
				}
				num++;
			}
			return list.ToArray();
		}
		public ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)
		{
			return this.UpdateTranslationUnit(translationUnit);
		}
		public ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)
		{
			List<ImportResult> list = new List<ImportResult>();
			for (int i = 0; i < translationUnits.Length; i++)
			{
				TranslationUnit translationUnit = translationUnits[i];
				list.Add(this.AddTranslationUnit(translationUnit, settings));
			}
			return list.ToArray();
		}
		public ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask)
		{
			List<ImportResult> list = new List<ImportResult>();
			int num = 0;
			for (int i = 0; i < translationUnits.Length; i++)
			{
				TranslationUnit translationUnit = translationUnits[i];
				bool flag = mask == null || mask[num];
				if (flag)
				{
					ImportResult item = this.AddTranslationUnit(translationUnit, settings);
					list.Add(item);
				}
				else
				{
					list.Add(null);
				}
				num++;
			}
			return list.ToArray();
		}
		private SearchResult CreateSearchResult(TmxMallSegmenter segmenter, TranslationResult result, bool formattingPenalty)
		{
			TranslationUnit translationUnit = new TranslationUnit();
            //付加 测试
            //result.Source = "<g id=\"149\">▶</g>前三句点明了时间、地点和特定环境，抒情主人公在深秋时节，独自一人伫立在橘子洲头，望着湘江水向北奔流不息。";
            //result.Translation = "<g id=\"149\">▶</g>The first three sentences clarify the time, place and specific environment. In the late autumn, the lyrical protagonist stands alone in the head of Orange Island and looks at the Xiangjiang River running northward.";

            translationUnit.SourceSegment=segmenter.AsSegment(result.Source);
			translationUnit.SourceSegment.Culture=this._languages.SourceCulture;
			translationUnit.TargetSegment=segmenter.AsSegment(result.Translation.Replace("\\\"", "\""));
			translationUnit.TargetSegment.Culture=this._languages.TargetCulture;
			translationUnit.ResourceId=new PersistentObjectToken(translationUnit.GetHashCode(), Guid.Empty);
			translationUnit.Origin=TranslationUnitOrigin.TM;
			bool flag = !string.IsNullOrEmpty(result.Createdby);
			if (flag)
			{
				translationUnit.FieldValues.Add(new SingleStringFieldValue(result.Createdby, result.TmName));
			}
			bool isMTMatch = result.IsMTMatch;
			if (isMTMatch)
			{
				translationUnit.Origin=TranslationUnitOrigin.MachineTranslation;
			}
			else
			{
				translationUnit.Origin=TranslationUnitOrigin.TM;
			}
			translationUnit.SystemFields.CreationDate=DateTime.Now;
			translationUnit.SystemFields.CreationUser=result.Createdby;
			translationUnit.SystemFields.ChangeDate=DateTime.Now;
			translationUnit.SystemFields.ChangeUser=result.Createdby;
			translationUnit.SystemFields.UseCount=1;
			translationUnit.SystemFields.UseDate=DateTime.Now;
			translationUnit.SystemFields.UseUser=result.Createdby;
			SearchResult searchResult = new SearchResult(translationUnit);
			searchResult.ScoringResult=new ScoringResult();
			searchResult.ScoringResult.BaseScore=(int)(result.Match * 100f);
			if (formattingPenalty)
			{
                translationUnit.ConfirmationLevel = ConfirmationLevel.Translated;
				Penalty penalty = new Penalty(PenaltyType.TagMismatch, 1);
				searchResult.ScoringResult.ApplyPenalty(penalty);
			}
			else
			{
                translationUnit.ConfirmationLevel = ConfirmationLevel.Translated;

            }
			searchResult.TranslationProposal=translationUnit;
			return searchResult;
		}
		private string GetPlainTextFromSegment(Segment segment, bool tagsAsText)
		{
			this._visitor.Reset(tagsAsText);
			foreach (SegmentElement current in segment.Elements)
			{
                //付加 给每一个翻译片段添加自定义的元素片段(一旦方法被调用,自定义的Tag VisitTag()方法会被调用)
                current.AcceptSegmentElementVisitor(this._visitor);
			}
			return this._visitor.PlainText;
		}
		public SearchResults SearchSegment(SearchSettings settings, Segment segment, bool concordance)
		{
			SearchResults result;
			try
			{
				string plainTextFromSegment = this.GetPlainTextFromSegment(segment, false);
				List<TranslationResult> list = this._translationClient.Translate(plainTextFromSegment, concordance);
				SearchResults searchResults = new SearchResults();
				searchResults.SourceSegment=segment.Duplicate();
				TmxMallSegmenter segmenter = new TmxMallSegmenter(segment);
				foreach (TranslationResult current in list)
				{
					searchResults.Add(this.CreateSearchResult(segmenter, current, segment.HasTags));
				}
				result = searchResults;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		public SearchResults SearchSegment(SearchSettings settings, Segment segment)
		{
			SearchResults result;
			try
			{
				string plainTextFromSegment = this.GetPlainTextFromSegment(segment, false);
				List<TranslationResult> list = this._translationClient.Translate(plainTextFromSegment);
				SearchResults searchResults = new SearchResults();
				searchResults.SourceSegment=segment.Duplicate();
				TmxMallSegmenter segmenter = new TmxMallSegmenter(segment);
				foreach (TranslationResult current in list)
				{
					searchResults.Add(this.CreateSearchResult(segmenter, current, segment.HasTags));
				}
				result = searchResults;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
		{
			SearchResults[] array = new SearchResults[segments.Length];
			for (int i = 0; i < segments.Length; i++)
			{
				array[i] = this.SearchSegment(settings, segments[i]);
			}
			return array;
		}
		public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
		{
			bool flag = segments == null;
			if (flag)
			{
				throw new ArgumentNullException("segments in SearchSegmentsMasked");
			}
			bool flag2 = mask == null || mask.Length != segments.Length;
			if (flag2)
			{
				throw new ArgumentException("mask in SearchSegmentsMasked");
			}
			SearchResults[] array = new SearchResults[segments.Length];
			for (int i = 0; i < segments.Length; i++)
			{
				bool flag3 = mask[i];
				if (flag3)
				{
					array[i] = this.SearchSegment(settings, segments[i]);
				}
				else
				{
					array[i] = null;
				}
			}
			return array;
		}
		public SearchResults SearchText(SearchSettings settings, string segment)
		{
			Segment segment2 = new Segment(this._languages.SourceCulture);
			segment2.Add(segment);
			return this.SearchSegment(settings, segment2, true);
		}
		public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
		{
			return this.SearchSegment(settings, translationUnit.SourceSegment);
		}
		public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
		{
			SearchResults[] array = new SearchResults[translationUnits.Length];
			for (int i = 0; i < translationUnits.Length; i++)
			{
				array[i] = this.SearchSegment(settings, translationUnits[i].SourceSegment);
			}
			return array;
		}
		public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
		{
			List<SearchResults> list = new List<SearchResults>();
			int num = 0;
			for (int i = 0; i < translationUnits.Length; i++)
			{
				TranslationUnit translationUnit = translationUnits[i];
				bool flag = mask == null || mask[num];
				if (flag)
				{
					SearchResults item = this.SearchTranslationUnit(settings, translationUnit);
					list.Add(item);
				}
				else
				{
					list.Add(null);
				}
				num++;
			}
			return list.ToArray();
		}
		public ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)
		{
			string plainTextFromSegment = this.GetPlainTextFromSegment(translationUnit.SourceSegment, true);
			string plainTextFromSegment2 = this.GetPlainTextFromSegment(translationUnit.TargetSegment, true);
			bool flag = this._translationClient.Set(plainTextFromSegment, plainTextFromSegment2);
			ImportResult result;
			if (flag)
			{
				result = new ImportResult();
			}
			else
			{
				result = new ImportResult(Action.Error, ErrorCode.StorageError);
			}
			return result;
		}
		public ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits)
		{
			List<ImportResult> list = new List<ImportResult>();
			for (int i = 0; i < translationUnits.Length; i++)
			{
				TranslationUnit translationUnit = translationUnits[i];
				list.Add(this.UpdateTranslationUnit(translationUnit));
			}
			return list.ToArray();
		}
	}
}
