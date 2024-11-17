Feature: Order Placement Validation
  As a customer,
  I want to place an order and verify it appears in my order history
  So that I can track my past purchases.

  Scenario: Verify order number after placing an order
    Given I am logged into the store as "<username>" and "<password>"
    When I add a "<item>" to the cart
    And I checkout with valid billing details
    And I select "Check payments" then place order
    Then the order number should appear in my order history

  Examples:
    | username          | password        | item |
    | email@address.com | strong!password | Polo |

