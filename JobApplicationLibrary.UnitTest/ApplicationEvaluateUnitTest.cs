using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;
using NUnit.Framework;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {
        // UnitOfWork_Condition_ExpectedResult

        [Test]
        public void Application_WithUnderAge_TransferredToAutoRejected()
        {
            // Arrange
            var evaluator = new ApplicationEvaluator(null);
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };

            // Action 
            var appResult = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.AutoRejected, appResult);
        }

        [Test]
        public void Application_WithNoTechStack_TransferredToAutoRejected()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(s => s.CountryDataProvider.CountryData.Country).Returns("TURKIYE");
            mockValidator.Setup(s => s.IsValid(It.IsAny<string>())).Returns(true);


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 },
                TechStackList = new System.Collections.Generic.List<string>() { "" }
            };


            // Action 
            var appResult = evaluator.Evaluate(form);


            // Assert
            Assert.AreEqual(ApplicationResult.AutoRejected, appResult);
        }

        [Test]
        public void Application_WithTechStackOver75P_TransferredToAutoAccepted()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(s => s.CountryDataProvider.CountryData.Country).Returns("TURKIYE");
            mockValidator.Setup(s => s.IsValid(It.IsAny<string>())).Returns(true);


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 },
                TechStackList = new System.Collections.Generic.List<string>() { "C#", "RabbitMQ", "Microservice", "Visual Studio" },
                YearsOfExperience = 16
            };


            // Action 
            var appResult = evaluator.Evaluate(form);


            // Assert
            Assert.AreEqual(ApplicationResult.AutoAccepted, appResult);
        }


        [Test]
        public void Application_WithInValidIdentityNumber_TransferredToHR()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(s => s.CountryDataProvider.CountryData.Country).Returns("TURKIYE");
            mockValidator.Setup(s => s.IsValid(It.IsAny<string>())).Returns(false);


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 },
            };


            // Action 
            var appResult = evaluator.Evaluate(form);


            // Assert
            Assert.AreEqual(ApplicationResult.TransferredToHR, appResult);
        }


        [Test]
        public void Application_WithOfficeLocation_TransferredToCTO()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(s => s.CountryDataProvider.CountryData.Country).Returns("SPAIN");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 },
            };

            // Action 
            var appResult = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.TransferredToCTO, appResult);
        }
    }
}