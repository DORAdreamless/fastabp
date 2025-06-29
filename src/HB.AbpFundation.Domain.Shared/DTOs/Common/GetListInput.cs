using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.DTOs.Common
{
    /// <summary>
    /// 基础分页查询参数
    /// </summary>
    public class GetListInput : PagedAndSortedResultRequestDto, IReadOnlyInput
    {
        /// <summary>
        /// 关键词查询
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 是否从读库查询
        /// </summary>
        public bool ReadOnly { get; set; } = true;


        //private int _pageIndex = 1;
        ///// <summary>
        ///// 第几页
        ///// </summary>
        //public int PageIndex
        //{
        //    get
        //    {
        //        if (SkipCount > 0)
        //        {
        //            return SkipCount / MaxResultCount + 1;
        //        }
        //        return _pageIndex;
        //    }
        //    set
        //    {
        //        _pageIndex = value;
        //    }
        //}
        //private int _pageSize = 20;
        ///// <summary>
        ///// 每页多少条
        ///// </summary>
        //public int PageSize
        //{
        //    get
        //    {

        //        if (MaxMaxResultCount > 0)
        //        {
        //            return MaxMaxResultCount;
        //        }
        //        return _pageSize;
        //    }
        //    set
        //    {
        //        _pageSize = value;
        //    }
        //}

        //public long GetPageCount(long totalCount)
        //{
        //    var  pageCount = totalCount/MaxMaxResultCount;
        //    if (totalCount % MaxMaxResultCount > 0)
        //    {
        //        pageCount++;
        //    }
        //    return pageCount;
        //}
    }
}
