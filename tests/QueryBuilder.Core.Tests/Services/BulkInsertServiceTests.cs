using System;
using System.Collections.Generic;
using Moq;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Services;
using QueryBuilder.Core.Tests.FakeModels;
using Xunit;

namespace QueryBuilder.Core.Tests.Services
{
    public class BulkInsertServiceTests
    {
        #region Constructors

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenBulkInsertIsNull()
        {
            var dataTransformerMoq = new Mock<IDataTransformer<IEnumerable<TestClass>, OtherClass>>();
            var mappingAdapterMoq  = new Mock<IMappingAdapter<TestClass>>();

            Assert.Throws<ArgumentNullException>(() => new BulkInsertService<TestClass, OtherClass>(
                    bulkInsert     : null,
                    dataTransformer: dataTransformerMoq.Object,
                    mappingAdapter : mappingAdapterMoq.Object
                )
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenDataTransformerIsNull()
        {
            var bulkInsertMoq     = new Mock<IBulkInsert<OtherClass>>();
            var mappingAdapterMoq = new Mock<IMappingAdapter<TestClass>>();

            Assert.Throws<ArgumentNullException>(() => new BulkInsertService<TestClass, OtherClass>(
                    bulkInsert     : bulkInsertMoq.Object,
                    dataTransformer: null,
                    mappingAdapter : mappingAdapterMoq.Object
                )
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenMappingAdapterIsNull()
        {
            var bulkInsertMoq      = new Mock<IBulkInsert<OtherClass>>();
            var dataTransformerMoq = new Mock<IDataTransformer<IEnumerable<TestClass>, OtherClass>>();

            Assert.Throws<ArgumentNullException>(() => new BulkInsertService<TestClass, OtherClass>(
                    bulkInsert     : bulkInsertMoq.Object,
                    dataTransformer: dataTransformerMoq.Object,
                    mappingAdapter : null
                )
            );
        }

        #endregion

        #region WriteToServer

        private BulkInsertService<TestClass, OtherClass> GetBulkServiceInstance()
        {
            var bulkInsertMoq = new Mock<IBulkInsert<OtherClass>>();
            var dataTransformerMoq = new Mock<IDataTransformer<IEnumerable<TestClass>, OtherClass>>();
            var mappingAdapterMoq = new Mock<IMappingAdapter<TestClass>>();

            return new BulkInsertService<TestClass, OtherClass>(
                bulkInsert     : bulkInsertMoq.Object,
                dataTransformer: dataTransformerMoq.Object,
                mappingAdapter : mappingAdapterMoq.Object
            );
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void WriteToServerThrowsArgumentExceptionWhenTableNameIsNullOrWhiteSpace(string tableName)
        {
            // Arrange
            BulkInsertService<TestClass, OtherClass> service = GetBulkServiceInstance();

            var data = new List<TestClass>() { new TestClass() { Id = 3 } };

            // Act and asert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(data, tableName)
            );
        }

        [Fact]
        public void WriteToServerThrowsArgumentExceptionWhenRecordsIsNull()
        {
            // Arrange
            BulkInsertService<TestClass, OtherClass> service = GetBulkServiceInstance();

            var data = new List<TestClass>();

            // Act and Asert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(data, "tableName")
            );
        }

        [Fact]
        public void WriteToServerThrowsArgumentExceptionWhenRecordsIsEmpty()
        {
            // Arrange
            BulkInsertService<TestClass, OtherClass> service = GetBulkServiceInstance();

            // Act and Asert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(null, "tableName")
            );
        }

        [Fact]
        public void WriteToServerShouldMapAndCallBulkInsert()
        {
            var records = new List<TestClass>() { new TestClass() { Id = 30 } };
            const string tableName = "TableName";
            var transformedData = new OtherClass();

            // Arrange
            var bulkInsertMoq = new Mock<IBulkInsert<OtherClass>>();

            var dataTransformerMoq = new Mock<IDataTransformer<IEnumerable<TestClass>, OtherClass>>();
            dataTransformerMoq.Setup(m => m.Transform(records))
                              .Returns(transformedData);

            var mappingAdapterMoq = new Mock<IMappingAdapter<TestClass>>();
            mappingAdapterMoq.Setup(m => m.GetTableName())
                             .Returns(tableName);

            // Act
            var service = new BulkInsertService<TestClass, OtherClass>(
                bulkInsert     : bulkInsertMoq.Object,
                dataTransformer: dataTransformerMoq.Object,
                mappingAdapter : mappingAdapterMoq.Object
            );

            service.WriteToServer(records);

            // Assert
            bulkInsertMoq.Verify(m => m.Write(tableName, transformedData));
            dataTransformerMoq.Verify(m => m.Transform(records), Times.Once);
            mappingAdapterMoq.Verify(m => m.GetTableName(), Times.Once);

            bulkInsertMoq.VerifyNoOtherCalls();
            dataTransformerMoq.VerifyNoOtherCalls();
            mappingAdapterMoq.VerifyNoOtherCalls();
        }

        #endregion
    }
}
