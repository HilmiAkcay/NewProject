
```mermaid
erDiagram

Order {
 uuid Id PK
 uuid CustomerId FK
 string OrderNumber
 int State "OPEN | PARTIAL | COMPLETED | CANCELLED"
 datetime CreatedAt
}

OrderLine {
 uuid Id PK
 uuid OrderId FK
 uuid ProductUnitId FK
 uuid CurrencyId FK
 int LineNumber
 string Barcode
 string Description
 decimal OrderedQuantity
 decimal UnitAmount
 decimal UnitDepositAmount
 decimal TaxRate
 int PriceType
}

Order ||--|{ OrderLine : has


Fulfillment {
 uuid Id PK
 uuid OrderId FK
 string FulfillmentNumber
 int Type "SHIP | RETURN | CANCEL"
 int State "PICK | COLLECT | PACKAGE | CANCELLED"
 datetime CreatedAt
}

FulfillmentLine {
 uuid Id PK
 uuid FulfillmentId FK
 uuid OrderLineId FK
 decimal Quantity
}

Order ||--o{ Fulfillment : produces
Fulfillment ||--|{ FulfillmentLine : contains
OrderLine ||--o{ FulfillmentLine : fulfilled_by


Stock {
 uuid ProductUnitId PK
 decimal PhysicalQuantity
}

StockReservation {
 uuid Id PK
 uuid OrderLineId FK
 uuid ProductUnitId FK
 decimal ReservedQuantity
}

OrderLine ||--o{ StockReservation : reserves
Stock ||--o{ StockReservation : allocated_from


Invoice {
 uuid Id PK
 uuid OrderId FK
 string InvoiceNumber
 datetime InvoiceDate
}

InvoiceLine {
 uuid Id PK
 uuid InvoiceId FK
 uuid FulfillmentLineId FK
 decimal Quantity
 decimal UnitAmount
}

Invoice ||--|{ InvoiceLine : contains
FulfillmentLine ||--o{ InvoiceLine : invoiced_from
