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

        private const string SelectProductQuery = @"SELECT ID, CODE, NAME, PRICE, VERSION, CASE WHEN ID < 20 THEN GETDATE() ELSE NULL END DATE, CASE WHEN O.PRODUCTID IS NULL THEN 1 ELSE 0 END CANDELETE FROM DBO.PRODUCTS P LEFT JOIN (SELECT DISTINCT PRODUCTID FROM DBO.ORDERSDETAILS) O ON P.ID = O.PRODUCTID {0} {1}";

        public Paged<FindProducts> FindProducts(string code, string name, PageAndSortCriteria pageAndSortCriteria)
        {
            var whereFragment = GetWhereFragment(code, name, null);
            var pagedFragment = GetPagedFragment(Page.FromPageAndSortCriteria(pageAndSortCriteria), GetTranslatedSort(pageAndSortCriteria.Sort));
            var countQuery = string.Format("SELECT COUNT(*) FROM DBO.PRODUCTS {0}", whereFragment.Item1);
            var count = QueryReturnsFirstOrDefault<int>(countQuery, whereFragment.Item2);
            var dataQuery = string.Format(SelectProductQuery, whereFragment.Item1, pagedFragment.Item1);
            whereFragment.Item2.AddDynamicParams(pagedFragment.Item2);
            var data = QueryReturnsEnumerable<FindProducts>(dataQuery, whereFragment.Item2);
            return new Paged<FindProducts>(count, data);
        }

        public IEnumerable<FindProducts> FindProducts(string code, string name, string sort)
        {
            var whereFragment = GetWhereFragment(code, name, "ID < 1000");

            var sortFragment = GetSort(GetTranslatedSort(sort));
            var dataQuery = string.Format(SelectProductQuery, whereFragment.Item1, sortFragment);
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

        private Tuple<string, DynamicParameters> GetWhereFragment(string code, string name, string optionalClause)
        {
            var dp = new DynamicParameters();
            var criteria = new List<string>();
            if (!string.IsNullOrEmpty(code))
            {
                var like = GetLikeCaluse("CODE", "CODE", code);
                SetValues(criteria, like, dp);
            }
            if (!string.IsNullOrEmpty(name))
            {
                var like = GetLikeCaluse("NAME", "NAME", name);
                SetValues(criteria, like, dp);
            }
            if (!string.IsNullOrEmpty(optionalClause))
                criteria.Add(optionalClause);
            var where = criteria.Count == 0 ? string.Empty : string.Format(" WHERE {0} ", string.Join(" AND ", criteria));
            return new Tuple<string, DynamicParameters>(where, dp);
        }

        private static void SetValues(ICollection<string> criteria, Tuple<string, Tuple<string, string>> like, DynamicParameters dp)
        {
            criteria.Add(like.Item1);
            dp.Add(like.Item2.Item1, like.Item2.Item2);
        }

        private string GetTranslatedSort(string modelColumn)
        {
            return GetTranslatedSort(modelColumn, string.Format("{0} ASC", Extension.GetPropertyName<FindProducts>(p => p.Code)), new[]
            {
                Extension.GetPropertyName<FindProducts>(p => p.Id),
                Extension.GetPropertyName<FindProducts>(p => p.Code),
                Extension.GetPropertyName<FindProducts>(p => p.Name),
                Extension.GetPropertyName<FindProducts>(p => p.Price),
                Extension.GetPropertyName<FindProducts>(p => p.Date)
            });
        }


    }

}
