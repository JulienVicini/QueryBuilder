//using EntityFramework.Extensions.Mappings;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;

//namespace EntityFramework.Extensions.Tests.BulkCopy
//{
//    [TestClass]
//    public class DataTableFactoryTests
//    {
//        private readonly DataTableFactory _dataTableFactory = new DataTableFactory();

//        #region CreatBulkInsertDataTable

//        [TestMethod]
//        public void CreateBulkInsertDataTableThrowsArgumentExceptionWhenMappingsIsNull()
//        {
//            Assert.ThrowsException<ArgumentException>(() =>
//                _dataTableFactory.CreateBulkInsertDataTable(null)
//            );
//        }

//        [TestMethod]
//        public void CreateBulkInsertDataTableThrowsArgumentExceptionWhenMappingsIsEmpty()
//        {
//            Assert.ThrowsException<ArgumentException>(() =>
//                _dataTableFactory.CreateBulkInsertDataTable(Enumerable.Empty<ColumnMapping>())
//            );
//        }

//        [TestMethod]
//        public void CreateBulkInsertDataTableReturnsMappedDataTable()
//        {
//            List<ColumnMapping> columns = new List<ColumnMapping>()
//            {
//                new ColumnMapping(isIdentity: false, propertyName: "Test" , propertyType: typeof(string), sqlName: "SqlTest" ),
//                new ColumnMapping(isIdentity: false, propertyName: "Test1", propertyType: typeof(string), sqlName: "SqlTest1"),
//                new ColumnMapping(isIdentity: false, propertyName: "Test2", propertyType: typeof(string), sqlName: "SqlTest2"),
//                new ColumnMapping(isIdentity: false, propertyName: "Test3", propertyType: typeof(int)   , sqlName: "SqlTest3")
//            };

//            DataTable dataTable = _dataTableFactory.CreateBulkInsertDataTable(columns);

//            // Check columns
//            Assert.AreEqual(4, dataTable.Columns.Count);
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest" ) );
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest1") );
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest2") );
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest3") );

//            Assert.AreEqual(typeof(string), dataTable.Columns["SqlTest" ].DataType);
//            Assert.AreEqual(typeof(string), dataTable.Columns["SqlTest1"].DataType);
//            Assert.AreEqual(typeof(string), dataTable.Columns["SqlTest2"].DataType);
//            Assert.AreEqual(typeof(int)   , dataTable.Columns["SqlTest3"].DataType);
//        }

//        [TestMethod]
//        public void CreateBulkInsertDataTableShouldSkipIdentityColumns()
//        {
//            List<ColumnMapping> columns = new List<ColumnMapping>()
//            {
//                new ColumnMapping(isIdentity: false, propertyName: "Test" , propertyType: typeof(string), sqlName: "SqlTest" ),
//                new ColumnMapping(isIdentity: false, propertyName: "Test1", propertyType: typeof(string), sqlName: "SqlTest1"),
//                new ColumnMapping(isIdentity: true , propertyName: "Test2", propertyType: typeof(string), sqlName: "SqlTest2"),
//                new ColumnMapping(isIdentity: false, propertyName: "Test3", propertyType: typeof(string), sqlName: "SqlTest3")
//            };

//            DataTable dataTable = _dataTableFactory.CreateBulkInsertDataTable(columns);

//            // Check columns
//            Assert.AreEqual(3, dataTable.Columns.Count);
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest" ) );
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest1") );
//            Assert.IsTrue( dataTable.Columns.Contains("SqlTest3") );
//        }

//        [TestMethod]
//        public void CreateBulkInsertDataTableThrowsInvalidOperationExceptionWheDuplicatedColumns()
//        {
//            List<ColumnMapping> columns = new List<ColumnMapping>()
//            {
//                new ColumnMapping(isIdentity: false, propertyName: "Test" , propertyType: typeof(string), sqlName: "SqlTest" ),
//                new ColumnMapping(isIdentity: false, propertyName: "Test1", propertyType: typeof(string), sqlName: "SqlTest1"),
//                new ColumnMapping(isIdentity: false, propertyName: "Test2", propertyType: typeof(string), sqlName: "SqlTest1"),
//            };

//            Assert.ThrowsException<InvalidOperationException>(() =>
//                _dataTableFactory.CreateBulkInsertDataTable(columns)
//            );
//        }

//        #endregion

//        #region CreateDataRow

//        public class TestModel
//        {
//            public int Id { get; set; }

//            public string Name { get; set; }
//        }

//        private readonly TestModel _testModel = new TestModel() { Id = 1, Name = "Test" };
//        private Dictionary<string, PropertyInfo> _mappingProperty = new Dictionary<string, PropertyInfo>()
//        {
//            { nameof(TestModel.Id)  , typeof(TestModel).GetProperty(nameof(TestModel.Id  )) },
//            { nameof(TestModel.Name), typeof(TestModel).GetProperty(nameof(TestModel.Name)) }
//        };

