﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Architecture.Repository.Exception;
using Architecture.Util;
using Dapper;

namespace Architecture.Repository.Command.Implementation.Base
{
    public abstract class BaseCommand
    {
        private readonly ConnectionWithTransaction _connectionWithTransaction;


        protected BaseCommand(ConnectionWithTransaction connectionWithTransaction)
        {
            _connectionWithTransaction = connectionWithTransaction;
        }

        private DbConnection Connection
        {
            get { return _connectionWithTransaction.ConnectionFunc(); }
        }

        private async Task<DbConnection> GetConnectionAsync()
        {
            return await _connectionWithTransaction.ConnectionFuncAsync();
        }

        private DbTransaction Transaction
        {
            get { return _connectionWithTransaction.TransactionFunc(); }
        }

        private bool IsActiveTransaction
        {
            get { return _connectionWithTransaction.IsActiveTransactionFunc(); }
        }

        protected string GetSort(string sort)
        {
            return string.Format(@"ORDER BY {0}", sort);
        }

        protected Tuple<string, DynamicParameters> GetPagedFragment(Page page, string sort)
        {
            const string query = @"{0} OFFSET @SKIP ROWS FETCH NEXT @PAGESIZE ROWS ONLY";
            var dp = new DynamicParameters();
            dp.Add("SKIP", page.Skip);
            dp.Add("PAGESIZE", page.PageSize);
            return new Tuple<string, DynamicParameters>(string.Format(query, GetSort(sort)), dp);
        }

        protected IEnumerable<T> QueryReturnsEnumerable<T>(string sql, object param = null)
        {
            var transaction = IsActiveTransaction ? Transaction : null;
            return Handler.HandleFunction(() => Connection.Query<T>(sql, param, transaction));
        }

        protected T QueryReturnsFirstOrDefault<T>(string sql, object param = null)
        {
            return QueryReturnsEnumerable<T>(sql, param).FirstOrDefault();
        }

        protected async Task<IEnumerable<T>> QueryReturnsEnumerableAsync<T>(string sql, object param = null)
        {
            var transaction = IsActiveTransaction ? Transaction : null;
            return await Handler.HandleFunctionAsync(async () =>
            {
                var con = await GetConnectionAsync();
                return await con.QueryAsync<T>(sql, param, transaction);
            });
        }

        protected async Task<T> QueryReturnsFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            var data = await QueryReturnsEnumerableAsync<T>(sql, param);
            return data.FirstOrDefault();
        }

        protected T ExecuteScalar<T>(string sql, object param = null)
        {
            return Handler.HandleFunction(() => Connection.ExecuteScalar<T>(sql, param, Transaction));
        }

        protected void Execute(string sql, object param = null)
        {
            Handler.HandleAction(() => Connection.Execute(sql, param, Transaction));
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            return await Handler.HandleFunctionAsync(async () =>
            {
                var con = await GetConnectionAsync();
                return await con.ExecuteScalarAsync<T>(sql, param, Transaction);
            });
        }

        protected async Task ExecuteAsync(string sql, object param = null)
        {
            await Handler.HandleActionAsync(async () =>
            {
                var con = await GetConnectionAsync();
                await con.ExecuteAsync(sql, param, Transaction);
            });
        }

    }
}
