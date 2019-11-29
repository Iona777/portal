Feature: GetGasPrices
	I want to check gas prices for kWh usaage

@MicroJourney
Scenario Outline: Enter Gas kWh consumption for micro journey  
	Given I have navigated to the Quote Tool Landing page
	And I have selected gas and kWh usage
	And I have entered "<kWh Usage>" value for a frequency of "<frequency>" for Gas
	When I click on Gas Next button
	Then the your details screen for Gas is displayed
	When I enter postcode "<postcode>" with LDZ value of "<LDZ>"
	And I select the first address on the Select Address screen
	And I click on View Prices button
	Then the Your Gas Packages micro prices screen is displayed	
	And the Estimated Annual Cost for Gas is calculated correctly for "<kWh Usage>" and "<duration>" and "<band>" and "<frequency>" and "<LDZ>" using matrix file "<matrix file>"
Examples: 
| kWh Usage | duration | band | frequency | LDZ | postcode | matrix file	|
| 1000      | 12       | 1    | Monthly   | NW  | M15 4RP  | GasMatrix.csv	|


#You can add extra micro journey scenarios here just by adding more examples.
#Won't deal with rounding up to 2000kWh, you will need separate cacluation methods for that

#For non-micor journey, you will need different navigation steps, but the calculation
#part should work the same.
