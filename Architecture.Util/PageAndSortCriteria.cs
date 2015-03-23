﻿using System;

namespace Architecture.Util
{
    public class PageAndSortCriteria : IEquatable<PageAndSortCriteria>
    {
        public PageAndSortCriteria(int pageSize, int skip, string sort)
        {
            var p = new Page(pageSize, skip);
            PageSize = p.PageSize;
            Skip = p.Skip;
            Sort = sort;
        }

        public readonly int PageSize;

        public readonly int Skip;

        public readonly string Sort;

        public bool Equals(PageAndSortCriteria other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return PageSize == other.PageSize && Skip == other.Skip && string.Equals(Sort, other.Sort);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != GetType()) 
                return false;
            return Equals((PageAndSortCriteria) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PageSize;
                hashCode = (hashCode*397) ^ Skip;
                hashCode = (hashCode*397) ^ (Sort != null ? Sort.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(PageAndSortCriteria left, PageAndSortCriteria right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PageAndSortCriteria left, PageAndSortCriteria right)
        {
            return !Equals(left, right);
        }

    }
}