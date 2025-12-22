# Product Design Notes

## Differences from PurePOS

### Single Sellable Entity
**Pros**
- Speeds up development (≈30% reduction in coding effort)
- Improves overall application performance
- Simplifies querying and data flow

---

### Single Price Principle (NET *or* GROSS)
**Pros**
- Prevents tax-excluded vs tax-included conversion errors
- Ensures consistent pricing logic across the system

---

### Separation of Sales and Purchase Prices
**Pros**
- Reduces data complexity and storage size
- Enables flexible pricing for both B2B and B2C scenarios
- Protects margin calculations

---

### Multi-Level Rounding Rules
**Pros**
- Flexible rounding at line, receipt, and payment levels
- Easier integration with accounting and finance systems
- Compliant with different fiscal regulations

---

### Purchase Rules per Vendor
**Pros**
- Allows supplier-specific ordering constraints
- Supports minimum, maximum, and multiple order quantities
- Improves purchase order accuracy and validation

---
- [Example of Sales Flow](SalesFlow.md)
- [Example of Purchase Flow](PurchaseFlow.md)


## Core Rules

- Every product must have **one default variant**  
  *Simplifies database queries and improves performance.*

- **SKU (Stock Keeping Unit) must be unique**  
  *Ensures unambiguous product identification across systems.*

 
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
    
