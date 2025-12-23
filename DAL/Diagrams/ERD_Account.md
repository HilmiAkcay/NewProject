# Account ERD
```mermaid
erDiagram

    CUSTOMER {
        int Id PK
        string Code
        string Name
        string Type          "ANONYMOUS | RETAIL | B2B | MARKETPLACE"
        int SalesChannelId FK "Nullable"
        string TaxNumber     "Nullable"
        string Email         "Nullable"
        string Phone         "Nullable"
        int PriceGroupId FK  "Nullable" 
    }

    CUSTOMERPRICEGROUP {
        int Id PK
        string Code
        string Description
        int CalculationPolicyId FK
    }

    SALESCHANNEL {
        int Id PK
        string Code          "POS | WEB | WOO | TOPSLIJTER"
        string Name
        int DefaultCalculationPolicyId FK
        int DefaultCurrencyId FK
    }

    TAG {
        int Id PK
        string Code
        string Name
    }

    CUSTOMERTAG {
        int CustomerId FK
        int TagId FK
    }

    VENDORTAG {
        int VendorId FK
        int TagId FK
    }

    CUSTOMERADDRESS {
        int CustomerId FK
        int AddressId FK
    }

    VENDORADDRESS {
        int VendorId FK
        int AddressId FK
    }

    ADDRESS{
        int Id PK
        string Name
        int CountryId
        string Type           "INVOICE | DELIVERY"
        string City
        string Street
        string PostalCode
        string HouseNumber
        string Email         "Nullable"
        string Phone         "Nullable"
    }
                

   
    %% =========================
    %% VENDOR (SUPPLIER)
    %% =========================

    VENDOR {
        int Id PK
        string Code
        string Name
        string TaxNumber
        int CurrencyId FK
        int DefaultTaxRateId FK
        int DefaultCalculationPolicyId FK
    }

    VENDORCONTACT {
        int Id PK
        int VendorId FK
        string Name
        string Email
        string Phone
    }

    VENDOR ||--o{ VENDORADDRESS : VendorId
    VENDOR ||--o{ VENDORCONTACT : VendorId
    VENDOR ||--o{ VENDORTAG : VendorId
    CUSTOMER ||--o{ CUSTOMERADDRESS : CustomerId
    CUSTOMER ||--o{ CUSTOMERTAG : CustomerId
    CUSTOMERPRICEGROUP ||--o{ CUSTOMER : PriceGroupId
    SALESCHANNEL ||--o{ CUSTOMER : SalesChannelId
    TAG ||--o{ CUSTOMERTAG :TagId
    TAG ||--o{ VENDORTAG :TagId

    ADDRESS ||--o{CUSTOMERADDRESS : AddressId
    ADDRESS ||--o{VENDORADDRESS : AddressId
