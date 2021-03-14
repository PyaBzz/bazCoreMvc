﻿using System;
using System.Collections.Generic;
using System.Linq;
using myCoreMvc.Domain;
using Dapper;

namespace myCoreMvc.App.Services
{
    public class WorkplanRepo : IWorkplanRepo
    {
        /*==================================  Interface Methods =================================*/

        public TransactionResult Add(WorkPlan wp)
        {
            using (var conn = SqlConFactory.Get())
            {
                try
                {
                    conn.Execute($"INSERT INTO WorkPlans (Id, Name) VALUES (@Id, @Name)", wp);
                    return TransactionResult.Added;
                }
                catch
                {
                    return TransactionResult.Failed;
                }
            }
        }

        public List<WorkPlan> GetAll()
        {
            using (var conn = SqlConFactory.Get())
            {
                var reader = conn.QueryMultiple($"SELECT * FROM WorkPlans");
                return reader.Read<WorkPlan>().ToList();
            }

        }

        public WorkPlan Get(string id)
        {
            using (var conn = SqlConFactory.Get())
            {
                try
                {
                    return conn.QuerySingle<WorkPlan>($"SELECT * FROM WorkPlans WHERE Id = @Id", new { Id = id });
                }
                catch
                {
                    return null;
                }
            }
        }

        public WorkPlan Get(Guid id) => Get(id.ToString());

        public TransactionResult Update(WorkPlan wp)
        {
            using (var conn = SqlConFactory.Get())
            {
                conn.Execute($"UPDATE WorkPlans SET Name = @Name WHERE Id = @Id", wp);
            }
            return TransactionResult.Updated;
        }

        public TransactionResult Delete(string id)
        {
            using (var conn = SqlConFactory.Get())
            {
                conn.Execute($"DELETE FROM WorkPlans WHERE Id = @Id", new { Id = id });
            }
            return TransactionResult.Deleted;
        }

        public TransactionResult Delete(Guid id) => Delete(id.ToString());
    }
}
