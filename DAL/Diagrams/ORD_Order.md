
```mermaid
erDiagram

Order {
 uuid Id PK
 uuid CustomerId FK
 string OrderNumber
 int State
}

OrderLine {
 uuid Id PK
 uuid OrderId FK
 uuid ProductUnitId FK
 uuid CurrencyId FK
 int LineNumber
 string Barcode
 string Description
 decimal Quantity
 decimal UnitAmount
 decimal UnitDepositAmount
 decimal DiscountPercentage
 decimal TaxRate
 int PriceType
}

Order ||--|{ OrderLine : "have"
 
