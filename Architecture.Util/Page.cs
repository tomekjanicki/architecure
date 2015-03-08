namespace Architecture.Util
{
    public struct Page
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

    }
}