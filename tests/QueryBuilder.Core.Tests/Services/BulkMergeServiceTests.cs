using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using QueryBuilder.Core.Services;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.Tests.FakeModels;
using Xunit;

namespace QueryBuilder.Core.Tests.Services
{
    public class BulkMergeServiceTests
    {
        #region Init

        private readonly List<TestClass> _records;
        private readonly List<MemberExpression> _mergeKeys;

        public BulkMergeServiceTests()
        {
            _records = new List<TestClass>() { new TestClass() { Id = 3 } };

            Expression<Func<TestClass, int>> expression = t => t.Id;
            var memberExpression = expression.Body as MemberExpression;
            _mergeKeys = new List<MemberExpression>() { memberExpression };
        }

        #endregion

        #region Constructor

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenBulkInsertServiceIsNull()
        {
            // Arrange
            var bulkInsertService = new Mock<IBulkInsertService<TestClass>>().Object;
            var commandService    = null as ICommandService<TestClass>;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => new BulkMergeService<TestClass>(bulkInsertService, commandService)
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenCommandServiceIsNull()
        {
            // Arrange
            var bulkInsertService = null as IBulkInsertService<TestClass>;
            var commandService    = new Mock<ICommandService<TestClass>>().Object;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => new BulkMergeService<TestClass>(bulkInsertService, commandService)
            );

        }

        #endregion

        #region WriteToServer

        [Fact]
        public void WriteToServerThrowsArgumentExceptionWhenRecordsIsNull()
        {
            // Arrange
            var bulkInsertService = new Mock<IBulkInsertService<TestClass>>().Object;
            var commandService    = new Mock<ICommandService<TestClass>>().Object;

            var service = new BulkMergeService<TestClass>(bulkInsertService, commandService);

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(null, _mergeKeys, true)
            );
        }

        [Fact]
        public void WriteToServerThrowsArgumentExceptionWhenRecordsIsEmpty()
        {
            // Arrange
            var bulkInsertService = new Mock<IBulkInsertService<TestClass>>().Object;
            var commandService    = new Mock<ICommandService<TestClass>>().Object;

            var service = new BulkMergeService<TestClass>(bulkInsertService, commandService);

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(new List<TestClass>(), _mergeKeys, true)
            );
        }

        [Fact]
        public void WriteToServerThrowsArgumentExceptionWhenMergeKeysIsNull()
        {
            // Arrange
            var bulkInsertService = new Mock<IBulkInsertService<TestClass>>().Object;
            var commandService    = new Mock<ICommandService<TestClass>>().Object;

            var service = new BulkMergeService<TestClass>(bulkInsertService, commandService);

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(_records, null, true)
            );
        }

        [Fact]
        public void WriteToServerThrowsArgumentExceptionWhenMergeKeysIsEmpty()
        {
            // Arrange
            var bulkInsertService = new Mock<IBulkInsertService<TestClass>>().Object;
            var commandService    = new Mock<ICommandService<TestClass>>().Object;

            var service = new BulkMergeService<TestClass>(bulkInsertService, commandService);

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => service.WriteToServer(_records, new List<MemberExpression>(), true)
            );
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteToServerShouldPerformBulkMerge(bool isUpdateOnly)
        {
            // Arrange
            const string tempTableName = "#tmp_bulk";
            var bulkInsertService = new Mock<IBulkInsertService<TestClass>>();
            var commandService    = new Mock<ICommandService<TestClass>>();

            var service = new BulkMergeService<TestClass>(bulkInsertService.Object, commandService.Object);

            // Act
            service.WriteToServer(_records, _mergeKeys, isUpdateOnly);

            // Assert Create and Drop Temp table create
            Expression<Func<AlterTableStatement<TestClass>, bool>> isCreateTableStatement 
                = s => s.TableName == tempTableName && s.Type == AlterTableStatement<TestClass>.AlterType.Create;
            Expression<Func<AlterTableStatement<TestClass>, bool>> isDropTableStatement 
                = s => s.TableName == tempTableName && s.Type == AlterTableStatement<TestClass>.AlterType.Drop;

            commandService.Verify(m => m.Alter(It.Is(isCreateTableStatement)), Times.Once);
            commandService.Verify(m => m.Alter(It.Is(isDropTableStatement))  , Times.Once);

            // Assert Bulk Insert
            bulkInsertService.Verify(m => m.WriteToServer(_records, tempTableName), Times.Once);

            // Assert Merge
            Expression<Func<MergeStatement<TestClass>, bool>> isMergeStatement = s => s.Keys == _mergeKeys && s.TemporaryTableName == tempTableName;
            commandService.Verify(m => m.Merge(It.Is(isMergeStatement)), Times.Once);

            bulkInsertService.VerifyNoOtherCalls();
            commandService.VerifyNoOtherCalls();
        }

        #endregion
    }
}
