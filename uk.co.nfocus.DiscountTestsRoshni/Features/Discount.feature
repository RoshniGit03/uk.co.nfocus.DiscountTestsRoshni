Feature: Discount Validation
  As a customer,
  I want to apply a discount and verify the total order cost
  So that I can ensure discounts are applied correctly and totals are accurate.

  Scenario: Apply 15% discount coupon and verify totals
    Given I am logged into the store as "<username>" and "<password>"
    When I add a "<item>" to the cart
    And I apply the coupon "edgewords"
    Then the discount should be 15% of the subtotal
    And the total should be calculated correctly

  Examples:
    | username          | password        | item |
    | email@address.com | strong!password | Polo |





