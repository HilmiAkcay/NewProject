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
 decimal UnitBasePrice
 decimal UnitDepositAmount
 decimal TaxRate
 int PriceType
}

Order ||--|{ OrderLine : has

%% ===============================
%% ORDER PRICE ADJUSTMENTS (PREVIEW / INTENT)
%% ===============================

OrderPriceAdjustment {
 uuid Id PK
 uuid OrderId FK
 uuid CurrencyId FK
 string Scope "ORDER | LINE | COMBO"
 decimal Value
 string ValueType "PERCENT | AMOUNT"
 string Reason
 boolean IsManual
 int Priority
 datetime CreatedAt
}

Order ||--o{ OrderPriceAdjustment : has

OrderPriceAdjustmentLine {
 uuid Id PK
 uuid OrderPriceAdjustmentId FK
 uuid OrderLineId FK
}

OrderPriceAdjustment ||--|{ OrderPriceAdjustmentLine : defines
OrderLine ||--o{ OrderPriceAdjustmentLine : participates_in

%% ===============================
%% FULFILLMENT (PHYSICAL FLOW)
%% ===============================

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

%% ===============================
%% INVOICE (FINANCIAL TRUTH)
%% ===============================

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

%% ===============================
%% INVOICE DISCOUNTS (FINAL, IMMUTABLE)
%% ===============================

InvoiceDiscount {
 uuid Id PK
 uuid InvoiceId FK
 uuid CurrencyId FK
 string Source "MANUAL | PROMOTION"
 uuid SourceRefId
 string Scope "ORDER | LINE | COMBO"
 decimal DiscountAmount
 string Description
 int Priority
 string AppliedRuleSnapshot "JSON"
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
