namespace HB.AbpFundation.DTOs.Common
{
    /// <summary>
    /// 查询基类
    /// </summary>
    public class QueryInput : IReadOnlyInput
    {
        /// <summary>
        /// 从读库查询
        /// </summary>
        public bool ReadOnly { get; set; } = true;
    }

    public class JWTToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; } = "Bearer";

        public int expires_in { get; set; }

        public string refresh_token { get; set; }
    }
}
