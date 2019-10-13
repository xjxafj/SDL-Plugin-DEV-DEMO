using Sdl.LanguagePlatform.Core;
using System;
using System.Collections.Generic;
namespace TmxMallPlugIn
{
	internal class TmxMallSegmenter
	{
        /// <summary>
        /// ÎÄ±¾Æ¬¶Î
        /// </summary>
		private List<string> _plaintags;
        /// <summary>
        /// tagsÆ¬¶Î
        /// </summary>
		private List<SegmentElement> _tags;
		public TmxMallSegmenter(Segment sourceSegment)
		{
			TmxMallSegmentElementVisitor tmxMallSegmentElementVisitor = new TmxMallSegmentElementVisitor();
			this._tags = new List<SegmentElement>();
			this._plaintags = new List<string>();
			foreach (SegmentElement current in sourceSegment.Elements)
			{
				SegmentElement item = current.Duplicate();
				current.AcceptSegmentElementVisitor(tmxMallSegmentElementVisitor);
				string plainText = tmxMallSegmentElementVisitor.PlainText;
				bool flag = !string.IsNullOrEmpty(plainText);
				if (flag)
				{
					bool flag2 = plainText.IndexOf("<g") >= 0;
					if (flag2)
					{
						this._tags.Add(item);
						this._plaintags.Add(plainText);
					}
					else
					{
						bool flag3 = plainText.IndexOf("/g>") >= 0;
						if (flag3)
						{
							this._tags.Add(item);
							this._plaintags.Add(plainText);
						}
					}
					tmxMallSegmentElementVisitor.Reset(false);
				}
			}
		}
		public Segment AsSegment(string translated)
		{
			Segment segment = new Segment();
			string text = translated;
			List<string>.Enumerator enumerator = this._plaintags.GetEnumerator();
			Segment result;
			foreach (SegmentElement current in this._tags)
			{
				enumerator.MoveNext();
				int num = text.IndexOf(enumerator.Current);
				bool flag = num < 0;
				if (flag)
				{
					segment.Add(text);
					result = segment;
					return result;
				}
				bool flag2 = num > 0;
				if (flag2)
				{
					string text2 = text.Substring(0, num);
					segment.Add(text2);
					text = text.Substring(num);
				}
				segment.Add(current.Duplicate());
				text = text.Substring(enumerator.Current.Length);
			}
			bool flag3 = !string.IsNullOrEmpty(text);
			if (flag3)
			{
				segment.Add(text);
			}
			result = segment;
			return result;
		}
	}
}
