namespace Architecture.Util
{
    public struct PageAndSortCriteria
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
    }
}