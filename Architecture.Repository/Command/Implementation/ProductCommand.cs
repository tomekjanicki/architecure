using System;
using System.Collections.Generic;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Dapper;

namespace Architecture.Repository.Command.Implementation
{
    public class ProductCommand : BaseCommand, IProductCommand
    {
        public ProductCommand(ConnectionWithTransaction connectionWithTransaction)
            : base(connectionWithTransaction)
        {
        }

        public Paged<FindProducts> FindProducts(string code, string name, PageAndSortCriteria pageAndSortCriteria)
        {
            var whereFragment = GetWhereFragment(code, name, null);
            var pagedFragment = GetPagedFragment(Page.FromPageAndSortCriteria(pageAndSortCriteria), GetTranslatedSort(pageAndSortCriteria.Sort));
            var countQuery = string.Format("SELECT COUNT(*) FROM DBO.PRODUCTS {0}", whereFragment.Item1);
            var count = QueryReturnsFirstOrDefault<int>(countQuery, whereFragment.Item2);
            var dataQuery = string.Format(@"SELECT ID, CODE, NAME, PRICE, VERSION, CASE WHEN ID < 20 THEN GETDATE() ELSE NULL END DATE FROM DBO.PRODUCTS {0} {1}", whereFragment.Item1, pagedFragment.Item1);
            whereFragment.Item2.AddDynamicParams(pagedFragment.Item2);
            var data = QueryReturnsEnumerable<FindProducts>(dataQuery, whereFragment.Item2);
            return new Paged<FindProducts>(count, data);
        }

        public IEnumerable<FindProducts> FindProducts(string code, string name, string sort)
        {
            var whereFragment = GetWhereFragment(code, name, "ID < 1000");

            var sortFragment = GetSort(GetTranslatedSort(sort));
            var dataQuery = string.Format(@"SELECT ID, CODE, NAME, PRICE, VERSION FROM DBO.PRODUCTS {0} {1}", whereFragment.Item1, sortFragment);
            return QueryReturnsEnumerable<FindProducts>(dataQuery, whereFragment.Item2);
        }

        public byte[] GetProductVersion(int id)
        {
            return QueryReturnsFirstOrDefault<byte[]>(@"SELECT VERSION FROM DBO.PRODUCTS WHERE ID = @ID", new { ID = id });
        }

        public void DeleteProduct(DeleteProduct deleteProduct)
        {
            Execute(@"DELETE FROM DBO.PRODUCTS WHERE ID = @ID", new { ID = deleteProduct.Id });
        }

        public bool CanDelete(int id)
        {
            return QueryReturnsFirstOrDefault<int>("SELECT COUNT(*) FROM DBO.ORDERSDETAILS WHERE PRODUCTID = @PRODUCTID", new { PRODUCTID = id }) == 0;
        }

        private static Tuple<string, DynamicParameters> GetWhereFragment(string code, string name, string optionalClause)
        {
            var dp = new DynamicParameters();
            var criteria = new List<string>();
            if (!string.IsNullOrEmpty(code))
            {
                criteria.Add("CODE LIKE @CODE");
                dp.Add("CODE", code.ToLikeString());
            }
            if (!string.IsNullOrEmpty(name))
            {
                criteria.Add("NAME LIKE @NAME");
                dp.Add("NAME", name.ToLikeString());
            }
            if (!string.IsNullOrEmpty(optionalClause))
                criteria.Add(optionalClause);
            var where = criteria.Count == 0 ? string.Empty : string.Format(" WHERE {0} ", string.Join(" AND ", criteria));
            return new Tuple<string, DynamicParameters>(where, dp);
        }

        private static string GetTranslatedSort(string modelColumn)
        {
            if (string.IsNullOrEmpty(modelColumn))
                return "CODE ASC";
            var arguments = modelColumn.Split(' ');
            if (arguments.Length != 2)
                return "CODE ASC";
            var ascending = arguments[1].ToLowerInvariant() == "asc";
            var column = arguments[0].ToUpperInvariant();
            return string.Format("{0} {1}", column, ascending ? "ASC" : "DESC");
        }


    }

}
