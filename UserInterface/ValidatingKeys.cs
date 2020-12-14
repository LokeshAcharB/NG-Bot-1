using System;

namespace UserInterface
{
    internal class ValidatingKeys
    {
        internal static Tuple<bool, int> IsValidKey(string key)
        {
            bool Valid = int.TryParse(key, out int ParsedKey);
            return Tuple.Create(Valid, ParsedKey);
        }
        internal static string AssigningJob()
        {
            string JobNumber = string.Empty;
            string JobTitle = string.Empty;
        AgainAssignTheJob:
            JobNumber = Console.ReadLine().Trim();
            var result = IsValidKey(JobNumber);
            if (result.Item1 && result.Item2 > 0 && result.Item2 < 5)
            {
                switch (result.Item2)
                {
                    case 1:
                        JobTitle = Convert.ToString(Jobs.Title.SeniorTechnicalLead);
                        break;
                    case 2:
                        JobTitle = Convert.ToString(Jobs.Title.BusinessAnalyst);
                        break;
                    case 3:
                        JobTitle = Convert.ToString(Jobs.Title.SoftwareEngineer);
                        break;
                    case 4:
                        JobTitle = Convert.ToString(Jobs.Title.QATester);
                        break;
                }
                return JobTitle;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("WARNING : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Provide right job number this time : ");
            goto AgainAssignTheJob;
        }
    }
}
