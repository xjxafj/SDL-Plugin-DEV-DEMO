using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.Core.Tokenization;
using System;
using System.Globalization;
using System.Text;
namespace TmxMallPlugIn
{
	internal class TmxMallSegmentElementVisitor : ISegmentElementVisitor
	{
        /// <summary>
        /// 新的自定义Tag形式字符串
        /// </summary>
		private StringBuilder _sb = new StringBuilder();
		private bool _tagsAsText = false;
		public string PlainText
		{
			get
			{
				return this._sb.ToString();
			}
		}
		public void Reset(bool tagsAsText)
		{
			this._sb = new StringBuilder();
			this._tagsAsText = tagsAsText;
		}
		public void VisitDateTimeToken(DateTimeToken token)
		{
			this._sb.Append(token.Text);
		}
		public void VisitMeasureToken(MeasureToken token)
		{
			this._sb.Append(token.Text);
		}
		public void VisitNumberToken(NumberToken token)
		{
			this._sb.Append(token.Text);
		}
		public void VisitSimpleToken(SimpleToken token)
		{
			this._sb.Append(token.Text);
		}

        /// <summary>
        /// SegmentElement current in segment.Elements 对象
        /// current.AcceptSegmentElementVisitor(自定义TmxMallSegmentElementVisitor对象)
        /// 
        /// </summary>
        /// <param name="tag"></param>
		public void VisitTag(Tag tag)
		{
			bool tagsAsText = this._tagsAsText;
			if (tagsAsText)
			{
				this._sb.Append(tag.TextEquivalent);
			}
			else
			{
				string text = tag.ToString();
				string text2 = tag.Anchor.ToString(CultureInfo.InvariantCulture);
				int num = text.IndexOf(text2, 1);
				bool flag = num < 0;
				if (flag)
				{
					this._sb.Append(tag.TextEquivalent);
				}
				else
				{
					text = text.Remove(num, text2.Length).Insert(num, "g");
					int num2 = -1;
					try
					{
						int num3 = text.IndexOf("id", num, StringComparison.InvariantCultureIgnoreCase);
						int num4 = text.IndexOf("=", num3 + 2, StringComparison.InvariantCultureIgnoreCase);
						num2 = text.IndexOf(tag.TagID, num4 + 1, StringComparison.InvariantCultureIgnoreCase);
					}
					catch
					{
						this._sb.Append(text);
						return;
					}
					bool flag2 = num2 > 0;
					if (flag2)
					{
						text = text.Insert(num2 + tag.TagID.Length, "\"").Insert(num2, "\"");
					}
					this._sb.Append(text);
				}
			}
		}
		public void VisitTagToken(TagToken token)
		{
			this._sb.Append(token.Text);
		}
		public void VisitText(Text text)
		{
			this._sb.Append(text.Value);
		}
	}
}
