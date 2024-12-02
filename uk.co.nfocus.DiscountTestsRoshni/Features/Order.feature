Feature: Order Placement Validation
  As a customer,
  I want to place an order and verify it appears in my order history
  So that I can track my past purchases.

  Scenario Outline: Verify order number after placing an order
    Given I am logged into the store as "<username>" and "<password>"
    And the cart is empty
    When I add a "<item>" to the cart
    And I checkout with the details of "<firstName>" "<lastName>" "<address>" "<city>" "<postcode>" "<phoneNumber>"
    And I select "<paymentMethod>" then place order
    Then the order number should appear in my order history

  Examples:
    | username          | password        | item  | firstName | lastName | address     | city    | postcode | phoneNumber | paymentMethod |
    | email@address.com | strong!password | Polo  | John      | Doe      | 123 Street  | London  | SW1A 1AA | 07123456789  | Check payments |
