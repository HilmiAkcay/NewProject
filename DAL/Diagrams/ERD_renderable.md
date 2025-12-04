# ERD - DAL (renderable)

This file contains a Mermaid `erDiagram` block intended for direct rendering. Copy the entire fenced block into a Mermaid renderer (VS Code Mermaid Preview, mermaid.live, or GitHub) to view.

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

    PURCHASEPRICE { int Id PK int ProductUnitId int AccountId decimal Price string Currency DateTime ValidFrom DateTime ValidTo "nullable" int TaxRateId bool IsGrossPrice }

    SALESPRICE { int Id PK int ProductUnitId decimal Price string Currency DateTime ValidFrom DateTime ValidTo "nullable" int TaxRateId int IsGrossPrice }

    PRICERULE { int Id PK string Name string Description byte RuleType decimal DiscountPercent decimal DiscountAmount decimal FixedPrice decimal MarginPercent int TaxRateId bool ApplyOnGrossPrice DateTime ValidFrom DateTime ValidTo "nullable" }

    PRICERULEASSIGNMENT { int Id PK int PriceRuleId int ProductId "nullable" int ProductGroupId "nullable" int AccountId "nullable" int AccountGroupId "nullable" int ProductUnitId "nullable" }

    ACCOUNT { int Id PK string Code string Name string TaxNumber string VATNumber string Currency int PaymentTermDays stringstring IBAN stringstring BankName bool TaxExempt }

    ACCOUNTGROUP { int Id PK string Name }

    ACCOUNTGROUPACCOUNT { int Id PK int AccountId int AccountGroupId }

    ACCOUNTCONTACT { int Id PK int AccountId stringstring ContactName stringstring Email stringstring Phone stringstring ContactType }

    ACCOUNTADDRESS { int Id PK int AccountId string AddressType string AddressLine1 stringstring AddressLine2 stringstring City stringstring State stringstring Country stringstring PostalCode }

    PRODUCT ||--o{ PRODUCTGROUP : "ProductGroupId"
    PRODUCT ||--|| PRODUCUNIT : "DefaultPuId"
    PRODUCT ||--o{ TAXRATE : "DefaultTaxRateId"

    PRODUCUNIT }|..|| UNIT : "UnitId"

    PURCHASEPRICE ||--|| PRODUCUNIT : "ProductUnitId"
    PURCHASEPRICE ||--o{ TAXRATE : "TaxRateId"
    PURCHASEPRICE ||--o{ ACCOUNT : "AccountId"

    SALESPRICE ||--|| PRODUCUNIT : "ProductUnitId"
    SALESPRICE ||--o{ TAXRATE : "TaxRateId"

    PRICERULE ||--o{ TAXRATE : "TaxRateId"

    PRICERULEASSIGNMENT ||--|| PRICERULE : "PriceRuleId"
    PRICERULEASSIGNMENT ||--o{ PRODUCT : "ProductId"
    PRICERULEASSIGNMENT ||--o{ PRODUCTGROUP : "ProductGroupId"
    PRICERULEASSIGNMENT ||--o{ ACCOUNT : "AccountId"
    PRICERULEASSIGNMENT ||--o{ ACCOUNTGROUP : "AccountGroupId"
    PRICERULEASSIGNMENT ||--|| PRODUCUNIT : "ProductUnitId"

    ACCOUNTGROUPACCOUNT ||--|| ACCOUNT : "AccountId"
    ACCOUNTGROUPACCOUNT ||--|| ACCOUNTGROUP : "AccountGroupId"
