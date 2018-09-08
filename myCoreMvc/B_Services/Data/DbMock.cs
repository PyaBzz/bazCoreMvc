﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using myCoreMvc.Models;
using PooyasFramework;

namespace myCoreMvc.Services
{
    public class DbMock : IDataProvider
    {
        /*================================  Properties ================================*/

        private List<WorkPlan> WorkPlans { get; set; }
        private List<WorkItem> WorkItems { get; set; }
        private List<User> Users { get; set; }

        /*==================================  Constructors ==================================*/

        public DbMock()
        {
            WorkPlans = new List<WorkPlan>
            {
                new WorkPlan { Id = Guid.Parse("60f9fc29-083f-4ed2-a3e2-3948b503c25f"), Name = "Plan1" },
                new WorkPlan { Id = Guid.Parse("53c88402-4092-4834-8e7f-6ce70057cdc5"), Name = "Plan2" }
            };

            WorkItems = new List<WorkItem>
            {
                new WorkItem { Id = Guid.Parse("7073ad87-4695-4a0b-b2c3-fa794d5ffa21"), Reference = "Wi11", Priority = 1, Name = "FirstItem", WorkPlan = Get<WorkPlan>("60f9fc29-083f-4ed2-a3e2-3948b503c25f")},
                new WorkItem { Id = Guid.Parse("5fc4bfcf-24e0-430a-8889-03b2f31387e1"), Reference = "Wi12", Priority = 2, Name = "SecondItem", WorkPlan = Get<WorkPlan>("60f9fc29-083f-4ed2-a3e2-3948b503c25f")},
                new WorkItem { Id = Guid.Parse("eb66287b-1cde-421e-868e-a0df5b21a90d"), Reference = "Wi21", Priority = 3, Name = "ThirdItem", WorkPlan = Get<WorkPlan>("53c88402-4092-4834-8e7f-6ce70057cdc5")}
            };

            //Task: Hash the PW!
            Users = new List<User>();
            //{
            //    new User {Id = Guid.Parse("5d45a66d-fc2d-4a7f-b9dc-aac9f723f034"),
            //        Name = "Junior", Hash = "jjj", DateOfBirth = new DateTime(2018, 01, 22), Role = AuthConstants.JuniorRoleName },
            //    new User {Id = Guid.Parse("91555540-6137-4668-9d55-5c22471237f3"),
            //        Name = "Senior", Hash = "sss", DateOfBirth = new DateTime(2010, 01, 22), Role = AuthConstants.SeniorRoleName },
            //    new User {Id = Guid.Parse("97ba3d59-a990-4b55-ba91-7865fca0a4a2"),
            //        Name = "Admin", Hash = "aaa", DateOfBirth = new DateTime(2000, 01, 22), Role = AuthConstants.AdminRoleName }
            //};
        }

        /*==================================  Methods =================================*/

        public List<T> GetList<T>()
        {
            var propertyInfos = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);
            var propertyInfo = propertyInfos.SingleOrDefault(pi => pi.PropertyType == typeof(List<T>));
            if (propertyInfo == null) throw new NullReferenceException($"DbMock knows no source collection of type {typeof(T)}.");
            var property = propertyInfo.GetValue(this) as List<T>;
            return property;
        }

        public List<T> GetList<T>(Func<T, bool> func)
        {
            return GetList<T>().Where(i => func(i)).ToList();
        }

        public T Get<T>(Func<T, bool> func)
        {
            return GetList<T>().SingleOrDefault(i => func(i));
        }

        public T Get<T>(Guid id) where T : Thing
        {
            return GetList<T>().SingleOrDefault(i => i.Id == id);
        }

        public T Get<T>(string id) where T : Thing
        {
            return GetList<T>().SingleOrDefault(i => i.Id == Guid.Parse(id));
        }

        public TransactionResult Add<T>(T obj) where T : Thing
        {
            obj.Id = Guid.NewGuid();
            var targetSource = GetList<T>();
            targetSource.Add(obj);
            return TransactionResult.Added;
        }

        public TransactionResult Update<T>(T obj) where T : Thing
        {
            var targetSource = GetList<T>();
            var existingObj = targetSource.SingleOrDefault(e => e.Id == obj.Id);
            if (existingObj == null)
            {
                return TransactionResult.NotFound;
            }
            else
            {
                existingObj.CopyPropertiesFrom(obj);
                return TransactionResult.Updated;
            }
        }

        public TransactionResult Delete<T>(Guid id) where T : Thing
        {
            var targetSource = GetList<T>();
            var existingObj = targetSource.SingleOrDefault(e => e.Id == id);
            if (existingObj == null) return TransactionResult.NotFound;
            targetSource.Remove(existingObj);
            return TransactionResult.Deleted;
        }
    }
}
