﻿using myCoreMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace myCoreMvc.App.Services
{
    public class WorkItemBizOf : IWorkItemBizOf
    {
        private readonly IDataProvider DataProvider;
        public WorkItem WorkItem { get; }

        public WorkItemBizOf(IDataProvider dataProvider, WorkItem workItem)
        {
            DataProvider = dataProvider;
            WorkItem = workItem;
        }

        TransactionResult IWorkItemBizOf.Save() => DataProvider.Save(WorkItem);
        TransactionResult IWorkItemBizOf.Delete() => DataProvider.Delete<WorkItem>(WorkItem.Id);
    }
}