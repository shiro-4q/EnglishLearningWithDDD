using ListeningService.Domain.ValueObjects;

namespace ListeningService.Domain.Interfaces
{
    public interface ISubtitleParser
    {
        /// <summary>
        /// 解析器是否能够解析当前字幕类型
        /// </summary>
        /// <param name="subtitleType">字幕类型</param>
        /// <returns></returns>
        bool CanParse(string subtitleType);

        /// <summary>
        /// 解析原字幕为句子
        /// </summary>
        /// <param name="subtitle">字幕</param>
        /// <returns></returns>
        IEnumerable<Sentence> Parse(string subtitle);
    }
}
