using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using NewQuoteTool.Utilities;
using OpenQA.Selenium;


namespace NewQuoteTool.Pages
{
    public class ElectricityPackagesPage : BasePage
    {
        //Class variables
        private readonly TestDataRepository _theTestDataRepository;
        private double _calculatedMonthlyCost;
        private double _calculatedAnnualCost;
        
        //Constructor
        public ElectricityPackagesPage()
        {
            _theTestDataRepository = new TestDataRepository();
        }

        //WebElement Locators - for new version, these should have proper IDs
        private readonly By _monthlyCost12M = By.XPath("//*[@id='main']/div/div[2]/div[3]/div/div/table/tbody/tr[1]/td[1]/p[1]/strong");                                                      
        private readonly By _annualCost12M = By.XPath("//*[@id='main']/div/div[2]/div[3]/div/div/table/tbody/tr[1]/td[1]/p[2]");
                                                  
        //Methods
        public bool IsPageDisplayed()
        {
           return 
            IsElementVisible(_monthlyCost12M) &&
            IsElementVisible(_annualCost12M);
        }

        public bool MonthlyCostMatchesExpected()
        {        
            var actualMonthlyCost = GetVisibleElement(_monthlyCost12M).Text;
            //Remove the unnecessary text and make formats the same
            actualMonthlyCost.Trim();
            actualMonthlyCost = actualMonthlyCost.Replace("£", "");
            actualMonthlyCost = actualMonthlyCost.Replace("*", "");
            var expectedMonthlyCost = string.Format("{0:0.00}", _calculatedMonthlyCost);

            return actualMonthlyCost.Equals(expectedMonthlyCost);
        }

        public bool AnnualCostMatchesExpected()
        {
            var actualAnnualCost = GetVisibleElement(_annualCost12M).Text;
            //Remove the unnecessary text and make formats the same
            actualAnnualCost.Trim();
            actualAnnualCost = actualAnnualCost.Replace("(Total cost £", "");
            actualAnnualCost = actualAnnualCost.Replace(")", "");
            var expectedAnnualCost = string.Format("{0:0.00}",actualAnnualCost);

            return actualAnnualCost.Equals(expectedAnnualCost);
        }

        public double CalculateMonthlyCost(string requiredUsage, string requiredDuration, string requiredFrequency, string requiredDNO, string matrixFile)
        {
            var electricMatrixRecords = _theTestDataRepository.GetElectricMatrix(matrixFile);
            var selectedRecord = electricMatrixRecords.FirstOrDefault(record => record.DNO == requiredDNO
                                                                           && record.MonthDuration == requiredDuration);
            
            int multiplier;
            float consumption = float.Parse(requiredUsage);

            switch (requiredFrequency.ToLower())
            {
                case "monthly":
                    multiplier = 12;
                    break;
                case "quarterly":
                    multiplier = 4;
                    break;
                case "6 monthly":
                    multiplier = 2;
                    break;
                case "annual":
                    multiplier = 1;
                    break;
                default:
                    throw new System.ArgumentException("Invalid frequency entered", requiredFrequency);
                    
            }

            var standingCharge = float.Parse(selectedRecord.StandingChargePence);
            var unitPrice = float.Parse(selectedRecord.UnitRatePence);
            var annualStandingChargeInPounds = standingCharge / 100 * 365;
            var unitRateInPoundsTimesConsumption = unitPrice / 100 * consumption * multiplier;
            var monthlyCost = (annualStandingChargeInPounds + unitRateInPoundsTimesConsumption) / 12;

            //Set class variable for use by later methods
            _calculatedMonthlyCost = Math.Round(monthlyCost, 2);
            
            return Math.Round(monthlyCost,2);
        }

        public double CalculateAnnualCost(string usage, string duration, string frequency, string DNO, string matrixFile)
        {
            var annualCost = (Math.Round(CalculateMonthlyCost(usage, duration, frequency, DNO,  matrixFile), 2)) * 12;

            //Set class variable for use by later methods
            _calculatedAnnualCost = Math.Round(annualCost, 2);

            return _calculatedAnnualCost;
        }

    }
}
