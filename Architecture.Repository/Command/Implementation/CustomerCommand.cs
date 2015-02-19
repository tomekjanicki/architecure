using System;
using System.Collections.Generic;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Dapper;

namespace Architecture.Repository.Command.Implementation
{
    public class CustomerCommand : BaseCommand, ICustomerCommand
    {
        public CustomerCommand(ConnectionWithTransaction connectionWithTransaction)
            : base(connectionWithTransaction)
        {
        }

        public Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria)
        {
            var whereFragment = GetWhereFragment(name);
            var pagedFragment = GetPagedFragment(Page.FromPageAndSortCriteria(pageAndSortCriteria), GetTranslatedSort(pageAndSortCriteria.Sort));
            var countQuery = string.Format("SELECT COUNT(*) FROM DBO.CUSTOMERS {0}", whereFragment.Item1);
            var count = QueryReturnsFirstOrDefault<int>(countQuery, whereFragment.Item2);
            var dataQuery = string.Format(@"SELECT ID, NAME, MAIL FROM DBO.CUSTOMERS {0} {1}", whereFragment.Item1, pagedFragment.Item1);
            whereFragment.Item2.AddDynamicParams(pagedFragment.Item2);
            var data = QueryReturnsEnumerable<FindCustomers>(dataQuery, whereFragment.Item2);
            return new Paged<FindCustomers>(count, data);
        }

        public string GetCustomerMail(int id)
        {
            return QueryReturnsFirstOrDefault<string>(@"SELECT MAIL FROM DBO.CUSTOMERS WHERE ID = @ID", new { ID = id });
        }

        private static Tuple<string, DynamicParameters> GetWhereFragment(string name)
        {
            var dp = new DynamicParameters();
            var criteria = new List<string>();
            if (!string.IsNullOrEmpty(name))
            {
                criteria.Add("NAME LIKE @NAME");
                dp.Add("NAME", name.ToLikeString());
            }
            var where = criteria.Count == 0 ? string.Empty : string.Format(" WHERE {0} ", string.Join(" AND ", criteria));
            return new Tuple<string, DynamicParameters>(where, dp);
        }

        private static string GetTranslatedSort(string modelColumn)
        {
            //todo dokończyć
            return "NAME ASC";
        }

    }
}