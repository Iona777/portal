Feature: GetElectricityPrices
	I want to check electricity prices for kWh usaage

@MicroJourney
Scenario Outline: Enter Electricity kWh consumption for micro journey  
	Given I have navigated to the Quote Tool Landing page
	And I have selected electricity and kWh usage
	And I have entered "<kWh Usage>" value for a frequency of "<frequency>" for Electricity
	When I click on Electricity Next button
	Then the your details screen for Electricity is displayed
	When I enter postcode "<postcode>" with DNO value of "<DNO>"
	And I select the first address on the Select Address screen
	And I click on View Prices button
	Then the Your Electricity Packages micro prices screen is displayed		
	And the Estimated Annual Cost for Electricity is calculated correctly for "<kWh Usage>" and "<duration>" and "<frequency>" and "<DNO>" using matrix file "<matrix file>"
Examples: 
| kWh Usage | duration | frequency | DNO | postcode | matrix file		 |
| 1000      | 12       | Monthly   | 16  | M15 4RP  | ElectricMatrix.csv |
