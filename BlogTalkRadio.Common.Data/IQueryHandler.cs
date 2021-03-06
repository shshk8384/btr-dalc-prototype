﻿using System.Collections.Generic;

namespace BlogTalkRadio.Common.Data
{
    public interface IQueryHandler<T> where T: class, new()
    {
        bool CanHandle(IQuery<T> query);
        int Count(IQuery<T> query = null);
        T Get(IQuery<T> query);
        List<T> Query(IQuery<T> query);
    }
}
