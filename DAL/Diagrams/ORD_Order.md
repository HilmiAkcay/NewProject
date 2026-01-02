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
 decimal UnitListAmount
 decimal UnitDepositAmount
 decimal TaxRate
 int PriceType
}

Order ||--|{ OrderLine : has

OrderPriceAdjustment {
 uuid Id PK
 uuid OrderId FK
 string Scope "ORDER | LINE | COMBO"
 uuid OrderLineId FK "nullable"
 decimal Value
 string ValueType "PERCENT | AMOUNT"
 string Reason
 int Priority
 datetime CreatedAt
}

Order ||--o{ OrderPriceAdjustment : has
OrderLine ||--o{ OrderPriceAdjustment : may_target


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
 decimal NetUnitAmount
 decimal TaxRate
}

Invoice ||--|{ InvoiceLine : contains
FulfillmentLine ||--o{ InvoiceLine : invoiced_from


InvoiceDiscount {
 uuid Id PK
 uuid InvoiceId FK
 string Source "MANUAL | PROMOTION"
 uuid SourceRefId
 string Scope "ORDER | LINE | COMBO"
 decimal DiscountAmount
 string Description
 int Priority
 datetime CreatedAt
}

Invoice ||--o{ InvoiceDiscount : applies

InvoiceDiscountLine {
 uuid Id PK
 uuid InvoiceDiscountId FK
 uuid InvoiceLineId FK
 decimal AllocatedAmount
}

InvoiceDiscount ||--|{ InvoiceDiscountLine : allocates
InvoiceLine ||--o{ InvoiceDiscountLine : discounted_by
