# ERD - NewDAL (renderable)

This file contains a Mermaid `erDiagram` block intended for direct rendering (no `?` or parentheses). Copy the entire fenced block into a Mermaid renderer (VS Code Mermaid Preview, mermaid.live, or GitHub) to view.

```mermaid
erDiagram
    PRODUCT {
        int Id PK
        string Name
        int ProductGroupId
        int DefaultPuId
        int DefaultTaxRateId
        string SKU
    }

    PRODUCUNIT { int Id PK int UnitId int Factor }

    UNIT { int Id PK string Code string Name }

    PRODUCTGROUP { int Id PK string Name }

    TAXRATE { int Id PK string Code string Name int Rate DateTime ValidFrom DateTime ValidTo "nullable" }

    PURCHASEPRICE { int Id PK int ProductUnitId int VendorId decimal Price string Currency DateTime ValidFrom DateTime ValidTo "nullable" int TaxRateId bool IsGrossPrice }

    SALESPRICE { int Id PK int ProductUnitId decimal Price string Currency DateTime ValidFrom DateTime ValidTo "nullable" int TaxRateId int IsGrossPrice }

    PRICERULE { int Id PK string Name string Description byte RuleType decimal DiscountPercent decimal DiscountAmount decimal FixedPrice decimal MarginPercent int TaxRateId bool ApplyOnGrossPrice DateTime ValidFrom DateTime ValidTo "nullable" }

    PRICERULEASSIGNMENT { int Id PK int PriceRuleId int ProductId "nullable" int ProductGroupId "nullable" int CustomerId "nullable" int CustomerGroupId "nullable" int ProductUnitId "nullable" }

    CUSTOMER { int Id PK string Name int TaxExempt }

    CUSTOMERGROUP { int Id PK string Name }

    CUSTOMERGROUPCUSTOMER { int Id PK int CustomerId int CustomerGroupId }

    PRODUCT ||--o{ PRODUCTGROUP : "ProductGroupId"
    PRODUCT ||--|| PRODUCUNIT : "DefaultPuId"
    PRODUCT ||--o{ TAXRATE : "DefaultTaxRateId"

    PRODUCUNIT }|..|| UNIT : "UnitId"

    PURCHASEPRICE ||--|| PRODUCUNIT : "ProductUnitId"
    PURCHASEPRICE ||--o{ TAXRATE : "TaxRateId"

    SALESPRICE ||--|| PRODUCUNIT : "ProductUnitId"
    SALESPRICE ||--o{ TAXRATE : "TaxRateId"

    PRICERULE ||--o{ TAXRATE : "TaxRateId"

    PRICERULEASSIGNMENT ||--|| PRICERULE : "PriceRuleId"
    PRICERULEASSIGNMENT ||--o{ PRODUCT : "ProductId"
    PRICERULEASSIGNMENT ||--o{ PRODUCTGROUP : "ProductGroupId"
    PRICERULEASSIGNMENT ||--o{ CUSTOMER : "CustomerId"
    PRICERULEASSIGNMENT ||--o{ CUSTOMERGROUP : "CustomerGroupId"
    PRICERULEASSIGNMENT ||--|| PRODUCUNIT : "ProductUnitId"

    CUSTOMERGROUPCUSTOMER ||--|| CUSTOMER : "CustomerId"
    CUSTOMERGROUPCUSTOMER ||--|| CUSTOMERGROUP : "CustomerGroupId"

    PURCHASEPRICE ||--o{ VENDOR : "VendorId"
```
