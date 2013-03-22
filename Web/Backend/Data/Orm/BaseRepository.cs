﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Backend.Data.Queries;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Data.Orm
{
    public abstract class BaseRepository<T>: IRepository<T> where T: class, new()
    {
        private readonly IEnumerable<IQueryInterpreter<T>> _queryInterpreters;

        public BaseRepository(IEnumerable<IQueryInterpreter<T>> queryInterpreters)
        {
            _queryInterpreters = queryInterpreters;
        }

        protected virtual IQueryInterpreter<T> GetQueryInterpreter(IQuery<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query", "The query is required");
            }

            var interpreter = _queryInterpreters.FirstOrDefault(x => x.CanInterpret(query));

            if (interpreter == null)
            {
                throw new Exception(string.Format("There query of type {0} cannot be interpreted", query.GetType()));
            }

            return interpreter;
        }

        public abstract T Get(object id);

        public virtual T Get(IQuery<T> query)
        {
            return GetQueryInterpreter(query).Get(query);
        }

        public virtual int Count(IQuery<T> query = null)
        {
            if (query == null)
            {
                query = new LinqPagedQuery<T>(0, 1);
            }

            return GetQueryInterpreter(query).Count(query);
        }

        public virtual List<T> All(int skip = 0, int take = 1000)
        {
            var query = new LinqPagedQuery<T>(skip, take);
            return GetQueryInterpreter(query).Query(query);
        }

        public virtual List<T> Query(IQuery<T> query)
        {
            return GetQueryInterpreter(query).Query(query);
        }

        public abstract T SaveOrUpdate(T instance);

        public abstract void Delete(object id);

        public abstract void DeleteAll();
    }
}