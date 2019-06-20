Feature: Login
	I want to login
	
@mytag
Scenario Outline: LoginTwo Email
	Given I am on the Login Page
	When I login as a using email address "<Email>"
	Then I should be logged in successfully
Examples: 
| Email				|
| cs@portal.com     |

@mytag
Scenario Outline: LoginTwo Userkey
	Given I am on the Login Page
	When I login as user "<UserKey>"
	Then I should be logged in successfully
Examples: 
| UserKey	|
| CsaUser	|

@mytag
Scenario Outline: LoginTwo Database Check
	Given I am on the Login Page
	When I login as user "<UserKey>"
	And user is NOT a cogs user
	Then I should be logged in successfully
Examples: 
| UserKey	|
| CsaUser	|	
	