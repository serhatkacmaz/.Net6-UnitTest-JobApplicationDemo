using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;
        private const int autoAcceptedYearOfExperience = 15;
        private List<string> techStackList = new List<string>() { "C#", "RabbitMQ", "Microservice", "Visual Studio" };
        private readonly IIdentityValidator identityValidator;

        public ApplicationEvaluator(IIdentityValidator identityValidator)
        {
            this.identityValidator = identityValidator;
        }

        public ApplicationResult Evaluate(JobApplication form)
        {
            if (form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            if (identityValidator.CountryDataProvider.CountryData.Country != "TURKIYE")
                return ApplicationResult.TransferredToCTO;

            var validIdentity = identityValidator.IsValid(form.Applicant.IdentityNumber);

            if (!validIdentity)
                return ApplicationResult.TransferredToHR;

            var sr = GetTechStackSimilarityRate(form.TechStackList);

            if (sr < 25)
                return ApplicationResult.AutoRejected;

            if (sr > 75 && form.YearsOfExperience >= autoAcceptedYearOfExperience)
                return ApplicationResult.AutoAccepted;

            return ApplicationResult.AutoAccepted;
        }

        private int GetTechStackSimilarityRate(List<string> techStacks)
        {
            var matchedCount = techStacks
                .Where(s => techStackList.Contains(s, StringComparer.OrdinalIgnoreCase))
                .Count();

            return (int)((double)matchedCount / techStackList.Count() * 100);
        }
    }

    public enum ApplicationResult
    {
        AutoRejected,
        TransferredToHR,
        TransferredToLead,
        TransferredToCTO,
        AutoAccepted
    }
}
