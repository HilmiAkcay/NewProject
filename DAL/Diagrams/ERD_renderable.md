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

    PRODUCTUNIT {
        int Id PK
        int ProductId
        int UnitId
        int Factor
    }

    UNIT { int Id PK string Code string Name }

    PRODUCTGROUP { int Id PK string Name }

    TAXRATE { int Id PK string Code string Name int Rate DateTime ValidFrom DateTime ValidTo "Nullable"}

    PURCHASEPRICE {
        int Id PK
        int ProductUnitId
        int AccountId
        decimal Price
        int CurrencyId
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
        int TaxRateId
        bool IsGrossPrice
    }

    SALESPRICE {
        int Id PK
        int ProductUnitId
        decimal Price
        int CurrencyId
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
        int TaxRateId
        bool IsGrossPrice
    }

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

    ACCOUNT {
        int Id PK
        string Code
        string Name
        string TaxNumber "Nullable"
        string VATNumber "Nullable" 
        int CurrencyId
        int PaymentTermDays
        string IBAN "Nullable"
        string BankName "Nullable"
        bool TaxExempt
    }

    ACCOUNTGROUP { int Id PK string Name }

    ACCOUNTGROUPACCOUNT { int Id PK int AccountId int AccountGroupId }

    ACCOUNTCONTACT {
        int Id PK
        int AccountId
        string ContactName
        string Email
        string Phone
        string ContactType
    }

    ACCOUNTADDRESS {
        int Id PK
        int AccountId
        string AddressType
        string AddressLine1
        string AddressLine2
        string City
        string State
        int CountryId
        string PostalCode
    }

    CURRENCY { int Id PK string Code string Name }

    COUNTRY { int Id PK string Code string Name }

    PRODUCT ||--o{ PRODUCTGROUP : "ProductGroupId"
    PRODUCT ||--|| PRODUCTUNIT : "DefaultPuId"
    PRODUCT ||--o{ TAXRATE : "DefaultTaxRateId"

    PRODUCTUNIT }|..|| UNIT : "UnitId"
    PRODUCTUNIT ||--o{ PRODUCT : "ProductId"

    PURCHASEPRICE ||--o{ TAXRATE : "TaxRateId"
    PURCHASEPRICE ||--o{ ACCOUNT : "AccountId"
    PURCHASEPRICE ||--o{ CURRENCY : "CurrencyId"

    PRODUCTUNIT ||--o{ PURCHASEPRICE : "ProductUnitId"
    PRODUCTUNIT ||--o{ SALESPRICE : "ProductUnitId"
    
    SALESPRICE ||--o{ TAXRATE : "TaxRateId"
    SALESPRICE ||--o{ CURRENCY : "CurrencyId"

    PRICERULE ||--o{ TAXRATE : "TaxRateId"
    PRICERULE ||--o{ PRICERULEASSIGNMENT : "Id"

    PRICERULEASSIGNMENT ||--|| PRICERULE : "PriceRuleId"
    PRICERULEASSIGNMENT ||--o{ PRODUCTUNIT : "ProductUnitId"
    PRICERULEASSIGNMENT ||--o{ PRODUCT : "ProductId"
    PRICERULEASSIGNMENT ||--o{ ACCOUNT : "AccountId"
    PRICERULEASSIGNMENT ||--o{ ACCOUNTGROUP : "AccountGroupId"

    ACCOUNT ||--o{ PURCHASEPRICE : "AccountId"
    ACCOUNT ||--o{ ACCOUNTCONTACT : "AccountId"
    ACCOUNT ||--o{ ACCOUNTADDRESS : "AccountId"
    ACCOUNT ||--o{ ACCOUNTGROUPACCOUNT : "AccountId"
    ACCOUNT ||--o{ PRICERULEASSIGNMENT : "AccountId"

    ACCOUNTGROUPACCOUNT ||--|| ACCOUNT : "AccountId"
    ACCOUNTGROUPACCOUNT ||--|| ACCOUNTGROUP : "AccountGroupId"

    ACCOUNTADDRESS ||--o{ COUNTRY : "CountryId"

    CURRENCY ||--o{ PURCHASEPRICE : "CurrencyId"
    CURRENCY ||--o{ SALESPRICE : "CurrencyId"
