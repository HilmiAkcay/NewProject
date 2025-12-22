# Product

#### Appendix
P: PROSS\
C: CONS

### Differences from the PurePOS
##### Single Sellable entity.
P: Speed up Coding (almost 30% decreases development time)\
P: Increases the performance of the application\ 
P: 
##### Single Price principle, keep NET or GROSS price.
P: Solves Excl - Incl conversion mistakes.
##### Seperation of Sales and Purchase Price.
P: Smaller data size.\
P: Flexible to B2B and B2C.
##### Different level Rounding Rule.
P: Flexible\
P: Easy for Finance Integration
##### Purchase Rules.
: Flexiable to define rules for each vendor.
  
### Rules
- Each Product must have default variant (simpliefies query database, increase the peorformence)
- SKU (Stock Keeping Unit) will be unique.
 
## ERD Diagram
```mermaid
erDiagram
    PRODUCT {
        int Id PK
        string Name
        int BaseUnitId FK
    }

    PRODUCTVARIANT {
        int Id PK
        int ProductId FK
        string Name
        string SKU
        bool IsDefault
    }

    PRODUCTUNIT {
        int Id PK
        string Name
        int ProductVariantId FK
        int UnitId FK
        decimal Multiplier
        bool IsDefault
        int UsageType "SALES | PURCHASE | BOTH | INVENTORY"
    }

    UNIT { 
        int Id PK 
        string Code 
        string Name 
    }

    TAXRATE { 
        int Id PK 
        string Code 
        string Name 
        decimal Rate 
   }

    PURCHASEPRICE {
        int Id PK
        int ProductUnitId FK
        int AccountId FK
        decimal NetPrice
        int CurrencyId FK
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
        int TaxRateId FK "Nullable"
    }

    SALESPRICE {
        int Id PK
        int ProductUnitId FK
        decimal Amount
        int CurrencyId FK
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
        int TaxRateId FK
        int CalculationPolicyId FK
    }

    PRODUCTBARCODE{
        int Id PK
        int ProductUnitId FK
        string Barcode
        decimal Qty
        bool IsDefault
    }

    ROUNDINGRULE {
        int Id PK
        string Code            "HALF_UP, CASH_0_05, NONE"
        decimal Precision       "2, 0.05, 0.01"
    }

    CALCULATIONPOLICY {
        int Id PK
        string Code
        int PriceType          "GROSS | NET"
        int TaxCalcLevel       "LINE | RECEIPT"
        int LineRoundingRuleId FK "Round Excl to incl"
        int ReceiptRoundingRuleId FK "Round Receipt total" 
        int CashRoundingRuleId FK "round when paid with cash"
    }

    PURCHASEUNITRULE {
        int Id PK
        int ProductUnitId FK
        int AccountId FK "Supplier"
        decimal MinOrderQty
        decimal MaxOrderQty
        decimal OrderMultiple
        bool AllowPartial
    }


    %% Relationships
    
    
    PRODUCT ||--|{ PRODUCTVARIANT : "ProductId"
    PRODUCTVARIANT ||--|{ PRODUCTUNIT : "ProductVariantId"

    UNIT ||--|{ PRODUCTUNIT : "UnitId"
    UNIT ||--|{ PRODUCT : "BaseUnitId"

    TAXRATE ||--|{ SALESPRICE : "TaxRateId"
    TAXRATE ||--|{ PURCHASEPRICE : "TaxRateId"


    PRODUCTUNIT ||--|{ PRODUCTBARCODE : "ProductUnitId"
    PRODUCTUNIT ||--|{ PURCHASEPRICE : "ProductUnitId"
    PRODUCTUNIT ||--|{ SALESPRICE : "ProductUnitId"

    SALESPRICE ||--|{ CALCULATIONPOLICY: "CalculationPolicyId"
    ROUNDINGRULE ||--o{ CALCULATIONPOLICY : "LineRoundingRuleId"
    ROUNDINGRULE ||--o{ CALCULATIONPOLICY : "ReceiptRoundingRuleId"
    ROUNDINGRULE ||--o{ CALCULATIONPOLICY : "CashRoundingRuleId"

    PRODUCTUNIT ||--|{PURCHASEUNITRULE : "ProductUnitId"
    
