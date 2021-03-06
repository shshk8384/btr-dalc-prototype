﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using BlogTalkRadio.Common.Data.DependencyInjection;
using Cinchcast.Framework.DependencyInjection.Autofac;

namespace BlogTalkRadio.Common.Data.Orm.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        public void Begin()
        {
            foreach (var dbContextFactory in GetDbContextFactories())
            {
                dbContextFactory.OpenDbContext();
            }
        }

        public void End()
        {
            foreach (var dbContextFactory in GetDbContextFactories())
            {
                EndDbContext(dbContextFactory.GetCurrentDbContext());
            }
        }

        private void EndDbContext(DbContext dbContext)
        {
            if (dbContext != null)
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Error == null)
                    {
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    dbContext.SaveChanges();
                }


                dbContext.Database.Connection.Close();
                dbContext.Database.Connection.Dispose();
                dbContext.Dispose();
            }
        }

        private static IEnumerable<IDbContextFactory> GetDbContextFactories()
        {
            return Ioc.Instance.Container.ResolveAll<IDbContextFactory>();
        }
    }
}