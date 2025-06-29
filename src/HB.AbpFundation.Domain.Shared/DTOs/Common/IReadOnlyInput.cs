namespace HB.AbpFundation.DTOs.Common
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public interface IReadOnlyInput
    {
        /// <summary>
        /// 是否从读库查询
        /// </summary>
        bool ReadOnly { get; set; } 

    }
}
