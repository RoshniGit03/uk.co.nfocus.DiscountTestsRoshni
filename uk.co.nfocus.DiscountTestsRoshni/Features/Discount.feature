Feature: Discount Validation
  As a customer,
  I want to apply a discount and verify the total order cost
  So that I can ensure discounts are applied correctly and totals are accurate.

  Scenario Outline: Apply 15% discount coupon and verify totals
    Given I am logged into the store as "<username>" and "<password>"
    And the cart is empty
    When I add a "<item>" to the cart
    And I apply a "<coupon>"
    Then the discount should be <discount> of the subtotal
    And the total should be calculated correctly

  Examples:
    | username          | password        | item | coupon    | discount |
    | email@address.com | strong!password | Polo | edgewords | 15%      |