//        [TestMethod]
//        public void CreateDataRowThrowsArgumentNullExceptonWhenDataTableIsNull()
//        {
//            Assert.ThrowsException<ArgumentNullException>(() =>
//                _dataTableFactory.CreateDataRow(null, _testModel, _mappingProperty)
//            );
//        }

//        [TestMethod]
//        public void CreateDataRowThrowsArgumentNullExceptonWhenMappingsAreNull()
//        {
//            Assert.ThrowsException<ArgumentException>(() =>
//                _dataTableFactory.CreateDataRow(new DataTable(), _testModel, null)
//            );
//        }

//        [TestMethod]
//        public void CreateDataRowThrowsArgumentNullExceptonWhenMappingsAreEmpty()
//        {
//            Assert.ThrowsException<ArgumentException>(() =>
//                _dataTableFactory.CreateDataRow(new DataTable(), _testModel, new Dictionary<string, PropertyInfo>())
//            );
//        }

//        [TestMethod]
//        public void CreateDataRowThrowsArgumentNullExceptonWhenRecordIsNull()
//        {
//            DataTable dataTable = new DataTable();
//            dataTable.Columns.Add(nameof(TestModel.Id));

//            Assert.ThrowsException<ArgumentNullException>(() =>
//                _dataTableFactory.CreateDataRow(dataTable, null as TestModel, _mappingProperty)
//            );
//        }

//        [TestMethod]
//        public void CreateDataRowReturnsDataRow()
//        {
//            DataTable dataTable = new DataTable();
//            dataTable.Columns.Add(nameof(TestModel.Id)  , typeof(int)   );
//            dataTable.Columns.Add(nameof(TestModel.Name), typeof(string));

//            DataRow dataRow = _dataTableFactory.CreateDataRow(dataTable, _testModel, _mappingProperty);

//            Assert.AreEqual(_testModel.Id  , dataRow[nameof(TestModel.Id)]  );
//            Assert.AreEqual(_testModel.Name, dataRow[nameof(TestModel.Name)]);
//        }

//        #endregion

//        #region Create Data Rows

//        [TestMethod]
//        public void CreateDataRowsThrowsArgumentNullExceptionWhenDataTableIsNull()
//        {
//            List<TestModel> models = new List<TestModel>() { _testModel };
//            List<ColumnMapping> mappings = new List<ColumnMapping>()
//            {
//                new ColumnMapping(true, "string", typeof(string), "str1")
//            };
//            Assert.ThrowsException<ArgumentNullException>(() => _dataTableFactory.CreateDataRows(null, models, mappings));
//        }

//        [TestMethod]
//        public void CreateDataRowsThrowsArgumentExceptionWhenRecordsAreNullOrEmpty()
//        {
//            List<ColumnMapping> mappings = new List<ColumnMapping>()
//            {
//                new ColumnMapping(true, "string", typeof(string), "str1")
//            };
//            Assert.ThrowsException<ArgumentException>(() => _dataTableFactory.CreateDataRows<TestModel>(new DataTable(), null, mappings));
//            Assert.ThrowsException<ArgumentException>(() => _dataTableFactory.CreateDataRows(new DataTable(), Enumerable.Empty<TestModel>(), mappings));
//        }

//        [TestMethod]
//        public void CreateDataRowsThrowsArgumentExceptionWhenMappingsAreNullOrEmpty()
//        {
//            List<TestModel> models = new List<TestModel>() { _testModel };

//            Assert.ThrowsException<ArgumentException>(() => _dataTableFactory.CreateDataRows(new DataTable(), models, null));
//            Assert.ThrowsException<ArgumentException>(() => _dataTableFactory.CreateDataRows(new DataTable(), models, Enumerable.Empty<ColumnMapping>()));
//        }

//        [TestMethod]
//        public void CreateDataRowsThrowsArgumentExceptionReturnsRows()
//        {
//            // Data
//            List<TestModel> models = Enumerable.Range(0, 100).Select(index => new TestModel()
//            {
//                Id   = index,
//                Name = $"Idem num {index}"
//            })
//            .ToList();

//            // Mappings
//            List<ColumnMapping> mappings = new List<ColumnMapping>()
//            {
//                new ColumnMapping(true , nameof(TestModel.Id  ), typeof(int)   , "P_Id"     ),
//                new ColumnMapping(false, nameof(TestModel.Id  ), typeof(int)   , "P_IdCopy" ),
//                new ColumnMapping(false, nameof(TestModel.Name), typeof(string), "P_Name"   ),
//            };

//            // Act
//            DataTable dataTable = _dataTableFactory.CreateBulkInsertDataTable(mappings);

//            IEnumerable<DataRow> rows = _dataTableFactory.CreateDataRows(dataTable, models, mappings);

//            Assert.AreEqual(100, rows.Count()        );
//            Assert.AreEqual(0  , dataTable.Rows.Count);

//            int assertCount = 0;
//            foreach(DataRow row in rows)
//            {
//                Assert.AreEqual( assertCount              , row["P_IdCopy"] );
//                Assert.AreEqual( $"Idem num {assertCount}", row["P_Name"]   );
//                assertCount++;
//            }
//        }

//        #endregion
//    }
//}
