Feature: CalculatorSteps
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I've entered 70 into the calculator
	And I enter "Feel great" as my mood
	When I press add
	Then the result should be 120 on the screen
