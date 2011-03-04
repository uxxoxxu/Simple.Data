﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using System.IO;
using Simple.Data.Ado;
using Simple.Data.SqlServer;

namespace Simple.Data.SqlTest
{
    /// <summary>
    /// Summary description for FindTests
    /// </summary>
    [TestFixture]
    public class FindTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            DatabaseHelper.Reset();
        }

        [Test]
        public void TestProviderIsSqlProvider()
        {
            var provider = ProviderHelper.GetProviderByConnectionString(Properties.Settings.Default.ConnectionString);
            Assert.IsInstanceOf(typeof(SqlConnectionProvider), provider);
        }

        [Test]
        public void TestFindById()
        {
            var db = DatabaseHelper.Open();
            var user = db.Users.FindById(1);
            Assert.AreEqual(1, user.Id);
        }

        [Test]
        public void TestFindAllByName()
        {
            var db = DatabaseHelper.Open();
            IEnumerable<User> users = db.Users.FindAllByName("Bob").Cast<User>();
            Assert.AreEqual(1, users.Count());
        }

        [Test]
        public void TestFindAllByPartialName()
        {
            var db = DatabaseHelper.Open();
            IEnumerable<User> users = db.Users.FindAll(db.Users.Name.Like("Bob")).ToList<User>();
            Assert.AreEqual(1, users.Count());
        }

        [Test]
        public void TestAllCount()
        {
            var db = DatabaseHelper.Open();
            var count = db.Users.All().ToList().Count;
            Assert.AreEqual(3, count);
        }

        [Test]
        public void TestImplicitCast()
        {
            var db = DatabaseHelper.Open();
            User user = db.Users.FindById(1);
            Assert.AreEqual(1, user.Id);
        }

        [Test]
        public void TestImplicitEnumerableCast()
        {
            var db = DatabaseHelper.Open();
            foreach (User user in db.Users.All())
            {
                Assert.IsNotNull(user);
            }
        }

        [Test]
        public void TestFindWithSchemaQualification()
        {
            var db = DatabaseHelper.Open();
            
            var dboActual = db.dbo.SchemaTable.FindById(1);
            var testActual = db.test.SchemaTable.FindById(1);

            Assert.AreEqual("Pass", dboActual.Description);
            Assert.IsNull(testActual);
        }

        [Test]
        public void TestFindWithCriteriaAndSchemaQualification()
        {
            var db = DatabaseHelper.Open();

            var dboActual = db.dbo.SchemaTable.Find(db.dbo.SchemaTable.Id == 1);

            Assert.AreEqual("Pass", dboActual.Description);
        }
    }
}
