using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace CSMSBE.Infrastructure.Implements
{
    public class PagedList<TItem> : IPagedList<TItem>
    {
        public TItem[] Items { get; set; }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public PagedList() { }
        public PagedList(IEnumerable<TItem> items, int pageIndex, int pageSize, int totalItemCount)
        {
            Items = items.ToArray();
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            PageCount = (int) Math.Ceiling(totalItemCount / (double)pageSize);
            HasPreviousPage = PageIndex > 0;
            HasNextPage = PageIndex + 1 < PageCount;
        }
    }
}
