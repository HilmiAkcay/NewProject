
```mermaid
erDiagram

Order {
uuid Id PK
uuid CustomerId FK
string Code
}

OrderLine {
uuid Id PK
uuid OrderId FK
uuid ProductUnitId FK
int LineNumber
string Barcode
string Description
decimal Quantity
decimal UnitAmount
decimal UnitDepositAmount
decimal DiscountPercentage
decimal VatPercenage
}
 
