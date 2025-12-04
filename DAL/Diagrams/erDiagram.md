```mermaid
erDiagram
    ACCOUNT {
        string BankName "nullable"
        string Code
        string Currency
        string IBAN "nullable"
        int Id PK
        string Name
        int PaymentTermDays
        bool TaxExempt
        string TaxNumber "nullable"
        string VATNumber "nullable"
    }

    ACCOUNTADDRESS {
        int AccountId
        string AddressLine1
        string AddressLine2 "nullable"
        string AddressType
        string City "nullable"
        string Country "nullable"
        int Id PK
        string PostalCode "nullable"
        string State "nullable"
    }

    ACCOUNTCONTACT {
        int AccountId
        string ContactName "nullable"
        string ContactType "nullable"
        string Email "nullable"
        int Id PK
        string Phone "nullable"
    }

    ACCOUNTGROUP { int Id PK string Name }

    ACCOUNTGROUPACCOUNT { int AccountGroupId int AccountId int Id PK }

    PRICERULE {
        bool ApplyOnGrossPrice
        string Description
        decimal DiscountAmount
        decimal DiscountPercent
        decimal FixedPrice
        int Id PK
        decimal MarginPercent
        string Name
        byte RuleType
        int TaxRateId
        DateTime ValidFrom
        DateTime ValidTo "nullable"
    }

    PRICERULEASSIGNMENT {
        int AccountGroupId "nullable"
        int AccountId "nullable"
        int Id PK
        int PriceRuleId
        int ProductGroupId "nullable"
        int ProductId "nullable"
        int ProductUnitId "nullable"
    }

    PRODUCUNIT { int Factor int Id PK int ProductId int UnitId }

    PRODUCT {
        int DefaultPuId
        int DefaultTaxRateId
        int Id PK
        string Name
        int ProductGroupId
        string SKU
    }

    PRODUCTGROUP { int Id PK string Name }

    PURCHASEPRICE {
        int AccountId
        string Currency
        int Id PK
        bool IsGrossPrice
        decimal Price
        int ProductUnitId
        int TaxRateId
        DateTime ValidFrom
        DateTime ValidTo "nullable"
    }

    SALESPRICE {
        string Currency
        int Id PK
        int IsGrossPrice
        decimal Price
        int ProductUnitId
        int TaxRateId
        DateTime ValidFrom
        DateTime ValidTo "nullable"
    }

    TAXRATE {
        string Code
        int Id PK
        string Name
        int Rate
        DateTime ValidFrom
        DateTime ValidTo "nullable"
    }

    UNIT { string Code int Id PK string Name }

