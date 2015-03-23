using System;

namespace Architecture.Util
{
    public class Page : IEquatable<Page>
    {
        public readonly int PageSize;
        public readonly int Skip;

        public Page(int pageSize, int skip)
        {
            Extension.EnsureArgumentIsInRange(pageSize > Const.MaxPageSize || pageSize < Const.MinPageSize, string.Format("Page size has to be between {0} and {1}. The value passed is {2}.", Const.MinPageSize, Const.MaxPageSize, pageSize));
            Extension.EnsureArgumentIsInRange(skip < 0, string.Format("Skip has to be equal or greater than 0, but it is {0}", skip));
            PageSize = pageSize;
            Skip = skip;
        }

        public static Page FromPageAndSortCriteria(PageAndSortCriteria pageAndSortCriteria)
        {
            return new Page(pageAndSortCriteria.PageSize, pageAndSortCriteria.Skip);
        }

        public bool Equals(Page other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return PageSize == other.PageSize && Skip == other.Skip;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != GetType()) 
                return false;
            return Equals((Page) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PageSize*397) ^ Skip;
            }
        }

        public static bool operator ==(Page left, Page right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Page left, Page right)
        {
            return !Equals(left, right);
        }


    }
}