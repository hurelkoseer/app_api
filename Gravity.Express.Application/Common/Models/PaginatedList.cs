﻿using Microsoft.EntityFrameworkCore;

namespace Gravity.Express.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int count, int pageIndex,
                                                               int pageSize)
        {
            var items = await source.ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var sourceList = source.ToList();
            var items = sourceList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, sourceList.Count, pageIndex, pageSize);
        }
    }
}
