# Product ERD
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
        bool IsDefault
    }

    ROUNDINGRULE {
        int Id PK
        string Code            "HALF_UP, CASH_0_05, NONE"
        decimal Precision       "2, 0.05, 0.01"
        string AppliesTo       "LINE, RECEIPT, PAYMENT"
    }

    CALCULATIONPOLICY {
        int Id PK
        string Code
        int PriceType          "GROSS | NET"
        int TaxCalcLevel       "LINE | RECEIPT"
        int LineRoundingRuleId FK
        int ReceiptRoundingRuleId FK
        int CashRoundingRuleId FK
    }

    PURCHASEUNITRULE {
        int Id PK
        int ProductUnitId FK
        int AccountId FK "Supplier"
        decimal MinOrderQty
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
    
