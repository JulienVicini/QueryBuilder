using EntityFramework.Extensions.Core.BulkCopy;
using EntityFramework.Extensions.Core.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Tests.Core.BulkCopy
{
    [TestClass]
    public class BulkCopyTest
    {
        private object _modelObject;
        private object _transformedObject;

        private Mock<IBulkCopyExecutor<IEnumerable<object>>> _bulkCopyExecutorMock;
        private Mock<IDataTransformer<IEnumerable<object>, IEnumerable<object>>> _dataTransformerMock;
        private Mock<IMappingAdapter<object>> _mappingAdapter;

        [TestInitialize]
        public void Init()
        {
            _modelObject       = new object();
            _transformedObject = new object();

            _bulkCopyExecutorMock = new Mock<IBulkCopyExecutor<IEnumerable<object>>>();
            _dataTransformerMock  = new Mock<IDataTransformer<IEnumerable<object>, IEnumerable<object>>>();
            _mappingAdapter       = new Mock<IMappingAdapter<object>>();
        }

        [TestMethod]
        public void ConstructorThrowsArgumentNullExceptionWhenBulkCopyIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BulkCopy<object, IEnumerable<object>>(null, _dataTransformerMock.Object, _mappingAdapter.Object));
        }

        [TestMethod]
        public void ConstructorThrowsArgumentNullExceptionWhenDataTransformerIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BulkCopy<object, IEnumerable<object>>(_bulkCopyExecutorMock.Object, null, _mappingAdapter.Object));
        }

        [TestMethod]
        public void ConstructorThrowsArgumentNullExceptionWhenMappingAdapterIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BulkCopy<object, IEnumerable<object>>(_bulkCopyExecutorMock.Object, _dataTransformerMock.Object, null));
        }

        [TestMethod]
        public void WriteToServerThrowArgumentExceptionWhenRecordsIsNullOrEmpty()
        {
            var bulkCopy = new BulkCopy<object, IEnumerable<object>>(_bulkCopyExecutorMock.Object, _dataTransformerMock.Object, _mappingAdapter.Object);

            Assert.ThrowsException<ArgumentException>(
                () => bulkCopy.WriteToServer(null)
            );

            Assert.ThrowsException<ArgumentException>(
                () => bulkCopy.WriteToServer(new List<object>())
            );
        }

        [TestMethod]
        public void WriteToServerShouldConvertData()
        {
            var objects = new List<object>() { new object() };

            _dataTransformerMock.Setup(d => d.Transform(It.IsAny<IEnumerable<object>>()));

            var bulkCopy = new BulkCopy<object, IEnumerable<object>>(_bulkCopyExecutorMock.Object, _dataTransformerMock.Object, _mappingAdapter.Object);

            bulkCopy.WriteToServer(objects);

            _dataTransformerMock.Verify(d => d.Transform(It.Is<IEnumerable<object>>(obj => obj == objects)), Times.Once());
        }

        [TestMethod]
        public void WriteToServerShouldCallExecutor()
        {
            var objects = new List<object>() { new object() };
            var transformedObjects = new List<object>(objects);

            // setup mock
            _dataTransformerMock.Setup(d => d.Transform(It.IsAny<IEnumerable<object>>()))
                                .Returns(transformedObjects);

            _mappingAdapter.Setup(m => m.GetTableName())
                           .Returns("RandomTable");

            // act
            var bulkCopy = new BulkCopy<object, IEnumerable<object>>(_bulkCopyExecutorMock.Object, _dataTransformerMock.Object, _mappingAdapter.Object);
            bulkCopy.WriteToServer(objects);

            _bulkCopyExecutorMock.Verify(
                e => e.Write(It.Is<string>(str => str == "RandomTable"), It.Is<IEnumerable<object>>(obj => obj == transformedObjects)),
                Times.Once()
            );
        }
    }
}
