# ERD - DAL (renderable)

This file contains a Mermaid `erDiagram` block intended for direct rendering. Copy the entire fenced block into a Mermaid renderer (VS Code Mermaid Preview, mermaid.live, or GitHub) to view.

```mermaid
erDiagram
    

    

    

    PRICERULE {
        int Id PK
        string Name
        string Description
        byte RuleType
        decimal DiscountPercent
        decimal DiscountAmount
        decimal FixedPrice
        decimal MarginPercent
        int TaxRateId
        bool ApplyOnGrossPrice
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
    }

    PRICERULEASSIGNMENT {
        int Id PK
        int PriceRuleId
        int ProductUnitId "Nullable"
        int ProductId "Nullable"
        int AccountId "Nullable"
        int AccountGroupId "Nullable"
        decimal FixedPrice "Nullable"
        decimal DiscountPercent "Nullable"
    }

   

    

    CURRENCY { int Id PK string Code string Name }

    COUNTRY { int Id PK string Code string Name }

    
    PURCHASEPRICE ||--o{ ACCOUNT : "AccountId"
    PURCHASEPRICE ||--o{ CURRENCY : "CurrencyId"

    

    PRICERULE ||--o{ TAXRATE : "TaxRateId"
    PRICERULE ||--o{ PRICERULEASSIGNMENT : "Id"

    PRICERULEASSIGNMENT ||--|| PRICERULE : "PriceRuleId"
    PRICERULEASSIGNMENT ||--o{ PRODUCTUNIT : "ProductUnitId"
    PRICERULEASSIGNMENT ||--o{ PRODUCT : "ProductId"
    PRICERULEASSIGNMENT ||--o{ ACCOUNT : "AccountId"
    PRICERULEASSIGNMENT ||--o{ ACCOUNTGROUP : "AccountGroupId"

    

    CURRENCY ||--o{ PURCHASEPRICE : "CurrencyId"
    CURRENCY ||--o{ SALESPRICE : "CurrencyId"
