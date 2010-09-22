#region Includes
using System;
using System.Collections.Generic;
using System.Data;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNETObjectsTests.TestClasses;
using NUnit.Framework;
#endregion

namespace KellermanSoftware.CompareNETObjectsTests
{
    [TestFixture(Description = "Tests for CompareObjects"), Category("CompareObjects")]
    public class CompareObjectsTests
    {
        #region Class Variables
        private CompareObjects _compare;
        #endregion

        #region Setup/Teardown

        /// <summary>
        /// Code that is run once for a suite of tests
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {

        }

        /// <summary>
        /// Code that is run once after a suite of tests has finished executing
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }

        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareObjects();
        }

        /// <summary>
        /// Code that is run after each test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _compare = null;
        }
        #endregion

        #region Dataset Tests
        private DataSet CreateMockDataset()
        {
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable("IceCream");
            ds1.Tables.Add(dt);
            dt.Columns.Add("Flavor", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            DataRow dr = dt.NewRow();
            dr["Flavor"] = "Chocolate";
            dr["Price"] = 1.99M;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Flavor"] = "Vanilla";
            dr["Price"] = 1.98M;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Flavor"] = "Banana Prune Delight";
            dr["Price"] = 2.99M;
            dt.Rows.Add(dr);
            return ds1;
        }

        [Test]
        public void DatasetPositiveTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            Assert.IsTrue(_compare.Compare(ds1,ds2));
        }

        [Test]
        public void DatasetNegativeRowTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Rows[0].Delete();
            Assert.IsFalse(_compare.Compare(ds1, ds2));
        }

        [Test]
        public void DatasetNegativeColumnTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Columns.RemoveAt(0);
            Assert.IsFalse(_compare.Compare(ds1, ds2));
        }

        [Test]
        public void DatasetNegativeDataTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Rows[2][0] = "Chunky Chocolate Heaven";
            Assert.IsFalse(_compare.Compare(ds1, ds2));
        }

        [Test]
        public void DataTableNegativeDataTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Rows[2][0] = "Chunky Chocolate Heaven";
            Assert.IsFalse(_compare.Compare(ds1.Tables[0], ds2.Tables[0]));
        }

        [Test]
        public void DataRowNegativeDataTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Rows[2][0] = "Chunky Chocolate Heaven";
            Assert.IsFalse(_compare.Compare(ds1.Tables[0].Rows[2], ds2.Tables[0].Rows[2]));
        }

        #endregion

        #region Indexer Tests

        [Test]
        public void TestIndexerPositive()
        {
            var jane = new Person { Name = "Jane" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, jack, mary };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            Assert.IsTrue(_compare.Compare(class1, class2));
        }

        [Test]
        public void TestIndexerNegative()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            Assert.IsFalse(_compare.Compare(class1, class2));
        }

        [Test]
        public void TestIndexerLengthNegative()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, john, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            Assert.IsFalse(_compare.Compare(class1, class2));
        }
        #endregion

        #region Shallow Tests
        [Test]
        public void ShallowWithNullNoChanges()
        {
            PrimitivePropertiesNullable p1 = new PrimitivePropertiesNullable();
            PrimitivePropertiesNullable p2 = new PrimitivePropertiesNullable();
            _compare.CompareChildren = false;

            if (!_compare.Compare(p1, p2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void ShallowWithNullWithChanges()
        {
            PrimitivePropertiesNullable p1 = new PrimitivePropertiesNullable();
            PrimitivePropertiesNullable p2 = new PrimitivePropertiesNullable();
            p2.BooleanProperty = true;
            _compare.CompareChildren = false;
            Assert.IsFalse(_compare.Compare(p1, p2));
        }

        #endregion

        #region Null Tests
        [Test]
        public void NullObjects()
        {
            Person p1 = null;
            Person p2 = null;
            if (!_compare.Compare(p1, p2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void ListOfNullObjects()
        {            
            Person p1 = null;
            Person p2 = null;

            List<Person> list1 = new List<Person>();
            list1.Add(p1);
            list1.Add(p2);

            List<Person> list2 = new List<Person>();
            list2.Add(p1);
            list2.Add(p2);            

            if (!_compare.Compare(list1, list2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void OneObjectNull()
        {
            Person p1 = null;
            Person p2 = new Person();     
            Assert.IsFalse(_compare.Compare(p1, p2));
            Assert.IsFalse(_compare.Compare(p2, p1));
        }


        #endregion

        #region Property Tests
        [Test]
        public void PropertyAndFieldTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            if (!_compare.Compare(p1, p2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TickTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated.AddTicks(1);

            Assert.IsFalse(_compare.Compare(p1, p2));
        }


        #endregion

        #region Dictionary Tests
        [Test]
        public void TestDictionary()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            Dictionary<string, Person> dict1 = new Dictionary<string, Person>();
            dict1.Add("1001", p1);
            dict1.Add("1002", p2);

            Dictionary<string, Person> dict2 = Common.CloneWithSerialization(dict1);

            if (!_compare.Compare(dict1, dict2))
                throw new Exception(_compare.DifferencesString);

        }

        [Test]
        public void TestDictionaryNegative()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            Dictionary<string, Person> dict1 = new Dictionary<string, Person>();
            dict1.Add("1001", p1);
            dict1.Add("1002", p2);

            Dictionary<string, Person> dict2 = Common.CloneWithSerialization(dict1);

            dict2["1002"].DateCreated = DateTime.Now.AddDays(1);

            Assert.IsFalse(_compare.Compare(dict1, dict2));

        }
        #endregion

        #region Struct Tests
        [Test]
        public void TestStruct()
        {
            Size size1 = new Size();
            size1.Width = 800;
            size1.Height = 600;

            Size size2 = new Size();
            size2.Width = 1024;
            size2.Height = 768;

            List<Size> list1 = new List<Size>();
            list1.Add(size1);
            list1.Add(size2);

            List<Size> list2 = new List<Size>();
            list2.Add(size1);
            list2.Add(size2);

            if (!_compare.Compare(list1, list2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TestStructNegative()
        {
            Size size1 = new Size();
            size1.Width = 800;
            size1.Height = 600;

            Size size2 = new Size();
            size2.Width = 1024;
            size2.Height = 768;

            List<Size> list1 = new List<Size>();
            list1.Add(size1);
            list1.Add(size2);

            List<Size> list2 = new List<Size>();
            list2.Add(size1);
            Size size3= new Size();
            size3.Width= 1025;
            size3.Height = 768;
            list2.Add(size3);

            Assert.IsFalse(_compare.Compare(list1, list2));
        }
        #endregion

        #region Enumeration Tests
        [Test]
        public void TestEnumeration()
        {
            List<Deck> list1 = new List<Deck>();
            list1.Add(Deck.Engineering);
            list1.Add(Deck.SickBay);

            List<Deck> list2 = new List<Deck>();
            list2.Add(Deck.Engineering);
            list2.Add(Deck.SickBay);

            if (!_compare.Compare(list1, list2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TestEnumerationNegative()
        {
            List<Deck> list1 = new List<Deck>();
            list1.Add(Deck.Engineering);
            list1.Add(Deck.SickBay);

            List<Deck> list2 = new List<Deck>();
            list2.Add(Deck.Engineering);
            list2.Add(Deck.AstroPhysics);

            Assert.IsFalse(_compare.Compare(list1, list2));
        }

        [Test]
        public void TestNullableEnumeration()
        {
            List<Deck?> list1 = new List<Deck?>();
            list1.Add(Deck.Engineering);
            list1.Add(null);

            List<Deck?> list2 = new List<Deck?>();
            list2.Add(Deck.Engineering);
            list2.Add(null);

            if (!_compare.Compare(list1, list2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TestNullableEnumerationNegative()
        {
            List<Deck?> list1 = new List<Deck?>();
            list1.Add(Deck.Engineering);
            list1.Add(null);

            List<Deck?> list2 = new List<Deck?>();
            list2.Add(Deck.Engineering);
            list2.Add(Deck.AstroPhysics);

            Assert.IsFalse(_compare.Compare(list1, list2));
        }

        #endregion

        #region GUID Tests
        [Test]
        public void TestGuid()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            List<Guid> list1 = new List<Guid>();
            list1.Add(guid1);
            list1.Add(guid2);

            List<Guid> list2 = new List<Guid>();
            list2.Add(guid1);
            list2.Add(guid2);

            if (!_compare.Compare(list1, list2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TestGuidNegative()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Guid guid3 = Guid.NewGuid();

            List<Guid> list1 = new List<Guid>();
            list1.Add(guid1);
            list1.Add(guid2);

            List<Guid> list2 = new List<Guid>();
            list2.Add(guid1);
            list2.Add(guid3);

            Assert.IsFalse(_compare.Compare(list1,list2));
        }

        #endregion

        #region Test Timespan
        [Test]
        public void TestTimespan()
        {
            TimeSpan ts1 = DateTime.Now - DateTime.Now.AddMinutes(-61);
            TimeSpan ts2 = DateTime.Now - DateTime.Now.AddHours(-49);

            List<TimeSpan> list1 = new List<TimeSpan>();
            list1.Add(ts1);
            list1.Add(ts2);

            List<TimeSpan> list2 = new List<TimeSpan>();
            list2.Add(ts1);
            list2.Add(ts2);

            if (!_compare.Compare(list1, list2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TestTimeSpanNegative()
        {
            TimeSpan ts1 = DateTime.Now - DateTime.Now.AddMinutes(-61);
            TimeSpan ts2 = DateTime.Now - DateTime.Now.AddHours(-49);
            TimeSpan ts3 = DateTime.Now - DateTime.Now.AddHours(-48);

            List<TimeSpan> list1 = new List<TimeSpan>();
            list1.Add(ts1);
            list1.Add(ts2);

            List<TimeSpan> list2 = new List<TimeSpan>();
            list2.Add(ts1);
            list2.Add(ts3);

            Assert.IsFalse(_compare.Compare(list1, list2));
        }
        #endregion

        #region Array Tests
        [Test]
        public void ByteArrayTest()
        {
            byte[] b1 = new byte[256];
            byte[] b2 = new byte[256];
            for (int i = 0; i <= 255; i++)
                b1[i] = (byte) i;
            
            b1.CopyTo(b2,0);

            if (!_compare.Compare(b1, b2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void ArrayTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";

            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            Person[] array1 = new Person[2];
            Person[] array2 = new Person[2];

            array1[0] = p1;
            array1[1] = p2;

            array2[0] = Common.CloneWithSerialization(p1);
            array2[1] = Common.CloneWithSerialization(p2);

            if (!_compare.Compare(array1, array2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void ArrayTestNegative()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";

            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            Person[] array1 = new Person[2];
            Person[] array2 = new Person[2];

            array1[0] = p1;
            array1[1] = p2;

            array2[0] = Common.CloneWithSerialization(p1);
            array2[1] = Common.CloneWithSerialization(p2);
            array2[1].Name = "Bob";

            Assert.IsFalse(_compare.Compare(array1, array2));
        }

        [Test]
        public void MultiDimensionalByteArrayTest()
        {
            byte[,] bytes1 = new byte[3,2];
            byte[,] bytes2 = new byte[3,2];

            bytes1[0, 0] = 3;
            bytes1[1, 0] = 35;
            bytes1[2, 0] = 6;
            bytes1[0, 1] = 3;
            bytes1[1, 1] = 35;
            bytes1[2, 1] = 6;

            bytes2[0, 0] = 3;
            bytes2[1, 0] = 35;
            bytes2[2, 0] = 6;
            bytes2[0, 1] = 3;
            bytes2[1, 1] = 35;
            bytes2[2, 1] = 6;

            if (!_compare.Compare(bytes1, bytes2))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void MultiDimensionalByteArrayNegative()
        {
            byte[,] bytes1 = new byte[3, 2];
            byte[,] bytes2 = new byte[3, 2];

            bytes1[0, 0] = 3;
            bytes1[1, 0] = 35;
            bytes1[2, 0] = 6;
            bytes1[0, 1] = 3;
            bytes1[1, 1] = 35;
            bytes1[2, 1] = 6;

            bytes2[0, 0] = 3;
            bytes2[1, 0] = 35;
            bytes2[2, 0] = 6;
            bytes2[0, 1] = 3;
            bytes2[1, 1] = 36;
            bytes2[2, 1] = 6;

            Assert.IsFalse(_compare.Compare(bytes1, bytes2));
        }

        #endregion

        #region Entity Tree Tests
        [Test]
        public void TestEntityTree()
        {
            List<Entity> entityTree = new List<Entity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            Entity div2 = new Entity();
            div2.Description = "Sub Contracted Fighting";
            div2.EntityLevel = Level.Division;
            div2.Parent = top1;
            top1.Children.Add(div2);

            Entity dep2 = new Entity();
            dep2.Description = "Trojan Rabbit Department";
            dep2.EntityLevel = Level.Department;
            dep2.Parent = div2;
            div2.Children.Add(dep2);

            //Roger the Shrubber's Fine Shrubberies
            Entity top1b = new Entity();
            top1b.Description = "Roger the Shrubber's Fine Shrubberies";
            top1b.Parent = null;
            top1b.EntityLevel = Level.Company;
            entityTree.Add(top1b);

            Entity div1b = new Entity();
            div1b.Description = "Manufacturing";
            div1b.EntityLevel = Level.Division;
            div1b.Parent = top1;
            top1b.Children.Add(div1);

            Entity dep1b = new Entity();
            dep1b.Description = "Design Department";
            dep1b.EntityLevel = Level.Department;
            dep1b.Parent = div1b;
            div1b.Children.Add(dep1b);

            Entity dep2b = new Entity();
            dep2b.Description = "Arranging Department";
            dep2b.EntityLevel = Level.Department;
            dep2b.Parent = div1b;
            div1b.Children.Add(dep2b);

            Entity div2b = new Entity();
            div2b.Description = "Sales";
            div2b.EntityLevel = Level.Division;
            div2b.Parent = top1;
            top1b.Children.Add(div2b);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            if (!_compare.Compare(entityTree, entityTreeCopy))
                throw new Exception(_compare.DifferencesString);
        }

        [Test]
        public void TestEntityTreeNegative()
        {
            List<Entity> entityTree = new List<Entity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            Entity div2 = new Entity();
            div2.Description = "Sub Contracted Fighting";
            div2.EntityLevel = Level.Division;
            div2.Parent = top1;
            top1.Children.Add(div2);

            Entity dep2 = new Entity();
            dep2.Description = "Trojan Rabbit Department";
            dep2.EntityLevel = Level.Department;
            dep2.Parent = div2;
            div2.Children.Add(dep2);

            //Roger the Shrubber's Fine Shrubberies
            Entity top1b = new Entity();
            top1b.Description = "Roger the Shrubber's Fine Shrubberies";
            top1b.Parent = null;
            top1b.EntityLevel = Level.Company;
            entityTree.Add(top1b);

            Entity div1b = new Entity();
            div1b.Description = "Manufacturing";
            div1b.EntityLevel = Level.Division;
            div1b.Parent = top1;
            top1b.Children.Add(div1);

            Entity dep1b = new Entity();
            dep1b.Description = "Design Department";
            dep1b.EntityLevel = Level.Department;
            dep1b.Parent = div1b;
            div1b.Children.Add(dep1b);

            Entity dep2b = new Entity();
            dep2b.Description = "Arranging Department";
            dep2b.EntityLevel = Level.Department;
            dep2b.Parent = div1b;
            div1b.Children.Add(dep2b);

            Entity div2b = new Entity();
            div2b.Description = "Sales";
            div2b.EntityLevel = Level.Division;
            div2b.Parent = top1;
            top1b.Children.Add(div2b);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[1].Children[1].Description = "Retail";

            Assert.IsFalse(_compare.Compare(entityTree,entityTreeCopy));
        }

        #endregion

        #region Private Property Tests
        [Test]
        public void PrivatePropertyPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.ComparePrivateProperties = true;
            Assert.IsTrue(_compare.Compare(detail1, detail2));
            _compare.ComparePrivateProperties = false;
        }

        [Test]
        public void PrivatePropertyNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Crunchy Frogs");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.ComparePrivateProperties = true;
            Assert.IsFalse(_compare.Compare(detail1, detail2));
            _compare.ComparePrivateProperties = false;
        }
        #endregion

        #region Private Field Tests
        [Test]
        public void PrivateFieldPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.ComparePrivateFields = true;
            Assert.IsTrue(_compare.Compare(detail1, detail2));
            _compare.ComparePrivateFields = false;
        }

        [Test]
        public void PrivateFieldNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Crunchy Frogs");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.ComparePrivateFields = true;
            Assert.IsFalse(_compare.Compare(detail1, detail2));
            _compare.ComparePrivateFields = false;
        }
        #endregion

        #region Ignore Read Only Tests
        [Test]
        public void IgnoreReadOnlyPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(false, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.CompareReadOnly = false;
            Assert.IsTrue(_compare.Compare(detail1, detail2));
            _compare.CompareReadOnly = true;
        }

        [Test]
        public void IgnoreReadOnlyNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(false, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            Assert.IsFalse(_compare.Compare(detail1, detail2));
        }
        #endregion

        #region Ignore Children Tests
        [Test]
        public void IgnoreChildDifferencesPositiveTest()
        {
            List<Entity> entityTree = new List<Entity>();

            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[0].Children[0].EntityLevel = Level.Department;

            _compare.CompareChildren = false;
            Assert.IsTrue(_compare.Compare(entityTree, entityTreeCopy));
            _compare.CompareChildren = true;
        }

        [Test]
        public void IgnoreChildDifferencesNegativeTest()
        {
            List<Entity> entityTree = new List<Entity>();

            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[0].Children[0].EntityLevel = Level.Department;

            _compare.CompareChildren = true;
            Assert.IsFalse(_compare.Compare(entityTree, entityTreeCopy));
        }

        #endregion

        #region Generic Entity List Test
        [Test]
        public void GenericEntityListTest()
        {
            GenericEntity<IEntity> genericEntity = new GenericEntity<IEntity>();
            genericEntity.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            genericEntity.MyList.Add(top1);

            GenericEntity<IEntity> genericEntityCopy = new GenericEntity<IEntity>();
            genericEntityCopy.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top2 = new Entity();
            top2.Description = "Brave Sir Robin Security Company";
            top2.Parent = null;
            top2.EntityLevel = Level.Company;
            genericEntityCopy.MyList.Add(top2);

            Assert.IsTrue(_compare.Compare(genericEntity, genericEntityCopy));

            genericEntityCopy.MyList[0].Description = "When danger reared its ugly head Brave Sir Robin fled.";

            Assert.IsFalse(_compare.Compare(genericEntity, genericEntityCopy));
        }

        #endregion

    }
}
