# Account ERD
```mermaid
erDiagram

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

    ACCOUNT ||--o{ ACCOUNTCONTACT : "AccountId"
    ACCOUNT ||--o{ ACCOUNTADDRESS : "AccountId"
    ACCOUNT ||--o{ ACCOUNTGROUPACCOUNT : "AccountId"

    ACCOUNTGROUPACCOUNT ||--|| ACCOUNT : "AccountId"
    ACCOUNTGROUPACCOUNT ||--|| ACCOUNTGROUP : "AccountGroupId"

    

