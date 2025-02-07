using Moq;
using MyORMLibrary;
using MyORMLibraryUnitTests.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MyORMLibraryUnitTests
{
    [TestClass]
    public class TestsContextTest
    {
        [TestMethod]
        public void GetByIdTest()
        {
            // Arrange
            var dbConnection = new Mock<IDbConnection>();
            var dbCommand = new Mock<IDbCommand>();
            var dbDataReader = new Mock<IDataReader>();
            var person = new Person()
            {
                Id = 1,
                Name = "Chel",
                Email = "test@test.ru"
            };
            var context = new TestContext<Person>(dbConnection.Object);

            // ������� ������ ��� ������������ ����������� ����������
            var parameterList = new List<IDbDataParameter>();

            // ��������� �������� ���������� � ���������� ������/������ �������
            dbCommand.Setup(c => c.CreateParameter())
                .Returns(() =>
                {
                    var paramMock = new Mock<IDbDataParameter>();
                    paramMock.SetupProperty(p => p.ParameterName); // ��������� �������� ParameterName
                    paramMock.SetupProperty(p => p.Value);         // ��������� �������� Value
                    return paramMock.Object;
                });

            // ����������� ���������� ����������
            dbCommand.Setup(c => c.Parameters.Add(It.IsAny<object>())) // ��������� object ������ IDbDataParameter
                .Callback<object>(param =>
                {
                    if (param is IDbDataParameter dataParam) // ���������, �������� �� �������� IDbDataParameter
                    {
                        parameterList.Add(dataParam);
                    }
                });

            dbCommand.Setup(c => c.ExecuteReader()).Returns(dbDataReader.Object);
            dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);

            // ��������� DataReader
            dbDataReader.SetupSequence(r => r.Read())
                .Returns(true)  // ������ ����� ���������� true, ����� ������������ ������� ������
                .Returns(false); // ������ ����� ���������� false, ����� ������������ ����� ������

            dbDataReader.Setup(r => r["Id"]).Returns(person.Id);
            dbDataReader.Setup(r => r["Name"]).Returns(person.Name);
            dbDataReader.Setup(r => r["Email"]).Returns(person.Email);

            // Act
            var result = context.GetById(person.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(person.Id, result.Id);
            Assert.AreEqual(person.Name, result.Name);
            Assert.AreEqual(person.Email, result.Email);

            // ���������, ��� �������� ��� �������� � ���������� ���������
            Assert.AreEqual(1, parameterList.Count, "�������� ���������� ����������.");
            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Id" && p.Value.Equals(person.Id)), "�������� Id �� ������ ��� �������� ��������.");
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Arrange
            var dbConnection = new Mock<IDbConnection>();
            var dbCommand = new Mock<IDbCommand>();
            var dbDataReader = new Mock<IDataReader>();

            var persons = new List<Person>
            {
                new Person { Id = 1, Name = "Chel", Email = "test@test.ru" },
                new Person { Id = 2, Name = "John", Email = "john@test.ru" }
            };

            var context = new TestContext<Person>(dbConnection.Object);

            dbCommand.Setup(c => c.ExecuteReader()).Returns(dbDataReader.Object);
            dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);

            dbDataReader.Setup(c => c.FieldCount).Returns(3); // ���������� �������� � �������

            var readSequence = dbDataReader.SetupSequence(c => c.Read());
            foreach (var person in persons)
            {
                readSequence = readSequence.Returns(true);
            }
            readSequence.Returns(false); // ����� ���� �������� ������� Read(), ���������� false

            // �������� ������� �������� **�����** `SetupSequence`
            dbDataReader.Setup(r => r["Id"]).Returns(() => persons[0].Id);
            dbDataReader.Setup(r => r["Name"]).Returns(() => persons[0].Name);
            dbDataReader.Setup(r => r["Email"]).Returns(() => persons[0].Email);

            int index = -1;
            dbDataReader.Setup(c => c.Read()).Returns(() =>
            {
                index++;
                return index < persons.Count;
            });

            dbDataReader.Setup(r => r["Id"]).Returns(() => persons[index].Id);
            dbDataReader.Setup(r => r["Name"]).Returns(() => persons[index].Name);
            dbDataReader.Setup(r => r["Email"]).Returns(() => persons[index].Email);

            // Act
            var result = context.GetAll().ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(persons.Count, result.Count);
            for (int i = 0; i < persons.Count; i++)
            {
                Assert.AreEqual(persons[i].Id, result[i].Id);
                Assert.AreEqual(persons[i].Name, result[i].Name);
                Assert.AreEqual(persons[i].Email, result[i].Email);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            // Arrange
            var dbConnection = new Mock<IDbConnection>();
            var dbCommand = new Mock<IDbCommand>();
            var dbParameterCollection = new Mock<IDataParameterCollection>();
            var person = new Person
            {
                Id = 1,
                Name = "Chel",
                Email = "test@test.ru"
            };
            var context = new TestContext<Person>(dbConnection.Object);

            // ������ ������ ���������� ��� ��������
            var parameterList = new List<IDbDataParameter>();

            dbCommand.Setup(c => c.ExecuteNonQuery()).Returns(1); // ���������� �������� ���������� �������

            // ��������� �������� ���������� � ���������� ������/������ �������
            dbCommand.Setup(c => c.CreateParameter())
                .Returns(() =>
                {
                    var paramMock = new Mock<IDbDataParameter>();
                    paramMock.SetupProperty(p => p.ParameterName); // ��������� �������� ParameterName
                    paramMock.SetupProperty(p => p.Value);         // ��������� �������� Value
                    return paramMock.Object;
                });

            // �������� ��������� ����������
            dbParameterCollection.Setup(c => c.Add(It.IsAny<object>()))
                .Callback<object>(param =>
                {
                    if (param is IDbDataParameter dbParam)
                    {
                        parameterList.Add(dbParam);
                    }
                })
                .Returns(0);

            dbParameterCollection.Setup(c => c.Count).Returns(() => parameterList.Count);
            dbCommand.SetupGet(c => c.Parameters).Returns(dbParameterCollection.Object);

            dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);

            // Act
            var result = context.Create(person);

            // Assert
            Assert.IsTrue(result);

            // ��������, ��� ��������� ���� ��������� � ����������� ����������
            Assert.AreEqual(3, parameterList.Count);

            // ��������� ���������, ����������� � ���������
            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Id" && p.Value.Equals(person.Id)), "�������� Id �� ������ ��� �������� ��������.");
            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Name" && p.Value.Equals(person.Name)), "�������� Name �� ������ ��� �������� ��������.");
            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Email" && p.Value.Equals(person.Email)), "�������� Email �� ������ ��� �������� ��������.");
        }

        [TestMethod]
        public void UpdateTest()
        {
            // Arrange
            var dbConnection = new Mock<IDbConnection>();
            var dbCommand = new Mock<IDbCommand>();
            var person = new Person
            {
                Id = 1,
                Name = "Chel",
                Email = "test@test.ru"
            };
            var context = new TestContext<Person>(dbConnection.Object);

            // ������� ������ ��� ������������ ����������� ����������
            var parameterList = new List<IDbDataParameter>();

            dbCommand.Setup(c => c.ExecuteNonQuery()).Returns(1); // ���������� �������� ���������� �������

            // ��������� �������� ���������� � ���������� ������/������ �������
            dbCommand.Setup(c => c.CreateParameter())
                .Returns(() =>
                {
                    var paramMock = new Mock<IDbDataParameter>();
                    paramMock.SetupProperty(p => p.ParameterName); // ��������� �������� ParameterName
                    paramMock.SetupProperty(p => p.Value);         // ��������� �������� Value
                    return paramMock.Object;
                });

            // ����������� ���������� ����������
            dbCommand.Setup(c => c.Parameters.Add(It.IsAny<object>())) // ��������� object ������ IDbDataParameter
                .Callback<object>(param =>
                {
                    // ���������, �������� �� �������� IDbDataParameter
                    var dataParam = param as IDbDataParameter;
                    if (dataParam != null)
                    {
                        parameterList.Add(dataParam);
                    }
                });

            dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);

            // Act
            var result = context.Update(person);

            // Assert
            Assert.IsTrue(result); // ���������, ��� ����� ������ true (�������� ����������)

            // ���������, ��� ��������� ���� ��������� � ����������� ����������
            Assert.AreEqual(3, parameterList.Count, "�������� ���������� ����������.");

            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Id" && p.Value.Equals(person.Id)), "�������� Id �� ������ ��� �������� ��������.");
            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Name" && p.Value.Equals(person.Name)), "�������� Name �� ������ ��� �������� ��������.");
            Assert.IsTrue(parameterList.Any(p => p.ParameterName == "@Email" && p.Value.Equals(person.Email)), "�������� Email �� ������ ��� �������� ��������.");
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Arrange
            var dbConnection = new Mock<IDbConnection>();
            var dbCommand = new Mock<IDbCommand>();
            var dbParameter = new Mock<IDbDataParameter>();
            var personId = 1;
            var context = new TestContext<Person>(dbConnection.Object);

            // ���������� �������� ���������� �������
            dbCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);

            // �������� �������� ��������� � ���������� � ���������
            dbCommand.Setup(c => c.CreateParameter()).Returns(dbParameter.Object);

            // ��������� ���-������� ��� ��������� ������ � ������ ������� ParameterName � Value
            dbParameter.SetupProperty(p => p.ParameterName); // ��������� �������� ParameterName
            dbParameter.SetupProperty(p => p.Value);        // ��������� �������� Value

            var parameterList = new List<IDbDataParameter>(); // ������ ��� ������������ ����������

            // ���������� It.IsAny<object> ��� ������ �������� ����
            dbCommand.Setup(c => c.Parameters.Add(It.IsAny<object>()))
                .Callback<object>(param =>
                {
                    if (param is IDbDataParameter dataParam)
                    {
                        parameterList.Add(dataParam);
                    }
                });

            dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);

            // Act
            var result = context.Delete(personId);

            // Assert
            Assert.IsTrue(result); // ���������, ��� ����� ������ true (�������� ��������)

            // ���������, ��� �������� ��� �������� � ���������� ���������
            Assert.AreEqual(1, parameterList.Count, "�������� �� ��� �������� ��� ���������� ���������� ��������.");

            var addedParameter = parameterList.FirstOrDefault();
            Assert.IsNotNull(addedParameter, "�������� �� ��� ��������.");
            Assert.AreEqual("@Id", addedParameter.ParameterName, "�������� ��� ���������.");
            Assert.AreEqual(personId, addedParameter.Value, "�������� �������� ���������.");
        }
    }
}