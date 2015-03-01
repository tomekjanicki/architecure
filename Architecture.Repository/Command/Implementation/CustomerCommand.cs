using System;
using System.Collections.Generic;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;
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

        public int InsertCustomer(InsertCustomer insertCustomer)
        {
            return ExecuteScalar<int>("INSERT INTO DBO.CUSTOMERS (NAME, MAIL) OUTPUT INSERTED.ID VALUES(@NAME, @MAIL)", new { NAME = insertCustomer.Name, MAIL = insertCustomer.Mail });
        }

        public bool IsMailUnique(IsMailUnique isMailUnique)
        {
            var r = QueryReturnsFirstOrDefault<int>("SELECT CASE WHEN @ID IS NULL THEN (SELECT COUNT(*) FROM DBO.CUSTOMERS WHERE MAIL = @MAIL) ELSE (SELECT COUNT(*) FROM DBO.CUSTOMERS WHERE MAIL = @MAIL AND ID NOT IN (@ID)) END", new { MAIL = isMailUnique.Mail, ID = isMailUnique.CustomerId });
            return r == 0;
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
            if (string.IsNullOrEmpty(modelColumn))
                return "NAME ASC";
            var arguments = modelColumn.Split(' ');
            if (arguments.Length != 2)
                return "NAME ASC";
            var ascending = arguments[1].ToLowerInvariant() == "asc";
            var column = arguments[0].ToUpperInvariant();
            return string.Format("{0} {1}", column, ascending ? "ASC" : "DESC");
        }

    }
}