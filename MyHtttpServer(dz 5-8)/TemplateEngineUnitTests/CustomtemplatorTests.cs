using MyHtttpServer.Core.Templator;
using TemplateEngineUnitTests.Models;

namespace MyHttpServer.UnitTests.Core.Templator
{
    [TestClass]
    public class CustomTemplatorTests
    {

        [TestMethod]
        public void GetHtmlByTemplate_When_NameIsnotNull_ResultSuccess()
        {
            // Arrange
            ICustomTemplator customTemplator = new CustomTemplator();
            string template = "<label>name</label><p>{name}</p>";
            string name = "Тимерхан";


            // Act
            var result = customTemplator.GetHtmlByTemplate(template, name);

            // Assert
            Assert.AreEqual("<label>name</label><p>Тимерхан</p>", result);
        }

        [TestMethod]
        public void GetHtmlByTemplate_When_UserIsnotNull_ResultSuccess()
        {
            // Arrange
            ICustomTemplator customTemplator = new CustomTemplator();
            string template = "<p>Ваш логин: {login}; Ваш пароль: {password};</p>";
            var user = new Person
            {
                Login = "test@test.ru",
                Password = "passWord",
                Name = "Ильхам"
            };

            // Act
            var result = customTemplator.GetHtmlByTemplate(template, user);

            // Assert
            Assert.AreEqual("<p>Ваш логин: test@test.ru; Ваш пароль: passWord;</p>", result);
        }

        [TestMethod]
        public void GetHtmlByTemplate_When_ObjectIsnotNull_ResultSuccess()
        {
            // Arrange
            var customTemplator = new CustomTemplator();
            string template = "Привет {name}! <p>Ваш логин: {login}; Ваш пароль: {password}; Мы очень рады с вами познакомится дорогой {name}!</p>";
            Person user = new Person() { Name = "Иван", Login = "test@test.ru", Password = "passWord" };
            var result = customTemplator.GetHtmlByTemplate(template, user);
            Assert.AreEqual("Привет Иван! <p>Ваш логин: test@test.ru; Ваш пароль: passWord; Мы очень рады с вами познакомится дорогой Иван!</p>", result);
        }


        [TestMethod]
        public void GetHtmlByTemplate_ResultSuccess()
        {
            // Arrange
            var customTemplator = new CustomTemplator();
            string template = "{title} Описание:{description}";
            Movies user = new Movies() { title = "Крутой парень", description = "Крутой парень курит большую сигару" };
            var result = customTemplator.GetHtmlByTemplate(template, user);
            Assert.AreEqual("Крутой парень Описание:Крутой парень курит большую сигару", result);
        }
    }
}
