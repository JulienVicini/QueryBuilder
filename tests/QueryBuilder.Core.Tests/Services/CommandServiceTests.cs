using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Services;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.Tests.FakeModels;
using Xunit;

namespace QueryBuilder.Core.Tests.Services
{
    public class CommandServiceTests
    {
        #region Test Init

        private readonly AlterTableStatement<TestClass> _alterQuery;
        private readonly DeleteStatement<TestClass> _deleteQuery;
        private readonly MergeStatement<TestClass> _mergeQuery;
        private readonly UpdateStatement<TestClass> _updateQuery;

        public CommandServiceTests()
        {
            // Create Alter Query
            _alterQuery = new AlterTableStatement<TestClass>("testtableName", AlterTableStatement<TestClass>.AlterType.Create);

            // Create Delete Query
            _deleteQuery = new DeleteStatement<TestClass>(m => m.Id > 10);

            // Create Update Query
            Expression<Func<TestClass>> newExpression = () => new TestClass() { Id = 3 };
            MemberInitExpression memberInit = newExpression.Body as MemberInitExpression;

            _updateQuery = new UpdateStatement<TestClass>(
                memberInit.Bindings.Cast<MemberAssignment>(),
                null
            );

            // Create Merge Query
            Expression<Func<TestClass, int>> expression = t => t.Id;
            MemberExpression memberExpression = expression.Body as MemberExpression;

            var mergeKeys = new List<MemberExpression>() { memberExpression };

            _mergeQuery = new MergeStatement<TestClass>(mergeKeys, "temporarytable", MergeStatement<TestClass>.MergeType.InsertOnly);
        }

        #endregion

        #region Constructors

        [Fact]
        public void ConstructorShouldThrowsArgumentNullExceptionWhenCommandProcessingIsNull()
        {
            // Arrange
            var commandProcessing = new Mock<ICommandProcessing<OtherClass>>();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => new CommandService<TestClass, OtherClass>(
                    queryProcessor : commandProcessing.Object,
                    queryTranslator: null
                )
            );
        }

        [Fact]
        public void ConstructorShouldThrowsArgumentNullExceptionWhenStatementTranslatorIsNull()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => new CommandService<TestClass, OtherClass>(
                    queryProcessor : null,
                    queryTranslator: queryTranslator.Object
                )
            );
        }

        #endregion

        #region Validate Arguments Are Not Null

        private CommandService<TestClass, OtherClass> CreateService()
        {
            var commandProcessing = new Mock<ICommandProcessing<OtherClass>>();
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();

            return new CommandService<TestClass, OtherClass>(
                queryProcessor : commandProcessing.Object,
                queryTranslator: queryTranslator.Object
            );
        }

        [Fact]
        public void AlterThrowsArgumentNullExceptionWhenQueryIsNull() {
            Assert.Throws<ArgumentNullException>(
                () => CreateService().Alter(null)
            );

            Assert.Throws<ArgumentNullException>(
                () => CreateService().Alter(null, new OtherClass())
            );
        }

        [Fact]
        public void DeleteThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CreateService().Delete(null)
            );

            Assert.Throws<ArgumentNullException>(
                () => CreateService().Delete(null, new OtherClass())
            );
        }

        [Fact]
        public void MergeThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CreateService().Merge(null)
            );

            Assert.Throws<ArgumentNullException>(
                () => CreateService().Merge(null, new OtherClass())
            );
        }

        [Fact]
        public void UpdateThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CreateService().Update(null)
            );

            Assert.Throws<ArgumentNullException>(
                () => CreateService().Update(null, new OtherClass())
            );
        }

        [Fact]
        public void AlterThrowsArgumentNullExceptionWhenTransactionIsNull()
        {
            // Arrange
            CommandService<TestClass, OtherClass> service = CreateService();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => service.Alter(_alterQuery, null)
            );
        }

        [Fact]
        public void DeleteThrowsArgumentNullExceptionWhenTransactionIsNull()
        {
            // Arrange
            CommandService<TestClass, OtherClass> service = CreateService();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => service.Delete(_deleteQuery, null)
            );
        }

        [Fact]
        public void MergeThrowsArgumentNullExceptionWhenTransactionIsNull()
        {
            // Arrange
            CommandService<TestClass, OtherClass> service = CreateService();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => service.Merge(_mergeQuery, null)
            );
        }

        [Fact]
        public void UpdateThrowsArgumentNullExceptionWhenTransactionIsNull()
        {
            // Arrange
            CommandService<TestClass, OtherClass> service = CreateService();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => service.Update(_updateQuery, null)
            );
        }

        #endregion

        #region Translate and execute query

        private readonly (string, IEnumerable<object>) _translatedQuery = ("query text", new object[] { });
        private readonly Mock<ICommandProcessing<OtherClass>> _commandProcessingMock = new Mock<ICommandProcessing<OtherClass>>();
        private readonly OtherClass _transaction = new OtherClass();
        const int _expectedQueryResult = 186;
        const int _expectedTransactionalQueryResult = 1587;


        private CommandService<TestClass, OtherClass> CreateCommandService(IStatementTranslator<TestClass> queryTranslator)
        {
            _commandProcessingMock.Setup(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2))
                                  .Returns(_expectedQueryResult);

            _commandProcessingMock.Setup(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2, _transaction))
                                  .Returns(_expectedTransactionalQueryResult);

            return new CommandService<TestClass, OtherClass>(
                queryProcessor : _commandProcessingMock.Object,
                queryTranslator: queryTranslator
            );
        }

        [Fact]
        public void AlterStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_alterQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Alter(_alterQuery);

            // Assert
            Assert.Equal(_expectedQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_alterQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_deleteQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Delete(_deleteQuery);

            // Assert
            Assert.Equal(_expectedQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_deleteQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void MergeStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_mergeQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Merge(_mergeQuery);

            // Assert
            Assert.Equal(_expectedQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_mergeQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void UpdateStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_updateQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Update(_updateQuery);

            // Assert
            Assert.Equal(_expectedQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_updateQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void AlterTransactionalStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_alterQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Alter(_alterQuery, _transaction);

            // Assert
            Assert.Equal(_expectedTransactionalQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_alterQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2, _transaction), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteTransactionalStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_deleteQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Delete(_deleteQuery, _transaction);

            // Assert
            Assert.Equal(_expectedTransactionalQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_deleteQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2, _transaction), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void MergeTransactionalStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_mergeQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Merge(_mergeQuery, _transaction);

            // Assert
            Assert.Equal(_expectedTransactionalQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_mergeQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2, _transaction), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        [Fact]
        public void UpdateTransactionalStatementShouldTranslateAndExecuteQuery()
        {
            // Arrange
            var queryTranslator = new Mock<IStatementTranslator<TestClass>>();
            queryTranslator.Setup(m => m.TranslateQuery(_updateQuery))
                           .Returns(_translatedQuery);

            CommandService<TestClass, OtherClass> service = CreateCommandService(queryTranslator.Object);

            // Act
            int actualResult = service.Update(_updateQuery, _transaction);

            // Assert
            Assert.Equal(_expectedTransactionalQueryResult, actualResult);
            queryTranslator.Verify(m => m.TranslateQuery(_updateQuery), Times.Once);
            _commandProcessingMock.Verify(m => m.ExecuteCommand(_translatedQuery.Item1, _translatedQuery.Item2, _transaction), Times.Once);

            _commandProcessingMock.VerifyNoOtherCalls();
            queryTranslator.VerifyNoOtherCalls();
        }

        #endregion
    }
}
