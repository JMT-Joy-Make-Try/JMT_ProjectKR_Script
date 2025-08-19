namespace JMT.Core.Tool.CSV
{
    public interface ICSVSerializable
    {
        /// <summary>
        /// CSV 문자열을 객체에 로드
        /// </summary>
        void FromCSV(string csvValue);

        /// <summary>
        /// 객체를 CSV 문자열로 변환
        /// </summary>
        string ToCSV();
    }
}
